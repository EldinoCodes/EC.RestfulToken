using EC.RestfulToken.Client.WorkerService.Services;

namespace EC.RestfulToken.Client.WorkerService;

public class Worker(ILogger<Worker> logger, IConfiguration configuration, IRestfulTokenServerService restfulTokenServerService) : BackgroundService
{
    private readonly ILogger<Worker> _logger = logger;
    private readonly IConfiguration _configuration = configuration;
    private readonly IRestfulTokenServerService _restfulTokenServerService = restfulTokenServerService;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var intervalSeconds = _configuration.GetValue<int>("IntervalSeconds");
        if (intervalSeconds <= 0) throw new ArgumentException("IntervalSeconds must be greater than 0.");

        while (!stoppingToken.IsCancellationRequested)
        {
            var res = await _restfulTokenServerService.TestContentGetAllAsync(stoppingToken);

            // should always get something unless the token is doa or the server is down
            _logger.LogWarning("TestContentGetAllAsync returned {1} record at {2}", [ res.Count, DateTime.Now ]);

            await Task.Delay(TimeSpan.FromSeconds(intervalSeconds), stoppingToken);
        }
    }
}
