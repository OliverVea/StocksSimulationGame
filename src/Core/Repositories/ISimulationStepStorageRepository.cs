using Core.Models;

namespace Core.Repositories;

public interface ISimulationStepStorageRepository
{
    Task SetCurrentSimulationStepAsync(SimulationStep simulationStep, CancellationToken cancellationToken);
    Task<SimulationStep?> GetCurrentSimulationStepAsync(CancellationToken cancellationToken);
}