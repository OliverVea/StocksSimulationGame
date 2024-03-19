using Core.Models.Prices;

namespace Core.Services;

public interface IBidResolutionService
{
    Task ResolveBidsForStockAsync(GetStockPriceResponse stockPrice, CancellationToken cancellationToken);
}