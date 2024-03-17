using Core.Models.Ids;

namespace Core.Models.User;

public sealed record ModifyUserBalanceRequest
{
    public required UserId UserId { get; init; }
    public required UserBalance Change { get; init; }
};