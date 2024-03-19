using Core.Models.Bids;
using Core.Models.Ids;
using Core.Models.Prices;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

internal sealed class BidStorageRepository(IDbContext dbContext) : IBidStorageRepository
{
    public Task CreateBidAsync(Bid bid, CancellationToken cancellationToken)
    {
        var entity = Map(bid);
        
        dbContext.Bids.Add(entity);
        
        return dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<GetBidsResponse> GetBidsAsync(GetBidsRequest request, CancellationToken cancellationToken)
    {
        var query = dbContext.Bids.AsQueryable();
            
        if (request.UserId is {} userId) query = query.Where(e => e.UserId == userId.Id);
        
        if (request.BidIds is {} bidIds) 
        {
            var bidIdGuids = bidIds.Select(id => id.Id);
            query = query.Where(e => bidIdGuids.Contains(e.BidId));
        }
        
        if (request.StockId is {} stockId) query = query.Where(e => e.StockId == stockId.Id);
        
        if (request.MinPrice is {} minPrice) query = query.Where(e => e.PricePerUnit >= minPrice.Value);
        
        return Map(await query.ToArrayAsync(cancellationToken));
    }

    public Task DeleteBidsAsync(DeleteBidsRequest request, CancellationToken cancellationToken)
    {
        var bidGuids = request.BidIds.Select(x => x.Id).ToArray();
        
        var entities = dbContext.Bids.Where(x => bidGuids.Contains(x.BidId));
        
        dbContext.Bids.RemoveRange(entities);
        
        return dbContext.SaveChangesAsync(cancellationToken);
    }
    
    
    private static Entities.Bid Map(Bid bid)
    {
        return new Entities.Bid
        {
            BidId = bid.BidId.Id,
            UserId = bid.UserId.Id,
            StockId = bid.StockId.Id,
            Amount = bid.Amount,
            PricePerUnit = bid.PricePerUnit.Value,
        };
    }

    private static GetBidsResponse Map(IEnumerable<Entities.Bid> entities)
    {
        return new GetBidsResponse
        {
            Bids = entities.Select(Map).ToArray(),
        };
    }

    private static Bid Map(Entities.Bid entity)
    {
        return new Bid
        {
            UserId = new UserId(entity.UserId),
            BidId = new BidId(entity.BidId),
            StockId = new StockId(entity.StockId),
            Amount = entity.Amount,
            PricePerUnit = new Price(entity.PricePerUnit)
        };
    }
}