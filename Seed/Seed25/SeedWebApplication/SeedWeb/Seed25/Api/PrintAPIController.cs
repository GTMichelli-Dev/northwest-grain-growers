// Controllers/PrintApiController.cs
using DevExpress.XtraReports.UI;
using Microsoft.AspNetCore.Mvc;
using Seed25.DTO;
using Seed25.Report;
using System.Drawing.Printing; // for listing printers (GET endpoint)
using System.Linq;
using System.Net.Sockets;

[ApiController]
[Route("api/[controller]")]

public class PrintApiController : ControllerBase
{



    [HttpGet("GetTestReport")]
    public IActionResult GetTestReport()
    {
        var dto = new InboundTicketDTO
        {
            UID = Guid.NewGuid(),
            Ticket = 123456,
            Weight = 2800,
            TimeIn = DateTime.Now,
            Plant = "Wasco Seed Plant.",
            Phone = "541-442-5555 ",
            Prompt1 = "",
            Prompt2 = ""
        };

        XtraReport report = new TestReport(dto);
        using (var ms = new MemoryStream())
        {
            report.ExportToPdf(ms);
            

            System.IO.File.WriteAllBytes(@"C:\Temp\TestReport.pdf", ms.ToArray());
            return Ok();

        }
    }



    [HttpPost("PrintInboundTicket")]
    public IActionResult PrintInboundTicket([FromBody] InboundTicketDTO dto)
    {
      

        XtraReport report = new InboundTicket(dto);
        //report.Print("Kiosk");
        using (var ms = new MemoryStream())
        {
            report.ExportToPdf(ms);


            System.IO.File.WriteAllBytes(@"C:\Temp\InboundTicket.pdf", ms.ToArray());
            return Ok();

        }
    }
}



