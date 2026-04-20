using CameraService.Data;
using CameraService.Models;
using CameraService.Services;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CameraService;

public class CameraWorker : BackgroundService
{
    private readonly ILogger<CameraWorker> _logger;
    private readonly CameraOptions _options;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IServiceProvider _serviceProvider;
    private readonly RestartSignal _restart;
    private readonly AnnounceSignal _announce;
    private HubConnection? _connection;
    private string _serviceId = "default";

    public CameraWorker(
        ILogger<CameraWorker> logger,
        IOptions<CameraOptions> options,
        IHttpClientFactory httpClientFactory,
        List<CameraBrandDefinition> brands,
        IServiceProvider serviceProvider,
        RestartSignal restart,
        AnnounceSignal announce)
    {
        _logger = logger;
        _options = options.Value;
        _options.SetBrands(brands);
        _httpClientFactory = httpClientFactory;
        _serviceProvider = serviceProvider;
        _restart = restart;
        _announce = announce;

        // Subscribe to announce signal — re-announce cameras when CRUD changes happen
        _announce.OnAnnounceRequested += () => _ = AnnounceCameras();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Initial delay to let the web host start up
        await Task.Delay(1000, stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Load settings from local database
                await LoadSettingsFromDb();

                // Create a linked token that cancels on either app shutdown or restart signal
                using var linked = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken, _restart.Token);

                await ConnectAndListen(linked.Token);
            }
            catch (OperationCanceledException) when (!stoppingToken.IsCancellationRequested)
            {
                // Restart signal was triggered — disconnect and reconnect with new settings
                _logger.LogInformation("Settings changed. Restarting connection...");
                try
                {
                    if (_connection != null)
                    {
                        await _connection.DisposeAsync();
                        _connection = null;
                    }
                }
                catch { /* ignore dispose errors */ }
            }
            catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogWarning("Connection failed: {Message}. Retrying in 5 seconds...", ex.Message);
                try
                {
                    if (_connection != null)
                    {
                        await _connection.DisposeAsync();
                        _connection = null;
                    }
                }
                catch { /* ignore dispose errors */ }

