using System.ComponentModel.DataAnnotations;

namespace Persistence.Entities;

public class CurrentSimulationStep
{
    [Key]
    public long Id { get; init; }
    
    public required long SimulationStep { get; set; }
}