#nullable disable
using System;

namespace GrainManagement.Models;

/// <summary>
/// Web-side role assignment for a physical camera that lives on a
/// CameraService instance. Source of truth for "which camera is the
/// inbound for scale X" / "which cameras are BOL cameras". The hardware
/// connection details (IP, USB, credentials) stay on the CameraService.
/// </summary>
public partial class CameraAssignment
{
    public int CameraAssignmentId { get; set; }

    /// <summary>Identifies the CameraService process (hostname or configured name).</summary>
    public string ServiceId { get; set; }

    /// <summary>Camera id within that service (unique per ServiceId).</summary>
    public string CameraId { get; set; }

    /// <summary>Friendly label for UIs.</summary>
    public string DisplayName { get; set; }

    public int? LocationId { get; set; }

    public int? ScaleId { get; set; }

    /// <summary>One of: Inbound, Outbound, BOL, View.</summary>
    public string Role { get; set; }

    /// <summary>When multiple cameras share a role/scale, the primary's image is the one shown on the ticket.</summary>
    public bool IsPrimary { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Location Location { get; set; }
}
