using API.Extensions;
using Core;
using Persistence;
using Messages;

namespace Host;

internal static class WebApplicationBuilderExtensions
{
    
    internal static void ConfigureBuilder(this WebApplicationBuilder app, IConfiguration configuration)
    {
        app.AddServiceDefaults();
        
        app.AddApi();
        app.Services.AddCore();
        app.AddMessages();
        app.AddPersistence(configuration);
    }

    private static void ConfigureLogging(ILoggingBuilder loggingBuilder)
    {
        loggingBuilder.AddConsole();
    }
}