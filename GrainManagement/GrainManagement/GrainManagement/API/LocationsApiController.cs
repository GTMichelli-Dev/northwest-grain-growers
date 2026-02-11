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
  

        return Ok(data);
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
}
