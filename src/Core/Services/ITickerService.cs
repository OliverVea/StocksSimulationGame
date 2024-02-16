using Core.Models.Tickers;

namespace Core.Services;

public interface ITickerService
{
    Task<ListTickersResponse> ListTickersAsync(ListTickersRequest request, CancellationToken cancellationToken);
    Task<ListTickersResponse> ListTickersWithIdsAsync(ListTickersWithIdsRequest request, CancellationToken cancellationToken);
    Task<DeleteTickersResponse> DeleteTickersAsync(DeleteTickersRequest request, CancellationToken cancellationToken);
    Task<AddTickersResponse> AddTickersAsync(AddTickersRequest request, CancellationToken cancellationToken);
}