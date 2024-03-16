using Core.Models.Prices;
using Core.Repositories;

namespace Core.Services;

public sealed class StockPriceService(IStockPriceStorageRepository stockPriceStorageRepository) : IStockPriceService
{
    public Task<GetStockPriceInIntervalResponse> GetStockPriceInIntervalAsync(GetStockPriceInIntervalRequest request, CancellationToken cancellationToken)
    {
        return stockPriceStorageRepository.GetStockPriceInIntervalAsync(request, cancellationToken);
    }

    public Task<GetStockPricesForStepResponse> GetStockPricesForStepAsync(GetStockPricesForStepRequest request, CancellationToken cancellationToken)
    {
        return stockPriceStorageRepository.GetStockPricesForStepAsync(request, cancellationToken);
    }

    public Task SetStockPricesAsync(SetStockPricesRequest request, CancellationToken cancellationToken)
    {
        return stockPriceStorageRepository.SetStockPricesAsync(request, cancellationToken);
    }

    public async Task<DeleteStockPricesResponse> DeleteStockPricesAsync(DeleteStockPricesRequest request, CancellationToken cancellationToken)
    {
        await stockPriceStorageRepository.DeleteStockPricesAsync(request, cancellationToken);

        var deletedStockPrices = request.StockPrices.Select(x => new DeleteStockPriceResponse
        {
            StockId = x.StockId,
        }).ToArray();

        return new DeleteStockPricesResponse
        {
            DeletedStockPrices = deletedStockPrices,
        };
    }
}