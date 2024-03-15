using System.ComponentModel.DataAnnotations;

namespace Persistence.Entities;

public abstract class BaseEntity
{
    [Key]
    public long Id { get; init; }
}