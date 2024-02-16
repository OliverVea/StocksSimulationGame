using AutoFixture;
using Core.Models.Ids;
using Core.Repositories;
using NUnit.Framework;

namespace Tests.Persistence;

public class TickerStorageRepositoryIT : BaseIT
{
    [Test]
    public async Task AddTickersAsync_WithSingleTicker_RespondsCreatedTicker()
    {
        // Arrange
        var tickerId = new TickerId("AAPL");

        var request = DataBuilder.AddTickersRequest()
            .With(x => x.TickerIds, [tickerId])
            .Create();

        // Act
        var response = await GetService<ITickerStorageRepository>().AddTickersAsync(request, CancellationToken.None);

        // Assert
        Assert.That(response.AddedTickerIds, Has.Count.EqualTo(1));

        var createdId = response.AddedTickerIds.Single();
        Assert.That(createdId, Is.EqualTo(tickerId));
    }

    [Test]
    public async Task ListTickersAsync_WithSingleTicker_RespondsSingleTicker()
    {
        // Arrange
        var tickerId = new TickerId("IBM");

        var addRequest = DataBuilder.AddTickersRequest()
            .With(x => x.TickerIds, [tickerId])
            .Create();

        await GetService<ITickerStorageRepository>().AddTickersAsync(addRequest, CancellationToken.None);

        var request = DataBuilder.ListTickersRequest().Create();

        // Act
        var listResponse = await GetService<ITickerStorageRepository>().ListTickersAsync(request, CancellationToken.None);

        // Assert
        var expectedTickerIds = listResponse.TickerIds.Where(x => x == tickerId).ToArray();
        Assert.That(expectedTickerIds, Has.Length.EqualTo(1));
    }

    [Test]
    public async Task DeleteTickersAsync_WithSingleTicker_RespondsDeletedTicker()
    {
        // Arrange
        var tickerId = new TickerId("GOOGL");

        var addRequest = DataBuilder.AddTickersRequest()
            .With(x => x.TickerIds, [tickerId])
            .Create();

        await GetService<ITickerStorageRepository>().AddTickersAsync(addRequest, CancellationToken.None);

        var deleteRequest = DataBuilder.DeleteTickersRequest()
            .With(x => x.TickerIds, [tickerId])
            .Create();

        // Act
        var deleteResponse = await GetService<ITickerStorageRepository>().DeleteTickersAsync(deleteRequest, CancellationToken.None);
        var listResponse = await GetService<ITickerStorageRepository>().ListTickersAsync(DataBuilder.ListTickersRequest().Create(), CancellationToken.None);

        // Assert
        Assert.That(deleteResponse.DeletedTickerIds, Has.Count.EqualTo(1));

        var deletedId = deleteResponse.DeletedTickerIds.Single();
        Assert.That(deletedId, Is.EqualTo(tickerId));

        var expectedTickerIds = listResponse.TickerIds.Where(x => x == tickerId).ToArray();
        Assert.That(expectedTickerIds, Has.Length.EqualTo(0));
    }
}