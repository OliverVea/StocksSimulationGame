using Core.Models;
using Core.Repositories;

namespace Core.Services;

public sealed class SimulationStepService(
    ISimulationStepCachingRepository cache,
    ISimulationStepStorageRepository store) : ISimulationStepService
{
    public Task SetCurrentSimulationStepAsync(SimulationStep simulationStep, CancellationToken cancellationToken)
    {
        return Task.WhenAll(
            cache.SetCurrentSimulationStepAsync(simulationStep, cancellationToken),
            store.SetCurrentSimulationStepAsync(simulationStep, cancellationToken));
    }

    public async Task<SimulationStep> GetCurrentSimulationStepAsync(CancellationToken cancellationToken)
    {
        var fromCache = await cache.GetCurrentSimulationStepAsync(cancellationToken);
        if (fromCache.HasValue) return fromCache.Value;

        var fallback = await CacheFallbackAsync(cancellationToken);
        
        await cache.SetCurrentSimulationStepAsync(fallback, cancellationToken);
        return fallback;
    }

    private async Task<SimulationStep> CacheFallbackAsync(CancellationToken cancellationToken)
    {
        var fromStore = await store.GetCurrentSimulationStepAsync(cancellationToken);
        if (fromStore.HasValue) return fromStore.Value;
        
        var fallback = new SimulationStep(0);
        await store.SetCurrentSimulationStepAsync(fallback, cancellationToken);
        return fallback;
    }
}