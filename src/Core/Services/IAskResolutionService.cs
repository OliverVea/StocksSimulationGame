using Core.Models.Prices;

namespace Core.Services;

public interface IAskResolutionService
{
    Task ResolveAsksForStockAsync(GetStockPriceResponse stockPrice, CancellationToken cancellationToken);
}