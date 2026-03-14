using Microsoft.AspNetCore.Mvc;

namespace GrainManagement.Controllers
{
    [Route("System")]
    public class SystemDashboardController : Controller
    {
        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {
            return View("~/Views/SystemHub/Index.cshtml");
        }
    }
}
