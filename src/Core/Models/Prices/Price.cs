namespace Core.Models.Prices;

public readonly record struct Price(float Value) : IComparable<Price>
{

    public int CompareTo(Price other)
    {
        return Value.CompareTo(other.Value);
    }
    
    public static Price operator*(Price price, int multiplier) => new(price.Value * multiplier);
    public static Price operator*(int multiplier, Price price) => new(price.Value * multiplier);
    public static Price operator+(Price price1, Price price2) => new(price1.Value + price2.Value);
    public static Price operator-(Price price1, Price price2) => new(price1.Value - price2.Value);
}