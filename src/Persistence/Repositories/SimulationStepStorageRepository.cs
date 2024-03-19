using Core.Models;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Persistence.Repositories;

internal sealed class SimulationStepStorageRepository(IDbContext dbContext) : ISimulationStepStorageRepository
{
    public Task SetCurrentSimulationStepAsync(SimulationStep simulationStep, CancellationToken cancellationToken)
    {
        var entity = dbContext.CurrentSimulationSteps.SingleOrDefault();

        if (entity is not null)
        {
            entity.SimulationStep = simulationStep.Step;
            return dbContext.SaveChangesAsync(cancellationToken);
        }
        
        entity = new CurrentSimulationStep
        {
            SimulationStep = simulationStep.Step,
        };
        
        dbContext.CurrentSimulationSteps.Add(entity);
        return dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<SimulationStep?> GetCurrentSimulationStepAsync(CancellationToken cancellationToken)
    {
        var entity = await dbContext.CurrentSimulationSteps.SingleOrDefaultAsync(cancellationToken);
        if (entity is null) return null;
        
        return new SimulationStep(entity.SimulationStep);
    }
}