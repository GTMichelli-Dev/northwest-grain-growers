#nullable enable
using GrainManagement.Dtos.Scales;
using GrainManagement.Services;
using GrainManagement.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

[ApiController]
[Route("api/[controller]")]
public class ScaleController : ControllerBase
{
    private readonly IScaleRegistry _scaleRegistry;
    private readonly IHubContext<ScaleHub, IScaleClient> _hub;

    public ScaleController(IScaleRegistry scaleRegistry, IHubContext<ScaleHub, IScaleClient> hub)
    {
        _scaleRegistry = scaleRegistry;
        _hub = hub;
    }

    [HttpPost("UpdateScale")]
    public async Task<IActionResult> UpdateScale([FromBody] ScaleDto scale)
    {
        if (scale == null) return BadRequest(new { error = "Invalid scale data." });
        if (scale.Id < 1) return BadRequest(new { error = "Id must be >= 1." });
        if (string.IsNullOrWhiteSpace(scale.Description)) return BadRequest(new { error = "Description is required." });

        _scaleRegistry.Upsert(scale);

        // Kiosks watching a specific scale
        await _hub.Clients.Group(ScaleHub.GroupName(scale.Id))
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
}
