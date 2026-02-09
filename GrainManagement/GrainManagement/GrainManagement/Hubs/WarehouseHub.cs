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
}
