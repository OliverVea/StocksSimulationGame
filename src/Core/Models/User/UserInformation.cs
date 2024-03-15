using Core.Models.Ids;

namespace Core.Models.User;

public sealed record UserInformation
{
    public required UserId Id { get; init; }
    public required UserBalance Balance { get; init; }
}