using GrainManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrainManagement.API;

/// <summary>
/// Backs the /Remote landing page on the central deployment. Returns
/// every active location that has a server mapping (via
/// <see cref="LocationSequenceMapping"/>), with the server's URL so the
/// page can render a Connect button that opens the remote site in a
/// new tab.
/// </summary>
[ApiController]
[Route("api/Remote")]
public sealed class RemoteApiController : ControllerBase
{
    private readonly dbContext _ctx;
    public RemoteApiController(dbContext ctx) => _ctx = ctx;

    [HttpGet("Districts")]
    public async Task<IActionResult> Districts(CancellationToken ct)
    {
        // Districts that have at least one active location which is
        // mapped to a server. Skipped districts have nothing to remote
        // into and would just clutter the picker.
        var rows = await _ctx.LocationDistricts.AsNoTracking()
            .Where(d => d.IsActive
                        && d.Locations.Any(l => l.IsActive
                                                && _ctx.LocationSequenceMappings
                                                    .Any(m => m.LocationId == l.LocationId
                                                              && m.Server != null
                                                              && m.Server.IsActive)))
            .OrderBy(d => d.Name)
            .Select(d => new { d.DistrictId, d.Name })
            .ToListAsync(ct);
        return Ok(rows);
    }

    [HttpGet("Locations")]
    public async Task<IActionResult> Locations([FromQuery] int districtId, CancellationToken ct)
    {
        // districtId == 0 → every district (the "All Locations" filter).
        // Each row pairs a Location with the first active server it's
        // mapped to. Inactive servers / locations are dropped so the
        // remote list only surfaces something the operator can actually
        // connect to.
        var rows = await (
            from l in _ctx.Locations.AsNoTracking()
            where l.IsActive && (districtId == 0 || l.DistrictId == districtId)
            from m in _ctx.LocationSequenceMappings.Where(m => m.LocationId == l.LocationId).Take(1)
            where m.Server != null && m.Server.IsActive
            select new
            {
                l.LocationId,
                LocationName = l.Name ?? "",
                l.DistrictId,
                DistrictName = l.District != null ? (l.District.Name ?? "") : "",
                ServerId = m.ServerId,
                ServerName = m.Server.ServerName ?? "",
                ServerFriendlyName = m.Server.FriendlyName ?? "",
                Url = m.Server.Url ?? "",
            })
            .OrderBy(r => r.LocationName)
            .ToListAsync(ct);
        return Ok(rows);
    }
}
