namespace Core.Models.Prices;

public record GetStockPriceResponse
{
    public required SimulationStep Step { get; init; }
    public required Price Price { get; init; }
}