using Core.Messages;

namespace Messages.Handlers;

public class SimulationSteppedMessageHandler
{
    public Task Handle(SimulationSteppedMessage message, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Simulation stepped: {message.SimulationStep.Step}");
        return Task.CompletedTask;
    }
}