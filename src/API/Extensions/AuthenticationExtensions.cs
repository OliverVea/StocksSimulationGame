using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions;

internal static class AuthenticationExtensions
{
    private const string ScopeClaim = "scope";
    
    public static void AddAuth(this WebApplicationBuilder builder)
    {
        builder.AddJwtAuth();
    }
    
    private static void AddJwtAuth(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddAuthentication(ConfigureAuthentication)
            .AddJwtBearer(options => ConfigureJwtBearer(options, builder));
        
        builder.Services.AddAuthorization(ConfigureAuthorization);
    }

    private static void ConfigureAuthentication(AuthenticationOptions options)
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }

    private static void ConfigureJwtBearer(JwtBearerOptions options, WebApplicationBuilder builder)
    {
        options.Authority = builder.Configuration["Auth0:Domain"];
        options.Audience = builder.Configuration["Auth0:Audience"];
        options.TokenValidationParameters.ValidateAudience = false;
    }

    private static void ConfigureAuthorization(AuthorizationOptions options)
    {
        options.AddPolicy(Policies.Admin, policy => policy.RequireAuthenticatedUser().RequireClaim(ScopeClaim, Claims.Admin));
        options.AddPolicy(Policies.User, policy => policy.RequireAuthenticatedUser());
    }

    public static void AddAuth(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
}