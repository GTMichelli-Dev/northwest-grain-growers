using ScaleReaderService.Models;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace ScaleReaderService.Services
{
    /// <summary>
    /// Minimal SMA-over-TCP client: sends a request command and parses the reply.
    /// </summary>
    public sealed class SmaClient
    {
        private readonly ILogger<SmaClient> _log;
        private static readonly Regex WeightRegex = new(@"\s*(-?\d+)(?:\.\d+)?\s*", RegexOptions.Compiled);

        public SmaClient(ILogger<SmaClient> log)
        {
            _log = log;
        }

        public async Task<(bool ok, int weight, bool motion, string status, string rawResponse)> QueryOnceAsync(
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

            byte[] request = BuildRequestBytes(config.RequestCommand, enc);
            await ns.WriteAsync(request.AsMemory(0, request.Length), cts.Token);
            await ns.FlushAsync(ct);

            var buffer = new byte[1024];
            var sb = new StringBuilder();
            do
            {
                if (ns.DataAvailable)
                {
                    int bytesRead = await ns.ReadAsync(buffer, cts.Token);
                    if (bytesRead <= 0) break;
                    sb.Append(enc.GetString(buffer, 0, bytesRead));
                    if (sb.ToString().Contains('\n') || sb.ToString().Contains('\r')) break;
                }
                else
                {
                    await Task.Delay(10, ct);
                }
            }
            while (!ct.IsCancellationRequested);

            // Trim only CR/LF, preserve internal spaces (SMA uses spaces as status codes)
            string reply = sb.ToString().Trim('\r', '\n');
            if (string.IsNullOrWhiteSpace(reply))
                return (false, 0, false, "No response", "");

            // ── SMA Standard Response: <LF><s><r><n><m><f><XXXXXX.XXX><uuu><CR>
            //   <s> Scale Status:  Z=Center of Zero, O=Over Capacity, U=Under Capacity,
            //                      E=Zero Error, T=Tare Error, space=None
            //   <r> Range:         Always '1' if multi-interval
            //   <n> Gross/Net:     G=Gross, T=Tare, N=Net
            //   <m> Motion:        M=scale is in motion, space=stable
            //   <f> Future:        Always space
            //   <XXXXXX.XXX>       Weight value
            //   <uuu>              Unit (lb, kg, etc.)

            // Try positional parse: find the status header before the weight digits
            // The header is typically <s><r><n><m><f> (5 chars) before the weight
            var headerMatch = Regex.Match(reply, @"^([ZOUET ])(\d)([GTN])([M ])( )", RegexOptions.None);

            bool motion = false;
            bool error = false;

            if (headerMatch.Success)
            {
                // Positional parse succeeded
                char scaleStatus = headerMatch.Groups[1].Value[0];
                char motionChar  = headerMatch.Groups[4].Value[0];

                motion = (motionChar == 'M');
                error  = scaleStatus == 'O'     // Over Capacity
                      || scaleStatus == 'U'     // Under Capacity
                      || scaleStatus == 'E'     // Zero Error
                      || scaleStatus == 'T';    // Tare Error
            }
            else
            {
                // Fallback: heuristic for non-standard responses
                motion = reply.Contains("US", StringComparison.OrdinalIgnoreCase)
                      || reply.Contains("UNST", StringComparison.OrdinalIgnoreCase)
                      || reply.Contains("MOTION", StringComparison.OrdinalIgnoreCase)
                      || reply.Contains('M');

                error = reply.Contains("ERR", StringComparison.OrdinalIgnoreCase)
                     || reply.Contains('O')
                     || reply.Contains('U')
                     || reply.Contains('E');
            }

            // Parse weight value
            var m = WeightRegex.Matches(reply);
            int weight = 0;
            bool ok = false;
            if (m.Count > 0)
            {
                if (int.TryParse(m[^1].Groups[1].Value, out int parsed))
                {
                    weight = parsed;
                    ok = true;
                }
            }

            if (error) ok = false;

            var status = !ok ? "Error" : (motion ? "Motion" : "Ok");
            var safeReply = ToSafeString(reply);
            return (ok, weight, motion, status, safeReply);
        }

        /// <summary>
        /// Converts a string to a safe display form, replacing non-printable characters
        /// with ASCII names (e.g. [STX], [CR], [LF], [ENQ]) or [0xXX] for unknowns.
        /// </summary>
        private static string ToSafeString(string s)
        {
            var sb = new StringBuilder(s.Length);
            foreach (char c in s)
            {
                if (c >= 0x20 && c <= 0x7E)
                {
                    sb.Append(c);
                }
                else
                {
                    sb.Append((int)c switch
                    {
                        0x00 => "[NUL]",
                        0x01 => "[SOH]",
                        0x02 => "[STX]",
                        0x03 => "[ETX]",
                        0x04 => "[EOT]",
                        0x05 => "[ENQ]",
                        0x06 => "[ACK]",
                        0x07 => "[BEL]",
                        0x08 => "[BS]",
                        0x09 => "[TAB]",
                        0x0A => "[LF]",
                        0x0B => "[VT]",
                        0x0C => "[FF]",
                        0x0D => "[CR]",
                        0x0E => "[SO]",
                        0x0F => "[SI]",
                        0x1B => "[ESC]",
                        0x7F => "[DEL]",
                        _ => $"[0x{(int)c:X2}]"
                    });
                }
            }
            return sb.ToString();
        }

        private static byte[] BuildRequestBytes(string s, Encoding enc)
        {
            s = s.Replace("\\r", "\r").Replace("\\n", "\n");
            s = Regex.Replace(s, @"\\u([0-9A-Fa-f]{4})", match =>
            {
                var code = Convert.ToInt32(match.Groups[1].Value, 16);
                return char.ConvertFromUtf32(code);
            });
            return enc.GetBytes(s);
        }
    }
}
