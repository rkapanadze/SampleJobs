using Worker.DummyAbstractions;

namespace Worker;

public class HostedService : IHostedService, IDisposable
{
    private readonly ILogger<HostedService> _logger;
    private readonly IDataService _dataService;
    private readonly IProcessingService _processingService;
    private Timer _timer;

    public HostedService(ILogger<HostedService> logger, IDataService dataService,
        IProcessingService processingService)
    {
        _logger = logger;
        _dataService = dataService;
        _processingService = processingService;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("HostedService Worker starting.");
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        return Task.CompletedTask;
    }

    private void DoWork(object state)
    {
        var data = _dataService.FetchData().Result;
        var processedData = _processingService.ProcessData(data).Result;
        _logger.LogInformation("HostedService Worker running at: {time} {processData}", DateTimeOffset.Now, processedData);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("HostedService Worker stopping.");
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}