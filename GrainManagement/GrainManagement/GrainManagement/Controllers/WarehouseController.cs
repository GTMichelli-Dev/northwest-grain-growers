using Microsoft.AspNetCore.Mvc;

namespace GrainManagement.Controllers;

public class WarehouseController : Controller
{
    public IActionResult Index() => View();

    // TODO: wire these to the real receiving/shipping screens
    public IActionResult ReceiveGrower() => View();
    public IActionResult ReceiveTransfer() => View();
    public IActionResult ShipLoad() => View();
}
