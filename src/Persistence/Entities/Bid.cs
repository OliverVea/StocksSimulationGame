using System.ComponentModel.DataAnnotations;

namespace Persistence.Entities;

internal sealed class Bid : BaseEntity
{
    [MaxLength(PersistenceConstants.MaxUserIdLength)]
    public required string UserId { get; init; }
    
    public required Guid StockId { get; init; }
        
    public required Guid BidId { get; init; }
    
    public required int Amount { get; init; }
    
    public required float PricePerUnit { get; init; }
}