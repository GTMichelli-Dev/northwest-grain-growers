#nullable enable
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using GrainManagement.Services.Print.Data;

namespace GrainManagement.Services.Print
{
    /// <summary>
    /// Background worker that connects as a SignalR CLIENT to the GrainManagement PrintHub,
    /// registers as a print device, receives PrintLoadTicket commands, downloads ticket PDFs,
    /// and prints them via CUPS.
    /// </summary>
    public sealed class PrintWorker : BackgroundService
    {
        private readonly ILogger<PrintWorker> _log;
        private readonly CupsClient _cups;
        private readonly RestartSignal _restartSignal;
        private readonly PrintDbContext _db;

        private HubConnection? _connection;

        public PrintWorker(
            ILogger<PrintWorker> log,
            CupsClient cups,
            RestartSignal restartSignal,
            PrintDbContext db)
        {
            _log = log;
            _cups = cups;
            _restartSignal = restartSignal;
            _db = db;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _restartSignal.Reset();

                try
                {
                    var settings = _db.GetSettings();
                    var hubUrl = (settings.ServerUrl?.TrimEnd('/') ?? "http://localhost:5000") + (settings.SignalRHub ?? "/hubs/print");
                    var deviceId = settings.DeviceId ?? Environment.MachineName;
                    var printer = settings.DefaultPrinter ?? "Kiosk";

                    _log.LogInformation("PrintWorker starting. Hub={Hub} DeviceId={DeviceId} Printer={Printer}",
                        hubUrl, deviceId, printer);

                    _connection = new HubConnectionBuilder()
                        .WithUrl(hubUrl)
                        .WithAutomaticReconnect(new ForeverRetryPolicy())
                        .Build();

                    WireEvents(_connection, deviceId, printer, settings.ServerUrl?.TrimEnd('/') ?? "http://localhost:5000");

                    await StartAndRegisterAsync(_connection, deviceId, stoppingToken);

                    _log.LogInformation("PrintWorker connected and listening.");

                    // Wait until cancelled or restart requested
                    await WaitForCancelOrRestartAsync(stoppingToken);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _log.LogError(ex, "PrintWorker error. Restarting in 10 seconds...");
                    try { await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken); }
                    catch (OperationCanceledException) { break; }
                }
                finally
                {
                    if (_connection is not null)
                    {
                        try { await _connection.DisposeAsync(); }
                        catch { /* ignore */ }
                        _connection = null;
                    }
                }
            }

