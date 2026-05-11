using GrainManagement.Dtos.TempTickets;

namespace GrainManagement.Services.TempTickets;

/// <summary>
/// Coordinates the kiosk-button → temp-ticket workflow:
///   1. Waits for the named scale's Motion bit to clear,
///   2. Snapshots Gross/Tare/Net at the stable moment,
///   3. Inserts the [Inventory].[TempWeightTickets] row,
///   4. Dispatches the temp ticket to the configured printer,
///   5. Fires a camera capture against the TempTicket-role cameras
///      (Phase 2 will composite the images; Phase 1 just kicks them off).
///
/// The whole sequence runs on a background task so the kiosk POST
/// returns 202 immediately without blocking the Pi's request thread.
/// </summary>
public interface ITempTicketOrchestrator
{
    /// <summary>
    /// Enqueue a press. Returns immediately with a placeholder response;
    /// the real work happens off-thread.
    /// </summary>
    TempTicketPressResponse Enqueue(TempTicketPressRequest request);
}
