using GrainManagement.Dtos.Warehouse;
using GrainManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace GrainManagement.Services.Warehouse;

/// <summary>
/// Loads data for the on-demand Reports views (Daily Intake/Transfer
/// picker, Daily Weight Sheet Series picker, Commodities By Date Range).
/// These are historical / multi-day reports — distinct from
/// <see cref="IEndOfDayReportService"/>, which targets the live EOD flow
/// and only operates on still-open weight sheets at one location.
/// </summary>
public interface IReportBuilderService
{
    Task<DailyIntakeReportDto> BuildDailyIntakeForDateAsync(
        int locationId, DateOnly day, CancellationToken ct);

    Task<DailyTransferReportDto> BuildDailyTransferForDateAsync(
        int locationId, DateOnly day, CancellationToken ct);

    Task<DailyWeightSheetSeriesReportDto> BuildWsSeriesAsync(
        int locationId, DateOnly day, CancellationToken ct);

    /// <summary>Daily intake/transfer picker candidate list — one row per
    /// (location, date, type) combination that had any weight-sheet
    /// activity in the given window.</summary>
    Task<List<SummaryCandidateDto>> GetSummaryCandidatesAsync(
        int[] locationIds, DateOnly fromDate, DateOnly toDate,
        bool includeIntake, bool includeTransfer, CancellationToken ct);

    /// <summary>WS-series picker candidate list — one row per
    /// (location, date) combination that had any weight-sheet
    /// activity in the window.</summary>
    Task<List<SeriesCandidateDto>> GetSeriesCandidatesAsync(
        int[] locationIds, DateOnly fromDate, DateOnly toDate, CancellationToken ct);

    Task<List<CommoditiesByDateRangeRowDto>> GetCommoditiesByDateRangeAsync(
        int[] locationIds, DateOnly fromDate, DateOnly toDate, CancellationToken ct);

    Task<List<BinProteinRowDto>> GetBinProteinAsync(
        int[] locationIds, DateOnly fromDate, DateOnly toDate, CancellationToken ct);

    Task<List<DailyLoadTimeRowDto>> GetDailyLoadTimesAsync(
        int[] locationIds, DateOnly fromDate, DateOnly toDate, CancellationToken ct);

    Task<List<PeakHourSlotDto>> GetPeakHoursAsync(
        int[] locationIds, DateOnly fromDate, DateOnly toDate, CancellationToken ct);

    /// <summary>Distinct crops (Product.CropId → parent crop Item) seen on
    /// intake weight sheets in the given window. Powers the multi-select
    /// Crop filter on the Producer Commodity report.</summary>
    Task<List<CropOptionDto>> GetIntakeCropsAsync(
        int[] locationIds, DateOnly fromDate, DateOnly toDate, CancellationToken ct);

    Task<List<ProducerCommodityRowDto>> GetProducerCommodityAsync(
        int[] locationIds, long[] cropIds, string accountSearch,
        DateOnly fromDate, DateOnly toDate, CancellationToken ct);

    /// <summary>Producer Delivery report — intake totals per producer
    /// (or per split group, depending on <paramref name="groupBySplit"/>).
    /// </summary>
    Task<List<ProducerDeliveryRowDto>> GetProducerDeliveryAsync(
        int[] locationIds, long[] cropIds, string accountSearch,
        bool groupBySplit, DateOnly fromDate, DateOnly toDate, CancellationToken ct);

    /// <summary>Intake Location report — same shape as the Producer
    /// Delivery report but with NetUnits + Primary UoM resolved from the
    /// Lot's item category.</summary>
    Task<List<IntakeLocationRowDto>> GetIntakeLocationAsync(
        int[] locationIds, long[] cropIds,
        bool groupBySplit, DateOnly fromDate, DateOnly toDate, CancellationToken ct);

    /// <summary>Load Dump Type report — one row per load that has an
    /// IS_END_DUMP transaction attribute set, in the given date range.</summary>
    Task<List<LoadDumpTypeRowDto>> GetLoadDumpTypesAsync(
        DateOnly fromDate, DateOnly toDate, CancellationToken ct);
}

public sealed class CropOptionDto
{
    public long CropId { get; set; }
    public string Description { get; set; } = "";
}

public sealed class SummaryCandidateDto
{
    public int LocationId { get; set; }
    public string LocationName { get; set; } = "";
    public DateOnly Date { get; set; }
    /// <summary>"Intake" or "Transfer".</summary>
    public string Type { get; set; } = "";
    public int LoadCount { get; set; }
}

public sealed class SeriesCandidateDto
{
    public int LocationId { get; set; }
    public string LocationName { get; set; } = "";
    public DateOnly Date { get; set; }
    public int WeightSheetCount { get; set; }
    public int LoadCount { get; set; }
}

public sealed class ReportBuilderService : IReportBuilderService
{
    private readonly dbContext _ctx;
    public ReportBuilderService(dbContext ctx) => _ctx = ctx;

    // ── Helper: chase Item → Product → Category → DefaultUom for the
    //    crop label, primary UoM code, and ToBaseFactor. The chain is
    //    flat scalar subqueries so EF can translate to a single SELECT
    //    per WS row.
    private record CropInfo(
        string Crop, string Uom, decimal? Factor, long? CropItemId);

