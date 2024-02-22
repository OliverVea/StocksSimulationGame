using AutoFixture;
using Core.Models.Prices;
using Core.Repositories;
using Core.Services;
using Moq;
using NUnit.Framework;

namespace Tests.Core;

public class StockPriceServiceUT : BaseUT<IStockPriceService, StockPriceService>
{
    private Mock<IStockPriceStorageRepository> _stockPriceStorageRepositoryMock = null!;
    
    [SetUp]
    public void Setup()
    {
        _stockPriceStorageRepositoryMock = SutBuilder.AddMock<IStockPriceStorageRepository>();
    }
    
    [Test]
    public async Task GetStockPricesAsync_WithNoStockPrices_ReturnsGetStockPricesResponse()
    {
        // Arrange
        var request = DataBuilder.GetStockPricesRequest().Create();
        var response = DataBuilder.GetStockPricesResponse().With(x => x.StockPrices, []).Create();
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
        var request = DataBuilder.GetStockPricesRequest().Create();
        
        var stockPrice = DataBuilder.GetStockPriceResponse().Create();
        var response = DataBuilder.GetStockPricesResponse([stockPrice]).Create();
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
        var request = DataBuilder.GetStockPricesRequest().Create();
        
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
        VerifySetStockPricesAsync(x => Equals(x.StockPrices, request.StockPrices), Times.Once);
    }
    
    private void MockGetStockPricesAsync(GetStockPriceInIntervalResponse inIntervalResponse)
    {
        _stockPriceStorageRepositoryMock.Setup(x => x.GetStockPriceInIntervalAsync(
                It.IsAny<GetStockPriceInIntervalRequest>(),
                CancellationToken))
            .ReturnsAsync(inIntervalResponse);
    }
    
    private void VerifySetStockPricesAsync(Func<SetStockPricesRequest, bool> match, Func<Times> times)
    {
        _stockPriceStorageRepositoryMock.Verify(x => x.SetStockPricesAsync(
            It.Is<SetStockPricesRequest>(r => match(r)), CancellationToken.None), times);
    }
}