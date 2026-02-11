using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using GrainManagement.Hubs;

namespace GrainManagement.Api
{
    [ApiController]
    [Route("api/printing")]
    public class PrintingController : ControllerBase
    {
        private readonly IHubContext<PrintHub> _hub;

        public PrintingController(IHubContext<PrintHub> hub)
        {
            _hub = hub;
        }

        [HttpPost("printer/{printerId}/print-ticket/{ticket}")]
        public async Task<IActionResult> PrintTicket([FromRoute] string printerId, [FromRoute] string ticket)
        {
            if (string.IsNullOrWhiteSpace(printerId))
                return BadRequest(new { error = "printerId is required" });

            if (string.IsNullOrWhiteSpace(ticket))
                return BadRequest(new { error = "ticket is required" });

            // Look up kiosk connection by printerId
            if (!PrintHub.TryGetConnection(printerId, out var connId))
                return NotFound(new { error = "Printer kiosk not connected", printerId });

            // Send print command + printerId (kiosk will verify it matches itself)
            await _hub.Clients.Client(connId)
                .SendAsync("PrintLoadTicket", new { ticket, printerId });

            return Ok(new { sent = true, printerId, ticket });
        }
    }
}