    // ── Daily Intake (historical, by CreationDate) ─────────────────────
    public async Task<DailyIntakeReportDto> BuildDailyIntakeForDateAsync(
        int locationId, DateOnly day, CancellationToken ct)
    {
        var locationName = await _ctx.Locations.AsNoTracking()
            .Where(l => l.LocationId == locationId)
            .Select(l => l.Name)
            .FirstOrDefaultAsync(ct) ?? "";

        var rows = await (
            from ws in _ctx.WeightSheets.AsNoTracking()
            where ws.LocationId == locationId
                  && ws.CreationDate == day
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
                Commodity = ws.LotId != null
                    ? _ctx.Lots.Where(l => l.LotId == ws.LotId)
                        .Join(_ctx.Items, l => l.ItemId, i => i.ItemId, (l, i) => i.Description)
                        .FirstOrDefault()
                    : null,
                UomCode = ws.LotId != null
                    ? _ctx.Lots.Where(l => l.LotId == ws.LotId)
                        .Join(_ctx.Items,    l => l.ItemId,    i => i.ItemId,    (l, i) => i)
                        .Join(_ctx.Products, i => i.ProductId, p => p.ProductId, (i, p) => p)
                        .Join(_ctx.Categories, p => p.CategoryId, c => c.CategoryId, (p, c) => c)
                        .Join(_ctx.UnitOfMeasures, c => c.DefaultUomId, u => (int?)u.UomId, (c, u) => u.Code)
                        .FirstOrDefault()
                    : null,
                UomFactor = ws.LotId != null
                    ? (decimal?)_ctx.Lots.Where(l => l.LotId == ws.LotId)
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

        rows = rows.OrderBy(r => r.Commodity).ThenBy(r => r.WeightSheetId).ToList();

        return new DailyIntakeReportDto
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
                Units = (r.UomFactor.HasValue && r.UomFactor.Value > 0)
                    ? Math.Round(r.NetQty / r.UomFactor.Value, 2)
                    : r.NetQty,
                Uom = string.IsNullOrEmpty(r.UomCode) ? "lbs" : r.UomCode,
                Closed = r.Closed,
            }).ToList(),
        };
    }

    // ── Daily Transfer (historical, by CreationDate) ───────────────────
    public async Task<DailyTransferReportDto> BuildDailyTransferForDateAsync(
        int locationId, DateOnly day, CancellationToken ct)
    {
        var locationName = await _ctx.Locations.AsNoTracking()
            .Where(l => l.LocationId == locationId)
            .Select(l => l.Name)
            .FirstOrDefaultAsync(ct) ?? "";

        var rows = await (
            from ws in _ctx.WeightSheets.AsNoTracking()
            where ws.LocationId == locationId
                  && ws.CreationDate == day
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
                UomCode = ws.ItemId != null
                    ? _ctx.Items.Where(i => i.ItemId == ws.ItemId)
                        .Join(_ctx.Products, i => i.ProductId, p => p.ProductId, (i, p) => p)
                        .Join(_ctx.Categories, p => p.CategoryId, c => c.CategoryId, (p, c) => c)
                        .Join(_ctx.UnitOfMeasures, c => c.DefaultUomId, u => (int?)u.UomId, (c, u) => u.Code)
                        .FirstOrDefault()
                    : null,
                UomFactor = ws.ItemId != null
                    ? (decimal?)_ctx.Items.Where(i => i.ItemId == ws.ItemId)
                        .Join(_ctx.Products, i => i.ProductId, p => p.ProductId, (i, p) => p)
                        .Join(_ctx.Categories, p => p.CategoryId, c => c.CategoryId, (p, c) => c)
                        .Join(_ctx.UnitOfMeasures, c => c.DefaultUomId, u => (int?)u.UomId, (c, u) => u.ToBaseFactor)
                        .FirstOrDefault()
                    : null,
                Closed = ws.StatusId >= 3,
            })
            .ToListAsync(ct);

        rows = rows.OrderBy(r => r.Commodity).ThenBy(r => r.WeightSheetId).ToList();

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

    // ── Daily Weight Sheet Series report ───────────────────────────────
    // One row per WS at the location on the day. Mixes Delivery and
    // Transfer; "Crop" resolves through Lot/Item → Product.CropId →
    // Item.Description for the parent-crop item.
    public async Task<DailyWeightSheetSeriesReportDto> BuildWsSeriesAsync(
        int locationId, DateOnly day, CancellationToken ct)
    {
        var locationName = await _ctx.Locations.AsNoTracking()
            .Where(l => l.LocationId == locationId)
            .Select(l => l.Name)
            .FirstOrDefaultAsync(ct) ?? "";

        var raw = await (
            from ws in _ctx.WeightSheets.AsNoTracking()
            where ws.LocationId == locationId && ws.CreationDate == day
            // Resolve the WS's item id once, then chase to the parent crop
            // (Product.CropId → Items). Delivery uses the lot's item;
            // transfer uses the WS's item directly.
            let itemId = ws.WeightSheetType == "Delivery"
                ? (ws.LotId != null
                    ? _ctx.Lots.Where(l => l.LotId == ws.LotId).Select(l => l.ItemId).FirstOrDefault()
                    : null)
                : ws.ItemId
            select new
            {
                ws.WeightSheetId,
                ws.As400Id,
                ws.RowUid,
                ws.WeightSheetType,
                CropId = itemId != null
                    ? (long?)_ctx.Items.Where(i => i.ItemId == itemId)
                        .Join(_ctx.Products, i => i.ProductId, p => p.ProductId, (i, p) => p.CropId)
                        .FirstOrDefault()
                    : null,
                ItemDescription = itemId != null
                    ? _ctx.Items.Where(i => i.ItemId == itemId).Select(i => i.Description).FirstOrDefault()
                    : null,
                NetLbs = _ctx.InventoryTransactionDetails
                    .Where(itd => itd.RefId == ws.RowUid && itd.RefType == "WeightSheet" && !itd.IsVoided)
                    .Sum(itd => (decimal?)(itd.NetQty ?? itd.DirectQty ?? 0)) ?? 0m,
                LoadCount = _ctx.InventoryTransactionDetails
                    .Count(itd => itd.RefId == ws.RowUid && itd.RefType == "WeightSheet" && !itd.IsVoided),
            })
            .ToListAsync(ct);

        // Resolve crop labels via the parent-crop ItemId. Distinct lookup
        // so we don't hit the Items table once per row.
        var cropIds = raw.Where(r => r.CropId.HasValue && r.CropId.Value > 0)
            .Select(r => r.CropId!.Value).Distinct().ToList();
        var cropLabels = cropIds.Count == 0
            ? new Dictionary<long, string>()
            : await _ctx.Items.AsNoTracking()
                .Where(i => cropIds.Contains(i.ItemId))
                .ToDictionaryAsync(i => i.ItemId, i => i.Description ?? "", ct);

        var rows = raw
            .OrderBy(r => r.WeightSheetType)
            .ThenBy(r => r.WeightSheetId)
            .Select(r => new DailyWeightSheetSeriesRow
            {
                WeightSheetId = r.As400Id > 0 ? r.As400Id.ToString() : r.WeightSheetId.ToString(),
                Type = r.WeightSheetType ?? "",
                Crop = (r.CropId.HasValue && cropLabels.TryGetValue(r.CropId.Value, out var label))
                    ? label
                    : (r.ItemDescription ?? ""),
                LoadCount = r.LoadCount,
                NetLbs = r.NetLbs,
                NetDisplay = r.NetLbs.ToString("N0") + " lbs",
            })
            .ToList();

        var totalLoads = rows.Sum(r => r.LoadCount);
        var totalNet = rows.Sum(r => r.NetLbs);

        return new DailyWeightSheetSeriesReportDto
        {
            LocationName = locationName,
            LocationId = locationId.ToString(),
            ReportDate = day.ToString("MM/dd/yyyy"),
            Rows = rows,
            TotalLoads = totalLoads,
            TotalNetLbs = totalNet,
            TotalNetDisplay = totalNet.ToString("N0") + " lbs",
        };
    }

    // ── Picker candidate lists ─────────────────────────────────────────
    public async Task<List<SummaryCandidateDto>> GetSummaryCandidatesAsync(
        int[] locationIds, DateOnly fromDate, DateOnly toDate,
        bool includeIntake, bool includeTransfer, CancellationToken ct)
    {
        if (locationIds == null || locationIds.Length == 0)
            return new List<SummaryCandidateDto>();
        if (toDate < fromDate) (fromDate, toDate) = (toDate, fromDate);
        if (!includeIntake && !includeTransfer)
            return new List<SummaryCandidateDto>();

        var locNameById = await _ctx.Locations.AsNoTracking()
            .Where(l => locationIds.Contains(l.LocationId))
            .ToDictionaryAsync(l => l.LocationId, l => l.Name ?? "", ct);

        var groups = await (
            from ws in _ctx.WeightSheets.AsNoTracking()
            where locationIds.Contains(ws.LocationId)
                  && ws.CreationDate >= fromDate
                  && ws.CreationDate <= toDate
                  && ((includeIntake && ws.WeightSheetType == "Delivery")
                      || (includeTransfer && ws.WeightSheetType == "Transfer"))
            group ws by new { ws.LocationId, ws.CreationDate, ws.WeightSheetType }
            into g
            select new
            {
                g.Key.LocationId,
                g.Key.CreationDate,
                g.Key.WeightSheetType,
                LoadCount = g.Sum(w => _ctx.InventoryTransactionDetails
                    .Count(itd => itd.RefId == w.RowUid && itd.RefType == "WeightSheet" && !itd.IsVoided))
            })
            .ToListAsync(ct);

        return groups
            .Select(g => new SummaryCandidateDto
            {
                LocationId = g.LocationId,
                LocationName = locNameById.TryGetValue(g.LocationId, out var n) ? n : "",
                Date = g.CreationDate,
                Type = string.Equals(g.WeightSheetType, "Delivery", StringComparison.OrdinalIgnoreCase)
                    ? "Intake" : "Transfer",
                LoadCount = g.LoadCount,
            })
            .OrderByDescending(c => c.Date)
            .ThenBy(c => c.LocationName, StringComparer.OrdinalIgnoreCase)
            .ThenBy(c => c.Type, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    public async Task<List<SeriesCandidateDto>> GetSeriesCandidatesAsync(
        int[] locationIds, DateOnly fromDate, DateOnly toDate, CancellationToken ct)
    {
        if (locationIds == null || locationIds.Length == 0)
            return new List<SeriesCandidateDto>();
        if (toDate < fromDate) (fromDate, toDate) = (toDate, fromDate);

        var locNameById = await _ctx.Locations.AsNoTracking()
            .Where(l => locationIds.Contains(l.LocationId))
            .ToDictionaryAsync(l => l.LocationId, l => l.Name ?? "", ct);

        var groups = await (
            from ws in _ctx.WeightSheets.AsNoTracking()
            where locationIds.Contains(ws.LocationId)
                  && ws.CreationDate >= fromDate
                  && ws.CreationDate <= toDate
            group ws by new { ws.LocationId, ws.CreationDate } into g
            select new
            {
                g.Key.LocationId,
                g.Key.CreationDate,
                WeightSheetCount = g.Count(),
                LoadCount = g.Sum(w => _ctx.InventoryTransactionDetails
                    .Count(itd => itd.RefId == w.RowUid && itd.RefType == "WeightSheet" && !itd.IsVoided))
            })
            .ToListAsync(ct);

        return groups
            .Select(g => new SeriesCandidateDto
            {
                LocationId = g.LocationId,
                LocationName = locNameById.TryGetValue(g.LocationId, out var n) ? n : "",
                Date = g.CreationDate,
                WeightSheetCount = g.WeightSheetCount,
                LoadCount = g.LoadCount,
            })
            .OrderByDescending(c => c.Date)
            .ThenBy(c => c.LocationName, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    // ── Commodities by date range ──────────────────────────────────────
    // Per-WS net + crop + UoM resolution. The same WS appears at most
    // once per bucket (Intake / TransferFrom / TransferTo) because each
    // bucket maps to a different LocationId column on the WS.
    public async Task<List<CommoditiesByDateRangeRowDto>> GetCommoditiesByDateRangeAsync(
        int[] locationIds, DateOnly fromDate, DateOnly toDate, CancellationToken ct)
    {
        if (locationIds == null || locationIds.Length == 0)
            return new List<CommoditiesByDateRangeRowDto>();
        if (toDate < fromDate) (fromDate, toDate) = (toDate, fromDate);

        var locNameById = await _ctx.Locations.AsNoTracking()
            .Where(l => locationIds.Contains(l.LocationId))
            .ToDictionaryAsync(l => l.LocationId, l => l.Name ?? "", ct);

        // Pull every WS that touches one of the requested locations as
        // home / source / destination, with the resolved item id.
        var raw = await (
            from ws in _ctx.WeightSheets.AsNoTracking()
            where ws.CreationDate >= fromDate
                  && ws.CreationDate <= toDate
                  && (locationIds.Contains(ws.LocationId)
                      || (ws.SourceLocationId != null && locationIds.Contains(ws.SourceLocationId.Value))
                      || (ws.DestinationLocationId != null && locationIds.Contains(ws.DestinationLocationId.Value)))
            let itemId = ws.WeightSheetType == "Delivery"
                ? (ws.LotId != null
                    ? _ctx.Lots.Where(l => l.LotId == ws.LotId).Select(l => l.ItemId).FirstOrDefault()
                    : null)
                : ws.ItemId
            select new
            {
                ws.WeightSheetType,
                ws.LocationId,
                ws.SourceLocationId,
                ws.DestinationLocationId,
                ItemId = itemId,
                CropId = itemId != null
                    ? (long?)_ctx.Items.Where(i => i.ItemId == itemId)
                        .Join(_ctx.Products, i => i.ProductId, p => p.ProductId, (i, p) => p.CropId)
                        .FirstOrDefault()
                    : null,
                UomCode = itemId != null
                    ? _ctx.Items.Where(i => i.ItemId == itemId)
                        .Join(_ctx.Products, i => i.ProductId, p => p.ProductId, (i, p) => p)
                        .Join(_ctx.Categories, p => p.CategoryId, c => c.CategoryId, (p, c) => c)
                        .Join(_ctx.UnitOfMeasures, c => c.DefaultUomId, u => (int?)u.UomId, (c, u) => u.Code)
                        .FirstOrDefault()
                    : null,
                UomFactor = itemId != null
                    ? (decimal?)_ctx.Items.Where(i => i.ItemId == itemId)
                        .Join(_ctx.Products, i => i.ProductId, p => p.ProductId, (i, p) => p)
                        .Join(_ctx.Categories, p => p.CategoryId, c => c.CategoryId, (p, c) => c)
                        .Join(_ctx.UnitOfMeasures, c => c.DefaultUomId, u => (int?)u.UomId, (c, u) => u.ToBaseFactor)
                        .FirstOrDefault()
                    : null,
                NetLbs = _ctx.InventoryTransactionDetails
                    .Where(itd => itd.RefId == ws.RowUid && itd.RefType == "WeightSheet" && !itd.IsVoided)
                    .Sum(itd => (decimal?)(itd.NetQty ?? itd.DirectQty ?? 0)) ?? 0m,
                LoadCount = _ctx.InventoryTransactionDetails
                    .Count(itd => itd.RefId == ws.RowUid && itd.RefType == "WeightSheet" && !itd.IsVoided),
            })
            .ToListAsync(ct);

        // Build crop label dictionary in one trip.
        var cropIds = raw.Where(r => r.CropId.HasValue && r.CropId.Value > 0)
            .Select(r => r.CropId!.Value).Distinct().ToList();
        var cropLabels = cropIds.Count == 0
            ? new Dictionary<long, string>()
            : await _ctx.Items.AsNoTracking()
                .Where(i => cropIds.Contains(i.ItemId))
                .ToDictionaryAsync(i => i.ItemId, i => i.Description ?? "", ct);

        // Fan WSs out into (LocationId, Crop, bucket) keys. A transfer
        // counts toward both the source location's "From" bucket and
        // the destination's "To" bucket — but only if those locations
        // are in the user's filter set.
        var keyed = new Dictionary<(int LocId, string Crop), CommoditiesByDateRangeRowDto>();

        CommoditiesByDateRangeRowDto Row(int locId, string crop, string uom)
        {
            var key = (locId, crop);
            if (!keyed.TryGetValue(key, out var row))
            {
                row = new CommoditiesByDateRangeRowDto
                {
                    LocationId = locId,
                    LocationName = locNameById.TryGetValue(locId, out var n) ? n : "",
                    Crop = crop,
                    PrimaryUom = uom,
                };
                keyed[key] = row;
            }
            // Late-binding the UoM if the first WS for this crop didn't
            // resolve one but a later WS does — keeps the column non-empty
            // when one of the items is half-configured.
            if (string.IsNullOrEmpty(row.PrimaryUom) && !string.IsNullOrEmpty(uom))
                row.PrimaryUom = uom;
            return row;
        }

        var locSet = new HashSet<int>(locationIds);

        foreach (var r in raw)
        {
            var crop = (r.CropId.HasValue && cropLabels.TryGetValue(r.CropId.Value, out var label))
                ? label : "(unknown)";
            var uom = string.IsNullOrEmpty(r.UomCode) ? "" : r.UomCode;
            var net = (r.UomFactor.HasValue && r.UomFactor.Value > 0)
                ? Math.Round(r.NetLbs / r.UomFactor.Value, 2)
                : r.NetLbs;

            if (string.Equals(r.WeightSheetType, "Delivery", StringComparison.OrdinalIgnoreCase))
            {
                if (locSet.Contains(r.LocationId))
                {
                    var row = Row(r.LocationId, crop, uom);
                    row.IntakeNet += net;
                    row.IntakeLoadCount += r.LoadCount;
                }
            }
            else if (string.Equals(r.WeightSheetType, "Transfer", StringComparison.OrdinalIgnoreCase))
            {
                if (r.SourceLocationId.HasValue && locSet.Contains(r.SourceLocationId.Value))
                {
                    var row = Row(r.SourceLocationId.Value, crop, uom);
                    row.TransferFromNet += net;
                    row.TransferFromLoadCount += r.LoadCount;
                }
                if (r.DestinationLocationId.HasValue && locSet.Contains(r.DestinationLocationId.Value))
                {
                    var row = Row(r.DestinationLocationId.Value, crop, uom);
                    row.TransferToNet += net;
                    row.TransferToLoadCount += r.LoadCount;
                }
            }
        }

        foreach (var row in keyed.Values)
        {
            row.NetChange = row.IntakeNet + row.TransferToNet - row.TransferFromNet;
        }

        return keyed.Values
            .OrderBy(r => r.LocationName, StringComparer.OrdinalIgnoreCase)
            .ThenBy(r => r.Crop, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    // ── Bin Protein Weighted Average ───────────────────────────────────
    // Per-load query joining the WS, the load's container (bin), the
    // load's bushel conversion (Lot → Item → Product → Category → UoM),
    // and the PROTEIN transaction attribute. Weighted average is
    // sum(protein × bushels) / sum(bushels) where protein > 0; loads
    // without a non-zero protein still count toward TotalBushels and
    // LoadCount but not the weighted figure.
    public async Task<List<BinProteinRowDto>> GetBinProteinAsync(
        int[] locationIds, DateOnly fromDate, DateOnly toDate, CancellationToken ct)
    {
        if (locationIds == null || locationIds.Length == 0)
            return new List<BinProteinRowDto>();
        if (toDate < fromDate) (fromDate, toDate) = (toDate, fromDate);

        var locNameById = await _ctx.Locations.AsNoTracking()
            .Where(l => locationIds.Contains(l.LocationId))
            .ToDictionaryAsync(l => l.LocationId, l => l.Name ?? "", ct);

        var raw = await (
            from ws in _ctx.WeightSheets.AsNoTracking()
            where locationIds.Contains(ws.LocationId)
                  && ws.CreationDate >= fromDate
                  && ws.CreationDate <= toDate
                  && ws.WeightSheetType == "Delivery"
            from itd in _ctx.InventoryTransactionDetails
                .Where(d => d.RefId == ws.RowUid && d.RefType == "WeightSheet" && !d.IsVoided)
            select new
            {
                ws.LocationId,
                NetLbs = (decimal?)(itd.NetQty ?? itd.DirectQty ?? 0) ?? 0m,
                Bin = _ctx.InventoryTransactionDetailToContainers
                    .Where(c => c.TransactionId == itd.TransactionId)
                    .Join(_ctx.Containers, c => c.ContainerId, c2 => c2.ContainerId, (c, c2) => c2.Description)
                    .FirstOrDefault(),
                UomFactor = ws.LotId != null
                    ? (decimal?)_ctx.Lots.Where(l => l.LotId == ws.LotId)
                        .Join(_ctx.Items,    l => l.ItemId,    i => i.ItemId,    (l, i) => i)
                        .Join(_ctx.Products, i => i.ProductId, p => p.ProductId, (i, p) => p)
                        .Join(_ctx.Categories, p => p.CategoryId, c => c.CategoryId, (p, c) => c)
                        .Join(_ctx.UnitOfMeasures, c => c.DefaultUomId, u => (int?)u.UomId, (c, u) => u.ToBaseFactor)
                        .FirstOrDefault()
                    : null,
                Protein = _ctx.TransactionAttributes
                    .Where(a => a.TransactionId == itd.TransactionId
                                && a.AttributeType.Code == "PROTEIN")
                    .Select(a => a.DecimalValue)
                    .FirstOrDefault(),
            })
            .ToListAsync(ct);

        var keyed = new Dictionary<(int LocId, string Bin), BinProteinRowDto>();
        var weightedSums = new Dictionary<(int, string), (decimal sumNumerator, decimal sumWeights)>();

        foreach (var r in raw)
        {
            var binName = string.IsNullOrEmpty(r.Bin) ? "(no bin)" : r.Bin;
            var key = (r.LocationId, binName);
            if (!keyed.TryGetValue(key, out var row))
            {
                row = new BinProteinRowDto
                {
                    LocationId = r.LocationId,
                    LocationName = locNameById.TryGetValue(r.LocationId, out var n) ? n : "",
                    Bin = binName,
                };
                keyed[key] = row;
                weightedSums[key] = (0m, 0m);
            }

            decimal bushels = (r.UomFactor.HasValue && r.UomFactor.Value > 0)
                ? Math.Round(r.NetLbs / r.UomFactor.Value, 4)
                : 0m;

            row.TotalBushels += bushels;
            row.LoadCount++;

            if (r.Protein.HasValue && r.Protein.Value > 0m && bushels > 0m)
            {
                var sums = weightedSums[key];
                weightedSums[key] = (sums.sumNumerator + r.Protein.Value * bushels,
                                     sums.sumWeights + bushels);
                row.SampledBushels += bushels;
                row.SamplesUsed++;
            }
        }

        foreach (var kv in weightedSums)
        {
            var row = keyed[kv.Key];
            row.AvgProtein = kv.Value.sumWeights > 0m
                ? Math.Round(kv.Value.sumNumerator / kv.Value.sumWeights, 3)
                : 0m;
            row.TotalBushels = Math.Round(row.TotalBushels, 2);
            row.SampledBushels = Math.Round(row.SampledBushels, 2);
        }

        return keyed.Values
            .OrderBy(r => r.LocationName, StringComparer.OrdinalIgnoreCase)
            .ThenBy(r => r.Bin, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    // ── Daily Load Times ───────────────────────────────────────────────
    public async Task<List<DailyLoadTimeRowDto>> GetDailyLoadTimesAsync(
        int[] locationIds, DateOnly fromDate, DateOnly toDate, CancellationToken ct)
    {
        if (locationIds == null || locationIds.Length == 0)
            return new List<DailyLoadTimeRowDto>();
        if (toDate < fromDate) (fromDate, toDate) = (toDate, fromDate);

        var locNameById = await _ctx.Locations.AsNoTracking()
            .Where(l => locationIds.Contains(l.LocationId))
            .ToDictionaryAsync(l => l.LocationId, l => l.Name ?? "", ct);

        var raw = await (
            from ws in _ctx.WeightSheets.AsNoTracking()
            where locationIds.Contains(ws.LocationId)
                  && ws.CreationDate >= fromDate
                  && ws.CreationDate <= toDate
            from itd in _ctx.InventoryTransactionDetails
                .Where(d => d.RefId == ws.RowUid && d.RefType == "WeightSheet" && !d.IsVoided)
            select new
            {
                ws.LocationId,
                ws.CreationDate,
                ws.WeightSheetId,
                ws.As400Id,
                itd.TransactionId,
                itd.StartedAt,
                itd.CompletedAt,
                NetLbs = (decimal?)(itd.NetQty ?? itd.DirectQty ?? 0) ?? 0m,
                Bin = _ctx.InventoryTransactionDetailToContainers
                    .Where(c => c.TransactionId == itd.TransactionId)
                    .Join(_ctx.Containers, c => c.ContainerId, c2 => c2.ContainerId, (c, c2) => c2.Description)
                    .FirstOrDefault(),
                TruckId = _ctx.TransactionAttributes
                    .Where(a => a.TransactionId == itd.TransactionId
                                && a.AttributeType.Code == "TRUCK_ID")
                    .Select(a => a.StringValue)
                    .FirstOrDefault(),
            })
            .ToListAsync(ct);

        return raw
            .Select(r =>
            {
                decimal? minutes = null;
                if (r.StartedAt.HasValue && r.CompletedAt.HasValue)
                {
                    var diff = r.CompletedAt.Value - r.StartedAt.Value;
                    minutes = (decimal)Math.Round(diff.TotalMinutes, 2);
                }
                return new DailyLoadTimeRowDto
                {
                    LocationId = r.LocationId,
                    LocationName = locNameById.TryGetValue(r.LocationId, out var n) ? n : "",
                    Date = r.CreationDate.ToString("yyyy-MM-dd"),
                    WeightSheetId = r.As400Id > 0 ? r.As400Id.ToString() : r.WeightSheetId.ToString(),
                    LoadId = r.TransactionId.ToString(),
                    TruckId = r.TruckId ?? "",
                    Bin = r.Bin ?? "",
                    TimeIn = r.StartedAt,
                    TimeOut = r.CompletedAt,
                    DurationMinutes = minutes,
                    NetLbs = r.NetLbs,
                };
            })
            .OrderBy(r => r.Date)
            .ThenBy(r => r.LocationName, StringComparer.OrdinalIgnoreCase)
            .ThenBy(r => r.TimeIn ?? DateTime.MinValue)
            .ToList();
    }

    // ── Peak Hours ─────────────────────────────────────────────────────
    // Per-location load counts bucketed by hour-of-day on TxnAt, plus an
    // aggregate row per hour with LocationId=0/Name="All". Hour is the
    // load's TxnAt hour in the server's local time (Pacific in
    // production), matching how StartedAt is stored.
    public async Task<List<PeakHourSlotDto>> GetPeakHoursAsync(
        int[] locationIds, DateOnly fromDate, DateOnly toDate, CancellationToken ct)
    {
        if (locationIds == null || locationIds.Length == 0)
            return new List<PeakHourSlotDto>();
        if (toDate < fromDate) (fromDate, toDate) = (toDate, fromDate);

        var locNameById = await _ctx.Locations.AsNoTracking()
            .Where(l => locationIds.Contains(l.LocationId))
            .ToDictionaryAsync(l => l.LocationId, l => l.Name ?? "", ct);

        var raw = await (
            from ws in _ctx.WeightSheets.AsNoTracking()
            where locationIds.Contains(ws.LocationId)
                  && ws.CreationDate >= fromDate
                  && ws.CreationDate <= toDate
            from itd in _ctx.InventoryTransactionDetails
                .Where(d => d.RefId == ws.RowUid && d.RefType == "WeightSheet" && !d.IsVoided)
            select new
            {
                ws.LocationId,
                Stamp = (DateTime?)(itd.StartedAt ?? itd.TxnAt),
            })
            .ToListAsync(ct);

        var perLocation = new Dictionary<(int LocId, int Hour), int>();
        var aggregate = new Dictionary<int, int>();

        foreach (var r in raw)
        {
            if (!r.Stamp.HasValue) continue;
            int hour = r.Stamp.Value.Hour;
            var key = (r.LocationId, hour);
            perLocation[key] = perLocation.TryGetValue(key, out var c) ? c + 1 : 1;
            aggregate[hour] = aggregate.TryGetValue(hour, out var ac) ? ac + 1 : 1;
        }

        var result = new List<PeakHourSlotDto>();

        // Emit one slot per (location, hour) for hours that have loads,
        // padding to a full 0-23 axis so the chart line doesn't jump
        // missing hours. Same for the aggregate.
        foreach (var locId in locationIds)
        {
            for (int h = 0; h < 24; h++)
            {
                perLocation.TryGetValue((locId, h), out var count);
                result.Add(new PeakHourSlotDto
                {
                    LocationId = locId,
                    LocationName = locNameById.TryGetValue(locId, out var n) ? n : ("Location " + locId),
                    Hour = h,
                    HourLabel = h.ToString("00") + ":00",
                    LoadCount = count,
                });
            }
        }
        for (int h = 0; h < 24; h++)
        {
            aggregate.TryGetValue(h, out var count);
            result.Add(new PeakHourSlotDto
            {
                LocationId = 0,
                LocationName = "All",
                Hour = h,
                HourLabel = h.ToString("00") + ":00",
                LoadCount = count,
            });
        }

        return result;
    }

    // ── Producer Commodity (intake only) ───────────────────────────────
    public async Task<List<CropOptionDto>> GetIntakeCropsAsync(
        int[] locationIds, DateOnly fromDate, DateOnly toDate, CancellationToken ct)
    {
        if (locationIds == null || locationIds.Length == 0)
            return new List<CropOptionDto>();
        if (toDate < fromDate) (fromDate, toDate) = (toDate, fromDate);

        // Get distinct (CropId, ItemDescription-of-CropId) for items used
        // on Delivery WSs in scope. CropId is Product.CropId — the parent
        // crop item id.
        var cropIds = await (
            from ws in _ctx.WeightSheets.AsNoTracking()
            where locationIds.Contains(ws.LocationId)
                  && ws.CreationDate >= fromDate
                  && ws.CreationDate <= toDate
                  && ws.WeightSheetType == "Delivery"
                  && ws.LotId != null
            let cropId = _ctx.Lots.Where(l => l.LotId == ws.LotId)
                .Join(_ctx.Items,    l => l.ItemId,    i => i.ItemId,    (l, i) => i)
                .Join(_ctx.Products, i => i.ProductId, p => p.ProductId, (i, p) => p.CropId)
                .FirstOrDefault()
            where cropId != null && cropId > 0
            select cropId.Value
        ).Distinct().ToListAsync(ct);

        if (cropIds.Count == 0) return new List<CropOptionDto>();

        var labels = await _ctx.Items.AsNoTracking()
            .Where(i => cropIds.Contains(i.ItemId))
            .Select(i => new { i.ItemId, i.Description })
            .ToListAsync(ct);

        return labels
            .Select(x => new CropOptionDto
            {
                CropId = x.ItemId,
                Description = x.Description ?? "",
            })
            .OrderBy(x => x.Description, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    public async Task<List<ProducerCommodityRowDto>> GetProducerCommodityAsync(
        int[] locationIds, long[] cropIds, string accountSearch,
        DateOnly fromDate, DateOnly toDate, CancellationToken ct)
    {
        if (locationIds == null || locationIds.Length == 0)
            return new List<ProducerCommodityRowDto>();
        if (toDate < fromDate) (fromDate, toDate) = (toDate, fromDate);
        var cropFilter = cropIds ?? Array.Empty<long>();
        var search = (accountSearch ?? "").Trim();

        // Pull location + district info up front so we can stamp district
        // names on each row without re-querying.
        var locInfo = await _ctx.Locations.AsNoTracking()
            .Where(l => locationIds.Contains(l.LocationId))
            .Select(l => new
            {
                l.LocationId,
                LocationName = l.Name ?? "",
                l.DistrictId,
                DistrictName = l.District != null ? l.District.Name : "",
            })
            .ToListAsync(ct);
        var locById = locInfo.ToDictionary(l => l.LocationId);

        var nonSeedCategoryCodes = new[] { "CHEM", "FERT", "PACK", "SERVICE" };

        var raw = await (
            from ws in _ctx.WeightSheets.AsNoTracking()
            where locationIds.Contains(ws.LocationId)
                  && ws.CreationDate >= fromDate
                  && ws.CreationDate <= toDate
                  && ws.WeightSheetType == "Delivery"
                  && ws.LotId != null
            let lotItemId = _ctx.Lots.Where(l => l.LotId == ws.LotId).Select(l => l.ItemId).FirstOrDefault()
            let prodInfo = lotItemId == null ? null : _ctx.Items.Where(i => i.ItemId == lotItemId)
                .Join(_ctx.Products, i => i.ProductId, p => p.ProductId, (i, p) => new
                {
                    p.ProductId,
                    p.CropId,
                    Crop = p.Description,
                    p.CategoryId,
                })
                .FirstOrDefault()
            select new
            {
                ws.WeightSheetId,
                ws.LocationId,
                LotId = ws.LotId!.Value,
                ItemId = lotItemId,
                ItemDescription = lotItemId == null
                    ? null
                    : _ctx.Items.Where(i => i.ItemId == lotItemId).Select(i => i.Description).FirstOrDefault(),
                CropId = prodInfo == null ? (long?)null : prodInfo.CropId,
                Crop = prodInfo == null ? "" : prodInfo.Crop,
                HasSeedTrait = lotItemId != null && _ctx.ItemTraits
                    .Any(t => t.ItemId == lotItemId && t.TraitId == 31),
                CategoryCode = prodInfo == null ? null : _ctx.Categories
                    .Where(c => c.CategoryId == prodInfo.CategoryId)
                    .Select(c => c.CategoryCode).FirstOrDefault(),
                UomCode = lotItemId == null ? null : _ctx.Items
                    .Where(i => i.ItemId == lotItemId)
                    .Join(_ctx.Products, i => i.ProductId, p => p.ProductId, (i, p) => p)
                    .Join(_ctx.Categories, p => p.CategoryId, c => c.CategoryId, (p, c) => c)
                    .Join(_ctx.UnitOfMeasures, c => c.DefaultUomId, u => (int?)u.UomId, (c, u) => u.Code)
                    .FirstOrDefault(),
                UomFactor = lotItemId == null ? (decimal?)null : _ctx.Items
                    .Where(i => i.ItemId == lotItemId)
                    .Join(_ctx.Products, i => i.ProductId, p => p.ProductId, (i, p) => p)
                    .Join(_ctx.Categories, p => p.CategoryId, c => c.CategoryId, (p, c) => c)
                    .Join(_ctx.UnitOfMeasures, c => c.DefaultUomId, u => (int?)u.UomId, (c, u) => (decimal?)u.ToBaseFactor)
                    .FirstOrDefault(),
                SplitGroupId = _ctx.Lots.Where(l => l.LotId == ws.LotId)
                    .Select(l => (int?)l.SplitGroupId).FirstOrDefault(),
                SplitGroupDescription = _ctx.Lots.Where(l => l.LotId == ws.LotId)
                    .Select(l => l.SplitGroup != null ? l.SplitGroup.SplitGroupDescription : null)
                    .FirstOrDefault(),
                PrimaryAccountId = _ctx.Lots.Where(l => l.LotId == ws.LotId)
                    .Select(l => l.SplitGroup != null ? (long?)l.SplitGroup.PrimaryAccountId : null)
                    .FirstOrDefault(),
                PrimaryAccountName = _ctx.Lots.Where(l => l.LotId == ws.LotId)
                    .Where(l => l.SplitGroup != null)
                    .Join(_ctx.Accounts,
                          l => l.SplitGroup!.PrimaryAccountId,
                          a => a.AccountId,
                          (l, a) => a.LookupName ?? a.EntityName)
                    .FirstOrDefault(),
                NetLbs = _ctx.InventoryTransactionDetails
                    .Where(itd => itd.RefId == ws.RowUid && itd.RefType == "WeightSheet" && !itd.IsVoided)
                    .Sum(itd => (decimal?)(itd.NetQty ?? itd.DirectQty ?? 0)) ?? 0m,
                LoadCount = _ctx.InventoryTransactionDetails
                    .Count(itd => itd.RefId == ws.RowUid && itd.RefType == "WeightSheet" && !itd.IsVoided),
            })
            .ToListAsync(ct);

        // Crop filter (applied client-side because the joined CropId is a
        // computed value rather than a column on WS).
        if (cropFilter.Length > 0)
        {
            var set = new HashSet<long>(cropFilter);
            raw = raw.Where(r => r.CropId.HasValue && set.Contains(r.CropId.Value)).ToList();
        }

        // Account search — matches against the primary account name, the
        // account id, the split group #, or the split group description.
        if (!string.IsNullOrEmpty(search))
        {
            raw = raw.Where(r =>
                (r.PrimaryAccountName ?? "").IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0
                || (r.PrimaryAccountId.HasValue && r.PrimaryAccountId.Value.ToString() == search)
                || (r.SplitGroupId.HasValue && r.SplitGroupId.Value.ToString() == search)
                || (r.SplitGroupDescription ?? "").IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0
            ).ToList();
        }

        // Group key: Location + PrimaryAccount + SplitGroup + Crop. For
        // seed items we also break out by ItemId so each variety gets
        // its own row (the report shows Variety + VarietyId).
        var keyed = new Dictionary<string, ProducerCommodityRowDto>();

        foreach (var r in raw)
        {
            bool isSeed = r.HasSeedTrait
                && (r.CategoryCode == null || !nonSeedCategoryCodes.Contains(r.CategoryCode));

            var district = locById.TryGetValue(r.LocationId, out var loc) ? loc : null;
            var districtName = district?.DistrictName ?? "";
            var districtId = district?.DistrictId ?? 0;
            var locationName = district?.LocationName ?? "";

            // Variety only matters for seed crops; otherwise lump all
            // items under the same crop key so each producer/crop gets a
            // single row.
            string varietyKey = isSeed ? (r.ItemId?.ToString() ?? "") : "";

            string key = string.Join("|",
                r.LocationId,
                r.PrimaryAccountId?.ToString() ?? "",
                r.SplitGroupId?.ToString() ?? "",
                r.CropId?.ToString() ?? "",
                varietyKey);

            if (!keyed.TryGetValue(key, out var row))
            {
                row = new ProducerCommodityRowDto
                {
                    DistrictId = districtId,
                    DistrictName = districtName,
                    LocationId = r.LocationId,
                    LocationName = locationName,
                    PrimaryAccountId = r.PrimaryAccountId?.ToString() ?? "",
                    PrimaryAccountName = r.PrimaryAccountName ?? "",
                    SplitGroupId = r.SplitGroupId?.ToString() ?? "",
                    SplitGroupDescription = r.SplitGroupDescription ?? "",
                    Crop = r.Crop ?? "",
                    CropId = r.CropId?.ToString() ?? "",
                    IsSeed = isSeed,
                    Variety = isSeed ? (r.ItemDescription ?? "") : "",
                    VarietyId = isSeed ? (r.ItemId?.ToString() ?? "") : "",
                    PrimaryUom = string.IsNullOrEmpty(r.UomCode) ? "lbs" : r.UomCode,
                };
                keyed[key] = row;
            }

            row.NetLbs += r.NetLbs;
            row.LoadCount += r.LoadCount;
            // Late-bind the UoM if a later row resolves one when an
            // earlier WS for the same crop didn't.
            if (string.IsNullOrEmpty(row.PrimaryUom) || row.PrimaryUom == "lbs")
            {
                if (!string.IsNullOrEmpty(r.UomCode))
                    row.PrimaryUom = r.UomCode;
            }
            // Track the latest non-zero UomFactor we saw for this row;
            // we'll resolve NetUnits once after the loop.
            row.NetUnits = (r.UomFactor.HasValue && r.UomFactor.Value > 0)
                ? Math.Round(row.NetLbs / r.UomFactor.Value, 2)
                : row.NetLbs;
        }

        return keyed.Values
            .OrderBy(r => r.DistrictName, StringComparer.OrdinalIgnoreCase)
            .ThenBy(r => r.LocationName, StringComparer.OrdinalIgnoreCase)
            .ThenBy(r => r.PrimaryAccountName, StringComparer.OrdinalIgnoreCase)
            .ThenBy(r => r.Crop, StringComparer.OrdinalIgnoreCase)
            .ThenBy(r => r.Variety, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    // ── Producer Delivery (intake only) ────────────────────────────────
    // Same audience as ProducerCommodity but grouped just by producer
    // (or by split group). No crop / variety / UoM breakouts.
    public async Task<List<ProducerDeliveryRowDto>> GetProducerDeliveryAsync(
        int[] locationIds, long[] cropIds, string accountSearch,
        bool groupBySplit, DateOnly fromDate, DateOnly toDate, CancellationToken ct)
    {
        if (locationIds == null || locationIds.Length == 0)
            return new List<ProducerDeliveryRowDto>();
        if (toDate < fromDate) (fromDate, toDate) = (toDate, fromDate);
        var cropFilter = cropIds ?? Array.Empty<long>();
        var search = (accountSearch ?? "").Trim();

        var locInfo = await _ctx.Locations.AsNoTracking()
            .Where(l => locationIds.Contains(l.LocationId))
            .Select(l => new
            {
                l.LocationId,
                LocationName = l.Name ?? "",
                l.DistrictId,
                DistrictName = l.District != null ? l.District.Name : "",
            })
            .ToListAsync(ct);
        var locById = locInfo.ToDictionary(l => l.LocationId);

        var raw = await (
            from ws in _ctx.WeightSheets.AsNoTracking()
            where locationIds.Contains(ws.LocationId)
                  && ws.CreationDate >= fromDate
                  && ws.CreationDate <= toDate
                  && ws.WeightSheetType == "Delivery"
                  && ws.LotId != null
            let lotItemId = _ctx.Lots.Where(l => l.LotId == ws.LotId).Select(l => l.ItemId).FirstOrDefault()
            select new
            {
                ws.LocationId,
                CropId = lotItemId == null ? (long?)null
                    : _ctx.Items.Where(i => i.ItemId == lotItemId)
                        .Join(_ctx.Products, i => i.ProductId, p => p.ProductId, (i, p) => p.CropId)
                        .FirstOrDefault(),
                SplitGroupId = _ctx.Lots.Where(l => l.LotId == ws.LotId)
                    .Select(l => (int?)l.SplitGroupId).FirstOrDefault(),
                SplitGroupDescription = _ctx.Lots.Where(l => l.LotId == ws.LotId)
                    .Select(l => l.SplitGroup != null ? l.SplitGroup.SplitGroupDescription : null)
                    .FirstOrDefault(),
                PrimaryAccountId = _ctx.Lots.Where(l => l.LotId == ws.LotId)
                    .Select(l => l.SplitGroup != null ? (long?)l.SplitGroup.PrimaryAccountId : null)
                    .FirstOrDefault(),
                PrimaryAccountName = _ctx.Lots.Where(l => l.LotId == ws.LotId)
                    .Where(l => l.SplitGroup != null)
                    .Join(_ctx.Accounts,
                          l => l.SplitGroup!.PrimaryAccountId,
                          a => a.AccountId,
                          (l, a) => a.LookupName ?? a.EntityName)
                    .FirstOrDefault(),
                NetLbs = _ctx.InventoryTransactionDetails
                    .Where(itd => itd.RefId == ws.RowUid && itd.RefType == "WeightSheet" && !itd.IsVoided)
                    .Sum(itd => (decimal?)(itd.NetQty ?? itd.DirectQty ?? 0)) ?? 0m,
                LoadCount = _ctx.InventoryTransactionDetails
                    .Count(itd => itd.RefId == ws.RowUid && itd.RefType == "WeightSheet" && !itd.IsVoided),
            })
            .ToListAsync(ct);

        if (cropFilter.Length > 0)
        {
            var set = new HashSet<long>(cropFilter);
            raw = raw.Where(r => r.CropId.HasValue && set.Contains(r.CropId.Value)).ToList();
        }
        if (!string.IsNullOrEmpty(search))
        {
            raw = raw.Where(r =>
                (r.PrimaryAccountName ?? "").IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0
                || (r.PrimaryAccountId.HasValue && r.PrimaryAccountId.Value.ToString() == search)
                || (r.SplitGroupId.HasValue && r.SplitGroupId.Value.ToString() == search)
                || (r.SplitGroupDescription ?? "").IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0
            ).ToList();
        }

        var keyed = new Dictionary<string, ProducerDeliveryRowDto>();
        foreach (var r in raw)
        {
            var loc = locById.TryGetValue(r.LocationId, out var li) ? li : null;
            string key = groupBySplit
                ? r.LocationId + "|S|" + (r.SplitGroupId?.ToString() ?? "")
                : r.LocationId + "|P|" + (r.PrimaryAccountId?.ToString() ?? "");

            if (!keyed.TryGetValue(key, out var row))
            {
                row = new ProducerDeliveryRowDto
                {
                    DistrictId = loc?.DistrictId ?? 0,
                    DistrictName = loc?.DistrictName ?? "",
                    LocationId = r.LocationId,
                    LocationName = loc?.LocationName ?? "",
                    PrimaryAccountId = r.PrimaryAccountId?.ToString() ?? "",
                    PrimaryAccountName = r.PrimaryAccountName ?? "",
                    SplitGroupId = groupBySplit ? (r.SplitGroupId?.ToString() ?? "") : "",
                    SplitGroupDescription = groupBySplit ? (r.SplitGroupDescription ?? "") : "",
                };
                keyed[key] = row;
            }
            row.LoadCount += r.LoadCount;
            row.NetLbs += r.NetLbs;
        }

        return keyed.Values
            .OrderBy(r => r.DistrictName, StringComparer.OrdinalIgnoreCase)
            .ThenBy(r => r.LocationName, StringComparer.OrdinalIgnoreCase)
            .ThenBy(r => r.PrimaryAccountName, StringComparer.OrdinalIgnoreCase)
            .ThenBy(r => r.SplitGroupDescription, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    // ── Intake Location (intake only, with UoM conversion) ─────────────
    public async Task<List<IntakeLocationRowDto>> GetIntakeLocationAsync(
        int[] locationIds, long[] cropIds,
        bool groupBySplit, DateOnly fromDate, DateOnly toDate, CancellationToken ct)
    {
        if (locationIds == null || locationIds.Length == 0)
            return new List<IntakeLocationRowDto>();
        if (toDate < fromDate) (fromDate, toDate) = (toDate, fromDate);
        var cropFilter = cropIds ?? Array.Empty<long>();

        var locInfo = await _ctx.Locations.AsNoTracking()
            .Where(l => locationIds.Contains(l.LocationId))
            .Select(l => new
            {
                l.LocationId,
                LocationName = l.Name ?? "",
                l.DistrictId,
                DistrictName = l.District != null ? l.District.Name : "",
            })
            .ToListAsync(ct);
        var locById = locInfo.ToDictionary(l => l.LocationId);

        var raw = await (
            from ws in _ctx.WeightSheets.AsNoTracking()
            where locationIds.Contains(ws.LocationId)
                  && ws.CreationDate >= fromDate
                  && ws.CreationDate <= toDate
                  && ws.WeightSheetType == "Delivery"
                  && ws.LotId != null
            let lotItemId = _ctx.Lots.Where(l => l.LotId == ws.LotId).Select(l => l.ItemId).FirstOrDefault()
            select new
            {
                ws.LocationId,
                CropId = lotItemId == null ? (long?)null
                    : _ctx.Items.Where(i => i.ItemId == lotItemId)
                        .Join(_ctx.Products, i => i.ProductId, p => p.ProductId, (i, p) => p.CropId)
                        .FirstOrDefault(),
                UomCode = lotItemId == null ? null : _ctx.Items
                    .Where(i => i.ItemId == lotItemId)
                    .Join(_ctx.Products, i => i.ProductId, p => p.ProductId, (i, p) => p)
                    .Join(_ctx.Categories, p => p.CategoryId, c => c.CategoryId, (p, c) => c)
                    .Join(_ctx.UnitOfMeasures, c => c.DefaultUomId, u => (int?)u.UomId, (c, u) => u.Code)
                    .FirstOrDefault(),
                UomFactor = lotItemId == null ? (decimal?)null : _ctx.Items
                    .Where(i => i.ItemId == lotItemId)
                    .Join(_ctx.Products, i => i.ProductId, p => p.ProductId, (i, p) => p)
                    .Join(_ctx.Categories, p => p.CategoryId, c => c.CategoryId, (p, c) => c)
                    .Join(_ctx.UnitOfMeasures, c => c.DefaultUomId, u => (int?)u.UomId, (c, u) => (decimal?)u.ToBaseFactor)
                    .FirstOrDefault(),
                SplitGroupId = _ctx.Lots.Where(l => l.LotId == ws.LotId)
                    .Select(l => (int?)l.SplitGroupId).FirstOrDefault(),
                SplitGroupDescription = _ctx.Lots.Where(l => l.LotId == ws.LotId)
                    .Select(l => l.SplitGroup != null ? l.SplitGroup.SplitGroupDescription : null)
                    .FirstOrDefault(),
                PrimaryAccountId = _ctx.Lots.Where(l => l.LotId == ws.LotId)
                    .Select(l => l.SplitGroup != null ? (long?)l.SplitGroup.PrimaryAccountId : null)
                    .FirstOrDefault(),
                PrimaryAccountName = _ctx.Lots.Where(l => l.LotId == ws.LotId)
                    .Where(l => l.SplitGroup != null)
                    .Join(_ctx.Accounts,
                          l => l.SplitGroup!.PrimaryAccountId,
                          a => a.AccountId,
                          (l, a) => a.LookupName ?? a.EntityName)
                    .FirstOrDefault(),
                NetLbs = _ctx.InventoryTransactionDetails
                    .Where(itd => itd.RefId == ws.RowUid && itd.RefType == "WeightSheet" && !itd.IsVoided)
                    .Sum(itd => (decimal?)(itd.NetQty ?? itd.DirectQty ?? 0)) ?? 0m,
                LoadCount = _ctx.InventoryTransactionDetails
                    .Count(itd => itd.RefId == ws.RowUid && itd.RefType == "WeightSheet" && !itd.IsVoided),
            })
            .ToListAsync(ct);

        if (cropFilter.Length > 0)
        {
            var set = new HashSet<long>(cropFilter);
            raw = raw.Where(r => r.CropId.HasValue && set.Contains(r.CropId.Value)).ToList();
        }

        var keyed = new Dictionary<string, IntakeLocationRowDto>();
        // Track sumLbs and sumLbsConverted separately so the final
        // NetUnits is internally consistent across loads with different
        // ToBaseFactors (mixing crops in one row).
        var unitsAccum = new Dictionary<string, decimal>();

        foreach (var r in raw)
        {
            var loc = locById.TryGetValue(r.LocationId, out var li) ? li : null;
            string key = groupBySplit
                ? r.LocationId + "|S|" + (r.SplitGroupId?.ToString() ?? "")
                : r.LocationId + "|P|" + (r.PrimaryAccountId?.ToString() ?? "");

            if (!keyed.TryGetValue(key, out var row))
            {
                row = new IntakeLocationRowDto
                {
                    DistrictId = loc?.DistrictId ?? 0,
                    DistrictName = loc?.DistrictName ?? "",
                    LocationId = r.LocationId,
                    LocationName = loc?.LocationName ?? "",
                    PrimaryAccountId = r.PrimaryAccountId?.ToString() ?? "",
                    PrimaryAccountName = r.PrimaryAccountName ?? "",
                    SplitGroupId = groupBySplit ? (r.SplitGroupId?.ToString() ?? "") : "",
                    SplitGroupDescription = groupBySplit ? (r.SplitGroupDescription ?? "") : "",
                    PrimaryUom = string.IsNullOrEmpty(r.UomCode) ? "" : r.UomCode,
                };
                keyed[key] = row;
                unitsAccum[key] = 0m;
            }
            row.LoadCount += r.LoadCount;
            row.NetLbs += r.NetLbs;

            if (string.IsNullOrEmpty(row.PrimaryUom) && !string.IsNullOrEmpty(r.UomCode))
                row.PrimaryUom = r.UomCode;

            // Per-load conversion accumulates correctly even when the
            // group spans multiple crops with different factors.
            decimal converted = (r.UomFactor.HasValue && r.UomFactor.Value > 0)
                ? r.NetLbs / r.UomFactor.Value
                : r.NetLbs;
            unitsAccum[key] += converted;
        }

        foreach (var kv in unitsAccum)
        {
            keyed[kv.Key].NetUnits = Math.Round(kv.Value, 2);
        }

        return keyed.Values
            .OrderBy(r => r.DistrictName, StringComparer.OrdinalIgnoreCase)
            .ThenBy(r => r.LocationName, StringComparer.OrdinalIgnoreCase)
            .ThenBy(r => r.PrimaryAccountName, StringComparer.OrdinalIgnoreCase)
            .ThenBy(r => r.SplitGroupDescription, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    // ── Load Dump Type ────────────────────────────────────────────────
    // One row per load that has an IS_END_DUMP transaction attribute
    // (joined back to its parent transaction → location). Filtered by
    // the WS CreationDate so an operator can pull a date range out of
    // history. Voided loads are dropped — they no longer count.
    public async Task<List<LoadDumpTypeRowDto>> GetLoadDumpTypesAsync(
        DateOnly fromDate, DateOnly toDate, CancellationToken ct)
    {
        if (toDate < fromDate) (fromDate, toDate) = (toDate, fromDate);

        var rows = await (
            from a in _ctx.TransactionAttributes.AsNoTracking()
            where a.AttributeType.Code == TransactionAttributeCodes.IsEndDump
                  && a.BoolValue != null
            join itd in _ctx.InventoryTransactionDetails.AsNoTracking()
                on a.TransactionId equals itd.TransactionId
            where !itd.IsVoided && itd.RefType == "WeightSheet" && itd.RefId != null
            join ws in _ctx.WeightSheets.AsNoTracking()
                on itd.RefId.Value equals ws.RowUid
            where ws.CreationDate >= fromDate && ws.CreationDate <= toDate
            select new
            {
                LoadId = itd.TransactionId,
                ws.LocationId,
                LocationName = ws.Location != null ? (ws.Location.Name ?? "") : "",
                TimeIn = itd.StartedAt ?? itd.TxnAt,
                IsEndDump = a.BoolValue!.Value,
            })
            .ToListAsync(ct);

        return rows
            .Select(r => new LoadDumpTypeRowDto
            {
                LoadId = r.LoadId,
                LocationId = r.LocationId,
                LocationName = r.LocationName,
                TimeIn = r.TimeIn,
                IsEndDump = r.IsEndDump,
                DumpType = r.IsEndDump ? "End Dump" : "Belly Dump",
            })
            .OrderBy(r => r.TimeIn ?? DateTime.MinValue)
            .ThenBy(r => r.LocationName, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }
}
