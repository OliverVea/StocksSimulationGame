using Core.Models;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Persistence.Repositories;

internal sealed class SimulationStepStorageRepository(IDbContext dbContext) : ISimulationStepStorageRepository
{
    private SimulationStep? _currentSimulationStep;

    public Task SetCurrentSimulationStepAsync(SimulationStep simulationStep, CancellationToken cancellationToken)
    {
        _currentSimulationStep = simulationStep;

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

    public async Task<SimulationStep> GetCurrentSimulationStepAsync(CancellationToken cancellationToken)
    {
        if (_currentSimulationStep != null)
        {
            return _currentSimulationStep.Value;
        }

        var entity = await dbContext.CurrentSimulationSteps.SingleOrDefaultAsync(cancellationToken);

        if (entity is not null)
        {
            _currentSimulationStep = new SimulationStep(entity.SimulationStep);
            return _currentSimulationStep.Value;
        }
        
        _currentSimulationStep = new SimulationStep(0);
        
        await SetCurrentSimulationStepAsync(_currentSimulationStep.Value, cancellationToken);
        
        return _currentSimulationStep.Value;
    }
}