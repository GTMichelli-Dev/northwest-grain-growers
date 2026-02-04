using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;



namespace GrainManagement.Controllers
{

    public class SeedController : Controller
    {

        private readonly ICurrentUser _me;

        public SeedController(ICurrentUser me)
        {
            _me = me;
        }

        public IActionResult Index()
        {
            return View();
        }

        // TODO: wire these to the real workflows
        public IActionResult ReceiveGrower() => View();
        public IActionResult ReceiveTransfer() => View();
        public IActionResult Graph()
        {
            var w = _me.IsManager;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View();
        }
    }
}
