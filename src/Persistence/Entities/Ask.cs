using System.ComponentModel.DataAnnotations;

namespace Persistence.Entities;

internal sealed class Ask : BaseEntity
{
    [MaxLength(PersistenceConstants.MaxUserIdLength)]
    public required string UserId { get; init; }
    
    public required Guid StockId { get; init; }
        
    public required Guid AskId { get; init; }
    
    public required int Amount { get; init; }
    
    public required float PricePerUnit { get; init; }
}