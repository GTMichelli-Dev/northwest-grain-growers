#nullable enable
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GrainManagement.Dtos.Cameras;
using GrainManagement.Hubs;
using GrainManagement.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GrainManagement.Services.Camera;

/// <summary>
/// Fans out capture commands to every CameraService that owns a camera
/// matching the role + location/scale of an event (weigh-in / weigh-out).
/// Pure fire-and-forget — failures here must not block the underlying
/// write (the load saved successfully whether a photo was taken or not).
/// </summary>
public interface ICameraCaptureTrigger
{
    Task FireAsync(string loadNumber, string direction, int? locationId, int? scaleId, CancellationToken ct = default);
}

public sealed class CameraCaptureTrigger : ICameraCaptureTrigger
{
    private readonly dbContext _db;
    private readonly IHubContext<CameraHub, ICameraClient> _hub;
    private readonly ICameraRegistry _registry;
    private readonly ILogger<CameraCaptureTrigger> _logger;

    public CameraCaptureTrigger(
        dbContext db,
        IHubContext<CameraHub, ICameraClient> hub,
        ICameraRegistry registry,
        ILogger<CameraCaptureTrigger> logger)
    {
        _db = db;
        _hub = hub;
        _registry = registry;
        _logger = logger;
    }

    public async Task FireAsync(string loadNumber, string direction, int? locationId, int? scaleId, CancellationToken ct = default)
    {
        try
        {
            var role = direction switch
            {
                "in"  => "Inbound",
                "out" => "Outbound",
                "bol" => "BOL",
                "tmp" => "TempTicket",
                _     => null
            };
            if (role is null) return;

            // Match: same role, plus location/scale either equals or is unspecified.
            // A location-wide camera (ScaleId=null) covers every load at that
            // location; a scale-bound camera only fires for its own scale.
            var matches = await _db.CameraAssignments.AsNoTracking()
                .Where(a => a.IsActive
                            && a.Role == role
                            && (a.LocationId == null || a.LocationId == locationId)
                            && (a.ScaleId    == null || a.ScaleId    == scaleId))
                .OrderByDescending(a => a.IsPrimary)
                .Select(a => new { a.ServiceId, a.CameraId })
                .ToListAsync(ct);

            if (matches.Count == 0)
            {
                _logger.LogInformation("No camera assigned for direction={Direction} location={LocationId} scale={ScaleId}; skip capture.", direction, locationId, scaleId);
                return;
            }

            foreach (var m in matches)
            {
                // Only send if the service is currently connected — otherwise
                // the command would silently drop into the void.
                if (_registry.Get(m.ServiceId, m.CameraId) is null)
                {
                    _logger.LogWarning("Camera {Service}/{Camera} assigned but not connected; skip capture for load {Load}.", m.ServiceId, m.CameraId, loadNumber);
                    continue;
                }

                await _hub.Clients.Group(CameraHub.ServiceGroupName(m.ServiceId))
                    .CaptureImage(new CaptureCommand(loadNumber, direction, m.CameraId));
            }
        }
        catch (System.Exception ex)
        {
            // Never let a capture failure surface to the caller — the load
            // write already succeeded.
            _logger.LogError(ex, "Capture trigger failed for load {Load} ({Direction}).", loadNumber, direction);
        }
    }
}
