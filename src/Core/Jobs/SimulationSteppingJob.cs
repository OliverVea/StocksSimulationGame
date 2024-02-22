using Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Jobs;

public class SimulationSteppingJob(IServiceScopeFactory serviceScopeFactory) : BaseJob(serviceScopeFactory)
{
    protected override TimeSpan Interval { get; } = TimeSpan.FromSeconds(5);

    protected override async Task DoWorkAsync(IServiceScope scope, CancellationToken cancellationToken)
    {
        var simulationSteppingService = scope.ServiceProvider.GetRequiredService<ISimulationSteppingService>();
        await simulationSteppingService.StepSimulationAsync(cancellationToken);
    }
}