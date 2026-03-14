using DevExpress.XtraEditors.Filtering;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using GrainManagement.Dtos;
using GrainManagement.Models;
using GrainManagement.Services;
using GrainManagement.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

[UseAdminConnection]
[ApiController]
[Route("api/locations")]
public class LocationsApiController : ControllerBase
{
    private readonly dbContext _ctx;
    private readonly ILogger<LocationsApiController> _logger;   
    private readonly ILocationService _locationService;

    public LocationsApiController(dbContext ctx, ILogger<LocationsApiController> logger, ILocationService locationService)
    {
        _ctx = ctx;
        _logger = logger;
        _locationService = locationService;
    }

    [HttpGet("Districts")]
    public IActionResult Districts()
    {

       var data=_locationService.GetLocationDistricts().Result.Select(d => new
        {
            DistrictId = d.DistrictId,
            Name = d.Name
        }).ToList();
        return Ok(data);
    }

    //[HttpGet("WarehouseLocations")]
    //public object WarehouseLocations(DataSourceLoadOptions loadOptions)
    //{

    //    DevExtremeUtils.NormalizeLoadOptions(loadOptions);
    //    var query =
    //        from l in _ctx.Locations.Include(l => l.District)
    //        .Where(l=> l.UseForWarehouse== true)
    //        .Where(l=> l.IsActive== true)
    //        select new LocationDto
    //        {
    //            LocationId = l.LocationId,
    //            Code = l.Code,
    //            Name = l.Name,
    //            UseForSeed = l.UseForSeed,
    //            UseForWarehouse = l.UseForWarehouse,
    //            IsActive = l.IsActive,
    //            DistrictId = l.DistrictId,
    //            District = l.District.Name
    //        };

    //    return DataSourceLoader.Load(query, loadOptions);
    //}

    [HttpGet("WarehouseLocationsList")]
    public async Task<IActionResult> WarehouseLocationsList()
    {
        var data = await _locationService.GetAllAsync();
        // sqlLocationService already filters UseForWarehouse + IsActive at the DB level
        var sorted = data.OrderBy(l => l.Name).ToList();

        return Ok(sorted);
    }

    [HttpGet("SeedLocationsList")]
    public async Task<IActionResult> SeedLocationsList()
    {
        var data = await _ctx.Locations
            .AsNoTracking()
            .Include(l => l.District)
            .Where(l => l.UseForSeed == true)
            .Where(l => l.IsActive == true)
            .OrderBy(l => l.Name)
            .Select(l => new
            {
                LocationId = l.LocationId,
                Name = l.Name,
                District = l.District.Name
            })
            .ToListAsync();

        return Ok(data);
    }

    //[HttpGet("SeedLocations")]
    //public object SeedLocations(DataSourceLoadOptions loadOptions)
    //{

    //    DevExtremeUtils.NormalizeLoadOptions(loadOptions);
    //    var query =
    //        from l in _ctx.Locations.Include(l => l.District)
    //        .Where(l => l.UseForSeed == true)
    //        .Where(l => l.IsActive == true)
    //        select new LocationDto
    //        {
    //            LocationId = l.LocationId,
    //            Code = l.Code,
    //            Name = l.Name,
    //            UseForSeed = l.UseForSeed,
    //            UseForWarehouse = l.UseForWarehouse,
    //            IsActive = l.IsActive,
    //            DistrictId = l.DistrictId,
    //            District = l.District.Name
    //        };

    //    return DataSourceLoader.Load(query, loadOptions);
    //}

    [HttpGet]
    public object Get(DataSourceLoadOptions loadOptions)
    {
        DevExtremeUtils.NormalizeLoadOptions(loadOptions);
        var query =
            from l in _ctx.Locations.Include(l => l.District)
            select new LocationDto
            {
                LocationId = l.LocationId,
                Code = l.Code,
                Name = l.Name,
                UseForSeed = l.UseForSeed,
                UseForWarehouse = l.UseForWarehouse,
                IsActive = l.IsActive,
                DistrictId = l.DistrictId,
                District = l.District.Name
            };

