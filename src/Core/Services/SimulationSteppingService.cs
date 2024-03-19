namespace Core.Services;

public sealed class SimulationSteppingService(
    ISimulationInformationService simulationInformationService,
    IStockPriceSteppingService stockPriceSteppingService,
    ITradeResolutionService tradeResolutionService) : ISimulationSteppingService
{
    public async Task StepSimulationAsync(CancellationToken cancellationToken)
    {
        var currentSimulationStep = await simulationInformationService.GetCurrentSimulationStepAsync(cancellationToken);
        var newSimulationStep = currentSimulationStep + 1;
        
        await stockPriceSteppingService.OnSimulationSteppedAsync(newSimulationStep, cancellationToken);
        await tradeResolutionService.ResolveTradesAsync(cancellationToken);
        
        await simulationInformationService.IncrementSimulationStepAsync(cancellationToken);
        
    }
}