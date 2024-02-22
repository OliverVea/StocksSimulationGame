using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Persistence;
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

        var logger = new Logger("console", InternalTraceLevel.Debug, TestContext.Out);

        services.AddSingleton<ILogger>(logger);

        var configurationBuilder = new Microsoft.Extensions.Configuration.ConfigurationBuilder();
        configurationBuilder.Add(new MemoryConfigurationSource
        {
            InitialData = new[]
            {
                new KeyValuePair<string, string?>("Persistence:Provider", "Sqlite"),
                new KeyValuePair<string, string?>("Persistence:Sqlite:ConnectionString", "DataSource=Stocks;Filename=:memory:"),
            },
        });
        
        var configuration = configurationBuilder.Build();

        services.AddPersistence(configuration);

        _services = services.BuildServiceProvider();

        await _services.EnsureCreatedAsync();
    }

    protected T GetService<T>() where T : notnull
    {
        return _services.GetRequiredService<T>();
    }
}