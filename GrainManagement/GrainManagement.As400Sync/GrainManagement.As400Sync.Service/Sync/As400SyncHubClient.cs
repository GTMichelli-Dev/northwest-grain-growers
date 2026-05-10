using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace GrainManagement.As400Sync;

/// <summary>
/// Tells SignalR to retry every 10 seconds indefinitely. The default policy
/// gives up after ~42 seconds, which would leave the service offline if the
/// website stays down longer than that.
/// </summary>
internal sealed class TenSecondForeverRetryPolicy : IRetryPolicy
{
    private static readonly TimeSpan Delay = TimeSpan.FromSeconds(10);
    public TimeSpan? NextRetryDelay(RetryContext retryContext) => Delay;
}

/// <summary>
/// Outbound SignalR client. Connects to the website's <c>/hubs/as400sync</c>
/// endpoint, registers as a service, and listens for <c>RunAccounts</c> /
/// <c>RunProducts</c> / <c>RunSplitGroups</c> commands. While a job runs we
/// stream progress (stage + counters) back to the hub so the admin page can
/// render a live status panel.
///
/// Mirrors the same pattern that ScaleReaderService and WebPrintService use.
/// </summary>
public sealed class As400SyncHubClient : BackgroundService
{
    private readonly ILogger<As400SyncHubClient> _log;
    private readonly As400SyncOptions _opt;
    private readonly SyncCoordinator _coord;
    private readonly As400SyncRunner _runner;
    private readonly WarehouseTransferUploader _whUploader;
    private readonly string _version;

    private HubConnection? _conn;

    public As400SyncHubClient(
        ILogger<As400SyncHubClient> log,
        IOptions<As400SyncOptions> opt,
        SyncCoordinator coord,
        As400SyncRunner runner,
        WarehouseTransferUploader whUploader)
    {
        _log = log;
        _opt = opt.Value;
        _coord = coord;
        _runner = runner;
        _whUploader = whUploader;

        var asm = Assembly.GetExecutingAssembly();
        var info = asm.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
        if (!string.IsNullOrWhiteSpace(info))
        {
            var plus = info.IndexOf('+');
            _version = plus > 0 ? info[..plus] : info;
        }
        else
        {
            _version = asm.GetName().Version?.ToString() ?? "0.0.0";
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (string.IsNullOrWhiteSpace(_opt.HubUrl))
        {
            _log.LogWarning("As400Sync:HubUrl is not configured; SignalR client disabled.");
            return;
        }

        var serviceId = string.IsNullOrWhiteSpace(_opt.ServiceId) ? "as400sync" : _opt.ServiceId;

        _conn = new HubConnectionBuilder()
            .WithUrl(_opt.HubUrl)
            .WithAutomaticReconnect(new TenSecondForeverRetryPolicy())
            .Build();

        _conn.On("RunAccounts", () => _ = HandleRunAsync(SyncJob.Accounts, serviceId, stoppingToken));
        _conn.On("RunProducts", () => _ = HandleRunAsync(SyncJob.Products, serviceId, stoppingToken));
        _conn.On("RunSplitGroups", () => _ = HandleRunAsync(SyncJob.SplitGroups, serviceId, stoppingToken));
        _conn.On<long[]>("RunWarehouseTransferUpload",
            ids => _ = HandleWarehouseTransferUploadAsync(ids, serviceId, stoppingToken));
        _conn.On("RunClearU5Siload",
            () => _ = HandleClearU5SiloadAsync(serviceId, stoppingToken));
        _conn.On("RequestSnapshot", () => _ = AnnounceSnapshotAsync(serviceId, stoppingToken));

        _conn.Reconnecting += ex =>
        {
            _log.LogDebug(ex, "AS400 sync hub Reconnecting event details.");
            _log.LogWarning("AS400 sync hub: connection lost. Retrying in 10 seconds...");
            return Task.CompletedTask;
        };

        _conn.Reconnected += async _ =>
        {
            _log.LogInformation("AS400 sync hub: connection restored. Re-registering as {ServiceId}.", serviceId);
            await SafeRegisterAsync(serviceId, stoppingToken);
        };

        // Belt-and-suspenders: if WithAutomaticReconnect ever gives up (e.g.
        // a transport-level error it considers fatal), kick off a fresh
        // StartAsync loop so we keep trying every 10 seconds forever.
        _conn.Closed += async ex =>
        {
            if (stoppingToken.IsCancellationRequested) return;
            _log.LogDebug(ex, "AS400 sync hub Closed event details.");
            _log.LogWarning("AS400 sync hub: connection closed. Retrying in 10 seconds...");
            try { await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken); }
            catch (OperationCanceledException) { return; }
            await ConnectLoopAsync(serviceId, stoppingToken);
        };

        await ConnectLoopAsync(serviceId, stoppingToken);

        // Park until cancelled.
        try { await Task.Delay(Timeout.Infinite, stoppingToken); }
        catch (OperationCanceledException) { /* shutdown */ }

        try { await _conn.DisposeAsync(); } catch { /* ignore */ }
    }

