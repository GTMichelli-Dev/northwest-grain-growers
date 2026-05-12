namespace WebPrintService;

/// <summary>
/// Configuration for the Web Print Service, bound from the "Print" section in appsettings.json.
/// Follows the same pattern as ScaleReaderService's ServiceSettings.
/// </summary>
public sealed class PrintServiceOptions
{
    public const string SectionName = "Print";

    /// <summary>Service identifier used when registering with the web app's PrintHub.</summary>
    public string ServiceId { get; set; } = "default";

    /// <summary>Server ID this kiosk represents — matches the Servers table in GrainManagement.</summary>
    public int ServerId { get; set; } = 1;

    /// <summary>Server base URLs. Each URL gets its own SignalR connection.</summary>
    public List<string> ServerUrls { get; set; } = new();

    /// <summary>SignalR hub path appended to each server URL.</summary>
    public string SignalRHub { get; set; } = "/hubs/print";

    /// <summary>Port this service listens on for its own REST API / Swagger.</summary>
    public string Port { get; set; } = "5230";

    /// <summary>Resolved list of full hub URLs built from ServerUrls + SignalRHub.</summary>
    public List<string> HubUrls
    {
        get
        {
            var urls = ServerUrls?.Where(u => !string.IsNullOrWhiteSpace(u)).ToList();
            if (urls == null || urls.Count == 0)
                urls = new List<string> { "http://localhost:5000" };
            return urls.Select(u => u.TrimEnd('/') + SignalRHub).ToList();
        }
    }
}
