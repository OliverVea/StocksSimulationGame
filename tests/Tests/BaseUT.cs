using NUnit.Framework;
using Tests.DataBuilders;

namespace Tests;

[Category("Unit Test")]
public class BaseUT<TContract, TImplementation>
    where TImplementation : class, TContract
{
    protected readonly CancellationToken CancellationToken = new();
    
    protected readonly DataBuilder DataBuilder = new();

    private SutBuilder<TImplementation>? _sutBuilder;
    protected SutBuilder<TImplementation> SutBuilder => _sutBuilder ?? throw new InvalidOperationException("SutBuilder not initialized");
    protected TContract Sut => SutBuilder.Sut();
    
    [SetUp] 
    public void SetUp()
    {
        _sutBuilder = new SutBuilder<TImplementation>();
    }
}