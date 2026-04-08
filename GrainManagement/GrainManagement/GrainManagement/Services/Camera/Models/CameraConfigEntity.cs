#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrainManagement.Services.Camera.Models;

/// <summary>
/// Represents a camera configuration stored in the local database.
/// Each camera is associated with a scale and has connection/capture settings.
/// </summary>
[Table("CameraConfigs")]
public class CameraConfigEntity
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// The scale this camera is associated with.
    /// </summary>
    public int ScaleId { get; set; }

    /// <summary>
    /// Friendly name or description for this camera.
    /// </summary>
    [MaxLength(200)]
    public string Name { get; set; } = "";

    /// <summary>
    /// Camera brand (e.g., "Axis", "Hikvision").
    /// </summary>
    [MaxLength(50)]
    public string Brand { get; set; } = "Axis";

    /// <summary>
    /// Camera IP address or hostname.
    /// </summary>
    [MaxLength(200)]
    public string IpAddress { get; set; } = "";

    /// <summary>
    /// HTTP/HTTPS port.
    /// </summary>
    public int Port { get; set; } = 80;

    /// <summary>
    /// Whether to use HTTPS.
    /// </summary>
    public bool UseHttps { get; set; } = false;

    /// <summary>
    /// Camera login username.
    /// </summary>
    [MaxLength(100)]
    public string Username { get; set; } = "";

    /// <summary>
    /// Camera login password.
    /// </summary>
    [MaxLength(100)]
    public string Password { get; set; } = "";

    /// <summary>
    /// Optional vendor-specific channel number (e.g., Hikvision 101/102).
    /// </summary>
    public int? Channel { get; set; }

    /// <summary>
    /// Default PTZ preset number to move to before capture.
    /// </summary>
    public int DefaultPreset { get; set; } = 1;

    /// <summary>
    /// Delay in milliseconds after moving to preset before capturing.
    /// </summary>
    public int PresetDelayMs { get; set; } = 5000;

    /// <summary>
    /// Whether this camera configuration is active.
    /// </summary>
    public bool IsEnabled { get; set; } = true;

    /// <summary>
    /// Optional RTSP address for streaming.
    /// </summary>
    [MaxLength(500)]
    public string? RtspAddress { get; set; }

    /// <summary>
    /// Optional camera location description (e.g., "Inbound", "Outbound").
    /// </summary>
    [MaxLength(200)]
    public string? CameraLocation { get; set; }

    /// <summary>
    /// Date/time this record was created.
    /// </summary>
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Date/time this record was last modified.
    /// </summary>
    public DateTime ModifiedUtc { get; set; } = DateTime.UtcNow;
}
