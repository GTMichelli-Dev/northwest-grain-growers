using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GrainManagement.As400Sync;

public sealed class As400SyncWorker : BackgroundService
{
    private readonly ILogger<As400SyncWorker> _log;
    private readonly As400SyncRunner _runner;
    private readonly SyncCoordinator _coord;
    private readonly As400SyncOptions _opt;

    public As400SyncWorker(
        ILogger<As400SyncWorker> log,
        As400SyncRunner runner,
        SyncCoordinator coord,
        IOptions<As400SyncOptions> opt)
    {
        _log = log;
        _runner = runner;
        _coord = coord;
        _opt = opt.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Run in the same order you had before, but each run respects the lock.
                if (_opt.SyncAccounts)
                    await RunIfNotRunningAsync(SyncJob.Accounts, stoppingToken);

                if (_opt.SyncProducts)
                    await RunIfNotRunningAsync(SyncJob.Products, stoppingToken);

                if (_opt.SyncSplitGroups)
                    await RunIfNotRunningAsync(SyncJob.SplitGroups, stoppingToken);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "AS400 sync worker loop failed");
            }

            var delay = TimeSpan.FromMinutes(Math.Max(1, _opt.RunEveryMinutes));
            await Task.Delay(delay, stoppingToken);
        }
    }

    private async Task RunIfNotRunningAsync(SyncJob job, CancellationToken ct)
    {
        await using var lease = await _coord.TryAcquireAsync(job, ct);
        if (lease is null)
        {
            _log.LogInformation("{Job} sync skipped (already running).", job);
            return;
        }

        await _runner.RunAsync(job, ct);
    }
}
