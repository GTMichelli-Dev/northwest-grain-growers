using GrainManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrainManagement.API;

[UseAdminConnection]
[ApiController]
[Route("api/LocationSequenceMappings")]
public sealed class LocationSequenceMappingApiController : ControllerBase
{
    private readonly dbContext _ctx;
    private readonly ILogger<LocationSequenceMappingApiController> _logger;

    public LocationSequenceMappingApiController(
        dbContext ctx,
        ILogger<LocationSequenceMappingApiController> logger)
    {
        _ctx = ctx;
        _logger = logger;
    }

    // GET /api/LocationSequenceMappings
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var data = await _ctx.LocationSequenceMappings
            .AsNoTracking()
            .OrderBy(m => m.Location.Name)
            .ThenBy(m => m.SequenceId)
            .Select(m => new
            {
                m.Id,
                m.ServerId,
                ServerName = m.Server.FriendlyName ?? m.Server.ServerName,
                m.LocationId,
                LocationName = m.Location.Name,
                m.SequenceId,
            })
            .ToListAsync(ct);

        return Ok(data);
    }

    // GET /api/LocationSequenceMappings/Servers
    [HttpGet("Servers")]
    public async Task<IActionResult> GetServers(CancellationToken ct)
    {
        var servers = await _ctx.Servers
            .AsNoTracking()
            .OrderBy(s => s.FriendlyName ?? s.ServerName)
            .Select(s => new
            {
                s.ServerId,
                Name = s.FriendlyName ?? s.ServerName,
            })
            .ToListAsync(ct);

        return Ok(servers);
    }

    // GET /api/LocationSequenceMappings/Locations
    [HttpGet("Locations")]
    public async Task<IActionResult> GetLocations(CancellationToken ct)
    {
        var locations = await _ctx.Locations
            .AsNoTracking()
            .Where(l => l.IsActive)
            .OrderBy(l => l.Name)
            .Select(l => new
            {
                l.LocationId,
                l.Name,
            })
            .ToListAsync(ct);

        return Ok(locations);
    }

    // POST /api/LocationSequenceMappings
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateLocationSequenceMappingDto dto,
        CancellationToken ct)
    {
        if (dto is null)
            return BadRequest(new { message = "Request body is required." });
        if (dto.ServerId <= 0)
            return BadRequest(new { message = "ServerId is required." });
        if (dto.LocationId <= 0)
            return BadRequest(new { message = "LocationId is required." });
        if (dto.SequenceId <= 0)
            return BadRequest(new { message = "SequenceId is required." });

        var exists = await _ctx.LocationSequenceMappings
            .AnyAsync(m => m.LocationId == dto.LocationId && m.SequenceId == dto.SequenceId, ct);
        if (exists)
            return Conflict(new { message = "This Location/Sequence combination already exists." });

        var locationOnServer = await _ctx.LocationSequenceMappings
            .AnyAsync(m => m.LocationId == dto.LocationId && m.ServerId == dto.ServerId, ct);
        if (locationOnServer)
            return Conflict(new { message = "This location is already assigned to that server." });

        var mapping = new LocationSequenceMapping
        {
            ServerId   = dto.ServerId,
            LocationId = dto.LocationId,
            SequenceId = dto.SequenceId,
        };

        _ctx.LocationSequenceMappings.Add(mapping);

        try
        {
            await _ctx.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex,
                "Failed to create LocationSequenceMapping for LocationId={LocationId}, SequenceId={SequenceId}",
                dto.LocationId, dto.SequenceId);
            return StatusCode(500, new { message = "Database error while creating mapping." });
        }

        return Ok(new { mapping.Id });
    }

    // PUT /api/LocationSequenceMappings/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] CreateLocationSequenceMappingDto dto,
        CancellationToken ct)
    {
        if (dto is null)
            return BadRequest(new { message = "Request body is required." });

        var mapping = await _ctx.LocationSequenceMappings.FindAsync(new object[] { id }, ct);
        if (mapping is null)
            return NotFound(new { message = "Mapping not found." });

        // Check uniqueness if LocationId or SequenceId changed
        if (mapping.LocationId != dto.LocationId || mapping.SequenceId != dto.SequenceId)
        {
            var exists = await _ctx.LocationSequenceMappings
                .AnyAsync(m => m.Id != id && m.LocationId == dto.LocationId && m.SequenceId == dto.SequenceId, ct);
            if (exists)
                return Conflict(new { message = "This Location/Sequence combination already exists." });
        }

        // Check location not already on this server
        if (mapping.LocationId != dto.LocationId || mapping.ServerId != dto.ServerId)
        {
            var locationOnServer = await _ctx.LocationSequenceMappings
                .AnyAsync(m => m.Id != id && m.LocationId == dto.LocationId && m.ServerId == dto.ServerId, ct);
            if (locationOnServer)
                return Conflict(new { message = "This location is already assigned to that server." });
        }

        mapping.ServerId   = dto.ServerId;
        mapping.LocationId = dto.LocationId;
        mapping.SequenceId = dto.SequenceId;

        try
        {
            await _ctx.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to update LocationSequenceMapping {Id}", id);
            return StatusCode(500, new { message = "Database error while updating mapping." });
        }

        return Ok(new { id });
    }

    // DELETE /api/LocationSequenceMappings/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var mapping = await _ctx.LocationSequenceMappings.FindAsync(new object[] { id }, ct);
        if (mapping is null)
            return NotFound(new { message = "Mapping not found." });

        _ctx.LocationSequenceMappings.Remove(mapping);

        try
        {
            await _ctx.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to delete LocationSequenceMapping {Id}", id);
            return StatusCode(500, new { message = "Database error while deleting mapping." });
        }

        return Ok(new { id });
    }
}

public record CreateLocationSequenceMappingDto(int ServerId, int LocationId, int SequenceId);
