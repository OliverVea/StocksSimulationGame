using AutoFixture;
using Core.Models.Ids;
using Core.Repositories;
using NUnit.Framework;

namespace Tests.Persistence;

public sealed class AskStorageRepositoryIT : BaseIT<IAskStorageRepository>
{
    [Test]
    public async Task GetAsksAsync_NoAsksInDatabase_EmptyResponseIsReturned()
    {
        // Arrange
        var userId = WithUserId();
        var request = DataBuilder.GetAsksRequest(userId, Array.Empty<AskId>()).Create();

        // Act
        var actual = await Sut.GetAsksAsync(request, CancellationToken);

        // Assert
        Assert.That(actual.Asks, Is.Empty);
    }
    
    [Test]
    public async Task GetAsksAsync_WithAskInDatabase_AskIsReturned()
    {
        // Arrange
        var userId = WithUserId();
        var ask = DataBuilder.Ask().With(x => x.UserId, userId).Create();
        await Sut.CreateAskAsync(ask, CancellationToken);
        
        var request = DataBuilder.GetAsksRequest(userId, [ask.AskId]).Create();

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
        var userId = WithUserId();
        var asks = DataBuilder.Ask().With(x => x.UserId, userId).CreateMany(askCount).ToArray();
        foreach (var ask in asks) await Sut.CreateAskAsync(ask, CancellationToken);

        var request = DataBuilder.GetAsksRequest(userId).Create();

        // Act
        var actual = await Sut.GetAsksAsync(request, CancellationToken);

        // Assert
        Assert.That(actual.Asks, Is.EquivalentTo(asks));
        Assert.That(actual.Asks, Has.Length.EqualTo(askCount));
    }
    
    [Test]
    public async Task DeleteAsksAsync_WithAskInDatabase_AskIsDeleted()
    {
        // Arrange
        var userId = WithUserId();
        var ask = DataBuilder.Ask().With(x => x.UserId, userId).Create();
        await Sut.CreateAskAsync(ask, CancellationToken);
        
        var request = DataBuilder.DeleteAsksRequest(userId, [ask.AskId]).Create();

        // Act
        await Sut.DeleteAsksAsync(request, CancellationToken);

        // Assert
        var getAsksRequest = DataBuilder.GetAsksRequest(userId, [ask.AskId]).Create();
        var actual = await Sut.GetAsksAsync(getAsksRequest, CancellationToken);
        Assert.That(actual.Asks, Is.Empty);
    }
}