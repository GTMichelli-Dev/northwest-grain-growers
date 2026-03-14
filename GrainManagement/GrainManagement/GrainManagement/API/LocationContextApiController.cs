using GrainManagement.Models;
using GrainManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrainManagement.API;

[ApiController]
[Route("api/[controller]")]
public class LocationContextApiController : ControllerBase
{
    private readonly dbContext _db;

    public LocationContextApiController(dbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Returns all active locations for the location selector dropdown.
    /// </summary>
    [HttpGet("available")]
    public async Task<IActionResult> GetAvailable()
    {
        var locations = await _db.Locations
            .AsNoTracking()
            .Where(l => l.IsActive)
            .OrderBy(l => l.Name)
            .Select(l => new
            {
                l.LocationId,
                l.Code,
                l.Name,
                l.UseForSeed,
                l.UseForWarehouse
            })
            .ToListAsync();

        return Ok(locations);
    }

    /// <summary>
    /// Returns the currently selected location (or null if none selected).
    /// </summary>
    [HttpGet("current")]
    public IActionResult GetCurrent([FromServices] ILocationContext locationContext)
    {
        if (!locationContext.HasLocation)
            return Ok(new { HasLocation = false });

        return Ok(new
        {
            HasLocation = true,
            locationContext.LocationId,
            locationContext.LocationName,
            locationContext.LocationCode,
            locationContext.CanDoWarehouse,
            locationContext.CanDoSeed,
            locationContext.IsAdminOnly
        });
    }

    /// <summary>
    /// Sets the user's selected location.
    /// </summary>
    [HttpPost("select")]
    public async Task<IActionResult> Select([FromBody] SelectLocationDto dto)
    {
        var location = await _db.Locations
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.LocationId == dto.LocationId && l.IsActive);

        if (location == null)
            return NotFound(new { Error = "Location not found or inactive." });

        LocationContext.SetLocation(HttpContext.Response, location.LocationId);

        return Ok(new
        {
            HasLocation = true,
            location.LocationId,
            LocationName = location.Name,
            LocationCode = location.Code,
            CanDoWarehouse = location.UseForWarehouse,
            CanDoSeed = location.UseForSeed,
            IsAdminOnly = !location.UseForWarehouse && !location.UseForSeed
        });
    }

    public class SelectLocationDto
    {
        public int LocationId { get; set; }
    }
}
