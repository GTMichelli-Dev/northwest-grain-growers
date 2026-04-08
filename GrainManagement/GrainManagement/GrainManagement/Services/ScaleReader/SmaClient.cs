using Microsoft.Extensions.Logging;
using GrainManagement.Services.ScaleReader.Models;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace GrainManagement.Services.ScaleReader
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
        private readonly ILogger<SmaClient> _log;

        // Heuristic regex: finds the last integer in the line (common for gross/net)
        private static readonly Regex WeightRegex = new(@"\s*(-?\d+)(?:\.\d+)?\s*", RegexOptions.Compiled);

        public SmaClient(ILogger<SmaClient> log)
        {
            _log = log;
        }

        public async Task<(bool ok, int weight, bool motion, string status)> QueryOnceAsync(
            ScaleConfigEntity config, int timeoutMs, CancellationToken ct)
        {
            var enc = string.Equals(config.Encoding, "utf-8", StringComparison.OrdinalIgnoreCase)
                ? Encoding.UTF8
                : Encoding.ASCII;

            using var client = new TcpClient();
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            cts.CancelAfter(timeoutMs);

            await client.ConnectAsync(config.IpAddress, config.Port, cts.Token);
            client.ReceiveTimeout = timeoutMs;
            client.SendTimeout = timeoutMs;

            using NetworkStream ns = client.GetStream();

            // Send request
            byte[] request = BuildRequestBytes(config.RequestCommand, enc);
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
                    sb.Append(enc.GetString(buffer, 0, bytesRead));
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

        private static byte[] BuildRequestBytes(string s, Encoding enc)
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

            return enc.GetBytes(s);
        }
    }
}
