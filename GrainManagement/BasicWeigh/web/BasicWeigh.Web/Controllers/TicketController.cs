using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using BasicWeigh.Web.Data;
using BasicWeigh.Web.Hubs;
using BasicWeigh.Web.Models;
using BasicWeigh.Web.Reports;
using BasicWeigh.Web.Services;
using DevExpress.XtraReports.UI;
using DevExpress.Drawing;

namespace BasicWeigh.Web.Controllers;

public class TicketController : Controller
{
    private readonly ScaleDbContext _db;
    private readonly PrintQueueService _printQueue;
    private readonly IHubContext<ScaleHub> _hub;
    private readonly AppSetupCache _setupCache;
    private static readonly string ReportPath = Path.Combine(
        Directory.GetCurrentDirectory(), "Reports", "TicketReport.repx");

    public TicketController(ScaleDbContext db, PrintQueueService printQueue, IHubContext<ScaleHub> hub, AppSetupCache setupCache)
    {
        _db = db;
        _printQueue = printQueue;
        _hub = hub;
        _setupCache = setupCache;
    }

    public IActionResult Print(string id)
    {
        var transaction = _db.Transactions.Find(id);
        if (transaction == null) return NotFound();

        var setup = _setupCache.Get();
        ViewBag.Header1 = setup.Header1;
        ViewBag.Header2 = setup.Header2;
        ViewBag.Header3 = setup.Header3;
        ViewBag.Header4 = setup.Header4;
        ViewBag.Theme = setup.Theme ?? "default";
        ViewBag.SavePicture = setup.SavePicture;

        // Check if camera images exist for this ticket
        var imgDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "tickets");
        ViewBag.HasInPhoto = System.IO.File.Exists(Path.Combine(imgDir, $"{id}_In.jpg"));
        ViewBag.HasOutPhoto = System.IO.File.Exists(Path.Combine(imgDir, $"{id}_Out.jpg"));

