using GrainManagement.API;
using GrainManagement.Models;
using GrainManagement.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GrainManagement.Controllers
{
    public class LocationsController : Controller
    {
        private readonly ILogger<LocationsController> _logger;
        private readonly dbContext _context;

        public LocationsController(dbContext context, ILogger<LocationsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Storage(int locationId)
        {
            var location = _context.Locations.Find(locationId);
            if (location == null) return NotFound();

            ViewBag.LocationId = locationId;
            ViewBag.LocationName = location.Name;
            return View();
        }
    }
}
