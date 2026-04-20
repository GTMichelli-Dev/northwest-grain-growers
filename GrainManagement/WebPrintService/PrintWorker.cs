using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using WebPrintService.Services;
using System.Text.Json;

namespace WebPrintService;

/// <summary>
/// Background worker that connects to one or more web app servers via SignalR,
/// listens for print commands, and prints via CUPS (Linux/macOS) or Windows Print.
/// Supports multiple ServerUrls — each gets its own independent SignalR connection,
/// matching the ScaleReaderService pattern.
/// </summary>
public class PrintWorker : BackgroundService
{
    private readonly ILogger<PrintWorker> _log;
    private readonly IPrintClient _printer;
    private readonly RestartSignal _restart;
    private readonly IHttpClientFactory _httpFactory;
    private readonly PrintServiceOptions _options;
    private readonly List<HubConnection> _connections = new();

    public PrintWorker(
        ILogger<PrintWorker> log,
        IPrintClient printer,
        RestartSignal restart,
        IHttpClientFactory httpFactory,
        IOptions<PrintServiceOptions> options)
    {
        _log = log;
        _printer = printer;
        _restart = restart;
        _httpFactory = httpFactory;
        _options = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            try
            {
                _restart.Reset();

                var hubUrls = _options.HubUrls;
                var version = typeof(PrintWorker).Assembly.GetName().Version?.ToString(3) ?? "0.0.0";

                _log.LogInformation("WebPrintService v{Version} — ServiceId={ServiceId}, {Count} server(s): {Urls}",
                    version, _options.ServiceId, hubUrls.Count, string.Join(", ", hubUrls));

                // Build a connection for each hub URL
                _connections.Clear();
                foreach (var hubUrl in hubUrls)
                {
                    var conn = new HubConnectionBuilder()
                        .WithUrl(hubUrl)
                        .WithAutomaticReconnect(new ForeverRetryPolicy())
                        .Build();

                    RegisterConnectionEvents(conn, hubUrl);
                    RegisterHandlers(conn);
                    _connections.Add(conn);
                }

                // Connect all hubs independently (one failing doesn't block others)
                var connectTasks = _connections.Select((conn, i) =>
                    ConnectWithRetryAsync(conn, hubUrls[i], ct));
                await Task.WhenAll(connectTasks);

                // Wait for restart signal or cancellation
                await Task.Run(() => _restart.WaitForRestart(Timeout.InfiniteTimeSpan), ct);
                _log.LogInformation("Restart triggered. Reconnecting all hubs...");

                await DisposeAllConnections();
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested) { break; }
            catch (Exception ex)
            {
                _log.LogWarning("PrintWorker error: {Msg}. Retrying in 5s...", ex.Message);
                await DisposeAllConnections();
                await Task.Delay(5000, ct);
            }
        }

