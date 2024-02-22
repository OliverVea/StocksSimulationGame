using System.ComponentModel.DataAnnotations;

namespace Persistence.Entities;

internal class Stock
{
    [Key]
    public long Id { get; init; }

    public required Guid StockId { get; init; }
    [MaxLength(16)] public required string Ticker { get; set; }
    public required float Volatility { get; set; }
    public required float Drift { get; set; }
    public required float StartingPrice { get; init; }
}