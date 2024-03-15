using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Persistence;
using Persistence.Configuration;
using Tests.DataBuilders;

namespace Tests;

[Category("Integration Test")]
public abstract class BaseIT<TSut> where TSut : notnull
{
    protected readonly DataBuilder DataBuilder = new();

    private IServiceProvider _services = null!;
    
    protected TSut Sut => GetService<TSut>();

    [OneTimeSetUp]
    public async Task CreateServiceProvider()
    {
        var services = new ServiceCollection();

        services.AddSqlite(new PersistenceConfiguration
        {
            Provider = PersistenceProvider.Sqlite,
            Sqlite = new SqliteConfiguration("DataSource=Stocks;Filename=:memory:")
        });

        _services = services.BuildServiceProvider();

        await _services.EnsureCreatedAsync();
    }

    protected T GetService<T>() where T : notnull
    {
        return _services.GetRequiredService<T>();
    }
}