using GrainManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrainManagement.API;

/// <summary>
/// Read-only API backing the /AgvantageWarehouseTransfer admin page.
/// Lists districts, their warehouse locations, and the closed weight sheets
/// inside a date range. The actual upload to AS400 is driven via SignalR
/// (As400SyncHub.RunWarehouseTransferUpload) — not through this controller.
/// </summary>
[ApiController]
[Route("api/AgvantageWarehouseTransfer")]
public sealed class AgvantageWarehouseTransferApiController : ControllerBase
{
    private const byte StatusClosed = 3;
    private static readonly TimeZoneInfo PacificTz = ResolvePacific();

    private readonly dbContext _ctx;

    public AgvantageWarehouseTransferApiController(dbContext ctx) => _ctx = ctx;

    private static TimeZoneInfo ResolvePacific()
    {
        try { return TimeZoneInfo.FindSystemTimeZoneById("America/Los_Angeles"); }
        catch
        {
            try { return TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"); }
            catch { return TimeZoneInfo.Local; }
        }
    }

    [HttpGet("Districts")]
    public async Task<IActionResult> Districts(CancellationToken ct)
    {
        // Districts that have at least one active warehouse-enabled location.
        var rows = await _ctx.LocationDistricts.AsNoTracking()
            .Where(d => d.IsActive
                        && d.Locations.Any(l => l.IsActive && l.UseForWarehouse))
            .OrderBy(d => d.Name)
            .Select(d => new { d.DistrictId, d.Name })
            .ToListAsync(ct);

        return Ok(rows);
    }

    [HttpGet("Locations")]
    public async Task<IActionResult> Locations([FromQuery] int districtId, CancellationToken ct)
    {
        if (districtId <= 0) return BadRequest(new { message = "districtId is required." });

        var rows = await _ctx.Locations.AsNoTracking()
            .Where(l => l.DistrictId == districtId && l.IsActive && l.UseForWarehouse)
            .OrderBy(l => l.Name)
            .Select(l => new { l.LocationId, l.Name, l.Code })
            .ToListAsync(ct);

        return Ok(rows);
    }

    public sealed class SheetsRequestDto
    {
        public DateOnly From { get; set; }
        public DateOnly To { get; set; }
        public int[] LocationIds { get; set; } = Array.Empty<int>();
    }

    [HttpPost("Sheets")]
    public async Task<IActionResult> Sheets([FromBody] SheetsRequestDto req, CancellationToken ct)
    {
        if (req is null) return BadRequest(new { message = "Request body required." });
        if (req.LocationIds is null || req.LocationIds.Length == 0)
            return Ok(Array.Empty<object>());

        DateOnly fromDate = req.From, toDate = req.To;
        if (toDate < fromDate) (fromDate, toDate) = (toDate, fromDate);
        var locIds = req.LocationIds;

        // CreationDate is a DateOnly column in Pacific calendar terms (the WS
        // is stamped on the day the operator creates it on the Remote site).
        // Filter is inclusive both ends.
        var rows = await (
            from ws in _ctx.WeightSheets.AsNoTracking()
            where ws.StatusId == StatusClosed
                  && ws.CreationDate >= fromDate && ws.CreationDate <= toDate
                  && (locIds.Contains(ws.LocationId)
                      || (ws.SourceLocationId != null && locIds.Contains(ws.SourceLocationId.Value))
                      || (ws.DestinationLocationId != null && locIds.Contains(ws.DestinationLocationId.Value)))
            select new
            {
                ws.WeightSheetId,
                ws.As400Id,
                ws.WeightSheetType,
                ws.LocationId,
                LocationName = _ctx.Locations.Where(l => l.LocationId == ws.LocationId)
                    .Select(l => l.Name).FirstOrDefault(),
                ws.SourceLocationId,
                SourceLocationName = ws.SourceLocationId == null ? null
                    : _ctx.Locations.Where(l => l.LocationId == ws.SourceLocationId).Select(l => l.Name).FirstOrDefault(),
                ws.DestinationLocationId,
                DestinationLocationName = ws.DestinationLocationId == null ? null
                    : _ctx.Locations.Where(l => l.LocationId == ws.DestinationLocationId).Select(l => l.Name).FirstOrDefault(),
                ws.LotId,
                LotAs400Id = ws.LotId == null ? (long?)null
                    : _ctx.Lots.Where(l => l.LotId == ws.LotId).Select(l => (long?)l.As400Id).FirstOrDefault(),
                Commodity = ws.WeightSheetType == "Delivery"
                    ? (ws.LotId == null ? null
                        : _ctx.Lots.Where(l => l.LotId == ws.LotId)
                            .Join(_ctx.Items, l => l.ItemId, i => i.ItemId, (l, i) => i.Description)
                            .FirstOrDefault())
                    : (ws.ItemId == null ? null
                        : _ctx.Items.Where(i => i.ItemId == ws.ItemId).Select(i => i.Description).FirstOrDefault()),
                ws.CreationDate,
                ws.ClosedAt,
                NetLbs = _ctx.InventoryTransactionDetails.AsNoTracking()
                    .Where(itd => itd.RefId == ws.RowUid && itd.RefType == "WeightSheet" && !itd.IsVoided)
                    .Sum(itd => (decimal?)(itd.NetQty ?? itd.DirectQty ?? 0)) ?? 0m,
            })
            .OrderBy(r => r.CreationDate)
            .ThenBy(r => r.WeightSheetId)
            .ToListAsync(ct);

        return Ok(rows);
    }
}
