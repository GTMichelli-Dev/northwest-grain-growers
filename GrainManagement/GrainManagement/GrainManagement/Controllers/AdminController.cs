using GrainManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GrainManagement
{
    /// <summary>
    /// Central deployment maintenance pages — gated by an Office Admin PIN
    /// (PrivilegeId=14) cookie set via /api/Auth/ValidateOfficeAdminPin.
    /// Pages redirect to /Admin/Pin when the cookie is missing, mirroring
    /// the RemoteAdmin pattern. Logout is intentionally ungated so the
    /// navbar Logout link can clear the cookie even if module config
    /// changes mid-session.
    /// </summary>
    [GrainManagement.Auth.RequiresModule(nameof(GrainManagement.Services.ModuleOptions.DatabaseAdmin))]
    public class AdminController : Controller
    {
        public const string OfficeAdminCookieName         = "GrainMgmt_OfficeAdminPin";
        public const string OfficeAdminUserNameCookieName = "GrainMgmt_OfficeAdminUser";

        private readonly ILogger<AdminController> _logger;
        private readonly dbContext _context;

        public AdminController(dbContext context, ILogger<AdminController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Pre-execution gate. Every action except Pin / Logout requires the
        // Office Admin cookie. Missing → redirect to /Admin/Pin, preserving
        // the original target via ?returnUrl= so the Pin page can bounce
        // the operator back where they were headed.
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var actionName = (context.ActionDescriptor.RouteValues.TryGetValue("action", out var a) ? a : "")
                ?? "";
            if (string.Equals(actionName, nameof(Pin), System.StringComparison.OrdinalIgnoreCase) ||
                string.Equals(actionName, nameof(Logout), System.StringComparison.OrdinalIgnoreCase))
                return;

            if (Request.Cookies.ContainsKey(OfficeAdminCookieName))
                return;

            var returnUrl = Request.Path + Request.QueryString;
            context.Result = RedirectToAction(nameof(Pin), new { returnUrl = returnUrl.ToString() });
        }

        // GET: Admin/Pin
        [HttpGet]
        public IActionResult Pin(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = string.IsNullOrWhiteSpace(returnUrl) ? "/Admin" : returnUrl;
            return View();
        }

        // GET: Admin/Logout
        [HttpGet]
        public IActionResult Logout()
        {
            Response.Cookies.Delete(OfficeAdminCookieName);
            Response.Cookies.Delete(OfficeAdminUserNameCookieName);
            return Redirect("/");
        }

        // GET: AdminController
        public IActionResult Index()
        {
            return View();
        }

        // GET: Admin/StateCounties
        public IActionResult StateCounties()
        {
            return View();
        }

        // GET: Admin/AccountItemFilters
        public IActionResult AccountItemFilters()
        {
            return View();
        }

        // GET: Admin/AuditTrail
        public IActionResult AuditTrail()
        {
            return View();
        }

        // GET: Admin/Hauling
        public IActionResult Hauling()
        {
            return View();
        }

        // GET: Admin/Haulers
        public IActionResult Haulers()
        {
            return View();
        }

        // GET: Admin/FuelSurcharge
        public IActionResult FuelSurcharge()
        {
            return View();
        }

        // GET: Admin/SystemSettings
        public IActionResult SystemSettings()
        {
            return View();
        }

        // GET: Admin/WeightSheetHaulerRates
        public IActionResult WeightSheetHaulerRates()
        {
            return View();
        }

        // GET: Admin/Servers
        public IActionResult Servers()
        {
            return View();
        }

        // GET: Admin/LocationSequenceMappings
        public IActionResult LocationSequenceMappings()
        {
            return View();
        }

        // GET: Admin/SplitGroups
        public IActionResult SplitGroups()
        {
            return View();
        }

        // GET: Admin/As400Sync
        public IActionResult As400Sync()
        {
            return View();
        }

    }
}
