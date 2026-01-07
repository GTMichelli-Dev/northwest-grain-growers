using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GrainManagement.As400Sync;

public sealed class As400SyncWorker : BackgroundService
{
    private readonly ILogger<As400SyncWorker> _log;
    private readonly As400Reader _as400;
    private readonly AccountsUpserter _upserter;
    private readonly As400SyncOptions _opt;

    public As400SyncWorker(
        ILogger<As400SyncWorker> log,
        As400Reader as400,
        AccountsUpserter upserter,
        IOptions<As400SyncOptions> opt)
    {
        _log = log;
        _as400 = as400;
        _upserter = upserter;
        _opt = opt.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var runStartedUtc = DateTime.UtcNow;

            try
            {
                _log.LogInformation("AS400 sync start at {Utc}", runStartedUtc);

                var syncRunId = Guid.NewGuid();
                int count = 0;

                await foreach (var row in _as400.ReadAccountsAsync(stoppingToken))
                {
                    await _upserter.UpsertAsync(row, syncRunId, DateTime.UtcNow, stoppingToken);
                    count++;

                    if (count % 500 == 0)
                        _log.LogInformation("Upserted {Count} rows...", count);
                }

                // Mark any AS400-sourced accounts not seen in this run as inactive.
                await _upserter.MarkMissingAsInactiveAsync(syncRunId, stoppingToken);

                _log.LogInformation("AS400 sync completed. Upserted={Count} SyncRunId={SyncRunId}", count, syncRunId);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "AS400 sync failed");
            }

            var delay = TimeSpan.FromMinutes(Math.Max(1, _opt.RunEveryMinutes));
            await Task.Delay(delay, stoppingToken);
        }
    }
}
