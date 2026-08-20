using System.Diagnostics;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace KioskPrintAgent;

public class PrintService : BackgroundService
{
    private readonly ILogger<PrintService> _logger;
    private readonly PrintAgentOptions _options;
    private readonly IHttpClientFactory _httpClientFactory;
    private HubConnection? _connection;

    public PrintService(
        ILogger<PrintService> logger,
        IOptions<PrintAgentOptions> options,
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _options = options.Value;
        _httpClientFactory = httpClientFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Ensure temp directory exists
        Directory.CreateDirectory(_options.TempDir);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ConnectAndListen(stoppingToken);
            }
            catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogError(ex, "Connection lost. Retrying in 5 seconds...");
                await Task.Delay(5000, stoppingToken);
            }
        }
    }

    private async Task ConnectAndListen(CancellationToken stoppingToken)
    {
        var hubUrl = _options.ServerUrl.TrimEnd('/') + "/scaleHub";
        _logger.LogInformation("Connecting to {HubUrl}", hubUrl);

        // Signalled when the hub permanently closes — drives the outer
        // while loop to rebuild the connection. Without this the
        // bounded retry overload gave up after ~47s and the worker sat
        // dead on Task.Delay(Infinite). Now we retry forever AND the
        // outer loop pops back to start a new connection if the
        // policy ever exits.
        var connectionLost = new TaskCompletionSource();

        _connection = new HubConnectionBuilder()
            .WithUrl(hubUrl)
            .WithAutomaticReconnect(new ForeverRetryPolicy(_logger, hubUrl))
            .Build();

        _connection.Reconnecting += error =>
        {
            _logger.LogWarning("Connection lost. Reconnecting...");
            return Task.CompletedTask;
        };

        _connection.Reconnected += async connectionId =>
        {
            _logger.LogInformation("Reconnected. Rejoining print group...");
            await _connection.InvokeAsync("JoinPrintGroup", stoppingToken);
        };

        _connection.Closed += error =>
        {
            if (error != null)
                _logger.LogError(error, "Connection closed with error");
            else
                _logger.LogInformation("Connection closed");
            connectionLost.TrySetResult();
            return Task.CompletedTask;
        };

        // Handle print requests (only print if PrinterId matches or is 0 = broadcast to all)
        _connection.On<PrintRequest>("PrintTicket", async request =>
        {
            if (request.PrinterId != 0 && request.PrinterId != _options.PrinterId)
            {
                _logger.LogInformation("Ignoring print request for PrinterId={PrinterId} (this agent is {MyId})",
                    request.PrinterId, _options.PrinterId);
                return;
            }
            _logger.LogInformation("Print request received: Ticket={TicketId}, Type={Type}, PrinterId={PrinterId}",
                request.TicketId, request.Type, request.PrinterId);
            await PrintTicketAsync(request);
        });

        await _connection.StartAsync(stoppingToken);
        _logger.LogInformation("Connected to server. Joining print group...");

        // Join the PrintClients group
        await _connection.InvokeAsync("JoinPrintGroup", stoppingToken);
        _logger.LogInformation("Joined print group. Waiting for print requests...");

        // Block until either the host is shutting down or the hub
        // signals Closed. Returning then pops the outer while loop into
        // its 5-second retry sleep, after which we rebuild from scratch.
        using var ctReg = stoppingToken.Register(() => connectionLost.TrySetResult());
        await connectionLost.Task;
        stoppingToken.ThrowIfCancellationRequested();

        _logger.LogWarning("SignalR connection closed; outer loop will rebuild.");
        try { await _connection.DisposeAsync(); } catch { }
    }

    private async Task PrintTicketAsync(PrintRequest request)
    {
        string? tempFile = null;
        try
        {
            // Download PDF from server
            var client = _httpClientFactory.CreateClient("TicketApi");
            var response = await client.GetAsync($"/api/ticket/{Uri.EscapeDataString(request.TicketId)}/pdf");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to download ticket PDF. Status: {Status}", response.StatusCode);
                return;
            }

            // Save to temp file
            tempFile = Path.Combine(_options.TempDir, $"ticket-{request.TicketId}-{Guid.NewGuid():N}.pdf");
            var pdfBytes = await response.Content.ReadAsByteArrayAsync();
            await File.WriteAllBytesAsync(tempFile, pdfBytes);

            _logger.LogInformation("Downloaded ticket PDF ({Bytes} bytes) to {Path}", pdfBytes.Length, tempFile);

            // Print via CUPS using lp command
            await PrintWithCups(tempFile);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error printing ticket {TicketId}", request.TicketId);
        }
        finally
        {
            // Clean up temp file
            if (tempFile != null && File.Exists(tempFile))
            {
                try { File.Delete(tempFile); }
                catch { /* ignore cleanup errors */ }
            }
        }
    }

    private async Task PrintWithCups(string filePath)
    {
        var args = new List<string>();

        // Printer selection (empty = default CUPS printer)
        if (!string.IsNullOrWhiteSpace(_options.PrinterName))
        {
            args.Add("-d");
            args.Add(_options.PrinterName);
        }

        // Number of copies
        if (_options.PrintCopies > 1)
        {
            args.Add("-n");
            args.Add(_options.PrintCopies.ToString());
        }

        args.Add(filePath);

        var startInfo = new ProcessStartInfo
        {
            FileName = "lp",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        foreach (var arg in args)
            startInfo.ArgumentList.Add(arg);

        _logger.LogInformation("Executing: lp {Args}", string.Join(" ", args));

        using var process = Process.Start(startInfo);
        if (process == null)
        {
            _logger.LogError("Failed to start lp process");
            return;
        }

        var stdout = await process.StandardOutput.ReadToEndAsync();
        var stderr = await process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();

        if (process.ExitCode == 0)
        {
            _logger.LogInformation("Print job submitted successfully. {Output}", stdout.Trim());
        }
        else
        {
            _logger.LogError("Print failed (exit code {Code}). Error: {Error}", process.ExitCode, stderr.Trim());
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_connection != null)
        {
            await _connection.DisposeAsync();
        }
        await base.StopAsync(cancellationToken);
    }
}

public class PrintRequest
{
    public string TicketId { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public int PrinterId { get; set; }
}

/// <summary>
/// SignalR retry policy that retries forever with a 2/5/10/30s backoff
/// (last entry holds for every subsequent retry). Each call logs a
/// warning so a long outage produces visible heartbeat-style "still
/// down, retrying in Xs" lines instead of one Reconnecting warning and
/// then silence.
/// </summary>
public class ForeverRetryPolicy : IRetryPolicy
{
    private static readonly TimeSpan[] Delays =
    {
        TimeSpan.FromSeconds(2),
        TimeSpan.FromSeconds(5),
        TimeSpan.FromSeconds(10),
        TimeSpan.FromSeconds(30),
    };

    private readonly ILogger _log;
    private readonly string _hubUrl;

    public ForeverRetryPolicy(ILogger log, string hubUrl)
    {
        _log = log;
        _hubUrl = hubUrl;
    }

    public TimeSpan? NextRetryDelay(RetryContext retryContext)
    {
        var idx = (int)Math.Min(retryContext.PreviousRetryCount, Delays.Length - 1);
        var delay = Delays[idx];

        _log.LogWarning(
            "SignalR still disconnected from {HubUrl} (retry #{Count}, " +
            "down for {Elapsed}, last error: {Error}). Trying again in {Delay}s.",
            _hubUrl,
            retryContext.PreviousRetryCount + 1,
            retryContext.ElapsedTime,
            retryContext.RetryReason?.Message ?? "(none)",
            (int)delay.TotalSeconds);

        return delay;
    }
}
