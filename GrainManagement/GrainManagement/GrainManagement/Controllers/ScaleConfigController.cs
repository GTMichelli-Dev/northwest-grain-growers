using GrainManagement.Auth;
using GrainManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace GrainManagement.Controllers
{
    [RequiresModule(nameof(ModuleOptions.Scales))]
    public class ScaleConfigController : Controller
    {
        [HttpGet("/ScaleConfig")]
        public IActionResult Index() => View();
    }
}
