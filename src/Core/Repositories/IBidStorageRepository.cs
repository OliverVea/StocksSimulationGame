using Core.Models.Bids;

namespace Core.Repositories;

public interface IBidStorageRepository
{
    Task CreateBidAsync(Bid bid, CancellationToken cancellationToken);
    Task<GetBidsResponse> GetBidsAsync(GetBidsRequest request, CancellationToken cancellationToken);
    Task DeleteBidsAsync(DeleteBidsRequest request, CancellationToken cancellationToken);
}