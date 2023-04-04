namespace IRanwa.EOD.Chart.Business;

/// <summary>
/// EOD http client.
/// </summary>
public interface IEODHttpClient
{
    /// <summary>
    /// Gets the asynchronous.
    /// </summary>
    /// <param name="url">The URL.</param>
    /// <param name="paramsList">The parameters list.</param>
    /// <returns>Returns http response.</returns>
    Task<HttpResponseMessage> GetAsync(string url, Dictionary<string, string> paramsList);

    /// <summary>
    /// Posts the asynchronous.
    /// </summary>
    /// <param name="url">The URL.</param>
    /// <param name="paramsList">The parameters list.</param>
    /// <param name="content">The content.</param>
    /// <returns>Returns http response.</returns>
    Task<HttpResponseMessage> PostAsync(string url, Dictionary<string, string> paramsList, HttpContent content);
}
