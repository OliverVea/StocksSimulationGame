using Core.Models.User;
using Core.Repositories;
using Microsoft.Extensions.Logging;
using OneOf;
using OneOf.Types;

namespace Core.Services;

public class UserService(ILogger<UserService> logger, IUserIdService userIdService, IUserStorageRepository userStorageRepository) : IUserService
{
    public Task<UserInformation?> GetUserAsync(CancellationToken cancellationToken)
    {
        if (userIdService.UserId is not { } userId) return Task.FromResult<UserInformation?>(null);
        
        return userStorageRepository.GetUserAsync(userId, cancellationToken);
    }

    public async Task<OneOf<UserInformation, NotFound, AlreadyExists>> CreateUserAsync(CancellationToken cancellationToken)
    {
        if (userIdService.UserId is not { } userId)
        {
            logger.LogError("User ID not set");
            return new NotFound();
        }
        
        var existingUser = await userStorageRepository.GetUserAsync(userId, cancellationToken);

        if (existingUser is not null)
        {
            logger.LogError("User already exists. UserId: {UserId}", userId);
            return new AlreadyExists();
        }

        var user = new UserInformation
        {
            Balance = Constants.StartingBalance,
            Id = userId
        };
        
        await userStorageRepository.AddUserAsync(user, cancellationToken);
        
        logger.LogInformation("User created. UserId: {UserId}", userId);
        
        return user;
    }
}