using Core.Models.Asks;

namespace Core.Services;

public interface IAskService
{
    Task<CreateAskResponse> CreateAskAsync(CreateAskRequest request, CancellationToken cancellationToken);
    Task<GetAsksResponse> GetAsksAsync(GetAsksRequest request, CancellationToken cancellationToken);
    Task<DeleteAsksResponse> DeleteAsksAsync(DeleteAsksRequest request, CancellationToken cancellationToken);
}