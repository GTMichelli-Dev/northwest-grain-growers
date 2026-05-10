#nullable enable
using GrainManagement.Auth;
using GrainManagement.Hubs;
using GrainManagement.Models;
using GrainManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace GrainManagement.API;

/// <summary>
/// Backend for the field-location kiosk (/Kiosk). The "ticket" is the
/// <see cref="InventoryTransactionDetail.TransactionId"/> printed on the
/// inbound load ticket — same id the existing print endpoints take.
/// </summary>
[ApiController]
[Route("api/Kiosk")]
[RequiresModule(nameof(ModuleOptions.Scales))]
public sealed class KioskApiController : ControllerBase
{
    private readonly dbContext _ctx;
    private readonly IWeightSheetNotifier _notifier;
    private readonly IHubContext<PrintHub> _printHub;
    private readonly ILogger<KioskApiController> _log;
    private readonly GrainManagement.Services.Camera.ICameraCaptureTrigger _cameraTrigger;

    public KioskApiController(
        dbContext ctx,
        IWeightSheetNotifier notifier,
        IHubContext<PrintHub> printHub,
        ILogger<KioskApiController> log,
        GrainManagement.Services.Camera.ICameraCaptureTrigger cameraTrigger)
    {
        _ctx = ctx;
        _notifier = notifier;
        _printHub = printHub;
        _log = log;
        _cameraTrigger = cameraTrigger;
    }

    public sealed class TicketLookupResponse
    {
        public long TransactionId { get; set; }
        public long WeightSheetId { get; set; }
        public long? WeightSheetAs400Id { get; set; }
        public int  LocationId { get; set; }
        public string WeightSheetType { get; set; } = "";
        public decimal InboundLbs { get; set; }
    }

    /// <summary>
    /// GET /api/Kiosk/ticket/{txnId} — validates the scanned ticket.
    /// Returns 404 with a "reason" field for "Cannot Find Ticket" or
    /// "Truck Already Weighed Out" so the kiosk can show the matching
    /// red overlay.
    /// </summary>
    [HttpGet("ticket/{txnId:long}")]
    public async Task<IActionResult> LookupTicket([FromRoute] long txnId, CancellationToken ct)
    {
        if (txnId <= 0) return BadRequest(new { reason = "Invalid ticket." });

        var detail = await _ctx.InventoryTransactionDetails.AsNoTracking()
            .FirstOrDefaultAsync(d => d.TransactionId == txnId, ct);

        if (detail is null)
            return NotFound(new { reason = "Cannot Find Ticket" });

        if (detail.IsVoided)
            return NotFound(new { reason = "Ticket Voided" });

        if (detail.EndQty.HasValue)
            return NotFound(new { reason = "Truck Already Weighed Out" });

        if (!detail.StartQty.HasValue)
            return NotFound(new { reason = "Ticket Has No Inbound Weight" });

        // Walk back to the parent WS via RefId (Guid) to provide the
        // location id + WS id the kiosk needs for downstream broadcasts.
        if (!string.Equals(detail.RefType, "WeightSheet", StringComparison.OrdinalIgnoreCase) || detail.RefId is null)
            return NotFound(new { reason = "Ticket Has No Weight Sheet" });

        var ws = await _ctx.WeightSheets.AsNoTracking()
            .Where(w => w.RowUid == detail.RefId)
            .Select(w => new { w.WeightSheetId, w.As400Id, w.LocationId, w.WeightSheetType, w.StatusId })
            .FirstOrDefaultAsync(ct);

        if (ws is null) return NotFound(new { reason = "Cannot Find Ticket" });
        if (ws.StatusId >= 3) return NotFound(new { reason = "Truck Already Weighed Out" });

        return Ok(new TicketLookupResponse
        {
            TransactionId      = detail.TransactionId,
            WeightSheetId      = ws.WeightSheetId,
            WeightSheetAs400Id = ws.As400Id > 0 ? ws.As400Id : null,
            LocationId         = ws.LocationId,
            WeightSheetType    = ws.WeightSheetType ?? "",
            InboundLbs         = detail.StartQty ?? 0m,
        });
    }

