using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Tests;

public sealed class SutBuilder<T> where T : class
{
    private readonly ServiceCollection _services = [];
    private ServiceProvider? _serviceProvider;
    private ServiceProvider ServiceProvider => _serviceProvider ??= BuildServiceProvider();

    public TService AddSubstitute<TService>() where TService : class
    {
        var substitute = Substitute.For<TService>();
        _services.AddSingleton(substitute);
        return substitute;
    }

    public TService GetService<TService>() where TService : notnull
    {
        return ServiceProvider.GetRequiredService<TService>();
    }

    public T Sut()
    {
        return ServiceProvider.GetRequiredService<T>();
    }
    
    private ServiceProvider BuildServiceProvider()
    {
        _services.AddSingleton<T>();
        
        return _services.BuildServiceProvider();
    }
}