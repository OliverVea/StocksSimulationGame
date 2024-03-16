using Core.Models;
using Core.Models.Ids;
using Core.Models.Prices;
using Core.Models.Stocks;
using Microsoft.Extensions.Logging;

namespace Core.Services;

public sealed class StockPriceSteppingService(ILogger<StockPriceSteppingService> logger,
    IStockService stockService,
    IStockPriceService stockPriceService,
    IRandomService randomService) : IStockPriceSteppingService
{
    public async Task OnSimulationSteppedAsync(SimulationStep simulationStep, CancellationToken cancellationToken)
    {
        var stocksByStockId = await GetStocksByStockIdAsync(cancellationToken);
        var stockPricesByStockId = await GetStockPricesByStockIdAsync(simulationStep, stocksByStockId, cancellationToken);

        var stocksToAdd = stocksByStockId.Keys.Except(stockPricesByStockId.Keys).ToArray();
        var stocksToUpdate = stocksByStockId.Keys.Intersect(stockPricesByStockId.Keys).ToArray();
        
        var newStockPrices = GetNewStockPrices(stocksToAdd, simulationStep, stocksByStockId).ToArray();
        
        var updatedStockPrices = GetUpdatedStockPrices(stocksToUpdate, simulationStep, stocksByStockId, stockPricesByStockId).ToArray();
        
        var combinedStockPrices = newStockPrices.Concat(updatedStockPrices).ToArray();
        
        await stockPriceService.SetStockPricesAsync(new SetStockPricesRequest
        {
            StockPrices = combinedStockPrices,
        }, cancellationToken);
        
        logger.SteppedStockPrices(newStockPrices.Length, updatedStockPrices.Length);
    }

    private async Task<Dictionary<StockId, ListStockResponse>> GetStocksByStockIdAsync(CancellationToken cancellationToken)
    {
        var listRequest = new ListStocksRequest();
        var stocksToStep = await stockService.ListStocksAsync(listRequest, cancellationToken);
        var stocksByStockId = stocksToStep.Stocks.ToDictionary(s => s.StockId);
        return stocksByStockId;
    }

    private async Task<Dictionary<StockId, GetStockPriceResponse>> GetStockPricesByStockIdAsync(
        SimulationStep simulationStep,
        Dictionary<StockId, ListStockResponse> stocksByStockId,
        CancellationToken cancellationToken)
    {
        var getPricesRequest = new GetStockPricesForStepRequest
        {
            StockIds = stocksByStockId.Keys,
            SimulationStep = simulationStep - 1,
        };

        var stockPrices = await stockPriceService.GetStockPricesForStepAsync(getPricesRequest, cancellationToken);
        var stockPricesByStockId = stockPrices.StockPrices.ToDictionary(x => x.StockId);
        return stockPricesByStockId;
    }

    private IEnumerable<SetStockPriceRequest> GetNewStockPrices(
        IEnumerable<StockId> stocksToAdd,
        SimulationStep simulationStep,
        Dictionary<StockId, ListStockResponse> stocksByStockId)
    {
        return stocksToAdd.Select(stockId =>
        {
            var stock = stocksByStockId[stockId];
            var newPrice = stock.StartingPrice;
            
            return new SetStockPriceRequest
            {
                StockId = stockId,
                SimulationStep = simulationStep,
                Price = newPrice,
            };
        });
    }

    private IEnumerable<SetStockPriceRequest> GetUpdatedStockPrices(
        IEnumerable<StockId> stocksToUpdate,
        SimulationStep simulationStep,
        Dictionary<StockId, ListStockResponse> stocksByStockId,
        Dictionary<StockId, GetStockPriceResponse> stockPricesByStockId)
    {
        return stocksToUpdate.Select(stockId =>
        {
            var stock = stocksByStockId[stockId];
            var stockPrice = stockPricesByStockId[stockId];
            
            var newPrice = StepPrice(stock, stockPrice);
            
            return new SetStockPriceRequest
            {
                StockId = stockId,
                SimulationStep = simulationStep,
                Price = newPrice,
            };
        });
    }

    private Price StepPrice(ListStockResponse stock, GetStockPriceResponse stockPrice)
    {
        var value = stockPrice.Price.Value + randomService.SampleNormal(stock.Drift, stock.Volatility);
        return new Price(value);
    }
}