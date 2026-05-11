using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GrainManagement.As400Sync;

public sealed class As400SyncRunner
{
    // Send a progress event at LEAST every N rows AND every M ms. The runner
    // chose 2,000 originally but with rows that take a few ms to upsert that
    // looked frozen for 30+ seconds. 25 rows or 250 ms is responsive without
    // flooding the SignalR hub when rows fly by.
    private const int ProgressEveryRows = 25;
    private static readonly TimeSpan ProgressMaxInterval = TimeSpan.FromMilliseconds(250);

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

    public Task RunAsync(SyncJob job, CancellationToken ct)
        => RunAsync(job, progress: null, ct);

    public async Task RunAsync(SyncJob job, IProgress<SyncProgress>? progress, CancellationToken ct)
    {
        var syncRunId = Guid.NewGuid();
        var nowUtc = DateTime.UtcNow;
        var jobName = job.ToString();

        _log.LogInformation("AS400 {Job} sync start. SyncRunId={SyncRunId}", job, syncRunId);

        Report(progress, jobName, "Connecting", 0, null, $"Starting {jobName} sync.", syncRunId);

        switch (job)
        {
            case SyncJob.Accounts:
                {
                    Report(progress, jobName, "Counting", 0, null, "Counting accounts on Agvantage...", syncRunId);
                    var total = await _as400.CountAccountsAsync(ct);

                    Report(progress, jobName, "Reading", 0, total,
                        total.HasValue ? $"Reading {total:N0} accounts from Agvantage." : "Reading accounts from Agvantage.",
                        syncRunId);

                    long count = 0;
                    var lastReport = DateTime.UtcNow;

                    await foreach (var row in _as400.ReadAccountsAsync(ct))
                    {
                        await _accountUpserter.UpsertAsync(row, syncRunId, nowUtc, ct);
                        count++;

                        if (ShouldReport(count, ref lastReport))
                        {
                            Report(progress, jobName, "Upserting", count, total,
                                $"Writing {count:N0}{(total.HasValue ? " of " + total.Value.ToString("N0") : "")} accounts to GrainManagement.",
                                syncRunId);
                        }
                    }

                    // Reset bar to indeterminate for the MarkInactive SQL — there's no
                    // row-level progress to stream, but we still want to show motion.
                    Report(progress, jobName, "MarkInactive", 0, null,
                        "Marking missing accounts as inactive in GrainManagement...", syncRunId);
                    await _accountUpserter.MarkMissingAsInactiveAsync(syncRunId, ct);

                    _log.LogInformation("Accounts sync completed. Upserted={Count} SyncRunId={SyncRunId}", count, syncRunId);
                    Report(progress, jobName, "Done", count, count,
                        $"Accounts sync completed. {count:N0} rows.", syncRunId);
                    break;
                }

            case SyncJob.Products:
                {
                    Report(progress, jobName, "Counting", 0, null, "Counting items on Agvantage...", syncRunId);
                    var total = await _as400.CountAllProductItemsAsync(ct);

                    Report(progress, jobName, "Reading", 0, total,
                        total.HasValue ? $"Reading {total:N0} items from Agvantage." : "Reading product items from Agvantage.",
                        syncRunId);

                    long count = 0;
                    var lastReport = DateTime.UtcNow;

                    await foreach (var row in _as400.ReadAllProductItemsAsync(ct))
                    {
                        await _productUpserter.UpsertAsync(row, syncRunId, nowUtc, ct);
                        count++;

                        if (ShouldReport(count, ref lastReport))
                        {
                            Report(progress, jobName, "Upserting", count, total,
                                $"Writing {count:N0}{(total.HasValue ? " of " + total.Value.ToString("N0") : "")} items to GrainManagement.",
                                syncRunId);
                        }
                    }

                    // Reset bar to indeterminate for the MarkInactive SQL.
                    Report(progress, jobName, "MarkInactive", 0, null,
                        "Marking missing items / products as inactive in GrainManagement...", syncRunId);
                    await _productUpserter.MarkMissingAsInactiveAsync(syncRunId, nowUtc, ct);

                    _log.LogInformation("Products sync completed. Upserted={Count} SyncRunId={SyncRunId}", count, syncRunId);
                    Report(progress, jobName, "Done", count, count,
                        $"Items sync completed. {count:N0} rows.", syncRunId);
                    break;
                }

            case SyncJob.SplitGroups:
                {
                    Report(progress, jobName, "Counting", 0, null, "Counting split rows on Agvantage...", syncRunId);
                    var total = await _as400.CountLandlordSplitPercentsAsync(ct);

                    Report(progress, jobName, "Reading", 0, total,
                        total.HasValue
                            ? $"Reading {total:N0} landlord split rows from Agvantage."
                            : "Reading landlord split percentages from Agvantage.",
                        syncRunId);

                    var rows = new List<As400LandlordSplitPercentRow>(
                        capacity: total.HasValue ? (int)Math.Min(total.Value, 200_000) : 50_000);

                    long readCount = 0;
                    var lastReport = DateTime.UtcNow;

                    await foreach (var row in _as400.ReadLandlordSplitPercentsAsync(ct))
                    {
                        rows.Add(row);
                        readCount++;

                        if (ShouldReport(readCount, ref lastReport))
                        {
                            Report(progress, jobName, "Reading", readCount, total,
                                $"Reading {readCount:N0}{(total.HasValue ? " of " + total.Value.ToString("N0") : "")} split rows from Agvantage.",
                                syncRunId);
                        }
                    }

                    // Reset the bar to indeterminate: the upsert is a single bulk SQL
                    // statement with no row-level progress to stream, so leaving the
                    // bar at 100% would make it look done while it's actually still
                    // running for another 1-2 minutes.
                    Report(progress, jobName, "Upserting", 0, null,
                        $"Writing {rows.Count:N0} split rows to GrainManagement...", syncRunId);
                    await _splitGroupsUpserter.UpsertAsync(rows, ct);

                    _log.LogInformation("SplitGroups sync complete. Rows={Count}", rows.Count);
                    Report(progress, jobName, "Done", rows.Count, rows.Count,
                        $"Split groups sync completed. {rows.Count:N0} rows.", syncRunId);
                    break;
                }

            default:
                throw new ArgumentOutOfRangeException(nameof(job), job, "Unknown sync job");
        }

        _log.LogInformation("AS400 {Job} sync done.", job);
    }

    private static bool ShouldReport(long count, ref DateTime lastReportUtc)
    {
        var now = DateTime.UtcNow;
        if (count % ProgressEveryRows == 0 || (now - lastReportUtc) >= ProgressMaxInterval)
        {
            lastReportUtc = now;
            return true;
        }
        return false;
    }

    private static void Report(
        IProgress<SyncProgress>? progress,
        string job,
        string stage,
        long current,
        long? total,
        string? message,
        Guid? syncRunId)
    {
        if (progress is null) return;
        progress.Report(new SyncProgress
        {
            Job = job,
            Stage = stage,
            Current = current,
            Total = total,
            Message = message,
            SyncRunId = syncRunId
        });
    }
}
