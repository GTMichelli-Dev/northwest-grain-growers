using Microsoft.AspNetCore.Mvc;

namespace GrainManagement.Controllers
{
    public class ScalesController : Controller
    {
      
            [HttpGet("/Scales")]
            public IActionResult Index()
            {
                return View();
            }
        
    }
}
