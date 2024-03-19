using System.ComponentModel.DataAnnotations;

namespace API.Models.Bid;

/// <summary>
/// Response model for the CreateBid endpoint.
/// </summary>
public sealed class CreateBidResponseModel
{
    /// <summary>
    /// The bid id.
    /// </summary>
    [Required]
    public Guid BidId { get; }
        
    /// <summary>
    /// The stock id of the bid.
    /// </summary>
    [Required]
    public Guid StockId { get; }
        
    /// <summary>
    /// The amount of stocks in the bid.
    /// </summary>
    [Range(1, int.MaxValue)]
    [Required]
    public int Amount { get; }
        
    /// <summary>
    /// The price per unit of the bid.
    /// </summary>
    [Range(0, float.MaxValue)]
    [Required]
    public float PricePerUnit { get; }
    
    internal CreateBidResponseModel(Core.Models.Bids.Bid bid)
    {
        BidId = bid.BidId.Id;
        StockId = bid.StockId.Id;
        Amount = bid.Amount;
        PricePerUnit = bid.PricePerUnit.Value;
    }

}