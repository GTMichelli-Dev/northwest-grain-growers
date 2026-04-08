namespace ScaleReaderService.Models
{
    /// <summary>
    /// Service-level settings loaded from appsettings.json → "Service" section.
    /// </summary>
    public sealed class ServiceSettings
    {
        /// <summary>Unique identifier for this service instance (e.g. "endicott", "colfax").</summary>
        public string ServiceId { get; set; } = "default";

        /// <summary>Server base URLs. Each URL gets its own SignalR connection.</summary>
        public List<string> ServerUrls { get; set; } = new() { "http://localhost:5000" };

        /// <summary>SignalR hub path appended to each server URL.</summary>
        public string SignalRHub { get; set; } = "/hubs/scale";

        /// <summary>Location ID from system.Locations.</summary>
        public int LocationId { get; set; }

        /// <summary>Human-readable location description.</summary>
        public string LocationDescription { get; set; } = string.Empty;

        /// <summary>How often (ms) to poll each scale indicator.</summary>
        public int PollIntervalMs { get; set; } = 750;

        /// <summary>TCP timeout (ms) for a single scale query.</summary>
        public int TimeoutMs { get; set; } = 1000;

        /// <summary>Initial reconnect backoff (ms).</summary>
        public int ReconnectBackoffMs { get; set; } = 500;

        /// <summary>Maximum backoff ceiling (ms).</summary>
        public int MaxBackoffMs { get; set; } = 5000;

        /// <summary>Resolved list of hub URLs built from ServerUrls + SignalRHub.</summary>
        public List<string> HubUrls =>
            (ServerUrls?.Where(u => !string.IsNullOrWhiteSpace(u)).ToList() ?? new List<string> { "http://localhost:5000" })
            .Select(u => u.TrimEnd('/') + SignalRHub)
            .ToList();
    }
}
