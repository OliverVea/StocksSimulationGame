﻿using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Tests;

public class SutBuilder<T> where T : class
{
    private readonly ServiceCollection _services = [];
    private ServiceProvider? _serviceProvider;
    private ServiceProvider ServiceProvider => _serviceProvider ??= BuildServiceProvider();

    public void AddMock<TService>(Action<Mock<TService>>? setup = null) where TService : class
    {
        var mock = new Mock<TService>();

        setup?.Invoke(mock);

        _services.AddSingleton(mock.Object);
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