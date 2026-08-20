using System.ComponentModel.DataAnnotations;

namespace ScaleReaderService.Models;

/// <summary>
/// Global service settings — single row table.
/// </summary>
public class ServiceSettings
{
    [Key]
    public int Id { get; set; }

    /// <summary>Unique identifier for this service instance (e.g. "site-a", "plant-2").</summary>
    [Required]
    [StringLength(50)]
    public string ServiceId { get; set; } = "default";

    /// <summary>Base URL of the web application (e.g. "http://localhost:5110")</summary>
    [Required]
    [StringLength(200)]
    public string ServerUrl { get; set; } = "http://localhost:5110";

    /// <summary>SignalR hub path on the web app (e.g. "/scaleHub")</summary>
    [StringLength(100)]
    public string SignalRHub { get; set; } = "/scaleHub";

    /// <summary>URL to remote scale model definitions JSON</summary>
    [StringLength(500)]
    public string? BrandsUrl { get; set; }

    /// <summary>GitHub PAT for private definitions repo</summary>
    [StringLength(200)]
    public string? BrandsToken { get; set; }
}
