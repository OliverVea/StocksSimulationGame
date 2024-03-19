using Core.Models.Bids;
using Core.Models.Ids;
using Core.Models.Portfolio;
using Core.Models.Prices;
using Core.Models.User;
using Core.Util;
using Microsoft.Extensions.Logging;

namespace Core.Services;

public sealed class BidResolutionService(
    ILogger<BidResolutionService> logger,
    IUserPortfolioService userPortfolioService,
    IBidService bidService,
    IUserService userService) : IBidResolutionService
{
    public async Task ResolveBidsForStockAsync(GetStockPriceResponse stockPrice, CancellationToken cancellationToken)
    {
        var getFulfilledBidsRequest = new GetBidsRequest { StockId = stockPrice.StockId, MinPrice = stockPrice.Price };
        var fulfilledBids = await bidService.GetBidsAsync(getFulfilledBidsRequest, cancellationToken);
        
        var fulfilledBidsForUsers = fulfilledBids.Bids.GroupBy(x => x.UserId);
        
        foreach (var fulfilledBidsForUser in fulfilledBidsForUsers)
        {
            await ResolveUserFulfilledBidsAsync(fulfilledBidsForUser, stockPrice, cancellationToken);
        }
    }

    private async Task ResolveUserFulfilledBidsAsync(IGrouping<UserId, Bid> bidsForUser, GetStockPriceResponse stockPrice, CancellationToken cancellationToken)
    {
        var (userId, bids) = (bidsForUser.Key, bidsForUser.ToArray());
        
        var loss = bids.Select(x => x.Amount * stockPrice.Price).Sum();

        var userBalanceChange = new UserBalance(-loss.Value);
        
        var modifyBalanceRequest = new ModifyUserBalanceRequest { UserId = userId, Change = userBalanceChange };
        var userUpdateResult = await userService.ModifyUserBalanceAsync(modifyBalanceRequest, cancellationToken);
        
        if (userUpdateResult.IsT1)
        {
            logger.LogWarning("Failed to update user balance for user {UserId} with negative change {Loss}", userId, loss.Value);
            return;
        }
        
        var stocksToAdd = bids.Select(x => new AddStockToPortfolio { Quantity = x.Amount, StockId = x.StockId }).ToArray();
        
        var portfolioRequest = new AddToPortfolioRequest { UserId = userId, Stocks = stocksToAdd };
        await userPortfolioService.AddToPortfolioAsync(portfolioRequest, cancellationToken);
        
        var removeBidsRequest = new DeleteBidsRequest { BidIds = bids.Select(x => x.BidId).ToArray() };
        await bidService.DeleteBidsAsync(removeBidsRequest, cancellationToken);
        
        logger.LogInformation("User {UserId} bought {Stocks} for {Loss}", userId, stocksToAdd, loss.Value);
    }
}