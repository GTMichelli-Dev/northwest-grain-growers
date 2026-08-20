namespace ScaleReaderService.Models;

/// <summary>
/// Used only for seeding from appsettings.json "Scales" section.
/// Runtime config is stored in SQLite (ScaleConfigEntity).
/// </summary>
public sealed class ScaleOptions
{
    public string Description { get; set; } = "Unknown";
    public int Id { get; set; } = 0;
    public string IpAddress { get; set; } = "127.0.0.1";
    public int Port { get; set; } = 10001;
}
