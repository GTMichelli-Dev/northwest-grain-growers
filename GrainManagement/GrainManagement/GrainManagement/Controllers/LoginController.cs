using Microsoft.AspNetCore.Mvc;

namespace GrainManagement.Controllers
{
    /// <summary>
    /// Single user-login UI. Every gated page redirects here when a PIN
    /// is required, with the originally-requested URL preserved via
    /// <c>?returnUrl=…</c> and the required privilege via
    /// <c>?requirePriv=…</c>. The view posts to /api/Auth/Login which
    /// drops the unified GrainMgmt_UserId / GrainMgmt_UserName cookies
    /// plus any legacy role cookies the user qualifies for.
    /// </summary>
    [Route("Login")]
    public sealed class LoginController : Controller
    {
        [HttpGet("")]
        public IActionResult Index(string? returnUrl = null, int? requirePriv = null, int? denied = null)
        {
            ViewBag.ReturnUrl   = string.IsNullOrWhiteSpace(returnUrl) ? "/" : returnUrl;
            ViewBag.RequirePriv = requirePriv ?? 0;
            ViewBag.Denied      = (denied ?? 0) == 1;
            return View();
        }

        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("GrainMgmt_UserId");
            Response.Cookies.Delete("GrainMgmt_UserName");
            Response.Cookies.Delete(AdminController.OfficeAdminCookieName);
            Response.Cookies.Delete(AdminController.OfficeAdminUserNameCookieName);
            Response.Cookies.Delete(RemoteAdminController.CookieName);
            Response.Cookies.Delete(RemoteAdminController.UserNameCookieName);
            Response.Cookies.Delete(AgvantageWarehouseTransferController.AgvantageCookieName);
            Response.Cookies.Delete(AgvantageWarehouseTransferController.AgvantageUserNameCookieName);
            return Redirect("/");
        }
    }
}
