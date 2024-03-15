namespace Core.Util;

public static class ColorHelper
{
    public static Models.Color FromHex(string hex)
    {
        return hex.Length switch
        {
            4 => FromShortHex(hex),
            7 => FromLongHex(hex),
            _ => throw new ArgumentException("Invalid hex string", nameof(hex))
        };
    }

    private static Models.Color FromShortHex(string hex)
    {
        var r = byte.Parse(hex[1].ToString() + hex[1], System.Globalization.NumberStyles.HexNumber);
        var g = byte.Parse(hex[2].ToString() + hex[2], System.Globalization.NumberStyles.HexNumber);
        var b = byte.Parse(hex[3].ToString() + hex[3], System.Globalization.NumberStyles.HexNumber);
        
        return new Models.Color(r, g, b);
    }

    private static Models.Color FromLongHex(string hex)
    {
        var r = byte.Parse(hex.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
        var g = byte.Parse(hex.Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
        var b = byte.Parse(hex.Substring(5, 2), System.Globalization.NumberStyles.HexNumber);
        
        return new Models.Color(r, g, b);
    }
    
    public static string ToHex(Models.Color color)
    {
        return $"#{color.Red:X2}{color.Green:X2}{color.Blue:X2}";
    }
}