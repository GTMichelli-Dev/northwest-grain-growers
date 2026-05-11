using Microsoft.AspNetCore.Mvc;

namespace GrainManagement.Controllers;

/// <summary>
/// MVC controller for the operator-facing temp-ticket management view.
/// All data flows through the existing /api/temp-tickets REST surface;
/// this controller just renders the Razor shell.
/// </summary>
public sealed class TempTicketsManagementController : Controller
{
    [HttpGet("/TempTickets")]
    public IActionResult Index() => View();
}
