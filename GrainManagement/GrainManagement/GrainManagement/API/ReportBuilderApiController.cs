using DevExpress.XtraReports.UI;
using GrainManagement.Models;
using GrainManagement.Reporting;
using GrainManagement.Services.Warehouse;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrainManagement.API;

/// <summary>
/// Backs the on-demand Reports views — Daily Intake/Transfer picker,
/// Daily Weight Sheet Series picker, and the Commodities By Date Range
/// table. Lookups (districts, warehouse-enabled locations) mirror the
/// AgvantageWarehouseTransfer endpoints; candidate lists and the
/// commodities aggregation come from <see cref="IReportBuilderService"/>.
/// PDF endpoints render the existing XtraReports for a chosen
/// (location, day) without going through the EOD finalize path.
/// </summary>
[ApiController]
[Route("api/ReportBuilder")]
public sealed class ReportBuilderApiController : ControllerBase
{
    private readonly dbContext _ctx;
    private readonly IReportBuilderService _builder;

    public ReportBuilderApiController(dbContext ctx, IReportBuilderService builder)
    {
        _ctx = ctx;
        _builder = builder;
    }

    // ── Lookups ────────────────────────────────────────────────────────
    // Both endpoints intentionally do NOT filter on UseForWarehouse — that
    // flag gates the AS400 warehouse-transfer upload, not historical
    // reports. A location like Clyde can be a transfer source/destination
    // without carrying the flag, and reports must still surface its rows.
    [HttpGet("Districts")]
    public async Task<IActionResult> Districts(CancellationToken ct)
    {
        var rows = await _ctx.LocationDistricts.AsNoTracking()
            .Where(d => d.IsActive && d.Locations.Any(l => l.IsActive))
            .OrderBy(d => d.Name)
            .Select(d => new { d.DistrictId, d.Name })
            .ToListAsync(ct);
        return Ok(rows);
    }

    [HttpGet("Locations")]
    public async Task<IActionResult> Locations([FromQuery] int districtId, CancellationToken ct)
    {
        // districtId == 0 → every active location across all districts
        // (the "All" option in the district picker).
        var query = _ctx.Locations.AsNoTracking()
            .Where(l => l.IsActive);
        if (districtId > 0)
            query = query.Where(l => l.DistrictId == districtId);

        var rows = await query
            .OrderBy(l => l.Name)
            .Select(l => new { l.LocationId, l.Name, l.Code })
            .ToListAsync(ct);
        return Ok(rows);
    }

    // ── Picker candidate lists ─────────────────────────────────────────
    public sealed class SummaryRequestDto
    {
        public DateOnly From { get; set; }
        public DateOnly To { get; set; }
        public int[] LocationIds { get; set; } = Array.Empty<int>();
        public bool IncludeIntake { get; set; } = true;
        public bool IncludeTransfer { get; set; } = true;
    }

    [HttpPost("SummaryCandidates")]
    public async Task<IActionResult> SummaryCandidates(
        [FromBody] SummaryRequestDto req, CancellationToken ct)
    {
        if (req is null) return BadRequest(new { message = "Request body required." });
        var rows = await _builder.GetSummaryCandidatesAsync(
            req.LocationIds ?? Array.Empty<int>(),
            req.From, req.To,
            req.IncludeIntake, req.IncludeTransfer, ct);

        return Ok(rows.Select(r => new
        {
            r.LocationId,
            r.LocationName,
            Date = r.Date.ToString("yyyy-MM-dd"),
            DateDisplay = r.Date.ToString("MM/dd/yyyy"),
            r.Type,
            r.LoadCount,
        }));
    }

    public sealed class SeriesRequestDto
    {
        public DateOnly From { get; set; }
        public DateOnly To { get; set; }
        public int[] LocationIds { get; set; } = Array.Empty<int>();
    }

    [HttpPost("SeriesCandidates")]
    public async Task<IActionResult> SeriesCandidates(
        [FromBody] SeriesRequestDto req, CancellationToken ct)
    {
        if (req is null) return BadRequest(new { message = "Request body required." });
        var rows = await _builder.GetSeriesCandidatesAsync(
            req.LocationIds ?? Array.Empty<int>(), req.From, req.To, ct);

        return Ok(rows.Select(r => new
        {
            r.LocationId,
            r.LocationName,
            Date = r.Date.ToString("yyyy-MM-dd"),
            DateDisplay = r.Date.ToString("MM/dd/yyyy"),
            r.WeightSheetCount,
            r.LoadCount,
        }));
    }

    [HttpPost("Commodities")]
    public async Task<IActionResult> Commodities(
        [FromBody] SeriesRequestDto req, CancellationToken ct)
    {
        if (req is null) return BadRequest(new { message = "Request body required." });
        var rows = await _builder.GetCommoditiesByDateRangeAsync(
            req.LocationIds ?? Array.Empty<int>(), req.From, req.To, ct);
        return Ok(rows);
    }

    [HttpPost("BinProtein")]
    public async Task<IActionResult> BinProtein(
        [FromBody] SeriesRequestDto req, CancellationToken ct)
    {
        if (req is null) return BadRequest(new { message = "Request body required." });
        var rows = await _builder.GetBinProteinAsync(
            req.LocationIds ?? Array.Empty<int>(), req.From, req.To, ct);
        return Ok(rows);
    }

    [HttpPost("DailyLoadTimes")]
    public async Task<IActionResult> DailyLoadTimes(
        [FromBody] SeriesRequestDto req, CancellationToken ct)
    {
        if (req is null) return BadRequest(new { message = "Request body required." });
        var rows = await _builder.GetDailyLoadTimesAsync(
            req.LocationIds ?? Array.Empty<int>(), req.From, req.To, ct);
        return Ok(rows);
    }

