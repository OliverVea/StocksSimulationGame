using System.ComponentModel.DataAnnotations;
using Core.Models.Stocks;

namespace API.Models;

/// <summary>
/// Contains information about stocks that were added.
/// </summary>
public class AddStocksResponseModel
{
    /// <summary>
    /// The stocks that were added.
    /// </summary>
    [Required] public AddStockResponseModel[] Stocks { get; }

    /// <summary>
    /// Contains information about a stock that was added.
    /// </summary>
    public class AddStockResponseModel
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
        
        internal AddStockResponseModel(AddStockResponse response)
        {
            StockId = response.StockId.Id.ToString();
            Ticker = response.Ticker;
        }   
    }

    internal AddStocksResponseModel(AddStocksResponse addStocksResponse)
    {
        Stocks = addStocksResponse.Stocks.Select(stock => new AddStockResponseModel(stock)).ToArray();
    }
}