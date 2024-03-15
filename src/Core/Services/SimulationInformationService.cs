using Core.Models;
using Core.Repositories;

namespace Core.Services;

public class SimulationInformationService(ISimulationStepStorageRepository simulationStepStorageRepository) : ISimulationInformationService
{
    public async Task<SimulationStep> IncrementSimulationStepAsync(CancellationToken cancellationToken)
    {
        var simulationStep = await simulationStepStorageRepository.GetCurrentSimulationStepAsync(cancellationToken);
        var newSimulationStep = new SimulationStep(simulationStep.Step + 1);
        await simulationStepStorageRepository.SetCurrentSimulationStepAsync(newSimulationStep, cancellationToken);
        return newSimulationStep;
    }

    public Task<SimulationStep> GetCurrentSimulationStepAsync(CancellationToken cancellationToken)
    {
        return simulationStepStorageRepository.GetCurrentSimulationStepAsync(cancellationToken);
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