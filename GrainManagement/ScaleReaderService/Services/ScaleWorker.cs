using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using ScaleReaderService.Data;
using ScaleReaderService.Models;
using System.Text.Json;

namespace ScaleReaderService.Services
{
    public sealed class ScaleWorker : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<ScaleWorker> _log;
        private readonly ServiceSettings _settings;
        private readonly SmaClient _smaClient;
        private readonly RestartSignal _restartSignal;

        private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNameCaseInsensitive = true };

        public ScaleWorker(
            IServiceProvider services,
            IOptions<ServiceSettings> settings,
            SmaClient smaClient,
            RestartSignal restartSignal,
            ILogger<ScaleWorker> log)
        {
            _services = services;
            _settings = settings.Value;
            _smaClient = smaClient;
            _restartSignal = restartSignal;
            _log = log;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Outer safety net. If the inner loop itself ever escapes —
            // even due to a logger throwing or some runtime corner case —
            // we sleep and re-enter rather than letting the worker exit.
            // Combined with HostOptions.BackgroundServiceExceptionBehavior =
            // Ignore in Program.cs, this means a bad day on the network
            // can't take the service down.
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await RunWorkerLoopAsync(stoppingToken);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    return;
                }
                catch (Exception ex)
                {
                    SafeLog(() => _log.LogError(ex, "ScaleWorker outer loop crashed. Restarting in 10s."));
                    try { await Task.Delay(10_000, stoppingToken); }
                    catch (OperationCanceledException) { return; }
                }
            }
        }

        private static void SafeLog(Action a)
        {
            try { a(); } catch { /* swallow logger failures so the recover-and-retry loop can keep going */ }
        }

        private async Task RunWorkerLoopAsync(CancellationToken stoppingToken)
        {
            var hubUrls = _settings.HubUrls;

            _log.LogInformation("ScaleWorker starting. ServiceId={ServiceId}, Hubs={HubUrls}",
                _settings.ServiceId, string.Join(", ", hubUrls));

            while (!stoppingToken.IsCancellationRequested)
            {
                using var restartCts = CancellationTokenSource.CreateLinkedTokenSource(
                    stoppingToken, _restartSignal.Token);
                var cycleToken = restartCts.Token;

                var hubs = new List<HubConnection>();
                try
                {
                    // ── Build and connect one hub per server URL ────────────────
                    foreach (var hubUrl in hubUrls)
                    {
                        var hub = new HubConnectionBuilder()
                            .WithUrl(hubUrl)
                            .AddJsonProtocol(o =>
                            {
                                o.PayloadSerializerOptions.PropertyNamingPolicy = null;
                            })
                            .WithAutomaticReconnect(new ForeverRetryPolicy(_log, hubUrl))
                            .Build();

                        var capturedUrl = hubUrl;

                        hub.Closed += ex =>
                        {
                            if (ex != null) _log.LogWarning(ex, "SignalR connection to {HubUrl} closed with error.", capturedUrl);
                            else _log.LogInformation("SignalR connection to {HubUrl} closed.", capturedUrl);

                            // Defense in depth: if SignalR ever gives up on
                            // its own auto-reconnect (anything that bypasses
                            // ForeverRetryPolicy — an exception thrown deep
                            // inside the transport, etc.) the worker would
                            // otherwise sit dead. Requesting a cycle restart
                            // tears down the hubs and rebuilds them in the
                            // outer loop. Suppressed during graceful shutdown
                            // (cycleToken already cancelled by intentional
                            // dispose).
                            if (!cycleToken.IsCancellationRequested)
                            {
                                _log.LogInformation("Requesting cycle restart to rebuild SignalR connection to {HubUrl}.", capturedUrl);
                                _restartSignal.RequestRestart();
                            }
                            return Task.CompletedTask;
                        };

                        hub.Reconnecting += ex =>
                        {
                            _log.LogWarning(ex, "SignalR reconnecting to {HubUrl}...", capturedUrl);
                            return Task.CompletedTask;
                        };

                        hub.Reconnected += connectionId =>
                        {
                            _log.LogInformation("SignalR reconnected to {HubUrl}. Re-joining group and re-announcing scales.", capturedUrl);
                            _ = Task.Run(async () =>
                            {
                                try
                                {
                                    await hub.InvokeAsync("JoinScaleGroup", _settings.ServiceId, cycleToken);
                                    await AnnounceScalesAsync(hub, capturedUrl, cycleToken);
                                }
                                catch (Exception ex)
                                {
                                    _log.LogWarning(ex, "Failed to re-join/re-announce on {HubUrl} after reconnect.", capturedUrl);
                                }
                            });
                            return Task.CompletedTask;
                        };

                        // Register CRUD handlers only on the first hub (avoid duplicate triggers)
                        if (hubs.Count == 0)
                            RegisterCrudHandlers(hub);

                        hubs.Add(hub);
                    }

                    // ── Connect each hub independently in background ────────────
                    // Each hub connects on its own — one being unavailable does not block the others.
                    for (int i = 0; i < hubs.Count; i++)
                    {
                        var hub = hubs[i];
                        var url = hubUrls[i];
                        _ = Task.Run(async () =>
                        {
                            try
                            {
                                await ConnectWithRetryAsync(hub, url, cycleToken);
                                await hub.InvokeAsync("JoinScaleGroup", _settings.ServiceId, cycleToken);
                                await AnnounceScalesAsync(hub, url, cycleToken);
                                _log.LogInformation("Connected and announced on {HubUrl}.", url);
                            }
                            catch (OperationCanceledException) { }
                            catch (Exception ex)
                            {
                                _log.LogWarning(ex, "Failed to connect/announce on {HubUrl}. Will retry on reconnect.", url);
                            }
                        }, cycleToken);
                    }

                    // ── Load scale configs and start polling ────────────────────
                    var scales = LoadScaleConfigs();

                    if (scales.Count == 0)
                    {
                        _log.LogWarning("No scale configurations found. Waiting for config via SignalR.");
                        try { await Task.Delay(Timeout.Infinite, cycleToken); }
                        catch (OperationCanceledException) { }
                        continue;
                    }

                    _log.LogInformation("Starting {Count} scale pollers across {HubCount} hub(s).",
                        scales.Count, hubs.Count);

                    var tasks = scales
                        .Select(s => Task.Run(() => PollScaleAsync(s, hubs, cycleToken), cycleToken))
                        .ToList();

                    await Task.WhenAll(tasks);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                catch (OperationCanceledException)
                {
                    _log.LogInformation("ScaleWorker restarting due to restart signal.");
                }
                catch (Exception ex)
                {
                    _log.LogError(ex, "ScaleWorker encountered an unexpected error. Restarting in 5s.");
                    await Task.Delay(5000, stoppingToken);
                }
                finally
                {
                    foreach (var hub in hubs)
                    {
                        try { await hub.DisposeAsync(); }
                        catch { /* best-effort cleanup */ }
                    }
                }
            }

            _log.LogInformation("ScaleWorker stopping.");
        }

        // ── CRUD Handlers (called by web UI via SignalR) ──────────────────────

        private void RegisterCrudHandlers(HubConnection hub)
        {
            // Announce (re-announce scales on demand from web UI)
            hub.On("Announce", async () =>
            {
                _log.LogInformation("Announce request received from web UI.");
                await AnnounceScalesAsync(hub, "primary", default);
            });

            // AddScale
            hub.On<JsonElement>("AddScale", async (config) =>
            {
                try
                {
                    using var scope = _services.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<ScaleDbContext>();
                    var entity = JsonSerializer.Deserialize<ScaleConfigEntity>(config.GetRawText(), JsonOpts);
                    if (entity == null)
                    {
                        await hub.InvokeAsync("ScaleCrudResult", new { success = false, message = "Invalid scale data." });
                        return;
                    }
                    entity.Id = 0; // ensure auto-increment
                    db.ScaleConfigs.Add(entity);
                    await db.SaveChangesAsync();
                    _log.LogInformation("Scale added: ScaleId={ScaleId}, Desc={Desc}", entity.Id, entity.Description);
                    await hub.InvokeAsync("ScaleCrudResult", new { success = true, message = $"Scale added: {entity.Description}" });
                    _restartSignal.RequestRestart();
                }
                catch (Exception ex)
                {
                    _log.LogError(ex, "AddScale failed.");
                    await hub.InvokeAsync("ScaleCrudResult", new { success = false, message = ex.Message });
                }
            });

            // UpdateScale config
            hub.On<int, JsonElement>("UpdateScaleConfig", async (scaleId, config) =>
            {
                try
                {
                    using var scope = _services.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<ScaleDbContext>();
                    var existing = db.ScaleConfigs.FirstOrDefault(s => s.Id == scaleId);
                    if (existing == null)
                    {
                        await hub.InvokeAsync("ScaleCrudResult", new { success = false, message = $"Scale {scaleId} not found." });
                        return;
                    }
                    var update = JsonSerializer.Deserialize<ScaleConfigEntity>(config.GetRawText(), JsonOpts);
                    if (update != null)
                    {
                        existing.Description = update.Description ?? existing.Description;
                        existing.IpAddress = update.IpAddress ?? existing.IpAddress;
                        existing.Brand = update.Brand ?? existing.Brand;
                        existing.RequestCommand = update.RequestCommand ?? existing.RequestCommand;
                        existing.Encoding = update.Encoding ?? existing.Encoding;
                        if (update.Port > 0) existing.Port = update.Port;
                        existing.LocationId = update.LocationId;
                        existing.LocationDescription = update.LocationDescription ?? existing.LocationDescription;
                        existing.Enabled = update.Enabled;
                    }
                    await db.SaveChangesAsync();
                    _log.LogInformation("Scale updated: ScaleId={ScaleId}", scaleId);
                    await hub.InvokeAsync("ScaleCrudResult", new { success = true, message = $"Scale updated: {existing.Description}" });
                    _restartSignal.RequestRestart();
                }
                catch (Exception ex)
                {
                    _log.LogError(ex, "UpdateScaleConfig failed for ScaleId={ScaleId}.", scaleId);
                    await hub.InvokeAsync("ScaleCrudResult", new { success = false, message = ex.Message });
                }
            });

            // DeleteScale
            hub.On<int>("DeleteScale", async (scaleId) =>
            {
                try
                {
                    using var scope = _services.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<ScaleDbContext>();
                    var existing = db.ScaleConfigs.FirstOrDefault(s => s.Id == scaleId);
                    if (existing == null)
                    {
                        await hub.InvokeAsync("ScaleCrudResult", new { success = false, message = $"Scale {scaleId} not found." });
                        return;
                    }
                    db.ScaleConfigs.Remove(existing);
                    await db.SaveChangesAsync();
                    _log.LogInformation("Scale deleted: ScaleId={ScaleId}", scaleId);
                    await hub.InvokeAsync("ScaleCrudResult", new { success = true, message = $"Scale deleted: {existing.Description}" });
                    _restartSignal.RequestRestart();
                }
                catch (Exception ex)
                {
                    _log.LogError(ex, "DeleteScale failed for ScaleId={ScaleId}.", scaleId);
                    await hub.InvokeAsync("ScaleCrudResult", new { success = false, message = ex.Message });
                }
            });
        }

        // ── Scale Polling ─────────────────────────────────────────────────────

        private async Task PollScaleAsync(ScaleConfigEntity config, List<HubConnection> hubs, CancellationToken ct)
        {
            var backoff = _settings.ReconnectBackoffMs;
            var wasDown = false;

            while (!ct.IsCancellationRequested)
            {
                try
                {
                    var (ok, weight, motion, status, rawResponse) =
                        await _smaClient.QueryOnceAsync(config, _settings.TimeoutMs, ct);

                    if (wasDown)
                    {
                        _log.LogInformation("Scale '{Desc}' reconnected. Weight={Weight} Status={Status}",
                            config.Description, weight, status);
                        wasDown = false;
                    }

                    var dto = new
                    {
                        Id = config.Id,
                        Description = config.Description,
                        Weight = weight,
                        Ok = ok,
                        Motion = motion,
                        Status = status,
                        RawResponse = rawResponse,
                        LastUpdate = DateTime.Now,
                        LocationId = config.LocationId,
                        LocationDescription = config.LocationDescription,
                        ServiceId = _settings.ServiceId
                    };

                    // Push to all connected hubs
                    foreach (var hub in hubs)
                    {
                        if (hub.State == HubConnectionState.Connected)
                        {
                            try
                            {
                                await hub.InvokeAsync("UpdateScale", dto, ct);
                            }
                            catch (Exception ex)
                            {
                                _log.LogWarning(ex, "Failed to push update for scale '{Desc}' to hub.",
                                    config.Description);
                            }
                        }
                    }

                    await Task.Delay(_settings.PollIntervalMs, ct);
                    backoff = _settings.ReconnectBackoffMs;
                }
                catch (OperationCanceledException) when (ct.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception ex)
                {
                    wasDown = true;
                    _log.LogWarning(ex, "Scale '{Desc}' poll failed. Backing off {Backoff}ms.",
                        config.Description, backoff);
                    await Task.Delay(backoff, ct);
                    backoff = Math.Min(backoff * 2, _settings.MaxBackoffMs);
                }
            }
        }

        // ── Announce Scales ───────────────────────────────────────────────────

        private async Task AnnounceScalesAsync(HubConnection hub, string hubUrl, CancellationToken ct)
        {
            if (hub.State != HubConnectionState.Connected) return;

            try
            {
                var scales = LoadScaleConfigs();
                var configs = scales.Select(s => new
                {
                    s.Id,
                    s.Description,
                    s.IpAddress,
                    s.Port,
                    s.Brand,
                    s.RequestCommand,
                    s.Encoding,
                    s.LocationId,
                    s.LocationDescription,
                    s.Enabled
                }).ToList();

                await hub.InvokeAsync("AnnounceScales", _settings.ServiceId,
                    _settings.LocationId, _settings.LocationDescription, configs, ct);

                _log.LogInformation("Announced {Count} scales for service '{ServiceId}' on {HubUrl}.",
                    configs.Count, _settings.ServiceId, hubUrl);
            }
            catch (Exception ex)
            {
                _log.LogWarning(ex, "Failed to announce scales on {HubUrl}.", hubUrl);
            }
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        private async Task ConnectWithRetryAsync(HubConnection hub, string hubUrl, CancellationToken ct)
        {
            var delay = 1000;
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    await hub.StartAsync(ct);
                    return;
                }
                catch (OperationCanceledException) when (ct.IsCancellationRequested) { throw; }
                catch (Exception ex)
                {
                    _log.LogWarning(ex, "SignalR connection to {HubUrl} failed. Retrying in {Delay}ms.", hubUrl, delay);
                    await Task.Delay(delay, ct);
                    delay = Math.Min(delay * 2, _settings.MaxBackoffMs);
                }
            }
            ct.ThrowIfCancellationRequested();
        }

        private List<ScaleConfigEntity> LoadScaleConfigs()
        {
            using var scope = _services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ScaleDbContext>();
            db.Database.EnsureCreated();
            return db.ScaleConfigs.Where(s => s.Enabled).ToList();
        }
    }

    /// <summary>
    /// SignalR retry policy that retries forever with exponential backoff
    /// capped at 30s. The exponent itself is capped so the inner
    /// <c>Math.Pow(2, n)</c> can never produce a value larger than
    /// <see cref="TimeSpan"/> can represent — the previous version overflowed
    /// after ~40 retries (about 20 min of disconnect with the 30s cap),
    /// which threw <see cref="OverflowException"/> from inside
    /// <see cref="NextRetryDelay"/> and made SignalR park the connection
    /// in Disconnected. Now NextRetryDelay always returns a value ≤
    /// <see cref="MaxDelay"/> and the retry loop genuinely never gives up.
    ///
    /// Each call also logs a warning so a long outage produces visible
    /// heartbeat-style "still down, retrying in Xs" lines instead of one
    /// warning and then silence.
    /// </summary>
    public sealed class ForeverRetryPolicy : IRetryPolicy
    {
        private static readonly TimeSpan MaxDelay = TimeSpan.FromSeconds(30);
        private const int MaxExponent = 5; // 2^5 = 32s, already > MaxDelay

        private readonly ILogger _log;
        private readonly string _hubUrl;

        public ForeverRetryPolicy(ILogger log, string hubUrl)
        {
            _log = log;
            _hubUrl = hubUrl;
        }

        public TimeSpan? NextRetryDelay(RetryContext retryContext)
        {
            // PreviousRetryCount is a long; cast after capping so the
            // exponent never overflows TimeSpan on long-running outages.
            long n = Math.Min(retryContext.PreviousRetryCount, (long)MaxExponent);
            double seconds = Math.Min(Math.Pow(2, n), MaxDelay.TotalSeconds);
            var delay = TimeSpan.FromSeconds(seconds);

            // Per-attempt visibility — the SignalR client only fires
            // `Reconnecting` once when the connection drops, so without
            // this log a multi-hour outage would otherwise be silent.
            _log.LogWarning(
                "SignalR still disconnected from {HubUrl} (retry #{Count}, " +
                "down for {Elapsed}, last error: {Error}). Trying again in {Delay}s.",
                _hubUrl,
                retryContext.PreviousRetryCount + 1,
                retryContext.ElapsedTime,
                retryContext.RetryReason?.Message ?? "(none)",
                (int)delay.TotalSeconds);

            return delay;
        }
    }
}
