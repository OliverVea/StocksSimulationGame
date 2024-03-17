using AutoFixture;
using Core.Repositories;
using NUnit.Framework;
using Tests.DataBuilders;

namespace Tests.Persistence;

public sealed class SimulationStepStorageRepositoryIT : BaseIT<ISimulationStepStorageRepository>
{
    [Test]
    public async Task SetCurrentSimulationStepAsync_WhenCalledWithSimulationStep_ShouldSetCurrentSimulationStep()
    {
        // Arrange
        var simulationStep = DataBuilder.SimulationStep().Create();

        // Act
        await Sut.SetCurrentSimulationStepAsync(simulationStep, CancellationToken.None);
        var actual = await Sut.GetCurrentSimulationStepAsync(CancellationToken.None);

        // Assert
        Assert.That(actual, Is.EqualTo(simulationStep));
    }
    
    [Test]
    public async Task GetCurrentSimulationStepAsync_WhenCalled_ShouldGetSimulationStep()
    {
        // Arrange
        var simulationStep = DataBuilder.SimulationStep().Create();
        await Sut.SetCurrentSimulationStepAsync(simulationStep, CancellationToken.None);

        // Act
        var actual = await Sut.GetCurrentSimulationStepAsync(CancellationToken.None);

        // Assert
        Assert.That(actual, Is.EqualTo(simulationStep));
    }
    
    [Test]
    [Order(0)]
    public async Task GetCurrentSimulationStepAsync_WhenCalledWithoutSetting_ShouldGetNull()
    {
        // Act
        var actual = await Sut.GetCurrentSimulationStepAsync(CancellationToken.None);

        // Assert
        Assert.That(actual.Step, Is.Zero);
    }
}