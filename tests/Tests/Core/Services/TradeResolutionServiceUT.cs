using AutoFixture;
using Core.Models;
using Core.Models.Prices;
using Core.Services;
using NSubstitute;
using NUnit.Framework;
using Tests.DataBuilders;

namespace Tests.Core;

public class TradeResolutionServiceUT : BaseUT<ITradeResolutionService, TradeResolutionService>
{
    
    private ISimulationInformationService _simulationInformationService = null!;
    private IStockPriceService _stockPriceService = null!;
    private IAskResolutionService _askResolutionService = null!;

    private const int StockPriceCount = 3;
    private static readonly SimulationStep DefaultSimulationStep = DataBuilder.SimulationStep().Create();
    private static readonly GetStockPricesForStepResponse DefaultStockPrices = DataBuilder.GetStockPricesForStepResponse(
        stockPriceCount: StockPriceCount,
        simulationStep: DefaultSimulationStep).Create();
    
    [SetUp]
    public void Setup()
    {
        _simulationInformationService = SutBuilder.AddSubstitute<ISimulationInformationService>();
        _stockPriceService = SutBuilder.AddSubstitute<IStockPriceService>();
        _askResolutionService = SutBuilder.AddSubstitute<IAskResolutionService>();
    }


    [Test]
    public async Task ResolveTradesAsync_WhenCalledWithStocks_ShouldResolveAsksForStocks()
    {
        // Arrange
        SetupMocks();

        // Act
        await Sut.ResolveTradesAsync(CancellationToken);

        // Assert
        foreach (var stockPrice in DefaultStockPrices.StockPrices)
        {
            await _askResolutionService.Received(1).ResolveAsksForStockAsync(stockPrice, CancellationToken);
        }
    }
    
    [Test]
    public async Task ResolveTradesAsync_WhenCalledWithStocks_ShouldGetStockPricesForCurrentStep()
    {
        // Arrange
        SetupMocks();

        // Act
        await Sut.ResolveTradesAsync(CancellationToken);

        // Assert
        await _stockPriceService.Received(1).GetStockPricesForStepAsync(
            Arg.Is<GetStockPricesForStepRequest>(x => x.SimulationStep == DefaultSimulationStep),
            CancellationToken);
    }
    
    private void SetupMocks()
    {
        MockCurrentSimulation();
        MockStockPrices();
    }

    private void MockStockPrices()
    {
        _stockPriceService.GetStockPricesForStepAsync(
                Arg.Any<GetStockPricesForStepRequest>(),
                CancellationToken)
            .Returns(DefaultStockPrices);
    }

    private void MockCurrentSimulation()
    {
        _simulationInformationService.GetCurrentSimulationStepAsync(CancellationToken)
            .Returns(DefaultSimulationStep);
    }
}