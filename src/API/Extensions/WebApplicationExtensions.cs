using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions;


/// <summary>
/// Provides extension methods for <see cref="IEndpointRouteBuilder" />.
/// </summary>
public static class WebApplicationExtensions
{
    /// <summary>
    /// Configures the given <see cref="WebApplicationBuilder" /> for the API.
    /// </summary>
    public static WebApplicationBuilder AddApi(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        
        builder.AddSwagger();
        builder.AddEndpoints();
        builder.AddAuth();
        builder.AddCors();

        builder.Services.AddTransient<UserIdInitializationService>();

        return builder;
    }
    
    /// <summary>
    /// Adds the stock endpoints to the given <see cref="IEndpointRouteBuilder" />.
    /// </summary>
    public static WebApplication AddApi(this WebApplication app)
    {
        app.AddSwagger();
        app.AddAuth();
        app.AddCors();
        app.AddEndpoints();
        
        return app;
    }
}