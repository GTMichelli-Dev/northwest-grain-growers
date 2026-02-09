using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;

//
// ===============================
// A) Configuration
// ===============================
//
var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

string baseUrl = (config["Server:BaseUrl"] ?? "").TrimEnd('/');
string deviceId = config["Server:DeviceId"] ?? Environment.MachineName;
string printer = config["Printing:DefaultPrinter"] ?? "Kiosk";

if (string.IsNullOrWhiteSpace(baseUrl))
{
    Console.Error.WriteLine("Missing Server:BaseUrl (set in appsettings.json or env var Server__BaseUrl).");
    return;
}

string hubUrl = $"{baseUrl}/hubs/print";

Console.WriteLine($"BaseUrl : {baseUrl}");
Console.WriteLine($"HubUrl  : {hubUrl}");
Console.WriteLine($"DeviceId: {deviceId}");
Console.WriteLine($"Printer : {printer}");

//
// ===============================
// B) Build SignalR connection
// ===============================
//
var connection = new HubConnectionBuilder()
    .WithUrl(hubUrl)
    .WithAutomaticReconnect(new[]
    {
        TimeSpan.Zero,
        TimeSpan.FromSeconds(2),
        TimeSpan.FromSeconds(5),
        TimeSpan.FromSeconds(10),
        TimeSpan.FromSeconds(30)
    })
    .Build();

//
// ===============================
// C) Reconnect behavior (kiosk-grade)
// ===============================
//
connection.Reconnecting += ex =>
{
    Console.WriteLine($"[SignalR] Reconnecting... {ex?.Message}");
    return Task.CompletedTask;
};

connection.Reconnected += async connectionId =>
{
    Console.WriteLine($"[SignalR] Reconnected. ConnectionId={connectionId}. Re-registering...");
    await SafeRegisterAsync(connection, deviceId);
};

connection.Closed += async ex =>
{
    Console.WriteLine($"[SignalR] Closed. {ex?.Message}");
    // When closed, we retry forever. This handles cases where auto-reconnect gives up.
    while (true)
    {
        try
        {
            Console.WriteLine("[SignalR] Trying to restart connection...");
            await connection.StartAsync();
            await SafeRegisterAsync(connection, deviceId);
            Console.WriteLine("[SignalR] Restarted OK.");
            break;
        }
        catch (Exception restartEx)
        {
            Console.WriteLine($"[SignalR] Restart failed: {restartEx.Message}. Retrying in 10s...");
            await Task.Delay(TimeSpan.FromSeconds(10));
        }
    }
};

//
// ===============================
// D) Message handler(s)
// ===============================
//
connection.On<JsonElement>("PrintLoadTicket", async payload =>
{
    // Payload can be { ticket: "123" } or { Ticket: "123" } etc.
    if (!TryGetString(payload, "ticket", out var ticket))
    {
        Console.Error.WriteLine($"[Print] Payload missing 'ticket'. Payload: {payload}");
        return;
    }

    Console.WriteLine($"[Print] Received print request for ticket: {ticket}");

    try
    {
        var pdfPath = await DownloadPdfAsync(baseUrl, ticket);
        var ok = await PrintPdfAsync(pdfPath, printer);

        try { File.Delete(pdfPath); } catch { /* ignore */ }

        Console.WriteLine(ok
            ? $"[Print] Printed OK: {ticket}"
            : $"[Print] Print failed: {ticket}");
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"[Print] Exception: {ex}");
    }
});

//
// ===============================
// E) Start loop
// ===============================
//
await StartAndRegisterAsync(connection, deviceId);

Console.WriteLine("[Main] Listening... Ctrl+C to exit.");
await Task.Delay(Timeout.InfiniteTimeSpan);


//
// ===============================
// Helpers
// ===============================
//

static async Task StartAndRegisterAsync(HubConnection conn, string deviceId)
{
    await conn.StartAsync();
    Console.WriteLine("[SignalR] Connected.");
    await SafeRegisterAsync(conn, deviceId);
}

