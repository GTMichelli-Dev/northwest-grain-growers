// Controllers/PrintApiController.cs
using DevExpress.XtraCharts;
using DevExpress.XtraReports.UI;
using DevExpress.XtraRichEdit.Commands;
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
    }







    [HttpPost("GetInvoice")]
    public IActionResult GetInvoice([FromBody] PrintRequest pr)
    {

        pr.Type = pr.Type.ToUpper();
        var dto = new InvoiceDTO
        {
            UID = Guid.NewGuid(),
            Ticket = 123456,
            InvoiceDate = DateTime.Now,
            CustomerName = "3040870 - Valley Agrinomics ",
            PO = "PO-78910",
            BOL = "BOL-54321",
            TruckId = "TRK-001",
            Weighmaster = "Mike Johnson",
            Comments = "Handle with care",
            Location = "Walla Walla Seed Plant",
            Address1 = "456 Seed St",
            Address2 = "Walla Walla, WA 99362",
            Phone = "509-555-1234",
            Type = pr.Type,
            TreatedSeed = pr.TreatedSeed,
            Clearfield = pr.Clearfield,
            Coaxium = pr.Coaxium,
            InvoiceVarietyDTOs = new List<InvoiceVarietyDTO>
            {
                new InvoiceVarietyDTO{ Description="SHINE CERTIFIED SWW", LotNumber="LOT-001-654", ItemId=987656, Percent= 0.01M ,Weight=1200 },
                new InvoiceVarietyDTO{ Description="LCS HYDRAX -1555 COAXIMUM CERTIFIED SWW", LotNumber="LOT-002-34321", ItemId=123456, Percent= 0.223M, Weight=11200 },
                new InvoiceVarietyDTO{ Description="LCS HYDRAX -777 COAXIMUM REGISTERED SWW", LotNumber="LOT-003-45654", ItemId=765432, Percent= 0.2534M, Weight=11200 },
                new InvoiceVarietyDTO{ Description="LCS HYDRAX -777 COAXIMUM REGISTERED SWW", LotNumber="LOT-004-5432", ItemId=765434, Percent= 0.0254M, Weight=11200 },
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
                new InvoiceWeightDTO{ ID=1, Gross= 23000M, Tare= 12000M, Net= 11000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=2, Gross= 28000M, Tare= 15000M, Net= 13000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=3, Gross= 32000M, Tare= 18000M, Net= 14000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=4, Gross= 23000M, Tare= 12000M, Net= 11000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=5, Gross= 28000M, Tare= 15000M, Net= 13000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=6, Gross= 32000M, Tare= 18000M, Net= 14000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=7, Gross= 23000M, Tare= 12000M, Net= 11000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=8, Gross= 28000M, Tare= 15000M, Net= 13000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=9, Gross= 32000M, Tare= 18000M, Net= 14000M, Unit="lbs" },
            },

            invoiceLotDTOs = new List<InvoiceLotDTO>
            {
                new InvoiceLotDTO{ Lot="LOT-12345", ItemId= 123456,  ItemDescription= "SHINE CERTIFIED SWW" },
                new InvoiceLotDTO{ Lot="LOT-23456", ItemId= 234567, ItemDescription= "LCS HYDRAX -1555 COAXIMUM CERTIFIED SWW" },
                new InvoiceLotDTO{ Lot="LOT-34567", ItemId= 345678, ItemDescription= "LCS HYDRAX -777 COAXIMUM REGISTERED SWW" },
                new InvoiceLotDTO{ Lot="LOT-45678", ItemId= 456789, ItemDescription= "LCS HYDRAX -777 COAXIMUM REGISTERED SWW" },
                new InvoiceLotDTO{ Lot="LOT-56789", ItemId= 567890, ItemDescription= "LCS HYDRAX -777 COAXIMUM REGISTERED SWW" },
    
            },

            invoiceAnalysisDTOs = new List<InvoiceAnalysisDTO>
            {
                new InvoiceAnalysisDTO{ DateTested= DateTime.Now.AddDays(-2),  PureSeed= 98.5m, Germination= 92.0m, OtherCrop= 0.5m, WeedSeed= 0.2m, InertMatter= 1.3m, ItemDescription= "Foundation Seed", ItemId=123456 },
                new InvoiceAnalysisDTO{ DateTested= DateTime.Now.AddDays(-5),  PureSeed= 97.2m, Germination= 90.5m, OtherCrop= 0.7m, WeedSeed= 0.3m, InertMatter= 1.8m, ItemDescription= "Certified Seed", ItemId=234567 },
                new InvoiceAnalysisDTO{ DateTested= DateTime.Now.AddDays(-10), PureSeed= 99.0m, Germination= 95.0m, OtherCrop= 0.2m, WeedSeed= 0.1m, InertMatter= 0.7m, ItemDescription= "Registered Seed", ItemId=345678 },
                new InvoiceAnalysisDTO{ DateTested= DateTime.Now.AddDays(-15), PureSeed= 96.8m, Germination= 88.0m, OtherCrop= 1.0m, WeedSeed= 0.4m, InertMatter= 1.8m, ItemDescription= "Treated Seed", ItemId=456789 },
            },
           

        };

      

        if (pr.Type == "TRUCK")
        {
         
            dto.invoiceWeightDTOs = dto.invoiceWeightDTOs.Take(1).ToList();
        }
        
        XtraReport report = new Invoice(dto);
        using (var ms = new MemoryStream())
        {
            report.ExportToPdf(ms);

            System.IO.File.WriteAllBytes(@"C:\Temp\invoice.pdf", ms.ToArray());
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



