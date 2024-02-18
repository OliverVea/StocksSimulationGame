using AutoFixture.Dsl;
using Core.Models;

namespace Tests.DataBuilders;

public partial class DataBuilder
{
    public IPostprocessComposer<SimulationStep> SimulationStep()
    {
        return Fixture.Build<SimulationStep>();
    }
}