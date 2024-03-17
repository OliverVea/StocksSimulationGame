namespace Core.Models.User;

public readonly record struct UserBalance(float Value)
{
    public static UserBalance operator+(UserBalance balance1, UserBalance balance2) => new(balance1.Value + balance2.Value);
    public static bool operator<(UserBalance balance1, int value) => balance1.Value < value;

    public static bool operator >(UserBalance balance1, int value) => balance1.Value > value;
}