using GrainManagement.Auth;
using GrainManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace GrainManagement.Controllers
{
    [RequiresModule(nameof(ModuleOptions.GrowerDelivery))]
    public class GrowerDeliveryController : Controller
    {
        public IActionResult Index() => View();

        [HttpGet]
        public IActionResult WeightSheetLots() => View();

        [HttpGet]
        public IActionResult NewWeightSheet() => View();

        [HttpGet]
        public IActionResult EditWeightSheetLot() => View();

        [HttpGet]
        public IActionResult WeightSheetDeliveryLoads() => View();

        [HttpGet]
        public IActionResult WeightSheetTransferLoads()
        {
            ViewData["WsType"] = "transfer";
            return View("WeightSheetDeliveryLoads");
        }

        [HttpGet]
        public IActionResult WeightSheetTransferLoad() => View();
    }
}
