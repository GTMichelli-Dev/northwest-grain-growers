#nullable enable
using System.Collections.Generic;

namespace GrainManagement.Dtos.Cameras;

/// <summary>
/// One camera as the web sees it — the union of hardware metadata (from
/// the CameraService) and role assignments (from system.CameraAssignments).
/// </summary>
public sealed record CameraDto(
    string ServiceId,
    string CameraId,
    string DisplayName,
    string CameraBrand,
    bool Active,
    string? StreamUrl,
    IReadOnlyList<CameraRoleDto> Roles
);

public sealed record CameraRoleDto(
    int CameraAssignmentId,
    string Role,
    int? LocationId,
    int? ScaleId,
    bool IsPrimary
);

/// <summary>Sent by the CameraService over SignalR when it joins the hub.</summary>
public sealed record CameraServiceAnnouncement(
    string ServiceId,
    int CameraCount,
    IReadOnlyList<CameraInfo> Cameras
);

public sealed record CameraInfo(
    string CameraId,
    string DisplayName,
    string CameraBrand,
    bool Active,
    string? StreamPath  // relative path on the CameraService for MJPEG, e.g. "/api/stream/{id}"
);

/// <summary>POST body for /api/cameras/assignments — create/update a role assignment.</summary>
public sealed class CameraAssignmentRequest
{
    public string ServiceId { get; set; } = "";
    public string CameraId { get; set; } = "";
    public string DisplayName { get; set; } = "";
    public int? LocationId { get; set; }
    public int? ScaleId { get; set; }
    public string Role { get; set; } = "";   // Inbound | Outbound | BOL | View
    public bool IsPrimary { get; set; }
}

/// <summary>Capture-trigger payload broadcast to CameraService instances.</summary>
public sealed record CaptureCommand(
    string Ticket,
    string Direction,   // in | out | bol
    string CameraId
);
