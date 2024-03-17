using AutoFixture.Dsl;
using Core.Models;

namespace Tests.DataBuilders;

public static partial class DataBuilder
{
    public static IPostprocessComposer<SimulationStep> SimulationStep()
    {
        return Fixture.Build<SimulationStep>();
    }
    
    public static IPostprocessComposer<SimulationStep> SimulationStep(int step)
    {
        return SimulationStep().With(x => x.Step, step);
    }
}