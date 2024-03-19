using Core.Models.Asks;
using Core.Models.Ids;
using Core.Models.Prices;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

internal sealed class AskStorageRepository(IDbContext dbContext) : IAskStorageRepository
{
    public Task CreateAskAsync(Ask ask, CancellationToken cancellationToken)
    {
        var entity = Map(ask);
        
        dbContext.Asks.Add(entity);
        
        return dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<GetAsksResponse> GetAsksAsync(GetAsksRequest request, CancellationToken cancellationToken)
    {
        var query = dbContext.Asks.AsQueryable();
            
        if (request.UserId is {} userId) query = query.Where(e => e.UserId == userId.Id);
        
        if (request.AskIds is {} askIds) 
        {
            var askIdGuids = askIds.Select(id => id.Id);
            query = query.Where(e => askIdGuids.Contains(e.AskId));
        }
        
        if (request.StockId is {} stockId) query = query.Where(e => e.StockId == stockId.Id);
        
        if (request.MaxPrice is {} maxPrice) query = query.Where(e => e.PricePerUnit <= maxPrice.Value);
        
        return Map(await query.ToArrayAsync(cancellationToken));
    }

    public Task DeleteAsksAsync(DeleteAsksRequest request, CancellationToken cancellationToken)
    {
        var askGuids = request.AskIds.Select(x => x.Id).ToArray();
        
        var entities = dbContext.Asks.Where(x => askGuids.Contains(x.AskId));
        
        dbContext.Asks.RemoveRange(entities);
        
        return dbContext.SaveChangesAsync(cancellationToken);
    }
    
    
    private static Entities.Ask Map(Ask ask)
    {
        return new Entities.Ask
        {
            AskId = ask.AskId.Id,
            UserId = ask.UserId.Id,
            StockId = ask.StockId.Id,
            Amount = ask.Amount,
            PricePerUnit = ask.PricePerUnit.Value,
        };
    }

    private static GetAsksResponse Map(IEnumerable<Entities.Ask> entities)
    {
        return new GetAsksResponse
        {
            Asks = entities.Select(Map).ToArray(),
        };
    }

    private static Ask Map(Entities.Ask entity)
    {
        return new Ask
        {
            UserId = new UserId(entity.UserId),
            AskId = new AskId(entity.AskId),
            StockId = new StockId(entity.StockId),
            Amount = entity.Amount,
            PricePerUnit = new Price(entity.PricePerUnit)
        };
    }
}