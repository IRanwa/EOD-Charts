namespace IRanwa.EOD.Chart.Business;

/// <summary>
/// EOD http client.
/// </summary>
/// <seealso cref="IEODHttpClient" />
public class EODHttpClient : IEODHttpClient
{
    /// <summary>
    /// The HTTP client
    /// </summary>
    private HttpClient httpClient = new();

    /// <summary>
    /// Gets or sets the API key.
    /// </summary>
    private string APIKey { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="EODHttpClient"/> class.
    /// </summary>
    public EODHttpClient() {
        httpClient.BaseAddress = new Uri("https://eodhistoricaldata.com");
        APIKey = "63caea0aebcd11.80376136";
        httpClient.Timeout = TimeSpan.FromMinutes(3);
    }

    /// <summary>
    /// Gets the asynchronous.
    /// </summary>
    /// <param name="url">The URL.</param>
    /// <param name="paramsList">The parameters list.</param>
    /// <returns>
    /// Returns http response.
    /// </returns>
    public async Task<HttpResponseMessage> GetAsync(string url, Dictionary<string, string> paramsList)
    {
        var fullURL = url;
        var parameters = string.Empty;
        if (paramsList != null)
        {
            foreach (var param in paramsList)
                if(!string.IsNullOrEmpty(param.Value))
                    parameters += $"{param.Key}={param.Value}&";
        }
        if (parameters != string.Empty)
            fullURL += $"?{parameters}api_token={APIKey}&fmt=json";
        else
            fullURL += $"?api_token={APIKey}&fmt=json";
        return await httpClient.GetAsync(fullURL);
    }

    /// <summary>
    /// Posts the asynchronous.
    /// </summary>
    /// <param name="url">The URL.</param>
    /// <param name="paramsList">The parameters list.</param>
    /// <param name="content">The content.</param>
    /// <returns>
    /// Returns http response.
    /// </returns>
    public async Task<HttpResponseMessage> PostAsync(string url, Dictionary<string, string> paramsList, HttpContent content)
    {
        var fullURL = url;
        var parameters = string.Empty;
        if (paramsList != null)
        {
            foreach (var param in paramsList)
                parameters += $"{param.Key}={param.Value}&";
        }
        if (parameters != string.Empty)
            fullURL += $"?{parameters}&api_token={APIKey}&fmt=json";
        else
            fullURL += $"?api_token={APIKey}&fmt=json";
        return await httpClient.PostAsync(fullURL, content);
    }
}
