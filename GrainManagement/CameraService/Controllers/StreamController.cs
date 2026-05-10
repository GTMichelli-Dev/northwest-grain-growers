using CameraService.Services;
using Microsoft.AspNetCore.Mvc;

namespace CameraService.Controllers;

/// <summary>
/// MJPEG live stream for a single camera. Reuses one producer per camera
/// via <see cref="MjpegStreamPool"/>, so N viewers cost the same as one.
/// </summary>
[ApiController]
[Route("api/stream")]
public sealed class StreamController : ControllerBase
{
    private readonly MjpegStreamPool _pool;
    private readonly ILogger<StreamController> _logger;
    private const string Boundary = "--gm-mjpeg-frame";

    public StreamController(MjpegStreamPool pool, ILogger<StreamController> logger)
    {
        _pool = pool;
        _logger = logger;
    }

    /// <summary>
    /// Multipart MJPEG stream. Embed via <c>&lt;img src="/api/stream/{cameraId}"&gt;</c>.
    /// </summary>
    [HttpGet("{cameraId}")]
    public async Task Stream(string cameraId, CancellationToken ct)
    {
        Response.Headers.Append("Cache-Control", "no-cache, no-store, must-revalidate");
        Response.Headers.Append("Pragma", "no-cache");
        Response.Headers.Append("Connection", "close");
        Response.ContentType = $"multipart/x-mixed-replace; boundary={Boundary}";

        using var sub = _pool.Subscribe(cameraId);
        var lastSent = DateTime.MinValue;

        try
        {
            while (!ct.IsCancellationRequested && !HttpContext.RequestAborted.IsCancellationRequested)
            {
                // Wait for the next frame from the shared producer; bail if the
                // client disconnects.
                try { await sub.WaitForNextFrameAsync(ct); }
                catch (OperationCanceledException) { break; }

                var (frame, atUtc) = sub.GetLatest();
                if (frame is null || atUtc == lastSent) continue;
                lastSent = atUtc;

                var header = $"\r\n{Boundary}\r\nContent-Type: image/jpeg\r\nContent-Length: {frame.Length}\r\n\r\n";
                var headerBytes = System.Text.Encoding.ASCII.GetBytes(header);

                try
                {
                    await Response.Body.WriteAsync(headerBytes, ct);
                    await Response.Body.WriteAsync(frame, ct);
                    await Response.Body.FlushAsync(ct);
                }
                catch (Exception ex) when (ex is OperationCanceledException || ex is IOException)
                {
                    break; // client gone
                }
            }
        }
        finally
        {
            _logger.LogDebug("MJPEG client disconnected from {Camera}.", cameraId);
        }
    }

    /// <summary>
    /// Single-frame snapshot — useful for thumbnails or for clients that
    /// can't handle multipart streams.
    /// </summary>
    [HttpGet("{cameraId}/snapshot")]
    public IActionResult Snapshot(string cameraId)
    {
        using var sub = _pool.Subscribe(cameraId);
        // Wait briefly for a frame, then return whatever we have.
        for (int i = 0; i < 30; i++) // up to ~1.5s for a fresh frame
        {
            var (frame, _) = sub.GetLatest();
            if (frame is not null) return File(frame, "image/jpeg");
            Thread.Sleep(50);
        }
        return NotFound(new { message = "No frame available yet." });
    }
}