                try
                {
                    await Task.Delay(5000, stoppingToken);
                }
                catch (OperationCanceledException) { break; }
            }
        }
    }

    private async Task LoadSettingsFromDb()
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<CameraDbContext>();
            var settings = await db.Settings.OrderBy(s => s.Id).FirstOrDefaultAsync();
            if (settings != null)
            {
                _options.ServerUrl = settings.ServerUrl;

                // Try to update brands from remote (non-blocking, uses local as fallback)
                if (!string.IsNullOrEmpty(settings.BrandsUrl))
                {
                    _options.BrandsUrl = settings.BrandsUrl;
                    _options.BrandsToken = settings.BrandsToken ?? "";
                    var localPath = Path.Combine(AppContext.BaseDirectory, "camera-snapshot.json");
                    var updated = await CameraOptions.UpdateBrandsFromRemoteAsync(
                        settings.BrandsUrl, localPath, settings.BrandsToken, _logger);
                    if (updated != null)
                        _options.SetBrands(updated);
                }

                _serviceId = settings.ServiceId ?? "default";
                _logger.LogInformation("Loaded settings from database: ServiceId={ServiceId}, ServerUrl={Url}", _serviceId, settings.ServerUrl);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to load settings from database. Using current settings.");
        }
    }

    private async Task ConnectAndListen(CancellationToken stoppingToken)
    {
        // Read hub path from DB settings
        string hubPath = "/scaleHub";
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<CameraDbContext>();
            var settings = await db.Settings.AsNoTracking().FirstOrDefaultAsync();
            if (settings != null)
                hubPath = settings.SignalRHub;
        }
        catch { /* use default */ }

        var hubUrl = _options.ServerUrl.TrimEnd('/') + hubPath;
        _logger.LogInformation("Connecting to {HubUrl}", hubUrl);

        _connection = new HubConnectionBuilder()
            .WithUrl(hubUrl)
            .WithAutomaticReconnect(new ForeverRetryPolicy())
            .Build();

        _connection.Reconnecting += error =>
        {
            _logger.LogWarning("Connection lost. Reconnecting...");
            return Task.CompletedTask;
        };

        _connection.Reconnected += async connectionId =>
        {
            _logger.LogInformation("Reconnected. Rejoining camera groups...");
            await _connection.InvokeAsync("JoinCameraGroup", _serviceId, stoppingToken);

            // Re-announce cameras after reconnect
            await AnnounceCameras();
        };

        _connection.Closed += error =>
        {
            if (error != null)
                _logger.LogError(error, "Connection closed with error");
            return Task.CompletedTask;
        };

        // Listen for capture commands
        _connection.On<CaptureCommand>("CaptureImage", (cmd) =>
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    _logger.LogInformation("Received CaptureImage: ticket={Ticket}, direction={Direction}, cameraId={CameraId}",
                        cmd.Ticket, cmd.Direction, cmd.CameraId ?? "default");
                    await CaptureAndUpload(cmd.Ticket, cmd.Direction, cmd.CameraId);
                }
                catch (Exception ex) { _logger.LogError(ex, "CaptureImage failed."); }
            });
        });

        // Listen for config reload
        _connection.On("ReloadConfig", () =>
        {
            _logger.LogInformation("Received ReloadConfig command. Restarting with new settings...");
            _restart.TriggerRestart();
        });

        // Listen for camera list request from web UI
        _connection.On("GetCameraList", () =>
        {
            _ = Task.Run(async () =>
            {
                _logger.LogInformation("Received GetCameraList request.");
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<CameraDbContext>();
                    var cameras = await db.Cameras.AsNoTracking().OrderBy(c => c.CameraId).ToListAsync();
                    await _connection.SendAsync("CameraListResponse", new
                    {
                        serviceId = _serviceId,
                        cameras
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to get camera list.");
                }
            });
        });

        // Listen for camera brands request from web UI
        _connection.On("GetCameraBrands", () =>
        {
            _ = Task.Run(async () =>
            {
                _logger.LogInformation("Received GetCameraBrands request.");
                try
                {
                    var brands = _options.GetBrands();
                    await _connection.SendAsync("CameraBrandsResponse", brands);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to get camera brands.");
                }
            });
        });

        // Listen for AddCamera CRUD command from web UI
        _connection.On<CameraConfigEntity>("AddCamera", (config) =>
        {
            _ = Task.Run(async () =>
            {
                _logger.LogInformation("Received AddCamera: {CameraId}", config.CameraId);
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<CameraDbContext>();
                    if (await db.Cameras.AsNoTracking().AnyAsync(c => c.CameraId == config.CameraId))
                    {
                        await _connection.SendAsync("CameraCrudResult", new
                        {
                            success = false, operation = "add", cameraId = config.CameraId,
                            serviceId = _serviceId, error = $"Camera '{config.CameraId}' already exists."
                        });
                        return;
                    }
                    config.Id = 0;
                    db.Cameras.Add(config);
                    await db.SaveChangesAsync();
                    _announce.TriggerAnnounce();
                    await _connection.SendAsync("CameraCrudResult", new
                    {
                        success = true, operation = "add", cameraId = config.CameraId, serviceId = _serviceId
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to add camera.");
                    try { await _connection.SendAsync("CameraCrudResult", new
                    {
                        success = false, operation = "add", cameraId = config.CameraId,
                        serviceId = _serviceId, error = ex.Message
                    }); } catch { }
                }
            });
        });

        // Listen for UpdateCamera CRUD command from web UI
        _connection.On<string, CameraConfigEntity>("UpdateCamera", (cameraId, config) =>
        {
            _ = Task.Run(async () =>
            {
                _logger.LogInformation("Received UpdateCamera: {CameraId}", cameraId);
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<CameraDbContext>();
                    var existing = await db.Cameras.FirstOrDefaultAsync(c => c.CameraId == cameraId);
                    if (existing == null)
                    {
                        await _connection.SendAsync("CameraCrudResult", new
                        {
                            success = false, operation = "update", cameraId,
                            serviceId = _serviceId, error = $"Camera '{cameraId}' not found."
                        });
                        return;
                    }
                    existing.DisplayName = config.DisplayName;
                    existing.CameraBrand = config.CameraBrand;
                    existing.CameraIp = config.CameraIp;
                    existing.CameraUser = config.CameraUser;
                    existing.CameraPassword = config.CameraPassword;
                    existing.UsbDeviceName = config.UsbDeviceName;
                    existing.CameraUrl = config.CameraUrl;
                    existing.UsbCommand = config.UsbCommand;
                    existing.TimeoutSeconds = config.TimeoutSeconds;
                    existing.Active = config.Active;
                    existing.IsDefault = config.IsDefault;
                    await db.SaveChangesAsync();
                    _announce.TriggerAnnounce();
                    await _connection.SendAsync("CameraCrudResult", new
                    {
                        success = true, operation = "update", cameraId, serviceId = _serviceId
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to update camera.");
                    try { await _connection.SendAsync("CameraCrudResult", new
                    {
                        success = false, operation = "update", cameraId,
                        serviceId = _serviceId, error = ex.Message
                    }); } catch { }
                }
            });
        });

        // Listen for DeleteCamera CRUD command from web UI
        _connection.On<string>("DeleteCamera", (cameraId) =>
        {
            _ = Task.Run(async () =>
            {
                _logger.LogInformation("Received DeleteCamera: {CameraId}", cameraId);
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<CameraDbContext>();
                    var camera = await db.Cameras.FirstOrDefaultAsync(c => c.CameraId == cameraId);
                    if (camera == null)
                    {
                        await _connection.SendAsync("CameraCrudResult", new
                        {
                            success = false, operation = "delete", cameraId,
                            serviceId = _serviceId, error = $"Camera '{cameraId}' not found."
                        });
                        return;
                    }
                    db.Cameras.Remove(camera);
                    await db.SaveChangesAsync();
                    _announce.TriggerAnnounce();
                    await _connection.SendAsync("CameraCrudResult", new
                    {
                        success = true, operation = "delete", cameraId, serviceId = _serviceId
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to delete camera.");
                    try { await _connection.SendAsync("CameraCrudResult", new
                    {
                        success = false, operation = "delete", cameraId,
                        serviceId = _serviceId, error = ex.Message
                    }); } catch { }
                }
            });
        });

        // Listen for TestCapture command from web UI
        // Use Task.Run to prevent async exceptions from killing the SignalR connection
        _connection.On<string>("TestCapture", (cameraId) =>
        {
            _ = Task.Run(async () =>
            {
                _logger.LogInformation("Received TestCapture for camera: {CameraId}", cameraId);
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var capture = scope.ServiceProvider.GetRequiredService<CameraCaptureService>();
                    _logger.LogInformation("Starting capture for camera: {CameraId}", cameraId);
                    var imageBytes = await capture.CaptureAsync(cameraId);
                    _logger.LogInformation("Capture succeeded, {Bytes} bytes. Sending result.", imageBytes.Length);
                    var base64 = Convert.ToBase64String(imageBytes);
                    await _connection.SendAsync("TestCaptureResult", new
                    {
                        success = true, cameraId, serviceId = _serviceId,
                        image = base64, contentType = "image/jpeg"
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Test capture failed for camera {CameraId}.", cameraId);
                    try
                    {
                        await _connection.SendAsync("TestCaptureResult", new
                        {
                            success = false, cameraId, serviceId = _serviceId, error = ex.Message
                        });
                    }
                    catch (Exception sendEx)
                    {
                        _logger.LogError(sendEx, "Failed to send test capture error result.");
                    }
                }
            });
        });

        await _connection.StartAsync(stoppingToken);
        _logger.LogInformation("Connected to server. Joining camera groups (ServiceId={ServiceId})...", _serviceId);

        await _connection.InvokeAsync("JoinCameraGroup", _serviceId, stoppingToken);
        _logger.LogInformation("Joined camera groups. Waiting for capture commands...");

        // Announce cameras to web clients
        await AnnounceCameras();

        // Keep alive
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private async Task CaptureAndUpload(string ticket, string direction, string? cameraId = null)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var capture = scope.ServiceProvider.GetRequiredService<CameraCaptureService>();
            var success = await capture.CaptureAndUploadAsync(ticket, direction, cameraId);
            if (!success)
                _logger.LogWarning("Capture/upload failed for ticket {Ticket} ({Direction})", ticket, direction);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error capturing image for ticket {Ticket}", ticket);
        }
    }

    private async Task AnnounceCameras()
    {
        if (_connection == null || _connection.State != HubConnectionState.Connected)
            return;

        try
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<CameraDbContext>();
            var cameras = await db.Cameras.AsNoTracking().Where(c => c.Active).OrderBy(c => c.CameraId).ToListAsync();
            await _connection.SendAsync("CameraServiceReady", new
            {
                serviceId = _serviceId,
                cameraCount = cameras.Count,
                cameras
            });
            _logger.LogInformation("Announced {Count} camera(s) to web app.", cameras.Count);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to announce cameras.");
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        try
        {
            if (_connection != null)
                await _connection.DisposeAsync();
        }
        catch { /* ignore */ }
        await base.StopAsync(cancellationToken);
    }
}

/// <summary>
/// Retry policy that never stops trying — backs off to 30s max.
/// </summary>
public class ForeverRetryPolicy : IRetryPolicy
{
    public TimeSpan? NextRetryDelay(RetryContext retryContext)
    {
        var delays = new[] { 2, 5, 10, 30 };
        var index = Math.Min(retryContext.PreviousRetryCount, delays.Length - 1);
        return TimeSpan.FromSeconds(delays[index]);
    }
}

public class CaptureCommand
{
    public string Ticket { get; set; } = string.Empty;
    public string Direction { get; set; } = string.Empty;
    public string? CameraId { get; set; }
}
