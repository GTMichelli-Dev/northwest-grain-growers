namespace GrainManagement.Services.ScaleReader.Models
{
    /// <summary>
    /// Top-level service settings loaded from configuration.
    /// </summary>
    public sealed class ServiceSettings
    {
        /// <summary>
        /// Base URL of the GrainManagement server (e.g. "http://localhost:5000").
        /// </summary>
        public string ServerUrl { get; set; } = "http://localhost:5000";

        /// <summary>
        /// SignalR hub path appended to ServerUrl (e.g. "/hubs/scale").
        /// </summary>
        public string SignalRHub { get; set; } = "/hubs/scale";

        /// <summary>
        /// How often (ms) to poll each scale indicator.
        /// </summary>
        public int PollIntervalMs { get; set; } = 750;

        /// <summary>
        /// TCP timeout (ms) for a single scale query.
        /// </summary>
        public int TimeoutMs { get; set; } = 1000;

        /// <summary>
        /// Initial reconnect backoff (ms) after a poll/connection failure.
        /// </summary>
        public int ReconnectBackoffMs { get; set; } = 2000;

        /// <summary>
        /// Maximum backoff ceiling (ms) for exponential retry.
        /// </summary>
        public int MaxBackoffMs { get; set; } = 30000;

        /// <summary>
        /// Location ID from system.Locations — identifies which facility this service is running at.
        /// </summary>
        public int LocationId { get; set; }

        /// <summary>
        /// Human-readable location description (e.g. "Endicott Elevator").
        /// Sent with every scale update so the server knows where the reading originated.
        /// </summary>
        public string LocationDescription { get; set; } = string.Empty;

        /// <summary>
        /// Full hub URL built from ServerUrl + SignalRHub.
        /// </summary>
        public string HubUrl => ServerUrl.TrimEnd('/') + SignalRHub;
    }
}
