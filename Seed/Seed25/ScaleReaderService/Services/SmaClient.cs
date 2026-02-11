using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ScaleReaderService.Models;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace ScaleReaderService.Services
{
    /// <summary>
    /// Minimal SMA-over-TCP client: sends a request command and parses the reply.
    /// Parser is tolerant and extracts:
    ///   - Weight (int, scaled to units of whole weight; adjust if you need decimals)
    ///   - Motion (true if line indicates motion/unstable)
    ///   - Ok (true when a valid weight was parsed and no explicit error tokens present)
    ///   - Status (raw response)
    /// </summary>
    public sealed class SmaClient
    {
        private readonly SmaOptions _sma;
        private readonly PollingOptions _poll;
        private readonly ILogger<SmaClient> _log;
        private readonly Encoding _enc;

        // Heuristic regex: finds the last integer in the line (common for gross/net)
        private static readonly Regex WeightRegex = new(@"\s*(-?\d+)(?:\.\d+)?\s*", RegexOptions.Compiled);

        public SmaClient(IOptions<SmaOptions> sma, IOptions<PollingOptions> poll, ILogger<SmaClient> log)
        {
            _sma = sma.Value;
            _poll = poll.Value;
            _log = log;
            _enc = string.Equals(_sma.Encoding, "utf-8", StringComparison.OrdinalIgnoreCase)
                 ? Encoding.UTF8
                 : Encoding.ASCII;
        }

        public async Task<(bool ok, int weight, bool motion, string status)> QueryOnceAsync(string ip, int port, CancellationToken ct)
        {
            using var client = new TcpClient();
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            cts.CancelAfter(_poll.TimeoutMs);

            await client.ConnectAsync(ip, port, cts.Token);
            client.ReceiveTimeout = _poll.TimeoutMs;
            client.SendTimeout = _poll.TimeoutMs;

            using NetworkStream ns = client.GetStream();

            // Send request
            byte[] request = BuildRequestBytes(_sma.RequestCommand);
            await ns.WriteAsync(request.AsMemory(0, request.Length), cts.Token);
            await ns.FlushAsync(ct);

            // Read response (read until CR/LF or socket idle)
            var buffer = new byte[1024];
            var sb = new StringBuilder();
            int bytesRead;
            do
            {
                if (ns.DataAvailable)
                {
                    bytesRead = await ns.ReadAsync(buffer, 0, buffer.Length, cts.Token);
                    if (bytesRead <= 0) break;
                    sb.Append(_enc.GetString(buffer, 0, bytesRead));
                    if (sb.ToString().Contains("\n") || sb.ToString().Contains("\r")) break;
                }
                else
                {
                    // short pause to allow device to respond
                    await Task.Delay(10, ct);
                }
            }
            while (!ct.IsCancellationRequested);

            string reply = sb.ToString().Trim();
            if (string.IsNullOrWhiteSpace(reply))
            {
                return (false, 0, false, "No response");
            }

            // Parse motion/status tokens commonly seen in SMA-like outputs:
            // Examples: "ST,GS,   000123 kg", "US,GS,000123", "MOTION", "STABLE"
            bool motion = reply.Contains("US", StringComparison.OrdinalIgnoreCase)
                       || reply.Contains("M", StringComparison.OrdinalIgnoreCase)
                        || reply.Contains("UNST", StringComparison.OrdinalIgnoreCase)
                       || reply.Contains("MOTION", StringComparison.OrdinalIgnoreCase);

            var m = WeightRegex.Matches(reply);
            int weight = 0;
            bool ok = false;
            if (m.Count > 0)
            {
                // take the last numeric token as gross (devices often include headings + weight)
                if (int.TryParse(m[^1].Groups[1].Value, out int parsed))
                {
                    weight = parsed;
                    ok = true;
                }
            }

            // Additional failure tokens
            if (reply.Contains("ERR", StringComparison.OrdinalIgnoreCase)
                || reply.Contains("U", StringComparison.OrdinalIgnoreCase)
                        || reply.Contains("O", StringComparison.OrdinalIgnoreCase)
                       || reply.Contains("E", StringComparison.OrdinalIgnoreCase))
                ok = false;
            var status = (!ok) ? "Error" : (motion ? "Motion" : "Ok");
            return (ok, weight, motion, status);
        }

        private byte[] BuildRequestBytes(string s)
        {
            // Support escape forms like \u0005 (ENQ) and \r\n
            s = s
                .Replace("\\r", "\r")
                .Replace("\\n", "\n");

            // \uXXXX support
            s = Regex.Replace(s, @"\\u([0-9A-Fa-f]{4})", match =>
            {
                var code = Convert.ToInt32(match.Groups[1].Value, 16);
                return char.ConvertFromUtf32(code);
            });

            return _enc.GetBytes(s);
        }
    }
}
