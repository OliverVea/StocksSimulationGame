using Core.Models.Prices;

namespace Core.Services;

public interface IStockPriceService
{
    Task<GetStockPricesResponse> GetStockPricesAsync(GetStockPricesRequest request, CancellationToken cancellationToken);
    Task SetStockPricesAsync(SetStockPricesRequest request, CancellationToken cancellationToken);
    Task<DeleteStockPricesResponse> DeleteStockPricesAsync(DeleteStockPricesRequest request, CancellationToken cancellationToken);
}