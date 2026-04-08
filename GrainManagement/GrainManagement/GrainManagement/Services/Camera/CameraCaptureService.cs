#nullable enable
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GrainManagement.Services.Camera.Data;
using GrainManagement.Services.Camera.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GrainManagement.Services.Camera;

/// <summary>
/// Performs camera capture operations: moves camera to preset, captures a snapshot,
/// and saves or uploads the image.
/// </summary>
public sealed class CameraCaptureService
{
    private readonly ILogger<CameraCaptureService> _logger;
    private readonly CameraDbContext _db;

    public CameraCaptureService(ILogger<CameraCaptureService> logger, CameraDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    /// <summary>
    /// Captures an image from the camera associated with the given scale.
    /// </summary>
    public async Task CaptureAsync(CaptureCommand cmd, ServiceSettings settings, CancellationToken ct = default)
    {
        var cameraConfig = await _db.CameraConfigs
            .FirstOrDefaultAsync(c => c.ScaleId == cmd.ScaleId && c.IsEnabled, ct);

        if (cameraConfig == null)
        {
            _logger.LogWarning("No enabled camera config found for ScaleId={ScaleId}", cmd.ScaleId);
            return;
        }

        _logger.LogInformation(
            "Capturing image for ScaleId={ScaleId}, Camera={CameraIp}, Brand={Brand}",
            cmd.ScaleId, cameraConfig.IpAddress, cameraConfig.Brand);

        var opts = new CameraOptions
        {
            HostOrIp = cameraConfig.IpAddress,
            Username = cameraConfig.Username,
            Password = cameraConfig.Password,
            UseHttps = cameraConfig.UseHttps,
            Port = cameraConfig.Port,
            Channel = cameraConfig.Channel
        };

        var brand = ResolveBrand(cameraConfig.Brand);

        // Determine preset
        int preset = cmd.PresetNumber ?? cameraConfig.DefaultPreset;

        // Determine output path
        var outputFolder = cmd.OutputFolder ?? settings.DefaultImageOutputPath ?? Path.Combine(Directory.GetCurrentDirectory(), "camera-images");
        Directory.CreateDirectory(outputFolder);

        var fileName = cmd.FileName ?? $"{cmd.TicketNumber}_{DateTime.UtcNow:yyyyMMdd_HHmmss}";
        var outputPath = Path.Combine(outputFolder, $"{fileName}.png");

        try
        {
            // Move to preset if specified
            if (preset > 0)
            {
                await MoveToPresetAsync(brand, opts, preset, ct);
                await Task.Delay(cameraConfig.PresetDelayMs, ct);
            }

            // Capture snapshot
            await CaptureSnapshotAsync(brand, opts, outputPath, ct);

            _logger.LogInformation("Image captured successfully: {OutputPath}", outputPath);

            // Upload to server if configured
            if (!string.IsNullOrWhiteSpace(settings.ServerUrl) && !string.IsNullOrWhiteSpace(settings.ImageUploadEndpoint))
            {
                await UploadImageAsync(settings, cmd, outputPath, ct);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to capture image for ScaleId={ScaleId}", cmd.ScaleId);
            throw;
        }
    }

    private static CameraBrandDefinition ResolveBrand(string brand)
    {
        return brand?.ToUpperInvariant() switch
        {
            "AXIS" => CameraBrandDefinition.Axis,
            "HIKVISION" or "HIK" => CameraBrandDefinition.Hikvision,
            _ => CameraBrandDefinition.Axis // default
        };
    }

    private async Task MoveToPresetAsync(CameraBrandDefinition brand, CameraOptions opts, int preset, CancellationToken ct)
    {
        using var http = CreateHttpClient(opts);
        var baseUri = BuildBaseUri(opts);

        string url = brand switch
        {
            CameraBrandDefinition.Axis =>
                $"{baseUri}/axis-cgi/com/ptz.cgi?gotoserverpresetno={preset}",
            CameraBrandDefinition.Hikvision =>
                $"{baseUri}/ISAPI/PTZCtrl/channels/1/presets/{preset}/goto",
            _ => throw new NotSupportedException($"Unsupported brand: {brand}")
        };

        using var req = brand == CameraBrandDefinition.Hikvision
            ? new HttpRequestMessage(HttpMethod.Put, url) { Content = new StringContent(string.Empty) }
            : new HttpRequestMessage(HttpMethod.Get, url);

        using var res = await http.SendAsync(req, ct).ConfigureAwait(false);
        res.EnsureSuccessStatusCode();

        _logger.LogDebug("Camera moved to preset {Preset}", preset);
    }

#pragma warning disable CA1416 // Platform compatibility — Image/ImageFormat are Windows-only
    private async Task CaptureSnapshotAsync(CameraBrandDefinition brand, CameraOptions opts, string outputPath, CancellationToken ct)
    {
        using var http = CreateHttpClient(opts);
        var baseUri = BuildBaseUri(opts);
        int channel = opts.Channel ?? 101;

        string url = brand switch
        {
            CameraBrandDefinition.Axis =>
                $"{baseUri}/axis-cgi/jpg/image.cgi",
            CameraBrandDefinition.Hikvision =>
                $"{baseUri}/ISAPI/Streaming/channels/{channel}/picture",
            _ => throw new NotSupportedException($"Unsupported brand: {brand}")
        };

        using var req = new HttpRequestMessage(HttpMethod.Get, url);
        using var res = await http.SendAsync(req, ct).ConfigureAwait(false);
        res.EnsureSuccessStatusCode();

        await using var jpgStream = await res.Content.ReadAsStreamAsync(ct).ConfigureAwait(false);
        using var img = Image.FromStream(jpgStream);
        Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);
        img.Save(outputPath, ImageFormat.Png);
    }

    /// <summary>
    /// Captures a snapshot and returns it as a MemoryStream (PNG).
    /// </summary>
    public async Task<MemoryStream> CaptureToStreamAsync(CameraBrandDefinition brand, CameraOptions opts, CancellationToken ct = default)
    {
        using var http = CreateHttpClient(opts);
        var baseUri = BuildBaseUri(opts);
        int channel = opts.Channel ?? 101;

        string url = brand switch
        {
            CameraBrandDefinition.Axis =>
                $"{baseUri}/axis-cgi/jpg/image.cgi",
            CameraBrandDefinition.Hikvision =>
                $"{baseUri}/ISAPI/Streaming/channels/{channel}/picture",
            _ => throw new NotSupportedException($"Unsupported brand: {brand}")
        };

        using var req = new HttpRequestMessage(HttpMethod.Get, url);
        using var res = await http.SendAsync(req, ct).ConfigureAwait(false);
        res.EnsureSuccessStatusCode();

        await using var jpgStream = await res.Content.ReadAsStreamAsync(ct).ConfigureAwait(false);
        using var img = Image.FromStream(jpgStream);
        var ms = new MemoryStream();
        img.Save(ms, ImageFormat.Png);
        ms.Position = 0;
        return ms;
    }
#pragma warning restore CA1416

    private async Task UploadImageAsync(ServiceSettings settings, CaptureCommand cmd, string imagePath, CancellationToken ct)
    {
        try
        {
            using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
            var uploadUrl = $"{settings.ServerUrl.TrimEnd('/')}{settings.ImageUploadEndpoint}";

            using var content = new MultipartFormDataContent();
            var fileBytes = await File.ReadAllBytesAsync(imagePath, ct);
            content.Add(new ByteArrayContent(fileBytes), "file", Path.GetFileName(imagePath));
            content.Add(new StringContent(cmd.ScaleId.ToString()), "scaleId");
            content.Add(new StringContent(cmd.TicketNumber), "ticketNumber");

            using var res = await http.PostAsync(uploadUrl, content, ct);
            res.EnsureSuccessStatusCode();

            _logger.LogInformation("Image uploaded to {Url}", uploadUrl);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to upload image to server. Image saved locally at {Path}", imagePath);
        }
    }

    private static HttpClient CreateHttpClient(CameraOptions opts)
    {
        var handler = new HttpClientHandler
        {
            Credentials = new System.Net.NetworkCredential(opts.Username, opts.Password),
            PreAuthenticate = false
        };

        if (opts.UseHttps && opts.TrustAllCertificates)
        {
            handler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        }

        return new HttpClient(handler)
        {
            Timeout = TimeSpan.FromSeconds(opts.TimeoutSeconds)
        };
    }

    private static string BuildBaseUri(CameraOptions opts)
    {
        var scheme = opts.UseHttps ? "https" : "http";
        return $"{scheme}://{opts.HostOrIp}:{opts.Port}";
    }
}
