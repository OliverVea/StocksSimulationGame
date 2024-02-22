using Core.Models.Stocks;
using Core.Repositories;

namespace Core.Services;

public sealed class StockService(IStockStorageRepository stockStorageRepository) : IStockService
{
    public Task<ListStocksResponse> ListStocksAsync(ListStocksRequest request, CancellationToken cancellationToken)
    {
        return stockStorageRepository.ListStocksAsync(request, cancellationToken);
    }

    public Task<ListStocksResponse> ListStocksWithIdsAsync(ListStocksWithIdsRequest request, CancellationToken cancellationToken)
    {
        return stockStorageRepository.ListStocksWithIdsAsync(request, cancellationToken);
    }

    public async Task<DeleteStocksResponse> DeleteStocksAsync(DeleteStocksRequest request, CancellationToken cancellationToken)
    {
        var existingRequest = new ListStocksWithIdsRequest { StockIds = request.StockIds };
        var existingStocks = await ListStocksWithIdsAsync(existingRequest, cancellationToken);

        var missing = request.StockIds.Except(existingStocks.Stocks.Select(x => x.StockId)).ToArray();

        var hasMissing = missing.Any();
        if (hasMissing && request.ErrorIfMissing) throw new InvalidOperationException("Tickets with the following ids do not exist: " + string.Join(", ", missing));

        request = request with
        {
            StockIds = request.StockIds.Except(missing).ToHashSet()
        };
        
        await stockStorageRepository.DeleteStocksAsync(request, cancellationToken);
        
        return new DeleteStocksResponse
        {
            DeletedStockIds = request.StockIds,
        };
    }

    public async Task<AddStocksResponse> AddStocksAsync(AddStocksRequest request, CancellationToken cancellationToken)
    {
        var stockIds = request.Stocks.Select(x => x.StockId).ToArray();
        var existingRequest = new ListStocksWithIdsRequest { StockIds = stockIds };
        var existingStocks = await ListStocksWithIdsAsync(existingRequest, cancellationToken);

        request = UpdateWithExisting(request, existingStocks);

        await stockStorageRepository.AddStocksAsync(request, cancellationToken);
        return Map(request);
    }

    public async Task<UpdateStocksResponse> UpdateStocksAsync(UpdateStocksRequest updateStocksRequest, CancellationToken cancellationToken)
    {
        var stockIds = updateStocksRequest.Stocks.Select(x => x.StockId).ToArray();
        var listStocksRequest = new ListStocksWithIdsRequest { StockIds = stockIds };
        var stocks = await ListStocksWithIdsAsync(listStocksRequest, cancellationToken);
        
        var missing = stockIds.Except(stocks.Stocks.Select(x => x.StockId)).ToArray();
        if (missing.Any()) throw new InvalidOperationException("Tickets with the following ids do not exist: " + string.Join(", ", missing));
        
        await stockStorageRepository.UpdateStocksAsync(updateStocksRequest, cancellationToken);

        return new UpdateStocksResponse();
    }

    private static AddStocksRequest UpdateWithExisting(AddStocksRequest request, ListStocksResponse existingStocks)
    {
        var noDuplicates = existingStocks.Stocks.Count == 0;
        if (noDuplicates) return request;
        
        var existingStockIds = existingStocks.Stocks.Select(x => x.StockId).ToArray();

        if (request.ErrorIfDuplicate) throw new InvalidOperationException("Tickets with the following ids already exist: " + string.Join(", ", existingStockIds));

        var newStockIds = request.Stocks.ExceptBy(existingStockIds, x => x.StockId).ToArray();

        request = request with
        {
            Stocks = newStockIds
        };

        return request;
    }

    private static AddStocksResponse Map(AddStocksRequest request)
    {
        var stocks = request.Stocks.Select(x => new AddStockResponse
        {
            StockId = x.StockId,
            Ticker = x.Ticker,
        }).ToArray();
        
        return new AddStocksResponse
        {
            Stocks = stocks,
        };
    }
}