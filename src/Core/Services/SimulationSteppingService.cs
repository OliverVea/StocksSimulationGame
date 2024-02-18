using Core.Messages;

namespace Core.Services;

public class SimulationSteppingService(IMessageBus messageBus, ISimulationInformationService simulationInformationService) : ISimulationSteppingService
{
    public async Task StepSimulationAsync(CancellationToken cancellationToken)
    {
        var newSimulationStep = await simulationInformationService.IncrementSimulationStepAsync(cancellationToken);
        var message = new SimulationSteppedMessage(newSimulationStep);
        await messageBus.PublishAsync(message, cancellationToken);
    }
}