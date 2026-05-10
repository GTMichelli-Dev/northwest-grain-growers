using GrainManagement.Auth;
using GrainManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace GrainManagement.Controllers
{
    /// <summary>
    /// Field-location kiosk page (served at /Kiosk). Walks the operator
    /// through a one-time cookie-based setup of LocationId / PrinterId /
    /// ScaleId / KioskType the first time it's opened, then runs as a
    /// SignalR-driven weight kiosk visually modeled on BasicWeigh's kiosk.
    ///
    /// Gated to the Scales module so it only renders on Remote deployments
    /// (matching the /api/Scale/Locations + /api/Scale/CachedScales
    /// endpoints the page calls during setup).
    /// </summary>
    [RequiresModule(nameof(ModuleOptions.Scales))]
    public class KioskController : Controller
    {
        private readonly IConfiguration _config;
        public KioskController(IConfiguration config) => _config = config;

        public IActionResult Index()
        {
            var themeKey = _config["Branding:ThemeKey"] ?? "default";
            HttpContext.Items["ThemeKey"] = themeKey;
            return View();
        }
    }
}
