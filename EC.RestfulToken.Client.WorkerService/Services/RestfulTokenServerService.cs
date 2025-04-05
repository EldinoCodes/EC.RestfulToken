using EC.RestfulToken.Client.WorkerService.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Headers;

namespace EC.RestfulToken.Client.WorkerService.Services;

public interface IRestfulTokenServerService
{
    Task<List<TestContent>> TestContentGetAllAsync(CancellationToken cancellationToken = default);
}

internal class RestfulTokenServerService(ILogger<IRestfulTokenServerService> logger, IConfiguration configuration, IMemoryCache memoryCache, IHttpService httpService) : IRestfulTokenServerService
{
    private readonly ILogger<IRestfulTokenServerService> _logger = logger;
    private readonly IConfiguration _configuration = configuration;
    private readonly IMemoryCache _memoryCache = memoryCache;
    private readonly IHttpService _httpService = httpService;

    public async Task<List<TestContent>> TestContentGetAllAsync(CancellationToken cancellationToken = default)
    {
        var ret = new List<TestContent>();

        var token = await GetTokenAsync(cancellationToken);
        if (string.IsNullOrEmpty(token?.TokenType) || string.IsNullOrEmpty(token?.AccessToken)) return ret;

        var uri = _configuration.GetValue<string>("RestfulTokenServer:Endpoint");

        var res = await _httpService.ExecuteAsync(
            "GET",
            $"{uri}/TestContent",
            null,
            (h) => h.Authorization = new AuthenticationHeaderValue(token.TokenType, token.AccessToken),
            cancellationToken: cancellationToken
        );
        var data = res.FromJson<List<TestContent>>();
        if (data?.Count > 0)
            ret.AddRange(data);

        return ret;
    }

    /// <summary>
    /// Any method that needs to call to the api with a token can use this method to pull the current token from the cache, but if one doesnt exist it will attempt to get it.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>AuthToken</returns>
    protected virtual async Task<AuthToken?> GetTokenAsync(CancellationToken cancellationToken = default)
    {
        return await _memoryCache.GetOrCreateAsync("RestfulTokenServerToken", async (entry) =>
        {
            var uri = _configuration.GetValue<string>("RestfulTokenServer:Endpoint");
            ArgumentException.ThrowIfNullOrEmpty(uri, nameof(uri));

            var tenantId = _configuration.GetValue<string>("RestfulTokenServer:TenantId");
            ArgumentException.ThrowIfNullOrEmpty(tenantId, nameof(tenantId));

            var clientId = _configuration.GetValue<string>("RestfulTokenServer:ClientId");
            ArgumentException.ThrowIfNullOrEmpty(clientId, nameof(clientId));

            var clientSecret = _configuration.GetValue<string>("RestfulTokenServer:ClientSecret");
            ArgumentException.ThrowIfNullOrEmpty(clientSecret, nameof(clientSecret));

            var data = await _httpService.ExecuteAsync(
                "POST",
                $"{uri}/Token/{tenantId}/",
                new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"clientId", clientId },
                    {"clientSecret", clientSecret },
                }),
                cancellationToken: cancellationToken
            );

            var token = data.FromJson<AuthToken>();
            if (token is null) return default;

            var now = DateTime.Now;
            entry.Value = token;
            entry.AbsoluteExpiration = now.AddSeconds(token.ExpiresIn - 10);

            // add some logging to indicate when the token was acquired and when it expires.
            entry.PostEvictionCallbacks.Add(new PostEvictionCallbackRegistration()
            {
                EvictionCallback = (key, value, reason, state) =>
                {
                    _logger.LogWarning("RestfulTokenServerToken expired! aquired: {1} expired: {2} lifespan: {3} secs", [now, DateTime.Now, token.ExpiresIn]);
                }
            });
            _logger.LogWarning("RestfulTokenServerToken acquired at {1} lifespan: {2} secs", [now, token.ExpiresIn]);

            return token;
        });

    }
}
