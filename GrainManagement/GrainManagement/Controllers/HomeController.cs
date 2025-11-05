using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;



namespace GrainManagement.Controllers
{

    public class HomeController : Controller
    {

        private readonly ICurrentUser _me;

        public HomeController(ICurrentUser me)
        {
            _me = me;
        }

        public IActionResult Index()
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
