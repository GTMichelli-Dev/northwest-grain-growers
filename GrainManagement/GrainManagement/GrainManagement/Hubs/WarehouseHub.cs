using GrainManagement.Services;
using GrainManagement.Services.Warehouse;
using Microsoft.AspNetCore.SignalR;

namespace GrainManagement.Hubs;

/// <summary>
/// SignalR hub for Warehouse screens (Intake/Transfer/Outbound).
/// </summary>
public sealed class WarehouseHub : Hub
{
    public const string HubRoute = "/hubs/warehouse";

    private readonly IWarehouseIntakeDataService _intakeService;
    private readonly IPriorDayWeightSheetGuard _priorDayGuard;

    public WarehouseHub(
        IWarehouseIntakeDataService intakeService,
        IPriorDayWeightSheetGuard priorDayGuard)
    {
        _intakeService = intakeService;
        _priorDayGuard = priorDayGuard;
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

    /// <summary>
    /// Returns the count of prior-day open weight sheets at the given
    /// location. Called by the warehouse dashboard on connect; a non-zero
    /// count drives an auto-prompt to launch End Of Day. Routed through
    /// SignalR (rather than HTTP) so it rides the dashboard's existing
    /// hub connection without an extra network round-trip on page load.
    /// </summary>
    public async Task<int> CheckPriorDayOpenWeightSheets(int locationId)
    {
        if (locationId <= 0) return 0;
        var ids = await _priorDayGuard.GetPriorDayOpenWeightSheetIdsAsync(
            locationId, Context.ConnectionAborted);
        return ids.Count;
    }
}
