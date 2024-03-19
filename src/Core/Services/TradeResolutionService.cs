using Core.Models.Prices;

namespace Core.Services;

public sealed class TradeResolutionService(
    ISimulationInformationService simulationInformationService,
    IStockPriceService stockPriceService,
    IAskResolutionService askResolutionService,
    IBidResolutionService bidResolutionService) : ITradeResolutionService
{
    public async Task ResolveTradesAsync(CancellationToken cancellationToken)
    {
        var step = await simulationInformationService.GetCurrentSimulationStepAsync(cancellationToken);
        
        var getStockPricesRequest = new GetStockPricesForStepRequest { SimulationStep = step };
        var stockPrices = await stockPriceService.GetStockPricesForStepAsync(getStockPricesRequest, cancellationToken);
        
        foreach (var stockPrice in stockPrices.StockPrices) 
        {
            await ResolveTradesForStockAsync(stockPrice, cancellationToken);
        } 
    }
    
    private Task ResolveTradesForStockAsync(GetStockPriceResponse stockPrice, CancellationToken cancellationToken)
    {
         var askTask = askResolutionService.ResolveAsksForStockAsync(stockPrice, cancellationToken);
         var bidTask = bidResolutionService.ResolveBidsForStockAsync(stockPrice, cancellationToken);
         
         return Task.WhenAll(askTask, bidTask);
    }

    
}