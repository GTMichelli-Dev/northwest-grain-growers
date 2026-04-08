namespace ScaleReaderService.Models
{
    /// <summary>
    /// Service-level settings loaded from appsettings.json → "Service" section.
    /// </summary>
    public sealed class ServiceSettings
    {
        /// <summary>Unique identifier for this service instance (e.g. "endicott", "colfax").</summary>
        public string ServiceId { get; set; } = "default";

        /// <summary>Base URL of the GrainManagement web server.</summary>
        public string ServerUrl { get; set; } = "http://localhost:5000";

        /// <summary>SignalR hub path appended to ServerUrl.</summary>
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
        public int ReconnectBackoffMs { get; set; } = 2000;

        /// <summary>Maximum backoff ceiling (ms).</summary>
        public int MaxBackoffMs { get; set; } = 30000;

        /// <summary>Full hub URL.</summary>
        public string HubUrl => ServerUrl.TrimEnd('/') + SignalRHub;
    }
}
