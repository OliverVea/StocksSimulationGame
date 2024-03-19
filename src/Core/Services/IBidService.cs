using Core.Models.Bids;

namespace Core.Services;

public interface IBidService
{
    Task<Bid> CreateBidAsync(CreateBidRequest request, CancellationToken cancellationToken);
    Task<GetBidsResponse> GetBidsAsync(GetBidsRequest request, CancellationToken cancellationToken);
    Task DeleteBidsAsync(DeleteBidsRequest request, CancellationToken cancellationToken);
}