using Core.Models.User;
using Core.Repositories;
using Microsoft.Extensions.Logging;
using OneOf;
using OneOf.Types;

namespace Core.Services;

public sealed class UserService(ILogger<UserService> logger, IUserIdService userIdService, IUserStorageRepository userStorageRepository) : IUserService
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
            UserId = userId
        };
        
        await userStorageRepository.AddUserAsync(user, cancellationToken);
        
        logger.LogInformation("User created. UserId: {UserId}", userId);
        
        return user;
    }

    public async Task<OneOf<Success, Error>> ModifyUserBalanceAsync(ModifyUserBalanceRequest request, CancellationToken cancellationToken)
    {
        var userInformation = await userStorageRepository.GetUserAsync(request.UserId, cancellationToken);
        if (userInformation is null)
        {
            logger.LogError("User not found. UserId: {UserId}", request.UserId);
            return new Error();
        }
        
        userInformation = userInformation with { Balance = userInformation.Balance + request.Change };
        
        if (userInformation.Balance < 0)
        {
            logger.LogError("User balance cannot be negative. UserId: {UserId}", request.UserId);
            return new Error();
        }
        
        await userStorageRepository.UpdateUserAsync(userInformation, cancellationToken);

        return new Success();
    }
}