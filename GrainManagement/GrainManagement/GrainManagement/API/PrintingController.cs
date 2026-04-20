#nullable enable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using GrainManagement.Hubs;
using GrainManagement.Services.Print;
using GrainManagement.Services.Print.Data;

namespace GrainManagement.Api
{
    [ApiController]
    [Route("api/printing")]
    public class PrintingController : ControllerBase
    {
        private readonly IHubContext<PrintHub> _hub;
        private readonly IPrintDispatchService _dispatch;
        private readonly PrintDbContext _printDb;
        private readonly ILogger<PrintingController> _log;

        public PrintingController(
            IHubContext<PrintHub> hub,
            IPrintDispatchService dispatch,
            PrintDbContext printDb,
            ILogger<PrintingController> log)
        {
            _hub = hub;
            _dispatch = dispatch;
            _printDb = printDb;
            _log = log;
        }

        /// <summary>
        /// Print a ticket using the assigned printer for the given role.
        /// If the role has no printer assigned (None), the request succeeds silently
        /// without printing anything — auto-print is suppressed.
        /// </summary>
        [HttpPost("print-ticket/{ticket}")]
        public async Task<IActionResult> PrintTicketByRole(
            [FromRoute] string ticket,
            [FromQuery] string role = "Inbound")
        {
            if (string.IsNullOrWhiteSpace(ticket))
                return BadRequest(new { error = "ticket is required" });

            var assignment = _printDb.GetAssignment(role);

            // Role has no printer → silently skip (None selected)
            if (assignment == null ||
                string.IsNullOrWhiteSpace(assignment.PrinterId) ||
                string.IsNullOrWhiteSpace(assignment.ServiceId))
            {
                _log.LogInformation(
                    "Auto-print skipped for role {Role}: no printer configured.", role);
                return Ok(new { sent = false, reason = "No printer configured", role });
            }

            var printerTarget = $"{assignment.ServiceId}:{assignment.PrinterId}";

            try
            {
                await _dispatch.DispatchTicketAsync(ticket, printerTarget, role);
                return Ok(new { sent = true, ticket, role, printerTarget });
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Failed to dispatch print ticket. Ticket={Ticket} Role={Role}", ticket, role);
                return StatusCode(500, new { error = "Failed to dispatch print request" });
            }
        }

        /// <summary>
        /// Reprint a ticket. Same as print but explicitly named for clarity.
        /// </summary>
        [HttpPost("reprint/{ticket}")]
        public Task<IActionResult> Reprint(
            [FromRoute] string ticket,
            [FromQuery] string role = "Inbound")
        {
            return PrintTicketByRole(ticket, role);
        }

        /// <summary>
        /// Returns whether a printer is configured for the given role.
        /// Used by the UI to validate before triggering a manual print.
        /// </summary>
        [HttpGet("has-printer")]
        public IActionResult HasPrinter([FromQuery] string role)
        {
            if (string.IsNullOrWhiteSpace(role))
                return BadRequest(new { error = "role is required" });

            var assignment = _printDb.GetAssignment(role);
            bool configured = assignment != null
                           && !string.IsNullOrWhiteSpace(assignment.PrinterId)
                           && !string.IsNullOrWhiteSpace(assignment.ServiceId);

            return Ok(new
            {
                role,
                configured,
                serviceId = configured ? assignment!.ServiceId : null,
                printerId = configured ? assignment!.PrinterId : null,
            });
        }

        /// <summary>
        /// Manually prints a lot label to the Lot role printer.
        /// Returns 400 with a friendly message if no Lot printer is configured.
        /// </summary>
        [HttpPost("print-lot-label/{lotId}")]
        public async Task<IActionResult> PrintLotLabel([FromRoute] string lotId)
        {
            if (string.IsNullOrWhiteSpace(lotId))
                return BadRequest(new { error = "lotId is required" });

            var assignment = _printDb.GetAssignment("Lot");
            if (assignment == null ||
                string.IsNullOrWhiteSpace(assignment.PrinterId) ||
                string.IsNullOrWhiteSpace(assignment.ServiceId))
            {
                return BadRequest(new { error = "No lot printer is configured. Please configure a Lot printer in Printer settings." });
            }

            var printerTarget = $"{assignment.ServiceId}:{assignment.PrinterId}";

            try
            {
                // Use "LotLabel" as the job type so PrintWorker knows which PDF endpoint to fetch
                await _dispatch.DispatchTicketAsync(lotId, printerTarget, "LotLabel");
                return Ok(new { sent = true, lotId, printerTarget });
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Failed to dispatch lot label print. LotId={LotId}", lotId);
                return StatusCode(500, new { error = "Failed to dispatch lot label print" });
            }
        }

        /// <summary>
        /// Legacy endpoint: send print command to a specific kiosk device by device ID.
        /// Used by the embedded PrintWorker (CUPS kiosk) flow.
        /// Falls back to group-based dispatch if device is not directly connected.
        /// </summary>
        [HttpPost("printer/{printerId}/print-ticket/{ticket}")]
        public async Task<IActionResult> PrintTicketByDevice(
            [FromRoute] string printerId,
            [FromRoute] string ticket)
        {
            if (string.IsNullOrWhiteSpace(printerId))
                return BadRequest(new { error = "printerId is required" });

            if (string.IsNullOrWhiteSpace(ticket))
                return BadRequest(new { error = "ticket is required" });

            // Legacy direct route: kiosk connection registered by exact printerId/deviceId
            if (PrintHub.TryGetConnection(printerId, out var connId))
            {
                await _hub.Clients.Client(connId)
                    .SendAsync("PrintLoadTicket", new { ticket, printerId });
                return Ok(new { sent = true, printerId, ticket, mode = "direct" });
            }

            // Fall back to group-based dispatch via PrintDispatchService
            try
            {
                await _dispatch.DispatchTicketAsync(ticket, printerId, "LoadTicket");
                return Ok(new { sent = true, printerId, ticket, mode = "group" });
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Failed to dispatch print ticket. Ticket={Ticket} Printer={Printer}", ticket, printerId);
                return StatusCode(500, new { error = "Failed to dispatch print request" });
            }
        }
    }
}
