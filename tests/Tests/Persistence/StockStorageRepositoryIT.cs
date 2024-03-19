using AutoFixture;
using Core.Models.Ids;
using Core.Models.Stocks;
using Core.Repositories;
using NUnit.Framework;
using Tests.DataBuilders;

namespace Tests.Persistence;

public sealed class StockStorageRepositoryIT : BaseIT<IStockStorageRepository>
{
    [Test]
    public async Task AddStocksAsync_WithSingleStock_RespondsCreatedStock()
    {
        // Arrange
        var stock = DataBuilder.AddStockRequest().Create();

        var request = DataBuilder.AddStocksRequest(stocks: [stock]).Create();

        // Act
        await Sut.AddStocksAsync(request, CancellationToken.None);
        var listResponse = await Sut.ListStocksAsync(DataBuilder.ListStocksRequest().Create(), CancellationToken.None);

        // Assert
        var expectedStockIds = listResponse.Stocks.Where(x => x.StockId == stock.StockId).ToArray();
        Assert.That(expectedStockIds, Has.Length.EqualTo(1));
    }

    [Test]
    public async Task ListStocksAsync_WithSingleStock_RespondsSingleStock()
    {
        // Arrange
        var stock = DataBuilder.AddStockRequest().Create();

        var addRequest = DataBuilder.AddStocksRequest(stocks: [stock]).Create();

        await Sut.AddStocksAsync(addRequest, CancellationToken.None);

        var request = DataBuilder.ListStocksRequest().Create();

        // Act
        var listResponse = await Sut.ListStocksAsync(request, CancellationToken.None);

        // Assert
        var expectedStockIds = listResponse.Stocks.Where(x => x.StockId == stock.StockId).ToArray();
        Assert.That(expectedStockIds, Has.Length.EqualTo(1));
    }

    [Test]
    public async Task DeleteStocksAsync_WithSingleStock_RespondsDeletedStock()
    {
        // Arrange
        var stock = DataBuilder.AddStockRequest().Create();
        var stockIds = new HashSet<StockId> { stock.StockId };

        var addRequest = DataBuilder.AddStocksRequest(stocks: [stock]).Create();

        await Sut.AddStocksAsync(addRequest, CancellationToken.None);

        var deleteRequest = DataBuilder.DeleteStocksRequest(stockIds: stockIds).Create();

        // Act
        await Sut.DeleteStocksAsync(deleteRequest, CancellationToken.None);
        var listResponse = await Sut.ListStocksAsync(DataBuilder.ListStocksRequest().Create(), CancellationToken.None);

        // Assert
        var expectedStockIds = listResponse.Stocks.Where(x => x.StockId == stock.StockId).ToArray();
        Assert.That(expectedStockIds, Has.Length.EqualTo(0));
    }

    [Test]
    public async Task ListStocksWithIdsAsync_WithStockIds_OnlyStocksWithIdsAreReturned()
    {
        // Arrange
        var stocks = DataBuilder.AddStockRequest().CreateMany(5).ToArray();
        var stockIds = stocks[..3].Select(x => x.StockId).ToArray();

        var addRequest = DataBuilder.AddStocksRequest(stocks: stocks).Create();

        await Sut.AddStocksAsync(addRequest, CancellationToken.None);

        var request = DataBuilder.ListStocksWithIdsRequest(stockIds: stockIds).Create();

        // Act
        var listResponse = await Sut.ListStocksWithIdsAsync(request, CancellationToken.None);

        // Assert
        var expectedStockIds = listResponse.Stocks.Select(x => x.StockId).ToArray();
        Assert.That(expectedStockIds, Is.EquivalentTo(stockIds));
    }
    
    [Test]
    public async Task UpdateStocksAsync_WithStocks_UpdatesStocks()
    {
        // Arrange
        var stocks = DataBuilder.AddStockRequest().CreateMany(1).ToArray();
        var addRequest = DataBuilder.AddStocksRequest(stocks: stocks).Create();

        await Sut.AddStocksAsync(addRequest, CancellationToken.None);

        var updatedStocks = MapToUpdate(stocks.Select(x => x with
        {
            Ticker = x.Ticker + "_new",
            Drift = x.Drift + 0.1f,
            Volatility = x.Volatility + 0.1f,
            Color = DataBuilder.Color().Create(),
        }).ToArray());
        
        var updateRequest = DataBuilder.UpdateStocksRequest(stocks: updatedStocks).Create();
        
        // Act
        await Sut.UpdateStocksAsync(updateRequest, CancellationToken.None);
        
        // Assert
        var listResponse = await Sut.ListStocksAsync(DataBuilder.ListStocksRequest().Create(), CancellationToken.None);
        
        var expectedStocks = MapToUpdate(listResponse.Stocks.Where(x => updatedStocks.Any(y => y.StockId == x.StockId)).ToArray());
        
        Assert.That(expectedStocks, Has.Length.EqualTo(updatedStocks.Count));
        Assert.That(expectedStocks, Is.EquivalentTo(updatedStocks));
    }
    
    private static IReadOnlyCollection<UpdateStockRequest> MapToUpdate(IReadOnlyCollection<AddStockRequest> stocks)
    {
        return stocks.Select(x => new UpdateStockRequest
        {
            StockId = x.StockId,
            Ticker = x.Ticker,
            Drift = x.Drift,
            Volatility = x.Volatility,
            Color = x.Color,
        }).ToArray();
    }
    
    private static IReadOnlyCollection<UpdateStockRequest> MapToUpdate(IReadOnlyCollection<ListStockResponse> stocks)
    {
        return stocks.Select(x => new UpdateStockRequest
        {
            StockId = x.StockId,
            Ticker = x.Ticker,
            Drift = x.Drift,
            Volatility = x.Volatility,
            Color = x.Color,
        }).ToArray();
    }
}