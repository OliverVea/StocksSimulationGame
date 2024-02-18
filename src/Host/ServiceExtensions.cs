using Core;
using Microsoft.OpenApi.Models;
using Persistence;
using Messages;

namespace Host;

internal static class ServiceExtensions
{
    private const string ApiTitle = "Stock Simulation API";
    private const string Version = "v1";
    private const string XmlFile = "API.xml";
    private static string XmlPath => Path.Combine(AppContext.BaseDirectory, XmlFile);
    
    internal static void ConfigureBuilder(this WebApplicationBuilder app, IConfiguration configuration)
    {
        app.Services.AddEndpointsApiExplorer();
        
        app.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(Version, new OpenApiInfo { Title = ApiTitle, Version = Version });
            c.IncludeXmlComments(XmlPath);
        });

        app.Services.AddMessages(app.Host);
        app.Services.AddCore();
        app.Services.AddPersistence(configuration);
    }

}