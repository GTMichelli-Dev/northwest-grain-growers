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

        // POST /api/printing/device/kiosk-ml-01/print-ticket/T-100234
        [HttpPost("device/{deviceId}/print-ticket/{ticket}")]
        public async Task<IActionResult> PrintTicket([FromRoute] string deviceId, [FromRoute] string ticket)
        {
            if (!PrintHub.TryGetConnection(deviceId, out var connId))
                return NotFound(new { error = "Device not connected", deviceId });

            // Minimal payload: Pi will download the PDF by ticket number.
            await _hub.Clients.Client(connId)
                .SendAsync("PrintLoadTicket", new { ticket });

            return Ok(new { sent = true, deviceId, ticket });
        }
    }
}
