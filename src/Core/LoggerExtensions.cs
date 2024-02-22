using Microsoft.Extensions.Logging;

namespace Core;

public static partial class LoggerExtensions
{
    [LoggerMessage(LogLevel.Information, "Stepped stock prices, added {NewPricesCount} updated {UpdatedPricesCount}")]
    public static partial void SteppedStockPrices(this ILogger logger, int newPricesCount, int updatedPricesCount);
}