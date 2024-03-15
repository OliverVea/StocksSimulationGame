namespace Core.Services;

public class SimulationSteppingService(ISimulationInformationService simulationInformationService, IStockPriceSteppingService stockPriceSteppingService) : ISimulationSteppingService
{
    public async Task StepSimulationAsync(CancellationToken cancellationToken)
    {
        var currentSimulationStep = await simulationInformationService.GetCurrentSimulationStepAsync(cancellationToken);
        var newSimulationStep = currentSimulationStep + 1;
        
        await stockPriceSteppingService.OnSimulationSteppedAsync(newSimulationStep, cancellationToken);
        
        await simulationInformationService.IncrementSimulationStepAsync(cancellationToken);
    }
}