namespace Core.Services;

public interface ITradeResolutionService
{
    Task ResolveTradesAsync(CancellationToken cancellationToken);
}