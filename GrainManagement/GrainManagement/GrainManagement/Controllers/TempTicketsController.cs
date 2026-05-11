using GrainManagement.Dtos.TempTickets;
using GrainManagement.Models;
using GrainManagement.Services;
using GrainManagement.Services.TempTickets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace GrainManagement.Controllers;

/// <summary>
/// REST surface for the temp-weight-ticket feature.
///
/// Endpoints:
///   POST  /api/temp-tickets/press         — kiosk Pi → web; queues a press
///   GET   /api/temp-tickets/today         — load-create picker (server + scale scoped)
///   GET   /api/temp-tickets               — management view (server scoped, all)
///   GET   /api/temp-tickets/{id}/image    — serves the captured image
///   DELETE /api/temp-tickets/{id}         — manual delete (management view)
///   POST  /api/temp-tickets/{id}/consume  — mark consumed by a Lot; load-save calls this
/// </summary>
[ApiController]
[Route("api/temp-tickets")]
public sealed class TempTicketsController : ControllerBase
{
    private readonly dbContext _db;
    private readonly IServerInfoProvider _serverInfo;
    private readonly ITempTicketOrchestrator _orchestrator;
    private readonly IConfiguration _config;
    private readonly ILogger<TempTicketsController> _log;

    public TempTicketsController(
        dbContext db,
        IServerInfoProvider serverInfo,
        ITempTicketOrchestrator orchestrator,
        IConfiguration config,
        ILogger<TempTicketsController> log)
    {
        _db = db;
        _serverInfo = serverInfo;
        _orchestrator = orchestrator;
        _config = config;
        _log = log;
    }

    // ── Kiosk Pi service → web ────────────────────────────────────────
    [HttpPost("press")]
    public IActionResult Press([FromBody] TempTicketPressRequest req)
    {
        if (req is null) return BadRequest(new { message = "Request body required." });
        var result = _orchestrator.Enqueue(req);
        if (result.Status == "rejected")
            return BadRequest(result);
        return Accepted(result);
    }

    // ── Load-create picker — "what temp tickets are available right now?" ──
    // scaleId is optional. When supplied, only that scale's tickets come back
    // (the natural filter for an intake that's bound to a specific scale);
    // when omitted, every unconsumed ticket this server captured today is
    // returned so the operator can pick across scales.
    [HttpGet("today")]
    public async Task<IActionResult> GetToday(int? scaleId, CancellationToken ct)
    {
        var server = await _serverInfo.GetAsync(ct);
        if (server is null) return Ok(Array.Empty<TempTicketDto>());

        var floorUtc = PacificMidnightUtc(DateTime.UtcNow);

        var q = _db.Set<TempWeightTicket>().AsNoTracking()
            .Where(t => t.ServerId == server.ServerId
                     && t.ConsumedByLotId == null
                     && t.CreatedAt >= floorUtc);

        if (scaleId.HasValue && scaleId.Value > 0)
            q = q.Where(t => t.ScaleId == scaleId.Value);

        var rows = await q
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new TempTicketDto
            {
                TempTicketId = t.TempTicketId,
                ServerId     = t.ServerId,
                ScaleId      = t.ScaleId,
                KioskId      = t.KioskId,
                Gross        = t.Gross,
                Tare         = t.Tare,
                Net          = t.Net,
                Units        = t.Units,
                ImagePath    = t.ImagePath,
                CreatedAt    = t.CreatedAt,
                CompletedAt  = t.CompletedAt,
            })
            .ToListAsync(ct);

