using System.Diagnostics;
using System.Net.Http.Headers;
using CameraService.Data;
using CameraService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CameraService.Services;

/// <summary>
/// Shared camera capture logic used by both the background worker and the API controller.
/// </summary>
public class CameraCaptureService
{
    private readonly CameraOptions _options;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<CameraCaptureService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly WatermarkService _watermark;

    public CameraCaptureService(
        IOptions<CameraOptions> options,
        IHttpClientFactory httpClientFactory,
        ILogger<CameraCaptureService> logger,
        IServiceProvider serviceProvider,
        WatermarkService watermark)
    {
        _options = options.Value;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _serviceProvider = serviceProvider;
        _watermark = watermark;
    }

    /// <summary>
    /// Captures a snapshot from a specific camera by cameraId (string).
    /// Falls back to default camera if cameraId is null.
    /// </summary>
    public async Task<byte[]> CaptureAsync(string? cameraId = null)
    {
        var camera = await ResolveCamera(cameraId);
        if (camera == null)
            throw new InvalidOperationException($"Camera '{cameraId ?? "default"}' not found.");

        return await CaptureFromConfigAsync(camera);
    }

    /// <summary>
    /// Captures a snapshot from a specific camera by database Id (int).
    /// </summary>
    public async Task<byte[]> CaptureByIdAsync(int id)
    {
        using var scope = _serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CameraDbContext>();
        var camera = await db.Cameras.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        if (camera == null)
            throw new InvalidOperationException($"Camera with Id {id} not found.");

        return await CaptureFromConfigAsync(camera);
    }

