using Core.Models.Ids;

namespace Core.Models.Stocks;

public record AddStockResponse
{
    public required StockId StockId { get; init; }
    public required string Ticker { get; init; }
};