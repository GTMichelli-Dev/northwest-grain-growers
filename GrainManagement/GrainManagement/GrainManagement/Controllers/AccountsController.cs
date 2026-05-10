using Microsoft.AspNetCore.Mvc;

namespace GrainManagement
{
    public class AccountsController : Controller
    {
        public IActionResult Index() => View();
    }
}
