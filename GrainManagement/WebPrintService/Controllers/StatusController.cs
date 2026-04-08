using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebPrintService.Services;

namespace WebPrintService.Controllers;

[ApiController]
public class StatusController : ControllerBase
{
    private readonly IPrintClient _printer;
    private readonly RestartSignal _restart;
    private readonly PrintServiceOptions _options;

    public StatusController(IPrintClient printer, RestartSignal restart, IOptions<PrintServiceOptions> options)
    {
        _printer = printer;
        _restart = restart;
        _options = options.Value;
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

    [HttpGet("api/printers")]
    public async Task<IActionResult> GetPrinters()
    {
        var printers = await _printer.GetPrintersAsync();
        return Ok(printers);
    }

    [HttpGet("api/printers/{printerId}/status")]
    public async Task<IActionResult> GetPrinterStatus(string printerId)
    {
        var status = await _printer.GetPrinterStatusAsync(printerId);
        return Ok(new { printerId, status });
    }

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

    // ===== SETTINGS (read-only — edit appsettings.json to change) =====

    [HttpGet("api/settings")]
    public IActionResult GetSettings()
    {
        return Ok(new
        {
            _options.ServiceId,
            _options.ServerUrls,
            _options.SignalRHub,
            _options.Port,
            hubUrls = _options.HubUrls
        });
    }

    // ===== RESTART =====

    [HttpPost("api/status/restart")]
    public IActionResult Restart()
    {
        _restart.TriggerRestart();
        return Ok(new { success = true, message = "Service restarting..." });
    }

    // ===== API README =====

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
                new { method = "GET", path = "/api/status/health", description = "Health check" },
                new { method = "GET", path = "/api/printers", description = "List all printers with status" },
                new { method = "GET", path = "/api/printers/{printerId}/status", description = "Get specific printer status" },
                new { method = "POST", path = "/api/printers/{printerId}/test", description = "Send test page to a printer" },
                new { method = "GET", path = "/api/settings", description = "View current settings (from appsettings.json)" },
                new { method = "POST", path = "/api/status/restart", description = "Restart SignalR connections" },
            },
            signalr = new object[]
            {
                new { direction = "Service -> Hub", method = "JoinPrintGroup(serviceId)", description = "Join the PrintClients SignalR group" },
                new { direction = "Service -> Hub", method = "PrintServiceReady(announcement)", description = "Announce available printers" },
                new { direction = "Service -> Hub", method = "PrinterListResponse(data)", description = "Respond to printer list request" },
                new { direction = "Service -> Hub", method = "PrintResult(result)", description = "Report print job result" },
                new { direction = "Service -> Hub", method = "TestPrintResult(result)", description = "Report test print result" },
                new { direction = "Hub -> Service", method = "PrintTicket(data)", description = "Print a ticket PDF" },
                new { direction = "Hub -> Service", method = "GetPrinterList", description = "Request printer list" },
                new { direction = "Hub -> Service", method = "TestPrint(printerId)", description = "Send test page" },
                new { direction = "Hub -> Service", method = "ReloadConfig", description = "Restart the service" }
            }
        });
    }
}
