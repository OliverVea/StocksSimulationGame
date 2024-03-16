using Core.Models.Ids;
using Core.Models.Portfolio;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

internal sealed class UserPortfolioStorageRepository(IDbContext dbContext) : IUserPortfolioStorageRepository
{
    public async Task SetPortfolioAsync(SetPortfolioRequest request, CancellationToken cancellationToken)
    {
        var requestStocksByStockId = request.Stocks.ToDictionary(x => x.StockId.Id);
        
        var requestStockIdGuids = request.Stocks.Select(x => x.StockId.Id).ToArray();
        var existing = await dbContext.UserPortfolioStocks
            .Where(x => x.UserId == request.UserId.Id && requestStockIdGuids.Contains(x.StockId))
            .ToArrayAsync(cancellationToken);
        
        var existingStockIds = existing.Select(x => x.StockId).ToHashSet();
        
        var toDelete = new List<Entities.UserPortfolioStock>();
        
        foreach (var stock in existing) 
        {
            var requestStock = requestStocksByStockId[stock.StockId];
            
            if (requestStock.Quantity == 0) toDelete.Add(stock);
            else stock.Quantity = requestStock.Quantity;
        }
        
        var toAdd = request.Stocks
            .Where(x => !existingStockIds.Contains(x.StockId.Id))
            .Select(x => Map(x, request.UserId))
            .ToArray();
        
        await dbContext.UserPortfolioStocks.AddRangeAsync(toAdd, cancellationToken);
        dbContext.UserPortfolioStocks.RemoveRange(toDelete);
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<GetUserPortfolioResponse> GetUserPortfolioAsync(GetUserPortfolioRequest request, CancellationToken cancellationToken)
    {
        var stocks = await dbContext.UserPortfolioStocks
            .Where(x => x.UserId == request.UserId.Id)
            .Select(x => Map(x))
            .ToArrayAsync(cancellationToken);
        
        return new GetUserPortfolioResponse
        {
            UserId = request.UserId,
            Stocks = stocks,
        };
    }
    
    private static Entities.UserPortfolioStock Map(SetPortfolioStock stock, UserId userId) => new()
    {
        UserId = userId.Id,
        StockId = stock.StockId.Id,
        Quantity = stock.Quantity,
    };
    
    private static GetUserPortfolioStock Map (Entities.UserPortfolioStock stock) => new()
    {
        StockId = new StockId(stock.StockId),
        Quantity = stock.Quantity,
    };
}