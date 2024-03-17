using Core.Models.Asks;
using Core.Models.Ids;
using Core.Models.Prices;

namespace Core.Services;

public interface IAskService
{
    Task<Ask> CreateAskAsync(CreateAskRequest request, CancellationToken cancellationToken);
    Task<GetAsksResponse> GetAsksAsync(GetAsksRequest request, CancellationToken cancellationToken);
    Task DeleteAsksAsync(DeleteAsksRequest request, CancellationToken cancellationToken);
}