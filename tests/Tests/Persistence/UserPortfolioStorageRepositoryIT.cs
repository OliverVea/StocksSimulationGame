using AutoFixture;
using Core.Models.Ids;
using Core.Repositories;
using NUnit.Framework;
using Tests.DataBuilders;

namespace Tests.Persistence;

public sealed class UserPortfolioStorageRepositoryIT : BaseIT<IUserPortfolioStorageRepository>
{
    private UserId _userId;
    
    [SetUp]
    public void SetupUser()
    {
        _userId = WithUserId();
    }
    
    [TearDown]
    public async Task TearDown()
    {
        var getRequest = DataBuilder.GetUserPortfolioRequest(_userId).Create();
        var portfolio = await Sut.GetUserPortfolioAsync(getRequest, CancellationToken);

        var setPortfolioStocks = portfolio.Stocks
            .Select(x => DataBuilder.SetPortfolioStock(x.StockId, quantity: 0).Create())
            .ToArray();
        
        var setRequest = DataBuilder.SetPortfolioRequest(_userId, setPortfolioStocks).Create();
        
        await Sut.SetPortfolioAsync(setRequest, CancellationToken);
    }
    
    [Test]
    public async Task GetPortfolioAsync_WithNoStocks_ReturnsEmptyStocks()
    {
        // Arrange
        var getRequest = DataBuilder.GetUserPortfolioRequest(_userId).Create();
        
        // Act
        var actual = await Sut.GetUserPortfolioAsync(getRequest, CancellationToken);

        // Assert
        Assert.That(actual.UserId, Is.EqualTo(_userId));
        Assert.That(actual.Stocks, Is.Empty);
    }
    
    [Test]
    public async Task SetPortfolioAsync_WithNoStocks_SavesNoStocks()
    {
        // Arrange
        var setRequest = DataBuilder.SetPortfolioRequest(_userId).Create();
        
        // Act
        await Sut.SetPortfolioAsync(setRequest, CancellationToken);
        
        // Assert
        var getRequest = DataBuilder.GetUserPortfolioRequest(_userId).Create();
        var actual = await Sut.GetUserPortfolioAsync(getRequest, CancellationToken);
        Assert.That(actual.UserId, Is.EqualTo(_userId));
        Assert.That(actual.Stocks, Is.Empty);
    }
    
    [Test]
    public async Task SetPortfolioAsync_WithSingleStock_SavesSingleStock()
    {
        // Arrange
        var stock = DataBuilder.SetPortfolioStock().Create();
        var setRequest = DataBuilder.SetPortfolioRequest(_userId, [stock]).Create();
        
        // Act
        await Sut.SetPortfolioAsync(setRequest, CancellationToken);
        
        // Assert
        var getRequest = DataBuilder.GetUserPortfolioRequest(_userId).Create();
        var actual = await Sut.GetUserPortfolioAsync(getRequest, CancellationToken);
        Assert.That(actual.UserId, Is.EqualTo(_userId));
        Assert.That(actual.Stocks, Has.Count.EqualTo(1));
        Assert.That(actual.Stocks.First().StockId, Is.EqualTo(stock.StockId));
        Assert.That(actual.Stocks.First().Quantity, Is.EqualTo(stock.Quantity));
    }
    
    [Test]
    public async Task SetPortfolioAsync_WithMultipleStocks_SavesMultipleStocks()
    {
        // Arrange
        var stocks = DataBuilder.SetPortfolioStock().CreateMany(10).ToArray();
        var setRequest = DataBuilder.SetPortfolioRequest(_userId, stocks).Create();
        
        // Act
        await Sut.SetPortfolioAsync(setRequest, CancellationToken);
        
        // Assert
        var getRequest = DataBuilder.GetUserPortfolioRequest(_userId).Create();
        var actual = await Sut.GetUserPortfolioAsync(getRequest, CancellationToken);
        Assert.That(actual.UserId, Is.EqualTo(_userId));
        Assert.That(actual.Stocks, Has.Count.EqualTo(10));
    }

    [Test]
    public async Task SetPortfolioAsync_WithZeroQuantity_IsDeleted()
    {
        // Arrange
        const int createCount = 999;
        const int deleteCount = 42;
        
        var stocks = DataBuilder.SetPortfolioStock().CreateMany(createCount).ToArray();
        var setRequest = DataBuilder.SetPortfolioRequest(_userId, stocks).Create();
        
        await Sut.SetPortfolioAsync(setRequest, CancellationToken);

        var zeroStocks = stocks[..deleteCount];
        zeroStocks = zeroStocks.Select(x => x with { Quantity = 0 }).ToArray();

        setRequest = DataBuilder.SetPortfolioRequest(_userId, zeroStocks).Create();

        // Act
        await Sut.SetPortfolioAsync(setRequest, CancellationToken);

        // Assert
        var getRequest = DataBuilder.GetUserPortfolioRequest(_userId).Create();
        var actual = await Sut.GetUserPortfolioAsync(getRequest, CancellationToken);
        Assert.That(actual.UserId, Is.EqualTo(_userId));
        Assert.That(actual.Stocks, Has.Count.EqualTo(createCount - deleteCount));
    }
}