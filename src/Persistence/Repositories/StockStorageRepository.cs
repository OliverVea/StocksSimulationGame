﻿using Core.Models.Ids;
using Core.Models.Stocks;
using Core.Repositories;
using Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

internal class StockStorageRepository(IDbContext dbContext) : IStockStorageRepository
{
    public async Task<ListStocksResponse> ListStocksAsync(ListStocksRequest request, CancellationToken cancellationToken)
    {
        var entities = await dbContext.Stocks.ToListAsync(cancellationToken);

        return new ListStocksResponse
        {
            Stocks = entities.Select(Map).ToArray(),
        };
    }

    public async Task<ListStocksResponse> ListStocksWithIdsAsync(ListStocksWithIdsRequest request, CancellationToken cancellationToken)
    {
        var stockGuids = request.StockIds.Select(x => x.Id);

        var entities = dbContext.Stocks.Where(x => stockGuids.Contains(x.StockId));
        var stocks = await entities.Select(x => Map(x)).ToListAsync(cancellationToken);

        return new ListStocksResponse
        {
            Stocks = stocks,
        };
    }

    public async Task DeleteStocksAsync(DeleteStocksRequest request, CancellationToken cancellationToken)
    {
        var stockGuids = request.StockIds.Select(x => x.Id).ToArray();

        var entities = dbContext.Stocks.Where(x => stockGuids.Contains(x.StockId));

        dbContext.Stocks.RemoveRange(entities);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task AddStocksAsync(AddStocksRequest request, CancellationToken cancellationToken)
    {
        var entities = request.Stocks.Select(Map);

        dbContext.Stocks.AddRange(entities);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
    
    
    private static ListStockResponse Map(Stock entity)
    {
        return new ListStockResponse
        {
            StockId = new StockId(entity.StockId),
            Ticker = entity.Ticker,
            Volatility = entity.Volatility,
            Drift = entity.Drift,
        };
    }
    
    private static Stock Map(AddStockRequest stock)
    {
        return new Stock
        {
            StockId = stock.StockId.Id,
            Ticker = stock.Ticker,
            Volatility = stock.Volatility,
            Drift = stock.Drift,
        };
    }
}