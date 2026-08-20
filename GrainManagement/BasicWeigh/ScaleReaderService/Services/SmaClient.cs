using Microsoft.Extensions.Logging;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace ScaleReaderService.Services;

/// <summary>
/// SMA (Scale Manufacturers Association) protocol client over TCP.
/// Supports Weigh-Tronix ZM-301 and other SMA-compatible indicators.
///
/// SMA 8.1.2 Standard Scale Response Message:
///   LF + s + r + n + m + f + xxxxxx.xxx + uuu + CR
///
///   s = Status:  ' '=normal, 'Z'=Center of Zero, 'O'=Over Capacity,
///                'U'=Under Capacity, 'E'=Zero Error, 'T'=Tare Error
///   r = Range:   '1' (multi-interval range, always '1' if disabled)
///   n = Net/Gross: 'G'=Gross, 'T'=Tare, 'N'=Net
///   m = Motion:  'M'=in motion, ' '=stable
///   f = Future:  ' ' (always space)
///   xxxxxx.xxx = Weight value (right-justified, space-padded)
///   uuu = Unit of measure (e.g. " lb", " kg")
/// </summary>
public sealed class SmaClient
{
    private readonly ILogger<SmaClient> _log;
    private static readonly Regex WeightRegex = new(@"(-?\d+(?:\.\d+)?)", RegexOptions.Compiled);

    public SmaClient(ILogger<SmaClient> log)
    {
        _log = log;
    }

    public async Task<(bool ok, int weight, bool motion, string status, string rawText, string rawHex)> QueryOnceAsync(
        string ip, int port, string? requestCommand, int timeoutMs, CancellationToken ct)
    {
        var command = string.IsNullOrWhiteSpace(requestCommand) ? "W\r\n" : requestCommand;

        using var client = new TcpClient();
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        cts.CancelAfter(timeoutMs > 0 ? timeoutMs : 2000);

        await client.ConnectAsync(ip, port, cts.Token);
        client.ReceiveTimeout = timeoutMs;
        client.SendTimeout = timeoutMs;

        using NetworkStream ns = client.GetStream();

        // Send request
        byte[] request = BuildRequestBytes(command);
        await ns.WriteAsync(request.AsMemory(0, request.Length), cts.Token);
        await ns.FlushAsync(ct);

        // Read raw response
        var rawBuffer = new List<byte>();
        var buffer = new byte[256];
        var deadline = DateTime.UtcNow.AddMilliseconds(timeoutMs > 0 ? timeoutMs : 2000);

        while (DateTime.UtcNow < deadline && !ct.IsCancellationRequested)
        {
            if (ns.DataAvailable)
            {
                int bytesRead = await ns.ReadAsync(buffer, cts.Token);
                if (bytesRead <= 0) break;
                for (int i = 0; i < bytesRead; i++)
                    rawBuffer.Add(buffer[i]);

                // SMA response ends with CR (0x0D)
                if (rawBuffer.Contains(0x0D)) break;
            }
            else
            {
                await Task.Delay(10, ct);
            }
        }

        string rawHex = BitConverter.ToString(rawBuffer.ToArray());

        if (rawBuffer.Count == 0)
        {
            return (false, 0, false, "No response", "", "");
        }

        // Convert to string for parsing, strip LF and CR
        string raw = Encoding.ASCII.GetString(rawBuffer.ToArray());
        string rawText = raw.Replace("\n", "<LF>").Replace("\r", "<CR>");
        string reply = raw.TrimStart('\n', '\r').TrimEnd('\n', '\r');

        _log.LogDebug("SMA raw ({Count} bytes): hex={Hex} text='{Reply}'",
            rawBuffer.Count, rawHex, reply);

        // ZM-301 response format (after LF/CR stripped):
        //   [echo/pad][status][range][gross/net][motion][future][weight][unit]
        // Find the SMA header by locating the pattern: status + digit(range) + G/N/T
        int headerStart = FindSmaHeader(reply);
        if (headerStart >= 0)
        {
            string smaData = reply.Substring(headerStart);
            _log.LogDebug("SMA header found at position {Pos}, data='{Data}'", headerStart, smaData);
            if (smaData.Length >= 7)
            {
                var (ok, weight, motion, status) = ParseSmaResponse(smaData);
                return (ok, weight, motion, status, rawText, rawHex);
            }
        }

        // Fallback for short/non-standard responses
        var fb = ParseFallback(reply);
        return (fb.ok, fb.weight, fb.motion, fb.status, rawText, rawHex);
    }

