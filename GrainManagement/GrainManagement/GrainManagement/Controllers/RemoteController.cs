using GrainManagement.Models;
using Microsoft.AspNetCore.Mvc;
using static GrainManagement.Controllers.CameraController;

namespace GrainManagement.Controllers
{
    public class RemoteController : Controller
    {
        private readonly dbContext _ctx;
        public RemoteController(dbContext ctx)
        {
            _ctx = ctx;
        }   

        public IActionResult Index()
        {
            return View(Remotes);
        }


        public record RemoteDef(string Id, string Name, string Url);
        private static readonly RemoteDef[] Remotes =
      {

            new("1", "Town", "Home/Index"),
            new("2", "Central Ferry", "Home/Index"),
            new("3", "CFTA", "Home/Index")



            // add more here
        };
    }
}
