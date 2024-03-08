using System.ComponentModel.DataAnnotations;
using Core.Models.Prices;

namespace API.Models.PriceHistory;

/// <summary>
/// Represents the response to a request to get the price history of a stock.
/// </summary>
public sealed record GetPriceHistoryResponseModel
{
    /// <summary>
    /// The unique identifier of the stock.
    /// </summary>
    [Required] public string StockId { get; }
    
    /// <summary>
    /// The first simulation step of the data.
    /// Can be higher than the "From" parameter of the request.
    /// </summary>
    /// <example>0</example>
    [Required] public long From { get; }
    
    /// <summary>
    /// The last simulation step of the data.
    /// Can be lower than the "To" parameter of the request.
    /// </summary>
    /// <example>100</example>
    [Required] public long To { get; }
    
    /// <summary>
    /// The price history of the stock.
    /// </summary>
    [Required] public IReadOnlyCollection<PriceHistoryEntryModel> Prices { get; }
    
    /// <summary>
    /// Represents an entry in the price history of a stock.
    /// </summary>
    public class PriceHistoryEntryModel
    {
        /// <summary>
        /// The simulation step of the price.
        /// </summary>
        [Required] public long Step { get; }
        
        /// <summary>
        /// The price of the stock at the simulation step.
        /// </summary>
        [Required] public float Price { get; }
        
        internal PriceHistoryEntryModel(GetStockPriceResponse getStockPriceResponse)
        {
            Step = getStockPriceResponse.Step.Step;
            Price = getStockPriceResponse.Price.Value;
        }
    }

    internal GetPriceHistoryResponseModel(GetStockPriceInIntervalResponse response)
    {
        StockId = response.StockId.Id.ToString();
        From = response.StockPrices.Select(x => x.Step.Step).Min();
        To = response.StockPrices.Select(x => x.Step.Step).Max();
        Prices = response.StockPrices.Select(x => new PriceHistoryEntryModel(x)).ToArray();
    }
}
