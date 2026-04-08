using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using ScaleReaderService.Data;
using ScaleReaderService.Models;
using ScaleReaderService.Services;

namespace ScaleReaderService;

public class ScaleWorker : BackgroundService
{
    private readonly IServiceProvider _sp;
    private readonly ILogger<ScaleWorker> _log;
    private readonly RestartSignal _restart;
    private readonly AnnounceSignal _announce;
    private readonly SmaClient _smaClient;
    private readonly ScaleWeightStore _weightStore;
    private HubConnection? _connection;
    private string _serviceId = "default";

    public ScaleWorker(
        IServiceProvider sp,
        ILogger<ScaleWorker> log,
        RestartSignal restart,
        AnnounceSignal announce,
        SmaClient smaClient,
        ScaleWeightStore weightStore)
    {
        _sp = sp;
        _log = log;
        _restart = restart;
        _announce = announce;
        _smaClient = smaClient;
        _weightStore = weightStore;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Subscribe to announce signal
        _announce.OnAnnounceRequested += async () =>
        {
            try { await AnnounceScales(); }
            catch (Exception ex) { _log.LogWarning("Announce failed: {Msg}", ex.Message); }
        };

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Load settings from DB
                string serverUrl, hubPath;
                using (var scope = _sp.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<ScaleDbContext>();
                    var settings = db.Settings.OrderBy(s => s.Id).FirstOrDefault();
                    serverUrl = settings?.ServerUrl ?? "http://localhost:5110";
                    hubPath = settings?.SignalRHub ?? "/scaleHub";
                    _serviceId = settings?.ServiceId ?? "default";

                    // Load brands from DB URL if available
                    if (!string.IsNullOrWhiteSpace(settings?.BrandsUrl))
                    {
                        var logger = _sp.GetRequiredService<ILoggerFactory>().CreateLogger("ScaleBrands");
                        var localPath = Path.Combine(AppContext.BaseDirectory, "scale-models.json");
                        await ScaleBrandDefinition.LoadBrandsAsync(settings.BrandsUrl, localPath, settings.BrandsToken, logger);
                    }
                }

                _log.LogInformation("Loaded settings from database: ServiceId={ServiceId}, ServerUrl={Url}", _serviceId, serverUrl);

                await ConnectAndRun(serverUrl, hubPath, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _log.LogWarning("Connection lost. Reconnecting in 5 seconds... Error: {Msg}", ex.Message);
                try { await Task.Delay(5000, stoppingToken); }
                catch (OperationCanceledException) { break; }
            }
            finally
            {
                if (_connection != null)
                {
                    try { await _connection.DisposeAsync(); }
                    catch { /* ignore cleanup errors */ }
                    _connection = null;
                }
            }
        }
    }

    private async Task ConnectAndRun(string serverUrl, string hubPath, CancellationToken stoppingToken)
    {
        _connection = new HubConnectionBuilder()
            .WithUrl($"{serverUrl.TrimEnd('/')}{hubPath}")
            .WithAutomaticReconnect(new ForeverRetryPolicy())
            .Build();

        _connection.Reconnecting += ex =>
        {
            _log.LogWarning("Connection lost. Reconnecting...");
            return Task.CompletedTask;
        };

        _connection.Reconnected += async _ =>
        {
            _log.LogInformation("Reconnected. Rejoining scale groups...");
            await JoinGroups();
            await AnnounceScales();
        };

        // Register handlers for SignalR commands from the web app
        RegisterHandlers();

        _log.LogInformation("Connecting to {Url}{Hub}", serverUrl, hubPath);
        await _connection.StartAsync(stoppingToken);
        _log.LogInformation("Connected to server. Joining scale groups (ServiceId={ServiceId})...", _serviceId);

        await JoinGroups();
        _log.LogInformation("Joined scale groups. Starting scale pollers...");

        await AnnounceScales();

        // Start polling all active scales
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken, _restart.Token);
        await PollScales(linked.Token);
    }

    private void RegisterHandlers()
    {
        // Web app can request scale list
        _connection!.On("GetScaleList", async () =>
        {
            try
            {
                using var scope = _sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ScaleDbContext>();
                var scales = await db.Scales.Where(s => s.Active).OrderBy(s => s.ScaleId).ToListAsync();
                await _connection!.InvokeAsync("ScaleListResponse", new
                {
                    serviceId = _serviceId,
                    scales = scales.Select(s => new
                    {
                        s.ScaleId, s.DisplayName, s.ScaleBrand, s.IpAddress,
                        s.Port, s.Active, s.PollingIntervalMs
                    })
                });
            }
            catch (Exception ex) { _log.LogWarning("GetScaleList failed: {Msg}", ex.Message); }
        });

        // Reload config command
        _connection!.On("ReloadConfig", () =>
        {
            _log.LogInformation("Received ReloadConfig command. Restarting...");
            _restart.TriggerRestart();
        });

        // Zero scale command
        _connection!.On<string>("ZeroScale", async (scaleId) =>
        {
            _log.LogInformation("Received ZeroScale command for scale: {ScaleId}", scaleId);
            try
            {
                using var scope = _sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ScaleDbContext>();
                var scale = await db.Scales.FirstOrDefaultAsync(s => s.ScaleId == scaleId && s.Active);
                if (scale == null)
                {
                    _log.LogWarning("ZeroScale: Scale '{ScaleId}' not found or inactive", scaleId);
                    return;
                }

                // Send zero command to the scale indicator
                var zeroCommand = "Z\r\n"; // Standard SMA zero command
                using var client = new System.Net.Sockets.TcpClient();
                using var cts = new CancellationTokenSource(scale.TimeoutMs > 0 ? scale.TimeoutMs : 2000);
                await client.ConnectAsync(scale.IpAddress, scale.Port, cts.Token);
                using var ns = client.GetStream();
                var bytes = System.Text.Encoding.ASCII.GetBytes(zeroCommand);
                await ns.WriteAsync(bytes, cts.Token);
                await ns.FlushAsync(cts.Token);
                _log.LogInformation("ZeroScale: Sent zero command to {ScaleId} at {Ip}:{Port}", scaleId, scale.IpAddress, scale.Port);
            }
            catch (Exception ex)
            {
                _log.LogError("ZeroScale failed for {ScaleId}: {Msg}", scaleId, ex.Message);
            }
        });

        // CRUD: Add scale
        _connection!.On<System.Text.Json.JsonElement>("AddScale", async (config) =>
        {
            try
            {
                using var scope = _sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ScaleDbContext>();
                var entity = System.Text.Json.JsonSerializer.Deserialize<ScaleConfigEntity>(config.GetRawText(),
                    new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (entity == null || string.IsNullOrWhiteSpace(entity.ScaleId))
                {
                    await _connection!.InvokeAsync("ScaleCrudResult", new { success = false, message = "Invalid scale data" });
                    return;
                }
                entity.Id = 0;
                db.Scales.Add(entity);
                await db.SaveChangesAsync();
                await _connection!.InvokeAsync("ScaleCrudResult", new { success = true, message = "Scale added: " + entity.ScaleId });
                _restart.TriggerRestart(); // restart to pick up new scale
            }
            catch (Exception ex)
            {
                await _connection!.InvokeAsync("ScaleCrudResult", new { success = false, message = ex.Message });
            }
        });

        // CRUD: Update scale
        _connection!.On<string, System.Text.Json.JsonElement>("UpdateScale", async (scaleId, config) =>
        {
            try
            {
                using var scope = _sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ScaleDbContext>();
                var existing = db.Scales.FirstOrDefault(s => s.ScaleId == scaleId);
                if (existing == null)
                {
                    await _connection!.InvokeAsync("ScaleCrudResult", new { success = false, message = "Scale not found: " + scaleId });
                    return;
                }
                var update = System.Text.Json.JsonSerializer.Deserialize<ScaleConfigEntity>(config.GetRawText(),
                    new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (update != null)
                {
                    existing.DisplayName = update.DisplayName ?? existing.DisplayName;
                    existing.ScaleBrand = update.ScaleBrand ?? existing.ScaleBrand;
                    existing.IpAddress = update.IpAddress ?? existing.IpAddress;
                    if (update.Port > 0) existing.Port = update.Port;
                    existing.RequestCommand = update.RequestCommand;
                    if (update.PollingIntervalMs > 0) existing.PollingIntervalMs = update.PollingIntervalMs;
                    if (update.TimeoutMs > 0) existing.TimeoutMs = update.TimeoutMs;
                    existing.Active = update.Active;
                }
                await db.SaveChangesAsync();
                await _connection!.InvokeAsync("ScaleCrudResult", new { success = true, message = "Scale updated: " + scaleId });
                _restart.TriggerRestart();
            }
            catch (Exception ex)
            {
                await _connection!.InvokeAsync("ScaleCrudResult", new { success = false, message = ex.Message });
            }
        });

        // CRUD: Delete scale
        _connection!.On<string>("DeleteScale", async (scaleId) =>
        {
            try
            {
                using var scope = _sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ScaleDbContext>();
                var existing = db.Scales.FirstOrDefault(s => s.ScaleId == scaleId);
                if (existing == null)
                {
                    await _connection!.InvokeAsync("ScaleCrudResult", new { success = false, message = "Scale not found: " + scaleId });
                    return;
                }
                db.Scales.Remove(existing);
                await db.SaveChangesAsync();
                await _connection!.InvokeAsync("ScaleCrudResult", new { success = true, message = "Scale deleted: " + scaleId });
                _restart.TriggerRestart();
            }
            catch (Exception ex)
            {
                await _connection!.InvokeAsync("ScaleCrudResult", new { success = false, message = ex.Message });
            }
        });
    }

    private async Task JoinGroups()
    {
        await _connection!.InvokeAsync("JoinScaleGroup", _serviceId);
    }

    private async Task AnnounceScales()
    {
        if (_connection?.State != HubConnectionState.Connected) return;

        try
        {
            using var scope = _sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ScaleDbContext>();
            var scales = await db.Scales.Where(s => s.Active).OrderBy(s => s.ScaleId).ToListAsync();

            await _connection.InvokeAsync("ScaleServiceReady", new
            {
                serviceId = _serviceId,
                scaleCount = scales.Count,
                scales = scales.Select(s => new
                {
                    s.ScaleId, s.DisplayName, s.ScaleBrand, s.IpAddress,
                    s.Port, s.Active
                })
            });

            _log.LogInformation("Announced {Count} scale(s) to web app.", scales.Count);
        }
        catch (Exception ex)
        {
            _log.LogWarning("Failed to announce scales: {Msg}", ex.Message);
        }
    }

    private async Task PollScales(CancellationToken ct)
    {
        List<ScaleConfigEntity> scales;
        using (var scope = _sp.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ScaleDbContext>();
            scales = await db.Scales.Where(s => s.Active).ToListAsync(ct);
        }

        if (scales.Count == 0)
        {
            _log.LogWarning("No active scales configured. Waiting for changes...");
            try { await Task.Delay(Timeout.Infinite, ct); }
            catch (OperationCanceledException) { }
            return;
        }

        _log.LogInformation("Starting {Count} scale poller(s).", scales.Count);

        // One independent task per scale
        var tasks = scales.Select(scale => Task.Run(() => PollSingleScale(scale, ct), ct)).ToArray();
        await Task.WhenAll(tasks);
    }

    private async Task PollSingleScale(ScaleConfigEntity scale, CancellationToken ct)
    {
        var backoff = 2000;
        var maxBackoff = 10000;

        while (!ct.IsCancellationRequested)
        {
            try
            {
                var (ok, weight, motion, status, rawText, rawHex) = await _smaClient.QueryOnceAsync(
                    scale.IpAddress, scale.Port, scale.RequestCommand, scale.TimeoutMs, ct);

                // Update in-memory store for REST API access
                _weightStore.Update(scale.ScaleId, new ScaleReading
                {
                    ScaleId = scale.ScaleId,
                    DisplayName = scale.DisplayName,
                    Weight = weight,
                    Motion = motion,
                    Ok = ok,
                    Status = status,
                    RawResponse = rawText,
                    RawHex = rawHex,
                    LastUpdate = DateTime.Now
                });

                // Send weight data to web app via SignalR
                if (_connection?.State == HubConnectionState.Connected)
                {
                    await _connection.InvokeAsync("ScaleWeight", new
                    {
                        serviceId = _serviceId,
                        scaleId = scale.ScaleId,
                        displayName = scale.DisplayName,
                        weight,
                        motion,
                        ok,
                        status,
                        rawResponse = rawText,
                        rawHex,
                        lastUpdate = DateTime.Now
                    }, ct);
                }

                await Task.Delay(scale.PollingIntervalMs, ct);
                backoff = 2000; // reset after success
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _log.LogWarning("Scale '{Name}' poll failed: {Msg}. Retrying in {Backoff}ms...",
                    scale.DisplayName, ex.Message, backoff);

                // Update store with error status
                _weightStore.Update(scale.ScaleId, new ScaleReading
                {
                    ScaleId = scale.ScaleId,
                    DisplayName = scale.DisplayName,
                    Weight = 0,
                    Motion = false,
                    Ok = false,
                    Status = "Disconnected",
                    LastUpdate = DateTime.Now
                });

                // Send error status via SignalR
                if (_connection?.State == HubConnectionState.Connected)
                {
                    try
                    {
                        await _connection.InvokeAsync("ScaleWeight", new
                        {
                            serviceId = _serviceId,
                            scaleId = scale.ScaleId,
                            displayName = scale.DisplayName,
                            weight = 0,
                            motion = false,
                            ok = false,
                            status = "Disconnected",
                            lastUpdate = DateTime.Now
                        }, ct);
                    }
                    catch { /* ignore */ }
                }

                try { await Task.Delay(backoff, ct); }
                catch (OperationCanceledException) { break; }
                backoff = Math.Min(backoff * 2, maxBackoff);
            }
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_connection != null)
        {
            try { await _connection.DisposeAsync(); }
            catch { /* ignore */ }
        }
        await base.StopAsync(cancellationToken);
    }
}

/// <summary>
/// Retry policy that never gives up — backs off 2s, 5s, 10s, then stays at 30s forever.
/// </summary>
public class ForeverRetryPolicy : IRetryPolicy
{
    private static readonly TimeSpan[] Delays = new[]
    {
        TimeSpan.FromSeconds(2),
        TimeSpan.FromSeconds(5),
        TimeSpan.FromSeconds(10),
        TimeSpan.FromSeconds(30)
    };

    public TimeSpan? NextRetryDelay(RetryContext retryContext)
    {
        var idx = Math.Min(retryContext.PreviousRetryCount, Delays.Length - 1);
        return Delays[idx];
    }
}
