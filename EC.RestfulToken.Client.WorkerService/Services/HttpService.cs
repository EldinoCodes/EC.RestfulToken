using System.Net.Http.Headers;

namespace EC.RestfulToken.Client.WorkerService.Services;

public interface IHttpService
{
    Task<string?> ExecuteAsync(string? method, string? uri, HttpContent? httpContent = null, Action<HttpRequestHeaders>? configureHttpHeaders = null, bool throwException = false, CancellationToken cancellationToken = default);
}

internal class HttpService(HttpClient httpClient) : IHttpService
{
    protected readonly HttpClient _httpClient = httpClient;

    public virtual async Task<string?> ExecuteAsync(string? method, string? uri, HttpContent? httpContent = null, Action<HttpRequestHeaders>? configureHttpHeaders = null, bool throwException = false, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(method)) return default;
        if (string.IsNullOrEmpty(uri)) return default;

        _httpClient.DefaultRequestHeaders.Clear();
        configureHttpHeaders?.Invoke(_httpClient.DefaultRequestHeaders);

        var response = method.ToLower() switch
        {
            "post" => await _httpClient.PostAsync(uri, httpContent, cancellationToken),
            "get" => await _httpClient.GetAsync(uri, cancellationToken),
            _ => throw new NotImplementedException()
        };
        var result = await response.Content.ReadAsStringAsync(cancellationToken);

        if (throwException && !response.IsSuccessStatusCode)
            throw new Exception(result);

        return result;
    }
}