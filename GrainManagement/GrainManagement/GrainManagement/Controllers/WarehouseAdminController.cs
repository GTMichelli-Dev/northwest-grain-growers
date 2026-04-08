using GrainManagement.Auth;
using GrainManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace GrainManagement.Controllers;

[Route("WarehouseAdmin")]
[RequiresModule(nameof(ModuleOptions.WarehouseAdmin))]
public class WarehouseAdminController : Controller
{
    [HttpGet("")]
    public IActionResult Index() => View();

    // ── Intake ───────────────────────────────────────────────────────────────
    [HttpGet("Intake")]
    public IActionResult Intake() => View();

    [HttpGet("WeightSheets")]
    public IActionResult WeightSheets() => View();

    [HttpGet("Lots")]
    public IActionResult Lots() => View();

    [HttpGet("Loads")]
    public IActionResult Loads() => View();

    // ── Transfers ─────────────────────────────────────────────────────────────
    [HttpGet("Transfers")]
    public IActionResult Transfers() => View();

    [HttpGet("TransferWeightSheets")]
    public IActionResult TransferWeightSheets() => View();

    [HttpGet("TransferLoads")]
    public IActionResult TransferLoads() => View();
}
