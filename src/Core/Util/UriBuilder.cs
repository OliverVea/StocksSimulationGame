namespace Core.Util;

public class UriBuilder(string baseUri) : UriBuilderBase
{
    private string BaseUri { get; } = baseUri.EndsWith('/') ? baseUri[..^1] : baseUri;
    private List<string> Segments { get; } = new();


    public static UriBuilder operator/(UriBuilder builder, string segment)
    {
        builder.Segments.Add(segment);
        
        return builder;
    }
    
    public static UriQueryBuilderValueSetter operator&(UriBuilder builder, string query)
    {
        return new UriQueryBuilderValueSetter(builder.BaseUri, builder.Segments, new Dictionary<string, string>(StringComparer.Ordinal), query);
    }

    protected override Uri Build()
    {
        var uri = Segments.Aggregate(BaseUri, (current, segment) => current + $"/{segment}");

        return new Uri(uri);
    }
}