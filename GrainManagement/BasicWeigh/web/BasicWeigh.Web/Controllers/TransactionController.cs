using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using BasicWeigh.Web.Data;
using BasicWeigh.Web.Hubs;
using BasicWeigh.Web.Models;
using BasicWeigh.Web.Services;

namespace BasicWeigh.Web.Controllers;

public class TransactionController : Controller
{
    private readonly ScaleDbContext _db;
    private readonly IScaleService _scaleService;
    private readonly PrintQueueService _printQueue;
    private readonly IHubContext<ScaleHub> _hub;
    private readonly AppSetupCache _setupCache;

    public TransactionController(ScaleDbContext db, IScaleService scaleService,
        PrintQueueService printQueue, IHubContext<ScaleHub> hub, AppSetupCache setupCache)
    {
        _db = db;
        _scaleService = scaleService;
        _printQueue = printQueue;
        _hub = hub;
        _setupCache = setupCache;
    }

    private void PopulateDropdowns()
    {
        ViewBag.Customers = _db.Customers.Where(c => c.Active).OrderBy(c => c.CustomerName).ToList();

        // Carriers = dedicated carriers + all active customers (any customer can be a carrier)
        var carrierNames = _db.Carriers.Where(c => c.Active).Select(c => c.CarrierName).ToList();
        var customerNames = _db.Customers.Where(c => c.Active).Select(c => c.CustomerName).ToList();
        ViewBag.CarrierOptions = carrierNames.Union(customerNames).OrderBy(n => n).ToList();

        // Trucks loaded via AJAX based on selected carrier
        ViewBag.Commodities = _db.Commodities.Where(c => c.Active).OrderBy(c => c.CommodityName).ToList();
        ViewBag.Locations = _db.Locations.Where(l => l.Active).OrderBy(l => l.LocationName).ToList();
        ViewBag.Destinations = _db.Destinations.Where(d => d.Active).OrderBy(d => d.DestinationName).ToList();
    }

    // GET: Transaction/WeighIn (new)
    // GET: Transaction/WeighIn/5 (edit existing inbound)
    public IActionResult WeighIn(string? id)
    {
        PopulateDropdowns();
        ViewBag.CurrentWeight = _scaleService.GetCurrentWeight();

        var setup = _setupCache.Get();
        ViewBag.SavePicture = setup.SavePicture;

        if (!string.IsNullOrEmpty(id))
        {
            var existing = _db.Transactions.Find(id);
            if (existing == null) return NotFound();
            if (existing.DateOut != null) return RedirectToAction("Edit", new { id });

            ViewBag.IsEdit = true;
            if (setup.SavePicture)
            {
                var imgDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "tickets");
                ViewBag.HasInPhoto = System.IO.File.Exists(Path.Combine(imgDir, $"{id}_In.jpg"));
            }
            return View(existing);
        }

        ViewBag.NextTicket = setup.TicketNumber.ToString();
        ViewBag.IsEdit = false;

