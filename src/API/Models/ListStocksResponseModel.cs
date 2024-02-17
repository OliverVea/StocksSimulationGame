using Core.Models.Stocks;

namespace API.Models;

/// <summary>
/// Contains a list of stocks.
/// </summary>
public class ListStocksResponseModel
{
    /// <summary>
    /// Contains information about a stock.
    /// </summary>
    public IReadOnlyCollection<ListStockResponseModel> Stocks { get; }
    
    /// <summary>
    /// Contains information about a stock.
    /// </summary>
    public class ListStockResponseModel
    {
        /// <summary>
        /// The unique identifier of the stock.
        /// </summary>
        /// <example>{9c79fb22-b4c2-4076-bc74-450c460287ad}</example>
        public string StockId { get; }
        
        /// <summary>
        /// The ticker symbol of the stock.
        /// </summary>
        /// <example>GME</example>
        public string Ticker { get; }
        
        /// <summary>
        /// The volatility of the stock.
        /// </summary>
        /// <example>0.05</example>
        public float Volatility { get; }
        
        /// <summary>
        /// The drift of the stock.
        /// </summary>
        /// <example>0.01</example>
        public float Drift { get; init; }
        
        internal ListStockResponseModel(ListStockResponse response)
        {
            StockId = response.StockId.Id.ToString();
            Ticker = response.Ticker;
            Volatility = response.Volatility;
            Drift = response.Drift;
        }
    }

    internal ListStocksResponseModel(ListStocksResponse response)
    {
        Stocks = response.Stocks.Select(x => new ListStockResponseModel(x)).ToArray();
    }
}

