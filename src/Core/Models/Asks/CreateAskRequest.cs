﻿using Core.Models.Ids;
using Core.Models.Prices;

namespace Core.Models.Asks;

public sealed record CreateAskRequest
{
    public required UserId UserId { get; init; }
    public required StockId StockId { get; init; }
    public int Amount { get; init; }
    public Price PricePerUnit { get; init; }
}