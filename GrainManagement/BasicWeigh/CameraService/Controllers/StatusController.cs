using CameraService.Data;
using CameraService.Models;
using CameraService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CameraService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatusController : ControllerBase
{
    private readonly CameraDbContext _db;
    private readonly CameraOptions _options;
    private readonly CameraCaptureService _capture;

    public StatusController(
        CameraDbContext db,
        IOptions<CameraOptions> options,
        CameraCaptureService capture)
    {
        _db = db;
        _options = options.Value;
        _capture = capture;
    }

    /// <summary>
    /// Health check — returns OK if the service is running.
    /// </summary>
    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok(new
        {
            status = "healthy",
            service = "Camera Capture Service",
            timestamp = DateTime.UtcNow,
            camerasConfigured = _db.Cameras.Count(c => c.Active)
        });
    }

    /// <summary>
    /// Returns the list of loaded camera brand definitions.
    /// </summary>
    [HttpGet("brands")]
    public IActionResult GetBrands()
    {
        var brands = _options.GetBrands() ?? new List<CameraBrandDefinition>();
        return Ok(brands.Select(b => new
        {
            b.Brand,
            b.Type,
            b.SnapshotUrlTemplate,
            b.CaptureCommandTemplate,
            b.Notes
        }));
    }

    /// <summary>
    /// Takes a test snapshot by cameraId (string) and returns the JPEG image.
    /// Pass no parameter to use the default camera.
    /// </summary>
    [HttpGet("test-capture")]
    [ProducesResponseType(typeof(FileResult), 200)]
    public async Task<IActionResult> TestCapture([FromQuery] string? cameraId = null)
    {
        try
        {
            var imageBytes = await _capture.CaptureAsync(cameraId);
            if (imageBytes.Length == 0)
                return StatusCode(502, new { error = "Camera returned empty image." });

            return File(imageBytes, "image/jpeg", "test-capture.jpg");
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(502, new { error = $"Camera connection failed: {ex.Message}" });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = $"Capture failed: {ex.Message}" });
        }
    }

    /// <summary>
    /// Takes a test snapshot by database Id (int) and returns the JPEG image.
    /// </summary>
    [HttpGet("test-capture/{id:int}")]
    [ProducesResponseType(typeof(FileResult), 200)]
    public async Task<IActionResult> TestCaptureById(int id)
    {
        try
        {
            var imageBytes = await _capture.CaptureByIdAsync(id);
            if (imageBytes.Length == 0)
                return StatusCode(502, new { error = "Camera returned empty image." });

            return File(imageBytes, "image/jpeg", "test-capture.jpg");
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(502, new { error = $"Camera connection failed: {ex.Message}" });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = $"Capture failed: {ex.Message}" });
        }
    }

    /// <summary>
    /// Takes a test snapshot and uploads it to the web app as a test ticket.
    /// </summary>
    [HttpPost("test-capture")]
    public async Task<IActionResult> TestCaptureAndUpload([FromQuery] string? cameraId = null)
    {
        try
        {
            var success = await _capture.CaptureAndUploadAsync("TEST", "test", cameraId);
            if (success)
                return Ok(new { message = "Test image captured and uploaded successfully." });
            else
                return StatusCode(502, new { error = "Capture or upload failed. Check service logs." });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = $"Test capture failed: {ex.Message}" });
        }
    }
}

// ===== SERVICE SETTINGS =====

[ApiController]
[Route("api/settings")]
public class SettingsController : ControllerBase
{
    private readonly CameraDbContext _db;
    private readonly RestartSignal _restart;
    private readonly AnnounceSignal _announce;

    public SettingsController(CameraDbContext db, RestartSignal restart, AnnounceSignal announce)
    {
        _db = db;
        _restart = restart;
        _announce = announce;
    }

    /// <summary>
    /// Returns the service settings (server URL, SignalR hub, brands config).
    /// </summary>
    [HttpGet]
    public IActionResult Get()
    {
        var settings = _db.Settings.OrderBy(s => s.Id).First();
        return Ok(settings);
    }