    public sealed class WeighOutDto
    {
        public long TransactionId { get; set; }
        public decimal WeightLbs { get; set; }
        /// <summary>Format: "serviceId:printerId" — the kiosk's resolved printer.</summary>
        public string? PrinterId { get; set; }
        public int? KioskLocationId { get; set; }
        public int? KioskScaleId { get; set; }
    }

    public sealed class WeighOutResponse
    {
        public long TransactionId { get; set; }
        public long WeightSheetId { get; set; }
        public int LocationId { get; set; }
        public decimal InboundLbs { get; set; }
        public decimal OutboundLbs { get; set; }
        public decimal NetLbs { get; set; }
        public DateTime CompletedAtUtc { get; set; }
    }

    /// <summary>
    /// POST /api/Kiosk/ticket/weighout — stamps EndQty + CompletedAt on the
    /// open load, then fires a print to the kiosk's printer (if supplied)
    /// and broadcasts the WS update so the operator's web UI refreshes.
    /// </summary>
    [HttpPost("ticket/weighout")]
    public async Task<IActionResult> WeighOut([FromBody] WeighOutDto dto, CancellationToken ct)
    {
        if (dto is null || dto.TransactionId <= 0)
            return BadRequest(new { message = "TransactionId required." });

        var detail = await _ctx.InventoryTransactionDetails
            .FirstOrDefaultAsync(d => d.TransactionId == dto.TransactionId, ct);

        if (detail is null)        return NotFound(new { reason = "Cannot Find Ticket" });
        if (detail.IsVoided)       return Conflict(new { reason = "Ticket Voided" });
        if (detail.EndQty.HasValue) return Conflict(new { reason = "Truck Already Weighed Out" });

        var nowUtc = DateTime.UtcNow;
        detail.EndQty                                    = dto.WeightLbs;
        detail.NetQty                                    = (detail.StartQty ?? 0m) - dto.WeightLbs >= 0
                                                              ? (detail.StartQty ?? 0m) - dto.WeightLbs
                                                              : dto.WeightLbs - (detail.StartQty ?? 0m);
        detail.CompletedAt                               = nowUtc;
        detail.UpdatedAt                                 = nowUtc;
        if (dto.KioskLocationId.HasValue && detail.EndQtyLocationQuantityMethodId is null)
            detail.EndQtyLocationQuantityMethodId = dto.KioskLocationId.Value;

        await _ctx.SaveChangesAsync(ct);

        // Resolve the WS for broadcast + reporting.
        var ws = string.Equals(detail.RefType, "WeightSheet", StringComparison.OrdinalIgnoreCase) && detail.RefId is not null
            ? await _ctx.WeightSheets.AsNoTracking()
                .Where(w => w.RowUid == detail.RefId)
                .Select(w => new { w.WeightSheetId, w.LocationId })
                .FirstOrDefaultAsync(ct)
            : null;

        if (ws is not null)
        {
            // Fire-and-forget broadcast so the WS dashboard / WS edit page
            // refresh as soon as the truck weighs out.
            _ = _notifier.NotifyAsync(ws.LocationId, ws.WeightSheetId, "kiosk-weigh-out", ct);

            // Camera capture — this is the moment the kiosk just finished the
            // ticket (weighed out). Pure fire-and-forget; the trigger swallows
            // its own errors so a missing camera never blocks the weigh-out.
            await _cameraTrigger.FireAsync(
                detail.TransactionId.ToString(), "out", ws.LocationId, scaleId: null, ct);
        }

        // Push the print to the kiosk's printer if one was provided. We
        // route via PrintHub.PrintTicket which the existing WebPrintService
        // (and the embedded PrintWorker) already handle.
        if (!string.IsNullOrWhiteSpace(dto.PrinterId))
        {
            await SendPrintToKioskAsync(dto.PrinterId!, detail.TransactionId, original: true, ct);
        }

        return Ok(new WeighOutResponse
        {
            TransactionId  = detail.TransactionId,
            WeightSheetId  = ws?.WeightSheetId ?? 0,
            LocationId     = ws?.LocationId ?? 0,
            InboundLbs     = detail.StartQty ?? 0m,
            OutboundLbs    = dto.WeightLbs,
            NetLbs         = detail.NetQty ?? 0m,
            CompletedAtUtc = nowUtc,
        });
    }

    public sealed class ReprintDto
    {
        public long TransactionId { get; set; }
        public string? PrinterId { get; set; }
    }

