﻿using Core.Models.Ids;
using Core.Models.Prices;

namespace Core.Models.Stocks;

public record ListStockResponse
{
    public required StockId StockId { get; init; }
    public required Color Color { get; init; }
    public required string Ticker { get; init; }
    public required float Volatility { get; init; }
    public required float Drift { get; init; }
    public required Price StartingPrice { get; init; }
};