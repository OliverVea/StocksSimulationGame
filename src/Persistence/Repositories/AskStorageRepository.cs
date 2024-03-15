using Core.Models.Asks;
using Core.Models.Ids;
using Core.Models.Prices;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

internal class AskStorageRepository(IDbContext dbContext) : IAskStorageRepository
{
    public Task CreateAskAsync(AskId askId, CreateAskRequest request, CancellationToken cancellationToken)
    {
        var entity = Map(askId, request);
        
        dbContext.Asks.Add(entity);
        
        return dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<GetAsksResponse> GetAsksAsync(GetAsksRequest request, CancellationToken cancellationToken)
    {
        var query = dbContext.Asks.Where(e => e.UserId == request.UserId.Id);
        
        if (request.AskIds is {} askIds) 
        {
            var askIdGuids = askIds.Select(id => id.Id);
            query = query.Where(e => askIdGuids.Contains(e.AskId));
        }
        
        var entities = await query.ToArrayAsync(cancellationToken);
        
        return Map(request.UserId, entities);
    }

    public Task DeleteAsksAsync(DeleteAsksRequest request, CancellationToken cancellationToken)
    {
        var askGuids = request.AskIds.Select(x => x.Id).ToArray();
        
        var entities = dbContext.Asks.Where(x => askGuids.Contains(x.AskId));
        
        dbContext.Asks.RemoveRange(entities);
        
        return dbContext.SaveChangesAsync(cancellationToken);
    }
    
    
    private static Entities.Ask Map(AskId askId, CreateAskRequest request)
    {
        return new Entities.Ask
        {
            AskId = askId.Id,
            UserId = request.UserId.Id,
            StockId = request.StockId.Id,
            Amount = request.Amount,
            PricePerUnit = request.PricePerUnit.Value
        };
    }

    private static GetAsksResponse Map(UserId userId, IEnumerable<Entities.Ask> entities)
    {
        return new GetAsksResponse
        {
            UserId = userId,
            Asks = entities.Select(Map).ToArray()
        };
    }

    private static GetAskResponse Map(Entities.Ask entity)
    {
        return new GetAskResponse
        {
            AskId = new AskId(entity.AskId),
            StockId = new StockId(entity.StockId),
            Amount = entity.Amount,
            PricePerUnit = new Price(entity.PricePerUnit)
        };
    }
}