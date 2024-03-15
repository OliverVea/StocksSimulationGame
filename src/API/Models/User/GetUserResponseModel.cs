namespace API.Models.User;

/// <summary>
/// Represents the response model for the GetUser endpoint.
/// </summary>
public sealed record GetUserResponseModel
{
    /// <summary>
    /// The user's identifier.
    /// </summary>
    public string Id { get; init; } = string.Empty;

    /// <summary>
    /// The user's balance.
    /// </summary>
    public decimal Balance { get; init; }
    
    internal GetUserResponseModel(Core.Models.User.UserInformation user) =>
        (Id, Balance) = (user.Id.Id, user.Balance.Value);
}