using Core.Models;

namespace Caching;

internal sealed class SimulationStepCache
{
    public SimulationStep? CurrentSimulationStep { get; set; }
}