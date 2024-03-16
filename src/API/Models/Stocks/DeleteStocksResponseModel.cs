using System.ComponentModel.DataAnnotations;
using Core.Models.Stocks;

namespace API.Models.Stocks;

/// <summary>
/// Contains information about the deleted stocks.
/// </summary>
public sealed class DeleteStocksResponseModel
{
    /// <summary>
    /// The unique identifiers of the deleted stocks.
    /// </summary>
    [Required] public string[] Ids { get; }

    internal DeleteStocksResponseModel(DeleteStocksResponse response)
    {
        Ids = response.DeletedStockIds.Select(x => x.Id.ToString()).ToArray();
    }
}