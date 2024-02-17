using Core.Models.Stocks;

namespace Core.Repositories;

public interface IStockStorageRepository
{
    Task<ListStocksResponse> ListStocksAsync(ListStocksRequest request, CancellationToken cancellationToken);
    Task<ListStocksResponse> ListStocksWithIdsAsync(ListStocksWithIdsRequest request, CancellationToken cancellationToken);
    Task DeleteStocksAsync(DeleteStocksRequest request, CancellationToken cancellationToken);
    Task AddStocksAsync(AddStocksRequest request, CancellationToken cancellationToken);
}