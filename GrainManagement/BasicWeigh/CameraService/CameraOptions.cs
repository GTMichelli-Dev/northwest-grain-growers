using System.Text.Json;

namespace CameraService;

public class CameraBrandDefinition
{
    public string Brand { get; set; } = string.Empty;
    public string Type { get; set; } = "IP";
    public string SnapshotUrlTemplate { get; set; } = string.Empty;
    public string CaptureCommandTemplate { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}

public class CameraOptions
{
    public string ServerUrl { get; set; } = "http://localhost:5110";

    /// <summary>
    /// URL to download camera brand definitions JSON. Falls back to local camera-snapshot.json if unavailable.
    /// </summary>
    public string BrandsUrl { get; set; } = string.Empty;

    /// <summary>
    /// GitHub Personal Access Token for accessing private repos. Leave empty for public repos.
    /// </summary>
    public string BrandsToken { get; set; } = string.Empty;

    /// <summary>
    /// Camera brand name from the brands JSON (e.g. "Hikvision", "USB Camera (Windows)").
    /// Use "Custom" for manual URL/command.
    /// </summary>
    public string CameraBrand { get; set; } = "Custom";

    public string CameraIp { get; set; } = "192.168.1.100";
    public string CameraUser { get; set; } = "admin";
    public string CameraPassword { get; set; } = "password";
    public string UsbDeviceName { get; set; } = string.Empty;

    /// <summary>Full snapshot URL — only used when CameraBrand is "Custom".</summary>
    public string CameraUrl { get; set; } = string.Empty;

    /// <summary>Full USB command — only used when CameraBrand is "Custom".</summary>
    public string UsbCommand { get; set; } = string.Empty;

    public int TimeoutSeconds { get; set; } = 10;

    // Loaded at startup from remote or local JSON
    private List<CameraBrandDefinition> _brands = new();

    public void SetBrands(List<CameraBrandDefinition> brands) => _brands = brands;
    public List<CameraBrandDefinition> GetBrands() => _brands;

    private CameraBrandDefinition? FindBrand() =>
        _brands.FirstOrDefault(b => b.Brand.Equals(CameraBrand, StringComparison.OrdinalIgnoreCase));

    public bool IsUsb
    {
        get
        {
            var brand = FindBrand();
            if (brand != null)
                return brand.Type.Equals("USB", StringComparison.OrdinalIgnoreCase);
            // Custom: USB if UsbCommand is set
            return CameraBrand?.Equals("Custom", StringComparison.OrdinalIgnoreCase) == true
                && !string.IsNullOrEmpty(UsbCommand);
        }
    }

    public string GetSnapshotUrl()
    {
        var brand = FindBrand();
        if (brand != null && !string.IsNullOrEmpty(brand.SnapshotUrlTemplate))
        {
            // Replace IP and credentials (some brands like Reolink/Foscam use query param auth)
            return brand.SnapshotUrlTemplate
                .Replace("{ip}", CameraIp)
                .Replace("{user}", Uri.EscapeDataString(CameraUser))
                .Replace("{password}", Uri.EscapeDataString(CameraPassword));
        }
        // Custom fallback
        return CameraUrl;
    }

    public string GetUsbCaptureCommand()
    {
        var brand = FindBrand();
        if (brand != null && !string.IsNullOrEmpty(brand.CaptureCommandTemplate))
        {
            return brand.CaptureCommandTemplate
                .Replace("{deviceName}", UsbDeviceName);
        }
        // Custom fallback
        return UsbCommand;
    }

    /// <summary>
    /// Load brand definitions from remote URL, falling back to local file.
    /// </summary>
    /// <summary>
    /// Load brands from local file first (fast startup).
    /// Call UpdateBrandsFromRemoteAsync after startup to check for updates.
    /// </summary>
    public static async Task<List<CameraBrandDefinition>> LoadBrandsAsync(
        string remoteUrl, string localPath, string? token = null, ILogger? logger = null)
    {
        // Load local first (fast, always available)
        if (File.Exists(localPath))
        {
            try
            {
                var json = await File.ReadAllTextAsync(localPath);
                var brands = JsonSerializer.Deserialize<List<CameraBrandDefinition>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (brands?.Count > 0)
                {
                    logger?.LogInformation("Loaded {Count} camera brands from local: {Path}", brands.Count, localPath);
                    return brands;
                }
            }
            catch (Exception ex)
            {
                logger?.LogWarning(ex, "Could not load local camera brands from {Path}.", localPath);
            }
        }

        logger?.LogWarning("No local camera brand definitions found. Only 'Custom' brand will be available.");
        return new List<CameraBrandDefinition>();
    }

    /// <summary>
    /// Try to update brands from remote URL. Returns updated list or null if remote is unavailable.
    /// Also saves to local file so next startup uses the latest version.
    /// </summary>
    public static async Task<List<CameraBrandDefinition>?> UpdateBrandsFromRemoteAsync(
        string remoteUrl, string localPath, string? token = null, ILogger? logger = null)
    {
        if (string.IsNullOrEmpty(remoteUrl)) return null;

        try
        {
            using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
            if (!string.IsNullOrEmpty(token))
            {
                http.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                http.DefaultRequestHeaders.UserAgent.ParseAdd("BasicWeigh-CameraService");
            }
            var json = await http.GetStringAsync(remoteUrl);
            var brands = JsonSerializer.Deserialize<List<CameraBrandDefinition>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (brands?.Count > 0)
            {
                logger?.LogInformation("Updated {Count} camera brands from remote: {Url}", brands.Count, remoteUrl);
                // Save to local file for faster next startup
                try { await File.WriteAllTextAsync(localPath, json); }
                catch { /* non-critical */ }
                return brands;
            }
        }
        catch (Exception ex)
        {
            logger?.LogDebug(ex, "Could not update camera brands from {Url}. Using local version.", remoteUrl);
        }
        return null;
    }
}
