using GrainManagement.Auth;
using GrainManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace GrainManagement.Controllers
{
    /// <summary>
    /// Remote-only "admin" landing page (the tiles view that used to live at /).
    /// Gated by a session cookie set by /api/Auth/ValidateRemoteAdminPin —
    /// caller must supply a PIN belonging to a user with PrivilegeId=7 (Remote Admin).
    /// </summary>
    [Route("RemoteAdmin")]
    [RequiresModule(nameof(ModuleOptions.System))]
    public class RemoteAdminController : Controller
    {
        public const string CookieName         = "GrainMgmt_RemoteAdminPin";
        public const string UserNameCookieName = "GrainMgmt_RemoteAdminUser";

        [HttpGet("")]
        [HttpGet("Index")]
        public IActionResult Index()
        {
            if (!Request.Cookies.ContainsKey(CookieName))
                return RedirectToAction(nameof(Pin));

            // Reuse the original tiles view that used to render at /.
            return View("~/Views/Home/Index.cshtml");
        }

        [HttpGet("Pin")]
        public IActionResult Pin()
        {
            return View();
        }

        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete(CookieName);
            Response.Cookies.Delete(UserNameCookieName);
            return Redirect("/");
        }
    }
}
