using Core.Models.Prices;

namespace Core.Repositories;

public interface IStockPriceStorageRepository
{
    Task<GetStockPricesResponse> GetStockPricesAsync(GetStockPricesRequest request, CancellationToken cancellationToken);
    Task SetStockPricesAsync(SetStockPricesRequest request, CancellationToken cancellationToken);
    Task DeleteStockPricesAsync(DeleteStockPricesRequest request, CancellationToken cancellationToken);
}