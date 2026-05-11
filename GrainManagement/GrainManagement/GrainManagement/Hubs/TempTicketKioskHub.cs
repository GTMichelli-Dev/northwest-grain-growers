using Microsoft.AspNetCore.SignalR;

namespace GrainManagement.Hubs;

/// <summary>
/// Client methods the web pushes to the kiosk browser at
/// <c>/TempTicketKiosk/{kioskId}</c>. The browser also subscribes to
/// <see cref="ScaleHub"/> directly for the live Motion bit; this hub
/// only carries press-confirmation events that are kiosk-specific.
/// </summary>
public interface ITempTicketKioskClient
{
    /// <summary>
    /// Fired right after the orchestrator stores a temp-ticket row.
    /// The browser flashes green with the captured weight for 5 s,
    /// then resets to its idle / "Waiting for motion" state.
    /// </summary>
    Task TempTicketCaptured(object payload);
}

/// <summary>
/// SignalR hub for the kiosk browser displays. One group per kiosk
/// keyed by <c>kiosk-{KioskId}</c>; the orchestrator pushes the
/// press-confirmation event to that group when a row lands.
/// </summary>
public sealed class TempTicketKioskHub : Hub<ITempTicketKioskClient>
{
    public const string HubRoute = "/hubs/temp-ticket-kiosk";

    public static string KioskGroupName(string kioskId) => $"kiosk-{kioskId}";

    /// <summary>
    /// Browser kiosk joins the group keyed by its kiosk id. The web
    /// pushes <see cref="ITempTicketKioskClient.TempTicketCaptured"/>
    /// to that group when the orchestrator stores a row for this kiosk.
    /// </summary>
    public Task JoinKiosk(string kioskId)
    {
        if (string.IsNullOrWhiteSpace(kioskId))
            return Task.CompletedTask;
        return Groups.AddToGroupAsync(Context.ConnectionId, KioskGroupName(kioskId));
    }

    public Task LeaveKiosk(string kioskId)
    {
        if (string.IsNullOrWhiteSpace(kioskId))
            return Task.CompletedTask;
        return Groups.RemoveFromGroupAsync(Context.ConnectionId, KioskGroupName(kioskId));
    }
}
