using Core.Models;

namespace Core.Services;

public interface IStockPriceSteppingService
{
    Task OnSimulationSteppedAsync(SimulationStep simulationStep, CancellationToken cancellationToken);
}