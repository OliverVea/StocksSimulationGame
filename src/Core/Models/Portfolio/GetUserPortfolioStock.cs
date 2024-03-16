﻿using Core.Models.Ids;

namespace Core.Models.Portfolio;

public sealed record GetUserPortfolioStock
{
    public required StockId StockId { get; init; }
    public required int Quantity { get; init; }
}