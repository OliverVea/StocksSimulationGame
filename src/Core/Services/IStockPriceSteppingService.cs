using Core.Messages;

namespace Core.Services;

public interface IStockPriceSteppingService
{
    Task OnSimulationSteppedAsync(SimulationSteppedMessage message, CancellationToken cancellationToken);
}