using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Wolverine;

namespace Messages;

public static class ServiceExtensions
{
    public static IServiceCollection AddMessages(this IServiceCollection services, IHostBuilder builder)
    {
        services.AddSingleton<Core.Messages.IMessageBus, MessageBus>();
        builder.UseWolverine();

        return services;
    }
}