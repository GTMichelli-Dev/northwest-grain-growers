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

        // The CreatedAt column is datetime2 (no timezone). EF returns DateTime
        // with Kind=Unspecified, which JSON.NET serializes without a 'Z'/offset
        // suffix — browsers then treat it as local time and never convert.
        // Tag each row's CreatedAt as UTC so the JSON carries 'Z' and the
        // client-side dxDataGrid `dataType: 'datetime'` renders in the user's
        // local timezone.
        var result = data.Select(a => new
        {
            a.AuditId,
            a.UserName,
            CreatedAt = DateTime.SpecifyKind(a.CreatedAt, DateTimeKind.Utc),
            a.LocationId,
            a.LocationName,
            a.ServerId,
            a.ServerName,
            a.TableName,
            a.Action,
            a.KeyJson,
            a.OldJson,
            a.NewJson,
        });

        return Ok(result);
    }
}
