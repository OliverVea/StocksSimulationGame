using Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Jobs;

public class SimulationSteppingJob(
    IServiceScopeFactory serviceScopeFactory,
    ILogger<SimulationSteppingJob> logger) : BackgroundService
{
    private const string ClassName = nameof(SimulationSteppingJob);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("{Name} is running.", ClassName);

        while (!stoppingToken.IsCancellationRequested)
        {
            await DoWorkAsync(stoppingToken);
            await Task.Delay(5000, stoppingToken);
        }
    }

    private async Task DoWorkAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("{Name} is working.", ClassName);

        using var scope = serviceScopeFactory.CreateScope();

        var simulationSteppingService = scope.ServiceProvider.GetRequiredService<ISimulationSteppingService>();
        await simulationSteppingService.StepSimulationAsync(stoppingToken);
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("{Name} is stopping.", ClassName);
        await base.StopAsync(stoppingToken);
    }
}