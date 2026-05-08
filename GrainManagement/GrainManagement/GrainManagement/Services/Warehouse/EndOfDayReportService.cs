using GrainManagement.Dtos.Warehouse;
using GrainManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace GrainManagement.Services.Warehouse;

/// <summary>
/// Loads the three End-Of-Day summary report DTOs (Daily Intake, Daily
/// Transfer, Closed Lots) for a given day + location. Mirrors the data
/// shape of the legacy Crystal Reports so the new XtraReports drop in
/// without reformatting.
/// </summary>
public interface IEndOfDayReportService
{
    Task<DailyIntakeReportDto> BuildDailyIntakeAsync(int locationId, DateTime day, CancellationToken ct);
    Task<DailyTransferReportDto> BuildDailyTransferAsync(int locationId, DateTime day, CancellationToken ct);
    Task<ClosedLotsReportDto> BuildClosedLotsAsync(int locationId, DateTime fromDay, DateTime toDay, CancellationToken ct);

    /// <summary>
    /// Resolves the open WeightSheet ids at this location whose CreationDate
    /// is the given day. The merged-PDF endpoint uses this to know which
    /// per-WS reports to append after the daily summaries.
    /// </summary>
    Task<List<long>> GetTodaysOpenWeightSheetIdsAsync(int locationId, DateTime day, CancellationToken ct);

    /// <summary>
    /// Returns one row per locationId with an open-WS count and a
    /// prior-day-open flag, in the order the input ids were supplied. Used
    /// by the multi-location EOD orchestrator to decide which sites still
    /// need processing. Skips ids not present in the Locations table.
    /// </summary>
    Task<List<EodCandidateLocation>> GetEodCandidatesAsync(
        IReadOnlyList<int> locationIds, DateTime serverToday, CancellationToken ct);
}

public sealed class EodCandidateLocation
{
    public int LocationId { get; set; }
    public string Name { get; set; } = "";
    public string Code { get; set; } = "";
    public int OpenCount { get; set; }
    public bool HasPriorDayOpen { get; set; }
}

public sealed class EndOfDayReportService : IEndOfDayReportService
{
    private readonly dbContext _ctx;
    public EndOfDayReportService(dbContext ctx) => _ctx = ctx;

