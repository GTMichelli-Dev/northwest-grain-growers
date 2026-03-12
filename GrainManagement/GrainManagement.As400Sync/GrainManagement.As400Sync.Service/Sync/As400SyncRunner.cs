using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GrainManagement.As400Sync;

public sealed class As400SyncRunner
{
    private readonly ILogger<As400SyncRunner> _log;
    private readonly As400Reader _as400;
    private readonly AccountsUpserter _accountUpserter;
    private readonly ProductItemsUpserter _productUpserter;
    private readonly SplitGroupsUpserter _splitGroupsUpserter;
    private readonly As400SyncOptions _opt;

    public As400SyncRunner(
        ILogger<As400SyncRunner> log,
        As400Reader as400,
        AccountsUpserter accountUpserter,
        ProductItemsUpserter productUpserter,
        SplitGroupsUpserter splitGroupsUpserter,
        IOptions<As400SyncOptions> opt)
    {
        _log = log;
        _as400 = as400;
        _accountUpserter = accountUpserter;
        _productUpserter = productUpserter;
        _splitGroupsUpserter = splitGroupsUpserter;
        _opt = opt.Value;
    }

    public async Task RunAsync(SyncJob job, CancellationToken ct)
    {
        var syncRunId = Guid.NewGuid();
        var nowUtc = DateTime.UtcNow;

        _log.LogInformation("AS400 {Job} sync start. SyncRunId={SyncRunId}", job, syncRunId);

        switch (job)
        {
            case SyncJob.Accounts:
                {
                    int accountCount = 0;
                    await foreach (var row in _as400.ReadAccountsAsync(ct))
                    {
                        await _accountUpserter.UpsertAsync(row, syncRunId, nowUtc, ct);
                        accountCount++;

                        if (accountCount % Math.Max(1, _opt.BatchSize) == 0)
                            _log.LogInformation("Upserted {Count} account rows...", accountCount);
                    }

                    await _accountUpserter.MarkMissingAsInactiveAsync(syncRunId, ct);

                    _log.LogInformation("Accounts sync completed. Upserted={Count} SyncRunId={SyncRunId}", accountCount, syncRunId);
                    break;
                }

            case SyncJob.Products:
                {
                    int count = 0;
                    await foreach (var row in _as400.ReadAllProductItemsAsync(ct))
                    {
                        await _productUpserter.UpsertAsync(row, syncRunId, nowUtc, ct);
                        count++;
                    }

                    await _productUpserter.MarkMissingAsInactiveAsync(syncRunId, nowUtc, ct);

                    _log.LogInformation("Products sync completed. Upserted={Count} SyncRunId={SyncRunId}", count, syncRunId);
                    break;
                }

            case SyncJob.SplitGroups:
                {
                    var rows = new List<As400LandlordSplitPercentRow>(capacity: 50_000);

                    await foreach (var row in _as400.ReadLandlordSplitPercentsAsync(ct))
                        rows.Add(row);

                    await _splitGroupsUpserter.UpsertAsync(rows, ct);

                    _log.LogInformation("SplitGroups sync complete. Rows={Count}", rows.Count);
                    break;
                }

            default:
                throw new ArgumentOutOfRangeException(nameof(job), job, "Unknown sync job");
        }

        _log.LogInformation("AS400 {Job} sync done.", job);
    }
}
