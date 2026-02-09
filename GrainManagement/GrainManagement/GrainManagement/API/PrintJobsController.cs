using GrainManagement.Reporting;
using Microsoft.AspNetCore.Mvc;

namespace GrainManagement.Api
{
    [ApiController]
    [Route("api/printjobs")]
    public class PrintJobsController : ControllerBase
    {
        [HttpGet("load-ticket/{ticket}/pdf")]
        public IActionResult GetLoadTicketPdf([FromRoute] string ticket)
        {
            // TODO: replace with real data lookup
            var dto = new GrainManagement.Dtos.Warehouse.LoadTicketPrintDto
            {
                Ticket = ticket,
                Location = "Moses Lake",
                DateTimeIn = DateTime.Now.AddMinutes(-18),
                DateTimeOut = DateTime.Now,
                Customer = "MADISON RANCHES",
                WeightSheetId = 12345,
                Commodity = "SWW",
                Hauler = "MCGREGORS",
                TruckId = "TRK-17",
                Bin = "01 - White Tank",
                Protein = 9.7m,
                Gross = 84120,
                Tare = 31400,
                Net = 52720
            };

            var report = new LoadTicketReport();
            report.DataSource = new[] { dto };
            report.CreateDocument();

            if (report.Pages.Count == 0)
                return Problem("Report rendered zero pages.");

            using var ms = new MemoryStream();
            report.ExportToPdf(ms);

            return File(ms.ToArray(), "application/pdf", $"LoadTicket-{ticket}.pdf");
        }
    }
}
