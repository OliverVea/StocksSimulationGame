using Core.Models;

namespace Core.Services;

public sealed class SimulationInformationService(ISimulationStepService simulationStepService) : ISimulationInformationService
{
    public async Task<SimulationStep> IncrementSimulationStepAsync(CancellationToken cancellationToken)
    {
        var simulationStep = await simulationStepService.GetCurrentSimulationStepAsync(cancellationToken);
        var newSimulationStep = new SimulationStep(simulationStep.Step + 1);
        await simulationStepService.SetCurrentSimulationStepAsync(newSimulationStep, cancellationToken);
        return newSimulationStep;
    }

    public Task<SimulationStep> GetCurrentSimulationStepAsync(CancellationToken cancellationToken)
    {
        return simulationStepService.GetCurrentSimulationStepAsync(cancellationToken);
    }

    public async Task<SimulationInformation> GetSimulationInformationAsync(CancellationToken cancellationToken)
    {
        var currentStep = await GetCurrentSimulationStepAsync(cancellationToken);
        var now = DateTime.UtcNow;
        var startTime = now - TimeSpan.FromSeconds(currentStep.Step * Constants.SimulationStepDuration.TotalSeconds);

        return new SimulationInformation
        {
            CurrentStep = currentStep,
            SimulationStepDuration = Constants.SimulationStepDuration,
            StartTime = startTime
        };
    }
}