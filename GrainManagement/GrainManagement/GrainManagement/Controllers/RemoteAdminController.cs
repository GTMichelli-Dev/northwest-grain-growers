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
    public class RemoteAdminController : Controller
    {
        public const string CookieName         = "GrainMgmt_RemoteAdminPin";
        public const string UserNameCookieName = "GrainMgmt_RemoteAdminUser";

        // The Index / Pin pages only make sense on Remote deployments, so
        // they keep the module gate. Logout is intentionally ungated — the
        // navbar Logout link is shown whenever the PIN cookie exists, and
        // a Central deployment with a stale cookie still needs to be able
        // to clear it without 404-ing.

        [HttpGet("")]
        [HttpGet("Index")]
        [RequiresModule(nameof(ModuleOptions.System))]
        public IActionResult Index()
        {
            if (!Request.Cookies.ContainsKey(CookieName))
                return RedirectToAction(nameof(Pin));

            // Reuse the original tiles view that used to render at /.
            return View("~/Views/Home/Index.cshtml");
        }

        [HttpGet("Pin")]
        [RequiresModule(nameof(ModuleOptions.System))]
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