            _log.LogInformation("PrintWorker stopped.");
        }

        private void WireEvents(HubConnection connection, string deviceId, string printer, string baseUrl)
        {
            connection.Reconnecting += ex =>
            {
                _log.LogWarning("SignalR reconnecting... {Message}", ex?.Message);
                return Task.CompletedTask;
            };

            connection.Reconnected += async connectionId =>
            {
                _log.LogInformation("SignalR reconnected. ConnectionId={ConnectionId}. Re-registering...", connectionId);
                await SafeRegisterAsync(connection, deviceId);
            };

            connection.Closed += async ex =>
            {
                _log.LogWarning("SignalR closed. {Message}", ex?.Message);

                // Retry forever until connected or cancellation
                while (true)
                {
                    try
                    {
                        _log.LogInformation("Attempting SignalR restart...");
                        await connection.StartAsync();
                        await SafeRegisterAsync(connection, deviceId);
                        _log.LogInformation("SignalR restarted OK.");
                        break;
                    }
                    catch (ObjectDisposedException)
                    {
                        // Connection was disposed (worker restarting), exit the loop
                        break;
                    }
                    catch (Exception restartEx)
                    {
                        _log.LogWarning("SignalR restart failed: {Message}. Retrying in 10s...", restartEx.Message);
                        await Task.Delay(TimeSpan.FromSeconds(10));
                    }
                }
            };

            // Handle legacy print commands from the server (direct kiosk flow)
            connection.On<JsonElement>("PrintLoadTicket", async payload =>
            {
                if (!TryGetString(payload, "ticket", out var ticket))
                {
                    _log.LogWarning("PrintLoadTicket payload missing 'ticket'. Payload: {Payload}", payload.ToString());
                    return;
                }

                await PrintJobAsync(baseUrl, ticket, "LoadTicket", printer);
            });

            // Handle new-style dispatch from PrintDispatchService — includes job type
            connection.On<JsonElement>("PrintTicket", async payload =>
            {
                if (!TryGetString(payload, "ticketId", out var ticket))
                {
                    _log.LogWarning("PrintTicket payload missing 'ticketId'. Payload: {Payload}", payload.ToString());
                    return;
                }

                TryGetString(payload, "type", out var type);
                if (string.IsNullOrWhiteSpace(type)) type = "LoadTicket";

                await PrintJobAsync(baseUrl, ticket, type, printer);
            });
        }

        private async Task PrintJobAsync(string baseUrl, string ticket, string type, string printer)
        {
            _log.LogInformation("Received print request. Ticket={Ticket} Type={Type}", ticket, type);

            try
            {
                var pdfPath = await DownloadPdfAsync(baseUrl, ticket, type);
                var ok = await _cups.PrintPdfAsync(pdfPath, printer);

                try { File.Delete(pdfPath); }
                catch { /* ignore cleanup failure */ }

                if (ok)
                    _log.LogInformation("Printed OK: {Ticket} ({Type})", ticket, type);
                else
                    _log.LogWarning("Print failed: {Ticket} ({Type})", ticket, type);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Print exception. Ticket={Ticket} Type={Type}", ticket, type);
            }
        }

        private async Task StartAndRegisterAsync(HubConnection connection, string deviceId, CancellationToken ct)
        {
            await connection.StartAsync(ct);
            _log.LogInformation("SignalR connected.");
            await SafeRegisterAsync(connection, deviceId);
        }

        private async Task SafeRegisterAsync(HubConnection connection, string deviceId)
        {
            try
            {
                await connection.InvokeAsync("Register", deviceId);
                _log.LogInformation("Registered device: {DeviceId}", deviceId);
            }
            catch (Exception ex)
            {
                _log.LogWarning("Register failed: {Message}", ex.Message);
            }
        }

        private async Task WaitForCancelOrRestartAsync(CancellationToken ct)
        {
            // Block until either the service is stopping or a restart is signaled
            var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            using var ctr = ct.Register(() => tcs.TrySetResult(false));
            var restartTask = Task.Run(() =>
            {
                _restartSignal.Wait(ct);
                tcs.TrySetResult(true);
            }, ct);

            await tcs.Task;
        }

        private static async Task<string> DownloadPdfAsync(string baseUrl, string ticket, string type = "LoadTicket")
        {
            // Choose endpoint based on job type
            string url = type switch
            {
                "LotLabel" => $"{baseUrl}/api/printjobs/lot-label/{Uri.EscapeDataString(ticket)}/pdf",
                "IntakeWeightSheet" => $"{baseUrl}/api/printjobs/intake-weight-sheet/{Uri.EscapeDataString(ticket)}/pdf",
                _ => $"{baseUrl}/api/printjobs/load-ticket/{Uri.EscapeDataString(ticket)}/pdf",
            };

            var tempDir = Path.GetTempPath();
            Directory.CreateDirectory(tempDir);

            var safeTicket = string.Concat(ticket.Where(ch => !Path.GetInvalidFileNameChars().Contains(ch)));
            var path = Path.Combine(tempDir, $"{type}-{safeTicket}-{Guid.NewGuid():N}.pdf");

            using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
            http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/pdf"));

            using var resp = await http.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            resp.EnsureSuccessStatusCode();

            await using (var fs = File.Create(path))
            await using (var rs = await resp.Content.ReadAsStreamAsync())
            {
                await rs.CopyToAsync(fs);
            }

            // Sanity check: first bytes should look like "%PDF"
            var header = new byte[4];
            await using (var check = File.OpenRead(path))
            {
                var read = await check.ReadAsync(header, 0, header.Length);
                if (read < 4 || header[0] != (byte)'%' || header[1] != (byte)'P' || header[2] != (byte)'D' || header[3] != (byte)'F')
                    throw new InvalidOperationException("Downloaded file does not look like a PDF.");
            }

            return path;
        }

        private static bool TryGetString(JsonElement obj, string name, out string value)
        {
            value = "";
            if (obj.ValueKind != JsonValueKind.Object) return false;

            foreach (var p in obj.EnumerateObject())
            {
                if (string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase))
                {
                    value = p.Value.ValueKind == JsonValueKind.String
                        ? (p.Value.GetString() ?? "")
                        : p.Value.ToString();
                    return !string.IsNullOrWhiteSpace(value);
                }
            }
            return false;
        }
    }

    /// <summary>
    /// SignalR retry policy that retries forever with exponential backoff capped at 60 seconds.
    /// </summary>
    public sealed class ForeverRetryPolicy : IRetryPolicy
    {
        private static readonly TimeSpan[] BackoffSchedule = new[]
        {
            TimeSpan.Zero,
            TimeSpan.FromSeconds(2),
            TimeSpan.FromSeconds(5),
            TimeSpan.FromSeconds(10),
            TimeSpan.FromSeconds(30),
            TimeSpan.FromSeconds(60)
        };

        public TimeSpan? NextRetryDelay(RetryContext retryContext)
        {
            var index = Math.Min(retryContext.PreviousRetryCount, BackoffSchedule.Length - 1);
            return BackoffSchedule[index];
        }
    }
}