    private async Task ConnectLoopAsync(string serviceId, CancellationToken ct)
    {
        if (_conn is null) return;
        while (!ct.IsCancellationRequested)
        {
            if (_conn.State == HubConnectionState.Connected) return;
            _log.LogInformation("AS400 sync hub: attempting to connect to {Url}...", _opt.HubUrl);
            try
            {
                await _conn.StartAsync(ct);
                _log.LogInformation("AS400 sync hub: connected as {ServiceId}.", serviceId);
                await SafeRegisterAsync(serviceId, ct);
                return;
            }
            catch (OperationCanceledException) { return; }
            catch (Exception ex)
            {
                _log.LogDebug(ex, "AS400 sync hub connect attempt threw.");
                _log.LogWarning("AS400 sync hub: connect failed ({Reason}). Retrying in 10 seconds...", ex.Message);
                try { await Task.Delay(TimeSpan.FromSeconds(10), ct); }
                catch (OperationCanceledException) { return; }
            }
        }
    }

    private async Task SafeRegisterAsync(string serviceId, CancellationToken ct)
    {
        if (_conn is null) return;
        try
        {
            await _conn.InvokeAsync("RegisterService", serviceId, ct);
            await AnnounceSnapshotAsync(serviceId, ct);
        }
        catch (Exception ex)
        {
            _log.LogWarning(ex, "AS400 sync hub register failed.");
        }
    }

    private async Task AnnounceSnapshotAsync(string serviceId, CancellationToken ct)
    {
        if (_conn is null) return;
        try
        {
            var busy = _coord.IsRunning(SyncJob.Accounts, out _)
                    || _coord.IsRunning(SyncJob.Products, out _)
                    || _coord.IsRunning(SyncJob.SplitGroups, out _);

            await _conn.InvokeAsync("ReportStatus", new SyncProgress
            {
                ServiceId = serviceId,
                Job = "",
                Stage = busy ? "Busy" : "Idle",
                Message = busy ? "A sync is currently running." : "Service idle.",
                Version = _version,
            }, ct);
        }
        catch (Exception ex)
        {
            _log.LogDebug(ex, "AS400 sync hub snapshot send failed.");
        }
    }

    private async Task HandleRunAsync(SyncJob job, string serviceId, CancellationToken ct)
    {
        if (_conn is null) return;

        await using var lease = await _coord.TryAcquireAsync(job, ct);
        if (lease is null)
        {
            await SafeSendAsync("ReportError", new
            {
                ServiceId = serviceId,
                Job = job.ToString(),
                Message = "A sync is already running."
            }, ct);
            return;
        }

        var progress = new Progress<SyncProgress>(async p =>
        {
            var stamped = new SyncProgress
            {
                ServiceId = serviceId,
                Job = p.Job,
                Stage = p.Stage,
                Current = p.Current,
                Total = p.Total,
                Message = p.Message,
                AtUtc = p.AtUtc,
                SyncRunId = p.SyncRunId,
                Version = _version,
            };
            await SafeSendAsync("ReportStatus", stamped, ct);
        });

        try
        {
            await _runner.RunAsync(job, progress, ct);

            await SafeSendAsync("ReportCompleted", new
            {
                ServiceId = serviceId,
                Job = job.ToString(),
                Message = $"{job} sync completed."
            }, ct);
        }
        catch (OperationCanceledException) { /* shutdown */ }
        catch (Exception ex)
        {
            _log.LogError(ex, "AS400 sync {Job} failed", job);
            await SafeSendAsync("ReportError", new
            {
                ServiceId = serviceId,
                Job = job.ToString(),
                Message = ex.Message
            }, ct);
        }
    }

