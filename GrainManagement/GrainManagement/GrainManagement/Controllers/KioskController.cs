using Microsoft.AspNetCore.Mvc;

namespace GrainManagement.Controllers
{
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
