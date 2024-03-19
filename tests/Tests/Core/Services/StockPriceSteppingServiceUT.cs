using AutoFixture;
using Core.Models.Ids;
using Core.Models.Prices;
using Core.Models.Stocks;
using Core.Services;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using DataBuilder = Tests.DataBuilders.DataBuilder;

namespace Tests.Core.Services;

public class StockPriceSteppingServiceUT : BaseUT<IStockPriceSteppingService, StockPriceSteppingService>
{
    private ILogger<StockPriceSteppingService> _logger = null!;
    private IStockService _stockService = null!;
    private IStockPriceService _stockPriceService = null!;
    private IRandomService _randomService = null!;
    
    [SetUp]
    public void SetupSubstitutes()
    {
        _logger = SutBuilder.AddSubstitute<ILogger<StockPriceSteppingService>>();
        _stockService = SutBuilder.AddSubstitute<IStockService>();
        _stockPriceService = SutBuilder.AddSubstitute<IStockPriceService>();
        _randomService = SutBuilder.AddSubstitute<IRandomService>();
    }
    
    [Test]
    public async Task OnSimulationSteppedAsync_NoExistingStockPrices_StockPricesAreSet()
    {
        // Arrange
        var stockId = DataBuilder.StockId().Create();
        var simulationStep = DataBuilder.SimulationStep().Create();

        SetupMocks([stockId], []);
        
        // Act
        await Sut.OnSimulationSteppedAsync(simulationStep, CancellationToken);

        // Assert
        await _stockPriceService.Received(1).SetStockPricesAsync(Arg.Any<SetStockPricesRequest>(), CancellationToken);
    }

    [Test]
    public async Task OnSimulationSteppedAsync_ExistingStockPrices_StockPricesAreUpdated()
    {
        // Arrange
        var stockId = DataBuilder.StockId().Create();
        var simulationStep = DataBuilder.SimulationStep().Create();

        SetupMocks([stockId], [stockId]);
        
        // Act
        await Sut.OnSimulationSteppedAsync(simulationStep, CancellationToken);

        // Assert
        await _stockPriceService.Received(1).SetStockPricesAsync(Arg.Any<SetStockPricesRequest>(), CancellationToken);
    }

    private void SetupMocks(StockId[] existingStocks, StockId[] stockIdsWithPrices)
    {
        MockListStocksResponse(existingStocks);
        MockStockPriceServiceResponse(stockIdsWithPrices);
    }
    
    private void MockListStocksResponse(StockId[] stockIds)
    {
        var response = DataBuilder.ListStocksResponse(stockIds: stockIds).Create();
        
        _stockService.ListStocksAsync(Arg.Any<ListStocksRequest>(), CancellationToken)
            .Returns(response);
    }
    
    private void MockStockPriceServiceResponse(StockId[] stockIds)
    {
        var response = DataBuilder.GetStockPricesForStepResponse(stockIds: stockIds).Create();
        
        _stockPriceService.GetStockPricesForStepAsync(Arg.Any<GetStockPricesForStepRequest>(), CancellationToken)
            .Returns(response);
    }
}