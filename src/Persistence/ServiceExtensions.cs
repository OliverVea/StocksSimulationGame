using Core.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;

namespace Persistence;

public static class ServiceExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, SqliteConnection connection)
    {
        services.AddScoped<IStockStorageRepository, StockStorageRepository>();
        services.AddScoped<IStockPriceStorageRepository, StockPriceStorageRepository>();

        services.AddSingleton(connection);

        services.AddDbContext<ProjectDbContext>(options =>
        {
            options.UseSqlite(connection);

            options.ConfigureWarnings(w =>
            {
                w.Throw(RelationalEventId.QueryPossibleUnintendedUseOfEqualsWarning);
                w.Throw(RelationalEventId.BoolWithDefaultWarning);
                w.Throw(CoreEventId.RowLimitingOperationWithoutOrderByWarning);
                w.Throw(CoreEventId.FirstWithoutOrderByAndFilterWarning);
                w.Throw(CoreEventId.PossibleUnintendedCollectionNavigationNullComparisonWarning);
            });
        });

        services.AddScoped<IDbContext>(provider => provider.GetRequiredService<ProjectDbContext>());

        return services;
    }

    public static async Task EnsureCreatedAsync(this IServiceProvider serviceProvider)
    {
        var sqliteConnection = serviceProvider.GetRequiredService<SqliteConnection>();
        await sqliteConnection.OpenAsync();

        var dbContext = serviceProvider.GetRequiredService<ProjectDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
    }

    public static async Task MigrateAsync(this IServiceProvider serviceProvider)
    {
#pragma warning disable MA0004
        await using var scope = serviceProvider.CreateAsyncScope();

        await using var dbContext = scope.ServiceProvider.GetRequiredService<ProjectDbContext>();
        await dbContext.Database.MigrateAsync();
#pragma warning restore MA0004
    }
}