        var txn = new Transaction();
        // Recall last ticket values if enabled
        if (setup.RecallLastValues)
        {
            var lastTxn = _db.Transactions
                .OrderByDescending(t => t.DateIn)
                .FirstOrDefault();
            if (lastTxn != null)
            {
                txn.Commodity = lastTxn.Commodity;
                txn.Customer = lastTxn.Customer;
                txn.Location = lastTxn.Location;
                txn.Destination = lastTxn.Destination;
            }
        }
        return View(txn);
    }

    // POST: Transaction/WeighIn
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> WeighIn(Transaction transaction, bool isEdit, bool goToWeighOut, bool manualWeight)
    {
        if (isEdit)
        {
            var existing = _db.Transactions.Find(transaction.Ticket);
            if (existing == null) return NotFound();

            existing.InWeight = transaction.InWeight;
            existing.ManualInbound = manualWeight;
            existing.DateIn = transaction.DateIn;
            existing.Customer = transaction.Customer;
            existing.Carrier = transaction.Carrier;
            existing.TruckId = transaction.TruckId;
            existing.Commodity = transaction.Commodity;
            existing.Location = transaction.Location;
            existing.Destination = transaction.Destination;
            existing.Notes = transaction.Notes;

            _db.SaveChanges();

            if (goToWeighOut)
                return RedirectToAction("WeighOut", new { id = transaction.Ticket });
        }
        else
        {
            var setup = _db.AppSetup.First();
            while (_db.Transactions.Any(t => t.Ticket == setup.TicketNumber.ToString()))
            {
                setup.TicketNumber++;
            }
            transaction.Ticket = setup.TicketNumber.ToString();
            transaction.DateIn = transaction.DateIn == default ? DateTime.Now : transaction.DateIn;
            transaction.Void = false;
            transaction.ManualInbound = manualWeight;

            setup.TicketNumber++;
            _db.AppSetup.Update(setup);

            _db.Transactions.Add(transaction);
            _db.SaveChanges();
            _setupCache.Invalidate();

            // Camera capture on inbound (only if not manual weight)
            if (setup.SavePicture && !manualWeight && !string.IsNullOrEmpty(setup.InboundCameraId))
            {
                var parts = setup.InboundCameraId.Split(':', 2);
                var serviceId = parts.Length > 1 ? parts[0] : "default";
                var cameraId = parts.Length > 1 ? parts[1] : parts[0];
                await _hub.Clients.Group($"Camera_{serviceId}").SendAsync("CaptureImage",
                    new { ticket = transaction.Ticket, direction = "in", cameraId });
            }

            // Auto-print to configured inbound printer
            var inboundPrinter = setup.InboundPrinterId;
            if (!string.IsNullOrEmpty(inboundPrinter))
            {
                if (inboundPrinter.Equals("Browser:Browser", StringComparison.OrdinalIgnoreCase))
                {
                    TempData["AutoPrintTicket"] = transaction.Ticket;
                    TempData["AutoPrintType"] = "weighin";
                }
                else
                {
                    var parts2 = inboundPrinter.Split(':', 2);
                    var svcId = parts2.Length > 1 ? parts2[0] : "";
                    var pName = parts2.Length > 1 ? parts2[1] : parts2[0];
                    var group = !string.IsNullOrEmpty(svcId) ? $"Print_{svcId}" : "PrintClients";
                    await _hub.Clients.Group(group).SendAsync("PrintTicket",
                        new { ticketId = transaction.Ticket, type = "weighin", printerId = pName });
                }
            }
        }

        return RedirectToAction("InboundTrucks");
    }

    // GET: Transaction/WeighOut/5
    public IActionResult WeighOut(string id)
    {
        var transaction = _db.Transactions.Find(id);
        if (transaction == null) return NotFound();

        ViewBag.CurrentWeight = _scaleService.GetCurrentWeight();
        PopulateDropdowns();
        return View(transaction);
    }

    // POST: Transaction/WeighOut/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> WeighOut(string id, Transaction transaction, bool manualOutWeight, bool manualInWeight)
    {
        var existing = _db.Transactions.Find(id);
        if (existing == null) return NotFound();

        existing.InWeight = transaction.InWeight;
        existing.ManualInbound = manualInWeight;
        existing.OutWeight = transaction.OutWeight;
        existing.ManualOutbound = manualOutWeight;
        existing.DateOut = transaction.DateOut ?? DateTime.Now;
        existing.DateIn = transaction.DateIn;
        existing.Customer = transaction.Customer;
        existing.Carrier = transaction.Carrier;
        existing.TruckId = transaction.TruckId;
        existing.Commodity = transaction.Commodity;
        existing.Location = transaction.Location;
        existing.Destination = transaction.Destination;
        existing.Notes = transaction.Notes;

        _db.SaveChanges();

        // Remote printing
        var setup = _setupCache.Get();

        // Camera capture on outbound (only if not manual weight)
        if (setup.SavePicture && !manualOutWeight && !string.IsNullOrEmpty(setup.OutboundCameraId))
        {
            var parts = setup.OutboundCameraId.Split(':', 2);
            var serviceId = parts.Length > 1 ? parts[0] : "default";
            var cameraId = parts.Length > 1 ? parts[1] : parts[0];
            await _hub.Clients.Group($"Camera_{serviceId}").SendAsync("CaptureImage",
                new { ticket = id, direction = "out", cameraId });
        }

        // Auto-print to configured outbound printer
        var outboundPrinter = setup.OutboundPrinterId;
        if (!string.IsNullOrEmpty(outboundPrinter))
        {
            if (outboundPrinter.Equals("Browser:Browser", StringComparison.OrdinalIgnoreCase))
            {
                // Redirect to ticket view with auto-print — opens in new window via CompletedTrucks
                TempData["AutoPrintTicket"] = id;
            }
            else
            {
                var parts2 = outboundPrinter.Split(':', 2);
                var svcId = parts2.Length > 1 ? parts2[0] : "";
                var pName = parts2.Length > 1 ? parts2[1] : parts2[0];
                var group = !string.IsNullOrEmpty(svcId) ? $"Print_{svcId}" : "PrintClients";
                await _hub.Clients.Group(group).SendAsync("PrintTicket",
                    new { ticketId = id, type = "weighout", printerId = pName });
            }
        }

        return RedirectToAction("View", "Ticket", new { id });
    }

    // GET: Transaction/InboundTrucks
    public IActionResult InboundTrucks()
    {
        var setup = _setupCache.Get();
        ViewBag.RemotePrintMode = setup.RemotePrintMode ?? "None";
        ViewBag.KioskCount = setup.KioskCount;
        ViewBag.DemoMode = setup.DemoMode;
        ViewBag.SavePicture = setup.SavePicture;
        return View();
    }

    // GET: Transaction/CompletedTrucks
    public IActionResult CompletedTrucks()
    {
        var setup = _setupCache.Get();
        ViewBag.RemotePrintMode = setup.RemotePrintMode ?? "None";
        ViewBag.KioskCount = setup.KioskCount;
        ViewBag.DemoMode = setup.DemoMode;
        ViewBag.UseQuickBooks = setup.UseQuickBooks;
        ViewBag.SavePicture = setup.SavePicture;
        return View();
    }

    // GET: Transaction/BasicTicket
    public IActionResult BasicTicket()
    {
        var setup = _setupCache.Get();
        ViewBag.NextTicket = setup.TicketNumber.ToString();
        ViewBag.CurrentWeight = _scaleService.GetCurrentWeight();
        PopulateDropdowns();
        return View();
    }

    // POST: Transaction/BasicTicket
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult BasicTicket(Transaction transaction)
    {
        var setup = _db.AppSetup.First();
        while (_db.Transactions.Any(t => t.Ticket == setup.TicketNumber.ToString()))
        {
            setup.TicketNumber++;
        }
        transaction.Ticket = setup.TicketNumber.ToString();
        transaction.DateIn = transaction.DateIn == default ? DateTime.Now : transaction.DateIn;
        transaction.DateOut = transaction.DateIn;
        transaction.OutWeight = transaction.InWeight;
        transaction.Void = false;

        setup.TicketNumber++;
        _db.AppSetup.Update(setup);

        _db.Transactions.Add(transaction);
        _db.SaveChanges();
        _setupCache.Invalidate();

        return RedirectToAction("CompletedTrucks", new { reset = "true" });
    }

    // POST: Transaction/Void/5
    [HttpPost("Transaction/Void/{id}")]
    public IActionResult ToggleVoid(string id)
    {
        var transaction = _db.Transactions.Find(id);
        if (transaction == null) return NotFound();

        transaction.Void = !transaction.Void;
        _db.SaveChanges();

        return Json(new { success = true, isVoid = transaction.Void });
    }

