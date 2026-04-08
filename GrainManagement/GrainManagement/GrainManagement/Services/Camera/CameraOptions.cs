namespace GrainManagement.Services.Camera;

/// <summary>
/// Supported camera brands/vendors.
/// </summary>
public enum CameraBrandDefinition
{
    Axis,
    Hikvision
}

/// <summary>
/// Connection options for a camera device.
/// </summary>
public sealed class CameraOptions
{
    public string HostOrIp { get; init; } = "";          // e.g. "192.168.1.50"
    public int Port { get; init; } = 80;                  // 80 (HTTP) or 443 (HTTPS)
    public bool UseHttps { get; init; } = false;
    public string Username { get; init; } = "";
    public string Password { get; init; } = "";

    // Optional vendor-specific tweaks
    public int? Channel { get; init; } = null;            // e.g., Hikvision 101/102...
    public int TimeoutSeconds { get; init; } = 15;
    public bool TrustAllCertificates { get; init; } = true; // Useful for self-signed cams (set false in prod if you have CA)
}
