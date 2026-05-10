using GrainManagement.Auth;
using GrainManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace GrainManagement.Controllers;

[RequiresModule(nameof(ModuleOptions.DatabaseAdmin))]
[Route("AgvantageWarehouseTransfer")]
public sealed class AgvantageWarehouseTransferController : Controller
{
    [HttpGet("")]
    public IActionResult Index() => View();
}
