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
        private readonly ICurrentUser _me;
        private readonly PrintDbContext _printDb;

        public PrinterController(ICurrentUser me, PrintDbContext printDb)
        {
            _me = me;
            _printDb = printDb;
        }

        [HttpGet("/Printer")]
        public IActionResult Index()
        {
            if (!_me.IsAdmin)
                return RedirectToAction("Index", "Home");

            return View();
        }

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
            if (dto.InboundPrinterId != null)
            {
                ParseAssignment(dto.InboundPrinterId, out var svcId, out var pId);
                _printDb.SaveAssignment("Inbound", svcId, pId);
            }
            if (dto.OutboundPrinterId != null)
            {
                ParseAssignment(dto.OutboundPrinterId, out var svcId, out var pId);
                _printDb.SaveAssignment("Outbound", svcId, pId);
            }

            return Ok(new { success = true });
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
        public string? InboundPrinterId { get; set; }
        public string? OutboundPrinterId { get; set; }
    }
}
