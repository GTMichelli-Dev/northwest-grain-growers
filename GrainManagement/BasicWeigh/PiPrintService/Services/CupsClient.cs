using System.Diagnostics;
using System.Text.RegularExpressions;

namespace WebPrintService.Services;

/// <summary>
/// Wrapper around CUPS command-line tools (lpstat, lp, lpoptions).
/// Works on Linux (Raspberry Pi) and macOS.
/// </summary>
public class CupsClient : IPrintClient
{
    private readonly ILogger<CupsClient> _log;

    public CupsClient(ILogger<CupsClient> log)
    {
        _log = log;
    }

    /// <summary>
    /// Get all printers known to CUPS with their status.
    /// </summary>
    public async Task<List<PrinterInfo>> GetPrintersAsync()
    {
        var printers = new List<PrinterInfo>();

        try
        {
            // lpstat -p -d gives printer list and default
            var (exitCode, output) = await RunCommandAsync("lpstat", "-p -d");
            if (exitCode != 0)
            {
                _log.LogWarning("lpstat failed (exit {Code}): {Output}", exitCode, output);
                return printers;
            }

            string? defaultPrinter = null;
            var lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                // "system default destination: PrinterName"
                if (line.StartsWith("system default destination:"))
                {
                    defaultPrinter = line.Split(':').LastOrDefault()?.Trim();
                    continue;
                }

                // "printer PrinterName is idle. enabled since ..."
                // "printer PrinterName disabled since ..."
                var match = Regex.Match(line, @"^printer\s+(\S+)\s+(is\s+)?(\w+)");
                if (match.Success)
                {
                    var name = match.Groups[1].Value;
                    var status = match.Groups[3].Value; // idle, disabled, printing

                    printers.Add(new PrinterInfo
                    {
                        PrinterId = name,
                        DisplayName = name,
                        Status = status,
                        IsDefault = false, // set below
                        Enabled = !status.Equals("disabled", StringComparison.OrdinalIgnoreCase)
                    });
                }
            }

            // Get descriptions via lpoptions
            foreach (var printer in printers)
            {
                printer.IsDefault = printer.PrinterId == defaultPrinter;

                try
                {
                    var (ec, opts) = await RunCommandAsync("lpoptions", $"-p {printer.PrinterId} -l");
                    // Parse printer-info if available
                    var (ec2, info) = await RunCommandAsync("lpoptions", $"-p {printer.PrinterId}");
                    var infoMatch = Regex.Match(info, @"printer-info='([^']*)'");
                    if (infoMatch.Success)
                        printer.DisplayName = infoMatch.Groups[1].Value;
                }
                catch { /* description is optional */ }
            }
        }
        catch (Exception ex)
        {
            _log.LogError("Failed to get CUPS printers: {Msg}", ex.Message);
        }

        return printers;
    }

    /// <summary>
    /// Print a file (PDF, image, etc.) to a specific printer.
    /// </summary>
    public async Task<(bool success, string message)> PrintFileAsync(string printerId, string filePath, string? jobTitle = null)
    {
        var args = $"-d {printerId}";
        if (!string.IsNullOrEmpty(jobTitle))
            args += $" -t \"{jobTitle}\"";
        args += $" \"{filePath}\"";

        var (exitCode, output) = await RunCommandAsync("lp", args);
        if (exitCode == 0)
        {
            _log.LogInformation("Print job sent to {Printer}: {Output}", printerId, output.Trim());
            return (true, output.Trim());
        }

        _log.LogWarning("Print failed on {Printer} (exit {Code}): {Output}", printerId, exitCode, output);
        return (false, output.Trim());
    }

    /// <summary>
    /// Print from a URL (downloads then prints).
    /// </summary>
    public async Task<(bool success, string message)> PrintFromUrlAsync(string printerId, string url, string? jobTitle = null, HttpClient? http = null)
    {
        http ??= new HttpClient();
        var tempFile = Path.Combine(Path.GetTempPath(), $"print_{Guid.NewGuid():N}.pdf");

        try
        {
            var response = await http.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return (false, $"Failed to download: HTTP {(int)response.StatusCode}");

            await using var fs = File.Create(tempFile);
            await response.Content.CopyToAsync(fs);
            fs.Close();

            return await PrintFileAsync(printerId, tempFile, jobTitle);
        }
        finally
        {
            try { File.Delete(tempFile); } catch { }
        }
    }

    /// <summary>
    /// Get the status of a specific printer.
    /// </summary>
    public async Task<string> GetPrinterStatusAsync(string printerId)
    {
        var (exitCode, output) = await RunCommandAsync("lpstat", $"-p {printerId}");
        return exitCode == 0 ? output.Trim() : $"Error: {output.Trim()}";
    }

    /// <summary>
    /// Check if CUPS is available on this system.
    /// </summary>
    public async Task<bool> IsAvailableAsync()
    {
        try
        {
            var (exitCode, _) = await RunCommandAsync("lpstat", "-r");
            return exitCode == 0;
        }
        catch
        {
            return false;
        }
    }

    private static async Task<(int exitCode, string output)> RunCommandAsync(string command, string args)
    {
        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = command,
                Arguments = args,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();
        var stdout = await process.StandardOutput.ReadToEndAsync();
        var stderr = await process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();

        var output = string.IsNullOrEmpty(stderr) ? stdout : $"{stdout}\n{stderr}";
        return (process.ExitCode, output);
    }
}

// PrinterInfo is defined in IPrintClient.cs
