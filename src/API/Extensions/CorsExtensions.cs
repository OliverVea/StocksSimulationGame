using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions;

internal static class CorsExtensions
{
    private const string PolicyName = "CorsPolicy";
    
    public static void AddCors(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options => options.AddPolicy(PolicyName, ConfigurePolicy));
    }
    
    private static void ConfigurePolicy(CorsPolicyBuilder policy)
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
    }

    public static void AddCors(this WebApplication app)
    {
        app.UseCors(PolicyName);
    }
}