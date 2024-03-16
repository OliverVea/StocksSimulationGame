using System.ComponentModel.DataAnnotations;
using Core.Models.Prices;
using Core.Models.Stocks;

namespace API.Models.Stocks;

/// <summary>
/// Contains a list of stocks.
/// </summary>
public sealed class SummarizeStocksResponseModel
{
    /// <summary>
    /// Contains summaries of a stock.
    /// </summary>
    [Required] public IReadOnlyCollection<SummarizeStockResponseModel> Stocks { get; }

    /// <summary>
    /// Contains summaries of a stock.
    /// </summary>
    public sealed class SummarizeStockResponseModel
    {
        /// <summary>
        /// The unique identifier of the stock.
        /// </summary>
        /// <example>{9c79fb22-b4c2-4076-bc74-450c460287ad}</example>
        [Required] public string StockId { get; }
        
        /// <summary>
        /// The ticker symbol of the stock.
        /// </summary>
        /// <example>GME</example>
        [Required] public string Ticker { get; }
        
        /// <summary>
        /// The price of the stock.
        /// </summary>
        /// <example>100</example>
        [Required] public float Price { get; }
        
        /// <summary>
        /// The color of the stock.
        /// </summary>
        /// <example>#007acc</example>
        [Required] public string Color { get; }
        
        internal SummarizeStockResponseModel((ListStockResponse, GetStockPriceResponse) stocksResponse)
        {
            var (response, priceResponse) = stocksResponse;
            StockId = response.StockId.Id.ToString();
            Ticker = response.Ticker;
            Price = priceResponse.Price.Value;
            Color = response.Color.ToHex();
        }
    }
    
    internal SummarizeStocksResponseModel(IEnumerable<(ListStockResponse, GetStockPriceResponse)> stocksResponse)
    {
        Stocks = stocksResponse.Select(x => new SummarizeStockResponseModel(x)).ToArray();
    }
}