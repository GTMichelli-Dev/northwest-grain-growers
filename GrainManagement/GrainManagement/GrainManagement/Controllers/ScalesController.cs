using GrainManagement.Auth;
using GrainManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace GrainManagement.Controllers
{
    [RequiresModule(nameof(ModuleOptions.Scales))]
    public class ScalesController : Controller
    {
      
            [HttpGet("/Scales")]
            public IActionResult Index()
            {
                return View();
            }
        
    }
}