    /// <summary>
    /// Parse SMA 8.1.2 standard response: s r n m f xxxxxx.xxx uuu
    /// </summary>
    private (bool ok, int weight, bool motion, string status) ParseSmaResponse(string reply)
    {
        // Position 0: s = Status
        char statusChar = reply[0];
        // Position 1: r = Range (ignore)
        // Position 2: n = Gross/Net/Tare
        char grossNet = reply.Length > 2 ? reply[2] : ' ';
        // Position 3: m = Motion
        char motionChar = reply.Length > 3 ? reply[3] : ' ';
        // Position 4: f = Future (ignore)
        // Position 5+: weight + unit
        string weightPart = reply.Length > 5 ? reply.Substring(5) : "";

        // Parse motion
        bool motion = motionChar == 'M';

        // Parse status
        bool ok = true;
        string status;

        switch (statusChar)
        {
            case 'O':
                ok = false;
                status = "Over Capacity";
                break;
            case 'U':
                ok = false;
                status = "Under Capacity";
                break;
            case 'E':
                ok = false;
                status = "Zero Error";
                break;
            case 'T':
                ok = false;
                status = "Tare Error";
                break;
            case 'Z':
                status = motion ? "Motion" : ">0<";
                break;
            default: // space = normal
                status = motion ? "Motion" : "Ok";
                break;
        }

        // Validate gross mode — must be 'G' for weighing operations
        if (ok && char.ToUpper(grossNet) != 'G')
        {
            ok = false;
            status = "Not Gross Mode";
            return (ok, 0, motion, status);
        }

        // Parse weight value from remaining string
        int weight = 0;
        var m = WeightRegex.Match(weightPart);
        if (m.Success && double.TryParse(m.Groups[1].Value, out double parsed))
        {
            weight = (int)Math.Round(parsed);
        }

        // Validate units — must contain 'LB'
        string unitPart = weightPart.ToUpper();
        if (ok && !unitPart.Contains("LB"))
        {
            ok = false;
            status = "Wrong Units";
            return (ok, weight, motion, status);
        }

        // Build display status
        status = motion ? "Motion" : "Ok";

        _log.LogDebug("SMA parsed: status='{Status}' range='{Range}' mode='{Mode}' motion={Motion} weight={Weight}",
            statusChar, reply.Length > 1 ? reply[1] : ' ', grossNet, motion, weight);

        return (ok, weight, motion, status);
    }

    /// <summary>
    /// Fallback parser for non-standard or short responses.
    /// </summary>
    private (bool ok, int weight, bool motion, string status) ParseFallback(string reply)
    {
        bool motion = reply.Contains("M") || reply.Contains("US", StringComparison.OrdinalIgnoreCase)
                   || reply.Contains("MOTION", StringComparison.OrdinalIgnoreCase);

        var matches = WeightRegex.Matches(reply);
        int weight = 0;
        bool ok = false;

        if (matches.Count > 0)
        {
            var lastMatch = matches[matches.Count - 1];
            if (double.TryParse(lastMatch.Groups[1].Value, out double parsed))
            {
                weight = (int)Math.Round(parsed);
                ok = true;
            }
        }

        if (reply.Contains("ERR", StringComparison.OrdinalIgnoreCase) ||
            reply.Contains("OL", StringComparison.OrdinalIgnoreCase))
        {
            ok = false;
        }

        string status = !ok ? "Error" : (motion ? "Motion" : "Ok");
        return (ok, weight, motion, status);
    }

    /// <summary>
    /// Find the start of the SMA header in the response.
    /// Looks for pattern: [status][digit][G/N/T] where status is space/O/U/E/Z/T
    /// and digit is the range number, followed by G(ross)/N(et)/T(are).
    /// </summary>
    private static int FindSmaHeader(string reply)
    {
        for (int i = 0; i <= reply.Length - 5; i++)
        {
            char s = reply[i];      // status
            char r = reply[i + 1];  // range (digit)
            char n = reply[i + 2];  // gross/net/tare

            bool validStatus = s == ' ' || s == 'O' || s == 'U' || s == 'E' || s == 'Z' || s == 'T';
            bool validRange = char.IsDigit(r);
            bool validMode = n == 'G' || n == 'N' || n == 'T';

            if (validStatus && validRange && validMode)
                return i;
        }
        return -1;
    }

    private static byte[] BuildRequestBytes(string s)
    {
        s = s.Replace("\\r", "\r").Replace("\\n", "\n");
        s = Regex.Replace(s, @"\\u([0-9A-Fa-f]{4})", match =>
        {
            var code = Convert.ToInt32(match.Groups[1].Value, 16);
            return char.ConvertFromUtf32(code);
        });
        return Encoding.ASCII.GetBytes(s);
    }
}
