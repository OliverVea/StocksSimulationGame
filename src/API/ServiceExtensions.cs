using Core;
using Persistence;
using Microsoft.Data.Sqlite;

namespace API;

public static class ServiceExtensions
{
    public static void InstallServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        var sqliteConnectionString = configuration.GetConnectionString("Sqlite");
        var sqliteConnection = new SqliteConnection(sqliteConnectionString);
        if (sqliteConnectionString is null) throw new InvalidOperationException("Sqlite connection string is not configured");

        services.AddCore();
        services.AddPersistence(sqliteConnection);
    }

}