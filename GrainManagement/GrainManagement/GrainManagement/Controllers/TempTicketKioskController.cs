using Microsoft.AspNetCore.Mvc;

namespace GrainManagement.Controllers;

/// <summary>
/// Hosts the full-screen kiosk display. URL shape:
///   /TempTicketKiosk/{kioskId}?scaleId={scaleId}
/// kioskId — must match the KioskId configured on the Pi GPIO service.
/// scaleId — query param so the browser knows which scale's Motion bit
///           to subscribe to. Both come from the operator's setup; the
///           Pi installer can launch the browser at the right URL.
/// </summary>
public sealed class TempTicketKioskController : Controller
{
    [HttpGet("/TempTicketKiosk/{kioskId}")]
    public IActionResult Index(string kioskId, int? scaleId)
    {
        ViewData["KioskId"] = kioskId ?? "";
        ViewData["ScaleId"] = scaleId ?? 0;
        return View();
    }
}
