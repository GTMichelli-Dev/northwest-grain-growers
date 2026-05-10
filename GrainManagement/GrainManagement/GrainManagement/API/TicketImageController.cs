#nullable enable
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GrainManagement.Hubs;
using GrainManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GrainManagement.API;

/// <summary>
/// Receives multipart image uploads from CameraService instances and
/// serves them back to the web UI / ticket views.
///
/// Filenames on disk are normalized to <c>{loadNumber}_{Direction}.jpg</c>
/// (e.g. <c>12345_In.jpg</c>, <c>12345_Out.jpg</c>, <c>12345_bol.jpg</c>) —
/// the form the user explicitly requested. The physical folder comes
/// from <c>TicketImages:PhysicalPath</c> (Linux: /var/grainmanagement/ticket-images;
/// dev Windows: C:\Images) and is also exposed for the FileMirrorMover /
/// PictureUpsert service to pick up.
/// </summary>
[ApiController]
[Route("api/ticket")]
public sealed class TicketImageController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly IHubContext<CameraHub, ICameraClient> _hub;
    private readonly ILogger<TicketImageController> _logger;
    private readonly dbContext _db;

    public TicketImageController(
        IConfiguration config,
        IHubContext<CameraHub, ICameraClient> hub,
        ILogger<TicketImageController> logger,
        dbContext db)
    {
        _config = config;
        _hub = hub;
        _logger = logger;
        _db = db;
    }

    private string TicketImageDir
        => _config["TicketImages:PhysicalPath"]
           ?? throw new InvalidOperationException("TicketImages:PhysicalPath is not configured.");

    [HttpPost("{loadNumber}/image")]
    [RequestSizeLimit(20_000_000)] // 20 MB cap
    public async Task<IActionResult> Upload(
        string loadNumber,
        [FromQuery] string direction,
        IFormFile file)
    {
        if (string.IsNullOrWhiteSpace(loadNumber))
            return BadRequest(new { message = "Load number is required." });

        if (file is null || file.Length == 0)
            return BadRequest(new { message = "No file was uploaded." });

        var suffix = NormalizeDirection(direction);
        if (suffix is null)
            return BadRequest(new { message = "direction must be 'in', 'out', or 'bol'." });

        Directory.CreateDirectory(TicketImageDir);
        var filename = $"{Sanitize(loadNumber)}_{suffix}.jpg";
        var path = Path.Combine(TicketImageDir, filename);

        await using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            await file.CopyToAsync(stream);
        }

        _logger.LogInformation("Saved ticket image {File} ({Bytes} bytes).", filename, file.Length);

        // Web UI hint — grids/ticket pages can refresh their "image available" state
        await _hub.Clients.All.ImageCaptured(loadNumber, suffix);

        return Ok(new { success = true, file = filename });
    }

    [HttpGet("{loadNumber}/image")]
    public IActionResult Get(string loadNumber, [FromQuery] string direction)
    {
        var suffix = NormalizeDirection(direction);
        if (suffix is null) return BadRequest();

        var path = Path.Combine(TicketImageDir, $"{Sanitize(loadNumber)}_{suffix}.jpg");
        if (!System.IO.File.Exists(path)) return NotFound();
        return PhysicalFile(path, "image/jpeg");
    }

    /// <summary>Returns which images exist for this load — used to gate the "view" buttons in the UI.</summary>
    [HttpGet("{loadNumber}/image-status")]
    public IActionResult Status(string loadNumber)
    {
        var safe = Sanitize(loadNumber);
        return Ok(new
        {
            inImage  = System.IO.File.Exists(Path.Combine(TicketImageDir, $"{safe}_In.jpg")),
            outImage = System.IO.File.Exists(Path.Combine(TicketImageDir, $"{safe}_Out.jpg")),
            bolImage = System.IO.File.Exists(Path.Combine(TicketImageDir, $"{safe}_bol.jpg"))
        });
    }

    /// <summary>
    /// Server-aware image lookup. The first 3 digits of a load number
    /// identify the server that recorded the weigh (and therefore holds
    /// the image on disk). This endpoint resolves the originating server
    /// via <see cref="Server"/>, then 302-redirects the browser to the
    /// static <c>/ticket-images/{load}_{dir}.jpg</c> path on that server.
    /// Use this as the PDF/UI link target so the same URL works regardless
    /// of which environment opened the document.
    /// </summary>
    /// <remarks>
    /// Route: <c>GET /api/ticket-image/{loadNumber}?direction=in|out|bol</c>.
    /// Lives in this controller (not its own) so the routes share a stem
    /// and the controller can also serve same-server images directly when
    /// the resolved server URL points back at us.
    /// </remarks>
    [HttpGet("/api/ticket-image/{loadNumber}")]
    public async Task<IActionResult> RedirectToOwningServer(string loadNumber, [FromQuery] string direction)
    {
        var safeLoad = Sanitize(loadNumber);
        var suffix = NormalizeDirection(direction);
        if (suffix is null)
            return BadRequest(new { message = "direction must be 'in', 'out', or 'bol'." });

        if (safeLoad.Length < 3 || !int.TryParse(safeLoad.AsSpan(0, 3), out var serverId))
            return BadRequest(new { message = "Load number must start with a 3-digit server id." });

        // Resolve the server URL. Active-only — we never want to redirect
        // browsers at a decommissioned host.
        var url = await _db.Servers.AsNoTracking()
            .Where(s => s.ServerId == serverId && s.IsActive && s.Url != null && s.Url != "")
            .Select(s => s.Url)
            .FirstOrDefaultAsync();
        if (string.IsNullOrWhiteSpace(url))
            return NotFound(new { message = $"No active server registered for id {serverId:000}." });

        var target = $"{url!.TrimEnd('/')}/ticket-images/{safeLoad}_{suffix}.jpg";
        // 302 (Found) rather than 301 so the browser doesn't permanent-cache
        // the redirect — a server URL change in Servers should take effect
        // on next click, not after a cache purge.
        return Redirect(target);
    }

    private static string? NormalizeDirection(string? d)
    {
        if (string.IsNullOrWhiteSpace(d)) return null;
        return d.Trim().ToLowerInvariant() switch
        {
            "in"  => "In",
            "out" => "Out",
            "bol" => "bol",
            _ => null
        };
    }

    private static string Sanitize(string s)
    {
        // Disallow path separators / NULs / parent-dir tricks — load numbers
        // are simple ints in practice but never trust client input on a
        // path-construction code path.
        foreach (var c in Path.GetInvalidFileNameChars())
            s = s.Replace(c, '_');
        return s.Replace("..", "_");
    }
}
