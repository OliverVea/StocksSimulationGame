using Core.Util;

namespace Core.Models;

public readonly record struct Color(byte Red, byte Green, byte Blue)
{
    public static Color FromHex(string hex) => ColorHelper.FromHex(hex);
    public string ToHex() => ColorHelper.ToHex(this);
    
    /// <summary>
    /// Returns the color as a hex string.
    /// </summary>
    /// <returns>The color as a hex string</returns>
    public override string ToString() => ToHex();
}