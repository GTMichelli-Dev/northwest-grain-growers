#nullable enable
using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;

namespace GrainManagement.Hubs;

/// <summary>
/// SignalR hub bridging the AS400Sync service to the admin web UI.
/// The service connects outbound (same pattern as ScaleHub / PrintHub),
/// registers itself by serviceId, and listens for run commands. Web admins
/// join the watchers group to receive live progress events while a sync runs.
/// </summary>
public sealed class As400SyncHub : Hub
{
    public const string HubRoute = "/hubs/as400sync";
    private const string WatcherGroup = "As400SyncWatchers";

    // serviceId -> connectionId  (registered AS400Sync service instances)
    private static readonly ConcurrentDictionary<string, string> ServiceConnections = new();

    // ── Service-side methods (called by GrainManagement.As400Sync) ──────────

    public Task RegisterService(string serviceId)
    {
        if (string.IsNullOrWhiteSpace(serviceId)) return Task.CompletedTask;
        ServiceConnections[serviceId] = Context.ConnectionId;
        return Clients.Group(WatcherGroup).SendAsync("ServiceConnected", serviceId);
    }

    public Task ReportStatus(object status)
        => Clients.Group(WatcherGroup).SendAsync("Status", status);

    public Task ReportCompleted(object result)
        => Clients.Group(WatcherGroup).SendAsync("Completed", result);

    public Task ReportError(object error)
        => Clients.Group(WatcherGroup).SendAsync("Error", error);

    // ── Web-UI-side methods ─────────────────────────────────────────────────

    public Task JoinWatcher()
        => Groups.AddToGroupAsync(Context.ConnectionId, WatcherGroup);

    public Task LeaveWatcher()
        => Groups.RemoveFromGroupAsync(Context.ConnectionId, WatcherGroup);

    /// <summary>Web UI asks the service for its current state (busy/idle).</summary>
    public Task RequestSnapshot(string serviceId)
    {
        if (ServiceConnections.TryGetValue(serviceId, out var connId))
            return Clients.Client(connId).SendAsync("RequestSnapshot");
        return Task.CompletedTask;
    }

    /// <summary>Web UI clicks "Sync Accounts" — forward to the service.</summary>
    public Task RunAccounts(string serviceId)
        => SendCommand(serviceId, "RunAccounts");

    /// <summary>Web UI clicks "Sync Items" — forward to the service.</summary>
    public Task RunProducts(string serviceId)
        => SendCommand(serviceId, "RunProducts");

    /// <summary>Web UI clicks "Sync Split Groups" — forward to the service.</summary>
    public Task RunSplitGroups(string serviceId)
        => SendCommand(serviceId, "RunSplitGroups");

    /// <summary>
    /// Web UI clicks "Upload selected" on /AgvantageWarehouseTransfer —
    /// forward the list of weight sheet ids to the service. The service
    /// streams progress / errors back via the same ReportStatus / Error
    /// events that the regular sync jobs use.
    /// </summary>
    public Task RunWarehouseTransferUpload(string serviceId, long[] weightSheetIds)
    {
        if (ServiceConnections.TryGetValue(serviceId, out var connId))
            return Clients.Client(connId).SendAsync("RunWarehouseTransferUpload", weightSheetIds ?? Array.Empty<long>());

        return Clients.Caller.SendAsync("Error", new
        {
            ServiceId = serviceId,
            Message = $"AS400 sync service '{serviceId}' is not connected."
        });
    }

    /// <summary>
    /// Web UI clicks "Clear U5SILOAD" — wipes the entire staging table on
    /// AS400 so the next upload starts from a clean slate. The destructive
    /// confirm prompt lives on the client; this hub method just forwards.
    /// </summary>
    public Task RunClearU5Siload(string serviceId)
        => SendCommand(serviceId, "RunClearU5Siload");

    public Task<string[]> GetConnectedServices()
        => Task.FromResult(ServiceConnections.Keys.ToArray());

    private Task SendCommand(string serviceId, string method)
    {
        if (ServiceConnections.TryGetValue(serviceId, out var connId))
            return Clients.Client(connId).SendAsync(method);

        return Clients.Caller.SendAsync("Error", new
        {
            ServiceId = serviceId,
            Message = $"AS400 sync service '{serviceId}' is not connected."
        });
    }

    // ── Lifecycle ───────────────────────────────────────────────────────────

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var disconnected = ServiceConnections
            .Where(kv => kv.Value == Context.ConnectionId)
            .Select(kv => kv.Key)
            .ToList();

        foreach (var serviceId in disconnected)
        {
            ServiceConnections.TryRemove(serviceId, out _);
            _ = Clients.Group(WatcherGroup).SendAsync("ServiceDisconnected", serviceId);
        }

        return base.OnDisconnectedAsync(exception);
    }
}
