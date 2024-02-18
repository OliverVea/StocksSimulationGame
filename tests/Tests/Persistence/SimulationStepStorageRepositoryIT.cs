using AutoFixture;
using Core.Models;
using Core.Repositories;
using NUnit.Framework;

namespace Tests.Persistence;

public class SimulationStepStorageRepositoryIT : BaseIT
{
    [Test]
    public async Task GetCurrentSimulationStepAsync_WhenNoStockPricesAreAvailable_ReturnsZeros()
    {
        // Act
        var result = await GetService<ISimulationStepStorageRepository>().GetCurrentSimulationStepAsync(CancellationToken.None);

        // Assert
        Assert.That(result.Step, Is.Zero);
    }
    
    [Test]
    public async Task GetCurrentSimulationStepAsync_WhenStockPricesAreAvailable_ReturnsMaxStep()
    {
        // Arrange
        var simulationStep = new SimulationStep(2);
        var setStockPrice = DataBuilder.SetStockPriceRequest().With(x => x.SimulationStep, simulationStep).Create();
        var setStockPricesRequest = DataBuilder.SetStockPricesRequest().With(x => x.StockPrices, [setStockPrice]).Create();
        await GetService<IStockPriceStorageRepository>().SetStockPricesAsync(setStockPricesRequest, CancellationToken.None);

        // Act
        var result = await GetService<ISimulationStepStorageRepository>().GetCurrentSimulationStepAsync(CancellationToken.None);

        // Assert
        Assert.That(result, Is.EqualTo(simulationStep));
    }
}