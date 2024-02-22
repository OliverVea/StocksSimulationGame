using Core.Messages;
using Core.Services;
using Microsoft.Extensions.Logging;

namespace Messages.Handlers;

public class SimulationSteppedMessageHandler(ILogger<SimulationSteppedMessageHandler> logger,
    IStockPriceSteppingService stockPriceSteppingService)
{
    public Task Handle(SimulationSteppedMessage message, CancellationToken cancellationToken)
    {
        logger.SimulationStepped(message.SimulationStep.Step);
        return stockPriceSteppingService.OnSimulationSteppedAsync(message, cancellationToken);
    }
}