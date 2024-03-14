using Aspire.ServiceDefaults;
using Core.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence.Configuration;
using Persistence.Repositories;

namespace Persistence;

public static class ServiceExtensions
{
    public static WebApplicationBuilder AddPersistence(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        var persistenceConfiguration = configuration.GetSection(PersistenceConfiguration.SectionName).Get<PersistenceConfiguration>();
        if (persistenceConfiguration is null) throw new InvalidOperationException("Persistence configuration is missing");

        builder.AddProvider(persistenceConfiguration);
        builder.Services.AddSingleton(persistenceConfiguration);
        builder.Services.AddScoped<IStockStorageRepository, StockStorageRepository>();
        builder.Services.AddScoped<IStockPriceStorageRepository, StockPriceStorageRepository>();
        builder.Services.AddScoped<ISimulationStepStorageRepository, SimulationStepStorageRepository>();
        builder.Services.AddScoped<IDbContext>(provider => provider.GetRequiredService<ProjectDbContext>());

        return builder;
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

    private static WebApplicationBuilder AddProvider(this WebApplicationBuilder builder, PersistenceConfiguration configuration)
    {
        builder.AddAspire();
        
        /*
        return configuration.Provider switch
        {
            PersistenceProvider.Sqlite => builder.AddSqlite(configuration),
            PersistenceProvider.Aspire => builder.AddAspire(),
            _ => throw new InvalidOperationException("Unsupported persistence provider")
        };
        */

        return builder;
    }
    
    private static WebApplicationBuilder AddSqlite(
        this WebApplicationBuilder builder,
        PersistenceConfiguration persistenceConfiguration)
    {
        if (persistenceConfiguration.Sqlite is null) throw new InvalidOperationException("Sqlite configuration is missing");
        
        builder.Services.AddSingleton(persistenceConfiguration.Sqlite.Connection);
        
        builder.Services.AddDbContext<ProjectDbContext>(options =>
        {
            options.AddWarnings();
            options.UseSqlite(persistenceConfiguration.Sqlite.Connection, ConfigureSqlite);
        });
        
        return builder;
    }

    private static WebApplicationBuilder AddAspire(
        this WebApplicationBuilder builder)
    {
        builder.AddSqlServerDbContext<ProjectDbContext>(Constants.StocksDatabase, c =>
        {
        }, o =>
        {
        });

        return builder;
    }

    private static void ConfigureSqlite(SqliteDbContextOptionsBuilder options)
    {
        options.MigrationsAssembly(typeof(ProjectDbContext).Assembly.FullName);
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