        return View(transaction);
    }

    [HttpGet("api/ticket/{id}")]
    public IActionResult GetTicketData(string id)
    {
        var transaction = _db.Transactions.Find(id);
        if (transaction == null) return NotFound();

        var setup = _setupCache.Get();

        // If this ticket was awaiting remote print confirmation, broadcast it
        if (_printQueue.TryConfirm(id))
        {
            var label = setup.RemotePrintMode == "Scale" ? "Scale" : "Remote Printer";
            _ = _hub.Clients.All.SendAsync("PrintConfirmed", new { ticketId = id, printer = label });
        }

        return Json(new
        {
            ticket = transaction.Ticket,
            dateIn = transaction.DateIn.ToString("MM/dd/yyyy hh:mm tt"),
            dateOut = transaction.DateOut?.ToString("MM/dd/yyyy hh:mm tt"),
            customer = transaction.Customer,
            carrier = transaction.Carrier,
            truckId = transaction.TruckId,
            commodity = transaction.Commodity,
            location = transaction.Location,
            destination = transaction.Destination,
            grossWeight = transaction.GrossWeight,
            tareWeight = transaction.TareWeight,
            netWeight = transaction.NetWeight,
            notes = transaction.Notes,
            isVoid = transaction.Void,
            header1 = setup.Header1,
            header2 = setup.Header2,
            header3 = setup.Header3,
            header4 = setup.Header4
        });
    }

    // Report Designer - edit a ticket template
    public IActionResult Designer(string reportName = "TicketReport")
    {
        return View("Designer", reportName);
    }

    // Document Viewer - preview/print a specific ticket
    public new IActionResult View(string id)
    {
        var transaction = _db.Transactions.Find(id);
        if (transaction == null) return NotFound();

        var setup = _setupCache.Get();

        // Load report template
        XtraReport report;
        if (System.IO.File.Exists(ReportPath))
        {
            report = XtraReport.FromFile(ReportPath);
        }
        else
        {
            report = new TicketReport();
        }

        // Set logo image on the picture box
        SetLogoImage(report, setup);

        // Set parameters
        SetParam(report, "Ticket", transaction.Ticket);
        SetParam(report, "DateIn", transaction.DateIn.ToString("MM/dd/yyyy hh:mm tt"));
        SetParam(report, "DateOut", transaction.DateOut?.ToString("MM/dd/yyyy hh:mm tt") ?? "");
        SetParam(report, "Customer", transaction.Customer ?? "");
        SetParam(report, "Carrier", transaction.Carrier ?? "");
        SetParam(report, "TruckId", transaction.TruckId ?? "");
        SetParam(report, "Commodity", transaction.Commodity ?? "");
        SetParam(report, "Location", transaction.Location ?? "");
        SetParam(report, "Destination", transaction.Destination ?? "");
        SetParam(report, "GrossWeight", transaction.GrossWeight.ToString("#,##0") + " lb");
        SetParam(report, "TareWeight", transaction.TareWeight.ToString("#,##0") + " lb");
        SetParam(report, "NetWeight", transaction.NetWeight.ToString("#,##0") + " lb");
        SetParam(report, "Notes", transaction.Notes ?? "");
        SetParam(report, "IsVoid", transaction.Void);
        SetParam(report, "Header1", setup.Header1 ?? "");
        SetParam(report, "Header2", setup.Header2 ?? "");
        SetParam(report, "Header3", setup.Header3 ?? "");
        SetParam(report, "Header4", setup.Header4 ?? "");

        ViewBag.TicketId = id;
        ViewBag.Header1 = setup.Header1 ?? "Basic Weigh";
        ViewBag.Theme = setup.Theme ?? "default";
        return View(report);
    }

    // Kiosk Ticket - print inbound kiosk ticket with barcode
    public IActionResult KioskView(string id)
    {
        var transaction = _db.Transactions.Find(id);
        if (transaction == null) return NotFound();

        var setup = _setupCache.Get();

        var kioskPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "KioskTicketReport.repx");
        XtraReport report;
        if (System.IO.File.Exists(kioskPath))
        {
            report = XtraReport.FromFile(kioskPath);
        }
        else
        {
            report = new KioskTicketReport();
        }

        SetParam(report, "Ticket", transaction.Ticket);
        SetParam(report, "DateIn", transaction.DateIn.ToString("MM/dd/yyyy hh:mm tt"));
        SetParam(report, "Customer", transaction.Customer ?? "");
        SetParam(report, "Carrier", transaction.Carrier ?? "");
        SetParam(report, "TruckId", transaction.TruckId ?? "");
        SetParam(report, "Commodity", transaction.Commodity ?? "");
        SetParam(report, "Location", transaction.Location ?? "");
        SetParam(report, "InWeight", transaction.InWeight.ToString("#,##0") + " lb");
        SetParam(report, "Header1", setup.Header1 ?? "");
        SetParam(report, "Header2", setup.Header2 ?? "");
        SetParam(report, "Header3", setup.Header3 ?? "");
        SetParam(report, "Header4", setup.Header4 ?? "");

        ViewBag.TicketId = id;
        return View(report);
    }

    [HttpGet("api/ticket/{id}/pdf")]
    public IActionResult GetTicketPdf(string id)
    {
        var transaction = _db.Transactions.Find(id);
        if (transaction == null) return NotFound();

        var setup = _setupCache.Get();

        // Use KioskTicketReport for inbound (not yet completed), TicketReport for completed
        bool isInbound = transaction.DateOut == null;

        XtraReport report;
        if (isInbound)
        {
            var kioskPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "KioskTicketReport.repx");
            report = System.IO.File.Exists(kioskPath)
                ? XtraReport.FromFile(kioskPath)
                : new KioskTicketReport();

            SetParam(report, "Ticket", transaction.Ticket);
            SetParam(report, "DateIn", transaction.DateIn.ToString("MM/dd/yyyy hh:mm tt"));
            SetParam(report, "Customer", transaction.Customer ?? "");
            SetParam(report, "Carrier", transaction.Carrier ?? "");
            SetParam(report, "TruckId", transaction.TruckId ?? "");
            SetParam(report, "Commodity", transaction.Commodity ?? "");
            SetParam(report, "Location", transaction.Location ?? "");
            SetParam(report, "InWeight", transaction.InWeight.ToString("#,##0") + " lb");
            SetParam(report, "Header1", setup.Header1 ?? "");
            SetParam(report, "Header2", setup.Header2 ?? "");
            SetParam(report, "Header3", setup.Header3 ?? "");
            SetParam(report, "Header4", setup.Header4 ?? "");
        }
        else
        {
            report = System.IO.File.Exists(ReportPath)
                ? XtraReport.FromFile(ReportPath)
                : new TicketReport();

            SetLogoImage(report, setup);
            SetParam(report, "Ticket", transaction.Ticket);
            SetParam(report, "DateIn", transaction.DateIn.ToString("MM/dd/yyyy hh:mm tt"));
            SetParam(report, "DateOut", transaction.DateOut?.ToString("MM/dd/yyyy hh:mm tt") ?? "");
            SetParam(report, "Customer", transaction.Customer ?? "");
            SetParam(report, "Carrier", transaction.Carrier ?? "");
            SetParam(report, "TruckId", transaction.TruckId ?? "");
            SetParam(report, "Commodity", transaction.Commodity ?? "");
            SetParam(report, "Location", transaction.Location ?? "");
            SetParam(report, "Destination", transaction.Destination ?? "");
            SetParam(report, "GrossWeight", transaction.GrossWeight.ToString("#,##0") + " lb");
            SetParam(report, "TareWeight", transaction.TareWeight.ToString("#,##0") + " lb");
            SetParam(report, "NetWeight", transaction.NetWeight.ToString("#,##0") + " lb");
            SetParam(report, "Notes", transaction.Notes ?? "");
            SetParam(report, "IsVoid", transaction.Void);
            SetParam(report, "Header1", setup.Header1 ?? "");
            SetParam(report, "Header2", setup.Header2 ?? "");
            SetParam(report, "Header3", setup.Header3 ?? "");
            SetParam(report, "Header4", setup.Header4 ?? "");
        }

        using var ms = new MemoryStream();
        report.ExportToPdf(ms);
        ms.Position = 0;

        // If this ticket was awaiting remote print confirmation, broadcast it
        if (_printQueue.TryConfirm(id))
        {
            var label = setup.RemotePrintMode == "Scale" ? "Scale" : "Remote Printer";
            _ = _hub.Clients.All.SendAsync("PrintConfirmed", new { ticketId = id, printer = label });
        }

        return File(ms.ToArray(), "application/pdf", $"ticket-{id}.pdf");
    }

    /// <summary>
    /// Reprint a ticket via Scale or Remote Printer.
    /// </summary>
    [HttpPost("api/ticket/{id}/reprint")]
    public async Task<IActionResult> Reprint(string id)
    {
        var transaction = _db.Transactions.Find(id);
        if (transaction == null) return NotFound();

        var setup = _setupCache.Get();

        if (setup.RemotePrintMode == "Scale")
        {
            _printQueue.Enqueue(id);
            _printQueue.AwaitConfirmation(id);
        }
        else if (setup.RemotePrintMode == "RemotePrinter")
        {
            _printQueue.Enqueue(id);
            _printQueue.AwaitConfirmation(id);
            await _hub.Clients.Group("PrintClients").SendAsync("PrintTicket",
                new { ticketId = id, pdfUrl = $"/api/ticket/{id}/pdf" });
        }
        else
        {
            return BadRequest(new { success = false, message = "Remote printing is not enabled." });
        }

        return Json(new { success = true, mode = setup.RemotePrintMode });
    }

    private void SetLogoImage(XtraReport report, AppSetup setup)
    {
        var picBox = report.FindControl("picLogo", true) as XRPictureBox;
        if (picBox == null) return;

        byte[]? iconBytes = setup.Icon;
        string? contentType = setup.IconContentType;

        // If custom icon is SVG, skip it (DevExpress can't render SVG in XRPictureBox)
        if (iconBytes != null && contentType != null && contentType.Contains("svg"))
        {
            iconBytes = null;
        }

        if (iconBytes == null)
        {
            // Fall back to default PNG icon
            var defaultPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "default-icon.png");
            if (System.IO.File.Exists(defaultPath))
                iconBytes = System.IO.File.ReadAllBytes(defaultPath);
        }

        if (iconBytes == null) return;

        try
        {
            using var ms = new MemoryStream(iconBytes);
            var dxImage = DXImage.FromStream(ms);
            picBox.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource(dxImage);
        }
        catch
        {
            // Unsupported image format - skip logo
        }
    }

    private void SetParam(XtraReport report, string name, object value)
    {
        var param = report.Parameters[name];
        if (param != null)
            param.Value = value;
    }
}
