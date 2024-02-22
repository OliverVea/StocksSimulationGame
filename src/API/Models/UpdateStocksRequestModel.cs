using Core.Models.Ids;
using Core.Models.Stocks;

namespace API.Models;

/// <summary>
/// Updates t
/// </summary>
public sealed record UpdateStocksRequestModel
{
    /// <summary>
    /// The stocks to update
    /// </summary>
    public required IReadOnlyCollection<UpdateStockRequestModel> Stocks { get; init; }

    /// <summary>
    /// The stock id to update
    /// </summary>
    public sealed record UpdateStockRequestModel
    {
        /// <summary>
        /// The stock id to update
        /// </summary>
        public required string StockId { get; init; }
        
        /// <summary>
        /// The updated ticker. If null, the ticker will not be updated.
        /// </summary>
        public string? Ticker { get; init; }
        
        /// <summary>
        /// The updated volatility. If null, the volatility will not be updated.
        /// </summary>
        public float? Volatility { get; init; }
        
        /// <summary>
        /// The updated drift. If null, the drift will not be updated.
        /// </summary>
        public float? Drift { get; init; }
        
        internal UpdateStockRequest ToRequest()
        {
            return new UpdateStockRequest
            {
                StockId = new StockId(Guid.Parse(StockId)),
                Ticker = Ticker,
                Volatility = Volatility,
                Drift = Drift
            };
        }
    }
    
    internal UpdateStocksRequest ToRequest()
    {
        return new UpdateStocksRequest
        {
            Stocks = Stocks.Select(x => x.ToRequest()).ToHashSet(),
        };
    }
}