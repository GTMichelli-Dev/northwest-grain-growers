using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using BasicWeigh.Web.Data;
using BasicWeigh.Web.Hubs;
using BasicWeigh.Web.Models;
using BasicWeigh.Web.Services;

namespace BasicWeigh.Web.Controllers;

public class CameraController : Controller
{
    private readonly ScaleDbContext _db;
    private readonly IHubContext<ScaleHub> _hub;
    private readonly AppSetupCache _setupCache;

    public CameraController(ScaleDbContext db, IHubContext<ScaleHub> hub, AppSetupCache setupCache)
    {
        _db = db;
        _hub = hub;
        _setupCache = setupCache;
    }

    // MVC view for Camera Management — all data loads via SignalR
    public IActionResult Index()
    {
        var setup = _setupCache.Get();
        ViewBag.InboundCameraId = setup.InboundCameraId ?? "";
        ViewBag.OutboundCameraId = setup.OutboundCameraId ?? "";
        return View();
    }

    // API to save camera assignments
    [HttpPost("api/cameras/assignments")]
    public IActionResult SaveAssignments([FromBody] CameraAssignmentDto dto)
    {
        var setup = _db.AppSetup.First();
        setup.InboundCameraId = dto.InboundCameraId;
        setup.OutboundCameraId = dto.OutboundCameraId;
        _db.SaveChanges();
        _setupCache.Invalidate();
        return Ok(new { success = true });
    }

    // ---- CRUD API ----

    [HttpGet("api/cameras")]
    public IActionResult GetAll()
    {
        var cameras = _db.CameraConfigs.OrderBy(c => c.DisplayName).ToList();
        return Json(cameras);
    }

    [HttpGet("api/cameras/{cameraId}")]
    public IActionResult GetById(string cameraId)
    {
        var camera = _db.CameraConfigs.FirstOrDefault(c => c.CameraId == cameraId);
        if (camera == null) return NotFound();
        return Json(camera);
    }

    [HttpPost("api/cameras")]
    public async Task<IActionResult> Create([FromBody] CameraConfig camera)
    {
        if (string.IsNullOrWhiteSpace(camera.CameraId))
            return BadRequest("CameraId is required.");

        if (_db.CameraConfigs.Any(c => c.CameraId == camera.CameraId))
            return Conflict($"Camera '{camera.CameraId}' already exists.");

        // If this is set as default, clear other defaults
        if (camera.IsDefault)
            await ClearOtherDefaults(null);

        _db.CameraConfigs.Add(camera);
        await _db.SaveChangesAsync();

        // Notify CameraService to reload
        await _hub.Clients.Group("CameraClients").SendAsync("ReloadConfig");

        return Json(camera);
    }

    [HttpPut("api/cameras/{cameraId}")]
    public async Task<IActionResult> Update(string cameraId, [FromBody] CameraConfig camera)
    {
        var existing = _db.CameraConfigs.FirstOrDefault(c => c.CameraId == cameraId);
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

        // If this is set as default, clear other defaults
        if (camera.IsDefault)
            await ClearOtherDefaults(existing.Id);

        await _db.SaveChangesAsync();

        // Notify CameraService to reload
        await _hub.Clients.Group("CameraClients").SendAsync("ReloadConfig");

        return Json(existing);
    }

    [HttpDelete("api/cameras/{cameraId}")]
    public async Task<IActionResult> Delete(string cameraId)
    {
        var camera = _db.CameraConfigs.FirstOrDefault(c => c.CameraId == cameraId);
        if (camera == null) return NotFound();

        _db.CameraConfigs.Remove(camera);
        await _db.SaveChangesAsync();

        // Notify CameraService to reload
        await _hub.Clients.Group("CameraClients").SendAsync("ReloadConfig");

        return Ok();
    }

    /// <summary>
    /// Test a camera by sending a capture command with a test ticket.
    /// </summary>
    [HttpPost("api/cameras/{cameraId}/test")]
    public async Task<IActionResult> TestCamera(string cameraId)
    {
        var camera = _db.CameraConfigs.FirstOrDefault(c => c.CameraId == cameraId);
        if (camera == null) return NotFound();
        if (!camera.Active) return BadRequest("Camera is not active.");

        await _hub.Clients.Group("CameraClients").SendAsync("CaptureImage",
            new { ticket = "TEST", direction = "test", cameraId });

        return Ok(new { message = $"Test capture sent to camera '{cameraId}'." });
    }

    /// <summary>
    /// Returns camera configs for the CameraService to load remotely.
    /// </summary>
    [HttpGet("api/setup/camera")]
    public IActionResult GetCameraSettings()
    {
        var setup = _setupCache.Get();
        var cameras = _db.CameraConfigs.Where(c => c.Active).ToList();
        return Json(new
        {
            savePicture = setup.SavePicture,
            cameras = cameras.Select(c => new
            {
                c.CameraId,
                c.DisplayName,
                c.CameraBrand,
                c.CameraIp,
                c.CameraUser,
                c.CameraPassword,
                c.UsbDeviceName,
                c.CameraUrl,
                c.UsbCommand,
                c.TimeoutSeconds,
                c.IsDefault
            })
        });
    }

    // ---- Helpers ----

    private async Task ClearOtherDefaults(int? exceptId)
    {
        var others = _db.CameraConfigs.Where(c => c.IsDefault);
        if (exceptId.HasValue)
            others = others.Where(c => c.Id != exceptId.Value);

        await others.ForEachAsync(c => c.IsDefault = false);
    }
}
