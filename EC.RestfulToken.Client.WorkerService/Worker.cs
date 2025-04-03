using EC.RestfulToken.Client.WorkerService.Services;
using Microsoft.Extensions.Caching.Memory;

namespace EC.RestfulToken.Client.WorkerService;

public class Worker(ILogger<Worker> logger, IRestfulTokenServerService restfulTokenServerService, IMemoryCache memoryCache) : BackgroundService
{
    private readonly ILogger<Worker> _logger = logger;
    private readonly IRestfulTokenServerService _restfulTokenServerService = restfulTokenServerService;
    private readonly IMemoryCache _memoryCache = memoryCache;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var res = await _restfulTokenServerService.TestContentGetAllAsync(stoppingToken);

            // should always get something unless the token is doa or the server is down
            _logger.LogWarning("TestContentGetAllAsync returned {1} record at {2}", [ res.Count, DateTime.Now ]);

            await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
        }
    }
}
