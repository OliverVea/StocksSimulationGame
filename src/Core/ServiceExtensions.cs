using Core.Jobs;
using Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Core;

public static class ServiceExtensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddScoped<IStockService, StockService>();
        services.AddScoped<IStockPriceService, StockPriceService>();
        services.AddScoped<ISimulationInformationService, SimulationInformationService>();
        services.AddScoped<ISimulationSteppingService, SimulationSteppingService>();
        services.AddScoped<IRandomService, RandomService>();
        services.AddScoped<IStockPriceSteppingService, StockPriceSteppingService>();
        services.AddScoped<IUserIdService, UserIdService>();
        
        services.AddHostedService<SimulationSteppingJob>();

        return services;
    }
}