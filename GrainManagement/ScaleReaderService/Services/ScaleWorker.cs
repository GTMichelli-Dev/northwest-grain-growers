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
            _log.LogInformation("ScaleWorker starting. ServiceId={ServiceId}, Hub={HubUrl}",
                _settings.ServiceId, _settings.HubUrl);

            while (!stoppingToken.IsCancellationRequested)
            {
                using var restartCts = CancellationTokenSource.CreateLinkedTokenSource(
                    stoppingToken, _restartSignal.Token);
                var cycleToken = restartCts.Token;

                HubConnection? hub = null;
                try
                {
                    hub = new HubConnectionBuilder()
                        .WithUrl(_settings.HubUrl)
                        .AddJsonProtocol(o =>
                        {
                            o.PayloadSerializerOptions.PropertyNamingPolicy = null;
                        })
                        .WithAutomaticReconnect(new ForeverRetryPolicy())
                        .Build();

                    hub.Closed += ex =>
                    {
                        if (ex != null) _log.LogWarning(ex, "SignalR connection closed with error.");
                        else _log.LogInformation("SignalR connection closed.");
                        return Task.CompletedTask;
                    };

                    hub.Reconnecting += ex =>
                    {
                        _log.LogWarning(ex, "SignalR reconnecting...");
                        return Task.CompletedTask;
                    };

                    hub.Reconnected += connectionId =>
                    {
                        _log.LogInformation("SignalR reconnected. Re-joining group and re-announcing scales.");
                        _ = Task.Run(async () =>
                        {
                            try
                            {
                                await hub.InvokeAsync("JoinScaleGroup", _settings.ServiceId, cycleToken);
                                await AnnounceScalesAsync(hub, cycleToken);
                            }
                            catch (Exception ex)
                            {
                                _log.LogWarning(ex, "Failed to re-join/re-announce after reconnect.");
                            }
                        });
                        return Task.CompletedTask;
                    };

                    // Register CRUD handlers before connecting
                    RegisterCrudHandlers(hub);

                    // Connect
                    await ConnectWithRetryAsync(hub, cycleToken);
                    _log.LogInformation("SignalR connected to {HubUrl}.", _settings.HubUrl);

                    // Join group and announce scales
                    await hub.InvokeAsync("JoinScaleGroup", _settings.ServiceId, cycleToken);
                    await AnnounceScalesAsync(hub, cycleToken);

                    // Load scale configs and start polling
                    var scales = LoadScaleConfigs();

                    if (scales.Count == 0)
                    {
                        _log.LogWarning("No scale configurations found. Waiting for config via SignalR.");
                        try { await Task.Delay(Timeout.Infinite, cycleToken); }
                        catch (OperationCanceledException) { }
                        continue;
                    }

                    _log.LogInformation("Starting {Count} scale pollers.", scales.Count);

                    var tasks = scales
                        .Select(s => Task.Run(() => PollScaleAsync(s, hub, cycleToken), cycleToken))
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
                    if (hub != null)
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
                await AnnounceScalesAsync(hub, default);
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

        private async Task PollScaleAsync(ScaleConfigEntity config, HubConnection hub, CancellationToken ct)
        {
            var backoff = _settings.ReconnectBackoffMs;

            while (!ct.IsCancellationRequested)
            {
                try
                {
                    var (ok, weight, motion, status, rawResponse) =
                        await _smaClient.QueryOnceAsync(config, _settings.TimeoutMs, ct);

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

                    if (hub.State == HubConnectionState.Connected)
                    {
                        try
                        {
                            await hub.InvokeAsync("UpdateScale", dto, ct);
                        }
                        catch (Exception ex)
                        {
                            _log.LogWarning(ex, "Failed to push update for scale '{Desc}'.", config.Description);
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
                    _log.LogWarning(ex, "Scale '{Desc}' poll failed. Backing off {Backoff}ms.",
                        config.Description, backoff);
                    await Task.Delay(backoff, ct);
                    backoff = Math.Min(backoff * 2, _settings.MaxBackoffMs);
                }
            }
        }

        // ── Announce Scales ───────────────────────────────────────────────────

        private async Task AnnounceScalesAsync(HubConnection hub, CancellationToken ct)
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

                _log.LogInformation("Announced {Count} scales for service '{ServiceId}'.",
                    configs.Count, _settings.ServiceId);
            }
            catch (Exception ex)
            {
                _log.LogWarning(ex, "Failed to announce scales.");
            }
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        private async Task ConnectWithRetryAsync(HubConnection hub, CancellationToken ct)
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
                    _log.LogWarning(ex, "SignalR connection failed. Retrying in {Delay}ms.", delay);
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
    /// SignalR retry policy that retries forever with exponential backoff (capped at 30s).
    /// </summary>
    public sealed class ForeverRetryPolicy : IRetryPolicy
    {
        private static readonly TimeSpan MaxDelay = TimeSpan.FromSeconds(30);

        public TimeSpan? NextRetryDelay(RetryContext retryContext)
        {
            var delay = TimeSpan.FromSeconds(Math.Pow(2, retryContext.PreviousRetryCount));
            return delay > MaxDelay ? MaxDelay : delay;
        }
    }
}
