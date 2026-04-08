using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using BasicWeigh.Web.Data;
using BasicWeigh.Web.Hubs;
using BasicWeigh.Web.Services;

namespace BasicWeigh.Web.Controllers;

public class ScaleController : Controller
{
    private readonly ScaleDbContext _db;
    private readonly IHubContext<ScaleHub> _hub;
    private readonly AppSetupCache _setupCache;

    public ScaleController(ScaleDbContext db, IHubContext<ScaleHub> hub, AppSetupCache setupCache)
    {
        _db = db;
        _hub = hub;
        _setupCache = setupCache;
    }

    // MVC view for Scale Management — all data loads via SignalR
    public IActionResult Index()
    {
        var setup = _setupCache.Get();
        ViewBag.ScaleId = setup.ScaleId ?? "";
        return View();
    }

    // API to save scale assignment
    [HttpPost("api/scales/assignment")]
    public IActionResult SaveAssignment([FromBody] ScaleAssignmentDto dto)
    {
        var setup = _db.AppSetup.First();
        setup.ScaleId = dto.ScaleId;
        _db.SaveChanges();
        _setupCache.Invalidate();
        return Ok(new { success = true });
    }
}

public class ScaleAssignmentDto
{
    public string? ScaleId { get; set; }
}
