using Core.Models.Prices;

namespace Core.Services;

public interface IStockPriceService
{
    Task<GetStockPriceInIntervalResponse> GetStockPriceInIntervalAsync(GetStockPriceInIntervalRequest request, CancellationToken cancellationToken);
    Task<GetStockPricesForStepResponse> GetStockPricesForStepAsync(GetStockPricesForStepRequest request, CancellationToken cancellationToken);
    Task SetStockPricesAsync(SetStockPricesRequest request, CancellationToken cancellationToken);
    Task<DeleteStockPricesResponse> DeleteStockPricesAsync(DeleteStockPricesRequest request, CancellationToken cancellationToken);
}