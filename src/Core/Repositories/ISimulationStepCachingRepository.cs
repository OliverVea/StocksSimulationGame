using Core.Models;

namespace Core.Repositories;

public interface ISimulationStepCachingRepository
{
    Task<SimulationStep?> GetCurrentSimulationStepAsync(CancellationToken cancellationToken);
    Task SetCurrentSimulationStepAsync(SimulationStep simulationStep, CancellationToken cancellationToken);
}