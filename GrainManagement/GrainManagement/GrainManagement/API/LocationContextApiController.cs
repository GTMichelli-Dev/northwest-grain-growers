using GrainManagement.Models;
using GrainManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GrainManagement.API;

[ApiController]
[Route("api/[controller]")]
public class LocationContextApiController : ControllerBase
{
    private readonly dbContext _db;
    private readonly ModuleOptions _modules;

    public LocationContextApiController(dbContext db, IOptions<ModuleOptions> modules)
    {
        _db = db;
        _modules = modules.Value;
    }

    /// <summary>
    /// Returns active locations for the location selector dropdown.
    /// Filters by enabled modules: Warehouse-only shows UseForWarehouse locations,
    /// Seed-only shows UseForSeed, both shows either. Admin/Reporting modes show all.
    /// AllowedLocationIds further restricts the list when non-empty.
    /// </summary>
    [HttpGet("available")]
    public async Task<IActionResult> GetAvailable()
    {
        var query = _db.Locations
            .AsNoTracking()
            .Where(l => l.IsActive);

        // If any admin or reporting module is on, show all locations (no capability filter)
        bool isAdminOrReporting = _modules.DatabaseAdmin || _modules.WarehouseAdmin
            || _modules.SeedAdmin || _modules.Reporting;

        if (!isAdminOrReporting)
        {
            bool wh = _modules.WarehouseIntake;
            bool seed = _modules.Seed;

            if (wh && seed)
                query = query.Where(l => l.UseForWarehouse || l.UseForSeed);
            else if (wh)
                query = query.Where(l => l.UseForWarehouse);
            else if (seed)
                query = query.Where(l => l.UseForSeed);
        }

        // Apply AllowedLocationIds restriction, but if that yields zero results
        // fall back to showing all locations matching the module capability filter
        var baseQuery = query;

        if (_modules.AllowedLocationIds.Count > 0)
        {
            var restricted = query.Where(l => _modules.AllowedLocationIds.Contains(l.LocationId));
            if (await restricted.AnyAsync())
                query = restricted;
            // else: fall back to baseQuery (all locations for the enabled module)
        }

        var locations = await query
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

    /// <summary>
    /// Clears the user's selected location cookie.
    /// </summary>
    [HttpPost("clear")]
    public IActionResult Clear()
    {
        LocationContext.ClearLocation(HttpContext.Response);
        return Ok(new { HasLocation = false });
    }

    public class SelectLocationDto
    {
        public int LocationId { get; set; }
    }
}
