using Core.Models;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

internal class SimulationStepStorageRepository(IDbContext dbContext) : ISimulationStepStorageRepository
{
    public async Task<SimulationStep> GetCurrentSimulationStepAsync(CancellationToken cancellationToken)
    {
        var count = await dbContext.StockPrices.CountAsync(cancellationToken);
        if (count == 0) return new SimulationStep(0);
        
        var step = await dbContext.StockPrices
            .Select(x => x.SimulationStep)
            .MaxAsync(cancellationToken);

        return new SimulationStep(step);
    }
}