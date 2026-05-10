using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PictureUpsert.Data;
using PictureUpsert.Models;
using PictureUpsert.Services;

namespace PictureUpsert.Controllers;

[ApiController]
[Route("api")]
public sealed class StatusController : ControllerBase
{
    private readonly UpsertDbContext _db;
    private readonly StatusState _status;

    public StatusController(UpsertDbContext db, StatusState status)
    {
        _db = db;
        _status = status;
    }

    /// <summary>Live operational status — what the web's Camera page polls.</summary>
    [HttpGet("status")]
    public IActionResult Status()
        => Ok(new
        {
            healthy = _status.RemoteReachable && _status.FailedCount == 0,
            remoteReachable = _status.RemoteReachable,
            pendingCount = _status.PendingCount,
            failedCount = _status.FailedCount,
            totalSent = _status.TotalSent,
            lastSuccessUtc = _status.LastSuccessUtc,
            lastAttemptUtc = _status.LastAttemptUtc,
            lastError = _status.LastError
        });

    [HttpGet("health")]
    public IActionResult Health() => Ok(new { ok = true });

    [HttpGet("settings")]
    public async Task<IActionResult> GetSettings()
        => Ok(await _db.Settings.OrderBy(s => s.Id).FirstAsync());

    [HttpPut("settings")]
    public async Task<IActionResult> SetSettings([FromBody] UpsertSettings update)
    {
        var s = await _db.Settings.OrderBy(x => x.Id).FirstAsync();
        s.LocalFolder        = update.LocalFolder;
        s.RemoteBaseUrl      = update.RemoteBaseUrl;
        s.RemoteUploadPath   = update.RemoteUploadPath;
        s.AuthToken          = update.AuthToken;
        s.DeleteAfterUpload  = update.DeleteAfterUpload;
        s.RetryDelaySeconds  = update.RetryDelaySeconds;
        s.Filter             = update.Filter;
        await _db.SaveChangesAsync();
        return Ok(s);
    }

    [HttpGet("queue")]
    public async Task<IActionResult> Queue([FromQuery] string? status = null, [FromQuery] int take = 100)
    {
        var q = _db.Queue.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(status)) q = q.Where(x => x.Status == status);
        var rows = await q.OrderByDescending(x => x.EnqueuedUtc).Take(Math.Clamp(take, 1, 1000)).ToListAsync();
        return Ok(rows);
    }

    /// <summary>Force a retry of every Failed item — useful when the remote came back.</summary>
    [HttpPost("queue/retry-failed")]
    public async Task<IActionResult> RetryFailed()
    {
        var failed = await _db.Queue.Where(q => q.Status == "Failed").ToListAsync();
        foreach (var f in failed)
        {
            f.Status = "Pending";
            f.LastError = null;
        }
        await _db.SaveChangesAsync();
        return Ok(new { reset = failed.Count });
    }
}
