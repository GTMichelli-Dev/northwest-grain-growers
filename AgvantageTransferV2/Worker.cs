using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Agvantage_TransferV2;

public sealed class Worker(
    ILogger<Worker> logger,
    IServiceProvider services,           // <-- inject IServiceProvider
    IOptions<AppSettings> options) : BackgroundService
{
    private readonly AppSettings _cfg = options.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Worker starting at {Time}", DateTimeOffset.Now);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                logger.LogInformation("Starting a transfer cycle at {Time}", DateTimeOffset.Now);
                using var scope = services.CreateScope();
                var transfer = scope.ServiceProvider.GetRequiredService<AgvantageTransfer>();

                await transfer.StartTransferAsync(
                    batchFile: _cfg.BatchFile,
                    completedFilePath: _cfg.CompletedFilePath,
                    timeout: TimeSpan.FromSeconds(_cfg.TimeoutSeconds),
                    updateInterval: TimeSpan.FromMinutes(_cfg.UpdateIntervalMinutes),
                    ct: stoppingToken);

                logger.LogInformation("Worker completed a transfer cycle at {Time}", DateTimeOffset.Now);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Transfer run failed");
            }

            // Single delay governing cadence
            try
            {
                await Task.Delay(TimeSpan.FromMinutes(_cfg.UpdateIntervalMinutes), stoppingToken);
            }
            catch (OperationCanceledException) { break; }
        }

        logger.LogInformation("Worker stopping at {Time}", DateTimeOffset.Now);
    }

}

public sealed class AppSettings
{
    public string BatchFile { get; set; } = @"";          // filled from appsettings.json
    public string CompletedFilePath { get; set; } = @"";  // filled from appsettings.json
    public int TimeoutSeconds { get; set; } = 900;
    public int UpdateIntervalMinutes { get; set; } = 60;
}
