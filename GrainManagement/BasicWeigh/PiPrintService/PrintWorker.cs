using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using WebPrintService.Data;
using WebPrintService.Services;
using System.Text.Json;

namespace WebPrintService;

/// <summary>
/// Background worker that connects to the web app via SignalR,
/// listens for print commands, and prints via CUPS (Linux/macOS) or Windows Print.
/// </summary>
public class PrintWorker : BackgroundService
{
    private readonly IServiceProvider _sp;
    private readonly ILogger<PrintWorker> _log;
    private readonly IPrintClient _printer;
    private readonly RestartSignal _restart;
    private readonly IHttpClientFactory _httpFactory;
    private HubConnection? _connection;
    private string _serviceId = "default";

    public PrintWorker(IServiceProvider sp, ILogger<PrintWorker> log, IPrintClient printer,
        RestartSignal restart, IHttpClientFactory httpFactory)
    {
        _sp = sp;
        _log = log;
        _printer = printer;
        _restart = restart;
        _httpFactory = httpFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            try
            {
                _restart.Reset();
                var (serverUrl, hubPath) = await LoadSettings();

                _log.LogInformation("Connecting to {Url}{Hub}", serverUrl, hubPath);

                _connection = new HubConnectionBuilder()
                    .WithUrl($"{serverUrl}{hubPath}")
                    .WithAutomaticReconnect(new ForeverRetryPolicy())
                    .Build();

                _connection.Reconnecting += _ =>
                {
                    _log.LogWarning("Connection lost. Reconnecting...");
                    return Task.CompletedTask;
                };

                _connection.Reconnected += async _ =>
                {
                    _log.LogInformation("Reconnected. Rejoining print groups...");
                    await JoinGroups();
                    await AnnouncePrinters();
                };

                RegisterHandlers();

                await _connection.StartAsync(ct);
                _log.LogInformation("Connected. Joining print groups (ServiceId={ServiceId})...", _serviceId);
                await JoinGroups();
                await AnnouncePrinters();

                // Wait for restart signal or cancellation
                await Task.Run(() => _restart.WaitForRestart(Timeout.InfiniteTimeSpan), ct);
                _log.LogInformation("Restart triggered. Reconnecting...");

                try { await _connection.DisposeAsync(); } catch { }
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested) { break; }
            catch (Exception ex)
            {
                _log.LogWarning("Connection error: {Msg}. Retrying in 5s...", ex.Message);
                try { if (_connection != null) await _connection.DisposeAsync(); } catch { }
                await Task.Delay(5000, ct);
            }
        }
    }

    private async Task<(string serverUrl, string hubPath)> LoadSettings()
    {
        using var scope = _sp.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<PrintDbContext>();
        var settings = await db.Settings.OrderBy(s => s.Id).FirstOrDefaultAsync();
        if (settings != null)
        {
            _serviceId = settings.ServiceId;
            return (settings.ServerUrl, settings.SignalRHub);
        }
        return ("http://localhost:5110", "/scaleHub");
    }

    private async Task JoinGroups()
    {
        await _connection!.InvokeAsync("JoinPrintGroup", _serviceId);
    }

    private async Task AnnouncePrinters()
    {
        if (_connection?.State != HubConnectionState.Connected) return;

        try
        {
            var printers = await _printer.GetPrintersAsync();
            await _connection.InvokeAsync("PrintServiceReady", new
            {
                serviceId = _serviceId,
                printerCount = printers.Count,
                printers = printers.Select(p => new
                {
                    p.PrinterId,
                    p.DisplayName,
                    p.Status,
                    p.IsDefault,
                    p.Enabled
                })
            });
            _log.LogInformation("Announced {Count} printer(s) to web app.", printers.Count);
        }
        catch (Exception ex)
        {
            _log.LogWarning("Failed to announce printers: {Msg}", ex.Message);
        }
    }

    private void RegisterHandlers()
    {
        // Print ticket command from web app
        _connection!.On<JsonElement>("PrintTicket", async (data) =>
        {
            try
            {
                var ticketId = data.TryGetProperty("ticketId", out var tid) ? tid.ToString() : "";
                var printerId = data.TryGetProperty("printerId", out var pid) ? pid.ToString() : "";
                var type = data.TryGetProperty("type", out var t) ? t.GetString() ?? "" : "";

                _log.LogInformation("Received PrintTicket: ticket={Ticket} printer={Printer} type={Type}",
                    ticketId, printerId, type);

                if (string.IsNullOrEmpty(ticketId))
                {
                    await ReportPrintResult(false, "No ticket ID provided");
                    return;
                }

                // Determine which CUPS printer to use
                string cupsPrinterId;
                if (!string.IsNullOrEmpty(printerId))
                {
                    cupsPrinterId = printerId;
                }
                else
                {
                    // Use default printer
                    var printers = await _printer.GetPrintersAsync();
                    var defaultPrinter = printers.FirstOrDefault(p => p.IsDefault) ?? printers.FirstOrDefault();
                    if (defaultPrinter == null)
                    {
                        await ReportPrintResult(false, "No printers available");
                        return;
                    }
                    cupsPrinterId = defaultPrinter.PrinterId;
                }

                // Build PDF URL
                var settings = await GetSettings();
                var pdfUrl = $"{settings.ServerUrl}/api/ticket/{ticketId}/pdf";

                _log.LogInformation("Printing ticket {Ticket} to {Printer} from {Url}", ticketId, cupsPrinterId, pdfUrl);

                var http = _httpFactory.CreateClient();
                var (success, message) = await _printer.PrintFromUrlAsync(cupsPrinterId, pdfUrl, $"Ticket-{ticketId}", http);

                await ReportPrintResult(success, success ? $"Ticket {ticketId} sent to {cupsPrinterId}" : message);
            }
            catch (Exception ex)
            {
                _log.LogError("PrintTicket failed: {Msg}", ex.Message);
                await ReportPrintResult(false, ex.Message);
            }
        });

        // Request printer list
        _connection!.On("GetPrinterList", async () =>
        {
            try
            {
                var printers = await _printer.GetPrintersAsync();
                await _connection!.InvokeAsync("PrinterListResponse", new
                {
                    serviceId = _serviceId,
                    printers = printers.Select(p => new
                    {
                        p.PrinterId,
                        p.DisplayName,
                        p.Status,
                        p.IsDefault,
                        p.Enabled
                    })
                });
            }
            catch (Exception ex)
            {
                _log.LogWarning("GetPrinterList failed: {Msg}", ex.Message);
            }
        });

        // Reload config
        _connection!.On("ReloadConfig", () =>
        {
            _log.LogInformation("Received ReloadConfig. Restarting...");
            _restart.TriggerRestart();
        });

        // Test print
        _connection!.On<string>("TestPrint", async (printerId) =>
        {
            _log.LogInformation("Received TestPrint for printer: {Printer}", printerId);
            try
            {
                // Create a simple test page
                var testFile = Path.Combine(Path.GetTempPath(), $"testprint_{Guid.NewGuid():N}.txt");
                await File.WriteAllTextAsync(testFile, $"Web Print Service Test Page\n\nServiceId: {_serviceId}\nPrinter: {printerId}\nDate: {DateTime.Now:yyyy-MM-dd HH:mm:ss}\n\nIf you can read this, printing is working!");

                var (success, message) = await _printer.PrintFileAsync(printerId, testFile, "Test Page");

                try { File.Delete(testFile); } catch { }

                await _connection!.InvokeAsync("TestPrintResult", new
                {
                    serviceId = _serviceId,
                    printerId,
                    success,
                    message
                });
            }
            catch (Exception ex)
            {
                await _connection!.InvokeAsync("TestPrintResult", new
                {
                    serviceId = _serviceId,
                    printerId,
                    success = false,
                    message = ex.Message
                });
            }
        });
    }

    private async Task ReportPrintResult(bool success, string message)
    {
        if (_connection?.State == HubConnectionState.Connected)
        {
            try
            {
                await _connection.InvokeAsync("PrintResult", new
                {
                    serviceId = _serviceId,
                    success,
                    message
                });
            }
            catch { }
        }
    }

    private async Task<ServiceSettings> GetSettings()
    {
        using var scope = _sp.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<PrintDbContext>();
        return await db.Settings.OrderBy(s => s.Id).FirstAsync();
    }
}

/// <summary>
/// Retry forever with exponential backoff: 2s, 5s, 10s, 30s, 30s, ...
/// </summary>
public class ForeverRetryPolicy : IRetryPolicy
{
    private static readonly TimeSpan[] Delays = { TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(30) };

    public TimeSpan? NextRetryDelay(RetryContext retryContext)
    {
        var idx = Math.Min(retryContext.PreviousRetryCount, Delays.Length - 1);
        return Delays[idx];
    }
}
