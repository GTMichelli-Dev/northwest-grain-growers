using GrainManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace GrainManagement.Services.Warehouse;

/// <summary>
/// Hard-blocks new weight-sheet / load creation when prior-day weight sheets
/// are still open at the same location. Operators must close them out (via
/// End Of Day) before they can start new work — otherwise yesterday's data
/// quietly accumulates and never makes it onto an EOD report.
///
/// "Prior day" is computed in the configured server time zone
/// (<c>appsettings:TimeZone</c>) — not UTC — so a 9pm Pacific run doesn't
/// incorrectly classify today's WSs as already-prior just because UTC
/// rolled over.
/// </summary>
public interface IPriorDayWeightSheetGuard
{
    /// <summary>
    /// Returns the WeightSheetIds of every open weight sheet at this
    /// location that was created on a prior day. Empty list = clear to
    /// proceed.
    /// </summary>
    Task<List<long>> GetPriorDayOpenWeightSheetIdsAsync(int locationId, CancellationToken ct);
}

public sealed class PriorDayWeightSheetGuard : IPriorDayWeightSheetGuard
{
    private readonly dbContext _ctx;
    private readonly TimeZoneInfo _serverTz;

    public PriorDayWeightSheetGuard(dbContext ctx, IConfiguration config)
    {
        _ctx = ctx;
        _serverTz = ResolveServerTimeZone(config);
    }

    public async Task<List<long>> GetPriorDayOpenWeightSheetIdsAsync(int locationId, CancellationToken ct)
    {
        if (locationId <= 0) return new List<long>();

        var serverToday = DateOnly.FromDateTime(
            TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _serverTz).Date);

        return await _ctx.WeightSheets.AsNoTracking()
            .Where(ws => ws.LocationId == locationId
                         && ws.StatusId < 3
                         && ws.CreationDate < serverToday)
            .Select(ws => ws.WeightSheetId)
            .ToListAsync(ct);
    }

    private static TimeZoneInfo ResolveServerTimeZone(IConfiguration config)
    {
        var id = config["TimeZone"];
        if (!string.IsNullOrWhiteSpace(id))
        {
            try { return TimeZoneInfo.FindSystemTimeZoneById(id); }
            catch { /* fall through */ }
        }
        return TimeZoneInfo.Local;
    }
}
