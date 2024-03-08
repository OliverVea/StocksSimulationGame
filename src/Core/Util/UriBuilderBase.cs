namespace Core.Util;

public abstract class UriBuilderBase
{
    public static implicit operator Uri(UriBuilderBase builder) => builder.Build();
    protected abstract Uri Build();
}