#nullable enable
using GrainManagement.Auth;
using GrainManagement.Dtos.Scales;
using GrainManagement.Models;
using GrainManagement.Services;
using GrainManagement.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
[RequiresModule(nameof(ModuleOptions.Scales))]
public class ScaleController : ControllerBase
{
    private readonly IScaleRegistry _scaleRegistry;
    private readonly IHubContext<ScaleHub, IScaleClient> _hub;
    private readonly dbContext _ctx;

    public ScaleController(IScaleRegistry scaleRegistry, IHubContext<ScaleHub, IScaleClient> hub, dbContext ctx)
    {
        _scaleRegistry = scaleRegistry;
        _hub = hub;
        _ctx = ctx;
    }

    [HttpPost("UpdateScale")]
    public async Task<IActionResult> UpdateScale([FromBody] ScaleDto scale)
    {
        if (scale == null) return BadRequest(new { error = "Invalid scale data." });
        if (scale.Id < 1) return BadRequest(new { error = "Id must be >= 1." });
        if (string.IsNullOrWhiteSpace(scale.Description)) return BadRequest(new { error = "Description is required." });

        _scaleRegistry.Upsert(scale);

        // Kiosks watching a specific scale
        await _hub.Clients.Group(ScaleHub.ScaleGroupName(scale.Id))
            .ScaleUpdated(scale);

        // Dashboards (all scales page)
        await _hub.Clients.All.ScaleUpdated(scale);

        return Ok(new { message = "Scale updated successfully." });
    }

    [HttpGet("CachedScales")]
    public IActionResult GetCachedScales()
    {
        var scales = _scaleRegistry.GetSnapshotWithHealth(TimeSpan.FromSeconds(5));
        return Ok(scales);
    }

    /// <summary>Returns active locations for scale config dropdown.</summary>
    [HttpGet("Locations")]
    public async Task<IActionResult> GetLocations()
    {
        var locations = await _ctx.Locations
            .AsNoTracking()
            .Where(l => l.IsActive)
            .OrderBy(l => l.Name)
            .Select(l => new { l.LocationId, l.Name })
            .ToListAsync();

        return Ok(locations);
    }
}
