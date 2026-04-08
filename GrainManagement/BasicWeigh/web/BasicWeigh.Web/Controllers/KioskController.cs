using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using BasicWeigh.Web.Data;
using BasicWeigh.Web.Hubs;
using BasicWeigh.Web.Models;
using BasicWeigh.Web.Services;

namespace BasicWeigh.Web.Controllers;

public class KioskController : Controller
{
    private readonly ScaleDbContext _db;
    private readonly IScaleService _scaleService;
    private readonly IHubContext<ScaleHub> _hub;
    private readonly AppSetupCache _setupCache;

    public KioskController(ScaleDbContext db, IScaleService scaleService, IHubContext<ScaleHub> hub, AppSetupCache setupCache)
    {
        _db = db;
        _scaleService = scaleService;
        _hub = hub;
        _setupCache = setupCache;
    }

    public IActionResult Index([FromQuery(Name = "service-id")] string? serviceId = null,
                               [FromQuery(Name = "printer-id")] string? printerId = null)
    {
        var setup = _setupCache.Get();
        ViewBag.ServiceId = serviceId ?? "";
        ViewBag.PrinterId = printerId ?? "";
        ViewBag.HasPrinter = !string.IsNullOrEmpty(serviceId) && !string.IsNullOrEmpty(printerId);
        ViewBag.ScaleId = setup.ScaleId ?? "";
        return View(setup);
    }

    [HttpGet("api/kiosk/lists")]
    public IActionResult GetLists()
    {
        var commodities = _db.Commodities
            .Where(c => c.Active && c.UseAtKiosk)
            .OrderBy(c => c.CommodityName)
            .Select(c => c.CommodityName)
            .ToList();

        var customers = _db.Customers
            .Where(c => c.Active && c.UseAtKiosk)
            .OrderBy(c => c.CustomerName)
            .Select(c => c.CustomerName)
            .ToList();

        // Carriers = dedicated carriers + all active kiosk customers
        var carrierNames = _db.Carriers
            .Where(c => c.Active && c.UseAtKiosk)
            .Select(c => c.CarrierName)
            .ToList();
        var customerNames = _db.Customers
            .Where(c => c.Active && c.UseAtKiosk)
            .Select(c => c.CustomerName)
            .ToList();
        var carriers = carrierNames.Union(customerNames).OrderBy(n => n).ToList();

        var locations = _db.Locations
            .Where(l => l.Active && l.UseAtKiosk)
            .OrderBy(l => l.LocationName)
            .Select(l => l.LocationName)
            .ToList();

        var destinations = _db.Destinations
            .Where(d => d.Active && d.UseAtKiosk)
            .OrderBy(d => d.DestinationName)
            .Select(d => d.DestinationName)
            .ToList();

        return Json(new { commodities, customers, carriers, locations, destinations });
    }

    [HttpGet("api/kiosk/trucks/{carrier}")]
    public IActionResult GetTrucks(string carrier)
    {
        var trucks = _db.Trucks
            .Where(t => t.CarrierName == carrier && t.UseAtKiosk)
            .OrderBy(t => t.TruckId)
            .Select(t => t.TruckId)
            .ToList();

        return Json(trucks);
    }

    [HttpGet("api/kiosk/ticket/{ticketNumber}")]
    public IActionResult FindTicket(string ticketNumber)
    {
        var transaction = _db.Transactions
            .FirstOrDefault(t => t.Ticket == ticketNumber);

        if (transaction == null)
            return NotFound(new { message = "Ticket not found" });

        if (transaction.Void)
            return BadRequest(new { message = "Ticket is voided" });

        if (transaction.DateOut != null)
            return BadRequest(new { message = "Ticket already completed" });

        return Json(new
        {
            ticket = transaction.Ticket,
            inWeight = transaction.InWeight,
            dateIn = transaction.DateIn,
            customer = transaction.Customer,
            carrier = transaction.Carrier,
            truckId = transaction.TruckId,
            commodity = transaction.Commodity,
            location = transaction.Location,
            destination = transaction.Destination
        });
    }

    [HttpPost("api/kiosk/weighin")]
    public async Task<IActionResult> WeighIn([FromBody] KioskWeighInRequest request)
    {
        var setup = _db.AppSetup.First();
        // Ensure ticket number doesn't collide with existing tickets
        while (_db.Transactions.Any(t => t.Ticket == setup.TicketNumber.ToString()))
        {
            setup.TicketNumber++;
        }
        var ticketNumber = setup.TicketNumber.ToString();

        var transaction = new Transaction
        {
            Ticket = ticketNumber,
            InWeight = request.Weight,
            DateIn = DateTime.Now,
            Commodity = request.Commodity,
            Customer = request.Customer,
            Carrier = request.Carrier,
            TruckId = request.TruckId,
            Location = request.Location,
            Destination = request.Destination,
            Void = false,
            ManualInbound = false
        };

        setup.TicketNumber++;
        _db.AppSetup.Update(setup);
        _db.Transactions.Add(transaction);
        _db.SaveChanges();
        _setupCache.Invalidate();

        // Notify all clients that a ticket was created
        await _hub.Clients.All.SendAsync("TicketCreated", new { ticket = ticketNumber, type = "weighin" });

        // Print the ticket
        await SendPrintCommand(ticketNumber.ToString(), "weighin", request.PrinterId);

        return Json(new { ticket = ticketNumber });
    }