    public async Task<DailyIntakeReportDto> BuildDailyIntakeAsync(int locationId, DateTime day, CancellationToken ct)
    {
        var locationName = await _ctx.Locations.AsNoTracking()
            .Where(l => l.LocationId == locationId)
            .Select(l => l.Name)
            .FirstOrDefaultAsync(ct) ?? "";

        // End-Of-Day audience: every still-open Delivery WS at this
        // location, regardless of CreationDate. Filtering on the report's
        // day excluded WSs that were started on a prior day and never
        // closed — exactly the rows the operator is trying to clean up.
        // The `day` parameter survives only as the date stamp in the
        // header.
        // Each lookup is its own scalar subquery — no `let lot = ...`
        // intermediate, because EF Core can't translate downstream ternary
        // accesses through a let-bound entity (it tries to inline the whole
        // tree and chokes). Server-side ordering by Commodity is dropped
        // for the same reason; we sort in memory after materializing.
        var rows = await (
            from ws in _ctx.WeightSheets.AsNoTracking()
            where ws.LocationId == locationId
                  && ws.StatusId < 3 // not closed
                  && ws.WeightSheetType == "Delivery"
            select new
            {
                ws.WeightSheetId,
                ws.As400Id,
                ws.LotId,
                LotAs400Id = ws.LotId != null
                    ? _ctx.Lots.Where(l => l.LotId == ws.LotId)
                        .Select(l => (long?)l.As400Id).FirstOrDefault()
                    : null,
                LotIsOpen = ws.LotId != null && _ctx.Lots
                    .Where(l => l.LotId == ws.LotId)
                    .Select(l => l.IsOpen).FirstOrDefault(),
                ItemId = ws.LotId != null
                    ? _ctx.Lots.Where(l => l.LotId == ws.LotId)
                        .Select(l => l.ItemId).FirstOrDefault()
                    : null,
                // Commodity = lot's item description. Joined via Lots so
                // EF can translate the chain to a flat correlated subquery.
                Commodity = ws.LotId != null
                    ? _ctx.Lots
                        .Where(l => l.LotId == ws.LotId)
                        .Join(_ctx.Items, l => l.ItemId, i => i.ItemId, (l, i) => i.Description)
                        .FirstOrDefault()
                    : null,
                // Default UOM Code via Lot -> Item -> Product -> Category
                // -> UnitOfMeasure. Same chain pattern flattened with
                // Joins so EF translates it to a single SELECT.
                UomCode = ws.LotId != null
                    ? _ctx.Lots
                        .Where(l => l.LotId == ws.LotId)
                        .Join(_ctx.Items,    l => l.ItemId,    i => i.ItemId,    (l, i) => i)
                        .Join(_ctx.Products, i => i.ProductId, p => p.ProductId, (i, p) => p)
                        .Join(_ctx.Categories, p => p.CategoryId, c => c.CategoryId, (p, c) => c)
                        .Join(_ctx.UnitOfMeasures, c => c.DefaultUomId, u => (int?)u.UomId, (c, u) => u.Code)
                        .FirstOrDefault()
                    : null,
                UomFactor = ws.LotId != null
                    ? (decimal?)_ctx.Lots
                        .Where(l => l.LotId == ws.LotId)
                        .Join(_ctx.Items,    l => l.ItemId,    i => i.ItemId,    (l, i) => i)
                        .Join(_ctx.Products, i => i.ProductId, p => p.ProductId, (i, p) => p)
                        .Join(_ctx.Categories, p => p.CategoryId, c => c.CategoryId, (p, c) => c)
                        .Join(_ctx.UnitOfMeasures, c => c.DefaultUomId, u => (int?)u.UomId, (c, u) => u.ToBaseFactor)
                        .FirstOrDefault()
                    : null,
                AccountName = ws.LotId != null
                    ? _ctx.LotSplitGroups.Where(lsg => lsg.LotId == ws.LotId && lsg.PrimaryAccount)
                        .Join(_ctx.Accounts, lsg => lsg.AccountId, a => a.AccountId,
                              (lsg, a) => a.EntityName != null && a.EntityName != ""
                                          ? a.EntityName : a.LookupName)
                        .FirstOrDefault()
                    : null,
                AccountId = ws.LotId != null
                    ? _ctx.LotSplitGroups.Where(lsg => lsg.LotId == ws.LotId && lsg.PrimaryAccount)
                        .Select(lsg => (long?)lsg.AccountId).FirstOrDefault()
                    : null,
                Landlord = ws.LotId != null
                    ? _ctx.LotTraits.Where(t => t.LotId == ws.LotId && t.TraitTypeId == 18)
                        .Select(t => t.Trait.TraitCode).FirstOrDefault()
                    : null,
                FsaNumber = ws.LotId != null
                    ? _ctx.LotTraits.Where(t => t.LotId == ws.LotId && t.TraitTypeId == 19)
                        .Select(t => t.Trait.TraitCode).FirstOrDefault()
                    : null,
                NetQty = _ctx.InventoryTransactionDetails
                    .Where(itd => itd.RefId == ws.RowUid && itd.RefType == "WeightSheet" && !itd.IsVoided)
                    .Sum(itd => (decimal?)(itd.NetQty ?? itd.DirectQty ?? 0)) ?? 0m,
                Closed = ws.StatusId >= 3,
                Comment = ws.Notes,
            })
            .ToListAsync(ct);

        rows = rows
            .OrderBy(r => r.Commodity)
            .ThenBy(r => r.WeightSheetId)
            .ToList();

        var dto = new DailyIntakeReportDto
        {
            LocationName = locationName,
            LocationId = locationId.ToString(),
            CreationDate = day.ToString("MM/dd/yyyy"),
            Rows = rows.Select(r => new DailyIntakeReportRow
            {
                WeightSheetId = r.As400Id > 0 ? r.As400Id.ToString() : r.WeightSheetId.ToString(),
                Commodity = r.Commodity ?? "",
                CommodityId = r.ItemId?.ToString() ?? "",
                LotNumber = r.LotAs400Id.HasValue && r.LotAs400Id.Value > 0
                    ? r.LotAs400Id.Value.ToString()
                    : (r.LotId.HasValue ? r.LotId.Value.ToString() : ""),
                LotOpen = r.LotIsOpen,
                Customer = r.AccountName ?? "",
                CustomerId = r.AccountId?.ToString() ?? "",
                Landlord = r.Landlord ?? "",
                FsaNumber = r.FsaNumber ?? "",
                Comment = r.Comment ?? "",
                Net = r.NetQty,
                // Net (lbs) -> default UOM via Category.DefaultUom.ToBaseFactor.
                // Falls back to Net (treat factor=1) when the category has no
                // configured default — better than dropping rows.
                Units = (r.UomFactor.HasValue && r.UomFactor.Value > 0)
                    ? Math.Round(r.NetQty / r.UomFactor.Value, 2)
                    : r.NetQty,
                Uom = string.IsNullOrEmpty(r.UomCode) ? "lbs" : r.UomCode,
                Closed = r.Closed,
            }).ToList(),
        };

        return dto;
    }

