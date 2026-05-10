using GrainManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GrainManagement
{
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly dbContext _context;

        public UsersController(dbContext context, ILogger<UsersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: UsersController
        public IActionResult Index()
        {
            return View();
        }
    }
}
