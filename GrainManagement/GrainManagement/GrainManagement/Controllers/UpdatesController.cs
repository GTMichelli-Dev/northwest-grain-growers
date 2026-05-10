using GrainManagement.Auth;
using GrainManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace GrainManagement.Controllers
{
    [RequiresModule(nameof(ModuleOptions.DatabaseAdmin))]
    public class UpdatesController : Controller
    {
        public IActionResult Index() => View();
    }
}
