using GrainManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using System.Net.Http.Headers;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

namespace GrainManagement
{
    // TEMP: disabled for local testing — re-enable before deploy!
    //[Authorize]
    //[AuthorizeForScopes(Scopes = new[] { "Group.Read.All" })]
    [GrainManagement.Auth.RequiresModule(nameof(GrainManagement.Services.ModuleOptions.DatabaseAdmin))]
    public class AdminController : Controller
    {

        private readonly ICurrentUser _me;

        private readonly ILogger<AdminController> _logger;
        private readonly dbContext _context;
        private readonly ITokenAcquisition _tokenAcq;
        private readonly IHttpClientFactory _http;
        public AdminController(ITokenAcquisition tokenAcq, IHttpClientFactory http, dbContext context, ILogger<AdminController> logger, ICurrentUser me)
        {
            _tokenAcq = tokenAcq;
            _http = http;
            _context = context;
            _logger = logger;
            _me = me;
        }




        // GET: AdminController
        public IActionResult Index()
        {
            if (!_me.IsManager && !_me.IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // GET: Admin/StateCounties
        public IActionResult StateCounties()
        {
            if (!_me.IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // GET: Admin/AccountItemFilters
        public IActionResult AccountItemFilters()
        {
            if (!_me.IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // GET: Admin/AuditTrail
        public IActionResult AuditTrail()
        {
            if (!_me.IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // GET: Admin/Hauling
        public IActionResult Hauling()
        {
            if (!_me.IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // GET: Admin/Haulers
        public IActionResult Haulers()
        {
            if (!_me.IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // GET: Admin/FuelSurcharge
        public IActionResult FuelSurcharge()
        {
            if (!_me.IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // GET: Admin/SystemSettings
        public IActionResult SystemSettings()
        {
            if (!_me.IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // GET: Admin/WeightSheetHaulerRates
        public IActionResult WeightSheetHaulerRates()
        {
            if (!_me.IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // GET: Admin/SplitGroups
        public IActionResult SplitGroups()
        {
            if (!_me.IsAdmin)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

    }
}
