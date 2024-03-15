using Core.Models.User;
using OneOf;
using OneOf.Types;

namespace Core.Services;

public interface IUserService
{
    Task<UserInformation?> GetUserAsync(CancellationToken cancellationToken);
    Task<OneOf<UserInformation, NotFound, AlreadyExists>> CreateUserAsync(CancellationToken cancellationToken);
}