    public async Task<DailyTransferReportDto> BuildDailyTransferAsync(int locationId, DateTime day, CancellationToken ct)
    {
        // Same audience as Delivery — every still-open Transfer WS at this
        // location, regardless of CreationDate. See BuildDailyIntakeAsync.
        var locationName = await _ctx.Locations.AsNoTracking()
            .Where(l => l.LocationId == locationId)
            .Select(l => l.Name)
            .FirstOrDefaultAsync(ct) ?? "";

        // Same flattening as the Delivery query — every lookup is its own
        // scalar subquery. Order is applied client-side after materializing.
        var rows = await (
            from ws in _ctx.WeightSheets.AsNoTracking()
            where ws.LocationId == locationId
                  && ws.StatusId < 3 // not closed
                  && ws.WeightSheetType == "Transfer"
            select new
            {
                ws.WeightSheetId,
                ws.As400Id,
                ws.ItemId,
                Commodity = ws.ItemId != null
                    ? _ctx.Items.Where(i => i.ItemId == ws.ItemId).Select(i => i.Description).FirstOrDefault()
                    : null,
                Variety = ws.ItemId != null
                    ? _ctx.Items.Where(i => i.ItemId == ws.ItemId).Select(i => i.Description).FirstOrDefault()
                    : null,
                Source = ws.SourceLocationId != null
                    ? _ctx.Locations.Where(l => l.LocationId == ws.SourceLocationId).Select(l => l.Name).FirstOrDefault()
                    : null,
                ws.SourceLocationId,
                Comment = ws.Notes,
                NetQty = _ctx.InventoryTransactionDetails
                    .Where(itd => itd.RefId == ws.RowUid && itd.RefType == "WeightSheet" && !itd.IsVoided)
                    .Sum(itd => (decimal?)(itd.NetQty ?? itd.DirectQty ?? 0)) ?? 0m,
                // Default UOM via Item -> Product -> Category -> DefaultUom.
                // The item lives on the WS itself for transfers (no lot).
                UomCode = ws.ItemId != null
                    ? _ctx.Items
                        .Where(i => i.ItemId == ws.ItemId)
                        .Join(_ctx.Products, i => i.ProductId, p => p.ProductId, (i, p) => p)
                        .Join(_ctx.Categories, p => p.CategoryId, c => c.CategoryId, (p, c) => c)
                        .Join(_ctx.UnitOfMeasures, c => c.DefaultUomId, u => (int?)u.UomId, (c, u) => u.Code)
                        .FirstOrDefault()
                    : null,
                UomFactor = ws.ItemId != null
                    ? (decimal?)_ctx.Items
                        .Where(i => i.ItemId == ws.ItemId)
                        .Join(_ctx.Products, i => i.ProductId, p => p.ProductId, (i, p) => p)
                        .Join(_ctx.Categories, p => p.CategoryId, c => c.CategoryId, (p, c) => c)
                        .Join(_ctx.UnitOfMeasures, c => c.DefaultUomId, u => (int?)u.UomId, (c, u) => u.ToBaseFactor)
                        .FirstOrDefault()
                    : null,
                Closed = ws.StatusId >= 3,
            })
            .ToListAsync(ct);

        rows = rows
            .OrderBy(r => r.Commodity)
            .ThenBy(r => r.WeightSheetId)
            .ToList();

        return new DailyTransferReportDto
        {
            LocationName = locationName,
            LocationId = locationId.ToString(),
            CreationDate = day.ToString("MM/dd/yyyy"),
            Rows = rows.Select(r => new DailyTransferReportRow
            {
                WeightSheetId = r.As400Id > 0 ? r.As400Id.ToString() : r.WeightSheetId.ToString(),
                Commodity = r.Commodity ?? "",
                CommodityId = r.ItemId?.ToString() ?? "",
                Source = r.Source ?? "",
                SourceId = r.SourceLocationId?.ToString() ?? "",
                Variety = r.Variety ?? "",
                VarietyId = r.ItemId?.ToString() ?? "",
                Comment = r.Comment ?? "",
                Net = r.NetQty,
                Units = (r.UomFactor.HasValue && r.UomFactor.Value > 0)
                    ? Math.Round(r.NetQty / r.UomFactor.Value, 2)
                    : r.NetQty,
                Uom = string.IsNullOrEmpty(r.UomCode) ? "lbs" : r.UomCode,
                Closed = r.Closed,
            }).ToList(),
        };
    }

