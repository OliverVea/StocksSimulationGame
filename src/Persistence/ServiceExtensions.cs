using Aspire.ServiceDefaults;
using Core.Repositories;
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
    public static IHostApplicationBuilder AddPersistence(this IHostApplicationBuilder builder)
    {
        var persistenceConfiguration = builder.Configuration.GetSection(PersistenceConfiguration.SectionName).Get<PersistenceConfiguration>();
        if (persistenceConfiguration is null) throw new InvalidOperationException("Persistence configuration is missing");

        builder.AddProvider(persistenceConfiguration);

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

    private static void AddProvider(this IHostApplicationBuilder builder, PersistenceConfiguration configuration)
    {
        switch (configuration.Provider)
        {
            case PersistenceProvider.Sqlite:
                builder.Services.AddSqlite(configuration);
                break;
            case PersistenceProvider.Aspire:
                builder.AddAspire(configuration);
                break;
            default:
                throw new InvalidOperationException("Unsupported persistence provider");
        }
    }
    
    public static void AddSqlite(this IServiceCollection services,
        PersistenceConfiguration persistenceConfiguration)
    {
        if (persistenceConfiguration.Sqlite is null) throw new InvalidOperationException("Sqlite configuration is missing");
        
        services.AddSingleton(persistenceConfiguration.Sqlite.Connection);
        
        services.AddDbContext<ProjectDbContext>(options =>
        {
            options.AddWarnings();
            options.UseSqlite(persistenceConfiguration.Sqlite.Connection, ConfigureSqlite);
        });
        
        services.AddShared(persistenceConfiguration);
    }
    
    private static void AddAspire(this IHostApplicationBuilder builder, PersistenceConfiguration configuration)
    {
        builder.AddSqlServerDbContext<ProjectDbContext>(Constants.StocksDatabase);
        builder.Services.AddShared(configuration);
    }
    
    private static void AddShared(this IServiceCollection builder,
        PersistenceConfiguration persistenceConfiguration)
    {
        builder.AddSingleton(persistenceConfiguration);
        builder.AddScoped<IStockStorageRepository, StockStorageRepository>();
        builder.AddScoped<IStockPriceStorageRepository, StockPriceStorageRepository>();
        builder.AddScoped<ISimulationStepStorageRepository, SimulationStepStorageRepository>();
        builder.AddScoped<IUserStorageRepository, UserStorageRepository>();
        builder.AddScoped<IUserPortfolioStorageRepository, UserPortfolioStorageRepository>();
        builder.AddScoped<IAskStorageRepository, AskStorageRepository>();
        builder.AddScoped<IBidStorageRepository, BidStorageRepository>();
        builder.AddScoped<IDbContext>(provider => provider.GetRequiredService<ProjectDbContext>());
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