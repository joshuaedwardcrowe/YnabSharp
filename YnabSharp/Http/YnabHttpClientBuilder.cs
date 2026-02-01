using System.Net.Http.Headers;
using System.Text;

namespace YnabSharpa.Http;

public class YnabHttpClientBuilder
{
    private const string BaseUrl = "https://api.ynab.com/v1";
    private readonly IHttpClientFactory _httpClientFactory;
    private string? _bearerToken;

    public YnabHttpClientBuilder(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public YnabHttpClientBuilder WithBearerToken(string bearerToken)
    {
        _bearerToken = bearerToken;
        return this;
    }
    
    public HttpClient Build(string? parentPath = null, string? nextPath = null)
    {
        var httpClient = _httpClientFactory.CreateClient();

        var uriPath = BuilUriPath(parentPath, nextPath);

        httpClient.BaseAddress = new Uri(uriPath);

        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _bearerToken);
        
        return httpClient;
    }
    
    private string BuilUriPath(string? parentPath, string? nextPath)
    {
        var uriPathBuilder = new StringBuilder();
        
        uriPathBuilder
            .Append(BaseUrl)
            .Append('/')
            .Append(parentPath)
            .Append('/')
            .Append(nextPath)
            .Append('/');

        return uriPathBuilder.ToString();
    }
}