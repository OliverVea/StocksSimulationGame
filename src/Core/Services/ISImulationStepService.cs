using Core.Models;

namespace Core.Services;

public interface ISimulationStepService
{
    Task SetCurrentSimulationStepAsync(SimulationStep simulationStep, CancellationToken cancellationToken);
    Task<SimulationStep> GetCurrentSimulationStepAsync(CancellationToken cancellationToken);
}