    /// <summary>POST /api/Kiosk/ticket/reprint — re-pushes the ticket to the kiosk's printer.</summary>
    [HttpPost("ticket/reprint")]
    public async Task<IActionResult> Reprint([FromBody] ReprintDto dto, CancellationToken ct)
    {
        if (dto is null || dto.TransactionId <= 0)
            return BadRequest(new { message = "TransactionId required." });
        if (string.IsNullOrWhiteSpace(dto.PrinterId))
            return BadRequest(new { message = "PrinterId required." });

        var exists = await _ctx.InventoryTransactionDetails.AsNoTracking()
            .AnyAsync(d => d.TransactionId == dto.TransactionId, ct);
        if (!exists) return NotFound(new { message = "Transaction not found." });

        await SendPrintToKioskAsync(dto.PrinterId!, dto.TransactionId, original: false, ct);
        return Ok(new { ok = true });
    }

    public sealed class PendingTicketResponse
    {
        public long TransactionId { get; set; }
        public long WeightSheetId { get; set; }
        public int  LocationId { get; set; }
        public string LocationName { get; set; } = "";
        public string Commodity { get; set; } = "";
        public string Account { get; set; } = "";
        public string TruckId { get; set; } = "";
        public decimal InboundLbs { get; set; }
        public decimal? OutboundLbs { get; set; }
        public decimal NetLbs { get; set; }
        public bool IsOutbound { get; set; }
        public DateTime? LastTouchedUtc { get; set; }
    }

