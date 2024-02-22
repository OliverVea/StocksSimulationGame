using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Core.Jobs;

public abstract class BaseJob(IServiceScopeFactory ssf) : BackgroundService
{
    protected abstract TimeSpan Interval { get; }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = ssf.CreateScope();
            
            await Task.Delay(Interval, stoppingToken);
            await DoWorkAsync(scope, stoppingToken);
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        await base.StopAsync(stoppingToken);
    }
    
    protected abstract Task DoWorkAsync(IServiceScope scope, CancellationToken stoppingToken);
}