#nullable enable
using GrainManagement.Auth;
using GrainManagement.Services;
using GrainManagement.Services.Print.Data;
using Microsoft.AspNetCore.Mvc;

namespace GrainManagement.Controllers
{
    [RequiresModule(nameof(ModuleOptions.Printers))]
    public class PrinterController : Controller
    {
        private readonly PrintDbContext _printDb;

        public PrinterController(PrintDbContext printDb)
        {
            _printDb = printDb;
        }

        [HttpGet("/Printer")]
        public IActionResult Index() => View();

        [HttpGet("api/printers/assignments")]
        public IActionResult GetAssignments()
        {
            var assignments = _printDb.GetAllAssignments()
                .ToDictionary(a => a.Role, a => new { a.ServiceId, a.PrinterId });

            return Ok(assignments);
        }

        [HttpPost("api/printers/assignments")]
        public IActionResult SaveAssignments([FromBody] PrinterAssignmentDto dto)
        {
            // Save each role. Null/empty = "None" = delete assignment (handled in SaveAssignment).
            // Report is intentionally absent — reports are printed via the
            // browser's PDF preview rather than a server-routed printer.
            SaveRole("Inbound",  dto.InboundPrinterId);
            SaveRole("Outbound", dto.OutboundPrinterId);
            SaveRole("Lot",      dto.LotPrinterId);

            return Ok(new { success = true });
        }

        private void SaveRole(string role, string? printerId)
        {
            if (printerId == null)
            {
                // Not provided in payload — leave existing assignment alone
                return;
            }

            if (string.IsNullOrWhiteSpace(printerId))
            {
                // Explicit None — clear the assignment
                _printDb.SaveAssignment(role, null, null);
                return;
            }

            ParseAssignment(printerId, out var svcId, out var pId);
            _printDb.SaveAssignment(role, svcId, pId);
        }

        /// <summary>
        /// Parses "serviceId:printerId" format (matching BasicWeigh convention).
        /// If no colon, assumes serviceId = "default".
        /// </summary>
        private static void ParseAssignment(string value, out string serviceId, out string printerId)
        {
            var idx = value.IndexOf(':');
            if (idx >= 0)
            {
                serviceId = value[..idx];
                printerId = value[(idx + 1)..];
            }
            else
            {
                serviceId = "default";
                printerId = value;
            }
        }
    }

    public class PrinterAssignmentDto
    {
        public string? InboundPrinterId  { get; set; }
        public string? OutboundPrinterId { get; set; }
        public string? LotPrinterId      { get; set; }
    }
}
