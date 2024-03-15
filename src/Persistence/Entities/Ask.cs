using System.ComponentModel.DataAnnotations;

namespace Persistence.Entities;

internal class Ask : BaseEntity
{
    [MaxLength(PersistenceConstants.MaxUserIdLength)]
    public required string UserId { get; init; }
    
    [MaxLength(PersistenceConstants.MaxStockIdLength)]
    public required Guid StockId { get; init; }
    
    [MaxLength(PersistenceConstants.MaxAskIdLength)]
    public required Guid AskId { get; init; }
    
    public required int Amount { get; init; }
    
    public required float PricePerUnit { get; init; }
}