    public async Task<ClosedLotsReportDto> BuildClosedLotsAsync(int locationId, DateTime fromDay, DateTime toDay, CancellationToken ct)
    {
        var locationName = await _ctx.Locations.AsNoTracking()
            .Where(l => l.LocationId == locationId)
            .Select(l => l.Name)
            .FirstOrDefaultAsync(ct) ?? "";

        var fromUtc = fromDay.Date;
        var toUtc = toDay.Date.AddDays(1);

        // Lots that closed in the date range at this location. UpdatedAt is
        // the closest proxy for "closed at" — when the close action flips
        // IsOpen=false, the trigger / API stamps UpdatedAt.
        var rows = await (
            from lot in _ctx.Lots.AsNoTracking()
            where lot.LocationId == locationId
                  && !lot.IsOpen
                  && lot.UpdatedAt != null
                  && lot.UpdatedAt >= fromUtc
                  && lot.UpdatedAt < toUtc
            select new
            {
                lot.LotId,
                lot.As400Id,
                lot.UpdatedAt,
                Crop = lot.ItemId != null
                    ? _ctx.Items.Where(i => i.ItemId == lot.ItemId).Select(i => i.Description).FirstOrDefault()
                    : null,
                Producer = _ctx.LotSplitGroups.Where(lsg => lsg.LotId == lot.LotId && lsg.PrimaryAccount)
                    .Join(_ctx.Accounts, lsg => lsg.AccountId, a => a.AccountId,
                          (lsg, a) => a.EntityName != null && a.EntityName != ""
                                      ? a.EntityName : a.LookupName)
                    .FirstOrDefault(),
            })
            .OrderBy(r => r.UpdatedAt)
            .ToListAsync(ct);

        return new ClosedLotsReportDto
        {
            LocationHeader = string.IsNullOrEmpty(locationName) ? "" : $"{locationName} ({locationId})",
            DateRangeHeader = fromDay.Date == toDay.Date
                ? fromDay.ToString("MM/dd/yyyy")
                : $"{fromDay:MM/dd/yyyy} - {toDay:MM/dd/yyyy}",
            Rows = rows.Select(r => new ClosedLotsReportRow
            {
                LotNumber = r.As400Id > 0 ? r.As400Id.ToString() : r.LotId.ToString(),
                CloseDate = r.UpdatedAt.HasValue ? r.UpdatedAt.Value.ToString("MM/dd/yyyy") : "",
                Crop = r.Crop ?? "",
                Producer = r.Producer ?? "",
            }).ToList(),
        };
    }

    public async Task<List<long>> GetTodaysOpenWeightSheetIdsAsync(int locationId, DateTime day, CancellationToken ct)
    {
        // The "today" framing is preserved in the method name for callers
        // that already use it, but the actual filter is "open at this
        // location" — operators close WSs that may have been created on
        // earlier days, so a strict day filter dropped exactly the rows
        // they were trying to finalize.
        return await _ctx.WeightSheets.AsNoTracking()
            .Where(ws => ws.LocationId == locationId && ws.StatusId < 3)
            .OrderBy(ws => ws.WeightSheetType)
            .ThenBy(ws => ws.WeightSheetId)
            .Select(ws => ws.WeightSheetId)
            .ToListAsync(ct);
    }

    public async Task<List<EodCandidateLocation>> GetEodCandidatesAsync(
        IReadOnlyList<int> locationIds, DateTime serverToday, CancellationToken ct)
    {
        if (locationIds == null || locationIds.Count == 0)
            return new List<EodCandidateLocation>();

        var todayDateOnly = DateOnly.FromDateTime(serverToday.Date);

        // One DB round-trip per dimension: location names, open WS counts
        // grouped by location, and prior-day-open flags grouped by location.
        // Materialize each into a dictionary, then assemble in the caller's
        // requested order.
        var locations = await _ctx.Locations.AsNoTracking()
            .Where(l => locationIds.Contains(l.LocationId))
            .Select(l => new { l.LocationId, l.Name, l.Code })
            .ToListAsync(ct);

        var openCounts = await _ctx.WeightSheets.AsNoTracking()
            .Where(ws => locationIds.Contains(ws.LocationId) && ws.StatusId < 3)
            .GroupBy(ws => ws.LocationId)
            .Select(g => new { LocationId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.LocationId, x => x.Count, ct);

        var priorDayOpenLocs = await _ctx.WeightSheets.AsNoTracking()
            .Where(ws => locationIds.Contains(ws.LocationId)
                         && ws.StatusId < 3
                         && ws.CreationDate < todayDateOnly)
            .Select(ws => ws.LocationId)
            .Distinct()
            .ToListAsync(ct);
        var priorDaySet = new HashSet<int>(priorDayOpenLocs);

        var byId = locations.ToDictionary(l => l.LocationId);
        var result = new List<EodCandidateLocation>(locationIds.Count);
        foreach (var id in locationIds)
        {
            if (!byId.TryGetValue(id, out var loc)) continue;
            result.Add(new EodCandidateLocation
            {
                LocationId = loc.LocationId,
                Name = loc.Name ?? "",
                Code = loc.Code ?? "",
                OpenCount = openCounts.TryGetValue(loc.LocationId, out var c) ? c : 0,
                HasPriorDayOpen = priorDaySet.Contains(loc.LocationId),
            });
        }
        return result;
    }
}
