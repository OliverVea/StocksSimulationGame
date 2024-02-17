﻿using Core.Models.Ids;

namespace Core.Models.Prices;

public record GetStockPricesRequest
{
    public required StockId StockId { get; init; }
    public required SimulationStep From { get; init; }
    public required SimulationStep To { get; init; }
}