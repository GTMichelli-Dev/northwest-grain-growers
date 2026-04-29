using GrainManagement.Services;
using GrainManagement.Services.Warehouse;
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

        // TEMP: disabled for local testing
        //[Authorize]
        public IActionResult WhoAmI()
        {
            var claims = User.Claims
                .Select(c => new { c.Type, c.Value })
                .OrderBy(x => x.Type)
                .ToList();

            return Json(claims);
        }


        private readonly ICurrentUser _me;
        private readonly IModuleContext _modules;
        private readonly IWarehouseDashboardService _dash;
        private readonly ILocationContext _locationContext;

        public HomeController(
            ICurrentUser me,
            IModuleContext modules,
            IWarehouseDashboardService dash,
            ILocationContext locationContext)
        {
            _me = me;
            _modules = modules;
            _dash = dash;
            _locationContext = locationContext;
        }

        public IActionResult Index()
        {
            // On Remote deployments the root URL renders the WeightSheets dashboard
            // directly. The legacy tiles landing page is moved to /RemoteAdmin
            // (PIN-gated, requires PrivilegeId=7).
            if (_modules.IsRemote)
            {
                var vm = _dash.GetDashboard(locationId: _locationContext.LocationId);
                return View("~/Views/Warehouse/Index.cshtml", vm);
            }

            // Central / Reporting deployments keep the original landing page.
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
