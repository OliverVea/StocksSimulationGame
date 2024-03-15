using Core.Models.Ids;
using Core.Models.User;

namespace Core.Repositories;

public interface IUserStorageRepository
{
    Task<UserInformation?> GetUserAsync(UserId userId, CancellationToken cancellationToken);
    Task AddUserAsync(UserInformation user, CancellationToken cancellationToken);
}