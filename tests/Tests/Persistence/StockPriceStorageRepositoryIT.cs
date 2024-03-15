using AutoFixture;
using Core.Repositories;
using NUnit.Framework;

namespace Tests.Persistence;

public class StockPriceStorageRepositoryIT : BaseIT<IStockPriceStorageRepository>
{
    [Test]
    public async Task GetStockPricesAsync_WithNoStockPrices_RespondsEmptyStockPrices()
    {
        // Arrange
        var stock = DataBuilder.AddStockRequest().Create();
        var request = DataBuilder.GetStockPriceInIntervalRequest().With(x => x.StockId, stock.StockId).Create();

        // Act
        var response = await Sut.GetStockPriceInIntervalAsync(request, CancellationToken.None);

        // Assert
        Assert.That(response.StockPrices, Is.Empty);
    }
    
    [Test]
    public async Task SetStockPricesAsync_WithSingleStockPrice_RespondsCreatedStockPrice()
    {
        // Arrange
        var stockId = DataBuilder.StockId().Create();
        var stockPrice = DataBuilder.SetStockPriceRequest().With(x => x.StockId, stockId).Create();
        var setStockPricesRequest = DataBuilder.SetStockPricesRequest().With(x => x.StockPrices, [stockPrice]).Create();

        var getStockPricesRequest = DataBuilder.GetStockPriceInIntervalRequest().With(x => x.StockId, stockId).Create();
        
        // Act
        await Sut.SetStockPricesAsync(setStockPricesRequest, CancellationToken.None);
        var response = await Sut.GetStockPriceInIntervalAsync(getStockPricesRequest, CancellationToken.None);

        // Assert
        Assert.That(response.StockPrices, Has.Length.EqualTo(1));
    }
    
    [Test]
    public async Task SetStockPricesAsync_WithMultipleStockPrices_RespondsCreatedStockPrices()
    {
        // Arrange
        var stockId = DataBuilder.StockId().Create();
        var stockPrices = DataBuilder.SetStockPriceRequest().With(x => x.StockId, stockId).CreateMany(10).ToArray();
        var setStockPricesRequest = DataBuilder.SetStockPricesRequest().With(x => x.StockPrices, stockPrices).Create();

        var getStockPricesRequest = DataBuilder.GetStockPriceInIntervalRequest().With(x => x.StockId, stockId).Create();
        
        // Act
        await Sut.SetStockPricesAsync(setStockPricesRequest, CancellationToken.None);
        var response = await Sut.GetStockPriceInIntervalAsync(getStockPricesRequest, CancellationToken.None);

        // Assert
        Assert.That(response.StockPrices, Has.Length.EqualTo(10));
    }

    [Test]
    public async Task GetStockPricesAsync_TwoDifferentStocks_OnlyGetsRequestedStock()
    {
        // Arrange
        var stock1 = DataBuilder.AddStockRequest().Create();
        var stock2 = DataBuilder.AddStockRequest().Create();
        var stockPrice1 = DataBuilder.SetStockPriceRequest().With(x => x.StockId, stock1.StockId).Create();
        var stockPrice2 = DataBuilder.SetStockPriceRequest().With(x => x.StockId, stock2.StockId).Create();
        var setStockPricesRequest = DataBuilder.SetStockPricesRequest().With(x => x.StockPrices, [stockPrice1, stockPrice2]).Create();
        var getStockPricesRequest = DataBuilder.GetStockPriceInIntervalRequest().With(x => x.StockId, stock1.StockId).Create();
        
        // Act
        await Sut.SetStockPricesAsync(setStockPricesRequest, CancellationToken.None);
        var response = await Sut.GetStockPriceInIntervalAsync(getStockPricesRequest, CancellationToken.None);

        // Assert
        Assert.That(response.StockPrices, Has.Length.EqualTo(1));
    }
    
    [Test]
    public async Task DeleteStockPricesAsync_WithSingleStockPrice_RespondsDeletedStockPrice()
    {
        // Arrange
        var stockId = DataBuilder.StockId().Create();
        var setStockPrice = DataBuilder.SetStockPriceRequest().With(x => x.StockId, stockId).Create();
        var deleteStockPrice = DataBuilder.DeleteStockPriceRequest().With(x => x.StockId, stockId).Create();
        var setStockPricesRequest = DataBuilder.SetStockPricesRequest().With(x => x.StockPrices, [setStockPrice]).Create();
        var deleteStockPricesRequest = DataBuilder.DeleteStockPricesRequest().With(x => x.StockPrices, [deleteStockPrice]).Create();
        var getStockPricesRequest = DataBuilder.GetStockPriceInIntervalRequest().With(x => x.StockId, stockId).Create();
        
        // Act
        await Sut.SetStockPricesAsync(setStockPricesRequest, CancellationToken.None);
        await Sut.DeleteStockPricesAsync(deleteStockPricesRequest, CancellationToken.None);
        var response = await Sut.GetStockPriceInIntervalAsync(getStockPricesRequest, CancellationToken.None);

        // Assert
        Assert.That(response.StockPrices, Is.Empty);
    }
    
    [Test]
    public async Task DeleteStockPricesAsync_WithMultipleStockPrices_OnlyDeletesRequestedStockPrices()
    {
        // Arrange
        var stockId1 = DataBuilder.StockId().Create();
        var stockId2 = DataBuilder.StockId().Create();
        var setStockPrice1 = DataBuilder.SetStockPriceRequest().With(x => x.StockId, stockId1).Create();
        var setStockPrice2 = DataBuilder.SetStockPriceRequest().With(x => x.StockId, stockId2).Create();
        var deleteStockPrice1 = DataBuilder.DeleteStockPriceRequest().With(x => x.StockId, stockId1).Create();
        var setStockPricesRequest = DataBuilder.SetStockPricesRequest().With(x => x.StockPrices, [setStockPrice1, setStockPrice2]).Create();
        var deleteStockPricesRequest = DataBuilder.DeleteStockPricesRequest().With(x => x.StockPrices, [deleteStockPrice1]).Create();
        var getStockPricesRequest1 = DataBuilder.GetStockPriceInIntervalRequest().With(x => x.StockId, stockId1).Create();
        var getStockPricesRequest2 = DataBuilder.GetStockPriceInIntervalRequest().With(x => x.StockId, stockId2).Create();
        
        // Act
        await Sut.SetStockPricesAsync(setStockPricesRequest, CancellationToken.None);
        await Sut.DeleteStockPricesAsync(deleteStockPricesRequest, CancellationToken.None);
        var response1 = await Sut.GetStockPriceInIntervalAsync(getStockPricesRequest1, CancellationToken.None);
        var response2 = await Sut.GetStockPriceInIntervalAsync(getStockPricesRequest2, CancellationToken.None);

        // Assert
        Assert.That(response1.StockPrices, Is.Empty);
        Assert.That(response2.StockPrices, Has.Length.EqualTo(1));
    }
}