    [HttpPost("PeakHours")]
    public async Task<IActionResult> PeakHours(
        [FromBody] SeriesRequestDto req, CancellationToken ct)
    {
        if (req is null) return BadRequest(new { message = "Request body required." });
        var rows = await _builder.GetPeakHoursAsync(
            req.LocationIds ?? Array.Empty<int>(), req.From, req.To, ct);
        return Ok(rows);
    }

    [HttpPost("IntakeCrops")]
    public async Task<IActionResult> IntakeCrops(
        [FromBody] SeriesRequestDto req, CancellationToken ct)
    {
        if (req is null) return BadRequest(new { message = "Request body required." });
        var rows = await _builder.GetIntakeCropsAsync(
            req.LocationIds ?? Array.Empty<int>(), req.From, req.To, ct);
        return Ok(rows);
    }

    public class ProducerCommodityRequestDto
    {
        public DateOnly From { get; set; }
        public DateOnly To { get; set; }
        public int[] LocationIds { get; set; } = Array.Empty<int>();
        public long[] CropIds { get; set; } = Array.Empty<long>();
        public string AccountSearch { get; set; } = "";
    }

    [HttpPost("ProducerCommodity")]
    public async Task<IActionResult> ProducerCommodity(
        [FromBody] ProducerCommodityRequestDto req, CancellationToken ct)
    {
        if (req is null) return BadRequest(new { message = "Request body required." });
        var rows = await _builder.GetProducerCommodityAsync(
            req.LocationIds ?? Array.Empty<int>(),
            req.CropIds ?? Array.Empty<long>(),
            req.AccountSearch ?? "",
            req.From, req.To, ct);
        return Ok(rows);
    }

    public sealed class ProducerDeliveryRequestDto : ProducerCommodityRequestDto
    {
        /// <summary>True → group by split group; false → group by primary account.</summary>
        public bool GroupBySplit { get; set; }
    }

    [HttpPost("ProducerDelivery")]
    public async Task<IActionResult> ProducerDelivery(
        [FromBody] ProducerDeliveryRequestDto req, CancellationToken ct)
    {
        if (req is null) return BadRequest(new { message = "Request body required." });
        var rows = await _builder.GetProducerDeliveryAsync(
            req.LocationIds ?? Array.Empty<int>(),
            req.CropIds ?? Array.Empty<long>(),
            req.AccountSearch ?? "",
            req.GroupBySplit,
            req.From, req.To, ct);
        return Ok(rows);
    }

    public sealed class IntakeLocationRequestDto
    {
        public DateOnly From { get; set; }
        public DateOnly To { get; set; }
        public int[] LocationIds { get; set; } = Array.Empty<int>();
        public long[] CropIds { get; set; } = Array.Empty<long>();
        public bool GroupBySplit { get; set; }
    }

    [HttpPost("IntakeLocation")]
    public async Task<IActionResult> IntakeLocation(
        [FromBody] IntakeLocationRequestDto req, CancellationToken ct)
    {
        if (req is null) return BadRequest(new { message = "Request body required." });
        var rows = await _builder.GetIntakeLocationAsync(
            req.LocationIds ?? Array.Empty<int>(),
            req.CropIds ?? Array.Empty<long>(),
            req.GroupBySplit,
            req.From, req.To, ct);
        return Ok(rows);
    }

    // ── PDF endpoints — the iframe modals load these directly ──────────
    [HttpGet("Pdf/Intake")]
    public async Task<IActionResult> PdfIntake(
        [FromQuery] int locationId, [FromQuery] DateOnly day, CancellationToken ct)
    {
        if (locationId <= 0) return BadRequest(new { message = "locationId is required." });
        var dto = await _builder.BuildDailyIntakeForDateAsync(locationId, day, ct);
        return RenderPdf(new DailyIntakeReport { DataSource = new[] { dto } },
                         $"DailyIntake_{Sanitize(dto.LocationName)}_{day:yyyy-MM-dd}.pdf");
    }

    [HttpGet("Pdf/Transfer")]
    public async Task<IActionResult> PdfTransfer(
        [FromQuery] int locationId, [FromQuery] DateOnly day, CancellationToken ct)
    {
        if (locationId <= 0) return BadRequest(new { message = "locationId is required." });
        var dto = await _builder.BuildDailyTransferForDateAsync(locationId, day, ct);
        return RenderPdf(new DailyTransferReport { DataSource = new[] { dto } },
                         $"DailyTransfer_{Sanitize(dto.LocationName)}_{day:yyyy-MM-dd}.pdf");
    }

    [HttpGet("Pdf/Series")]
    public async Task<IActionResult> PdfSeries(
        [FromQuery] int locationId, [FromQuery] DateOnly day, CancellationToken ct)
    {
        if (locationId <= 0) return BadRequest(new { message = "locationId is required." });
        var dto = await _builder.BuildWsSeriesAsync(locationId, day, ct);
        return RenderPdf(new DailyWeightSheetSeriesReport { DataSource = new[] { dto } },
                         $"DailyWeightSheetSeries_{Sanitize(dto.LocationName)}_{day:yyyy-MM-dd}.pdf");
    }

    private FileContentResult RenderPdf(XtraReport report, string fileName)
    {
        report.CreateDocument();
        using var ms = new MemoryStream();
        report.ExportToPdf(ms);
        Response.Headers["Content-Disposition"] = $"inline; filename=\"{fileName}\"";
        return File(ms.ToArray(), "application/pdf");
    }

    private static string Sanitize(string s)
    {
        if (string.IsNullOrWhiteSpace(s)) return "report";
        return new string(s.Select(c => "\"\\/<>|:*?\r\n ".IndexOf(c) >= 0 ? '_' : c).ToArray());
    }
}
