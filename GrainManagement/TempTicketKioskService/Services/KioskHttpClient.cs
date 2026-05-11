using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using TempTicketKioskService.Models;

namespace TempTicketKioskService.Services;

/// <summary>
/// Thin HTTP client that POSTs button-press events to the GrainManagement
/// web app's <c>/api/temp-tickets/press</c> endpoint.
///
/// One short retry on transient failure (network glitch) then drop —
/// queuing button presses through a long outage and replaying them
/// would generate ghost temp tickets the operator never intended,
/// which is worse than losing a single press.
/// </summary>
public sealed class KioskHttpClient
{
    private static readonly TimeSpan PressTimeout = TimeSpan.FromSeconds(5);

    private readonly IHttpClientFactory _http;
    private readonly IOptionsMonitor<KioskOptions> _opt;
    private readonly ILogger<KioskHttpClient> _log;

    public KioskHttpClient(
        IHttpClientFactory http,
        IOptionsMonitor<KioskOptions> opt,
        ILogger<KioskHttpClient> log)
    {
        _http = http;
        _opt = opt;
        _log = log;
    }

    public Task<PressResult> PressAsync(CancellationToken ct = default)
        => PressAsync(source: "gpio", ct);

    public async Task<PressResult> PressAsync(string source, CancellationToken ct = default)
    {
        var opt = _opt.CurrentValue;
        var payload = new
        {
            KioskId       = opt.KioskId,
            ScaleId       = opt.ScaleId,
            PrinterTarget = opt.PrinterTarget,
        };

        for (var attempt = 1; attempt <= 2; attempt++)
        {
            try
            {
                using var client = _http.CreateClient();
                client.Timeout = PressTimeout;
                var url = opt.ServerUrl.TrimEnd('/') + "/api/temp-tickets/press";

                var resp = await client.PostAsJsonAsync(url, payload, ct);
                if (resp.IsSuccessStatusCode)
                {
                    _log.LogInformation(
                        "Press ({Source}) accepted by {Url} (kiosk={Kiosk} scale={Scale}).",
                        source, url, opt.KioskId, opt.ScaleId);
                    return new PressResult(true, null);
                }

                var body = await resp.Content.ReadAsStringAsync(ct);
                _log.LogWarning(
                    "Press ({Source}) rejected by {Url} (attempt {Attempt}): {Status} {Body}",
                    source, url, attempt, (int)resp.StatusCode, body);

                // 4xx — caller is wrong; don't bother retrying.
                if ((int)resp.StatusCode is >= 400 and < 500)
                    return new PressResult(false, $"{(int)resp.StatusCode}: {body}");
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex)
            {
                _log.LogWarning(ex,
                    "Press ({Source}) failed (attempt {Attempt}).", source, attempt);
            }

            if (attempt == 1)
            {
                try { await Task.Delay(TimeSpan.FromMilliseconds(500), ct); }
                catch (OperationCanceledException) { return new PressResult(false, "cancelled"); }
            }
        }

        return new PressResult(false, "Web app unreachable after retry.");
    }

    public sealed record PressResult(bool Success, string? Error);
}
