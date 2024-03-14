using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Wolverine;

namespace Messages;

public static class ServiceExtensions
{
    public static WebApplicationBuilder AddMessages(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<Core.Messages.IMessageBus, MessageBus>();
        builder.Host.UseWolverine();

        return builder;
    }
}