using GrainManagement.Auth;
using GrainManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace GrainManagement.Controllers;

[Route("SeedAdmin")]
[RequiresModule(nameof(ModuleOptions.SeedAdmin))]
public class SeedAdminController : Controller
{
    [HttpGet("")]
    public IActionResult Index() => View();

    // ── Raw Seed ─────────────────────────────────────────────────────────────
    [HttpGet("Raw")]
    public IActionResult Raw() => View();

    [HttpGet("PurchaseOrders")]
    public IActionResult PurchaseOrders() => View();

    [HttpGet("Lots")]
    public IActionResult Lots() => View();

    [HttpGet("Receipts")]
    public IActionResult Receipts() => View();

    // ── Treated & Cleaned Seed ───────────────────────────────────────────────
    [HttpGet("Treated")]
    public IActionResult Treated() => View();

    [HttpGet("Treatments")]
    public IActionResult Treatments() => View();

    [HttpGet("Sales")]
    public IActionResult Sales() => View();
}
