#nullable enable
using GrainManagement.Dtos.Scales;
using GrainManagement.Services;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Text.Json;

namespace GrainManagement.Hubs;

public interface IScaleClient
{
    // ── Web UI notifications ────────────────────────────
    Task ScaleUpdated(ScaleDto dto);
    Task ScalesAnnounced(string serviceId, int locationId, string locationDescription, object scales);
    Task ServiceConnected(string serviceId, int locationId, string locationDescription);
    Task ServiceDisconnected(string serviceId);
    Task CrudResult(object result);

    // ── Service CRUD commands (forwarded to ScaleReaderService) ──
    Task AddScale(object config);
    Task UpdateScaleConfig(int scaleId, object config);
    Task DeleteScale(int scaleId);
    Task Announce();
}

public sealed class ScaleHub : Hub<IScaleClient>
{
    // Track connected services: serviceId → connectionId
    private static readonly ConcurrentDictionary<string, string> _serviceConnections = new();

    private readonly IScaleRegistry _scaleRegistry;

    public ScaleHub(IScaleRegistry scaleRegistry)
    {
        _scaleRegistry = scaleRegistry;
    }

    public static string ScaleGroupName(int scaleId) => $"scale:{scaleId}";
    public static string ServiceGroupName(string serviceId) => $"service:{serviceId}";

    // ── Called by web UI clients ─────────────────────────────────────────

    public Task JoinScale(int scaleId)
        => Groups.AddToGroupAsync(Context.ConnectionId, ScaleGroupName(scaleId));

    public Task LeaveScale(int scaleId)
        => Groups.RemoveFromGroupAsync(Context.ConnectionId, ScaleGroupName(scaleId));

    // ── Called by ScaleReaderService instances ───────────────────────────

    /// <summary>Service joins its group and registers as connected.</summary>
    public async Task JoinScaleGroup(string serviceId)
    {
        _serviceConnections[serviceId] = Context.ConnectionId;
        await Groups.AddToGroupAsync(Context.ConnectionId, ServiceGroupName(serviceId));
    }

    /// <summary>Service announces its scale configurations to all web clients.</summary>
    public async Task AnnounceScales(string serviceId, int locationId, string locationDescription, object scales)
    {
        _serviceConnections[serviceId] = Context.ConnectionId;
        await Clients.All.ServiceConnected(serviceId, locationId, locationDescription);
        await Clients.All.ScalesAnnounced(serviceId, locationId, locationDescription, scales);
    }

    /// <summary>Service pushes a scale weight update.</summary>
    public async Task UpdateScale(ScaleDto dto)
    {
        // Update the server-side registry (for CachedScales API)
        try { _scaleRegistry.Upsert(dto); } catch { /* ignore duplicate desc errors */ }

        // Forward to specific scale group and all clients
        await Clients.Group(ScaleGroupName(dto.Id)).ScaleUpdated(dto);
        await Clients.All.ScaleUpdated(dto);
    }

    /// <summary>Service reports CRUD operation result.</summary>
    public Task ScaleCrudResult(object result)
        => Clients.All.CrudResult(result);

    // ── Called by web UI to push CRUD commands to a specific service ─────

    /// <summary>Web UI tells a service to add a scale config.</summary>
    public async Task AddScaleToService(string serviceId, object config)
    {
        if (_serviceConnections.TryGetValue(serviceId, out var connId))
            await Clients.Client(connId).AddScale(config);
    }

    /// <summary>Web UI tells a service to update a scale config.</summary>
    public async Task UpdateScaleOnService(string serviceId, int scaleId, object config)
    {
        if (_serviceConnections.TryGetValue(serviceId, out var connId))
            await Clients.Client(connId).UpdateScaleConfig(scaleId, config);
    }

    /// <summary>Web UI tells a service to delete a scale config.</summary>
    public async Task DeleteScaleFromService(string serviceId, int scaleId)
    {
        if (_serviceConnections.TryGetValue(serviceId, out var connId))
            await Clients.Client(connId).DeleteScale(scaleId);
    }

    /// <summary>Web UI asks a service to re-announce its scales.</summary>
    public async Task RequestServiceAnnounce(string serviceId)
    {
        if (_serviceConnections.TryGetValue(serviceId, out var connId))
            await Clients.Client(connId).Announce();
    }

    /// <summary>Get list of currently connected service IDs.</summary>
    public Task<string[]> GetConnectedServices()
        => Task.FromResult(_serviceConnections.Keys.ToArray());

    // ── Connection lifecycle ─────────────────────────────────────────────

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        // Find and remove the service that disconnected
        var disconnected = _serviceConnections
            .Where(kv => kv.Value == Context.ConnectionId)
            .Select(kv => kv.Key)
            .ToList();

        foreach (var serviceId in disconnected)
        {
            _serviceConnections.TryRemove(serviceId, out _);
            await Clients.All.ServiceDisconnected(serviceId);
        }

        await base.OnDisconnectedAsync(exception);
    }
}
