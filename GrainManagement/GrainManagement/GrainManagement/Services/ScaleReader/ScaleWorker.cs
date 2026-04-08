#nullable enable
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using GrainManagement.Dtos.Scales;
using GrainManagement.Services.ScaleReader.Data;
using GrainManagement.Services.ScaleReader.Models;

namespace GrainManagement.Services.ScaleReader
{
    /// <summary>
    /// Background service that polls all configured scale indicators via TCP (SMA protocol)
    /// and pushes weight updates to the GrainManagement ScaleHub via a SignalR client connection.
    /// Uses ForeverRetryPolicy to keep the SignalR connection alive.
    /// </summary>
    public sealed class ScaleWorker : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<ScaleWorker> _log;
        private readonly ServiceSettings _settings;
        private readonly SmaClient _smaClient;
        private readonly ScaleWeightStore _weightStore;
        private readonly RestartSignal _restartSignal;
        private readonly AnnounceSignal _announceSignal;

        public ScaleWorker(
            IServiceProvider services,
            IOptions<ServiceSettings> settings,
            SmaClient smaClient,
            ScaleWeightStore weightStore,
            RestartSignal restartSignal,
            AnnounceSignal announceSignal,
            ILogger<ScaleWorker> log)
        {
            _services = services;
            _settings = settings.Value;
            _smaClient = smaClient;
            _weightStore = weightStore;
            _restartSignal = restartSignal;
            _announceSignal = announceSignal;
            _log = log;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _log.LogInformation("ScaleWorker starting. Hub URL: {HubUrl}", _settings.HubUrl);

            while (!stoppingToken.IsCancellationRequested)
            {
                // Create a linked token so RestartSignal can tear down this cycle
                using var restartCts = CancellationTokenSource.CreateLinkedTokenSource(
                    stoppingToken, _restartSignal.Token);
                var cycleToken = restartCts.Token;

                HubConnection? hub = null;
                try
                {
                    // Build SignalR client connection with forever-retry policy
                    hub = new HubConnectionBuilder()
                        .WithUrl(_settings.HubUrl)
                        .WithAutomaticReconnect(new ForeverRetryPolicy())
                        .Build();

                    hub.Closed += ex =>
                    {
                        if (ex != null)
                            _log.LogWarning(ex, "SignalR connection closed with error.");
                        else
                            _log.LogInformation("SignalR connection closed.");
                        return Task.CompletedTask;
                    };

                    hub.Reconnecting += ex =>
                    {
                        _log.LogWarning(ex, "SignalR reconnecting...");
                        return Task.CompletedTask;
                    };

                    hub.Reconnected += connectionId =>
                    {
                        _log.LogInformation("SignalR reconnected. ConnectionId: {ConnectionId}", connectionId);
                        return Task.CompletedTask;
                    };

                    // Connect to the hub
                    await ConnectWithRetryAsync(hub, cycleToken);
                    _log.LogInformation("SignalR connected to {HubUrl}.", _settings.HubUrl);

                    // Load scale configurations from the local database
                    var scales = LoadScaleConfigs();

                    if (scales.Count == 0)
                    {
                        _log.LogWarning("No scale configurations found. Waiting for restart signal.");
                        try { await Task.Delay(Timeout.Infinite, cycleToken); }
                        catch (OperationCanceledException) { }
                        continue;
                    }

                    _log.LogInformation("Starting {Count} scale pollers.", scales.Count);

                    // One independent worker task per scale, plus an announce listener
                    var tasks = new List<Task>();
                    foreach (var scale in scales)
                    {
                        tasks.Add(Task.Run(() => PollScaleAsync(scale, hub, cycleToken), cycleToken));
                    }
                    tasks.Add(Task.Run(() => AnnounceListenerAsync(hub, cycleToken), cycleToken));

                    await Task.WhenAll(tasks);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                catch (OperationCanceledException)
                {
                    // RestartSignal fired -- loop around and reinitialize
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

        /// <summary>
        /// Continuously polls a single scale indicator and pushes updates via SignalR.
        /// </summary>
        private async Task PollScaleAsync(ScaleConfigEntity config, HubConnection hub, CancellationToken ct)
        {
            var backoff = _settings.ReconnectBackoffMs;

            while (!ct.IsCancellationRequested)
            {
                try
                {
                    var (ok, weight, motion, status) =
                        await _smaClient.QueryOnceAsync(config, _settings.TimeoutMs, ct);

                    var dto = new ScaleDto
                    {
                        Id = config.ScaleId,
                        Description = config.Description,
                        Weight = weight,
                        Ok = ok,
                        Motion = motion,
                        Status = status,
                        LastUpdate = DateTime.Now,
                        LocationId = _settings.LocationId,
                        LocationDescription = _settings.LocationDescription
                    };

                    // Store locally
                    _weightStore.Update(dto);

                    // Push to server via SignalR (invoke the server hub method)
                    if (hub.State == HubConnectionState.Connected)
                    {
                        try
                        {
                            await hub.InvokeAsync("UpdateScale", dto, ct);
                        }
                        catch (Exception ex)
                        {
                            _log.LogWarning(ex, "Failed to push update for scale '{Desc}' via SignalR.", config.Description);
                        }
                    }

                    await Task.Delay(_settings.PollIntervalMs, ct);
                    backoff = _settings.ReconnectBackoffMs; // reset after success
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

        /// <summary>
        /// Listens for AnnounceSignal and re-pushes all stored scale states to SignalR.
        /// </summary>
        private async Task AnnounceListenerAsync(HubConnection hub, CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(Timeout.Infinite, _announceSignal.Token);
                }
                catch (OperationCanceledException) when (ct.IsCancellationRequested)
                {
                    break;
                }
                catch (OperationCanceledException)
                {
                    // AnnounceSignal fired
                }

                if (ct.IsCancellationRequested) break;

                _log.LogInformation("Announce signal received. Re-pushing all scale states.");

                foreach (var dto in _weightStore.GetAll())
                {
                    if (hub.State == HubConnectionState.Connected)
                    {
                        try
                        {
                            await hub.InvokeAsync("UpdateScale", dto, ct);
                        }
                        catch (Exception ex)
                        {
                            _log.LogWarning(ex, "Failed to announce scale {Id}.", dto.Id);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Connects to the SignalR hub with retry logic (forever retry until cancelled).
        /// </summary>
        private async Task ConnectWithRetryAsync(HubConnection hub, CancellationToken ct)
        {
            var delay = 1000;
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    await hub.StartAsync(ct);
                    return; // connected
                }
                catch (OperationCanceledException) when (ct.IsCancellationRequested)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    _log.LogWarning(ex, "SignalR connection failed. Retrying in {Delay}ms.", delay);
                    await Task.Delay(delay, ct);
                    delay = Math.Min(delay * 2, _settings.MaxBackoffMs);
                }
            }

            ct.ThrowIfCancellationRequested();
        }

        /// <summary>
        /// Loads enabled scale configurations from the local SQLite database.
        /// </summary>
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
    /// Used with HubConnectionBuilder.WithAutomaticReconnect().
    /// </summary>
    public sealed class ForeverRetryPolicy : IRetryPolicy
    {
        private static readonly TimeSpan MaxDelay = TimeSpan.FromSeconds(30);

        public TimeSpan? NextRetryDelay(RetryContext retryContext)
        {
            // Exponential backoff: 0s, 1s, 2s, 4s, 8s, 16s, 30s, 30s, ...
            var delay = TimeSpan.FromSeconds(Math.Pow(2, retryContext.PreviousRetryCount));
            return delay > MaxDelay ? MaxDelay : delay;
        }
    }
}
