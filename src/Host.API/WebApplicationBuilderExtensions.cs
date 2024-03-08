using API.Extensions;
using Core;
using Persistence;
using Messages;

namespace Host;

internal static class WebApplicationBuilderExtensions
{
    
    internal static void ConfigureBuilder(this WebApplicationBuilder app, IConfiguration configuration)
    {
        app.AddApi();
        app.Services.AddMessages(app.Host);
        app.Services.AddCore();
        app.Services.AddPersistence(configuration);
        
        app.Services.AddLogging(c => c.AddConsole());
    }
}