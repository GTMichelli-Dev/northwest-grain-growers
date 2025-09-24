using Microsoft.AspNetCore.Mvc;

namespace Seed25.Controllers
{
    public class KioskController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