    /// <summary>
    /// Updates the service settings. Persists to database.
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ServiceSettings update)
    {
        var settings = _db.Settings.OrderBy(s => s.Id).First();
        if (!string.IsNullOrWhiteSpace(update.ServerUrl))
            settings.ServerUrl = update.ServerUrl;
        if (!string.IsNullOrWhiteSpace(update.SignalRHub))
            settings.SignalRHub = update.SignalRHub;
        settings.BrandsUrl = update.BrandsUrl;
        settings.BrandsToken = update.BrandsToken;

        await _db.SaveChangesAsync();

        // Trigger service restart to apply new settings
        _restart.TriggerRestart();

        return Ok(new { settings, message = "Settings saved. Service is restarting..." });
    }
}

// ===== CAMERAS CRUD =====

[ApiController]
[Route("api/cameras")]
public class CamerasController : ControllerBase
{
    private readonly CameraDbContext _db;
    private readonly AnnounceSignal _announce;

    public CamerasController(CameraDbContext db, AnnounceSignal announce)
    {
        _db = db;
        _announce = announce;
    }

    /// <summary>
    /// List all cameras.
    /// </summary>
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_db.Cameras.OrderBy(c => c.DisplayName).ToList());
    }

    /// <summary>
    /// Get a camera by its unique cameraId.
    /// </summary>
    [HttpGet("{cameraId}")]
    public IActionResult GetById(string cameraId)
    {
        var camera = _db.Cameras.FirstOrDefault(c => c.CameraId == cameraId);
        if (camera == null) return NotFound();
        return Ok(camera);
    }

    /// <summary>
    /// Add a new camera.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CameraConfigEntity camera)
    {
        if (string.IsNullOrWhiteSpace(camera.CameraId))
            return BadRequest("CameraId is required.");

        if (_db.Cameras.AsNoTracking().Any(c => c.CameraId == camera.CameraId))
            return Conflict($"Camera '{camera.CameraId}' already exists.");

        camera.Id = 0; // Ensure EF treats as new entity

        if (camera.IsDefault)
            await ClearOtherDefaults(null);

        _db.Cameras.Add(camera);
        await _db.SaveChangesAsync();
        _announce.TriggerAnnounce();
        return CreatedAtAction(nameof(GetById), new { cameraId = camera.CameraId }, camera);
    }

    /// <summary>
    /// Update an existing camera.
    /// </summary>
    [HttpPut("{cameraId}")]
    public async Task<IActionResult> Update(string cameraId, [FromBody] CameraConfigEntity camera)
    {
        var existing = _db.Cameras.FirstOrDefault(c => c.CameraId == cameraId);
        if (existing == null) return NotFound();

        existing.DisplayName = camera.DisplayName;
        existing.CameraBrand = camera.CameraBrand;
        existing.CameraIp = camera.CameraIp;
        existing.CameraUser = camera.CameraUser;
        existing.CameraPassword = camera.CameraPassword;
        existing.UsbDeviceName = camera.UsbDeviceName;
        existing.CameraUrl = camera.CameraUrl;
        existing.UsbCommand = camera.UsbCommand;
        existing.TimeoutSeconds = camera.TimeoutSeconds;
        existing.Active = camera.Active;
        existing.IsDefault = camera.IsDefault;

        if (camera.IsDefault)
            await ClearOtherDefaults(existing.Id);

        await _db.SaveChangesAsync();
        _announce.TriggerAnnounce();
        return Ok(existing);
    }

    /// <summary>
    /// Delete a camera.
    /// </summary>
    [HttpDelete("{cameraId}")]
    public async Task<IActionResult> Delete(string cameraId)
    {
        var camera = _db.Cameras.FirstOrDefault(c => c.CameraId == cameraId);
        if (camera == null) return NotFound();

        _db.Cameras.Remove(camera);
        await _db.SaveChangesAsync();
        _announce.TriggerAnnounce();
        return Ok();
    }

    private async Task ClearOtherDefaults(int? exceptId)
    {
        var others = _db.Cameras.Where(c => c.IsDefault);
        if (exceptId.HasValue)
            others = others.Where(c => c.Id != exceptId.Value);

        await others.ForEachAsync(c => c.IsDefault = false);
        await _db.SaveChangesAsync();
    }
}
