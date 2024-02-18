using AutoFixture;
using Core.Models.Ids;
using Core.Repositories;
using NUnit.Framework;

namespace Tests.Persistence;

public class StockStorageRepositoryIT : BaseIT<IStockStorageRepository>
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

        var addRequest = DataBuilder.AddStocksRequest()
            .With(x => x.Stocks, [stock])
            .Create();

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

        var addRequest = DataBuilder.AddStocksRequest()
            .With(x => x.Stocks, [stock])
            .Create();

        await Sut.AddStocksAsync(addRequest, CancellationToken.None);

        var deleteRequest = DataBuilder.DeleteStocksRequest()
            .With(x => x.StockIds, stockIds)
            .Create();

        // Act
        await Sut.DeleteStocksAsync(deleteRequest, CancellationToken.None);
        var listResponse = await Sut.ListStocksAsync(DataBuilder.ListStocksRequest().Create(), CancellationToken.None);

        // Assert
        var expectedStockIds = listResponse.Stocks.Where(x => x.StockId == stock.StockId).ToArray();
        Assert.That(expectedStockIds, Has.Length.EqualTo(0));
    }
}