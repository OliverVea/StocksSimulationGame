using Core.Models;

namespace Core.Services;

public interface ISimulationInformationService
{
    Task<SimulationStep> IncrementSimulationStepAsync(CancellationToken cancellationToken);
    Task<SimulationStep> GetCurrentSimulationStepAsync(CancellationToken cancellationToken);
    Task<SimulationInformation> GetSimulationInformationAsync(CancellationToken cancellationToken);
}