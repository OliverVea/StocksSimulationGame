using Core.Models.Stocks;

namespace Core.Services;

public interface IStockService
{
    Task<ListStocksResponse> ListStocksAsync(ListStocksRequest request, CancellationToken cancellationToken);
    Task<ListStocksResponse> ListStocksWithIdsAsync(ListStocksWithIdsRequest request, CancellationToken cancellationToken);
    Task<DeleteStocksResponse> DeleteStocksAsync(DeleteStocksRequest request, CancellationToken cancellationToken);
    Task<AddStocksResponse> AddStocksAsync(AddStocksRequest request, CancellationToken cancellationToken);
}