    [HttpPost("api/kiosk/weighout")]
    public async Task<IActionResult> WeighOut([FromBody] KioskWeighOutRequest request)
    {
        var transaction = _db.Transactions
            .FirstOrDefault(t => t.Ticket == request.Ticket && !t.Void && t.DateOut == null);

        if (transaction == null)
            return NotFound(new { message = "Ticket not found" });

        transaction.OutWeight = request.Weight;
        transaction.DateOut = DateTime.Now;
        transaction.ManualOutbound = false;

        if (!string.IsNullOrEmpty(request.Destination))
            transaction.Destination = request.Destination;

        _db.SaveChanges();

        // Notify all clients that a ticket was completed
        await _hub.Clients.All.SendAsync("TicketCompleted", new { ticket = transaction.Ticket, type = "weighout" });

        // Print the ticket
        await SendPrintCommand(transaction.Ticket.ToString(), "weighout", request.PrinterId);

        return Json(new { ticket = transaction.Ticket });
    }

    [HttpPost("api/kiosk/reprint/{ticketId}")]
    public async Task<IActionResult> Reprint(string ticketId, [FromQuery] string? printerId = null)
    {
        var transaction = _db.Transactions.Find(ticketId);
        if (transaction == null)
            return NotFound(new { message = "Ticket not found" });

        var type = transaction.DateOut != null ? "weighout" : "weighin";

        await SendPrintCommand(ticketId, type, printerId);

        return Ok(new { message = "Reprint requested" });
    }

    /// <summary>
    /// Sends a print command to the correct print service.
    /// printerId format: "serviceId:printerId" (e.g., "office-1:BIXOLON BK3-3")
    /// If not set and demo mode: uses "KioskPrinter"
    /// If not set and not demo: returns without printing (kiosk should prompt)
    /// </summary>
    private async Task SendPrintCommand(string ticketId, string type, string? printerId)
    {
        var setup = _setupCache.Get();

        // If no printer specified, use defaults
        if (string.IsNullOrEmpty(printerId))
        {
            if (setup.DemoMode)
            {
                // In demo mode, use a virtual "KioskPrinter" so the flow works
                printerId = "demo:KioskPrinter";
            }
            else
            {
                // Use the inbound/outbound printer assignment from setup
                printerId = type == "weighout"
                    ? setup.OutboundPrinterId
                    : setup.InboundPrinterId;
            }
        }

        if (string.IsNullOrEmpty(printerId)) return;

        // Browser printing — handled client-side, skip server-side print command
        if (printerId.Equals("Browser:Browser", StringComparison.OrdinalIgnoreCase)) return;

        // Split serviceId:printerId
        var parts = printerId.Split(':', 2);
        var serviceId = parts.Length > 1 ? parts[0] : "";
        var printerName = parts.Length > 1 ? parts[1] : parts[0];

        if (!string.IsNullOrEmpty(serviceId))
        {
            // Route to specific service
            await _hub.Clients.Group($"Print_{serviceId}").SendAsync("PrintTicket",
                new { ticketId, type, printerId = printerName });
        }
        else
        {
            // Broadcast to all print services
            await _hub.Clients.Group("PrintClients").SendAsync("PrintTicket",
                new { ticketId, type, printerId = printerName });
        }
    }

    public class KioskWeighInRequest
    {
        public int Weight { get; set; }
        public string? Commodity { get; set; }
        public string? Customer { get; set; }
        public string? Carrier { get; set; }
        public string? TruckId { get; set; }
        public string? Location { get; set; }
        public string? Destination { get; set; }
        /// <summary>
        /// Optional printer in "serviceId:printerId" format.
        /// If not set: demo mode uses "demo:KioskPrinter", normal mode uses inbound printer from Setup.
        /// </summary>
        public string? PrinterId { get; set; }
    }

    public class KioskWeighOutRequest
    {
        public string Ticket { get; set; } = string.Empty;
        public int Weight { get; set; }
        public string? Destination { get; set; }
        /// <summary>
        /// Optional printer in "serviceId:printerId" format.
        /// If not set: demo mode uses "demo:KioskPrinter", normal mode uses outbound printer from Setup.
        /// </summary>
        public string? PrinterId { get; set; }
    }
}
