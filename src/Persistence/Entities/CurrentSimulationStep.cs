namespace Persistence.Entities;

public sealed class CurrentSimulationStep : BaseEntity
{
    public required long SimulationStep { get; set; }
}