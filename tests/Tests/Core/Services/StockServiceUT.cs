using AutoFixture;
using Core.Models.Ids;
using Core.Models.Stocks;
using Core.Repositories;
using Core.Services;
using NSubstitute;
using NUnit.Framework;
using Tests.DataBuilders;

namespace Tests.Core;

public sealed class StockServiceUT : BaseUT<IStockService, StockService>
{
    private IStockStorageRepository _stockStorageRepository = null!;
    
    [SetUp]
    public void Setup()
    {
        _stockStorageRepository = SutBuilder.AddSubstitute<IStockStorageRepository>();
    }
    
    [Test]
    public async Task AddStocksAsync_NoDuplicates_ReturnsAddStocksResponse()
    {
        // Arrange
        var request = DataBuilder.AddStocksRequest().Create();
        
        MockListStocksWithIdsAsync([]);
        
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
        var request = DataBuilder.AddStocksRequest(errorIfDuplicate: false).Create();
        var existingStocks = DataBuilder.ListStocksResponse().Create();
        
        MockListStocksWithIdsAsync(existingStocks);
        
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
        var request = DataBuilder.AddStocksRequest(errorIfDuplicate: true).Create();
        var existingStocks = DataBuilder.ListStocksResponse().Create();
        
        MockListStocksWithIdsAsync(existingStocks);
        
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
        
        MockListStocksWithIdsAsync(existingStocks);
        
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
        var request = DataBuilder.DeleteStocksRequest(stockIdCount: 1, errorIfMissing: false).Create();
        
        MockListStocksWithIdsAsync([]);
        
        // Act
        var actual = await Sut.DeleteStocksAsync(request, CancellationToken.None);

        // Assert
        Assert.That(actual.DeletedStockIds, Is.Empty);
    }
    
    [Test]
    public void DeleteStocksAsync_WithMissingStocksAndThrowErrorTrue_ThrowsInvalidOperationException()
    {
        // Arrange
        var request = DataBuilder.DeleteStocksRequest(stockIdCount: 1, errorIfMissing: true).Create();

        MockListStocksWithIdsAsync([]);
        
        // Act
        var ex = Assert.ThrowsAsync<InvalidOperationException>(
            async () => await Sut.DeleteStocksAsync(request, CancellationToken.None));

        // Assert
        Assert.That(ex?.Message, Is.EqualTo("Tickets with the following ids do not exist: " + string.Join(", ", request.StockIds)));
    }

    private void MockListStocksWithIdsAsync(IEnumerable<ListStockResponse> responses)
    {
        var response = DataBuilder.ListStocksResponse().With(x => x.Stocks, responses).Create();
        MockListStocksWithIdsAsync(response);
    }

    private void MockListStocksWithIdsAsync(ListStocksResponse response)
    {
        _stockStorageRepository.ListStocksWithIdsAsync(
                Arg.Any<ListStocksWithIdsRequest>(),
                CancellationToken)
            .Returns(response);
    }
    
    
    private IEnumerable<ListStockResponse> GetListStockResponsesWithIds(IEnumerable<StockId> stockIds)
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