    /// <summary>
    /// GET /api/Kiosk/PendingTicketForKiosk?wsId=&amp;kioskType=
    ///
    /// Returns the most recently-touched, non-voided load on the WS
    /// whose state matches the kiosk's direction:
    ///   * KioskType=0 (Single)   — newest load, either direction
    ///   * KioskType=1 (Inbound)  — newest "open" load (StartQty set, EndQty null)
    ///   * KioskType=2 (Outbound) — newest "closed" load (EndQty set)
    ///
    /// Used by the kiosk's wait-for-clerk flow: when the WarehouseHub
    /// fires <c>weightSheetUpdated</c> on a clerk save, the kiosk calls
    /// this endpoint and shows a lime-green print confirmation if the
    /// load matches its direction. Returns 204 No Content when nothing
    /// matches.
    /// </summary>
    [HttpGet("PendingTicketForKiosk")]
    public async Task<IActionResult> PendingTicketForKiosk(
        [FromQuery] long wsId,
        [FromQuery] int kioskType,
        CancellationToken ct)
    {
        if (wsId <= 0) return BadRequest(new { message = "wsId is required." });

        var ws = await _ctx.WeightSheets.AsNoTracking()
            .Where(w => w.WeightSheetId == wsId)
            .Select(w => new { w.WeightSheetId, w.RowUid, w.LocationId, w.LotId })
            .FirstOrDefaultAsync(ct);
        if (ws is null) return NoContent();

        var locationName = await _ctx.Locations.AsNoTracking()
            .Where(l => l.LocationId == ws.LocationId)
            .Select(l => l.Name)
            .FirstOrDefaultAsync(ct) ?? "";

        // Newest "matching" load on this WS. We rank by CompletedAt then
        // StartedAt (descending) so the freshly-touched row wins.
        var qb = _ctx.InventoryTransactionDetails.AsNoTracking()
            .Where(d => d.RefId == ws.RowUid && d.RefType == "WeightSheet" && !d.IsVoided);

        // KioskType direction filter — see XML doc above.
        if (kioskType == 1)        // Inbound
            qb = qb.Where(d => d.StartQty != null && d.EndQty == null);
        else if (kioskType == 2)   // Outbound
            qb = qb.Where(d => d.EndQty != null);
        // Single (0) — no extra filter; kiosk handles both directions.

        var match = await qb
            .OrderByDescending(d => d.CompletedAt ?? d.StartedAt ?? d.UpdatedAt ?? DateTime.MinValue)
            .Select(d => new
            {
                d.TransactionId,
                d.StartQty,
                d.EndQty,
                d.NetQty,
                d.CompletedAt,
                d.StartedAt,
                d.UpdatedAt,
                d.ItemId,
                d.AccountId,
            })
            .FirstOrDefaultAsync(ct);

        if (match is null) return NoContent();

        // Commodity = item description on the load. Falls back to the
        // lot's item description for direct loads where the ITD itself
        // doesn't carry an ItemId.
        string commodity = "";
        if (match.ItemId.HasValue)
        {
            commodity = await _ctx.Items.AsNoTracking()
                .Where(i => i.ItemId == match.ItemId)
                .Select(i => i.Description)
                .FirstOrDefaultAsync(ct) ?? "";
        }
        if (string.IsNullOrEmpty(commodity) && ws.LotId.HasValue)
        {
            commodity = await _ctx.Lots.AsNoTracking()
                .Where(l => l.LotId == ws.LotId)
                .Join(_ctx.Items, l => l.ItemId, i => i.ItemId, (l, i) => i.Description)
                .FirstOrDefaultAsync(ct) ?? "";
        }

        // Account name — prefer the ITD's bound account, fall back to
        // the lot's primary split account so newly-saved inbound loads
        // (which often inherit the lot's account) still show one.
        string account = "";
        if (match.AccountId.HasValue)
        {
            account = await _ctx.Accounts.AsNoTracking()
                .Where(a => a.AccountId == match.AccountId)
                .Select(a => !string.IsNullOrEmpty(a.EntityName) ? a.EntityName : a.LookupName)
                .FirstOrDefaultAsync(ct) ?? "";
        }
        if (string.IsNullOrEmpty(account) && ws.LotId.HasValue)
        {
            account = await _ctx.LotSplitGroups.AsNoTracking()
                .Where(lsg => lsg.LotId == ws.LotId && lsg.PrimaryAccount)
                .Join(_ctx.Accounts, lsg => lsg.AccountId, a => a.AccountId,
                      (lsg, a) => !string.IsNullOrEmpty(a.EntityName) ? a.EntityName : a.LookupName)
                .FirstOrDefaultAsync(ct) ?? "";
        }

        // TruckId comes from the TRUCK_ID transaction attribute, same
        // lookup the existing print pipeline uses.
        string truckId = "";
        try
        {
            truckId = await _ctx.TransactionAttributes.AsNoTracking()
                .Where(a => a.TransactionId == match.TransactionId
                            && a.AttributeType.Code == "TRUCK_ID")
                .Select(a => a.StringValue)
                .FirstOrDefaultAsync(ct) ?? "";
        }
        catch { /* table-shape variants — tolerate */ }

        return Ok(new PendingTicketResponse
        {
            TransactionId  = match.TransactionId,
            WeightSheetId  = ws.WeightSheetId,
            LocationId     = ws.LocationId,
            LocationName   = locationName,
            Commodity      = commodity,
            Account        = account,
            TruckId        = truckId,
            InboundLbs     = match.StartQty ?? 0m,
            OutboundLbs    = match.EndQty,
            NetLbs         = match.NetQty ?? 0m,
            IsOutbound     = match.EndQty != null,
            LastTouchedUtc = match.CompletedAt ?? match.StartedAt ?? match.UpdatedAt,
        });
    }

    private async Task SendPrintToKioskAsync(string serviceAndPrinter, long txnId, bool original, CancellationToken ct)
    {
        // serviceAndPrinter is "serviceId:printerId" — split it for the
        // PrintHub fanout that scopes to a single service group.
        string serviceId = "default", printerId = serviceAndPrinter;
        var sep = serviceAndPrinter.IndexOf(':');
        if (sep > 0)
        {
            serviceId = serviceAndPrinter[..sep];
            printerId = serviceAndPrinter[(sep + 1)..];
        }

        var payload = new
        {
            ServiceId = serviceId,
            PrinterId = printerId,
            Kind      = "OutboundTicket",
            TransactionId = txnId,
            // Many existing print agents can fetch the PDF themselves from
            // the standard endpoint when only the txn id is given. Pass
            // both for forward-compat.
            PdfUrl = $"/api/printjobs/outbound-ticket/{txnId}/pdf?original={(original ? "true" : "false")}",
        };

        try
        {
            await _printHub.Clients
                .Group($"Print_{serviceId}")
                .SendAsync("PrintTicket", payload, ct);
        }
        catch (Exception ex)
        {
            _log.LogWarning(ex, "Failed to push kiosk print for txn {Txn} to {Printer}", txnId, serviceAndPrinter);
        }
    }
}