    private async Task HandleWarehouseTransferUploadAsync(long[]? wsIds, string serviceId, CancellationToken ct)
    {
        if (_conn is null) return;
        wsIds ??= Array.Empty<long>();
        if (wsIds.Length == 0)
        {
            await SafeSendAsync("ReportError", new
            {
                ServiceId = serviceId,
                Job = "WarehouseTransferUpload",
                Message = "No weight sheets selected."
            }, ct);
            return;
        }

        await SafeSendAsync("ReportStatus", new SyncProgress
        {
            ServiceId = serviceId,
            Job = "WarehouseTransferUpload",
            Stage = "Connecting",
            Current = 0,
            Total = wsIds.Length,
            Message = $"Preparing to upload {wsIds.Length:N0} weight sheets to Agvantage.",
            Version = _version,
        }, ct);

        var progress = new Progress<(int current, int total, string? message)>(async tuple =>
        {
            await SafeSendAsync("ReportStatus", new SyncProgress
            {
                ServiceId = serviceId,
                Job = "WarehouseTransferUpload",
                Stage = "Upserting",
                Current = tuple.current,
                Total = tuple.total,
                Message = tuple.message,
                Version = _version,
            }, ct);
        });

        try
        {
            var result = await _whUploader.UploadAsync(wsIds, progress, ct);

            // Stream individual error events so the UI can list them.
            foreach (var err in result.Errors)
            {
                await SafeSendAsync("ReportError", new
                {
                    ServiceId = serviceId,
                    Job = "WarehouseTransferUpload",
                    WeightSheetId = err.WeightSheetId,
                    As400Id = err.As400Id,
                    Message = err.Message,
                }, ct);
            }

            await SafeSendAsync("ReportStatus", new SyncProgress
            {
                ServiceId = serviceId,
                Job = "WarehouseTransferUpload",
                Stage = "Done",
                Current = result.Total,
                Total = result.Total,
                Message = $"Uploaded {result.Succeeded:N0} of {result.Total:N0} weight sheets" +
                          (result.Errors.Count > 0 ? $" ({result.Errors.Count} error{(result.Errors.Count == 1 ? "" : "s")})." : "."),
                Version = _version,
            }, ct);

            await SafeSendAsync("ReportCompleted", new
            {
                ServiceId = serviceId,
                Job = "WarehouseTransferUpload",
                Total = result.Total,
                Succeeded = result.Succeeded,
                ErrorCount = result.Errors.Count,
                Message = $"Upload complete. {result.Succeeded:N0} ok, {result.Errors.Count:N0} error{(result.Errors.Count == 1 ? "" : "s")}.",
            }, ct);
        }
        catch (OperationCanceledException) { /* shutdown */ }
        catch (Exception ex)
        {
            _log.LogError(ex, "Warehouse transfer upload failed");
            await SafeSendAsync("ReportError", new
            {
                ServiceId = serviceId,
                Job = "WarehouseTransferUpload",
                Message = ex.Message
            }, ct);
        }
    }

    private async Task HandleClearU5SiloadAsync(string serviceId, CancellationToken ct)
    {
        if (_conn is null) return;

        await SafeSendAsync("ReportStatus", new SyncProgress
        {
            ServiceId = serviceId,
            Job = "ClearU5Siload",
            Stage = "Connecting",
            Current = 0,
            Total = null,
            Message = "Clearing U5SILOAD on Agvantage...",
            Version = _version,
        }, ct);

        try
        {
            var affected = await _whUploader.ClearAllAsync(ct);

            await SafeSendAsync("ReportStatus", new SyncProgress
            {
                ServiceId = serviceId,
                Job = "ClearU5Siload",
                Stage = "Done",
                Current = affected < 0 ? 0 : affected,
                Total  = affected < 0 ? null : (long?)affected,
                Message = affected < 0
                    ? "U5SILOAD cleared (driver did not report row count)."
                    : $"U5SILOAD cleared. {affected:N0} row{(affected == 1 ? "" : "s")} deleted.",
                Version = _version,
            }, ct);

            await SafeSendAsync("ReportCompleted", new
            {
                ServiceId = serviceId,
                Job = "ClearU5Siload",
                Affected = affected,
                Message = affected < 0
                    ? "U5SILOAD cleared (row count unknown)."
                    : $"U5SILOAD cleared. {affected:N0} row{(affected == 1 ? "" : "s")} deleted.",
            }, ct);
        }
        catch (OperationCanceledException) { /* shutdown */ }
        catch (Exception ex)
        {
            _log.LogError(ex, "Clear U5SILOAD failed");
            await SafeSendAsync("ReportError", new
            {
                ServiceId = serviceId,
                Job = "ClearU5Siload",
                Message = ex.Message,
            }, ct);
        }
    }

    private async Task SafeSendAsync(string method, object payload, CancellationToken ct)
    {
        if (_conn is null) return;
        if (_conn.State != HubConnectionState.Connected) return;
        try { await _conn.InvokeAsync(method, payload, ct); }
        catch (Exception ex) { _log.LogDebug(ex, "AS400 sync hub send {Method} failed.", method); }
    }
}
