using GrainManagement.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace GrainManagement.Services;

/// <summary>
/// Pushes "weight sheet changed" notifications to subscribed clients via the
/// WarehouseHub. Pages that show WS data (dashboard open list, delivery-loads,
/// transfer-loads) join their location's group and refresh on each event.
/// </summary>
public interface IWeightSheetNotifier
{
    /// <summary>
    /// Notify all clients subscribed to a location's group that the given
    /// weight sheet (or a load on it) was modified. Fire-and-forget — failures
    /// are logged but don't block the originating request.
    /// </summary>
    Task NotifyAsync(int locationId, long weightSheetId, string changeKind = null, CancellationToken ct = default);
}

public sealed class WeightSheetNotifier : IWeightSheetNotifier
{
    private readonly IHubContext<WarehouseHub> _hub;
    private readonly ILogger<WeightSheetNotifier> _log;

    public WeightSheetNotifier(IHubContext<WarehouseHub> hub, ILogger<WeightSheetNotifier> log)
    {
        _hub = hub;
        _log = log;
    }

    public async Task NotifyAsync(int locationId, long weightSheetId, string changeKind = null, CancellationToken ct = default)
    {
        if (locationId <= 0) return;
        try
        {
            var payload = new
            {
                LocationId    = locationId,
                WeightSheetId = weightSheetId,
                ChangeKind    = changeKind ?? "",
                At            = DateTime.UtcNow,
            };
            await _hub.Clients
                .Group(WarehouseHub.LocationGroup(locationId))
                .SendAsync("weightSheetUpdated", payload, ct);
        }
        catch (Exception ex)
        {
            // Don't let a SignalR hiccup fail the originating request.
            _log.LogWarning(ex, "Failed to push weightSheetUpdated for WS {WsId} loc {Loc}", weightSheetId, locationId);
        }
    }
}
