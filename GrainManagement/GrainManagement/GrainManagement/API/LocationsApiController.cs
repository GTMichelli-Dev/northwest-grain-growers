using DevExpress.XtraEditors.Filtering;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using GrainManagement.Dtos;
using GrainManagement.Models;
using GrainManagement.Services;
using GrainManagement.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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
                Licensed = l.Licensed,
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

        // Auto-add default weight methods (MANUAL + TRUCK_SCALE) for the new location
        try
        {
            var defaultCodes = new[] { "MANUAL", "TRUCK_SCALE" };
            var defaultMethodIds = await _ctx.QuantityMethods
                .AsNoTracking()
                .Where(m => m.IsActive && defaultCodes.Contains(m.Code))
                .Select(m => m.QuantityMethodId)
                .ToListAsync();

            foreach (var methodId in defaultMethodIds)
            {
                _ctx.LocationQuantityMethods.Add(new LocationQuantityMethod
                {
                    LocationId = entity.LocationId,
                    QuantityMethodId = methodId
                });
            }

            if (defaultMethodIds.Count > 0)
                await _ctx.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to auto-add default weight methods for LocationId={LocationId}. Table may not exist yet.", entity.LocationId);
        }

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

    // ── Container Locations (container.ContainerLocations) ───────────────────

    [HttpGet("{locationId}/ContainerLocations")]
    public async Task<IActionResult> GetContainerLocations(int locationId, CancellationToken ct)
    {
        var conn = (SqlConnection)_ctx.Database.GetDbConnection();
        if (conn.State != System.Data.ConnectionState.Open) await conn.OpenAsync(ct);

        var list = new List<object>();
        using var cmd = new SqlCommand(@"
            SELECT ContainerLocationId, LocationId, Description, IsActive, Idx
            FROM   [container].[ContainerLocations]
            WHERE  LocationId = @loc
            ORDER BY Idx, Description", conn);
        cmd.Parameters.AddWithValue("@loc", locationId);
        using var rdr = await cmd.ExecuteReaderAsync(ct);
        while (await rdr.ReadAsync(ct))
        {
            list.Add(new
            {
                ContainerLocationId = rdr.GetInt32(0),
                LocationId          = rdr.GetInt32(1),
                Description         = rdr.GetString(2),
                IsActive            = rdr.GetBoolean(3),
                Idx                 = rdr.GetInt32(4)
            });
        }
        return Ok(list);
    }

    [HttpPost("{locationId}/ContainerLocations")]
    public async Task<IActionResult> InsertContainerLocation(
        int locationId, [FromBody] ContainerLocationUpsertDto dto, CancellationToken ct)
    {
        var conn = (SqlConnection)_ctx.Database.GetDbConnection();
        if (conn.State != System.Data.ConnectionState.Open) await conn.OpenAsync(ct);

        // Check for duplicate name
        using var dupNameCmd = new SqlCommand(@"
            SELECT COUNT(*) FROM [container].[ContainerLocations]
            WHERE LocationId = @loc AND Description = @desc", conn);
        dupNameCmd.Parameters.AddWithValue("@loc",  locationId);
        dupNameCmd.Parameters.AddWithValue("@desc", dto.Description ?? "");
        if ((int)await dupNameCmd.ExecuteScalarAsync(ct) > 0)
            return BadRequest(new { message = $"A storage area named '{dto.Description}' already exists at this location." });

        // Check for duplicate sort index
        using var dupIdxCmd = new SqlCommand(@"
            SELECT COUNT(*) FROM [container].[ContainerLocations]
            WHERE LocationId = @loc AND Idx = @idx", conn);
        dupIdxCmd.Parameters.AddWithValue("@loc", locationId);
        dupIdxCmd.Parameters.AddWithValue("@idx", dto.Idx);
        if ((int)await dupIdxCmd.ExecuteScalarAsync(ct) > 0)
            return BadRequest(new { message = $"Sort index {dto.Idx} is already in use by another storage area at this location." });

        using var cmd = new SqlCommand(@"
            INSERT INTO [container].[ContainerLocations]
                (LocationId, Description, IsActive, Idx, CreatedAt)
            OUTPUT INSERTED.ContainerLocationId
            VALUES (@loc, @desc, @active, @idx, SYSUTCDATETIME())", conn);
        cmd.Parameters.AddWithValue("@loc",    locationId);
        cmd.Parameters.AddWithValue("@desc",   dto.Description ?? "");
        cmd.Parameters.AddWithValue("@active", dto.IsActive);
        cmd.Parameters.AddWithValue("@idx",    dto.Idx);
        var newId = (int)await cmd.ExecuteScalarAsync(ct);
        return Ok(new { ContainerLocationId = newId });
    }

    [HttpPut("{locationId}/ContainerLocations/{id}")]
    public async Task<IActionResult> UpdateContainerLocation(
        int locationId, int id, [FromBody] ContainerLocationUpsertDto dto, CancellationToken ct)
    {
        var conn = (SqlConnection)_ctx.Database.GetDbConnection();
        if (conn.State != System.Data.ConnectionState.Open) await conn.OpenAsync(ct);

        // Check for duplicate name (exclude self)
        using var dupNameCmd = new SqlCommand(@"
            SELECT COUNT(*) FROM [container].[ContainerLocations]
            WHERE LocationId = @loc AND Description = @desc AND ContainerLocationId <> @id", conn);
        dupNameCmd.Parameters.AddWithValue("@loc",  locationId);
        dupNameCmd.Parameters.AddWithValue("@desc", dto.Description ?? "");
        dupNameCmd.Parameters.AddWithValue("@id",   id);
        if ((int)await dupNameCmd.ExecuteScalarAsync(ct) > 0)
            return BadRequest(new { message = $"A storage area named '{dto.Description}' already exists at this location." });

        // Check for duplicate sort index (exclude self)
        using var dupIdxCmd = new SqlCommand(@"
            SELECT COUNT(*) FROM [container].[ContainerLocations]
            WHERE LocationId = @loc AND Idx = @idx AND ContainerLocationId <> @id", conn);
        dupIdxCmd.Parameters.AddWithValue("@loc", locationId);
        dupIdxCmd.Parameters.AddWithValue("@idx", dto.Idx);
        dupIdxCmd.Parameters.AddWithValue("@id",  id);
        if ((int)await dupIdxCmd.ExecuteScalarAsync(ct) > 0)
            return BadRequest(new { message = $"Sort index {dto.Idx} is already in use by another storage area at this location." });

        using var cmd = new SqlCommand(@"
            UPDATE [container].[ContainerLocations]
            SET    Description = @desc, IsActive = @active, Idx = @idx, UpdatedAt = SYSUTCDATETIME()
            WHERE  ContainerLocationId = @id AND LocationId = @loc", conn);
        cmd.Parameters.AddWithValue("@desc",   dto.Description ?? "");
        cmd.Parameters.AddWithValue("@active", dto.IsActive);
        cmd.Parameters.AddWithValue("@idx",    dto.Idx);
        cmd.Parameters.AddWithValue("@id",     id);
        cmd.Parameters.AddWithValue("@loc",    locationId);
        var rows = await cmd.ExecuteNonQueryAsync(ct);
        return rows == 0 ? NotFound() : Ok();
    }

    [HttpDelete("{locationId}/ContainerLocations/{id}")]
    public async Task<IActionResult> DeleteContainerLocation(
        int locationId, int id, CancellationToken ct)
    {
        var conn = (SqlConnection)_ctx.Database.GetDbConnection();
        if (conn.State != System.Data.ConnectionState.Open) await conn.OpenAsync(ct);

        // Check if any containers reference this storage area
        using var checkCmd = new SqlCommand(@"
            SELECT COUNT(*) FROM [container].[Containers]
            WHERE ContainerLocationId = @id", conn);
        checkCmd.Parameters.AddWithValue("@id", id);
        var count = (int)await checkCmd.ExecuteScalarAsync(ct);
        if (count > 0)
            return BadRequest(new { message = $"Cannot delete this storage area because {count} container(s) are associated with it. Remove or reassign the containers first." });

        using var cmd = new SqlCommand(@"
            DELETE FROM [container].[ContainerLocations]
            WHERE ContainerLocationId = @id AND LocationId = @loc", conn);
        cmd.Parameters.AddWithValue("@id",  id);
        cmd.Parameters.AddWithValue("@loc", locationId);
        var rows = await cmd.ExecuteNonQueryAsync(ct);
        return rows == 0 ? NotFound() : Ok();
    }

    // ── Container Types ───────────────────────────────────────────────────────

    [HttpGet("{locationId}/ContainerTypes")]
    public async Task<IActionResult> GetContainerTypes(int locationId, CancellationToken ct)
    {
        var conn = (SqlConnection)_ctx.Database.GetDbConnection();
        if (conn.State != System.Data.ConnectionState.Open) await conn.OpenAsync(ct);

        var list = new List<object>();
        using var cmd = new SqlCommand(@"
            SELECT ContainerTypeId, Description, DefaultCapacityLb
            FROM   [container].[ContainerTypes]
            WHERE  IsActive = 1
            ORDER BY Description", conn);
        using var rdr = await cmd.ExecuteReaderAsync(ct);
        while (await rdr.ReadAsync(ct))
        {
            list.Add(new
            {
                ContainerTypeId   = rdr.GetInt32(0),
                Description       = rdr.GetString(1),
                DefaultCapacityLb = rdr.IsDBNull(2) ? (decimal?)null : rdr.GetDecimal(2)
            });
        }
        return Ok(list);
    }

    // ── Containers (container.Containers) ─────────────────────────────────────

    [HttpGet("{locationId}/Containers")]
    public async Task<IActionResult> GetContainers(int locationId, CancellationToken ct)
    {
        var conn = (SqlConnection)_ctx.Database.GetDbConnection();
        if (conn.State != System.Data.ConnectionState.Open) await conn.OpenAsync(ct);

        var list = new List<object>();
        using var cmd = new SqlCommand(@"
            SELECT c.ContainerId, c.ContainerLocationId, cl.Description AS ContainerLocationDescription,
                   c.ContainerTypeId, ct.Description AS ContainerTypeDescription,
                   c.Description, c.CapacityLb, c.IsActive, c.Notes
            FROM   [container].[Containers] c
            LEFT JOIN [container].[ContainerLocations] cl ON cl.ContainerLocationId = c.ContainerLocationId
            LEFT JOIN [container].[ContainerTypes]     ct ON ct.ContainerTypeId     = c.ContainerTypeId
            WHERE  c.LocationId = @loc
               AND (c.Destroyed IS NULL OR c.Destroyed = 0)
            ORDER BY c.Description", conn);
        cmd.Parameters.AddWithValue("@loc", locationId);
        using var rdr = await cmd.ExecuteReaderAsync(ct);
        while (await rdr.ReadAsync(ct))
        {
            list.Add(new
            {
                ContainerId                   = rdr.GetInt64(0),
                ContainerLocationId           = rdr.IsDBNull(1) ? (int?)null : rdr.GetInt32(1),
                ContainerLocationDescription  = rdr.IsDBNull(2) ? null : rdr.GetString(2),
                ContainerTypeId               = rdr.IsDBNull(3) ? (int?)null : rdr.GetInt32(3),
                ContainerTypeDescription      = rdr.IsDBNull(4) ? null : rdr.GetString(4),
                Description                   = rdr.IsDBNull(5) ? null : rdr.GetString(5),
                CapacityLb                    = rdr.IsDBNull(6) ? (decimal?)null : rdr.GetDecimal(6),
                IsActive                      = rdr.GetBoolean(7),
                Notes                         = rdr.IsDBNull(8) ? null : rdr.GetString(8)
            });
        }
        return Ok(list);
    }

    [HttpPost("{locationId}/Containers")]
    public async Task<IActionResult> InsertContainer(
        int locationId, [FromBody] ContainerUpsertDto dto, CancellationToken ct)
    {
        var conn = (SqlConnection)_ctx.Database.GetDbConnection();
        if (conn.State != System.Data.ConnectionState.Open) await conn.OpenAsync(ct);

        // Check for duplicate container name at this location
        using var dupCmd = new SqlCommand(@"
            SELECT COUNT(*) FROM [container].[Containers]
            WHERE LocationId = @loc AND Description = @desc AND (Destroyed IS NULL OR Destroyed = 0)", conn);
        dupCmd.Parameters.AddWithValue("@loc",  locationId);
        dupCmd.Parameters.AddWithValue("@desc", dto.Description ?? "");
        if ((int)await dupCmd.ExecuteScalarAsync(ct) > 0)
            return BadRequest(new { message = $"A container named '{dto.Description}' already exists at this location." });

        // INSTEAD OF trigger on Containers generates the ContainerId — OUTPUT clause not allowed with INSTEAD OF triggers.
        // Insert first, then query back the generated ID.
        using var cmd = new SqlCommand(@"
            INSERT INTO [container].[Containers]
                (LocationId, ContainerLocationId, ContainerTypeId, Description, CapacityLb, IsActive, Notes, Destroyed, CreatedAt)
            VALUES (@loc, @clid, @ctid, @desc, @cap, @active, @notes, 0, SYSUTCDATETIME());
            SELECT TOP 1 ContainerId FROM [container].[Containers]
            WHERE LocationId = @loc AND Description = @desc AND (Destroyed IS NULL OR Destroyed = 0)
            ORDER BY ContainerId DESC;", conn);
        cmd.Parameters.AddWithValue("@loc",    locationId);
        cmd.Parameters.AddWithValue("@clid",   (object)dto.ContainerLocationId ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@ctid",   (object)dto.ContainerTypeId     ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@desc",   dto.Description ?? "");
        cmd.Parameters.AddWithValue("@cap",    (object)dto.CapacityLb          ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@active", dto.IsActive);
        cmd.Parameters.AddWithValue("@notes",  (object)dto.Notes               ?? DBNull.Value);
        var newId = (long)await cmd.ExecuteScalarAsync(ct);
        return Ok(new { ContainerId = newId });
    }

    [HttpPut("{locationId}/Containers/{id}")]
    public async Task<IActionResult> UpdateContainer(
        int locationId, long id, [FromBody] ContainerUpsertDto dto, CancellationToken ct)
    {
        var conn = (SqlConnection)_ctx.Database.GetDbConnection();
        if (conn.State != System.Data.ConnectionState.Open) await conn.OpenAsync(ct);

        // Check for duplicate container name at this location (exclude self)
        using var dupCmd = new SqlCommand(@"
            SELECT COUNT(*) FROM [container].[Containers]
            WHERE LocationId = @loc AND Description = @desc AND ContainerId <> @id AND (Destroyed IS NULL OR Destroyed = 0)", conn);
        dupCmd.Parameters.AddWithValue("@loc",  locationId);
        dupCmd.Parameters.AddWithValue("@desc", dto.Description ?? "");
        dupCmd.Parameters.AddWithValue("@id",   id);
        if ((int)await dupCmd.ExecuteScalarAsync(ct) > 0)
            return BadRequest(new { message = $"A container named '{dto.Description}' already exists at this location." });

        using var cmd = new SqlCommand(@"
            UPDATE [container].[Containers]
            SET ContainerLocationId = @clid, ContainerTypeId = @ctid,
                Description = @desc, CapacityLb = @cap, IsActive = @active,
                Notes = @notes, UpdatedAt = SYSUTCDATETIME()
            WHERE ContainerId = @id AND LocationId = @loc", conn);
        cmd.Parameters.AddWithValue("@clid",   (object)dto.ContainerLocationId ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@ctid",   (object)dto.ContainerTypeId     ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@desc",   dto.Description ?? "");
        cmd.Parameters.AddWithValue("@cap",    (object)dto.CapacityLb          ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@active", dto.IsActive);
        cmd.Parameters.AddWithValue("@notes",  (object)dto.Notes               ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@id",     id);
        cmd.Parameters.AddWithValue("@loc",    locationId);
        var rows = await cmd.ExecuteNonQueryAsync(ct);
        return rows == 0 ? NotFound() : Ok();
    }

    [HttpDelete("{locationId}/Containers/{id}")]
    public async Task<IActionResult> DeleteContainer(
        int locationId, long id, CancellationToken ct)
    {
        var conn = (SqlConnection)_ctx.Database.GetDbConnection();
        if (conn.State != System.Data.ConnectionState.Open) await conn.OpenAsync(ct);

        using var cmd = new SqlCommand(@"
            UPDATE [container].[Containers]
            SET Destroyed = 1, UpdatedAt = SYSUTCDATETIME()
            WHERE ContainerId = @id AND LocationId = @loc", conn);
        cmd.Parameters.AddWithValue("@id",  id);
        cmd.Parameters.AddWithValue("@loc", locationId);
        var rows = await cmd.ExecuteNonQueryAsync(ct);
        return rows == 0 ? NotFound() : Ok();
    }

    // ══════════════════════════════════════════════════════════════════════
    // LOCATION QUANTITY METHODS
    // ══════════════════════════════════════════════════════════════════════

    // GET /api/locations/{locationId}/QuantityMethods
    [HttpGet("{locationId}/QuantityMethods")]
    public async Task<IActionResult> GetQuantityMethods(int locationId, CancellationToken ct)
    {
        try
        {
            var data = await _ctx.LocationQuantityMethods
                .AsNoTracking()
                .Where(lqm => lqm.LocationId == locationId)
                .Select(lqm => new
                {
                    lqm.QuantityMethodId,
                    lqm.QuantityMethod.Code,
                    lqm.QuantityMethod.Description,
                    lqm.QuantityMethod.IsActive
                })
                .OrderBy(x => x.Code)
                .ToListAsync(ct);

            return Ok(data);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "LocationQuantityMethods query failed for LocationId={LocationId}. Table may not exist yet.", locationId);
            return Ok(Array.Empty<object>());
        }
    }

    // GET /api/locations/AllQuantityMethods
    [HttpGet("AllQuantityMethods")]
    public async Task<IActionResult> GetAllQuantityMethods(CancellationToken ct)
    {
        var data = await _ctx.QuantityMethods
            .AsNoTracking()
            .Where(m => m.IsActive)
            .OrderBy(m => m.Code)
            .Select(m => new
            {
                m.QuantityMethodId,
                m.Code,
                m.Description
            })
            .ToListAsync(ct);

        return Ok(data);
    }

    // POST /api/locations/{locationId}/QuantityMethods
    [HttpPost("{locationId}/QuantityMethods")]
    public async Task<IActionResult> AddQuantityMethod(
        int locationId,
        [FromBody] AddLocationQuantityMethodDto dto,
        CancellationToken ct)
    {
        if (dto is null || dto.QuantityMethodId <= 0)
            return BadRequest(new { message = "QuantityMethodId is required." });

        var exists = await _ctx.LocationQuantityMethods
            .AnyAsync(lqm => lqm.LocationId == locationId && lqm.QuantityMethodId == dto.QuantityMethodId, ct);

        if (exists)
            return Conflict(new { message = "This quantity method is already configured for this location." });

        _ctx.LocationQuantityMethods.Add(new LocationQuantityMethod
        {
            LocationId = locationId,
            QuantityMethodId = dto.QuantityMethodId
        });

        try
        {
            await _ctx.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to add QuantityMethod {MethodId} to Location {LocationId}", dto.QuantityMethodId, locationId);
            return StatusCode(500, new { message = "Database error." });
        }

        return Ok();
    }

    // DELETE /api/locations/{locationId}/QuantityMethods/{methodId}
    [HttpDelete("{locationId}/QuantityMethods/{methodId}")]
    public async Task<IActionResult> RemoveQuantityMethod(
        int locationId, int methodId, CancellationToken ct)
    {
        // Prevent deletion of MANUAL — it is always required
        var methodCode = await _ctx.QuantityMethods
            .AsNoTracking()
            .Where(m => m.QuantityMethodId == methodId)
            .Select(m => m.Code)
            .FirstOrDefaultAsync(ct);

        if (methodCode == "MANUAL")
            return BadRequest(new { message = "Manual is always required and cannot be removed." });

        var entity = await _ctx.LocationQuantityMethods
            .FirstOrDefaultAsync(lqm => lqm.LocationId == locationId && lqm.QuantityMethodId == methodId, ct);

        if (entity is null)
            return NotFound(new { message = "Quantity method not found for this location." });

        _ctx.LocationQuantityMethods.Remove(entity);

        try
        {
            await _ctx.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to remove QuantityMethod {MethodId} from Location {LocationId}", methodId, locationId);
            return StatusCode(500, new { message = "Database error." });
        }

        return Ok();
    }
}

/// <summary>DTO for adding a quantity method to a location.</summary>
public class AddLocationQuantityMethodDto
{
    public int QuantityMethodId { get; set; }
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

public class ContainerLocationUpsertDto
{
    public string Description { get; set; } = "";
    public bool IsActive { get; set; } = true;
    public int Idx { get; set; }
}

public class ContainerUpsertDto
{
    public string Description { get; set; } = "";
    public int? ContainerLocationId { get; set; }
    public int? ContainerTypeId { get; set; }
    public decimal? CapacityLb { get; set; }
    public bool IsActive { get; set; } = true;
    public string Notes { get; set; } = "";
}