static async Task SafeRegisterAsync(HubConnection conn, string deviceId)
{
    try
    {
        // Your server hub must have: Task Register(string deviceId)
        await conn.InvokeAsync("Register", deviceId);
        Console.WriteLine($"[SignalR] Registered: {deviceId}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[SignalR] Register failed: {ex.Message}");
    }
}

static bool TryGetString(JsonElement obj, string name, out string value)
{
    value = "";
    if (obj.ValueKind != JsonValueKind.Object) return false;

    // Case-insensitive property lookup
    foreach (var p in obj.EnumerateObject())
    {
        if (string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase))
        {
            value = p.Value.ValueKind == JsonValueKind.String ? (p.Value.GetString() ?? "") : p.Value.ToString();
            return !string.IsNullOrWhiteSpace(value);
        }
    }
    return false;
}

static async Task<string> DownloadPdfAsync(string baseUrl, string ticket)
{
    var url = $"{baseUrl}/api/printjobs/load-ticket/{Uri.EscapeDataString(ticket)}/pdf";

    var tempDir = Path.GetTempPath();
    Directory.CreateDirectory(tempDir);

    // Make filename safe-ish (ticket is usually numeric, but we’ll be careful)
    var safeTicket = string.Concat(ticket.Where(ch => !Path.GetInvalidFileNameChars().Contains(ch)));
    var path = Path.Combine(tempDir, $"LoadTicket-{safeTicket}-{Guid.NewGuid():N}.pdf");

    using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
    http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/pdf"));

    using var resp = await http.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
    resp.EnsureSuccessStatusCode();

    await using (var fs = File.Create(path))
    await using (var rs = await resp.Content.ReadAsStreamAsync())
    {
        await rs.CopyToAsync(fs);
    }

    // Quick sanity check: first bytes should look like "%PDF"
    var header = new byte[4];
    await using (var check = File.OpenRead(path))
    {
        var read = await check.ReadAsync(header, 0, header.Length);
        if (read < 4 || header[0] != (byte)'%' || header[1] != (byte)'P' || header[2] != (byte)'D' || header[3] != (byte)'F')
            throw new Exception("Downloaded file does not look like a PDF.");
    }

    return path;
}

static async Task<bool> PrintPdfAsync(string pdfPath, string printer)
{
    // Always specify -d so we don't depend on default destination.
    // Add options to prevent CUPS scaling surprises:
    // -o scaling=100 -o fit-to-page=false
    var args = $"-d {EscapeArg(printer)} -o scaling=100 -o fit-to-page=false {EscapeArg(pdfPath)}";

    var psi = new ProcessStartInfo
    {
        FileName = "/usr/bin/lp",
        Arguments = args,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false
    };

    // On Windows dev box, printing won't work; just log and return true/false as you prefer.
    if (OperatingSystem.IsWindows())
    {
        Console.WriteLine($"[Print] (Windows dev) Would run: lp {args}");
        return true;
    }

    using var p = Process.Start(psi);
    if (p == null) return false;

    var stdout = await p.StandardOutput.ReadToEndAsync();
    var stderr = await p.StandardError.ReadToEndAsync();
    await p.WaitForExitAsync();

    if (!string.IsNullOrWhiteSpace(stdout)) Console.WriteLine($"[lp] {stdout.Trim()}");
    if (!string.IsNullOrWhiteSpace(stderr)) Console.WriteLine($"[lp] {stderr.Trim()}");

    return p.ExitCode == 0;
}

static string EscapeArg(string s)
{
    // Minimal quoting for paths/printer names with spaces.
    if (string.IsNullOrEmpty(s)) return "\"\"";
    if (s.Contains(' ') || s.Contains('"'))
        return "\"" + s.Replace("\"", "\\\"") + "\"";
    return s;
}
