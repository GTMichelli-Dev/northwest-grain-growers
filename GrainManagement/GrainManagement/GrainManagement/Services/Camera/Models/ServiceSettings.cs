#nullable enable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrainManagement.Services.Camera.Models;

/// <summary>
/// Stores the camera service runtime settings in the local database.
/// Typically a single row.
/// </summary>
[Table("ServiceSettings")]
public class ServiceSettings
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Base URL of the GrainManagement server (e.g., "http://localhost:5000").
    /// </summary>
    [MaxLength(500)]
    public string ServerUrl { get; set; } = "http://localhost:5000";

    /// <summary>
    /// SignalR hub path to connect to (e.g., "/hubs/scale").
    /// </summary>
    [MaxLength(200)]
    public string SignalRHub { get; set; } = "/hubs/scale";

    /// <summary>
    /// Default local path where captured images are saved.
    /// </summary>
    [MaxLength(500)]
    public string? DefaultImageOutputPath { get; set; }

    /// <summary>
    /// Optional API endpoint path for uploading images to the server
    /// (e.g., "/api/camera/upload").
    /// </summary>
    [MaxLength(500)]
    public string? ImageUploadEndpoint { get; set; }

    /// <summary>
    /// Whether the camera service is enabled.
    /// </summary>
    public bool IsEnabled { get; set; } = true;

    /// <summary>
    /// Location identifier for this camera service instance.
    /// </summary>
    public int? LocationId { get; set; }
}
