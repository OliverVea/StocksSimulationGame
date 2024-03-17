using Core.Util;
using NUnit.Framework;

namespace Tests.Core.Util;

public class ColorHelperUT
{
    private const string ValidCharsString = "0123456789ABCDEF";
    public static string[] Chars => ValidCharsString.Select(x => x.ToString()).ToArray();
    
    [TestCase("#000000", 0, 0, 0, "#000000")]
    [TestCase("#FFFFFF", 255, 255, 255, "#FFFFFF")]
    [TestCase("#FFF", 255, 255, 255, "#FFFFFF")]
    [TestCase("#000", 0, 0, 0, "#000000")]
    [TestCase("#AAAAAA", 170, 170, 170, "#AAAAAA")]
    [TestCase("#AAA", 170, 170, 170, "#AAAAAA")]
    [TestCase("#010203", 1, 2, 3, "#010203")]
    public void FromHex_GivenValue_ReturnsCorrectColor(string hex, byte r, byte g, byte b, string outHex)
    {
        var color = ColorHelper.FromHex(hex);
        Assert.That(color.Red, Is.EqualTo(r));
        Assert.That(color.Green, Is.EqualTo(g));
        Assert.That(color.Blue, Is.EqualTo(b));
        
        var hexString = ColorHelper.ToHex(color);
        Assert.That(hexString, Is.EqualTo(outHex));
    }
    
    [Test]
    public void FromHex_WithInvalidHex_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => ColorHelper.FromHex("not a hex value"));
    }

    [TestCaseSource(nameof(Chars))]
    public void FromHex_ShortHex_ReturnsSameAsLongVariant(string s)
    {
        var shortHex = $"#{s}{s}{s}";
        var longHex = $"#{s}{s}{s}{s}{s}{s}";
        
        var shortHexColor = ColorHelper.FromHex(shortHex);
        var longHexColor = ColorHelper.FromHex(longHex);
        
        Assert.That(shortHexColor, Is.EqualTo(longHexColor));
    }
    
}