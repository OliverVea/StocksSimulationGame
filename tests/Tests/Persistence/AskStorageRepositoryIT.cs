using AutoFixture;
using Core.Models.Ids;
using Core.Repositories;
using NUnit.Framework;
using Tests.DataBuilders;

namespace Tests.Persistence;

public sealed class AskStorageRepositoryIT : BaseIT<IAskStorageRepository>
{
    private UserId _userId;
    
    [SetUp]
    public void SetupUser()
    {
        _userId = WithUserId();
    }
    
    [Test]
    public async Task GetAsksAsync_NoAsksInDatabase_EmptyResponseIsReturned()
    {
        // Arrange
        var request = DataBuilder.GetAsksRequest(_userId, Array.Empty<AskId>()).Create();

        // Act
        var actual = await Sut.GetAsksAsync(request, CancellationToken);

        // Assert
        Assert.That(actual.Asks, Is.Empty);
    }
    
    [Test]
    public async Task GetAsksAsync_WithAskInDatabase_AskIsReturned()
    {
        // Arrange
        var ask = DataBuilder.Ask().With(x => x.UserId, _userId).Create();
        await Sut.CreateAskAsync(ask, CancellationToken);
        
        var request = DataBuilder.GetAsksRequest(_userId, [ask.AskId]).Create();

        // Act
        var actual = await Sut.GetAsksAsync(request, CancellationToken);

        // Assert
        Assert.That(actual.Asks, Has.Length.EqualTo(1));
        var actualAsk = actual.Asks.Single();
        Assert.That(actualAsk, Is.EqualTo(ask));
    }

    [Test]
    public async Task GetAsksAsync_AskIdsNotSpecified_AllAsksForUserAreReturned()
    {
        // Arrange
        const int askCount = 42;
        var asks = DataBuilder.Ask().With(x => x.UserId, _userId).CreateMany(askCount).ToArray();
        foreach (var ask in asks) await Sut.CreateAskAsync(ask, CancellationToken);

        var request = DataBuilder.GetAsksRequest(_userId).Create();

        // Act
        var actual = await Sut.GetAsksAsync(request, CancellationToken);

        // Assert
        Assert.That(actual.Asks, Is.EquivalentTo(asks));
        Assert.That(actual.Asks, Has.Length.EqualTo(askCount));
    }
    
    [Test]
    public async Task GetAsksAsync_WithStockIdSpecified_OnlyAsksForStockAreReturned()
    {
        // Arrange
        var asks = DataBuilder.Ask().With(x => x.UserId, _userId).CreateMany(2).ToArray();
        var ask = asks.First();
        foreach (var a in asks) await Sut.CreateAskAsync(a, CancellationToken);

        var request = DataBuilder.GetAsksRequest(_userId, stockId: ask.StockId).Create();

        // Act
        var actual = await Sut.GetAsksAsync(request, CancellationToken);

        // Assert
        Assert.That(actual.Asks, Has.Length.EqualTo(1));
        Assert.That(actual.Asks.Single(), Is.EqualTo(ask));
    }
    
    [Test]
    public async Task GetAsksAsync_WithMaxPriceSpecified_OnlyAsksWithPriceBelowMaxAreReturned()
    {
        // Arrange
        const int count = 100;
        const int top = 5;
        
        var asks = DataBuilder.Ask().With(x => x.UserId, _userId)
            .CreateMany(count)
            .OrderBy(x => x.PricePerUnit.Value)
            .ToArray();
        
        foreach (var ask in asks) await Sut.CreateAskAsync(ask, CancellationToken);
        
        var topAsks = asks[..top];
        var maxPrice = topAsks.Last().PricePerUnit;

        var request = DataBuilder.GetAsksRequest(_userId, maxPrice: maxPrice).Create();

        // Act
        var actual = await Sut.GetAsksAsync(request, CancellationToken);

        // Assert
        Assert.That(actual.Asks, Is.EquivalentTo(topAsks));
    }
    
    [Test]
    public async Task GetAsksAsync_WithAskIdsSpecified_OnlySpecifiedAsksAreReturned()
    {
        // Arrange
        var asks = DataBuilder.Ask().With(x => x.UserId, _userId).CreateMany(10).ToArray();
        foreach (var ask in asks) await Sut.CreateAskAsync(ask, CancellationToken);
        
        var request = DataBuilder.GetAsksRequest(_userId, asks[..5].Select(x => x.AskId).ToArray()).Create();

        // Act
        var actual = await Sut.GetAsksAsync(request, CancellationToken);

        // Assert
        Assert.That(actual.Asks, Is.EquivalentTo(asks[..5]));
    }
    
    [Test]
    public async Task DeleteAsksAsync_WithAskInDatabase_AskIsDeleted()
    {
        // Arrange
        var ask = DataBuilder.Ask().With(x => x.UserId, _userId).Create();
        await Sut.CreateAskAsync(ask, CancellationToken);
        
        var request = DataBuilder.DeleteAsksRequest([ask.AskId]).Create();

        // Act
        await Sut.DeleteAsksAsync(request, CancellationToken);

        // Assert
        var getAsksRequest = DataBuilder.GetAsksRequest(_userId, [ask.AskId]).Create();
        var actual = await Sut.GetAsksAsync(getAsksRequest, CancellationToken);
        Assert.That(actual.Asks, Is.Empty);
    }
}