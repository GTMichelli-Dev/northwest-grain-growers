using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TempTicketKioskService.Models;
using TempTicketKioskService.Services;

namespace TempTicketKioskService.Controllers;

/// <summary>
/// Local HTTP surface for the kiosk service:
///   GET  /api/status         — service health + current config snapshot
///   POST /api/test/press     — simulate a button press (useful on Windows
///                              dev hosts where there's no GPIO)
///   GET  /api/config         — read the current Kiosk options
/// </summary>
[ApiController]
[Route("api")]
public sealed class KioskController : ControllerBase
{
    private readonly IOptionsMonitor<KioskOptions> _opt;
    private readonly KioskHttpClient _client;
    private readonly GpioMonitor _monitor;
    private readonly IHostEnvironment _env;

    public KioskController(
        IOptionsMonitor<KioskOptions> opt,
        KioskHttpClient client,
        GpioMonitor monitor,
        IHostEnvironment env)
    {
        _opt = opt;
        _client = client;
        _monitor = monitor;
        _env = env;
    }

    [HttpGet("status")]
    public IActionResult Status()
    {
        var opt = _opt.CurrentValue;
        return Ok(new
        {
            service     = "TempTicketKioskService",
            version     = typeof(KioskController).Assembly.GetName().Version?.ToString() ?? "0.0.0",
            environment = _env.EnvironmentName,
            gpio = new
            {
                supported    = OperatingSystem.IsLinux(),
                pin          = opt.GpioPin,
                pullMode     = opt.PullMode,
                debounceMs   = opt.DebounceMs,
                currentValue = _monitor.ReadPin()?.ToString() ?? "n/a",
                lastPressUtc = _monitor.LastEdgeUtc,
            },
            kiosk = new
            {
                opt.KioskId,
                opt.ScaleId,
                opt.PrinterTarget,
                opt.ServerUrl,
            }
        });
    }

    /// <summary>Simulate a button press. Use this from Swagger on Windows / macOS hosts.</summary>
    [HttpPost("test/press")]
    public async Task<IActionResult> TestPress(CancellationToken ct)
    {
        var result = await _client.PressAsync(source: "test", ct);
        return result.Success
            ? Ok(new { ok = true })
            : StatusCode(502, new { ok = false, error = result.Error });
    }

    [HttpGet("config")]
    public IActionResult GetConfig()
    {
        // appsettings.json is authoritative — no SQLite store for this
        // service since one Pi = one kiosk = one config row.
        return Ok(_opt.CurrentValue);
    }
}
