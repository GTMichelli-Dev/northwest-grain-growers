// Controllers/PrintApiController.cs
using DevExpress.XtraCharts;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using DevExpress.XtraRichEdit.Commands;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Seed25.DTO;
using Seed25.Report;
using System.Drawing.Printing; // for listing printers (GET endpoint)
using System.Linq;



using MailKit.Net.Smtp;
using MailKit.Security;

using System.Net.Mail;
using System.IO;
using System.Threading.Tasks;

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

    [HttpGet("ListPrinters")]
    public IActionResult ListPrinters()
    {

        var printers = PrinterSettings.InstalledPrinters.Cast<string>().ToList();
        return Ok(printers);
    }


    public class PrintRequest
    {

        public bool TreatedSeed { get; set; } = true;
        public bool Clearfield { get; set; } = false;
        public bool Coaxium { get; set; } = false;
        public string Type { get; set; } = "Truck";
        public string Email { get; set; }
    }






    [HttpPost("GetInvoice")]
    public IActionResult GetInvoice([FromBody] PrintRequest pr)
    {
        var dto= SampleData.GetSampleData(pr);

        XtraReport report = new Invoice(dto);
        using (var ms = new MemoryStream())
        {
            report.ExportToPdf(ms);

            System.IO.File.WriteAllBytes(@"C:\Temp\LetterInvoice.pdf", ms.ToArray());
            return Ok();

        }

    }



    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<MemoryStream> GetPictureInvoiceEmbed(long TicketNumber ,bool KioskTicket)
    {
        var dto = SampleData.GetSampleData(TicketNumber);

        XtraReport report;
        if (KioskTicket)
            report = new KioskInvoice(dto);
        else
            report = new Invoice(dto);

        var overlayMs = new MemoryStream();
        await report.ExportToImageAsync(overlayMs);
        overlayMs.Position = 0;
        return overlayMs;
    }

    [HttpPost("EmailInvoice")]
    public async Task<IActionResult> EmailInvoice([FromBody] PrintRequest pr)
    {
        try
        {
            if (string.IsNullOrEmpty(pr.Email))
            {
                return BadRequest(new { error = "Email is required." });
            }

            var dto = SampleData.GetSampleData(pr);

            var report = new Invoice(dto);
            report.DisplayName = $"Seed_Invoice_{dto.Ticket}_{dto.CustomerName}_{DateTime.Now:yyyyMMdd}";
            report.CreateDocument(); // ensure the report is built before exporting

            // Step 1: export HTML mail (async)
            var options = new MailMessageExportOptions
            {
                ExportMode = HtmlExportMode.SingleFile,
                TableLayout = true,
                ExportWatermarks = false
            };

            // IMPORTANT: set the real "From" that matches your authenticated user or a valid Send-As
            var fromAddress = "noreply@nwgrgr.com";
            var toAddress = pr.Email; // Replace with actual recipient or pull from request
            var mailMsg = await report.ExportToMailAsync(
                options,
                fromAddress,
                toAddress,
                $"Invoice {report.DisplayName ?? report.Name}",
                CancellationToken.None
            );

            //// Step 1b: add recipients (REQUIRED)
            //// Replace with your real recipients or pull from the request
            //mailMsg.To.Add(new System.Net.Mail.MailAddress("you@example.com"));
            //// Optionally CC/BCC
            //// mailMsg.CC.Add(new System.Net.Mail.MailAddress("ap@example.com"));

            // Step 2: attach PDF copy (optional but recommended)
            // Copy into a byte[] so the attachment stream is independent of object disposal timing.
            using (var pdf = new MemoryStream())
            {
                report.ExportToPdf(pdf);
                var pdfBytes = pdf.ToArray();
                var pdfStream = new MemoryStream(pdfBytes, writable: false);
                mailMsg.Attachments.Add(
                    new Attachment(pdfStream, $"{report.DisplayName ?? "Report"}.pdf", "application/pdf")
                );
            }

            // Step 3: convert to MimeMessage for MailKit
            var mime = MimeMessage.CreateFromMailMessage(mailMsg);

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            await smtp.ConnectAsync("mail.smtp2go.com", 25, MailKit.Security.SecureSocketOptions.StartTls);

            // IMPORTANT: the username should match the From or have Send-As permission
            var smtpUser = "NWGG";
            var smtpPass = "NWFyMHN3aW91b2Iw"; // store in KeyVault/User-Secrets
            await smtp.AuthenticateAsync(smtpUser, smtpPass);

            await smtp.SendAsync(mime);
            await smtp.DisconnectAsync(true);

            return Ok(new { sent = true, to = string.Join(",", mailMsg.To.Select(x => x.Address)), subject = mailMsg.Subject });
        }
        catch (Exception ex)
        {
            // Surface enough detail to debug (but avoid leaking secrets)
            return StatusCode(500, new
            {
                sent = false,
                error = ex.Message,
                // ex.StackTrace is useful during dev; remove or guard in production
            });
        }
    }





            [HttpPost("PrintInvoice")]
    public IActionResult PrintInvoice([FromBody] PrintRequest pr)
    {
        var dto = SampleData.GetSampleData(pr);

        XtraReport report = new Invoice(dto);
        using (var ms = new MemoryStream())
        {
            report.Print("ReportPrinter");
            return Ok();

        }

    }


    [HttpPost("GetKioskInvoice")]
    public IActionResult GetKioskInvoice([FromBody] PrintRequest pr)
    {
        var dto = SampleData.GetSampleData(pr);
        XtraReport report = new KioskInvoice(dto);
        using (var ms = new MemoryStream())
        {
            report.ExportToPdf(ms);

            System.IO.File.WriteAllBytes(@"C:\Temp\KioskInvoice.pdf", ms.ToArray());
            return Ok();

        }

    }






    [HttpPost("PrintInboundTicket")]
    public IActionResult PrintInboundTicket([FromBody] InboundTicketDTO dto)
    {


        XtraReport report = new InboundTicket(dto);
        report.Print("Kiosk");
        using (var ms = new MemoryStream())
        {
            report.ExportToPdf(ms);


            System.IO.File.WriteAllBytes(@"C:\Temp\InboundTicket.pdf", ms.ToArray());
            return Ok();

        }
    }
}





