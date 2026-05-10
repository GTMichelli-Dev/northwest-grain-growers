using GrainManagement.Auth;
using GrainManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GrainManagement.Controllers
{
    /// <summary>
    /// AS400 sync / Agvantage upload pages — gated by the same Office
    /// Admin PIN cookie that protects /Admin. A missing cookie redirects
    /// to /Admin/Pin?returnUrl=… so the operator returns here after
    /// authenticating.
    /// </summary>
    [RequiresModule(nameof(ModuleOptions.DatabaseAdmin))]
    public class UpdatesController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            if (Request.Cookies.ContainsKey(AdminController.OfficeAdminCookieName))
                return;

            var returnUrl = Request.Path + Request.QueryString;
            context.Result = RedirectToAction(
                nameof(AdminController.Pin),
                "Admin",
                new { returnUrl = returnUrl.ToString() });
        }

        public IActionResult Index() => View();
    }
}
