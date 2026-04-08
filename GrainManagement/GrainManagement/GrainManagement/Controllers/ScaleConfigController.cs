using GrainManagement.Auth;
using GrainManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace GrainManagement.Controllers
{
    [RequiresModule(nameof(ModuleOptions.Scales))]
    public class ScaleConfigController : Controller
    {
        private readonly ICurrentUser _me;

        public ScaleConfigController(ICurrentUser me)
        {
            _me = me;
        }

        [HttpGet("/ScaleConfig")]
        public IActionResult Index()
        {
            if (!_me.IsAdmin)
                return RedirectToAction("Index", "Home");

            return View();
        }
    }
}
