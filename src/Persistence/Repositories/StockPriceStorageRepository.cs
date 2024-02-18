using Core.Models;
using Core.Models.Prices;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Persistence.Repositories;

internal class StockPriceStorageRepository(IDbContext dbContext) : IStockPriceStorageRepository
{
    public async Task<GetStockPricesResponse> GetStockPricesAsync(GetStockPricesRequest request, CancellationToken cancellationToken)
    {
        var stockPrices = await dbContext.StockPrices
            .Where(x => x.StockId == request.StockId.Id)
            .OrderByDescending(x => x.SimulationStep)
            .Select(x => Map(x))
            .ToArrayAsync(cancellationToken);

        return new GetStockPricesResponse
        {
            StockId = request.StockId,
            StockPrices = stockPrices,
        };
    }

    public Task SetStockPricesAsync(SetStockPricesRequest request, CancellationToken cancellationToken)
    {
        var updatesByStockId = request.StockPrices.ToLookup(x => x.StockId);

        foreach (var (stockId, setRequests) in updatesByStockId.Select(x => (x.Key, x.AsEnumerable())))
        {
            var simulationSteps = setRequests.Select(x => x.SimulationStep.Step).ToArray();
            var existing = dbContext.StockPrices.Where(x => x.StockId == stockId.Id && simulationSteps.Contains(x.SimulationStep));
            dbContext.StockPrices.RemoveRange(existing);
        }
        
        var stockPrices = request.StockPrices.Select(Map).ToArray();

        dbContext.StockPrices.AddRange(stockPrices);

        return dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task DeleteStockPricesAsync(DeleteStockPricesRequest request, CancellationToken cancellationToken)
    {
        var stockIds = request.StockPrices.Select(x => x.StockId.Id).ToArray();
        
        var stockPrices = dbContext.StockPrices
            .Where(x => stockIds.Contains(x.StockId))
            .ToArray();

        dbContext.StockPrices.RemoveRange(stockPrices);

        return dbContext.SaveChangesAsync(cancellationToken);
    }

    private static StockPrice Map(SetStockPriceRequest request)
    {
        return new StockPrice
        {
            Price = request.Price,
            SimulationStep = request.SimulationStep.Step,
            StockId = request.StockId.Id,
        };
    }
    
    private static GetStockPriceResponse Map(StockPrice entity) => new()
    {
        Step = new SimulationStep
        {
            Step = entity.SimulationStep,
        },
        Price = new Price
        {
            Value = entity.Price,
        },
    };
}