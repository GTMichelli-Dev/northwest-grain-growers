using GrainManagement.Dtos.Warehouse;
using GrainManagement.Models;
using GrainManagement.Reporting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrainManagement.Api
{
    [ApiController]
    [Route("api/printjobs")]
    public class PrintJobsController : ControllerBase
    {
        private readonly dbContext _db;
        private readonly ILogger<PrintJobsController> _log;

        public PrintJobsController(dbContext db, ILogger<PrintJobsController> log)
        {
            _db = db;
            _log = log;
        }

        [HttpGet("load-ticket/{ticket}/pdf")]
        public IActionResult GetLoadTicketPdf([FromRoute] string ticket)
        {
            if (!long.TryParse(ticket, out var transactionId))
                return BadRequest(new { error = "Invalid ticket (expected transaction ID)." });

            var detail = _db.InventoryTransactionDetails
                .Include(d => d.Transaction).ThenInclude(t => t.Location)
                .Include(d => d.Product)
                .Include(d => d.Account)
                .AsNoTracking()
                .FirstOrDefault(d => d.TransactionId == transactionId);

            if (detail == null)
                return NotFound(new { error = $"Transaction {ticket} not found." });

            // Load the destination container (bin) if available
            var toContainer = _db.InventoryTransactionDetailToContainers
                .Include(c => c.Container)
                .AsNoTracking()
                .FirstOrDefault(c => c.TransactionId == transactionId);

            // Load hauler via WeightSheet if linked
            // Note: WeightSheetLoad.InventoryTransactionId is Guid but TransactionId is long —
            // we try to match by WeightSheet.LotId or fall back gracefully.
            string haulerName = "";
            long? weightSheetId = null;
            var ws = _db.WeightSheets
                .Include(w => w.Hauler)
                .AsNoTracking()
                .FirstOrDefault(w => w.LotId == detail.LotId && w.LocationId == detail.Transaction.LocationId);
            if (ws != null)
            {
                haulerName = ws.Hauler?.Description ?? "";
                weightSheetId = ws.WeightSheetId;
            }

            // Load transaction attributes (TruckId, Protein, etc.)
            var attrs = _db.TransactionAttributes
                .Include(a => a.AttributeType)
                .AsNoTracking()
                .Where(a => a.TransactionId == transactionId)
                .ToList();

            string truckId = attrs.FirstOrDefault(a =>
                a.AttributeType?.Code != null &&
                a.AttributeType.Code.Equals("TRUCK_ID", StringComparison.OrdinalIgnoreCase))?.StringValue ?? "";

            decimal protein = attrs.FirstOrDefault(a =>
                a.AttributeType?.Code != null &&
                a.AttributeType.Code.Equals("PROTEIN", StringComparison.OrdinalIgnoreCase))?.DecimalValue ?? 0m;

            var dto = new LoadTicketPrintDto
            {
                Ticket = ticket,
                Location = detail.Transaction?.Location?.Name ?? "",
                DateTimeIn = detail.StartedAt ?? detail.TxnAt,
                DateTimeOut = detail.CompletedAt ?? detail.TxnAt,
                Customer = detail.Account?.EntityName ?? "",
                WeightSheetId = (int)(weightSheetId ?? 0),
                Commodity = detail.Product?.ProductCode ?? "",
                Hauler = haulerName,
                TruckId = truckId,
                Bin = toContainer?.Container?.Description ?? "",
                Protein = protein,
                Gross = (int)(detail.StartQty ?? 0),
                Tare = (int)(detail.EndQty ?? 0),
                Net = (int)(detail.NetQty ?? 0)
            };

            _log.LogInformation("Generating load ticket PDF for transaction {TransactionId}", transactionId);

            var report = new LoadTicketReport();
            report.DataSource = new[] { dto };
            report.CreateDocument();

            if (report.Pages.Count == 0)
                return Problem("Report rendered zero pages.");

            using var ms = new MemoryStream();
            report.ExportToPdf(ms);

            return File(ms.ToArray(), "application/pdf", $"LoadTicket-{ticket}.pdf");
        }

        /// <summary>
        /// Generates a test page PDF using the TestTicketReport (DevExpress XtraReport).
        /// Static content — no data source or parameters needed.
        /// Edit the report layout in the DevExpress Report Designer.
        /// Used by WebPrintService for test prints.
        /// </summary>
        [HttpGet("test-page/pdf")]
        public IActionResult GetTestPagePdf()
        {
            var report = new TestTicketReport();
            report.CreateDocument();

            if (report.Pages.Count == 0)
                return Problem("Report rendered zero pages.");

            using var ms = new MemoryStream();
            report.ExportToPdf(ms);

            return File(ms.ToArray(), "application/pdf", "TestPage.pdf");
        }
    }
}
