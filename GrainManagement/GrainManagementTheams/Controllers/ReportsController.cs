using Microsoft.AspNetCore.Mvc;

namespace GrainManagement.Controllers;

public class ReportsController : Controller
{
    public IActionResult Index() => View();
}
