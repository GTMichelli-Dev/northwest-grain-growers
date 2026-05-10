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

    public IActionResult DailyWeightSheetSummary() => View();
    public IActionResult DailyWeightSheetSeries() => View();
    public IActionResult CommoditiesByDateRange() => View();
    public IActionResult BinProtein() => View();
    public IActionResult DailyLoadTimes() => View();
    public IActionResult PeakHours() => View();
    public IActionResult ProducerCommodity() => View();
    public IActionResult ProducerDelivery() => View();
    public IActionResult IntakeLocation() => View();
}
