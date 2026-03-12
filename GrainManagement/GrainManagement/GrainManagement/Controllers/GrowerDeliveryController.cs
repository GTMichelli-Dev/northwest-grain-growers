using Microsoft.AspNetCore.Mvc;

namespace GrainManagement.Controllers
{
    public class GrowerDeliveryController : Controller
    {
        public IActionResult Index() => View();

        [HttpGet]
        public IActionResult WeightSheetLots() => View();
    }
}
