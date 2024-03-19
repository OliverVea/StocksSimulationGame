using Core.Models.Prices;
using Core.Util;
using NUnit.Framework;

namespace Tests.Core.Util;

public class IEnumerableExtensionsUT : BaseUT
{
    [Test]
    public void SumPrice_EmptyList_ReturnsZeroPrice()
    {
        // Arrange
        List<Price> prices = [];
        
        // Act
        var actual = prices.Sum();
        
        // Assert
        Assert.That(actual, Is.EqualTo(new Price(0)));
    }
    
    [Test]
    public void SumPrice_SinglePrice_ReturnsSamePrice()
    {
        // Arrange
        var price = new Price(10);
        List<Price> prices = [price];
        
        // Act
        var actual = prices.Sum();
        
        // Assert
        Assert.That(actual, Is.EqualTo(price));
    }
    
    [Test]
    public void SumPrice_MultiplePrices_ReturnsSumOfPrices()
    {
        // Arrange
        List<Price> prices =
        [
            new(10),
            new(20),
            new(30)
        ];
        
        // Act
        var actual = prices.Sum();
        
        // Assert
        Assert.That(actual, Is.EqualTo(new Price(60)));
    }
}