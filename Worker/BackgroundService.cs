using Worker.DummyAbstractions;

namespace Worker;

public class BackgroundService : Microsoft.Extensions.Hosting.BackgroundService
{
    private readonly ILogger<BackgroundService> _logger;
    private readonly IDataService _dataService;
    private readonly IProcessingService _processingService;

    public BackgroundService(ILogger<BackgroundService> logger, IDataService dataService, IProcessingService processingService)
    {
        _logger = logger;
        _dataService = dataService;
        _processingService = processingService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            // Fetch data from the external API
            var data = await _dataService.FetchData();

            // Process the fetched data
            var result = await _processingService.ProcessData(data);

            // Log the result
            _logger.LogInformation("Processed data result: {result}", result);

            await Task.Delay(1000, stoppingToken);
        }
    }
}