using Core.Models.Tickers;
using Core.Repositories;

namespace Core.Services;

public sealed class TickerService(ITickerStorageRepository tickerStorageRepository) : ITickerService
{
    public Task<ListTickersResponse> ListTickersAsync(ListTickersRequest request, CancellationToken cancellationToken)
    {
        return tickerStorageRepository.ListTickersAsync(request, cancellationToken);
    }

    public Task<ListTickersResponse> ListTickersWithIdsAsync(ListTickersWithIdsRequest request, CancellationToken cancellationToken)
    {
        return tickerStorageRepository.ListTickersWithIdsAsync(request, cancellationToken);
    }

    public async Task<DeleteTickersResponse> DeleteTickersAsync(DeleteTickersRequest request, CancellationToken cancellationToken)
    {
        var existingRequest = new ListTickersWithIdsRequest { TickerIds = request.TickerIds };
        var existingTickers = await ListTickersWithIdsAsync(existingRequest, cancellationToken);

        var missing = request.TickerIds.Except(existingTickers.TickerIds).ToArray();

        var hasMissing = missing.Any();
        if (hasMissing && request.ErrorIfNotFound) throw new InvalidOperationException("Tickets with the following ids do not exist: " + string.Join(", ", missing));

        return await tickerStorageRepository.DeleteTickersAsync(request, cancellationToken);
    }

    public async Task<AddTickersResponse> AddTickersAsync(AddTickersRequest request, CancellationToken cancellationToken)
    {
        var existingRequest = new ListTickersWithIdsRequest { TickerIds = request.TickerIds };
        var existingTickers = await ListTickersWithIdsAsync(existingRequest, cancellationToken);

        var noDuplicates = !existingTickers.TickerIds.Any();
        if (noDuplicates) return await tickerStorageRepository.AddTickersAsync(request, cancellationToken);

        if (request.ErrorIfDuplicate) throw new InvalidOperationException("One or more tickers already exist");

        var newTickerIds = request.TickerIds.Except(existingTickers.TickerIds).ToArray();

        request = request with
        {
            TickerIds = newTickerIds
        };

        return await tickerStorageRepository.AddTickersAsync(request, cancellationToken);
    }
}