    /// <summary>
    /// Captures a snapshot and uploads it to the web API.
    /// </summary>
    public async Task<bool> CaptureAndUploadAsync(string ticket, string direction, string? cameraId = null)
    {
        try
        {
            var imageBytes = await CaptureAsync(cameraId);

            if (imageBytes.Length == 0)
            {
                _logger.LogWarning("Empty image captured for ticket {Ticket}", ticket);
                return false;
            }

            // Burn the audit banner into the JPEG before upload — load #,
            // direction, and the instant the photo was taken. The service
            // returns the original bytes if anything goes wrong (no font on
            // the host, decode failure, etc.) so a watermark issue never
            // blocks the capture pipeline.
            imageBytes = _watermark.Apply(imageBytes, ticket, direction, DateTime.Now);

            var client = _httpClientFactory.CreateClient("BasicWeighApi");
            using var content = new MultipartFormDataContent();
            var fileContent = new ByteArrayContent(imageBytes);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            content.Add(fileContent, "file", $"{ticket}_{direction}.jpg");

            // Identify ourselves so the web saves a per-camera file
            // ({ticket}_{direction}__{cameraId}.jpg). Sites with multiple
            // cameras at one scale need this — otherwise every camera
            // overwrites the same canonical file and only the last one
            // survives. Single-camera deployments still work without it.
            var uploadPath = string.IsNullOrWhiteSpace(cameraId)
                ? $"api/ticket/{ticket}/image?direction={direction}"
                : $"api/ticket/{ticket}/image?direction={direction}&cameraId={Uri.EscapeDataString(cameraId)}";

            var response = await client.PostAsync(uploadPath, content);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Image uploaded for ticket {Ticket} ({Direction})", ticket, direction);
                return true;
            }

            _logger.LogWarning("Failed to upload image for ticket {Ticket}: {Status}",
                ticket, response.StatusCode);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error capturing image for ticket {Ticket}", ticket);
            return false;
        }
    }

    /// <summary>
    /// Resolves a camera from the database by cameraId string, or returns the default.
    /// </summary>
    private async Task<CameraConfigEntity?> ResolveCamera(string? cameraId)
    {
        using var scope = _serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CameraDbContext>();

        if (!string.IsNullOrEmpty(cameraId))
        {
            return await db.Cameras.AsNoTracking()
                .FirstOrDefaultAsync(c => c.CameraId == cameraId && c.Active);
        }

        // Default camera
        return await db.Cameras.AsNoTracking()
            .FirstOrDefaultAsync(c => c.IsDefault && c.Active)
            ?? await db.Cameras.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Active);
    }

    /// <summary>
    /// Captures from a specific camera config entity.
    /// </summary>
    private async Task<byte[]> CaptureFromConfigAsync(CameraConfigEntity camera)
    {
        var isUsb = IsUsbCamera(camera);
        if (isUsb)
            return await CaptureUsbAsync(camera);
        else
            return await CaptureIpAsync(camera);
    }

    private bool IsUsbCamera(CameraConfigEntity camera)
    {
        var brand = _options.GetBrands()
            .FirstOrDefault(b => b.Brand.Equals(camera.CameraBrand, StringComparison.OrdinalIgnoreCase));
        if (brand != null)
            return brand.Type.Equals("USB", StringComparison.OrdinalIgnoreCase);
        return camera.CameraBrand?.Equals("Custom", StringComparison.OrdinalIgnoreCase) == true
            && !string.IsNullOrEmpty(camera.UsbCommand);
    }

    private string GetSnapshotUrl(CameraConfigEntity camera)
    {
        var brand = _options.GetBrands()
            .FirstOrDefault(b => b.Brand.Equals(camera.CameraBrand, StringComparison.OrdinalIgnoreCase));
        if (brand != null && !string.IsNullOrEmpty(brand.SnapshotUrlTemplate))
        {
            // Replace IP and credentials (some brands like Reolink/Foscam use query param auth)
            return brand.SnapshotUrlTemplate
                .Replace("{ip}", camera.CameraIp ?? "")
                .Replace("{user}", Uri.EscapeDataString(camera.CameraUser ?? ""))
                .Replace("{password}", Uri.EscapeDataString(camera.CameraPassword ?? ""));
        }
        return camera.CameraUrl ?? "";
    }

    private string GetUsbCommand(CameraConfigEntity camera)
    {
        var brand = _options.GetBrands()
            .FirstOrDefault(b => b.Brand.Equals(camera.CameraBrand, StringComparison.OrdinalIgnoreCase));
        if (brand != null && !string.IsNullOrEmpty(brand.CaptureCommandTemplate))
        {
            return brand.CaptureCommandTemplate
                .Replace("{deviceName}", camera.UsbDeviceName ?? "");
        }
        return camera.UsbCommand ?? "";
    }

    private async Task<byte[]> CaptureIpAsync(CameraConfigEntity camera)
    {
        var url = GetSnapshotUrl(camera);
        _logger.LogDebug("Capturing from {Url} (camera: {CameraId})", url, camera.CameraId);

        // Use CredentialCache to support both Basic and Digest auth (Hikvision uses Digest)
        var uri = new Uri(url);

        // Strip credentials from URL if embedded (user:pass@host)
        var cleanUrl = uri.GetComponents(UriComponents.SchemeAndServer | UriComponents.PathAndQuery, UriFormat.UriEscaped);
        var user = camera.CameraUser ?? "";
        var pass = camera.CameraPassword ?? "";

        // If credentials were in the URL, extract them
        if (!string.IsNullOrEmpty(uri.UserInfo))
        {
            var parts = uri.UserInfo.Split(':', 2);
            user = Uri.UnescapeDataString(parts[0]);
            pass = parts.Length > 1 ? Uri.UnescapeDataString(parts[1]) : "";
        }

        var credentialCache = new System.Net.CredentialCache();
        var baseUri = new Uri($"{uri.Scheme}://{uri.Host}:{uri.Port}");
        credentialCache.Add(baseUri, "Digest", new System.Net.NetworkCredential(user, pass));
        credentialCache.Add(baseUri, "Basic", new System.Net.NetworkCredential(user, pass));

        using var handler = new HttpClientHandler { Credentials = credentialCache };
        using var client = new HttpClient(handler);
        var timeout = camera.TimeoutSeconds > 0 ? camera.TimeoutSeconds : 10;
        client.Timeout = TimeSpan.FromSeconds(timeout);

        try
        {
            return await client.GetByteArrayAsync(cleanUrl);
        }
        catch (TaskCanceledException)
        {
            throw new TimeoutException($"Camera '{camera.CameraId}' did not respond within {timeout} seconds.");
        }
        catch (HttpRequestException ex)
        {
            throw new Exception($"Camera connection failed: {ex.Message}", ex);
        }
    }

    private async Task<byte[]> CaptureUsbAsync(CameraConfigEntity camera)
    {
        var tempFile = Path.Combine(Path.GetTempPath(), $"camera_{Guid.NewGuid()}.jpg");
        try
        {
            var command = $"{GetUsbCommand(camera)} \"{tempFile}\"";
            var psi = new ProcessStartInfo("cmd", $"/c {command}")
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using var process = Process.Start(psi);
            if (process == null)
                throw new InvalidOperationException("Failed to start USB capture process.");

            await process.WaitForExitAsync();

            if (File.Exists(tempFile))
                return await File.ReadAllBytesAsync(tempFile);

            _logger.LogWarning("USB capture did not produce output file. Exit code: {Code}", process.ExitCode);
            return Array.Empty<byte>();
        }
        finally
        {
            if (File.Exists(tempFile))
                File.Delete(tempFile);
        }
    }
}
