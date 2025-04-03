using System.Text.Json;

namespace EC.RestfulToken.Client.WorkerService;

internal static class StringExtensions
{
    private static readonly JsonSerializerOptions jssOptions = new() { PropertyNameCaseInsensitive = true };
    internal static readonly string[] jsonElementStart = [ "{", "[" ];
    internal static readonly string[] jsonElementEnd = [ "}", "]" ];

    public static T? FromJson<T>(this string? json)
    {
        T? ret = default;
        if (string.IsNullOrEmpty(json)) return ret;

        json = json.Trim();
        if (
            jsonElementStart.Any(json.StartsWith) 
            && jsonElementEnd.Any(json.EndsWith)
        )
            ret = JsonSerializer.Deserialize<T?>(json, jssOptions);

        return ret;
    }
}
