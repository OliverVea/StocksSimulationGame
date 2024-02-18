using Core.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Configuration;
using Persistence.Repositories;

namespace Persistence;

public static class ServiceExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var persistenceConfiguration = configuration.GetSection(PersistenceConfiguration.SectionName).Get<PersistenceConfiguration>();
        if (persistenceConfiguration is null) throw new InvalidOperationException("Persistence configuration is missing");

        services.AddProvider(persistenceConfiguration);
        services.AddSingleton(persistenceConfiguration);
        services.AddScoped<IStockStorageRepository, StockStorageRepository>();
        services.AddScoped<IStockPriceStorageRepository, StockPriceStorageRepository>();
        services.AddScoped<ISimulationStepStorageRepository, SimulationStepStorageRepository>();
        services.AddScoped<IDbContext>(provider => provider.GetRequiredService<ProjectDbContext>());

        return services;
    }
    
    private static void AddWarnings(this DbContextOptionsBuilder options)
    {
        options.ConfigureWarnings(w =>
        {
            w.Throw(RelationalEventId.QueryPossibleUnintendedUseOfEqualsWarning);
            w.Throw(RelationalEventId.BoolWithDefaultWarning);
            w.Throw(CoreEventId.RowLimitingOperationWithoutOrderByWarning);
            w.Throw(CoreEventId.FirstWithoutOrderByAndFilterWarning);
            w.Throw(CoreEventId.PossibleUnintendedCollectionNavigationNullComparisonWarning);
        });
    }

    private static IServiceCollection AddProvider(this IServiceCollection services, PersistenceConfiguration configuration)
    {
        return configuration.Provider switch
        {
            PersistenceProvider.Sqlite => services.AddSqlite(configuration),
            _ => throw new InvalidOperationException("Unsupported persistence provider")
        };
    }
    
    private static IServiceCollection AddSqlite(
        this IServiceCollection services,
        PersistenceConfiguration persistenceConfiguration)
    {
        if (persistenceConfiguration.Sqlite is null) throw new InvalidOperationException("Sqlite configuration is missing");
        
        services.AddSingleton(persistenceConfiguration.Sqlite.Connection);
        
        services.AddDbContext<ProjectDbContext>(options =>
        {
            options.AddWarnings();
            options.UseSqlite(persistenceConfiguration.Sqlite.Connection);
        });
        
        return services;
    }

    public static async Task EnsureCreatedAsync(this IServiceProvider serviceProvider)
    {
        var persistenceConfiguration = serviceProvider.GetRequiredService<PersistenceConfiguration>();

        if (persistenceConfiguration.Provider is PersistenceProvider.Sqlite)
        {
            var sqliteConnection = serviceProvider.GetRequiredService<SqliteConnection>();
            await sqliteConnection.OpenAsync();
        }

        var dbContext = serviceProvider.GetRequiredService<ProjectDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
    }

    public static async Task MigrateAsync(this IServiceProvider serviceProvider)
    {
        await using var scope = serviceProvider.CreateAsyncScope();

        await using var dbContext = scope.ServiceProvider.GetRequiredService<ProjectDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}