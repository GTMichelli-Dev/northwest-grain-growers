using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GrainManagement.As400Sync;

public sealed class As400SyncWorker : BackgroundService
{
    private readonly ILogger<As400SyncWorker> _log;
    private readonly As400Reader _as400;
    private readonly AccountsUpserter _accountUpserter;
    private readonly ProductItemsUpserter _productUpserter;
    private readonly SplitGroupsUpserter _splitGroupsUpserter;

    private readonly As400SyncOptions _opt;

    public As400SyncWorker(
        ILogger<As400SyncWorker> log,
    As400Reader as400,
    AccountsUpserter accountUpserter,
    IOptions<As400SyncOptions> opt,
    ProductItemsUpserter productUpserter,
    SplitGroupsUpserter splitGroupsUpserter)
    {
        _log = log;
        _as400 = as400;
        _accountUpserter = accountUpserter;
        _productUpserter = productUpserter;
        _splitGroupsUpserter = splitGroupsUpserter;
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
                if (_opt.SyncAccounts)
                {
                    int accountCount = 0;

                    await foreach (var row in _as400.ReadAccountsAsync(stoppingToken))
                    {
                      


                        await _accountUpserter.UpsertAsync(row, syncRunId, DateTime.UtcNow, stoppingToken);
                        accountCount++;

                        if (accountCount % Math.Max(1, _opt.BatchSize) == 0)
                            _log.LogInformation("Upserted {Count} account rows...", accountCount);
                    }

                    // Mark any AS400-sourced accounts not seen in this run as inactive.
                    await _accountUpserter.MarkMissingAsInactiveAsync(syncRunId, stoppingToken);

                    _log.LogInformation("Accounts sync completed. Upserted={Count} SyncRunId={SyncRunId}", accountCount, syncRunId);
                }
                if (_opt.SyncProducts)
                {
                    int count = 0;

                    await foreach (var row in _as400.ReadAllProductItemsAsync(stoppingToken))
                    {
                        await _productUpserter.UpsertAsync(row, syncRunId, DateTime.UtcNow, stoppingToken);
                        count++;
                    }

                    await _productUpserter.MarkMissingAsInactiveAsync(syncRunId, DateTime.UtcNow, stoppingToken);

                    _log.LogInformation("Products sync completed. Upserted={Count} SyncRunId={SyncRunId}",
                        count, syncRunId);
                }
                if (_opt.SyncSplitGroups)
                {
                    var rows = new List<As400LandlordSplitPercentRow>(capacity: 50_000);

                    await foreach (var row in _as400.ReadLandlordSplitPercentsAsync(stoppingToken))
                        rows.Add(row);

                    await _splitGroupsUpserter.UpsertAsync(rows, stoppingToken);

                    _log.LogInformation("SplitGroups sync complete. Rows={Count}", rows.Count);
                }



                _log.LogInformation("AS400 sync completed.");
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
