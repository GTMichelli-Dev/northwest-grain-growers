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




    private InvoiceDTO GetSampleData(PrintRequest pr)
    {
        pr.Type = pr.Type.ToUpper();
        if (pr.Type == "STRING") pr.Type = "TRUCK";
        var dto = new InvoiceDTO
        {
            UID = Guid.NewGuid(),
            Ticket = 123456,
            InvoiceDate = DateTime.Now,
            CustomerName = "3040870 - Valley Agrinomics ",
            PO = "ABC-78910",
            BOL = "ZXQ-345-54321",
            TruckId = "TRK-001",
            Weighmaster = "Mike Johnson",
            Comments = "This is a comment Line ",
            Location = "Walla Walla Seed Plant",
            Address1 = "456 Seed St",
            Address2 = "Walla Walla, WA 99362",
            Phone = "509-555-1234",
            Type = pr.Type,
            RequestedAmount = "Requested 1500 bu.",
            TreatedSeed = pr.TreatedSeed,
            Clearfield = pr.Clearfield,
            Coaxium = pr.Coaxium,
            InvoiceVarietyDTOs = new List<InvoiceVarietyDTO>
            {
                new InvoiceVarietyDTO{ Description="SHINE CERTIFIED SWW", LotNumber="001-654", ItemId=987656, Percent= 0.01M ,Weight=1200 },
                new InvoiceVarietyDTO{ Description="LCS HYDRAX -1555 COAXIMUM CERTIFIED SWW", LotNumber="002-34321", ItemId=123456, Percent= 0.223M, Weight=11200 },
                new InvoiceVarietyDTO{ Description="LCS HYDRAX -777 COAXIMUM REGISTERED SWW", LotNumber="003-45654", ItemId=765432, Percent= 0.2534M, Weight=11200 },
                new InvoiceVarietyDTO{ Description="LCS HYDRAX -777 COAXIMUM REGISTERED SWW", LotNumber="004-5432", ItemId=765434, Percent= 0.0254M, Weight=11200 },
                //new InvoiceVarietyDTO{ Description="LCS HYDRAX -777 COAXIMUM REGISTERED SWW", ItemId=765423, Percent= 0.4M, Weight=11200 },
                //new InvoiceVarietyDTO{ Description="Shine Certified", ItemId=987656, Percent= 0.02M, Weight=1200 },
                //new InvoiceVarietyDTO{ Description="LCS HYDRAX -1555 COAXIMUM CERTIFIED SWW", ItemId=162345, Percent= 0.33M, Weight=11200 },
                //new InvoiceVarietyDTO{ Description="LCS HYDRAX -777 COAXIMUM REGISTERED SWW", ItemId=763454, Percent= 0.03M, Weight=11200 },
                //new InvoiceVarietyDTO{ Description="LCS HYDRAX -777 COAXIMUM REGISTERED SWW", ItemId=764354, Percent= 0.20M, Weight=11200 },
                //new InvoiceVarietyDTO{ Description="LCS AST", ItemId=732654, Percent= 0.100111M, Weight=1 },
            },

            invoiceTreatmentDTOs = new List<InvoiceTreatmentDTO>
            {
                new InvoiceTreatmentDTO{ Description="Apron XL", ItemId =111222, Rate=1.23M, Gallons=12.10M },
                new InvoiceTreatmentDTO{ Description="Helix", ItemId=333444, Rate=0.23M, Gallons=2.10M },
                new InvoiceTreatmentDTO{ Description="Inspire", ItemId=555666, Rate=4.23M, Gallons=882.10M },
                new InvoiceTreatmentDTO{ Description="Seed Start", ItemId =111222, Rate=1.23M, Gallons=12.10M },
                new InvoiceTreatmentDTO{ Description="Albaugh", ItemId=333444, Rate=0.23M, Gallons=2.10M },
                new InvoiceTreatmentDTO{ Description="Legend", ItemId=555666, Rate=4.23M, Gallons=882.10M },
            },
            invoiceMiscDTOs = new List<InvoiceMiscDTO>
            {
                new InvoiceMiscDTO{ ItemID=543212, Description="Loading Fee", Unit="each", Amount= 25.00M },
                new InvoiceMiscDTO{ ItemID=543212, Description="Bags",Unit="each", Amount= 15.00M },
                new InvoiceMiscDTO{ ItemID=543212, Description="Palletts",Unit="each", Amount= 1M },
            },
            invoiceWeightDTOs = new List<InvoiceWeightDTO>
            {
                new InvoiceWeightDTO{  ID=12345678, Gross= 23000M, Tare= 12000M, Net= 11000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=12345679, Gross= 28000M, Tare= 15000M, Net= 13000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=12345680, Gross= 32000M, Tare= 18000M, Net= 14000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=12345681, Gross= 23000M, Tare= 12000M, Net= 11000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=12345682, Gross= 28000M, Tare= 15000M, Net= 13000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=12345683, Gross= 32000M, Tare= 18000M, Net= 14000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=12345684, Gross= 23000M, Tare= 12000M, Net= 11000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=12345678, Gross= 28000M, Tare= 15000M, Net= 13000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=9, Gross= 32000M, Tare= 18000M, Net= 14000M, Unit="lbs" },
                 new InvoiceWeightDTO{ ID=3, Gross= 32000M, Tare= 18000M, Net= 14000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=4, Gross= 23000M, Tare= 12000M, Net= 11000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=5, Gross= 28000M, Tare= 15000M, Net= 13000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=6, Gross= 32000M, Tare= 18000M, Net= 14000M, Unit="lbs" },
                 new InvoiceWeightDTO{ ID=5, Gross= 28000M, Tare= 15000M, Net= 13000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=6, Gross= 32000M, Tare= 18000M, Net= 14000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=7, Gross= 23000M, Tare= 12000M, Net= 11000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=8, Gross= 28000M, Tare= 15000M, Net= 13000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=9, Gross= 32000M, Tare= 18000M, Net= 14000M, Unit="lbs" },
                 new InvoiceWeightDTO{ ID=3, Gross= 32000M, Tare= 18000M, Net= 14000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=4, Gross= 23000M, Tare= 12000M, Net= 11000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=9, Gross= 32000M, Tare= 18000M, Net= 14000M, Unit="lbs" },
                 new InvoiceWeightDTO{ ID=3, Gross= 32000M, Tare= 18000M, Net= 14000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=4, Gross= 23000M, Tare= 12000M, Net= 11000M, Unit="lbs" },
            },
            invoiceBagsDTOs = new List<InvoiceBagDTO>
            {
                new InvoiceBagDTO{ ID=1, Weight= 50M, Quantity= 100M, Unit="lbs" },
                new InvoiceBagDTO{ ID=2, Weight= 25M, Quantity= 35M, Unit="lbs" },
                new InvoiceBagDTO{ ID=3, Weight= 15.25M, Quantity= 1M, Unit="lbs" },
            },

            invoiceLotDTOs = new List<InvoiceLotDTO>
            {
                new InvoiceLotDTO{ Lot="ABCD-12345", ItemId= 123456,  ItemDescription= "SHINE CERTIFIED SWW" },
                new InvoiceLotDTO{ Lot="AODFD-23456", ItemId= 234567, ItemDescription= "LCS HYDRAX -1555 COAXIMUM CERTIFIED SWW" },
                new InvoiceLotDTO{ Lot="VFG-34567", ItemId= 345678, ItemDescription= "LCS HYDRAX -777 COAXIMUM REGISTERED SWW" },
                new InvoiceLotDTO{ Lot="WSUD-45678", ItemId= 456789, ItemDescription= "LCS HYDRAX -777 COAXIMUM REGISTERED SWW" },
                new InvoiceLotDTO{ Lot="IODdd-56789", ItemId= 567890, ItemDescription= "LCS HYDRAX -777 COAXIMUM REGISTERED SWW" },

            },



        };

        dto.invoiceAnalysisDTOs = dto.InvoiceVarietyDTOs.Select((v, i) => new InvoiceAnalysisDTO
        {
            DateTested = DateTime.Now.AddDays(-2 - i * 3),
            PureSeed = .95m - i * 0.1m,
            Germination = 1,
            OtherCrop = 0.5m + i * 0.1m,
            WeedSeed = 0.2m + i * 0.05m,
            InertMatter = .13m + i * 0.2m,
            ItemDescription = v.Description,
            ItemId = v.ItemId
        }).ToList();


        if (pr.Type == "TRUCK")
        {

            dto.invoiceWeightDTOs = dto.invoiceWeightDTOs.Take(1).ToList();
            dto.invoiceWeightDTOs[0].TruckId = dto.TruckId;
        }
        return dto;
    }


    [HttpPost("GetInvoice")]
    public IActionResult GetInvoice([FromBody] PrintRequest pr)
    {
        var dto= GetSampleData(pr);

        XtraReport report = new Invoice(dto);
        using (var ms = new MemoryStream())
        {
            report.ExportToPdf(ms);

            System.IO.File.WriteAllBytes(@"C:\Temp\LetterInvoice.pdf", ms.ToArray());
            return Ok();

        }

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

            var dto = GetSampleData(pr);

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
        var dto = GetSampleData(pr);

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
        var dto = GetSampleData(pr);
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





