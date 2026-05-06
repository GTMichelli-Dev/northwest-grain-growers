using GrainManagement.Auth;
using GrainManagement.Dtos.Warehouse;
using GrainManagement.Reporting;
using GrainManagement.Services;
using GrainManagement.Services.Warehouse;
using Microsoft.AspNetCore.Mvc;


namespace GrainManagement.Controllers;

[Route("WeightSheets")]
[RequiresModule(nameof(ModuleOptions.WarehouseIntake))]
public class WarehouseController : Controller
{
    private readonly IWarehouseDashboardService _dash;
    private readonly ILocationContext _locationContext;

    public WarehouseController(IWarehouseDashboardService dash, ILocationContext locationContext)
    {
        _dash = dash;
        _locationContext = locationContext;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        var vm = _dash.GetDashboard(locationId: _locationContext.LocationId);
        return View(vm);
    }

    [HttpGet("ModePartial")]
    public IActionResult ModePartial(string mode)
    {
        return mode?.ToLowerInvariant() switch
        {
            "intake" => PartialView("_WarehouseIntake"),
            "transfer" => PartialView("_WarehouseTransfer"),
            "outbound" => PartialView("_WarehouseOutbound"),
            "newtruck" => PartialView("_NewIntakeTruck"),
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



    [HttpGet("LoadType")]
    public IActionResult LoadType() => View();

    [HttpGet("Transfer")]
    public IActionResult Transfer() => View();

    [HttpGet("NewIntakeTruck")]
    public IActionResult _NewIntakeTruck()
    {
        return PartialView("_NewIntakeTruck");
    }   


}

