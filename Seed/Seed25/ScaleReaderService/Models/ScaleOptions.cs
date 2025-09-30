namespace ScaleReaderService.Models
{
    public sealed class ScaleOptions
    {
        public string Description { get; set; } = "Unknown";
        public string IpAddress { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 3001;
    }

    public sealed class PollingOptions
    {
        public int IntervalMs { get; set; } = 750;
        public int TimeoutMs { get; set; } = 1000;
        public int ReconnectBackoffMs { get; set; } = 2000;
        public int MaxBackoffMs { get; set; } = 10000;
    }

    public sealed class SmaOptions
    {
        /// <summary>
        /// SMA request command sent each poll. Common values:
        /// - "W\\r\\n"  (ASCII 'W' + CRLF) frequent on TCP SMA bridges
        /// - "\\u0005"  (ENQ 0x05) for some devices
        /// </summary>
        public string RequestCommand { get; set; } = "W\r\n";
        public string Encoding { get; set; } = "ascii"; // "ascii" or "utf-8"
    }

    public sealed class EndpointOptions
    {
        public string Name { get; set; } = "Endpoint";
        public string Url { get; set; } = "";
        public string Method { get; set; } = "POST";
        public int TimeoutMs { get; set; } = 2000;
    }
}
