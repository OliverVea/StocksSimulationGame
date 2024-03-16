using Core.Models;
using Core.Models.Ids;
using Core.Models.Prices;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Persistence.Repositories;

internal sealed class StockPriceStorageRepository(IDbContext dbContext) : IStockPriceStorageRepository
{
    public async Task<GetStockPriceInIntervalResponse> GetStockPriceInIntervalAsync(GetStockPriceInIntervalRequest request, CancellationToken cancellationToken)
    {
        var baseQuery = dbContext.StockPrices
            .Where(x => x.StockId == request.StockId.Id)
            .Where(x => x.SimulationStep >= request.From.Step && x.SimulationStep <= request.To.Step)
            .OrderBy(x => x.SimulationStep);
        
        var simulationSteps = await baseQuery
            .Select(x => x.SimulationStep)
            .ToListAsync(cancellationToken);

        int sampleRate = Math.Max(1, simulationSteps.Count / request.DataPoints);

        var sampledTimesteps = new List<long>();
        for (int i = 0; i < simulationSteps.Count; i += sampleRate)
        {
            sampledTimesteps.Add(simulationSteps[i]);
        }

        var stockPrices = await baseQuery
            .Where(x => sampledTimesteps.Contains(x.SimulationStep))
            .Select(x => Map(x))
            .ToArrayAsync(cancellationToken);

        return new GetStockPriceInIntervalResponse
        {
            StockId = request.StockId,
            StockPrices = stockPrices,
        };
    }

    public async Task<GetStockPricesForStepResponse> GetStockPricesForStepAsync(GetStockPricesForStepRequest request, CancellationToken cancellationToken)
    {
        var stockGuids = request.StockIds.Select(x => x.Id).ToArray();
        
        var stockPrices = await dbContext.StockPrices
            .Where(x => stockGuids.Contains(x.StockId) && x.SimulationStep == request.SimulationStep.Step)
            .Select(x => Map(x))
            .ToArrayAsync(cancellationToken);

        return new GetStockPricesForStepResponse
        {
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
            Price = request.Price.Value,
            SimulationStep = request.SimulationStep.Step,
            StockId = request.StockId.Id,
        };
    }
    
    private static GetStockPriceResponse Map(StockPrice entity) => new()
    {
        StockId = new StockId(entity.StockId),
        Step = new SimulationStep(entity.SimulationStep),
        Price = new Price(entity.Price),
    };
}