        return Ok(rows);
    }

    // ── "Is the temp-ticket feature actually being used on this server?"
    // The Remote dashboard hides its Temp Tickets card unless this returns
    // true. Active = at least one row for this ServerId in the last 30 days
    // (consumed or not). Once a kiosk presses for the first time the card
    // appears and stays through the purge horizon.
    [HttpGet("feature-active")]
    public async Task<IActionResult> FeatureActive(CancellationToken ct)
    {
        var server = await _serverInfo.GetAsync(ct);
        if (server is null) return Ok(new { active = false });

        var horizon = DateTime.UtcNow - TimeSpan.FromDays(30);
        var active = await _db.Set<TempWeightTicket>().AsNoTracking()
            .AnyAsync(t => t.ServerId == server.ServerId && t.CreatedAt >= horizon, ct);

        return Ok(new { active });
    }

    // ── Management view — all unconsumed tickets on this server ───────
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var server = await _serverInfo.GetAsync(ct);
        if (server is null) return Ok(Array.Empty<TempTicketDto>());

        var rows = await _db.Set<TempWeightTicket>().AsNoTracking()
            .Where(t => t.ServerId == server.ServerId && t.ConsumedByLotId == null)
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new TempTicketDto
            {
                TempTicketId = t.TempTicketId,
                ServerId     = t.ServerId,
                ScaleId      = t.ScaleId,
                KioskId      = t.KioskId,
                Gross        = t.Gross,
                Tare         = t.Tare,
                Net          = t.Net,
                Units        = t.Units,
                ImagePath    = t.ImagePath,
                CreatedAt    = t.CreatedAt,
                CompletedAt  = t.CompletedAt,
            })
            .ToListAsync(ct);

        return Ok(rows);
    }

    // ── Image serving — used by thumbnails + the expanded view ────────
    [HttpGet("{id:long}/image")]
    public async Task<IActionResult> GetImage(long id, CancellationToken ct)
    {
        var row = await _db.Set<TempWeightTicket>().AsNoTracking()
            .FirstOrDefaultAsync(t => t.TempTicketId == id, ct);
        if (row is null || string.IsNullOrWhiteSpace(row.ImagePath))
            return NotFound();

        var imagesRoot = _config["TicketImages:PhysicalPath"];
        if (string.IsNullOrWhiteSpace(imagesRoot)) return NotFound();

        var full = Path.Combine(imagesRoot, row.ImagePath);
        if (!System.IO.File.Exists(full)) return NotFound();

        var ext = Path.GetExtension(full).ToLowerInvariant();
        var ctype = ext switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            _ => "application/octet-stream",
        };
        return PhysicalFile(full, ctype);
    }

    // ── Manual delete (management view) ───────────────────────────────
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id, CancellationToken ct)
    {
        var row = await _db.Set<TempWeightTicket>()
            .FirstOrDefaultAsync(t => t.TempTicketId == id, ct);
        if (row is null) return NotFound();

        DeleteImageFile(row.ImagePath);

        _db.Set<TempWeightTicket>().Remove(row);
        await _db.SaveChangesAsync(ct);

        return NoContent();
    }

    // ── "Use This Ticket" → load save consumes the temp row ───────────
    [HttpPost("{id:long}/consume")]
    public async Task<IActionResult> Consume(long id, [FromBody] TempTicketConsumeRequest req, CancellationToken ct)
    {
        if (req is null || req.LotId <= 0)
            return BadRequest(new { message = "LotId is required." });

        var row = await _db.Set<TempWeightTicket>()
            .FirstOrDefaultAsync(t => t.TempTicketId == id, ct);
        if (row is null) return NotFound();

        row.ConsumedByLotId = req.LotId;
        await _db.SaveChangesAsync(ct);

        // Per spec: once the lot is saved, the temp ticket + its image
        // are deleted. The consume endpoint runs after the lot is
        // persisted, so it's safe to nuke them here.
        DeleteImageFile(row.ImagePath);
        _db.Set<TempWeightTicket>().Remove(row);
        await _db.SaveChangesAsync(ct);

        return NoContent();
    }

    private void DeleteImageFile(string? relPath)
    {
        if (string.IsNullOrWhiteSpace(relPath)) return;
        var imagesRoot = _config["TicketImages:PhysicalPath"];
        if (string.IsNullOrWhiteSpace(imagesRoot)) return;
        var full = Path.Combine(imagesRoot, relPath);
        try { if (System.IO.File.Exists(full)) System.IO.File.Delete(full); }
        catch (Exception ex)
        {
            _log.LogWarning(ex, "Failed to delete temp ticket image at {Path}.", full);
        }
    }

    /// <summary>
    /// Midnight Pacific Time, expressed in UTC. The "today" filter for
    /// the picker is naturally a wall-clock day at the location, not a
    /// rolling 24h window from UTC midnight.
    /// </summary>
    private static DateTime PacificMidnightUtc(DateTime nowUtc)
    {
        TimeZoneInfo pt;
        try   { pt = TimeZoneInfo.FindSystemTimeZoneById("America/Los_Angeles"); }
        catch { pt = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"); }
        var nowPt = TimeZoneInfo.ConvertTimeFromUtc(nowUtc, pt);
        var midnightPt = new DateTime(nowPt.Year, nowPt.Month, nowPt.Day, 0, 0, 0, DateTimeKind.Unspecified);
        return TimeZoneInfo.ConvertTimeToUtc(midnightPt, pt);
    }
}

public sealed class TempTicketConsumeRequest
{
    public long LotId { get; set; }
}
