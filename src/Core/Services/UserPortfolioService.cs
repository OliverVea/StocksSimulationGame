﻿using Core.Models.Portfolio;
using Core.Repositories;
using Microsoft.Extensions.Logging;
using OneOf;
using OneOf.Types;

namespace Core.Services;

public sealed class UserPortfolioService(ILogger<UserPortfolioService> logger, IUserPortfolioStorageRepository repository) : IUserPortfolioService
{
    public async Task AddToPortfolioAsync(AddToPortfolioRequest request, CancellationToken cancellationToken)
    {
        var getRequest = new GetUserPortfolioRequest
        {
            UserId = request.UserId,
            StockIds = request.Stocks.Select(x => x.StockId).ToArray(),
        };

        var existingStocks = await GetUserPortfolioAsync(getRequest, cancellationToken);

        var setExisting = existingStocks.Stocks.Select(portfolioStock =>
        {
            var fromRequest = request.Stocks.Where(x => x.StockId == portfolioStock.StockId);
            var toAdd = fromRequest.Sum(x => x.Quantity);

            return new SetPortfolioStock
            {
                StockId = portfolioStock.StockId,
                Quantity = portfolioStock.Quantity + toAdd,
            };
        });
        
        var setNew = request.Stocks
            .Where(x => existingStocks.Stocks.All(y => y.StockId != x.StockId))
            .Select(x => new SetPortfolioStock
        {
            StockId = x.StockId,
            Quantity = x.Quantity,
        });
        
        var setRequest = new SetPortfolioRequest
        {
            UserId = request.UserId,
            Stocks = setExisting.Concat(setNew).ToArray(),
        };
        
        await repository.SetPortfolioAsync(setRequest, cancellationToken);
    }

    public async Task<OneOf<Success, Error>> RemoveFromPortfolioAsync(RemoveFromPortfolioRequest request, CancellationToken cancellationToken)
    {
        var getRequest = new GetUserPortfolioRequest
        {
            UserId = request.UserId,
            StockIds = request.Stocks.Select(x => x.StockId).ToArray(),
        };
        
        var existingStocks = await GetUserPortfolioAsync(getRequest, cancellationToken);
        
        if (existingStocks.Stocks.Any(x => x.Quantity < request.Stocks.First(y => y.StockId == x.StockId).Quantity))
        {
            logger.LogWarning("User {UserId} attempted to remove more stocks than they own", request.UserId);
            return new Error();
        }

        var setExisting = existingStocks.Stocks.Select(portfolioStock =>
        {
            var fromRequest = request.Stocks.Where(x => x.StockId == portfolioStock.StockId);
            var toRemove = fromRequest.Sum(x => x.Quantity);

            return new SetPortfolioStock
            {
                StockId = portfolioStock.StockId,
                Quantity = portfolioStock.Quantity - toRemove,
            };
        });
        
        var setRequest = new SetPortfolioRequest
        {
            UserId = request.UserId,
            Stocks = setExisting.ToArray(),
        };
        
        await repository.SetPortfolioAsync(setRequest, cancellationToken);
        
        return new Success();
    }

    public Task<GetUserPortfolioResponse> GetUserPortfolioAsync(GetUserPortfolioRequest request, CancellationToken cancellationToken)
    {
        return repository.GetUserPortfolioAsync(request, cancellationToken);
    }
}