using System.ComponentModel.DataAnnotations;
using Core.Models;
using Core.Models.Ids;
using Core.Models.Prices;
using Core.Models.Stocks;

namespace API.Models.Stocks;

/// <summary>
/// Creates new stocks with the given parameters.
/// </summary>
public sealed record AddStocksRequestModel
{
    /// <summary>
    /// The stocks to create.
    /// </summary>
    [Required] public required IReadOnlyCollection<AddStockRequestModel> Stocks { get; init; }
    
    /// <summary>
    /// If true, an error will be thrown if a stock with the same ticker already exists.
    /// Otherwise, the duplicate stock will be ignored.
    /// </summary>
    /// <example>true</example>
    [Required] public required bool ErrorIfDuplicate { get; init; } = false;

    /// <summary>
    /// Creates a single stock with the given parameters.
    /// </summary>
    public class AddStockRequestModel
    {
        /// <summary>
        /// The ticker of the stock. Must be unique.
        /// </summary>
        /// <example>GME</example>
        [Required] public required string Ticker { get; init; }
        
        /// <summary>
        /// The starting price of the stock.
        /// </summary>
        /// <example>100.0</example>
        [Required] public required float StartingPrice { get; init; }
        
        /// <summary>
        /// The volatility of the stock.
        /// </summary>
        /// <example>1.0</example>
        [Required] public required float Volatility { get; init; }
        
        /// <summary>
        /// The drift of the stock.
        /// </summary>
        /// <example>0.25</example>
        [Required] public required float Drift { get; init; }
        
        /// <summary>
        /// The color of the stock.
        /// </summary>
        /// <example>#007acc</example>
        [Required] public required string Color { get; init; }
    }
    
    internal AddStocksRequest ToRequests()
    {
        var stocks = Stocks.Select(x => new AddStockRequest
        {
            StockId = new StockId(Guid.NewGuid()),
            Ticker = x.Ticker,
            Drift = x.Drift,
            Volatility = x.Volatility,
            StartingPrice = new Price(x.StartingPrice),
            Color = Color.FromHex(x.Color)
        }).ToArray();
        
        return new AddStocksRequest
        {
            Stocks = stocks,
            ErrorIfDuplicate = ErrorIfDuplicate,
        };
    }
}