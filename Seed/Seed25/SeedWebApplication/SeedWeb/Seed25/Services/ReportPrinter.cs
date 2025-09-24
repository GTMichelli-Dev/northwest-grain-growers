#nullable enable
// Services/ReportPrinter.cs
using System;
using System.Linq;
using System.Drawing.Printing;               // Windows-only
using System.Threading.Tasks;
using DevExpress.XtraReports.UI;
using Seed25.Report;                        // your InboundTicket report

public sealed class ReportPrinter : IReportPrinter
{
    public Task<(bool ok, string? message, string? resolvedPrinter)> PrintInboundTicketAsync(long ticket, string desiredPrinterName = "Kiosk")
    {
        // Discover a printer that matches "Kiosk"
        var printers = PrinterSettings.InstalledPrinters.Cast<string>().ToList();
        var resolved = printers.FirstOrDefault(p => string.Equals(p, desiredPrinterName, StringComparison.OrdinalIgnoreCase))
                    ?? printers.FirstOrDefault(p => p.Contains(desiredPrinterName, StringComparison.OrdinalIgnoreCase));

        if (resolved is null)
            return Task.FromResult<(bool ok, string? message, string? resolvedPrinter)>(
                (false,
                $"Printer '{desiredPrinterName}' not found. Installed: {string.Join(", ", printers)}",
                null));

        try
        {
            var rpt = new InboundTicket { RequestParameters = false };
            rpt.Parameters["Ticket"].Value = ticket;
            rpt.Parameters["Ticket"].Visible = false;

            rpt.CreateDocument();     // catch data/layout issues up front
            rpt.Print(resolved);      // send to server-side spooler

            return Task.FromResult<(bool ok, string? message, string? resolvedPrinter)>((true, null, resolved));
        }
        catch (Exception ex)
        {
            return Task.FromResult<(bool ok, string? message, string? resolvedPrinter)>((false, ex.ToString(), resolved));
        }
    }
}
#nullable restore