        return DataSourceLoader.Load(query, loadOptions);
    }
    [HttpPut]
    public async Task<IActionResult> Put([FromForm] int key, [FromForm] string values)
    {
        var entity = await _ctx.Locations.FindAsync(key);
        if (entity == null)
            return NotFound();

        // Snapshot current LocationId (key)
        var originalId = entity.LocationId;

        JsonConvert.PopulateObject(values, entity);

        // Enforce immutable LocationId
        if (entity.LocationId != originalId)
            return BadRequest(new { message = "LocationId cannot be changed." });

        entity.Code = Helpers.Norm(entity.Code);
        entity.Name = Helpers.Norm(entity.Name);

        var dupCode = await _ctx.Locations.AnyAsync(x => x.LocationId != key && x.Code == entity.Code);
        if (dupCode)
            return Conflict(new { message = $"Code '{entity.Code}' already exists." });

        var dupName = await _ctx.Locations.AnyAsync(x => x.LocationId != key && x.Name == entity.Name);
        if (dupName)
            return Conflict(new { message = $"Name '{entity.Name}' already exists." });

        await _ctx.SaveChangesAsync();
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromForm] string values)
    {
        var entity = new Location();
        JsonConvert.PopulateObject(values, entity);

        entity.Code = Helpers.Norm(entity.Code);
        entity.Name = Helpers.Norm(entity.Name);

        // LocationId must be provided and unique
        if (entity.LocationId <= 0)
            return BadRequest(new { message = "LocationId is required." });

        var dupId = await _ctx.Locations.AnyAsync(x => x.LocationId == entity.LocationId);
        if (dupId)
            return Conflict(new { message = $"LocationId '{entity.LocationId}' already exists." });

        var dupCode = await _ctx.Locations.AnyAsync(x => x.Code == entity.Code);
        if (dupCode)
            return Conflict(new { message = $"Code '{entity.Code}' already exists." });

        var dupName = await _ctx.Locations.AnyAsync(x => x.Name == entity.Name);
        if (dupName)
            return Conflict(new { message = $"Name '{entity.Name}' already exists." });

        _ctx.Locations.Add(entity);
        await _ctx.SaveChangesAsync();

        return Ok(new { entity.LocationId });
    }

    // ── LocationCounties CRUD ────────────────────────────────────────────

    /// <summary>GET /api/locations/{locationId}/Counties</summary>
    [HttpGet("{locationId}/Counties")]
    public async Task<IActionResult> GetLocationCounties(int locationId, CancellationToken ct)
    {
        var rows = await _ctx.LocationCounties
            .AsNoTracking()
            .Where(lc => lc.LocationId == locationId)
            .Include(lc => lc.County)
                .ThenInclude(c => c.State)
            .OrderBy(lc => lc.County.State.State1)
                .ThenBy(lc => lc.County.County1)
            .Select(lc => new
            {
                lc.Id,
                lc.LocationId,
                lc.CountyId,
                StateAbv   = lc.County.State.StateAbv,
                StateName  = lc.County.State.State1,
                CountyName = lc.County.County1,
            })
            .ToListAsync(ct);

        return Ok(rows);
    }

    /// <summary>POST /api/locations/{locationId}/Counties</summary>
    [HttpPost("{locationId}/Counties")]
    public async Task<IActionResult> AddLocationCounty(
        int locationId, [FromBody] AddLocationCountyDto dto, CancellationToken ct)
    {
        if (dto == null || dto.CountyId <= 0)
            return BadRequest(new { message = "CountyId is required." });

        var locationExists = await _ctx.Locations.AnyAsync(l => l.LocationId == locationId, ct);
        if (!locationExists)
            return NotFound(new { message = "Location not found." });

        var countyExists = await _ctx.Counties.AnyAsync(c => c.Id == dto.CountyId, ct);
        if (!countyExists)
            return NotFound(new { message = "County not found." });

        var duplicate = await _ctx.LocationCounties
            .AnyAsync(lc => lc.LocationId == locationId && lc.CountyId == dto.CountyId, ct);
        if (duplicate)
            return Conflict(new { message = "This county is already assigned to the location." });

        var entity = new LocationCounty
        {
            LocationId = locationId,
            CountyId   = dto.CountyId
        };

        _ctx.LocationCounties.Add(entity);
        await _ctx.SaveChangesAsync(ct);

        return Ok(new { entity.Id });
    }

    /// <summary>DELETE /api/locations/{locationId}/Counties/{id}</summary>
    [HttpDelete("{locationId}/Counties/{id}")]
    public async Task<IActionResult> DeleteLocationCounty(
        int locationId, int id, CancellationToken ct)
    {
        var entity = await _ctx.LocationCounties
            .FirstOrDefaultAsync(lc => lc.Id == id && lc.LocationId == locationId, ct);

        if (entity == null)
            return NotFound(new { message = "Location-county mapping not found." });

        _ctx.LocationCounties.Remove(entity);
        await _ctx.SaveChangesAsync(ct);

        return Ok();
    }

    // ── LocationItemFilters CRUD ───────────────────────────────────────────

    /// <summary>GET /api/locations/AllItemFilters — all filters grouped by LocationId</summary>
    [HttpGet("AllItemFilters")]
    public async Task<IActionResult> GetAllItemFilters(CancellationToken ct)
    {
        var rows = await _ctx.LocationItemFilters
            .AsNoTracking()
            .Include(li => li.Item)
            .OrderBy(li => li.Item.Description)
            .Select(li => new
            {
                li.LocationId,
                ItemName = li.Item.Description,
            })
            .ToListAsync(ct);

        // Group by LocationId → { LocationId: [itemName, ...] }
        var grouped = rows
            .GroupBy(r => r.LocationId)
            .ToDictionary(g => g.Key, g => g.Select(r => r.ItemName).ToList());

        return Ok(grouped);
    }

    /// <summary>GET /api/locations/{locationId}/Items</summary>
    [HttpGet("{locationId}/Items")]
    public async Task<IActionResult> GetLocationItems(int locationId, CancellationToken ct)
    {
        var rows = await _ctx.LocationItemFilters
            .AsNoTracking()
            .Where(li => li.LocationId == locationId)
            .Include(li => li.Item)
            .OrderBy(li => li.Item.Description)
            .Select(li => new
            {
                li.Id,
                li.LocationId,
                li.ItemId,
                ItemName = li.Item.Description,
            })
            .ToListAsync(ct);

        return Ok(rows);
    }

    /// <summary>POST /api/locations/{locationId}/Items</summary>
    [HttpPost("{locationId}/Items")]
    public async Task<IActionResult> AddLocationItem(
        int locationId, [FromBody] AddLocationItemDto dto, CancellationToken ct)
    {
        if (dto == null || dto.ItemId <= 0)
            return BadRequest(new { message = "ItemId is required." });

        var locationExists = await _ctx.Locations.AnyAsync(l => l.LocationId == locationId, ct);
        if (!locationExists)
            return NotFound(new { message = "Location not found." });

        var itemExists = await _ctx.Items.AnyAsync(i => i.ItemId == dto.ItemId, ct);
        if (!itemExists)
            return NotFound(new { message = "Item not found." });

        var duplicate = await _ctx.LocationItemFilters
            .AnyAsync(li => li.LocationId == locationId && li.ItemId == dto.ItemId, ct);
        if (duplicate)
            return Conflict(new { message = "This item is already assigned to the location." });

        var entity = new LocationItemFilter
        {
            LocationId = locationId,
            ItemId     = dto.ItemId
        };

        _ctx.LocationItemFilters.Add(entity);
        await _ctx.SaveChangesAsync(ct);

        return Ok(new { entity.Id });
    }

    /// <summary>DELETE /api/locations/{locationId}/Items/{id}</summary>
    [HttpDelete("{locationId}/Items/{id}")]
    public async Task<IActionResult> DeleteLocationItem(
        int locationId, Guid id, CancellationToken ct)
    {
        var entity = await _ctx.LocationItemFilters
            .FirstOrDefaultAsync(li => li.Id == id && li.LocationId == locationId, ct);

        if (entity == null)
            return NotFound(new { message = "Location-item mapping not found." });

        _ctx.LocationItemFilters.Remove(entity);
        await _ctx.SaveChangesAsync(ct);

        return Ok();
    }
}

/// <summary>DTO for adding a county to a location.</summary>
public class AddLocationCountyDto
{
    public int CountyId { get; set; }
}

/// <summary>DTO for adding an item to a location.</summary>
public class AddLocationItemDto
{
    public long ItemId { get; set; }
}
