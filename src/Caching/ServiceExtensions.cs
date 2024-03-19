using Core.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Caching;

public static class ServiceExtensions
{
    public static void AddCaching(this IServiceCollection services)
    {
        services.AddSingleton<ISimulationStepCachingRepository, SimulationStepCachingRepository>();
        services.AddSingleton<SimulationStepCache>();
    }
}