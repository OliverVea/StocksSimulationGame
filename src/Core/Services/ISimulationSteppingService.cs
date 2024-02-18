namespace Core.Services;

public interface ISimulationSteppingService
{
    Task StepSimulationAsync(CancellationToken cancellationToken);
}