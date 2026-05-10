using GrainManagement.Auth;
using GrainManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GrainManagement.Controllers;

/// <summary>
/// Agvantage Seed Transfer push page — placeholder ("Coming Soon").
/// Gated by the same Agvantage PIN cookie (priv 15) as the warehouse
/// transfer page; reuses the warehouse Pin view via redirect so we
/// don't have two PIN entry pages drifting apart.
/// </summary>
[RequiresModule(nameof(ModuleOptions.DatabaseAdmin))]
[Route("AgvantageSeedTransfer")]
public sealed class AgvantageSeedTransferController : Controller
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);
        if (Request.Cookies.ContainsKey(AgvantageWarehouseTransferController.AgvantageCookieName))
            return;

        var returnUrl = Request.Path + Request.QueryString;
        context.Result = RedirectToAction(
            nameof(AgvantageWarehouseTransferController.Pin),
            "AgvantageWarehouseTransfer",
            new { returnUrl = returnUrl.ToString() });
    }

    [HttpGet("")]
    public IActionResult Index() => View();
}
