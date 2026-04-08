using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPrintService.Data;
using WebPrintService.Services;

namespace WebPrintService.Controllers;

[ApiController]
public class StatusController : ControllerBase
{
    private readonly PrintDbContext _db;
    private readonly IPrintClient _printer;
    private readonly RestartSignal _restart;

    public StatusController(PrintDbContext db, IPrintClient printer, RestartSignal restart)
    {
        _db = db;
        _printer = printer;
        _restart = restart;
    }

    // ===== HEALTH =====

    [HttpGet("api/status/health")]
    public async Task<IActionResult> Health()
    {
        var printOk = await _printer.IsAvailableAsync();
        var printers = printOk ? await _printer.GetPrintersAsync() : new();
        var printSystem = OperatingSystem.IsWindows() ? "Windows" : "CUPS";
        return Ok(new
        {
            status = printOk ? "ok" : "print_system_unavailable",
            printSystem,
            printSystemAvailable = printOk,
            printerCount = printers.Count,
            printers = printers.Select(p => new { p.PrinterId, p.DisplayName, p.Status, p.Enabled, p.IsDefault })
        });
    }

    // ===== PRINTERS =====

    /// <summary>
    /// List all printers with status.
    /// </summary>
    [HttpGet("api/printers")]
    public async Task<IActionResult> GetPrinters()
    {
        var printers = await _printer.GetPrintersAsync();
        return Ok(printers);
    }

    /// <summary>
    /// Get status of a specific printer.
    /// </summary>
    [HttpGet("api/printers/{printerId}/status")]
    public async Task<IActionResult> GetPrinterStatus(string printerId)
    {
        var status = await _printer.GetPrinterStatusAsync(printerId);
        return Ok(new { printerId, status });
    }

    /// <summary>
    /// Send a test print to a specific printer.
    /// </summary>
    [HttpPost("api/printers/{printerId}/test")]
    public async Task<IActionResult> TestPrint(string printerId)
    {
        var testFile = Path.Combine(Path.GetTempPath(), $"testprint_{Guid.NewGuid():N}.txt");
        await System.IO.File.WriteAllTextAsync(testFile,
            $"Print Service Test Page\n\nPrinter: {printerId}\nDate: {DateTime.Now:yyyy-MM-dd HH:mm:ss}\nPlatform: {(OperatingSystem.IsWindows() ? "Windows" : "Linux/macOS")}\n\nIf you can read this, printing is working!");

        var (success, message) = await _printer.PrintFileAsync(printerId, testFile, "Test Page");

        try { System.IO.File.Delete(testFile); } catch { }

        return Ok(new { success, message });
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
        if (settings == null)
        {
            settings = new ServiceSettings();
            _db.Settings.Add(settings);
        }

        if (update.ServiceId != null) settings.ServiceId = update.ServiceId;
        if (update.ServerUrl != null) settings.ServerUrl = update.ServerUrl;
        if (update.SignalRHub != null) settings.SignalRHub = update.SignalRHub;

        _db.SaveChanges();
        _restart.TriggerRestart();
        return Ok(new { success = true, message = "Settings saved. Service restarting..." });
    }

    // ===== RESTART =====

    /// <summary>
    /// Restart the service (reconnects SignalR, reloads printers).
    /// </summary>
    [HttpPost("api/status/restart")]
    public IActionResult Restart()
    {
        _restart.TriggerRestart();
        return Ok(new { success = true, message = "Service restarting..." });
    }

    // ===== API README =====

    /// <summary>
    /// Returns API documentation with all available endpoints, their methods, parameters, and example responses.
    /// </summary>
    [HttpGet("api/readme")]
    public IActionResult GetReadme()
    {
        var printSystem = OperatingSystem.IsWindows() ? "Windows" : "CUPS (Linux/macOS)";
        return Ok(new
        {
            service = "Web Print Service",
            version = typeof(StatusController).Assembly.GetName().Version?.ToString(3) ?? "0.0.0",
            printSystem,
            swagger = "/swagger",
            endpoints = new object[]
            {
                new {
                    method = "GET", path = "/api/status/health",
                    description = "Health check — returns print system type, availability, and printer count",
                    response = "{ status, printSystem, printSystemAvailable, printerCount, printers[] }"
                },
                new {
                    method = "GET", path = "/api/readme",
                    description = "This endpoint — returns API documentation as JSON",
                    response = "{ service, version, printSystem, endpoints[], signalr[] }"
                },
                new {
                    method = "GET", path = "/api/printers",
                    description = "List all printers with status (idle, paused, error, etc.)",
                    response = "[{ printerId, displayName, status, isDefault, enabled }]"
                },
                new {
                    method = "GET", path = "/api/printers/{printerId}/status",
                    description = "Get detailed status of a specific printer",
                    response = "{ printerId, status }"
                },
                new {
                    method = "POST", path = "/api/printers/{printerId}/test",
                    description = "Send a test page to a specific printer (no body required)",
                    response = "{ success, message }"
                },
                new {
                    method = "GET", path = "/api/settings",
                    description = "Get current service settings (serviceId, serverUrl, signalRHub)",
                    response = "{ id, serviceId, serverUrl, signalRHub }"
                },
                new {
                    method = "PUT", path = "/api/settings",
                    description = "Update service settings — triggers reconnect to web app",
                    body = "{ serviceId?, serverUrl?, signalRHub? }",
                    response = "{ success, message }"
                }
            },
            signalr = new object[]
            {
                new { direction = "Service -> Hub", method = "JoinPrintGroup(serviceId)", description = "Join the PrintClients SignalR group" },
                new { direction = "Service -> Hub", method = "PrintServiceReady(announcement)", description = "Announce available printers on connect/reconnect" },
                new { direction = "Service -> Hub", method = "PrinterListResponse(data)", description = "Respond to printer list request with { serviceId, printers[] }" },
                new { direction = "Service -> Hub", method = "PrintResult(result)", description = "Report print job result { serviceId, success, message }" },
                new { direction = "Service -> Hub", method = "TestPrintResult(result)", description = "Report test print result { serviceId, printerId, success, message }" },
                new { direction = "Hub -> Service", method = "PrintTicket(data)", description = "Print a ticket PDF { ticketId, printerId, type }" },
                new { direction = "Hub -> Service", method = "GetPrinterList", description = "Request updated printer list" },
                new { direction = "Hub -> Service", method = "TestPrint(printerId)", description = "Send a test page to the specified printer" },
                new { direction = "Hub -> Service", method = "ReloadConfig", description = "Restart the service to reload settings" }
            }
        });
    }
}