// POST: Transaction/DeleteInbound/5
    [HttpPost("Transaction/DeleteInbound/{id}")]
    public IActionResult DeleteInbound(string id)
    {
        var transaction = _db.Transactions.Find(id);
        if (transaction == null) return NotFound();

        _db.Transactions.Remove(transaction);
        _db.SaveChanges();

        return Json(new { success = true });
    }

    // GET: Transaction/Edit/5
    public IActionResult Edit(string id)
    {
        var transaction = _db.Transactions.Find(id);
        if (transaction == null) return NotFound();

        var setup = _setupCache.Get();
        ViewBag.SavePicture = setup.SavePicture;
        var ticketsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "tickets");
        ViewBag.HasInImage = System.IO.File.Exists(Path.Combine(ticketsDir, $"{id}_in.jpg"));
        ViewBag.HasOutImage = System.IO.File.Exists(Path.Combine(ticketsDir, $"{id}_out.jpg"));

        PopulateDropdowns();
        return View(transaction);
    }

    // POST: Transaction/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(string id, Transaction transaction)
    {
        if (id != transaction.Ticket) return BadRequest();

        var existing = _db.Transactions.Find(id);
        if (existing == null) return NotFound();

        existing.InWeight = transaction.InWeight;
        existing.OutWeight = transaction.OutWeight;
        existing.DateIn = transaction.DateIn;
        existing.DateOut = transaction.DateOut;
        existing.Customer = transaction.Customer;
        existing.Carrier = transaction.Carrier;
        existing.TruckId = transaction.TruckId;
        existing.Commodity = transaction.Commodity;
        existing.Location = transaction.Location;
        existing.Destination = transaction.Destination;
        existing.Notes = transaction.Notes;
        existing.Void = transaction.Void;
        existing.SentToQuickBooks = false; // Reset QB flag on edit

        _db.SaveChanges();

        return RedirectToAction("CompletedTrucks");
    }

    private static string TicketsImageDir => Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "tickets");

    private static bool HasImage(string ticket, string direction)
        => System.IO.File.Exists(Path.Combine(TicketsImageDir, $"{ticket}_{direction}.jpg"));

    // API: GET api/transactions/inbound
    [HttpGet("api/transactions/inbound")]
    public IActionResult GetInbound()
    {
        var transactions = _db.Transactions
            .Where(t => t.DateOut == null && !t.Void)
            .OrderByDescending(t => t.DateIn)
            .ToList()
            .Select(t => new
            {
                t.Ticket,
                t.Customer,
                t.Carrier,
                t.TruckId,
                t.Commodity,
                t.InWeight,
                t.DateIn,
                t.Location,
                t.Destination,
                t.Notes,
                t.ManualInbound,
                HasInImage = HasImage(t.Ticket, "in")
            })
            .ToList();

        return Json(transactions);
    }

    // API: GET api/transactions/completed
    [HttpGet("api/transactions/completed")]
    public IActionResult GetCompleted(DateTime? startDate, DateTime? endDate)
    {
        var start = startDate ?? DateTime.Today.AddDays(-30);
        var end = endDate ?? DateTime.Today;
        // Make end date inclusive — include all tickets through end of that day
        var endInclusive = end.Date.AddDays(1);

        var transactions = _db.Transactions
            .Where(t => t.DateOut != null && t.DateIn >= start && t.DateIn < endInclusive)
            .OrderByDescending(t => t.DateIn)
            .ToList()
            .Select(t => new
            {
                t.Ticket,
                t.Customer,
                t.Carrier,
                t.TruckId,
                t.Commodity,
                t.InWeight,
                t.OutWeight,
                t.NetWeight,
                t.GrossWeight,
                t.TareWeight,
                t.DateIn,
                t.DateOut,
                t.Location,
                t.Destination,
                t.Notes,
                t.Void,
                t.SentToQuickBooks,
                HasInImage = HasImage(t.Ticket, "in"),
                HasOutImage = HasImage(t.Ticket, "out")
            })
            .ToList();

        return Json(transactions);
    }

    // API: POST api/transactions/update
    [HttpPost("api/transactions/update")]
    public IActionResult UpdateTransaction([FromBody] Transaction transaction)
    {
        var existing = _db.Transactions.Find(transaction.Ticket);
        if (existing == null) return NotFound();

        existing.InWeight = transaction.InWeight;
        existing.OutWeight = transaction.OutWeight;
        existing.DateIn = transaction.DateIn;
        existing.DateOut = transaction.DateOut;
        existing.Customer = transaction.Customer;
        existing.Carrier = transaction.Carrier;
        existing.TruckId = transaction.TruckId;
        existing.Commodity = transaction.Commodity;
        existing.Location = transaction.Location;
        existing.Destination = transaction.Destination;
        existing.Notes = transaction.Notes;
        existing.Void = transaction.Void;
        existing.SentToQuickBooks = false; // Reset QB flag on edit

        _db.SaveChanges();

        return Json(new { success = true });
    }

    // API: POST api/transactions/mark-sent-to-qb
    [HttpPost("api/transactions/mark-sent-to-qb")]
    public IActionResult MarkSentToQuickBooks([FromBody] List<string> ticketIds)
    {
        if (ticketIds == null || ticketIds.Count == 0)
            return BadRequest(new { error = "No ticket IDs provided." });

        var tickets = _db.Transactions
            .Where(t => ticketIds.Contains(t.Ticket))
            .ToList();

        foreach (var t in tickets)
            t.SentToQuickBooks = true;

        _db.SaveChanges();
        return Json(new { marked = tickets.Count });
    }

    // API: POST api/ticket/{id}/image?direction=in|out — upload ticket image
    [HttpPost("api/ticket/{id}/image")]
    public async Task<IActionResult> UploadTicketImage(string id, [FromQuery] string direction, IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { error = "No file provided." });
        if (direction != "in" && direction != "out")
            return BadRequest(new { error = "Direction must be 'in' or 'out'." });

        var dir = TicketsImageDir;
        Directory.CreateDirectory(dir);

        var filePath = Path.Combine(dir, $"{id}_{direction}.jpg");
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Notify all web clients that an image is available
        await _hub.Clients.All.SendAsync("ImageCaptured", new { ticket = id, direction });

        return Ok(new { success = true });
    }

    // API: GET api/ticket/{id}/image?direction=in|out — serve ticket image
    [HttpGet("api/ticket/{id}/image")]
    public IActionResult GetTicketImage(string id, [FromQuery] string direction)
    {
        if (direction != "in" && direction != "out")
            return BadRequest();

        var filePath = Path.Combine(TicketsImageDir, $"{id}_{direction}.jpg");
        if (!System.IO.File.Exists(filePath))
            return NotFound();

        return PhysicalFile(filePath, "image/jpeg");
    }
}
