namespace Core.Util;

public class UriQueryBuilder(
    string baseUri,
    IEnumerable<string> segments,
    IDictionary<string, string> queryParameters)
    : UriBuilderBase
{
    private string BaseUri { get; } = baseUri;
    private IEnumerable<string> Segments { get; } = segments;
    private IDictionary<string, string> QueryParameters { get; } = queryParameters;


    protected override Uri Build()
    {
        var uri = Segments.Aggregate(BaseUri, (current, segment) => current + $"/{segment}");
        var query = QueryParameters.Aggregate("", (current, parameter) => current + $"&{parameter.Key}={parameter.Value}");

        return new Uri(uri + "?" + query[1..]);
    }
}