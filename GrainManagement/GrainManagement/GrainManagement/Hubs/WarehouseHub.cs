using GrainManagement.Services;
using Microsoft.AspNetCore.SignalR;

namespace GrainManagement.Hubs;

/// <summary>
/// SignalR hub for Warehouse screens (Intake/Transfer/Outbound).
/// </summary>
public sealed class WarehouseHub : Hub
{
    public const string HubRoute = "/hubs/warehouse";

    private readonly IWarehouseIntakeDataService _intakeService;

    public WarehouseHub(IWarehouseIntakeDataService intakeService)
    {
        _intakeService = intakeService;
    }

    /// <summary>
    /// Called by the browser after the Intake partial loads.
    /// Returns the latest snapshot for the selected location.
    /// </summary>
    public async Task RequestIntakeRefresh(int locationId)
    {
        var snapshot = await _intakeService.GetSnapshotAsync(locationId, Context.ConnectionAborted);
        await Clients.Caller.SendAsync("intakeSnapshot", snapshot, Context.ConnectionAborted);
    }

    /// <summary>
    /// Subscribe this connection to weight-sheet update broadcasts for a
    /// specific location. The dashboard, delivery-loads, and transfer-loads
    /// pages call this on connect so they can react to WS mutations made
    /// from other browsers/operators in real time.
    /// </summary>
    public Task JoinLocation(int locationId)
    {
        if (locationId <= 0) return Task.CompletedTask;
        return Groups.AddToGroupAsync(Context.ConnectionId, LocationGroup(locationId));
    }

    public Task LeaveLocation(int locationId)
    {
        if (locationId <= 0) return Task.CompletedTask;
        return Groups.RemoveFromGroupAsync(Context.ConnectionId, LocationGroup(locationId));
    }

    /// <summary>Group name used by IWeightSheetNotifier for per-location fan-out.</summary>
    public static string LocationGroup(int locationId) => $"loc-{locationId}";
}
