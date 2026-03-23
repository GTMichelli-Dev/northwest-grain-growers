using GrainManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrainManagement.API;

[UseAdminConnection]
[ApiController]
[Route("api/AuditTrail")]
public sealed class AuditTrailApiController : ControllerBase
{
    private readonly dbContext _ctx;

    public AuditTrailApiController(dbContext ctx)
    {
        _ctx = ctx;
    }

    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] DateTime? dateFrom,
        [FromQuery] DateTime? dateTo,
        CancellationToken ct)
    {
        var query = _ctx.AuditTrails.AsNoTracking().AsQueryable();

        if (dateFrom.HasValue)
            query = query.Where(a => a.CreatedAt >= dateFrom.Value);
        if (dateTo.HasValue)
            query = query.Where(a => a.CreatedAt <= dateTo.Value.Date.AddDays(1));

        var data = await query
            .OrderByDescending(a => a.CreatedAt)
            .Select(a => new
            {
                a.AuditId,
                a.UserName,
                a.CreatedAt,
                a.LocationId,
                LocationName = _ctx.Locations
                    .Where(l => l.LocationId == a.LocationId)
                    .Select(l => l.Name)
                    .FirstOrDefault(),
                a.ServerId,
                ServerName = _ctx.Servers
                    .Where(s => s.ServerId == a.ServerId)
                    .Select(s => s.FriendlyName ?? s.ServerName)
                    .FirstOrDefault(),
                a.TableName,
                a.Action,
                a.KeyJson,
                a.OldJson,
                a.NewJson,
            })
            .ToListAsync(ct);

        return Ok(data);
    }
}
