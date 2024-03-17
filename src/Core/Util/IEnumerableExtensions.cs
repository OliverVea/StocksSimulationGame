using Core.Models.Prices;

namespace Core.Util;

public static class IEnumerableExtensions
{
    public static Price Sum(this IEnumerable<Price> prices)
    {
        return prices.Aggregate(new Price(0), (current, price) => current + price);
    }
}