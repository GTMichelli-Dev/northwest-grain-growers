#nullable enable
using System.Threading.Tasks;

public interface IReportPrinter
{
    Task<(bool ok, string? message, string? resolvedPrinter)> PrintInboundTicketAsync(long ticket, string desiredPrinterName = "Kiosk");
}
 #nullable restore