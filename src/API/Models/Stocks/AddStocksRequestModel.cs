using System.ComponentModel.DataAnnotations;
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
        /// <example>0.05</example>
        [Required] public required float Volatility { get; init; }
        
        /// <summary>
        /// The drift of the stock.
        /// </summary>
        /// <example>0.01</example>
        [Required] public required float Drift { get; init; }
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
        }).ToArray();
        
        return new AddStocksRequest
        {
            Stocks = stocks,
            ErrorIfDuplicate = ErrorIfDuplicate,
        };
    }
}