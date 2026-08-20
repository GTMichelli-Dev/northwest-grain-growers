using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using BasicWeigh.Web.Data;
using BasicWeigh.Web.Hubs;
using BasicWeigh.Web.Models;
using BasicWeigh.Web.Services;

namespace BasicWeigh.Web.Controllers;

public class HomeController : Controller
{
    private readonly IScaleService _scaleService;
    private readonly ScaleDbContext _db;
    private readonly PrintQueueService _printQueue;
    private readonly AppSetupCache _setupCache;
    private readonly IHubContext<ScaleHub> _hub;
    private readonly ScaleWeightStore _weightStore;

    public HomeController(IScaleService scaleService, ScaleDbContext db, PrintQueueService printQueue, AppSetupCache setupCache, IHubContext<ScaleHub> hub, ScaleWeightStore weightStore)
    {
        _scaleService = scaleService;
        _db = db;
        _printQueue = printQueue;
        _setupCache = setupCache;
        _hub = hub;
        _weightStore = weightStore;
    }

    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// Get weight from the configured scale (Setup > ScaleId).
    /// Or pass serviceId and scaleId to get a specific scale.
    /// </summary>
    [HttpGet("api/scale/weight")]
    public IActionResult GetWeight([FromQuery] string? serviceId = null, [FromQuery] string? scaleId = null)
    {
        var setup = _setupCache.Get();

        // In demo mode, use the simulated service
        if (setup.DemoMode)
        {
            return Json(new
            {
                weight = _scaleService.GetCurrentWeight(),
                motion = _scaleService.IsInMotion(),
                ok = _scaleService.IsConnected(),
                error = _scaleService.HasError(),
                comError = (_scaleService is SimulatedScaleService sim2) && sim2.HasComError()
            });
        }

        // Determine which scale to query
        string lookupId;
        if (!string.IsNullOrEmpty(serviceId) && !string.IsNullOrEmpty(scaleId))
        {
            lookupId = $"{serviceId}:{scaleId}";
        }
        else if (!string.IsNullOrEmpty(scaleId))
        {
            lookupId = scaleId;
        }
        else
        {
            lookupId = setup.ScaleId ?? "";
        }

        if (string.IsNullOrEmpty(lookupId))
        {
            return Json(new { weight = 0, motion = false, ok = false, error = true, comError = true });
        }

        var reading = _weightStore.Get(lookupId);
        if (reading == null)
        {
            return Json(new { weight = 0, motion = false, ok = false, error = true, comError = true });
        }

        return Json(new
        {
            weight = reading.Weight,
            motion = reading.Motion,
            ok = reading.Ok,
            error = !reading.Ok,
            comError = reading.ComError,
            scaleId = reading.ScaleId,
            serviceId = reading.ServiceId
        });
    }

    [HttpPost("api/scale/simulate")]
    public IActionResult Simulate([FromBody] SimulateRequest request)
    {
        var setup = _setupCache.Get();
        if (!setup.DemoMode)
            return BadRequest(new { success = false, message = "Not in demo mode. Enable Demo Mode in Setup to use the simulator." });

        if (_scaleService is SimulatedScaleService sim)
        {
            sim.SetWeight(request.Weight);
            sim.SetMotion(request.Motion);
            sim.SetError(request.Error);
            return Json(new { success = true });
        }
        return BadRequest(new { success = false, message = "Scale service is not a simulator." });
    }

    /// <summary>
    /// Called by the physical scale indicator (or Pi bridge) to push weight readings.
    /// Only works when DemoMode is OFF. Updates the multi-scale weight store.
    /// Returns any pending print job in the response.
    /// </summary>
    [HttpPost("api/scale/weight")]
    public IActionResult UpdateWeight([FromBody] ScaleWeightRequest request)
    {
        var setup = _setupCache.Get();
        if (setup.DemoMode)
            return BadRequest(new { success = false, message = "System is in demo mode. Use api/scale/simulate instead." });

        var scaleId = request.ScaleId ?? "default";
        var serviceId = request.ServiceId ?? "api";

        // Update the multi-scale weight store
        _weightStore.Update(scaleId, serviceId, request.Weight, request.Motion, !request.Error,
            request.Error ? "Error" : (request.Motion ? "Motion" : "Ok"));

        // Check for pending print jobs (Scale mode only — RemotePrinter uses SignalR)
        if (setup.RemotePrintMode == "Scale" && _printQueue.TryDequeue(out var ticketId) && ticketId != null)
        {
            return Json(new
            {
                success = true,
                print = new { ticketId, pdfUrl = $"/api/ticket/{ticketId}/pdf" }
            });
        }

        return Json(new { success = true });
    }

    public class ScaleWeightRequest
    {
        public string? ScaleId { get; set; }
        public string? ServiceId { get; set; }
        public int Weight { get; set; }
        public bool Motion { get; set; }
        public bool Error { get; set; }
    }

    public class SimulateRequest
    {
        public int Weight { get; set; }
        public bool Motion { get; set; }
        public bool Error { get; set; }
    }

    /// <summary>
    /// Zero the scale via SignalR to the ScaleReaderService.
    /// The service sends the zero command to the physical scale indicator.
    /// </summary>
    /// <summary>
    /// Zero the scale via SignalR. Pass serviceId/scaleId or uses the configured scale from Setup.
    /// </summary>
    [HttpPost("api/scale/zero")]
    public async Task<IActionResult> ZeroScale([FromQuery] string? serviceId = null, [FromQuery] string? scaleId = null)
    {
        var setup = _setupCache.Get();

        // Determine target scale
        string targetScaleId;
        string targetServiceId;

        if (!string.IsNullOrEmpty(serviceId) && !string.IsNullOrEmpty(scaleId))
        {
            targetServiceId = serviceId;
            targetScaleId = scaleId;
        }
        else if (!string.IsNullOrEmpty(setup.ScaleId) && setup.ScaleId.Contains(':'))
        {
            var parts = setup.ScaleId.Split(':', 2);
            targetServiceId = parts[0];
            targetScaleId = parts[1];
        }
        else
        {
            return Json(new { success = false, message = "No scale configured. Set the scale in Setup > Scales." });
        }

        // Send to the specific scale service group
        await _hub.Clients.Group($"Scale_{targetServiceId}").SendAsync("ZeroScale", targetScaleId);
        return Json(new { success = true, message = $"Zero command sent to {targetServiceId}:{targetScaleId}." });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
