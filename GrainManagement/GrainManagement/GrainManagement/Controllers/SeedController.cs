using GrainManagement.Auth;
using GrainManagement.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;



namespace GrainManagement.Controllers
{
    [RequiresModule(nameof(ModuleOptions.Seed))]
    public class SeedController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // TODO: wire these to the real workflows
        public IActionResult ReceiveGrower() => View();
        public IActionResult ReceiveTransfer() => View();
        public IActionResult Graph() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View();
        }
    }
}
