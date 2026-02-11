using DevExpress.CodeParser;
using GrainManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace GrainManagement.Controllers
{
    public class CameraController : Controller
    {
        private readonly dbContext _ctx;
        public CameraController(dbContext ctx)
        {
            _ctx = ctx;
        }



        // Hard-code for now; later you can move this to appsettings.json.
        private static readonly CameraDef[] Cameras =
        {
            

            new("1", "Kiosk", "1"),
            new("2", "Scale", "2"),
            // add more here
        };

        [HttpGet("/Camera")]
        public IActionResult Index()
        {
            return View(Cameras);
        }

        [HttpGet("/Camera/View/{id}")]
        public IActionResult ViewCamera(string id, bool pop = false)
        {
            var cam = Cameras.FirstOrDefault(c => c.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
            if (cam is null) return RedirectToAction(nameof(Index));

            ViewData["Pop"] = pop; // optional: hide chrome when popped out
            return View("View", cam);
        }

        public record CameraDef(string Id, string Name, string StreamKey);
    }
}
