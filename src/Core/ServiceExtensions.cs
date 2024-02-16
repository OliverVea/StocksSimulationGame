using Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Core;

public static class ServiceExtensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddScoped<ITickerService, TickerService>();

        return services;
    }
}