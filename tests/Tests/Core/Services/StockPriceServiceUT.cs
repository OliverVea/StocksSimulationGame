using AutoFixture;
using Core.Models.Prices;
using Core.Repositories;
using Core.Services;
using NSubstitute;
using NUnit.Framework;
using Tests.DataBuilders;

namespace Tests.Core;

public sealed class StockPriceServiceUT : BaseUT<IStockPriceService, StockPriceService>
{
    private IStockPriceStorageRepository _stockPriceStorageRepositoryMock = null!;
    
    [SetUp]
    public void Setup()
    {
        _stockPriceStorageRepositoryMock = SutBuilder.AddSubstitute<IStockPriceStorageRepository>();
    }
    
    [Test]
    public async Task GetStockPricesAsync_WithNoStockPrices_ReturnsGetStockPricesResponse()
    {
        // Arrange
        var request = DataBuilder.GetStockPriceInIntervalRequest().Create();
        var response = DataBuilder.GetStockPricesResponse(stockPrices: []).Create();
        
        MockGetStockPricesAsync(response);
        
        // Act
        var actual = await Sut.GetStockPriceInIntervalAsync(request, CancellationToken.None);
        
        // Assert
        Assert.That(actual.StockPrices, Is.Empty);
    }
    
    [Test]
    public async Task GetStockPricesAsync_WithSingleStockPrice_ReturnsGetStockPricesResponse()
    {
        // Arrange
        var request = DataBuilder.GetStockPriceInIntervalRequest().Create();
        
        var stockPrice = DataBuilder.GetStockPriceResponse().Create();
        var response = DataBuilder.GetStockPricesResponse(stockPrices: [stockPrice]).Create();
        MockGetStockPricesAsync(response);
        
        // Act
        var actual = await Sut.GetStockPriceInIntervalAsync(request, CancellationToken.None);
        
        // Assert
        Assert.That(actual.StockPrices, Has.Count.EqualTo(1));
    }
    
    [Test]
    public async Task GetStockPricesAsync_WithMultipleStockPrices_ReturnsGetStockPricesResponse()
    {
        // Arrange
        var request = DataBuilder.GetStockPriceInIntervalRequest().Create();
        
        var stockPrices = DataBuilder.GetStockPriceResponse().CreateMany(10).ToArray();
        var response = DataBuilder.GetStockPricesResponse(stockPrices).Create();
        MockGetStockPricesAsync(response);
        
        // Act
        var actual = await Sut.GetStockPriceInIntervalAsync(request, CancellationToken.None);
        
        // Assert
        Assert.That(actual.StockPrices, Has.Count.EqualTo(10));
    }
    
    [Test]
    public async Task SetStockPricesAsync_WithSetStockPricesRequest_CallsSetStockPricesAsync()
    {
        // Arrange
        var request = DataBuilder.SetStockPricesRequest().Create();
        
        // Act
        await Sut.SetStockPricesAsync(request, CancellationToken.None);
        
        // Assert
        VerifySetStockPricesAsync(x => Equals(x.StockPrices, request.StockPrices), 1);
    }
    
    private void MockGetStockPricesAsync(GetStockPriceInIntervalResponse inIntervalResponse)
    {
        _stockPriceStorageRepositoryMock.GetStockPriceInIntervalAsync(
                Arg.Any<GetStockPriceInIntervalRequest>(),
                CancellationToken)
            .Returns(inIntervalResponse);
    }
    
    private void VerifySetStockPricesAsync(Func<SetStockPricesRequest, bool> match, int calls)
    {
        _stockPriceStorageRepositoryMock
            .Received(calls)
            .SetStockPricesAsync(
                Arg.Is<SetStockPricesRequest>(r => match(r)),
                CancellationToken);
    }
}