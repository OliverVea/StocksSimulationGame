using AutoFixture.Dsl;
using Core.Models;

namespace Tests.DataBuilders;

public sealed partial class DataBuilder
{
    public IPostprocessComposer<SimulationStep> SimulationStep()
    {
        return Fixture.Build<SimulationStep>();
    }
    
    public IPostprocessComposer<SimulationStep> SimulationStep(int step)
    {
        return SimulationStep().With(x => x.Step, step);
    }
}