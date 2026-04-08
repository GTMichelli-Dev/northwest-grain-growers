using System.ComponentModel.DataAnnotations;

namespace ScaleReaderService.Models;

/// <summary>
/// A configured scale stored in the local SQLite database.
/// </summary>
public class ScaleConfigEntity
{
    [Key]
    public int Id { get; set; }

    /// <summary>Unique string identifier for this scale (e.g. "truck-scale-1")</summary>
    [Required]
    [StringLength(50)]
    public string ScaleId { get; set; } = "";

    /// <summary>Human-friendly name (e.g. "Truck Scale 2")</summary>
    [StringLength(100)]
    public string DisplayName { get; set; } = "";

    /// <summary>Scale brand/protocol from scale-models.json (e.g. "Generic SMA", "Mettler Toledo")</summary>
    [StringLength(50)]
    public string ScaleBrand { get; set; } = "Generic SMA";

    /// <summary>IP address of the scale indicator</summary>
    [StringLength(100)]
    public string IpAddress { get; set; } = "127.0.0.1";

    /// <summary>TCP port for the scale connection</summary>
    public int Port { get; set; } = 10001;

    /// <summary>Custom request command override (e.g. "W\r\n"). If empty, uses brand default.</summary>
    [StringLength(100)]
    public string? RequestCommand { get; set; }

    /// <summary>Polling interval in milliseconds</summary>
    public int PollingIntervalMs { get; set; } = 750;

    /// <summary>Socket timeout in milliseconds</summary>
    public int TimeoutMs { get; set; } = 1000;

    /// <summary>Whether this scale is active</summary>
    public bool Active { get; set; } = true;
}