        await DisposeAllConnections();
    }

    private async Task ConnectWithRetryAsync(HubConnection conn, string hubUrl, CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            try
            {
                _log.LogInformation("Connecting to {HubUrl}...", hubUrl);
                await conn.StartAsync(ct);
                _log.LogInformation("Connected to {HubUrl}. Joining print group (ServiceId={ServiceId})...",
                    hubUrl, _options.ServiceId);
                await JoinGroups(conn);
                await AnnouncePrinters(conn);
                return;
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested) { return; }
            catch (Exception ex)
            {
                _log.LogWarning("Connection to {HubUrl} failed: {Msg}. Retrying in 5s...", hubUrl, ex.Message);
                await Task.Delay(5000, ct);
            }
        }
    }

    private void RegisterConnectionEvents(HubConnection conn, string hubUrl)
    {
        conn.Reconnecting += _ =>
        {
            _log.LogWarning("Connection to {HubUrl} lost. Reconnecting...", hubUrl);
            return Task.CompletedTask;
        };

        conn.Reconnected += async _ =>
        {
            _log.LogInformation("Reconnected to {HubUrl}. Rejoining print groups...", hubUrl);
            await JoinGroups(conn);
            await AnnouncePrinters(conn);
        };
    }

    private async Task JoinGroups(HubConnection conn)
    {
        if (conn.State != HubConnectionState.Connected) return;
        try
        {
            await conn.InvokeAsync("JoinPrintGroup", _options.ServiceId);
        }
        catch (Exception ex)
        {
            _log.LogWarning("JoinPrintGroup failed: {Msg}", ex.Message);
        }
    }

    private async Task AnnouncePrinters(HubConnection conn)
    {
        if (conn.State != HubConnectionState.Connected) return;

        try
        {
            var printers = await _printer.GetPrintersAsync();
            await conn.InvokeAsync("PrintServiceReady", new
            {
                serviceId = _options.ServiceId,
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
            _log.LogInformation("Announced {Count} printer(s).", printers.Count);
        }
        catch (Exception ex)
        {
            _log.LogWarning("Failed to announce printers: {Msg}", ex.Message);
        }
    }

    private void RegisterHandlers(HubConnection conn)
    {
        // Print ticket command from web app
        conn.On<JsonElement>("PrintTicket", async (data) =>
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
                    await ReportPrintResult(conn, false, "No ticket ID provided");
                    return;
                }

                // Determine which printer to use
                string cupsPrinterId;
                if (!string.IsNullOrEmpty(printerId))
                {
                    cupsPrinterId = printerId;
                }
                else
                {
                    var printers = await _printer.GetPrintersAsync();
                    var defaultPrinter = printers.FirstOrDefault(p => p.IsDefault) ?? printers.FirstOrDefault();
                    if (defaultPrinter == null)
                    {
                        await ReportPrintResult(conn, false, "No printers available");
                        return;
                    }
                    cupsPrinterId = defaultPrinter.PrinterId;
                }

                // Build PDF URL from the first configured server — endpoint depends on job type
                var serverUrl = _options.ServerUrls.FirstOrDefault()?.TrimEnd('/') ?? "http://localhost:5000";
                string pdfUrl = type switch
                {
                    "LotLabel"           => $"{serverUrl}/api/printjobs/lot-label/{ticketId}/pdf",
                    "IntakeWeightSheet"  => $"{serverUrl}/api/printjobs/intake-weight-sheet/{ticketId}/pdf",
                    _                    => $"{serverUrl}/api/printjobs/load-ticket/{ticketId}/pdf",
                };
                var jobLabel = type switch
                {
                    "LotLabel"          => $"LotLabel-{ticketId}",
                    "IntakeWeightSheet" => $"WeightSheet-{ticketId}",
                    _                   => $"Ticket-{ticketId}",
                };

                _log.LogInformation("Printing {Type} {Ticket} to {Printer} from {Url}", type, ticketId, cupsPrinterId, pdfUrl);

                var http = _httpFactory.CreateClient();
                var (success, message) = await _printer.PrintFromUrlAsync(cupsPrinterId, pdfUrl, jobLabel, http);

                await ReportPrintResult(conn, success, success ? $"Ticket {ticketId} sent to {cupsPrinterId}" : message);
            }
            catch (Exception ex)
            {
                _log.LogError("PrintTicket failed: {Msg}", ex.Message);
                await ReportPrintResult(conn, false, ex.Message);
            }
        });

        // Request printer list
        conn.On("GetPrinterList", async () =>
        {
            try
            {
                var printers = await _printer.GetPrintersAsync();
                await conn.InvokeAsync("PrinterListResponse", new
                {
                    serviceId = _options.ServiceId,
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
        conn.On("ReloadConfig", () =>
        {
            _log.LogInformation("Received ReloadConfig. Restarting...");
            _restart.TriggerRestart();
        });

        // Test print — downloads test page PDF from the web server (same DevExpress report as real tickets)
        conn.On<string>("TestPrint", async (printerId) =>
        {
            _log.LogInformation("Received TestPrint for printer: {Printer}", printerId);
            try
            {
                var serverUrl = _options.ServerUrls.FirstOrDefault()?.TrimEnd('/') ?? "http://localhost:5000";
                var testPdfUrl = $"{serverUrl}/api/printjobs/test-page/pdf";

                _log.LogInformation("Downloading test page from {Url}", testPdfUrl);

                var http = _httpFactory.CreateClient();
                var (success, message) = await _printer.PrintFromUrlAsync(printerId, testPdfUrl, "Test Page", http);

                await conn.InvokeAsync("TestPrintResult", new
                {
                    serviceId = _options.ServiceId,
                    printerId,
                    success,
                    message
                });
            }
            catch (Exception ex)
            {
                await conn.InvokeAsync("TestPrintResult", new
                {
                    serviceId = _options.ServiceId,
                    printerId,
                    success = false,
                    message = ex.Message
                });
            }
        });
    }

    private async Task ReportPrintResult(HubConnection conn, bool success, string message)
    {
        if (conn.State == HubConnectionState.Connected)
        {
            try
            {
                await conn.InvokeAsync("PrintResult", new
                {
                    serviceId = _options.ServiceId,
                    success,
                    message
                });
            }
            catch { }
        }
    }

    private async Task DisposeAllConnections()
    {
        foreach (var conn in _connections)
        {
            try { await conn.DisposeAsync(); } catch { }
        }
        _connections.Clear();
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
