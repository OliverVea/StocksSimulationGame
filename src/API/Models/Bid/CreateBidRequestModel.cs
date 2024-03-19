using System.ComponentModel.DataAnnotations;
using Core.Models.Bids;
using Core.Models.Ids;
using Core.Models.Prices;

namespace API.Models.Bid;

/// <summary>
/// Response model for the CreateBid endpoint.
/// </summary>
public sealed class CreateBidRequestModel
{
    /// <summary>
    /// The stock id of the bid.
    /// </summary>
    [Required]
    public Guid StockId { get; init; }
        
    /// <summary>
    /// The amount of stocks in the bid.
    /// </summary>
    [Range(1, int.MaxValue)]
    [Required]
    public int Amount { get; init; }
        
    /// <summary>
    /// The price per unit of the bid.
    /// </summary>
    [Range(0, float.MaxValue)]
    [Required]
    public float PricePerUnit { get; init; }


    internal CreateBidRequest ToRequest(UserId userId)
    {
        return new CreateBidRequest
        {
            StockId = new StockId(StockId),
            UserId = userId,
            Amount = Amount,
            PricePerUnit = new Price(PricePerUnit)
        };
    }
}