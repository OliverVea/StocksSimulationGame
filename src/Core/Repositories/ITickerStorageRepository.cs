using Core.Models.Tickers;

namespace Core.Repositories;

public interface ITickerStorageRepository
{
    Task<ListTickersResponse> ListTickersAsync(ListTickersRequest request, CancellationToken cancellationToken);
    Task<ListTickersResponse> ListTickersWithIdsAsync(ListTickersWithIdsRequest request, CancellationToken cancellationToken);
    Task<DeleteTickersResponse> DeleteTickersAsync(DeleteTickersRequest request, CancellationToken cancellationToken);
    Task<AddTickersResponse> AddTickersAsync(AddTickersRequest request, CancellationToken cancellationToken);
}