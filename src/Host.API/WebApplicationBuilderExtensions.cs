using API.Extensions;
using Core;
using Persistence;
using Caching;

namespace Host;

internal static class WebApplicationBuilderExtensions
{
    
    internal static void ConfigureBuilder(this WebApplicationBuilder app)
    {
        app.Services.AddLogging(ConfigureLogging);
        
        app.AddServiceDefaults();
        
        app.AddApi();
        app.Services.AddCaching();
        app.Services.AddCore();
        app.AddPersistence();
    }

    private static void ConfigureLogging(ILoggingBuilder loggingBuilder)
    {
        loggingBuilder.AddConsole();
    }
}