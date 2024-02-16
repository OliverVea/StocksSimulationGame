using Core.Models.Ids;
using Core.Models.Tickers;
using Core.Repositories;
using Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

internal class TickerStorageRepository(IDbContext dbContext) : ITickerStorageRepository
{
    public async Task<ListTickersResponse> ListTickersAsync(ListTickersRequest request, CancellationToken cancellationToken)
    {
        var entities = await dbContext.Tickers.ToListAsync(cancellationToken);

        return new ListTickersResponse
        {
            TickerIds = entities.Select(x => new TickerId(x.TickerId)).ToArray(),
        };
    }

    public async Task<ListTickersResponse> ListTickersWithIdsAsync(ListTickersWithIdsRequest request, CancellationToken cancellationToken)
    {
        var tickerIdStrings = request.TickerIds.Select(x => x.Ticker).ToArray();

        var entities = dbContext.Tickers.Where(x => tickerIdStrings.Contains(x.TickerId));
        var tickerIds = await entities.Select(x => new TickerId(x.TickerId)).ToListAsync(cancellationToken);

        return new ListTickersResponse
        {
            TickerIds = tickerIds,
        };
    }

    public async Task<DeleteTickersResponse> DeleteTickersAsync(DeleteTickersRequest request, CancellationToken cancellationToken)
    {
        var tickerIdStrings = request.TickerIds.Select(x => x.Ticker).ToArray();

        var entities = dbContext.Tickers.Where(x => tickerIdStrings.Contains(x.TickerId));
        var deletedTickerIds = await entities.Select(x => new TickerId(x.TickerId)).ToListAsync(cancellationToken: cancellationToken);

        dbContext.Tickers.RemoveRange(entities);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new DeleteTickersResponse
        {
            DeletedTickerIds = deletedTickerIds,
        };
    }

    public async Task<AddTickersResponse> AddTickersAsync(AddTickersRequest request, CancellationToken cancellationToken)
    {
        var entities = request.TickerIds.Select(x => new Ticker { TickerId = x.Ticker });

        dbContext.Tickers.AddRange(entities);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new AddTickersResponse
        {
            AddedTickerIds = request.TickerIds,
        };
    }
}