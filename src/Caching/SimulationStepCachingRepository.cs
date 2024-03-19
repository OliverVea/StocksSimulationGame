using Core.Models;
using Core.Repositories;

namespace Caching;

internal sealed class SimulationStepCachingRepository(SimulationStepCache simulationStepCache) : ISimulationStepCachingRepository
{

    public Task<SimulationStep?> GetCurrentSimulationStepAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(simulationStepCache.CurrentSimulationStep);
    }

    public Task SetCurrentSimulationStepAsync(SimulationStep simulationStep, CancellationToken cancellationToken)
    {
        simulationStepCache.CurrentSimulationStep = simulationStep;
        return Task.CompletedTask;
    }
}