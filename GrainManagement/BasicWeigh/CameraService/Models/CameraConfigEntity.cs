using System.ComponentModel.DataAnnotations;

namespace CameraService.Models;

public class CameraConfigEntity
{
    [Key]
    public int Id { get; set; }

    /// <summary>Unique camera identifier (e.g. "scale-cam-1", "inbound-cam")</summary>
    [Required]
    [StringLength(50)]
    public string CameraId { get; set; } = string.Empty;

    /// <summary>Friendly display name (e.g. "Scale Camera - Inbound")</summary>
    [StringLength(100)]
    public string? DisplayName { get; set; }

    /// <summary>Brand name from camera-snapshot.json (e.g. "Hikvision", "Dahua", "Custom")</summary>
    [Required]
    [StringLength(50)]
    public string CameraBrand { get; set; } = "Custom";

    /// <summary>IP address of the camera</summary>
    [StringLength(50)]
    public string? CameraIp { get; set; }

    /// <summary>Username for camera authentication</summary>
    [StringLength(50)]
    public string? CameraUser { get; set; }

    /// <summary>Password for camera authentication</summary>
    [StringLength(100)]
    public string? CameraPassword { get; set; }

    /// <summary>USB device name (for USB cameras via ffmpeg)</summary>
    [StringLength(100)]
    public string? UsbDeviceName { get; set; }

    /// <summary>Custom snapshot URL override (when CameraBrand is "Custom")</summary>
    [StringLength(500)]
    public string? CameraUrl { get; set; }

    /// <summary>Custom USB command override (when CameraBrand is "Custom")</summary>
    [StringLength(500)]
    public string? UsbCommand { get; set; }

    /// <summary>HTTP timeout for IP camera capture in seconds</summary>
    public int TimeoutSeconds { get; set; } = 10;

    /// <summary>Whether this camera is active and available for capture</summary>
    public bool Active { get; set; } = true;

    /// <summary>Whether this is the default camera when no cameraId is specified</summary>
    public bool IsDefault { get; set; }
}
