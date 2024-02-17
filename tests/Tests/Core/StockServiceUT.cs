using AutoFixture;
using Core.Models.Ids;
using Core.Models.Stocks;
using Core.Repositories;
using Core.Services;
using Moq;
using NUnit.Framework;

namespace Tests.Core;

public class StockServiceUT : BaseUT<IStockService, StockService>
{
    [Test]
    public async Task AddStocksAsync_NoDuplicates_ReturnsAddStocksResponse()
    {
        // Arrange
        var request = DataBuilder.AddStocksRequest().Create();
        var expected = DataBuilder.AddStocksResponse().Create();
        var existingStocks = DataBuilder.ListStocksResponse()
            .With(x => x.Stocks, Array.Empty<ListStockResponse>())
            .Create();

        SutBuilder.AddMock<IStockStorageRepository>(
            mock => mock.Setup(x => x.ListStocksWithIdsAsync(
                    It.IsAny<ListStocksWithIdsRequest>(),
                    CancellationToken))
                .ReturnsAsync(existingStocks));
        
        // Act
        var actual = await Sut.AddStocksAsync(request, CancellationToken.None);

        // Assert
        var actualStockIds = actual.Stocks.Select(x => x.StockId);
        var expectedStockIds = request.Stocks.Select(x => x.StockId);
        
        Assert.That(actualStockIds, Is.EquivalentTo(expectedStockIds));
    }
    
    [Test]
    public async Task AddStocksAsync_WithDuplicatesAndThrowErrorFalse_ReturnsAddStocksResponse()
    {
        // Arrange
        var request = DataBuilder.AddStocksRequest()
            .With(x => x.ErrorIfDuplicate, false).Create();
        var existingStocks = DataBuilder.ListStocksResponse().Create();

        SutBuilder.AddMock<IStockStorageRepository>(
            mock => mock.Setup(x => x.ListStocksWithIdsAsync(
                    It.IsAny<ListStocksWithIdsRequest>(),
                    CancellationToken))
                .ReturnsAsync(existingStocks));
        
        // Act
        var actual = await Sut.AddStocksAsync(request, CancellationToken.None);

        // Assert
        var actualStockIds = actual.Stocks.Select(x => x.StockId);
        var expectedStockIds = request.Stocks.Select(x => x.StockId);
        
        Assert.That(actualStockIds, Is.EquivalentTo(expectedStockIds));
    }
    
    [Test]
    public void AddStocksAsync_WithDuplicatesAndThrowErrorTrue_ThrowsInvalidOperationException()
    {
        // Arrange
        var request = DataBuilder.AddStocksRequest()
            .With(x => x.ErrorIfDuplicate, true).Create();
        var existingStocks = DataBuilder.ListStocksResponse().Create();

        SutBuilder.AddMock<IStockStorageRepository>(
            mock => mock.Setup(x => x.ListStocksWithIdsAsync(
                    It.IsAny<ListStocksWithIdsRequest>(),
                    CancellationToken))
                .ReturnsAsync(existingStocks));
        
        // Act
        var ex = Assert.ThrowsAsync<InvalidOperationException>(
            async () => await Sut.AddStocksAsync(request, CancellationToken.None));

        // Assert
        Assert.That(ex?.Message, Is.EqualTo("Tickets with the following ids already exist: " + string.Join(", ", existingStocks.Stocks.Select(x => x.StockId))));
    }
    
    [Test]
    public async Task DeleteStocksAsync_WithExistingStocks_ReturnsDeleteStocksResponse()
    {
        // Arrange
        var request = DataBuilder.DeleteStocksRequest().Create();
        var existingStocks = GetListStockResponsesWithIds(request.StockIds);
        var listStocksResponse = DataBuilder.ListStocksResponse().With(x => x.Stocks, existingStocks).Create();

        SutBuilder.AddMock<IStockStorageRepository>(
            mock =>
            {
                mock.Setup(x => x.ListStocksWithIdsAsync(
                        It.IsAny<ListStocksWithIdsRequest>(),
                        CancellationToken))
                    .ReturnsAsync(listStocksResponse);
            });
        
        // Act
        var actual = await Sut.DeleteStocksAsync(request, CancellationToken.None);

        // Assert
        var actualStockIds = actual.DeletedStockIds;
        var expectedStockIds = request.StockIds;
        
        Assert.That(actualStockIds, Is.EquivalentTo(expectedStockIds));
    }
    
    [Test]
    public async Task DeleteStocksAsync_WithMissingStocksAndThrowErrorFalse_ReturnsDeleteStocksResponse()
    {
        // Arrange
        var request = DataBuilder.DeleteStocksRequest().With(x => x.ErrorIfMissing, false).Create();
        var existingStocks = DataBuilder.ListStocksResponse().With(x => x.Stocks, Array.Empty<ListStockResponse>()).Create();

        SutBuilder.AddMock<IStockStorageRepository>(
            mock => mock.Setup(x => x.ListStocksWithIdsAsync(
                    It.IsAny<ListStocksWithIdsRequest>(),
                    CancellationToken))
                .ReturnsAsync(existingStocks));
        
        // Act
        var actual = await Sut.DeleteStocksAsync(request, CancellationToken.None);

        // Assert
        Assert.That(actual.DeletedStockIds, Is.Empty);
    }
    
    [Test]
    public void DeleteStocksAsync_WithMissingStocksAndThrowErrorTrue_ThrowsInvalidOperationException()
    {
        // Arrange
        var request = DataBuilder.DeleteStocksRequest().With(x => x.ErrorIfMissing, true).Create();
        var existingStocks = DataBuilder.ListStocksResponse()
            .With(x => x.Stocks, Array.Empty<ListStockResponse>())
            .Create();

        SutBuilder.AddMock<IStockStorageRepository>(
            mock => mock.Setup(x => x.ListStocksWithIdsAsync(
                    It.IsAny<ListStocksWithIdsRequest>(),
                    CancellationToken))
                .ReturnsAsync(existingStocks));
        
        // Act
        var ex = Assert.ThrowsAsync<InvalidOperationException>(
            async () => await Sut.DeleteStocksAsync(request, CancellationToken.None));

        // Assert
        Assert.That(ex?.Message, Is.EqualTo("Tickets with the following ids do not exist: " + string.Join(", ", request.StockIds)));
    }
    
    
    private IEnumerable<ListStockResponse> GetListStockResponsesWithIds(IReadOnlyCollection<StockId> stockIds)
    {
        List<ListStockResponse> listStockResponses = [];
        
        foreach (var stockId in stockIds)
        {
            var listStockResponse = DataBuilder.ListStockResponse()
                .With(x => x.StockId, stockId)
                .Create();
            
            listStockResponses.Add(listStockResponse);
        }
        
        return listStockResponses;
    }
}