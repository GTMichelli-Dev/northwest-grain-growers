using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        public IActionResult WhoAmI()
        {
            var claims = User.Claims
                .Select(c => new { c.Type, c.Value })
                .OrderBy(x => x.Type)
                .ToList();

            return Json(claims);
        }


        private readonly ICurrentUser _me;

        public HomeController(ICurrentUser me)
        {
            _me = me;
        }

        public IActionResult Index()
        {
            // Landing page (Warehouse / Seed / Reports)
            return View();
        }

        /// <summary>
        /// Existing "Location Summary" dashboard.
        /// </summary>
        public IActionResult Dashboard()
        {
            var w = _me.IsManager;
            return View();
        }
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
