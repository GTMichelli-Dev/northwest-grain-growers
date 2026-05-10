using Microsoft.AspNetCore.Mvc;

namespace GrainManagement
{
    public class ItemsController : Controller
    {
        public IActionResult Index() => View();
    }
}
