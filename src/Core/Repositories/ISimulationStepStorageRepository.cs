using Core.Models;

namespace Core.Repositories;

public interface ISimulationStepStorageRepository
{
    Task<SimulationStep> GetCurrentSimulationStepAsync(CancellationToken cancellationToken);
}