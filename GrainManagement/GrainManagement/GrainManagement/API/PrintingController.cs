using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using GrainManagement.Hubs;
using GrainManagement.Services.Print;

namespace GrainManagement.Api
{
    [ApiController]
    [Route("api/printing")]
    public class PrintingController : ControllerBase
    {
        private readonly IHubContext<PrintHub> _hub;
        private readonly IPrintDispatchService _dispatch;
        private readonly ILogger<PrintingController> _log;

        public PrintingController(
            IHubContext<PrintHub> hub,
            IPrintDispatchService dispatch,
            ILogger<PrintingController> log)
        {
            _hub = hub;
            _dispatch = dispatch;
            _log = log;
        }

        [HttpPost("printer/{printerId}/print-ticket/{ticket}")]
        public async Task<IActionResult> PrintTicket([FromRoute] string printerId, [FromRoute] string ticket)
        {
            if (string.IsNullOrWhiteSpace(printerId))
                return BadRequest(new { error = "printerId is required" });

            if (string.IsNullOrWhiteSpace(ticket))
                return BadRequest(new { error = "ticket is required" });

            // Legacy direct route: kiosk connection registered by exact printerId/deviceId
            if (!PrintHub.TryGetConnection(printerId, out var connId))
            {
                // BasicWeigh-style route: serviceId:printerId (or just printerId)
                await _dispatch.DispatchTicketAsync(ticket, printerId, "weighout");
                return Ok(new { sent = true, printerId, ticket, mode = "group" });
            }

            // Send print command + printerId (kiosk can verify it matches itself)
            await _hub.Clients.Client(connId)
                .SendAsync("PrintLoadTicket", new { ticket, printerId });

            return Ok(new { sent = true, printerId, ticket, mode = "direct" });
        }

        [HttpPost("print-ticket/{ticket}")]
        public async Task<IActionResult> PrintTicketByGroup([FromRoute] string ticket, [FromQuery] string? printerId = null, [FromQuery] string type = "weighout")
        {
            if (string.IsNullOrWhiteSpace(ticket))
                return BadRequest(new { error = "ticket is required" });

            try
            {
                await _dispatch.DispatchTicketAsync(ticket, printerId, type);
                return Ok(new { sent = true, ticket, printerId, type, mode = "group" });
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Failed to dispatch print ticket. Ticket={Ticket} Printer={Printer}", ticket, printerId);
                return StatusCode(500, new { error = "Failed to dispatch print request" });
            }
        }
    }
}
