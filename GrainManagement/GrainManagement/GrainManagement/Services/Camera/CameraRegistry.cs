#nullable enable
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using GrainManagement.Dtos.Cameras;

namespace GrainManagement.Services.Camera;

/// <summary>
/// In-memory registry of camera services currently connected to the web
/// via <c>CameraHub</c>. The CameraService announces itself on connect
/// and re-announces when its local config changes; this registry keeps
/// the canonical view of "what hardware exists right now."
///
/// Role assignments (Inbound/Outbound/BOL/View) live in SQL Server
/// (system.CameraAssignments), not here.
/// </summary>
public interface ICameraRegistry
{
    void Announce(string serviceId, IReadOnlyList<CameraInfo> cameras, string? streamBaseUrl);
    void Forget(string serviceId);

    IReadOnlyList<RegisteredCamera> GetAll();
    RegisteredCamera? Get(string serviceId, string cameraId);

    /// <summary>Service IDs currently connected.</summary>
    IReadOnlyList<string> ConnectedServices { get; }
}

public sealed record RegisteredCamera(
    string ServiceId,
    string CameraId,
    string DisplayName,
    string CameraBrand,
    bool Active,
    string? StreamUrl   // absolute URL pointing at the CameraService MJPEG endpoint, or null
);

public sealed class CameraRegistry : ICameraRegistry
{
    // serviceId → (cameraId → camera)
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, RegisteredCamera>> _byService
        = new(StringComparer.OrdinalIgnoreCase);

    public void Announce(string serviceId, IReadOnlyList<CameraInfo> cameras, string? streamBaseUrl)
    {
        if (string.IsNullOrWhiteSpace(serviceId)) return;

        var inner = new ConcurrentDictionary<string, RegisteredCamera>(StringComparer.OrdinalIgnoreCase);
        foreach (var c in cameras ?? Array.Empty<CameraInfo>())
        {
            if (string.IsNullOrWhiteSpace(c.CameraId)) continue;

            string? streamUrl = null;
            if (!string.IsNullOrWhiteSpace(streamBaseUrl) && !string.IsNullOrWhiteSpace(c.StreamPath))
            {
                streamUrl = streamBaseUrl!.TrimEnd('/') + (c.StreamPath!.StartsWith('/') ? c.StreamPath : "/" + c.StreamPath);
            }

            inner[c.CameraId] = new RegisteredCamera(
                serviceId, c.CameraId, c.DisplayName ?? c.CameraId,
                c.CameraBrand ?? "", c.Active, streamUrl);
        }

        _byService[serviceId] = inner;
    }

    public void Forget(string serviceId)
    {
        if (string.IsNullOrWhiteSpace(serviceId)) return;
        _byService.TryRemove(serviceId, out _);
    }

    public IReadOnlyList<RegisteredCamera> GetAll()
        => _byService.Values
            .SelectMany(d => d.Values)
            .OrderBy(c => c.ServiceId, StringComparer.OrdinalIgnoreCase)
            .ThenBy(c => c.DisplayName, StringComparer.OrdinalIgnoreCase)
            .ToList();

    public RegisteredCamera? Get(string serviceId, string cameraId)
    {
        if (_byService.TryGetValue(serviceId, out var inner) &&
            inner.TryGetValue(cameraId, out var cam))
            return cam;
        return null;
    }

    public IReadOnlyList<string> ConnectedServices => _byService.Keys.ToList();
}
