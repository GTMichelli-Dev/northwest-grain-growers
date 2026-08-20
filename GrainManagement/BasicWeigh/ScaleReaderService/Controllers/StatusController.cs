using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScaleReaderService.Data;
using ScaleReaderService.Models;
using ScaleReaderService.Services;

namespace ScaleReaderService.Controllers;

[ApiController]
public class StatusController : Controller
{
    private readonly ScaleDbContext _db;
    private readonly ScaleWeightStore _weightStore;

    public StatusController(ScaleDbContext db, ScaleWeightStore weightStore)
    {
        _db = db;
        _weightStore = weightStore;
    }

    // ===== HEALTH =====

    [HttpGet("api/status/health")]
    public IActionResult Health()
    {
        var scaleCount = _db.Scales.Count(s => s.Active);
        return Ok(new { status = "ok", activeScales = scaleCount });
    }

    // ===== LIVE WEIGHT =====

    /// <summary>
    /// Get the latest weight reading from a specific scale.
    /// </summary>
    [HttpGet("api/weight/{scaleId}")]
    public IActionResult GetWeight(string scaleId)
    {
        var reading = _weightStore.Get(scaleId);
        if (reading == null)
            return NotFound(new { error = $"No reading for scale '{scaleId}'. Scale may not be active or hasn't been polled yet." });
        return Ok(reading);
    }

    /// <summary>
    /// Diagnostic: get the last raw SMA response for all active scales.
    /// Shows raw text (with LF/CR markers), hex bytes, parsed values, and timing.
    /// </summary>
    [HttpGet("api/diagnostic")]
    public IActionResult GetDiagnostic()
    {
        var readings = _weightStore.GetAll();
        var result = readings.Values.OrderBy(r => r.ScaleId).Select(r => new
        {
            r.ScaleId,
            r.DisplayName,
            parsed = new { r.Weight, r.Motion, r.Ok, r.Status },
            raw = new { text = r.RawResponse, hex = r.RawHex },
            r.LastUpdate
        });
        return Ok(result);
    }

    /// <summary>
    /// Diagnostic: get the last raw SMA response for a specific scale.
    /// </summary>
    [HttpGet("api/diagnostic/{scaleId}")]
    public IActionResult GetDiagnosticForScale(string scaleId)
    {
        var reading = _weightStore.Get(scaleId);
        if (reading == null)
            return NotFound(new { error = $"No reading for scale '{scaleId}'." });
        return Ok(new
        {
            reading.ScaleId,
            reading.DisplayName,
            parsed = new { reading.Weight, reading.Motion, reading.Ok, reading.Status },
            raw = new { text = reading.RawResponse, hex = reading.RawHex },
            reading.LastUpdate
        });
    }

    // ===== SETTINGS =====

    [HttpGet("api/settings")]
    public IActionResult GetSettings()
    {
        var settings = _db.Settings.OrderBy(s => s.Id).FirstOrDefault();
        return Ok(settings);
    }

    [HttpPut("api/settings")]
    public IActionResult UpdateSettings([FromBody] ServiceSettings update)
    {
        var settings = _db.Settings.OrderBy(s => s.Id).FirstOrDefault();
        if (settings == null) return NotFound();

        if (!string.IsNullOrWhiteSpace(update.ServiceId)) settings.ServiceId = update.ServiceId;
        if (!string.IsNullOrWhiteSpace(update.ServerUrl)) settings.ServerUrl = update.ServerUrl;
        if (!string.IsNullOrWhiteSpace(update.SignalRHub)) settings.SignalRHub = update.SignalRHub;
        if (update.BrandsUrl != null) settings.BrandsUrl = update.BrandsUrl;
        if (update.BrandsToken != null) settings.BrandsToken = update.BrandsToken;

        _db.SaveChanges();

        // Trigger service restart to pick up new settings
        var restart = HttpContext.RequestServices.GetService<RestartSignal>();
        restart?.TriggerRestart();

        return Ok(settings);
    }

    // ===== SCALES CRUD =====

    [HttpGet("api/scales")]
    public IActionResult GetAll()
    {
        var scales = _db.Scales.OrderBy(s => s.ScaleId).ToList();
        return Ok(scales);
    }

    [HttpGet("api/scales/{scaleId}")]
    public IActionResult GetById(string scaleId)
    {
        var scale = _db.Scales.AsNoTracking().FirstOrDefault(s => s.ScaleId == scaleId);
        if (scale == null) return NotFound();
        return Ok(scale);
    }

    [HttpPost("api/scales")]
    public IActionResult Create([FromBody] ScaleConfigEntity scale)
    {
        if (string.IsNullOrWhiteSpace(scale.ScaleId))
            return BadRequest("ScaleId is required");

        if (_db.Scales.AsNoTracking().Any(s => s.ScaleId == scale.ScaleId))
            return Conflict($"Scale '{scale.ScaleId}' already exists");

        scale.Id = 0;
        _db.Scales.Add(scale);
        _db.SaveChanges();

        var announce = HttpContext.RequestServices.GetService<AnnounceSignal>();
        announce?.TriggerAnnounce();

        return Created($"/api/scales/{scale.ScaleId}", scale);
    }

    [HttpPut("api/scales/{scaleId}")]
    public IActionResult Update(string scaleId, [FromBody] ScaleConfigEntity update)
    {
        var existing = _db.Scales.FirstOrDefault(s => s.ScaleId == scaleId);
        if (existing == null) return NotFound();

        existing.DisplayName = update.DisplayName ?? existing.DisplayName;
        existing.ScaleBrand = update.ScaleBrand ?? existing.ScaleBrand;
        existing.IpAddress = update.IpAddress ?? existing.IpAddress;
        existing.Port = update.Port > 0 ? update.Port : existing.Port;
        existing.RequestCommand = update.RequestCommand;
        existing.PollingIntervalMs = update.PollingIntervalMs > 0 ? update.PollingIntervalMs : existing.PollingIntervalMs;
        existing.TimeoutMs = update.TimeoutMs > 0 ? update.TimeoutMs : existing.TimeoutMs;
        existing.Active = update.Active;

        _db.SaveChanges();

        var announce = HttpContext.RequestServices.GetService<AnnounceSignal>();
        announce?.TriggerAnnounce();

        return Ok(existing);
    }

    [HttpDelete("api/scales/{scaleId}")]
    public IActionResult Delete(string scaleId)
    {
        var existing = _db.Scales.FirstOrDefault(s => s.ScaleId == scaleId);
        if (existing == null) return NotFound();

        _db.Scales.Remove(existing);
        _db.SaveChanges();

        var announce = HttpContext.RequestServices.GetService<AnnounceSignal>();
        announce?.TriggerAnnounce();

        return Ok(new { deleted = scaleId });
    }

    // ===== BRANDS =====

    [HttpGet("api/status/brands")]
    public IActionResult GetBrands()
    {
        var brands = HttpContext.RequestServices.GetService<List<ScaleBrandDefinition>>();
        return Ok(brands ?? new List<ScaleBrandDefinition>());
    }
}
