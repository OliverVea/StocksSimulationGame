using Core;
using Core.Models.Ids;
using Core.Services;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Persistence;
using Persistence.Configuration;

namespace Tests;

[Category("Integration Test")]
public abstract class BaseIT<TSut> where TSut : notnull
{
    protected CancellationTokenSource CancellationTokenSource = new();
    protected CancellationToken CancellationToken => CancellationTokenSource.Token;

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

        services.AddCore();

        _services = services.BuildServiceProvider();

        await _services.EnsureCreatedAsync();
    }

    protected T GetService<T>() where T : notnull
    {
        return _services.GetRequiredService<T>();
    }
    
    [TearDown]
    protected void ResetCancellationToken()
    {
        CancellationTokenSource.Cancel();
        CancellationTokenSource.Dispose();
        CancellationTokenSource = new CancellationTokenSource();
    }
    
    [TearDown]
    protected void ResetUserId()
    {
        GetService<IUserIdService>().Reset();
    }

    protected UserId WithUserId()
    {
        var userId = Guid.NewGuid().ToString();
        GetService<IUserIdService>().Initialize(userId);
        return GetService<IUserIdService>().UserId ?? throw new NotSupportedException();
    }
}