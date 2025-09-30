using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ScaleReaderService.Models;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Collections.Generic;   // ✅ for List<T>
using System.Linq;                  // ✅ for ToArray()

// Use your existing ScaleDTO (in global namespace)
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ScaleReaderService.Services
{
    public sealed class ScalePoller
    {
        private readonly IHttpClientFactory _httpFactory;
        private readonly EndpointOptions[] _endpoints;
        private readonly ScaleOptions[] _scales;
        private readonly PollingOptions _poll;
        private readonly ILogger<ScalePoller> _log;
        private readonly SmaClient _smaClient;

        public ScalePoller(
            IHttpClientFactory httpFactory,
            IOptions<List<EndpointOptions>> endpoints,   // ✅ List<T> via options
            IOptions<List<ScaleOptions>> scales,         // ✅ List<T> via options
            IOptions<PollingOptions> poll,
            SmaClient smaClient,
            ILogger<ScalePoller> log)
        {
            _httpFactory = httpFactory;
            _endpoints = (endpoints.Value ?? new List<EndpointOptions>()).ToArray();
            _scales = (scales.Value ?? new List<ScaleOptions>()).ToArray();
            _poll = poll.Value;
            _smaClient = smaClient;
            _log = log;
        }

        public async Task RunAsync(CancellationToken ct)
        {
            if (_scales.Length == 0)
            {
                _log.LogWarning("No scales configured.");
                await Task.Delay(Timeout.Infinite, ct);
                return;
            }

            _log.LogInformation("Starting {Count} scale workers.", _scales.Length);

            // One independent worker Task per scale
            var tasks = _scales.Select(scale => Task.Run(() => WorkerAsync(scale, ct), ct)).ToArray();
            await Task.WhenAll(tasks);
        }

        private async Task WorkerAsync(ScaleOptions scale, CancellationToken ct)
        {
            var backoff = _poll.ReconnectBackoffMs;
            var maxBackoff = _poll.MaxBackoffMs;

            while (!ct.IsCancellationRequested)
            {
                try
                {
                    var (ok, weight, motion, status) = await _smaClient.QueryOnceAsync(scale.IpAddress, scale.Port, ct);

                    var dto = new ScaleDTO
                    {
                        Description = scale.Description,
                        Motion = motion,
                        Ok = ok,
                        Weight = weight,
                        Status = ok ? "OK" : status,
                        LastUpdate = DateTime.Now
                    };

                    // Fan-out to all endpoints without blocking the poll loop
                    foreach (var ep in _endpoints)
                    {
                        _ = Task.Run(() => PostToEndpointAsync(ep, dto, ct));
                    }

                    await Task.Delay(_poll.IntervalMs, ct);
                    backoff = _poll.ReconnectBackoffMs; // reset after success
                }
                catch (OperationCanceledException) when (ct.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _log.LogWarning(ex, "Scale '{Desc}' connection/query failed. Backing off then retrying.", scale.Description);
                    await Task.Delay(backoff, ct);
                    backoff = Math.Min(backoff * 2, maxBackoff);
                }
            }
        }

        private async Task PostToEndpointAsync(EndpointOptions endpoint, ScaleDTO dto, CancellationToken ct)
        {
            try
            {
                var client = _httpFactory.CreateClient("endpoints");
                client.Timeout = TimeSpan.FromMilliseconds(endpoint.TimeoutMs);

                using var request = new HttpRequestMessage(new HttpMethod(endpoint.Method ?? "POST"), endpoint.Url)
                {
                    Content = JsonContent.Create(dto)
                };

                using var resp = await client.SendAsync(request, ct);
                if (!resp.IsSuccessStatusCode)
                {
                    var body = await SafeReadAsync(resp);
                    _log.LogWarning("Endpoint {Name} responded {Code}. Body: {Body}",
                        endpoint.Name, (int)resp.StatusCode, Truncate(body, 200));
                }
                else
                {
                    _log.LogDebug("Posted to endpoint {Name} successfully.", endpoint.Name);
                }
            }
            catch (TaskCanceledException tce)
            {
                _log.LogWarning("Endpoint {Name} timed out: {Msg}", endpoint.Name, tce.Message);
            }
            catch (Exception ex)
            {
                _log.LogWarning(ex, "Endpoint {Name} post failed.", endpoint.Name);
            }
        }

        private static async Task<string> SafeReadAsync(HttpResponseMessage resp)
        {
            try { return await resp.Content.ReadAsStringAsync(); }
            catch { return "<unreadable>"; }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string Truncate(string s, int max)
            => (s?.Length ?? 0) <= max ? (s ?? "") : s!.Substring(0, max) + "…";
    }
}
