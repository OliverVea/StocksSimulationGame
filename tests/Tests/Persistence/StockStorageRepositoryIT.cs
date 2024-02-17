using AutoFixture;
using Core.Models.Ids;
using Core.Repositories;
using NUnit.Framework;

namespace Tests.Persistence;

public class StockStorageRepositoryIT : BaseIT
{
    [Test]
    public async Task AddStocksAsync_WithSingleStock_RespondsCreatedStock()
    {
        // Arrange
        var stock = DataBuilder.AddStockRequest().Create();

        var request = DataBuilder.AddStocksRequest()
            .With(x => x.Stocks, [stock])
            .Create();

        // Act
        await GetService<IStockStorageRepository>().AddStocksAsync(request, CancellationToken.None);
        var listResponse = await GetService<IStockStorageRepository>().ListStocksAsync(DataBuilder.ListStocksRequest().Create(), CancellationToken.None);

        // Assert
        var expectedStockIds = listResponse.Stocks.Where(x => x.StockId == stock.StockId).ToArray();
        Assert.That(expectedStockIds, Has.Length.EqualTo(1));
    }

    [Test]
    public async Task ListStocksAsync_WithSingleStock_RespondsSingleStock()
    {
        // Arrange
        var stock = DataBuilder.AddStockRequest().Create();

        var addRequest = DataBuilder.AddStocksRequest()
            .With(x => x.Stocks, [stock])
            .Create();

        await GetService<IStockStorageRepository>().AddStocksAsync(addRequest, CancellationToken.None);

        var request = DataBuilder.ListStocksRequest().Create();

        // Act
        var listResponse = await GetService<IStockStorageRepository>().ListStocksAsync(request, CancellationToken.None);

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

        var addRequest = DataBuilder.AddStocksRequest()
            .With(x => x.Stocks, [stock])
            .Create();

        await GetService<IStockStorageRepository>().AddStocksAsync(addRequest, CancellationToken.None);

        var deleteRequest = DataBuilder.DeleteStocksRequest()
            .With(x => x.StockIds, stockIds)
            .Create();

        // Act
        await GetService<IStockStorageRepository>().DeleteStocksAsync(deleteRequest, CancellationToken.None);
        var listResponse = await GetService<IStockStorageRepository>().ListStocksAsync(DataBuilder.ListStocksRequest().Create(), CancellationToken.None);

        // Assert
        var expectedStockIds = listResponse.Stocks.Where(x => x.StockId == stock.StockId).ToArray();
        Assert.That(expectedStockIds, Has.Length.EqualTo(0));
    }
}