using Core.Models.Asks;
using Core.Models.Ids;
using Core.Models.Portfolio;
using Core.Models.Prices;
using Core.Models.User;
using Core.Util;
using Microsoft.Extensions.Logging;

namespace Core.Services;

public sealed class AskResolutionService(
    ILogger<AskResolutionService> logger,
    IUserPortfolioService userPortfolioService,
    IAskService askService,
    IUserService userService) : IAskResolutionService
{
    public async Task ResolveAsksForStockAsync(GetStockPriceResponse stockPrice, CancellationToken cancellationToken)
    {
        var getFulfilledAsksRequest = new GetAsksRequest { StockId = stockPrice.StockId, MaxPrice = stockPrice.Price };
        var fulfilledAsks = await askService.GetAsksAsync(getFulfilledAsksRequest, cancellationToken);
        
        var fulfilledAsksForUsers = fulfilledAsks.Asks.GroupBy(x => x.UserId);
        
        foreach (var fulfilledAsksForUser in fulfilledAsksForUsers)
        {
            await ResolveUserFulfilledAsksAsync(fulfilledAsksForUser, stockPrice, cancellationToken);
        }
    }

    private async Task ResolveUserFulfilledAsksAsync(IGrouping<UserId, Ask> asksForUser, GetStockPriceResponse stockPrice, CancellationToken cancellationToken)
    {
        var (userId, asks) = (asksForUser.Key, asksForUser.ToArray());

        var stocksToRemove = asks.Select(x => new RemoveStockFromPortfolio { Quantity = x.Amount, StockId = x.StockId }).ToArray();
        
        var portfolioRequest = new RemoveFromPortfolioRequest { UserId = userId, Stocks = stocksToRemove };
        var portfolioResult =  await userPortfolioService.RemoveFromPortfolioAsync(portfolioRequest, cancellationToken);

        if (portfolioResult.IsT1)
        {
            logger.LogError("Failed to remove stocks from portfolio for user {UserId}", userId);
            return;
        }
        
        var gain = asks.Select(x => x.Amount * stockPrice.Price).Sum();

        var userBalanceChange = new UserBalance(gain.Value);
        
        var modifyBalanceRequest = new ModifyUserBalanceRequest { UserId = userId, Change = userBalanceChange };
        var userUpdateResult = await userService.ModifyUserBalanceAsync(modifyBalanceRequest, cancellationToken);
        
        if (userUpdateResult.IsT1)
        {
            logger.LogError("Failed to update user balance for user {UserId} with positive change {Gain}", userId, gain.Value);
            throw new Exception($"Failed to update user balance for user {userId} with positive change {gain.Value}");
        }
        
        var removeAsksRequest = new DeleteAsksRequest { AskIds = asks.Select(x => x.AskId).ToArray() };
        await askService.DeleteAsksAsync(removeAsksRequest, cancellationToken);
        
        logger.LogInformation("User {UserId} sold {Stocks} for {Gain}", userId, stocksToRemove, gain.Value);
    }
}