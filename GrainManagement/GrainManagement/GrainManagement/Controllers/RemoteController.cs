using Microsoft.AspNetCore.Mvc;

namespace GrainManagement.Controllers
{
    public class RemoteController : Controller
    {
        public IActionResult Index() => View();
    }
}
