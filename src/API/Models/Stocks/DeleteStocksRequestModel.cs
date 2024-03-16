using System.ComponentModel.DataAnnotations;
using Core.Models.Ids;
using Core.Models.Stocks;

namespace API.Models.Stocks;

/// <summary>
/// Represents a request to delete stocks.
/// </summary>
public sealed class DeleteStocksRequestModel
{
    /// <summary>
    /// The unique identifiers of the stocks to delete.
    /// </summary>
    [Required] public required string[] StockIds { get; init; }
    
    /// <summary>
    /// Whether to return an error if any of the stocks are missing.
    /// </summary>
    [Required] public required bool ErrorIfMissing { get; init; }

    internal DeleteStocksRequest ToRequest()
    {
        var guids = StockIds.Select(Guid.Parse);
        var stockIds = guids.Select(x => new StockId(x)).ToHashSet();
        
        return new DeleteStocksRequest
        {
            StockIds = stockIds,
            ErrorIfMissing = ErrorIfMissing,
        };
    }
}