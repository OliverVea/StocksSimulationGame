using NUnit.Framework;
using Tests.DataBuilders;

namespace Tests;

[Category("Unit Test")]
public abstract class BaseUT
{
    protected readonly CancellationToken CancellationToken = new();
}

public abstract class BaseUT<TContract, TImplementation> : BaseUT where TImplementation : class, TContract
{

    private SutBuilder<TImplementation>? _sutBuilder;
    protected SutBuilder<TImplementation> SutBuilder => _sutBuilder ?? throw new InvalidOperationException("SutBuilder not initialized");
    protected TContract Sut => SutBuilder.Sut();
    
    [SetUp] 
    public void SetUp()
    {
        _sutBuilder = new SutBuilder<TImplementation>();
    }
}