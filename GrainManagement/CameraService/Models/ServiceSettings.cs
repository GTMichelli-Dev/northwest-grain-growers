using System.ComponentModel.DataAnnotations;

namespace CameraService.Models;

/// <summary>
/// Global service settings — single row table.
/// </summary>
public class ServiceSettings
{
    [Key]
    public int Id { get; set; }

    /// <summary>Unique identifier for this service instance (e.g. "site-a", "plant-2").
    /// Used to route capture commands to the correct service when multiple services are connected.</summary>
    [Required]
    [StringLength(50)]
    public string ServiceId { get; set; } = "default";

    /// <summary>Base URL of the web application (e.g. "http://localhost:5110")</summary>
    [Required]
    [StringLength(200)]
    public string ServerUrl { get; set; } = "http://localhost:5110";

    /// <summary>SignalR hub path on the web app (e.g. "/hubs/camera")</summary>
    [StringLength(100)]
    public string SignalRHub { get; set; } = "/hubs/camera";

    /// <summary>
    /// Externally-reachable base URL of THIS CameraService (e.g.
    /// "http://192.168.1.50:5210"). Announced to the web so browsers
    /// can pull MJPEG live feeds directly from this process. Leave empty
    /// to disable live view for this service's cameras.
    /// </summary>
    [StringLength(200)]
    public string? StreamBaseUrl { get; set; }

    /// <summary>URL to remote camera brand definitions JSON</summary>
    [StringLength(500)]
    public string? BrandsUrl { get; set; }

    /// <summary>GitHub PAT for private brand definitions repo</summary>
    [StringLength(200)]
    public string? BrandsToken { get; set; }
}
