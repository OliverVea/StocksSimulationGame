using System.ComponentModel.DataAnnotations;

namespace Persistence.Entities;

internal class UserInformation : BaseEntity
{
    [MaxLength(PersistenceConstants.MaxUserIdLength)]
    public required string UserId { get; init; }
    
    public required decimal Balance { get; init; }
}