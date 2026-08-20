using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using BasicWeigh.Web.Data;
using BasicWeigh.Web.Hubs;
using BasicWeigh.Web.Services;

namespace BasicWeigh.Web.Controllers;

public class PrinterController : Controller
{
    private readonly ScaleDbContext _db;
    private readonly IHubContext<ScaleHub> _hub;
    private readonly AppSetupCache _setupCache;

    public PrinterController(ScaleDbContext db, IHubContext<ScaleHub> hub, AppSetupCache setupCache)
    {
        _db = db;
        _hub = hub;
        _setupCache = setupCache;
    }

    public IActionResult Index()
    {
        var setup = _setupCache.Get();
        ViewBag.InboundPrinterId = setup.InboundPrinterId ?? "";
        ViewBag.OutboundPrinterId = setup.OutboundPrinterId ?? "";
        return View();
    }

    [HttpPost("api/printers/assignments")]
    public IActionResult SaveAssignments([FromBody] PrinterAssignmentDto dto)
    {
        var setup = _db.AppSetup.First();
        if (dto.InboundPrinterId != null) setup.InboundPrinterId = dto.InboundPrinterId;
        if (dto.OutboundPrinterId != null) setup.OutboundPrinterId = dto.OutboundPrinterId;
        _db.SaveChanges();
        _setupCache.Invalidate();
        return Ok(new { success = true });
    }
}

public class PrinterAssignmentDto
{
    public string? InboundPrinterId { get; set; }
    public string? OutboundPrinterId { get; set; }
}
