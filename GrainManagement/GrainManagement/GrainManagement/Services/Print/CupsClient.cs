#nullable enable
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace GrainManagement.Services.Print
{
    /// <summary>
    /// Wraps CUPS printing via the /usr/bin/lp command on Linux.
    /// On Windows dev boxes, the print is simulated and logged.
    /// </summary>
    public sealed class CupsClient
    {
        private readonly ILogger<CupsClient> _log;

        public CupsClient(ILogger<CupsClient> log)
        {
            _log = log;
        }

        /// <summary>
        /// Prints a PDF file to the specified CUPS printer.
        /// Returns true if the print job was submitted successfully.
        /// </summary>
        public async Task<bool> PrintPdfAsync(string pdfPath, string printer)
        {
            if (string.IsNullOrWhiteSpace(pdfPath))
            {
                _log.LogWarning("PrintPdfAsync called with empty pdfPath.");
                return false;
            }

            if (!File.Exists(pdfPath))
            {
                _log.LogWarning("PDF file does not exist: {Path}", pdfPath);
                return false;
            }

            // On Windows dev box, printing won't work; just log and simulate success.
            if (OperatingSystem.IsWindows())
            {
                _log.LogInformation("[Windows dev] Would print: lp -d {Printer} -o scaling=100 -o fit-to-page=false {Path}",
                    printer, pdfPath);
                return true;
            }

            var args = $"-d {EscapeArg(printer)} -o scaling=100 -o fit-to-page=false {EscapeArg(pdfPath)}";

            _log.LogDebug("Executing: lp {Args}", args);

            var psi = new ProcessStartInfo
            {
                FileName = "/usr/bin/lp",
                Arguments = args,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };

            using var process = Process.Start(psi);
            if (process == null)
            {
                _log.LogError("Failed to start lp process.");
                return false;
            }

            var stdout = await process.StandardOutput.ReadToEndAsync();
            var stderr = await process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();

            if (!string.IsNullOrWhiteSpace(stdout))
                _log.LogInformation("[lp] {Output}", stdout.Trim());

            if (!string.IsNullOrWhiteSpace(stderr))
                _log.LogWarning("[lp] {Error}", stderr.Trim());

            return process.ExitCode == 0;
        }

        /// <summary>
        /// Lists available CUPS printers on the system.
        /// Returns an empty list on Windows or if lpstat is unavailable.
        /// </summary>
        public async Task<List<CupsPrinter>> ListPrintersAsync()
        {
            var printers = new List<CupsPrinter>();

            if (OperatingSystem.IsWindows())
            {
                _log.LogDebug("[Windows dev] ListPrintersAsync returning empty list.");
                return printers;
            }

            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = "/usr/bin/lpstat",
                    Arguments = "-p -d",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false
                };

                using var process = Process.Start(psi);
                if (process == null) return printers;

                var output = await process.StandardOutput.ReadToEndAsync();
                await process.WaitForExitAsync();

                string? defaultPrinter = null;

                foreach (var line in output.Split('\n', StringSplitOptions.RemoveEmptyEntries))
                {
                    var trimmed = line.Trim();

                    // Parse "printer <name> is idle." or "printer <name> disabled since..."
                    if (trimmed.StartsWith("printer ", StringComparison.OrdinalIgnoreCase))
                    {
                        var parts = trimmed.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length >= 3)
                        {
                            var name = parts[1];
                            var isEnabled = !trimmed.Contains("disabled", StringComparison.OrdinalIgnoreCase);
                            printers.Add(new CupsPrinter
                            {
                                Name = name,
                                IsEnabled = isEnabled,
                                IsDefault = false
                            });
                        }
                    }
                    // Parse "system default destination: <name>"
                    else if (trimmed.StartsWith("system default destination:", StringComparison.OrdinalIgnoreCase))
                    {
                        defaultPrinter = trimmed.Substring("system default destination:".Length).Trim();
                    }
                }

                // Mark the default printer
                if (defaultPrinter != null)
                {
                    foreach (var p in printers)
                    {
                        if (string.Equals(p.Name, defaultPrinter, StringComparison.OrdinalIgnoreCase))
                        {
                            p.IsDefault = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogWarning(ex, "Failed to enumerate CUPS printers.");
            }

            return printers;
        }

        /// <summary>
        /// Cancels all pending jobs for the given printer.
        /// </summary>
        public async Task<bool> CancelAllJobsAsync(string printer)
        {
            if (OperatingSystem.IsWindows())
            {
                _log.LogInformation("[Windows dev] Would cancel all jobs on printer: {Printer}", printer);
                return true;
            }

            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = "/usr/bin/cancel",
                    Arguments = $"-a {EscapeArg(printer)}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false
                };

                using var process = Process.Start(psi);
                if (process == null) return false;

                await process.WaitForExitAsync();
                return process.ExitCode == 0;
            }
            catch (Exception ex)
            {
                _log.LogWarning(ex, "Failed to cancel jobs on printer {Printer}.", printer);
                return false;
            }
        }

        private static string EscapeArg(string s)
        {
            if (string.IsNullOrEmpty(s)) return "\"\"";
            if (s.Contains(' ') || s.Contains('"'))
                return "\"" + s.Replace("\"", "\\\"") + "\"";
            return s;
        }
    }

    /// <summary>
    /// Represents a CUPS printer discovered on the system.
    /// </summary>
    public class CupsPrinter
    {
        public string Name { get; set; } = "";
        public bool IsEnabled { get; set; }
        public bool IsDefault { get; set; }

        public override string ToString()
            => $"{Name} (Enabled={IsEnabled}, Default={IsDefault})";
    }
}
