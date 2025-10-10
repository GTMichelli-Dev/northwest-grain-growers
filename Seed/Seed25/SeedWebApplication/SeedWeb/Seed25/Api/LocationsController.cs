using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Seed25.Models;

namespace Seed25.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {

        private readonly Seed_DataContext _db;
        public LocationsController(Seed_DataContext db) => _db = db;

        [HttpGet("list")]
        public async Task<IActionResult> List()
        {
            var items = await _db.Set<Location>()
                .OrderBy(l => l.Description)
                .Select(l => new { id = l.Id, description = l.Description })
                .ToListAsync();
            return Ok(items);
        }
    }
}
