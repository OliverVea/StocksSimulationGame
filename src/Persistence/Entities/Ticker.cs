using System.ComponentModel.DataAnnotations;

namespace Persistence.Entities;

internal class Ticker
{
    [Key]
    public long Id { get; init; }

    [MaxLength(255)]
    public required string TickerId { get; init; }
}