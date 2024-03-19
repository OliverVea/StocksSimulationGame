using Core.Models.Bids;
using Core.Models.Ids;
using Core.Repositories;
using Microsoft.Extensions.Logging;

namespace Core.Services;

public sealed class BidService(ILogger<BidService> logger, IBidStorageRepository storageRepository) : IBidService
{
    private static BidId NewBidId() => new(Guid.NewGuid());

    public async Task<Bid> CreateBidAsync(CreateBidRequest request, CancellationToken cancellationToken)
    {
        var bid = new Bid
        {
            BidId = NewBidId(),
            StockId = request.StockId,
            UserId = request.UserId,
            PricePerUnit = request.PricePerUnit,
            Amount = request.Amount,
        };
        
        await storageRepository.CreateBidAsync(bid, cancellationToken);

        return bid;
    }

    public Task<GetBidsResponse> GetBidsAsync(GetBidsRequest request, CancellationToken cancellationToken)
    {
        return storageRepository.GetBidsAsync(request, cancellationToken);
    }

    public async Task DeleteBidsAsync(DeleteBidsRequest request, CancellationToken cancellationToken)
    {
        var existingRequest = new GetBidsRequest { BidIds = request.BidIds };
        var existingBids = await storageRepository.GetBidsAsync(existingRequest, cancellationToken);
        
        var missing = request.BidIds.Except(existingBids.Bids.Select(a => a.BidId)).ToArray();
        
        if (missing.Any())
        {
            logger.LogWarning("Tried to delete Bids with the following ids {MissingBids}", missing);
        }

        request = new DeleteBidsRequest { BidIds = request.BidIds.Except(missing).ToArray() };
        
        await storageRepository.DeleteBidsAsync(request, cancellationToken);
    }
}