using System.ComponentModel.DataAnnotations;

namespace Persistence.Entities;

internal class Stock
{
    [Key]
    public long Id { get; init; }

    public required Guid StockId { get; init; }
    
    [MaxLength(16)]
    public required string Ticker { get; init; }
    
    public required float Volatility { get; init; }
    public required float Drift { get; init; }
}