using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Persistence;
using Tests.DataBuilders;

namespace Tests;

[Category("Integration Test")]
public abstract class BaseIT
{
    protected readonly DataBuilder DataBuilder = new();

    private IServiceProvider _services = null!;

    [OneTimeSetUp]
    public async Task CreateServiceProvider()
    {
        var services = new ServiceCollection();

        var logger = new Logger("console", InternalTraceLevel.Debug, TestContext.Out);

        services.AddSingleton<ILogger>(logger);

        var sqliteConnection = new SqliteConnection("DataSource=Search;Filename=:memory:");
        services.AddPersistence(sqliteConnection);

        _services = services.BuildServiceProvider();

        await _services.EnsureCreatedAsync();
    }

    protected T GetService<T>() where T : notnull
    {
        return _services.GetRequiredService<T>();
    }
}