using System.ComponentModel.DataAnnotations;

namespace Persistence.Entities;

public sealed class UserPortfolioStock : BaseEntity
{
    [MaxLength(PersistenceConstants.MaxUserIdLength)]
    public required string UserId { get; init; }
    public required Guid StockId { get; init; }
    
    [Range(1, int.MaxValue)]
    public required int Quantity { get; set; }
}