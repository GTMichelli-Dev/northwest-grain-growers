using GrainManagement.Dtos.Warehouse;
using Microsoft.AspNetCore.Mvc;
using GrainManagement.Reporting;


namespace GrainManagement.Controllers;

[Route("Warehouse")]
public class WarehouseController : Controller
{
    [HttpGet("")]
    public IActionResult Index() => View();

    [HttpGet("ModePartial")]
    public IActionResult ModePartial(string mode)
    {
        return mode?.ToLowerInvariant() switch
        {
            "intake" => PartialView("_WarehouseIntake"),
            "transfer" => PartialView("_WarehouseTransfer"),
            "outbound" => PartialView("_WarehouseOutbound"),
            _ => PartialView("_WarehouseSelectMode")
        };
    }

    [HttpGet("WeighTruck")]
    public IActionResult WeighTruck() => View();

    [HttpGet("ReceiveGrower")]
    public IActionResult ReceiveGrower() => View();

    [HttpGet("ReceiveTransfer")]
    public IActionResult ReceiveTransfer() => View();

    [HttpGet("ShipLoad")]
    public IActionResult ShipLoad() => View();

    [HttpGet("KioskCamera")]
    public IActionResult KioskCamera() => PartialView("_KioskCamera");

    [HttpGet("TestLoadTicketPdf")]
    public IActionResult TestLoadTicketPdf()
    {
        var dto = new LoadTicketPrintDto
        {
            Ticket = "T-100234",
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

        var pdf = BuildLoadTicketPdf(dto);
        return File(pdf, "application/pdf", $"LoadTicket-{dto.Ticket}.pdf");
    }


    private static byte[] BuildLoadTicketPdf(LoadTicketPrintDto dto)
    {
        var report = new LoadTicketReport();
        report.DataSource = new[] { dto }; // root enumerable
        report.CreateDocument();

        using var ms = new MemoryStream();
        report.ExportToPdf(ms);
        return ms.ToArray();
    }

  


  
}
