using System.ComponentModel.DataAnnotations;

namespace Persistence.Entities;

internal sealed class Stock : BaseEntity
{
    public required Guid StockId { get; init; }
    public required float StartingPrice { get; init; }
    [MaxLength(16)] public required string Ticker { get; set; }
    public required float Volatility { get; set; }
    public required float Drift { get; set; }
    public required byte Red { get; set; }
    public required byte Green { get; set; }
    public required byte Blue { get; set; }
}