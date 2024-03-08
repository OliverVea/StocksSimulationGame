using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace API.Extensions;


internal static class SwaggerExtensions
{
    private const string ApiTitle = "Stock Simulation API";
    private const string Version = "v1";
    private const string Description = "API for the stock simulation application.";
    private const string Oauth2 = "oauth2";
    private static readonly string XmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    private static string XmlPath => Path.Combine(AppContext.BaseDirectory, XmlFile);

    private static readonly string[] Scopes = [Claims.Admin];
    
    internal static void AddSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(options => ConfigureSwaggerGen(options, builder.Configuration));
    }

    private static void ConfigureSwaggerGen(SwaggerGenOptions options, ConfigurationManager configuration)
    {
        options.IncludeXmlComments(XmlPath);
        
        options.SwaggerDoc(Version, new OpenApiInfo
        {
            Title = ApiTitle,
            Version = Version,
            Description = Description,
        });
        
        var domain = configuration["Auth0:Domain"] ?? throw new InvalidOperationException("Auth0:Domain is not set.");
        var audience = configuration["Auth0:Audience"] ?? throw new InvalidOperationException("Auth0:Audience is not set.");
        
        options.AddSecurityDefinition(Oauth2, new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows
            {
                AuthorizationCode = new OpenApiOAuthFlow
                {
                    AuthorizationUrl = new Uri($"{domain}/authorize?audience={audience}"),
                    TokenUrl = new Uri($"{domain}/oauth/token"),
                    Scopes = Scopes.ToDictionary(x => x, StringComparer.Ordinal),
                }
            }
        });
        
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = Oauth2,
                    },
                },
                Scopes
            },
        });
    }

    internal static void AddSwagger(this WebApplication app)
    {
        
        app.UseSwagger();
        app.UseSwaggerUI(settings =>
        {
            settings.SwaggerEndpoint($"/swagger/{Version}/swagger.json", $"{ApiTitle}: {Version}");
            settings.OAuthClientId(app.Configuration["Auth0:ClientId"]);
            settings.OAuthClientSecret(app.Configuration["Auth0:ClientSecret"]);
            settings.OAuthScopes(Scopes);
            settings.OAuthScopeSeparator(",");
            settings.OAuthUsePkce();
        });
    }
}
