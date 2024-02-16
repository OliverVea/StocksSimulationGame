using AutoFixture;
using Core.Models.Ids;
using Core.Models.Tickers;
using Core.Repositories;
using Core.Services;
using Moq;
using NUnit.Framework;

namespace Tests.Core;

public class TickerServiceUT : BaseUT<ITickerService, TickerService>
{
    [Test]
    public async Task AddTickersAsync_NoDuplicates_ReturnsAddTickersResponse()
    {
        // Arrange
        var request = DataBuilder.AddTickersRequest().Create();
        var expected = DataBuilder.AddTickersResponse().Create();
        var existingTickers = DataBuilder.ListTickersResponse()
            .With(x => x.TickerIds, Array.Empty<TickerId>())
            .Create();

        SutBuilder.AddMock<ITickerStorageRepository>(
            mock =>
            {
                mock.Setup(x => x.ListTickersWithIdsAsync(
                        It.IsAny<ListTickersWithIdsRequest>(),
                        CancellationToken))
                    .ReturnsAsync(existingTickers);

                mock.Setup(x => x.AddTickersAsync(
                        It.IsAny<AddTickersRequest>(),
                        CancellationToken))
                    .ReturnsAsync(expected);
            });
        
        // Act
        var actual = await Sut.AddTickersAsync(request, CancellationToken.None);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    public async Task AddTickersAsync_WithDuplicatesAndThrowErrorFalse_ReturnsAddTickersResponse()
    {
        // Arrange
        var request = DataBuilder.AddTickersRequest()
            .With(x => x.ErrorIfDuplicate, false).Create();
        var expected = DataBuilder.AddTickersResponse().Create();
        var existingTickers = DataBuilder.ListTickersResponse().Create();

        SutBuilder.AddMock<ITickerStorageRepository>(
            mock =>
            {
                mock.Setup(x => x.ListTickersWithIdsAsync(
                        It.IsAny<ListTickersWithIdsRequest>(),
                        CancellationToken))
                    .ReturnsAsync(existingTickers);

                mock.Setup(x => x.AddTickersAsync(
                        It.IsAny<AddTickersRequest>(),
                        CancellationToken))
                    .ReturnsAsync(expected);
            });
        
        // Act
        var actual = await Sut.AddTickersAsync(request, CancellationToken.None);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    public void AddTickersAsync_WithDuplicatesAndThrowErrorTrue_ThrowsInvalidOperationException()
    {
        // Arrange
        var request = DataBuilder.AddTickersRequest()
            .With(x => x.ErrorIfDuplicate, true).Create();
        var existingTickers = DataBuilder.ListTickersResponse().Create();

        SutBuilder.AddMock<ITickerStorageRepository>(
            mock =>
            {
                mock.Setup(x => x.ListTickersWithIdsAsync(
                        It.IsAny<ListTickersWithIdsRequest>(),
                        CancellationToken))
                    .ReturnsAsync(existingTickers);
            });
        
        // Act
        var ex = Assert.ThrowsAsync<InvalidOperationException>(
            async () => await Sut.AddTickersAsync(request, CancellationToken.None));

        // Assert
        Assert.That(ex?.Message, Is.EqualTo("One or more tickers already exist"));
    }
    
    [Test]
    public async Task DeleteTickersAsync_WithExistingTickers_ReturnsDeleteTickersResponse()
    {
        // Arrange
        var request = DataBuilder.DeleteTickersRequest().Create();
        var expected = DataBuilder.DeleteTickersResponse().Create();
        var existingTickers = DataBuilder.ListTickersResponse()
            .With(x => x.TickerIds, request.TickerIds).Create();

        SutBuilder.AddMock<ITickerStorageRepository>(
            mock =>
            {
                mock.Setup(x => x.ListTickersWithIdsAsync(
                        It.IsAny<ListTickersWithIdsRequest>(),
                        CancellationToken))
                    .ReturnsAsync(existingTickers);

                mock.Setup(x => x.DeleteTickersAsync(
                        It.IsAny<DeleteTickersRequest>(),
                        CancellationToken))
                    .ReturnsAsync(expected);
            });
        
        // Act
        var actual = await Sut.DeleteTickersAsync(request, CancellationToken.None);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    public async Task DeleteTickersAsync_WithMissingTickersAndThrowErrorFalse_ReturnsDeleteTickersResponse()
    {
        // Arrange
        var request = DataBuilder.DeleteTickersRequest().With(x => x.ErrorIfNotFound, false).Create();
        var expected = DataBuilder.DeleteTickersResponse().Create();
        var existingTickers = DataBuilder.ListTickersResponse().Create();

        SutBuilder.AddMock<ITickerStorageRepository>(
            mock =>
            {
                mock.Setup(x => x.ListTickersWithIdsAsync(
                        It.IsAny<ListTickersWithIdsRequest>(),
                        CancellationToken))
                    .ReturnsAsync(existingTickers);

                mock.Setup(x => x.DeleteTickersAsync(
                        It.IsAny<DeleteTickersRequest>(),
                        CancellationToken))
                    .ReturnsAsync(expected);
            });
        
        // Act
        var actual = await Sut.DeleteTickersAsync(request, CancellationToken.None);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    public void DeleteTickersAsync_WithMissingTickersAndThrowErrorTrue_ThrowsInvalidOperationException()
    {
        // Arrange
        var request = DataBuilder.DeleteTickersRequest().With(x => x.ErrorIfNotFound, true).Create();
        var existingTickers = DataBuilder.ListTickersResponse()
            .With(x => x.TickerIds, Array.Empty<TickerId>())
            .Create();

        SutBuilder.AddMock<ITickerStorageRepository>(
            mock =>
            {
                mock.Setup(x => x.ListTickersWithIdsAsync(
                        It.IsAny<ListTickersWithIdsRequest>(),
                        CancellationToken))
                    .ReturnsAsync(existingTickers);
            });
        
        // Act
        var ex = Assert.ThrowsAsync<InvalidOperationException>(
            async () => await Sut.DeleteTickersAsync(request, CancellationToken.None));

        // Assert
        Assert.That(ex?.Message, Is.EqualTo("Tickets with the following ids do not exist: " + string.Join(", ", request.TickerIds)));
    }
}