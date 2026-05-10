#nullable enable
using System.Collections.Concurrent;
using System.Threading.Tasks;
using GrainManagement.Dtos.Cameras;
using GrainManagement.Services.Camera;
using Microsoft.AspNetCore.SignalR;

namespace GrainManagement.Hubs;

public interface ICameraClient
{
    // ── Web UI notifications ────────────────────────────
    Task ServiceConnected(string serviceId);
    Task ServiceDisconnected(string serviceId);
    Task CamerasAnnounced(string serviceId);
    Task ImageCaptured(string ticket, string direction);

    // ── Service-bound commands ──────────────────────────
    Task CaptureImage(CaptureCommand cmd);
    Task Reannounce();
}

/// <summary>
/// Hub used by both web UI clients and external CameraService instances.
/// Mirrors the topology of <see cref="ScaleHub"/>: services join a
/// per-service group, announce their cameras, and accept capture
/// commands routed by serviceId. Role/assignment data is fetched
/// separately via the REST API; this hub is for live config and
/// fire-and-forget capture pings.
/// </summary>
public sealed class CameraHub : Hub<ICameraClient>
{
    public const string HubRoute = "/hubs/camera";

    // serviceId → connectionId
    private static readonly ConcurrentDictionary<string, string> _serviceConnections = new();

    private readonly ICameraRegistry _registry;

    public CameraHub(ICameraRegistry registry)
    {
        _registry = registry;
    }

    public static string ServiceGroupName(string serviceId) => $"camera-service:{serviceId}";

    // ── Called by CameraService instances ────────────────────────────────

    /// <summary>Service joins its group on connect.</summary>
    public async Task JoinCameraGroup(string serviceId)
    {
        _serviceConnections[serviceId] = Context.ConnectionId;
        await Groups.AddToGroupAsync(Context.ConnectionId, ServiceGroupName(serviceId));
    }

    /// <summary>
    /// Service announces its camera list. Called on connect, on reconnect,
    /// and after local CRUD. <paramref name="streamBaseUrl"/> is how the
    /// browser will reach this service for MJPEG streaming (e.g.
    /// "http://192.168.1.50:5210"). Optional — when null, live view is
    /// disabled for this service's cameras.
    /// </summary>
    public Task AnnounceCameras(CameraServiceAnnouncement announcement, string? streamBaseUrl)
    {
        _serviceConnections[announcement.ServiceId] = Context.ConnectionId;
        _registry.Announce(announcement.ServiceId, announcement.Cameras, streamBaseUrl);
        return Clients.All.CamerasAnnounced(announcement.ServiceId);
    }

    /// <summary>Service reports it just wrote an image — web grids can refresh.</summary>
    public Task ImageCaptured(string ticket, string direction)
        => Clients.All.ImageCaptured(ticket, direction);

    // ── Called by the web (UI or server actions) ─────────────────────────

    /// <summary>
    /// Fire-and-forget capture trigger. <paramref name="serviceId"/> is
    /// required when the camera is targeted (cameraId scoped per service).
    /// </summary>
    public async Task CaptureImage(string serviceId, string cameraId, string ticket, string direction)
    {
        if (_serviceConnections.TryGetValue(serviceId, out var connId))
        {
            await Clients.Client(connId).CaptureImage(new CaptureCommand(ticket, direction, cameraId));
        }
    }

    /// <summary>Ask a service to re-send its camera list (admin tool).</summary>
    public async Task RequestReannounce(string serviceId)
    {
        if (_serviceConnections.TryGetValue(serviceId, out var connId))
            await Clients.Client(connId).Reannounce();
    }

    public Task<string[]> GetConnectedServices()
        => Task.FromResult(_serviceConnections.Keys.ToArray());

    // ── Lifecycle ────────────────────────────────────────────────────────

    public override async Task OnDisconnectedAsync(System.Exception? exception)
    {
        var disconnected = _serviceConnections
            .Where(kv => kv.Value == Context.ConnectionId)
            .Select(kv => kv.Key)
            .ToList();

        foreach (var serviceId in disconnected)
        {
            _serviceConnections.TryRemove(serviceId, out _);
            _registry.Forget(serviceId);
            await Clients.All.ServiceDisconnected(serviceId);
        }

        await base.OnDisconnectedAsync(exception);
    }
}
