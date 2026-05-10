using GrainManagement.Auth;
using GrainManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GrainManagement.Controllers;

/// <summary>
/// Agvantage Warehouse Transfer push page. Gated by an Agvantage PIN
/// (PrivilegeId=15) cookie set via /api/Auth/ValidateAgvantagePin.
/// Missing cookie → redirect to /AgvantageWarehouseTransfer/Pin, which
/// itself bounces back to /Updates on a wrong PIN.
/// </summary>
[RequiresModule(nameof(ModuleOptions.DatabaseAdmin))]
[Route("AgvantageWarehouseTransfer")]
public sealed class AgvantageWarehouseTransferController : Controller
{
    public const string AgvantageCookieName         = "GrainMgmt_AgvantagePin";
    public const string AgvantageUserNameCookieName = "GrainMgmt_AgvantageUser";

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);

        var actionName = (context.ActionDescriptor.RouteValues.TryGetValue("action", out var a) ? a : "")
            ?? "";
        if (string.Equals(actionName, nameof(Pin), System.StringComparison.OrdinalIgnoreCase))
            return;

        if (Request.Cookies.ContainsKey(AgvantageCookieName))
            return;

        var returnUrl = Request.Path + Request.QueryString;
        context.Result = RedirectToAction(nameof(Pin), new { returnUrl = returnUrl.ToString() });
    }

    [HttpGet("")]
    public IActionResult Index() => View();

    [HttpGet("Pin")]
    public IActionResult Pin(string? returnUrl = null)
    {
        return Redirect("/Login?returnUrl="
            + System.Uri.EscapeDataString(string.IsNullOrWhiteSpace(returnUrl)
                ? "/AgvantageWarehouseTransfer"
                : returnUrl)
            + "&requirePriv=15");
    }
}
