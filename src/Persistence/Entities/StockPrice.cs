using System.ComponentModel.DataAnnotations;

namespace Persistence.Entities;

public class StockPrice
{
    [Key]
    public long Id { get; init; }
    
    public required Guid StockId { get; init; }
    public required long SimulationStep { get; init; }
    public required float Price { get; set; }
}