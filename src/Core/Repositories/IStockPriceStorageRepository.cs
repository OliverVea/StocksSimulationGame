using Core.Models.Prices;

namespace Core.Repositories;

public interface IStockPriceStorageRepository
{
    Task<GetStockPriceInIntervalResponse> GetStockPriceInIntervalAsync(GetStockPriceInIntervalRequest request, CancellationToken cancellationToken);
    Task<GetStockPricesForStepResponse> GetStockPricesForStepAsync(GetStockPricesForStepRequest request, CancellationToken cancellationToken);
    Task SetStockPricesAsync(SetStockPricesRequest request, CancellationToken cancellationToken);
    Task DeleteStockPricesAsync(DeleteStockPricesRequest request, CancellationToken cancellationToken);
}