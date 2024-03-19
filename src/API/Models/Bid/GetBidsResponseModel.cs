using System.ComponentModel.DataAnnotations;
using Core.Models.Bids;

namespace API.Models.Bid;

/// <summary>
/// Response model for the GetBids endpoint.
/// </summary>
public sealed class GetBidsResponseModel
{
    /// <summary>
    /// The bids for the current user.
    /// </summary>
    public IReadOnlyCollection<GetBidResponseModel> Bids { get; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="GetBidsResponseModel"/> class.
    /// </summary>
    public sealed class GetBidResponseModel
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
        
        internal GetBidResponseModel(Core.Models.Bids.Bid bid)
        {
            BidId = bid.BidId.Id;
            StockId = bid.StockId.Id;
            Amount = bid.Amount;
            PricePerUnit = bid.PricePerUnit.Value;
        }
    }
    
    internal GetBidsResponseModel(GetBidsResponse bids)
    {
        Bids = bids.Bids.Select(bid => new GetBidResponseModel(bid)).ToList();
    }

}