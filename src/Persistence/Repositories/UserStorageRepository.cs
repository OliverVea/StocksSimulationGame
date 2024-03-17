using Core.Models.Ids;
using Core.Models.User;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

internal sealed class UserStorageRepository(IDbContext dbContext) : IUserStorageRepository
{
    public async Task<UserInformation?> GetUserAsync(UserId userId, CancellationToken cancellationToken)
    { 
        var entity = await dbContext.UserInformation.FirstOrDefaultAsync(x => x.UserId == userId.Id, cancellationToken: cancellationToken);
        return entity is null ? null : Map(entity);
    }

    public Task AddUserAsync(UserInformation user, CancellationToken cancellationToken)
    {
        var entity = Map(user);
        dbContext.UserInformation.Add(entity);
        return dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task UpdateUserAsync(UserInformation user, CancellationToken cancellationToken)
    {
        var entity = dbContext.UserInformation.First(x => x.UserId == user.UserId.Id);
        entity.Balance = user.Balance.Value;
        return dbContext.SaveChangesAsync(cancellationToken);
    }

    private static Entities.UserInformation Map(UserInformation entity)
    {
        return new Entities.UserInformation
        {
            Balance = entity.Balance.Value,
            UserId = entity.UserId.Id
        };
    }

    private static UserInformation Map(Entities.UserInformation entity)
    {
        return new UserInformation
        {
            Balance = new UserBalance(entity.Balance),
            UserId = new UserId(entity.UserId)
        };
    }
}