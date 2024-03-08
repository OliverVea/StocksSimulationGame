namespace Core.Util;

public class UriQueryBuilderValueSetter(
    string baseUrl,
    IReadOnlyCollection<string> segments,
    IDictionary<string, string> queryParameters,
    string currentParameter)
{
    private string BaseUrl { get; } = baseUrl;
    private IReadOnlyCollection<string> Segments { get; } = segments;
    private IDictionary<string, string> QueryParameters { get; } = queryParameters;
    private string CurrentParameter { get; } = currentParameter;


    public static UriQueryBuilder operator-(UriQueryBuilderValueSetter setter, string value)
    {
        setter.QueryParameters.Add(setter.CurrentParameter, value);
        
        return new UriQueryBuilder(setter.BaseUrl, setter.Segments, setter.QueryParameters);
    }
}