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
        /// Looks up the printer assignment from SQLite, then dispatches via SignalR groups.
        /// </summary>
        [HttpPost("print-ticket/{ticket}")]
        public async Task<IActionResult> PrintTicketByRole(
            [FromRoute] string ticket,
            [FromQuery] string role = "Inbound")
        {
            if (string.IsNullOrWhiteSpace(ticket))
                return BadRequest(new { error = "ticket is required" });

            var assignment = _printDb.GetAssignment(role);
            string? printerTarget = null;
            if (assignment != null && !string.IsNullOrWhiteSpace(assignment.PrinterId))
            {
                printerTarget = $"{assignment.ServiceId}:{assignment.PrinterId}";
            }

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
