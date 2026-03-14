using Microsoft.AspNetCore.Mvc;

namespace GrainManagement
{
    public class ItemsController : Controller
    {
        private readonly ICurrentUser _me;

        public ItemsController(ICurrentUser me)
        {
            _me = me;
        }

        public IActionResult Index()
        {
            if (!_me.IsManager && !_me.IsAdmin)
                return RedirectToAction("Index", "Home");

            return View();
        }
    }
}
