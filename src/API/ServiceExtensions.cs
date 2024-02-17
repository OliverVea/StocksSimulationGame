using System.Reflection;
using Core;
using Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.OpenApi.Models;

namespace API;

internal static class ServiceExtensions
{
    internal static void InstallServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();
        
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });

        var sqliteConnectionString = configuration.GetConnectionString("Sqlite");
        var sqliteConnection = new SqliteConnection(sqliteConnectionString);
        if (sqliteConnectionString is null) throw new InvalidOperationException("Sqlite connection string is not configured");

        services.AddCore();
        services.AddPersistence(sqliteConnection);
    }

}