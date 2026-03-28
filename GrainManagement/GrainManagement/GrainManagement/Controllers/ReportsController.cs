using GrainManagement.Auth;
using GrainManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace GrainManagement.Controllers;

[RequiresModule(nameof(ModuleOptions.Reporting))]
public class ReportsController : Controller
{
    public IActionResult Index(string report = null)
    {
        ViewBag.ReportName = report;
        return View();
    }
}
