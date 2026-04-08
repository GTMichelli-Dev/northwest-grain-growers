#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using GrainManagement.Services.Camera.Data;
using GrainManagement.Services.Camera.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GrainManagement.Services.Camera;

/// <summary>
/// Background worker that connects as a SignalR CLIENT to the GrainManagement ScaleHub
/// and listens for camera capture commands.
/// </summary>
public sealed class CameraWorker : BackgroundService
{
    private readonly ILogger<CameraWorker> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly RestartSignal _restartSignal;
    private readonly AnnounceSignal _announceSignal;
    private HubConnection? _hub;

    public CameraWorker(
        ILogger<CameraWorker> logger,
        IServiceScopeFactory scopeFactory,
        RestartSignal restartSignal,
        AnnounceSignal announceSignal)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _restartSignal = restartSignal;
        _announceSignal = announceSignal;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var settings = await LoadSettingsAsync();
                if (settings == null)
                {
                    _logger.LogWarning("No ServiceSettings found in database. Retrying in 10 seconds...");
                    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                    continue;
                }

                var hubUrl = $"{settings.ServerUrl.TrimEnd('/')}{settings.SignalRHub}";
                _logger.LogInformation("Connecting to SignalR hub at {HubUrl}", hubUrl);

                _hub = new HubConnectionBuilder()
                    .WithUrl(hubUrl)
                    .WithAutomaticReconnect(new ForeverRetryPolicy())
                    .Build();

                RegisterHandlers(_hub, settings);

                _hub.Closed += async (ex) =>
                {
                    _logger.LogWarning(ex, "SignalR connection closed. Will reconnect...");
                    await Task.CompletedTask;
                };

                _hub.Reconnecting += async (ex) =>
                {
                    _logger.LogInformation(ex, "SignalR reconnecting...");
                    await Task.CompletedTask;
                };

                _hub.Reconnected += async (connectionId) =>
                {
                    _logger.LogInformation("SignalR reconnected with connectionId {ConnectionId}", connectionId);
                    await Task.CompletedTask;
                };

                await _hub.StartAsync(stoppingToken);
                _logger.LogInformation("Connected to SignalR hub. ConnectionId: {ConnectionId}", _hub.ConnectionId);

                // Signal that we are ready
                _announceSignal.Signal();

                // Wait for restart signal or cancellation
                await WaitForRestartOrCancellationAsync(stoppingToken);

                // Dispose connection before restarting
                await _hub.DisposeAsync();
                _hub = null;
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CameraWorker. Retrying in 10 seconds...");
                if (_hub != null)
                {
                    try { await _hub.DisposeAsync(); } catch { /* ignore */ }
                    _hub = null;
                }
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }

    private void RegisterHandlers(HubConnection hub, ServiceSettings settings)
    {
        hub.On<CaptureCommand>("CaptureImage", async (cmd) =>
        {
            _logger.LogInformation(
                "Received CaptureImage command: ScaleId={ScaleId}, TicketNumber={TicketNumber}",
                cmd.ScaleId, cmd.TicketNumber);

            try
            {
                using var scope = _scopeFactory.CreateScope();
                var captureService = scope.ServiceProvider.GetRequiredService<CameraCaptureService>();
                await captureService.CaptureAsync(cmd, settings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to capture image for ScaleId={ScaleId}", cmd.ScaleId);
            }
        });
    }

    private async Task WaitForRestartOrCancellationAsync(CancellationToken stoppingToken)
    {
        var restartTask = _restartSignal.WaitAsync(stoppingToken);
        await restartTask;
        _logger.LogInformation("Restart signal received. Reconnecting...");
    }

    private async Task<ServiceSettings?> LoadSettingsAsync()
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CameraDbContext>();
        await db.Database.EnsureCreatedAsync();
        return await db.ServiceSettings.FirstOrDefaultAsync();
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("CameraWorker stopping...");
        if (_hub != null)
        {
            try { await _hub.DisposeAsync(); } catch { /* ignore */ }
        }
        await base.StopAsync(cancellationToken);
    }
}

/// <summary>
/// Retry policy that retries forever with increasing delay (capped at 60 seconds).
/// </summary>
public sealed class ForeverRetryPolicy : IRetryPolicy
{
    private static readonly TimeSpan[] _retryDelays = new[]
    {
        TimeSpan.FromSeconds(0),
        TimeSpan.FromSeconds(2),
        TimeSpan.FromSeconds(5),
        TimeSpan.FromSeconds(10),
        TimeSpan.FromSeconds(30),
        TimeSpan.FromSeconds(60),
    };

    public TimeSpan? NextRetryDelay(RetryContext retryContext)
    {
        var index = Math.Min(retryContext.PreviousRetryCount, _retryDelays.Length - 1);
        return _retryDelays[index];
    }
}

/// <summary>
/// Command received from the SignalR hub to trigger a camera capture.
/// </summary>
public sealed class CaptureCommand
{
    public int ScaleId { get; set; }
    public string TicketNumber { get; set; } = "";
    public int? PresetNumber { get; set; }
    public string? OutputFolder { get; set; }
    public string? FileName { get; set; }
}
