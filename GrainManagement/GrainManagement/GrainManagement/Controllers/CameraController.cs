#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrainManagement.Dtos.Cameras;
using GrainManagement.Hubs;
using GrainManagement.Models;
using GrainManagement.Services.Camera;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace GrainManagement.Controllers
{
    /// <summary>
    /// Camera admin UI + role-assignment API. Hardware definitions live
    /// inside each CameraService and reach this controller via
    /// <see cref="ICameraRegistry"/>; role/scale/BOL assignments live in
    /// SQL Server (system.CameraAssignments) and are managed here.
    /// </summary>
    public class CameraController : Controller
    {
        private readonly dbContext _db;
        private readonly ICameraRegistry _registry;
        private readonly IHubContext<CameraHub, ICameraClient> _hub;

        public CameraController(
            dbContext db,
            ICameraRegistry registry,
            IHubContext<CameraHub, ICameraClient> hub)
        {
            _db = db;
            _registry = registry;
            _hub = hub;
        }

        // ── MVC views ────────────────────────────────────────────────────

        [HttpGet("/Camera")]
        public IActionResult Index() => View();

        [HttpGet("/Camera/View/{serviceId}/{cameraId}")]
        public IActionResult ViewCamera(string serviceId, string cameraId, bool pop = false)
        {
            var cam = _registry.Get(serviceId, cameraId);
            if (cam is null) return RedirectToAction(nameof(Index));
            ViewData["Pop"] = pop;
            return View("View", cam);
        }

        // ── Registry / status API ────────────────────────────────────────

        /// <summary>Cameras currently connected (no role merge — that's done client-side from /assignments).</summary>
        [HttpGet("/api/cameras")]
        public IActionResult GetAll()
        {
            var cams = _registry.GetAll().Select(c => new
            {
                c.ServiceId,
                c.CameraId,
                c.DisplayName,
                c.CameraBrand,
                c.Active,
                c.StreamUrl
            });
            return Json(cams);
        }

        [HttpGet("/api/cameras/services")]
        public IActionResult GetServices()
            => Json(new { connected = _registry.ConnectedServices });

        // ── Assignments (system.CameraAssignments) ───────────────────────

        [HttpGet("/api/cameras/assignments")]
        public async Task<IActionResult> GetAssignments(int? scaleId, string? role)
        {
            var q = _db.CameraAssignments.AsNoTracking().Where(a => a.IsActive);
            if (scaleId.HasValue) q = q.Where(a => a.ScaleId == scaleId.Value);
            if (!string.IsNullOrWhiteSpace(role)) q = q.Where(a => a.Role == role);

            var rows = await q
                .OrderByDescending(a => a.IsPrimary)
                .ThenBy(a => a.DisplayName)
                .Select(a => new
                {
                    a.CameraAssignmentId,
                    a.ServiceId,
                    a.CameraId,
                    a.DisplayName,
                    a.LocationId,
                    a.ScaleId,
                    a.Role,
                    a.IsPrimary
                })
                .ToListAsync();

            return Json(rows);
        }

        [HttpPost("/api/cameras/assignments")]
        public async Task<IActionResult> CreateAssignment([FromBody] CameraAssignmentRequest req)
        {
            var validRoles = new[] { "Inbound", "Outbound", "BOL", "View", "TempTicket" };
            if (req is null || string.IsNullOrWhiteSpace(req.ServiceId) ||
                string.IsNullOrWhiteSpace(req.CameraId) || !validRoles.Contains(req.Role))
                return BadRequest(new { message = "ServiceId, CameraId, and Role (Inbound/Outbound/BOL/View/TempTicket) are required." });

            // If marked primary, demote other primaries for the same (location, scale, role) bucket.
            if (req.IsPrimary)
                await DemoteSiblingPrimariesAsync(req.LocationId, req.ScaleId, req.Role, exceptId: null);

            var row = new CameraAssignment
            {
                ServiceId = req.ServiceId,
                CameraId = req.CameraId,
                DisplayName = string.IsNullOrWhiteSpace(req.DisplayName) ? req.CameraId : req.DisplayName,
                LocationId = req.LocationId,
                ScaleId = req.ScaleId,
                Role = req.Role,
                IsPrimary = req.IsPrimary,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            _db.CameraAssignments.Add(row);
            await _db.SaveChangesAsync();

            return Json(new { row.CameraAssignmentId });
        }

        [HttpPut("/api/cameras/assignments/{id:int}")]
        public async Task<IActionResult> UpdateAssignment(int id, [FromBody] CameraAssignmentRequest req)
        {
            var row = await _db.CameraAssignments.FirstOrDefaultAsync(a => a.CameraAssignmentId == id);
            if (row is null) return NotFound();

            if (req.IsPrimary && !row.IsPrimary)
                await DemoteSiblingPrimariesAsync(req.LocationId, req.ScaleId, req.Role, exceptId: id);

            row.ServiceId = req.ServiceId;
            row.CameraId = req.CameraId;
            row.DisplayName = string.IsNullOrWhiteSpace(req.DisplayName) ? req.CameraId : req.DisplayName;
            row.LocationId = req.LocationId;
            row.ScaleId = req.ScaleId;
            row.Role = req.Role;
            row.IsPrimary = req.IsPrimary;
            row.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("/api/cameras/assignments/{id:int}")]
        public async Task<IActionResult> DeleteAssignment(int id)
        {
            var row = await _db.CameraAssignments.FirstOrDefaultAsync(a => a.CameraAssignmentId == id);
            if (row is null) return NotFound();
            row.IsActive = false;
            row.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return Ok();
        }

        // ── BOL helpers (used by the scan-BOL modal in scale views) ─────

        /// <summary>Returns active BOL cameras for the given location, with the connected/streamable flag merged in.</summary>
        [HttpGet("/api/cameras/bol")]
        public async Task<IActionResult> GetBolCameras(int? locationId)
        {
            var q = _db.CameraAssignments.AsNoTracking()
                .Where(a => a.IsActive && a.Role == "BOL");
            if (locationId.HasValue)
                q = q.Where(a => a.LocationId == locationId || a.LocationId == null);

            var rows = await q.OrderBy(a => a.DisplayName).ToListAsync();

            var result = rows.Select(a =>
            {
                var live = _registry.Get(a.ServiceId, a.CameraId);
                return new
                {
                    a.CameraAssignmentId,
                    a.ServiceId,
                    a.CameraId,
                    a.DisplayName,
                    online = live is not null && live.Active,
                    streamUrl = live?.StreamUrl
                };
            });
            return Json(result);
        }

        /// <summary>
        /// Whether the ScanBOL button should be visible at all — true iff any
        /// active BOL assignment exists. Returns the count so the UI can also
        /// auto-pick when count == 1.
        /// </summary>
        [HttpGet("/api/cameras/bol/available")]
        public async Task<IActionResult> BolAvailable(int? locationId)
        {
            var q = _db.CameraAssignments.AsNoTracking()
                .Where(a => a.IsActive && a.Role == "BOL");
            if (locationId.HasValue)
                q = q.Where(a => a.LocationId == locationId || a.LocationId == null);

            var count = await q.CountAsync();
            return Json(new { available = count > 0, count });
        }

        /// <summary>Web-initiated BOL capture — called when the user clicks "Take BOL Picture" in the modal.</summary>
        [HttpPost("/api/cameras/{serviceId}/{cameraId}/capture")]
        public async Task<IActionResult> Capture(string serviceId, string cameraId, [FromQuery] string ticket, [FromQuery] string direction)
        {
            if (string.IsNullOrWhiteSpace(ticket) || string.IsNullOrWhiteSpace(direction))
                return BadRequest(new { message = "ticket and direction are required." });

            await _hub.Clients.Group(CameraHub.ServiceGroupName(serviceId))
                .CaptureImage(new CaptureCommand(ticket, direction, cameraId));

            return Ok(new { sent = true });
        }

        // ── Helpers ───────────────────────────────────────────────────────

        private async Task DemoteSiblingPrimariesAsync(int? locationId, int? scaleId, string role, int? exceptId)
        {
            var siblings = await _db.CameraAssignments
                .Where(a => a.IsActive
                            && a.IsPrimary
                            && a.Role == role
                            && a.LocationId == locationId
                            && a.ScaleId == scaleId
                            && (exceptId == null || a.CameraAssignmentId != exceptId.Value))
                .ToListAsync();
            foreach (var s in siblings)
            {
                s.IsPrimary = false;
                s.UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}
