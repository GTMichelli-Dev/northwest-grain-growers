using GrainManagement.Dtos.Warehouse;
using GrainManagement.Models;
using GrainManagement.Reporting;
using GrainManagement.Services;
using GrainManagement.Services.Email;
using GrainManagement.Services.Warehouse;
using DevExpress.XtraReports.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace GrainManagement.Api
{
    [ApiController]
    [Route("api/printjobs")]
    public class PrintJobsController : ControllerBase
    {
        private readonly dbContext _db;
        private readonly ILogger<PrintJobsController> _log;
        private readonly TimeZoneInfo _serverTz;
        private readonly IEndOfDayReportService _eodService;
        private readonly IEmailService _email;
        private readonly IWeightSheetNotifier _notifier;
        private readonly EmailOptions _emailOpts;

        public PrintJobsController(
            dbContext db,
            ILogger<PrintJobsController> log,
            IConfiguration config,
            IEndOfDayReportService eodService,
            IEmailService email,
            IWeightSheetNotifier notifier,
            IOptions<EmailOptions> emailOpts)
        {
            _db = db;
            _log = log;
            _serverTz = ResolveServerTimeZone(config);
            _eodService = eodService;
            _email = email;
            _notifier = notifier;
            _emailOpts = emailOpts.Value;
        }

        // Resolves the configured "TimeZone" key (IANA tz id) to a TimeZoneInfo,
        // falling back to TimeZoneInfo.Local if unset or unrecognized. Used to
        // convert UTC datetimes to the operator-facing zone before binding to
        // reports — storage stays UTC.
        private static TimeZoneInfo ResolveServerTimeZone(IConfiguration config)
        {
            var id = config["TimeZone"];
            if (!string.IsNullOrWhiteSpace(id))
            {
                try { return TimeZoneInfo.FindSystemTimeZoneById(id); }
                catch { /* fall through */ }
            }
            return TimeZoneInfo.Local;
        }

        // The current calendar day in the configured server time zone.
        // Used by End-Of-Day so a 5pm-PST run (which is already "tomorrow"
        // in UTC) still lands on today's date for the operator.
        private DateTime ServerToday() =>
            TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _serverTz).Date;

        // Converts a UTC datetime to the configured server zone for display.
        // Returns null when the input is null. Treats already-Local kinds as UTC
        // (every server-side write uses DateTime.UtcNow; values from EF come back
        // as Unspecified — we coerce all of them to UTC before converting).
        private DateTime? ToServerTime(DateTime? utc)
        {
            if (!utc.HasValue) return null;
            var asUtc = utc.Value.Kind == DateTimeKind.Utc
                ? utc.Value
                : DateTime.SpecifyKind(utc.Value, DateTimeKind.Utc);
            return TimeZoneInfo.ConvertTimeFromUtc(asUtc, _serverTz);
        }

        // ── Primary endpoint — auto-selects report based on transaction quantities ──

        /// <summary>
        /// Generates a load ticket PDF. Automatically selects the correct report:
        ///   - Inbound:  StartQty is set, EndQty is null, DirectQty is null
        ///   - Outbound: StartQty is set, EndQty is set, DirectQty is null
        ///   - Direct:   DirectQty is set
        /// </summary>
        [HttpGet("load-ticket/{ticket}/pdf")]
        public IActionResult GetLoadTicketPdf([FromRoute] string ticket)
        {
            // Resolve the detail row first so we can branch on TxnType. Transfer
            // rows (TRANSFER_IN/TRANSFER_OUT) bind to TransferLoadTicketDataModel
            // and one of the Transfer*LoadTicketReport variants; everything else
            // continues to use the Recieved* reports + LoadTicketDataModel.
            var detail = ResolveLoadDetail(ticket);
            if (detail == null)
                return NotFound(new { error = $"Transaction {ticket} not found." });

            XtraReport report;
            object dataRow;
            string reportName;

            if (IsTransferTxn(detail))
            {
                var transferModel = BuildTransferLoadTicketDataModel(detail);
                report     = SelectTransferReport(detail);
                dataRow    = transferModel;
                reportName = report.GetType().Name;
            }
            else
            {
                var model = BuildLoadTicketDataModel(ticket, out var resolved);
                if (model == null || resolved == null)
                    return NotFound(new { error = $"Transaction {ticket} not found." });
                report     = SelectReport(resolved);
                dataRow    = model;
                reportName = report.GetType().Name;
            }

            _log.LogInformation(
                "Generating {ReportType} PDF for transaction {TransactionId}",
                reportName, ticket);

            report.DataSource = new[] { dataRow };
            report.CreateDocument();

            if (report.Pages.Count == 0)
                return Problem("Report rendered zero pages.");

            using var ms = new MemoryStream();
            report.ExportToPdf(ms);

            return File(ms.ToArray(), "application/pdf", $"LoadTicket-{ticket}.pdf");
        }

        private static bool IsTransferTxn(InventoryTransactionDetail detail)
            => detail.TxnType == "TRANSFER_IN" || detail.TxnType == "TRANSFER_OUT";

        // Cheaply resolves just the detail row (no joins) so GetLoadTicketPdf
        // can decide which build path to take before paying for the heavier
        // join queries inside the build helpers.
        private InventoryTransactionDetail? ResolveLoadDetail(string ticket)
        {
            if (!long.TryParse(ticket, out var id)) return null;
            return _db.InventoryTransactionDetails
                .AsNoTracking()
                .FirstOrDefault(d => d.TransactionId == id);
        }

        // ── Explicit endpoints (for when the caller knows which report they want) ──

        [HttpGet("inbound-ticket/{ticket}/pdf")]
        public IActionResult GetInboundTicketPdf([FromRoute] string ticket)
            => RenderReport<RecievedInboundLoadTicketReport>(ticket, "InboundTicket");

        [HttpGet("outbound-ticket/{ticket}/pdf")]
        public IActionResult GetOutboundTicketPdf([FromRoute] string ticket)
            => RenderReport<RecievedOutboundLoadTicketReport>(ticket, "OutboundTicket");

        [HttpGet("direct-ticket/{ticket}/pdf")]
        public IActionResult GetDirectTicketPdf([FromRoute] string ticket)
            => RenderReport<DirectQuantityLoadTicketReport>(ticket, "DirectTicket");

        // ── Test page ───────────────────────────────────────────────────────────

        [HttpGet("test-page/pdf")]
        public IActionResult GetTestPagePdf()
        {
            var report = new TestTicketReport();
            report.CreateDocument();

            if (report.Pages.Count == 0)
                return Problem("Report rendered zero pages.");

            using var ms = new MemoryStream();
            report.ExportToPdf(ms);
            return File(ms.ToArray(), "application/pdf", "TestPage.pdf");
        }

        // ── Intake Weight Sheet ─────────────────────────────────────────────────

        [HttpGet("intake-weight-sheet/{wsId:long}/pdf")]
        public IActionResult GetIntakeWeightSheetPdf([FromRoute] long wsId, [FromQuery] bool original = true)
        {
            var model = BuildIntakeWeightSheet(wsId, original);
            if (model == null)
                return NotFound(new { error = $"Weight sheet {wsId} not found." });

            var report = new IntakeWeightSheetReport();
            report.DataSource = new[] { model };
            report.CreateDocument();

            if (report.Pages.Count == 0)
                return Problem("Report rendered zero pages.");

            using var ms = new MemoryStream();
            report.ExportToPdf(ms);
            // Inline so the iframe in the warehouse-dashboard preview modal
            // (and a window.open in another tab) renders the PDF in the
            // browser viewer instead of forcing a download. The 3-arg
            // File(bytes, contentType, fileDownloadName) overload sets
            // Content-Disposition: attachment under the hood, which is
            // exactly what we don't want here.
            Response.Headers["Content-Disposition"] = $"inline; filename=\"WeightSheet-{wsId}.pdf\"";
            return File(ms.ToArray(), "application/pdf");
        }

        // ── Combined Weight Sheets (multi-WS PDF) ───────────────────────────────
        //
        // POST /api/printjobs/weight-sheets/combined-pdf
        //   { "WeightSheetIds": [123, 456, 789] }
        //
        // Renders an Intake or Transfer report per WS (based on each row's
        // WeightSheetType) and merges the page collections into one PDF —
        // same approach the EOD endpoint uses. Used by the warehouse
        // dashboard's toolbar Print button to print every WS currently
        // visible in the grid (after filters / search) as a single document.
        public sealed class CombinedWeightSheetsRequest
        {
            public List<long> WeightSheetIds { get; set; } = new();
        }

        [HttpPost("weight-sheets/combined-pdf")]
        public async Task<IActionResult> GetCombinedWeightSheetsPdf(
            [FromBody] CombinedWeightSheetsRequest req,
            CancellationToken ct)
        {
            if (req == null || req.WeightSheetIds == null || req.WeightSheetIds.Count == 0)
                return BadRequest(new { message = "At least one weight sheet id is required." });

            // Cap to a sane upper bound so a runaway "All bucket + Print"
            // can't pin the server building a 10k-WS PDF. The dashboard's
            // typical filtered view is well under this.
            const int MaxBatch = 500;
            if (req.WeightSheetIds.Count > MaxBatch)
                return BadRequest(new { message = $"Too many weight sheets ({req.WeightSheetIds.Count}); maximum is {MaxBatch} per print." });

            // Pull the type per id in one round-trip so we can dispatch to
            // the right report builder. Preserves the caller's order so
            // the printed batch matches the on-screen sort.
            var typeMap = await _db.WeightSheets.AsNoTracking()
                .Where(w => req.WeightSheetIds.Contains(w.WeightSheetId))
                .Select(w => new { w.WeightSheetId, w.WeightSheetType })
                .ToDictionaryAsync(x => x.WeightSheetId, x => x.WeightSheetType, ct);

            var reports = new List<XtraReport>();
            foreach (var wsId in req.WeightSheetIds)
            {
                if (!typeMap.TryGetValue(wsId, out var wsType) || string.IsNullOrEmpty(wsType))
                    continue;

                if (string.Equals(wsType, "Transfer", StringComparison.OrdinalIgnoreCase))
                {
                    var dto = BuildTransferWeightSheet(wsId, original: true);
                    if (dto == null) continue;
                    var rpt = new TransferWeightSheetReport { DataSource = new[] { dto } };
                    rpt.CreateDocument();
                    if (rpt.Pages.Count > 0) reports.Add(rpt);
                }
                else
                {
                    var dto = BuildIntakeWeightSheet(wsId, original: true);
                    if (dto == null) continue;
                    var rpt = new IntakeWeightSheetReport { DataSource = new[] { dto } };
                    rpt.CreateDocument();
                    if (rpt.Pages.Count > 0) reports.Add(rpt);
                }
            }

            if (reports.Count == 0)
                return Problem("None of the requested weight sheets rendered any pages.");

            // Same merge pattern as GetEndOfDayPdf: keep the first report
            // as the master, append each subsequent report's Pages, then
            // export once.
            var master = reports[0];
            for (int i = 1; i < reports.Count; i++)
                master.Pages.AddRange(reports[i].Pages);
            master.PrintingSystem.ContinuousPageNumbering = true;

            using var ms = new MemoryStream();
            master.ExportToPdf(ms);
            var stamp = DateTime.Now.ToString("yyyy-MM-dd");
            Response.Headers["Content-Disposition"] = $"inline; filename=\"WeightSheets {stamp}.pdf\"";
            return File(ms.ToArray(), "application/pdf");
        }

        // ── Lot Label ───────────────────────────────────────────────────────────

        [HttpGet("lot-label/{lotId:long}/pdf")]
        public IActionResult GetLotLabelPdf([FromRoute] long lotId)
        {
            var model = BuildLotLabel(lotId);
            if (model == null)
                return NotFound(new { error = $"Lot {lotId} not found." });

            var report = new LotLabelReport();
            report.DataSource = new[] { model };
            report.CreateDocument();

            if (report.Pages.Count == 0)
                return Problem("Report rendered zero pages.");

            using var ms = new MemoryStream();
            report.ExportToPdf(ms);
            return File(ms.ToArray(), "application/pdf", $"LotLabel-{lotId}.pdf");
        }

        private LotLabelDto? BuildLotLabel(long lotId)
        {
            var lot = _db.Lots
                .Include(l => l.SplitGroup)
                .Include(l => l.Product)
                .AsNoTracking()
                .FirstOrDefault(l => l.LotId == lotId);

            if (lot == null) return null;

            // Primary account is read from the lot's own LotSplitGroups row flagged
            // PrimaryAccount = 1 — override-mode split groups have a null
            // SplitGroup.PrimaryAccountId and would otherwise print blank.
            string accountName = "";
            string accountId = "";
            var primaryAccountId = _db.LotSplitGroups
                .AsNoTracking()
                .Where(lsg => lsg.LotId == lot.LotId && lsg.PrimaryAccount)
                .Select(lsg => (long?)lsg.AccountId)
                .FirstOrDefault();
            if (primaryAccountId.HasValue)
            {
                var acct = _db.Accounts
                    .AsNoTracking()
                    .FirstOrDefault(a => a.AccountId == primaryAccountId.Value);
                if (acct != null)
                {
                    accountName = !string.IsNullOrWhiteSpace(acct.EntityName) ? acct.EntityName : acct.LookupName ?? "";
                    accountId = acct.AccountId.ToString();
                }
            }

            // Explicit Location lookup — don't rely on nav property in case FK is misconfigured
            string locationName = _db.Locations
                .AsNoTracking()
                .Where(l => l.LocationId == lot.LocationId)
                .Select(l => l.Name)
                .FirstOrDefault() ?? "";

            _log.LogInformation(
                "BuildLotLabel LotId={LotId} LocationId={LocationId} LocationName={LocationName}",
                lotId, lot.LocationId, locationName);

            return new LotLabelDto
            {
                LotId = lot.LotId.ToString(),
                As400Id = lot.As400Id > 0 ? FormatId(lot.As400Id) : FormatId(lot.LotId),
                SplitGroupId = lot.SplitGroupId?.ToString() ?? "",
                SplitGroupDescription = lot.SplitGroup?.SplitGroupDescription ?? "",
                CropName = lot.Product?.Description ?? "",
                CreatedByUserName = lot.CreatedByUserName ?? "",
                CreatedDate = lot.CreatedAt.ToString("M/d/yyyy"),
                PrimaryAccountName = accountName,
                PrimaryAccountId = accountId,
                LocationId = lot.LocationId.ToString(),
                LocationDescription = locationName
            };
        }

        // ══════════════════════════════════════════════════════════════════════════
        // Private helpers
        // ══════════════════════════════════════════════════════════════════════════

        /// <summary>
        /// Formats a numeric ID with hyphens: 604041000013 → 604-041-000013
        /// </summary>
        private static string FormatId(long id)
        {
            var s = id.ToString();
            if (s.Length < 7) return s;
            return s[..3] + "-" + s[3..6] + "-" + s[6..];
        }

        private static string FormatId(string id) =>
            long.TryParse(id, out var v) ? FormatId(v) : id;

        /// <summary>
        /// Renders a specific report type for the given transaction.
        /// Used by the explicit endpoints.
        /// </summary>
        private IActionResult RenderReport<TReport>(string ticket, string filePrefix)
            where TReport : XtraReport, new()
        {
            var model = BuildLoadTicketDataModel(ticket, out _);
            if (model == null)
                return NotFound(new { error = $"Transaction {ticket} not found." });

            var report = new TReport();
            report.DataSource = new[] { model };
            report.CreateDocument();

            if (report.Pages.Count == 0)
                return Problem("Report rendered zero pages.");

            using var ms = new MemoryStream();
            report.ExportToPdf(ms);
            return File(ms.ToArray(), "application/pdf", $"{filePrefix}-{ticket}.pdf");
        }

        /// <summary>
        /// Selects the correct DevExpress report based on the transaction's quantity fields.
        ///
        /// Rules:
        ///   1. DirectQty is set              → DirectQuantityLoadTicketReport
        ///   2. StartQty set + EndQty set      → RecievedOutboundLoadTicketReport
        ///   3. StartQty set + EndQty null      → RecievedInboundLoadTicketReport
        /// </summary>
        private static XtraReport SelectReport(InventoryTransactionDetail detail)
        {
            // Direct quantity loads (non-truck: manual, rail, bulk loader, etc.)
            if (detail.DirectQty.HasValue)
                return new DirectQuantityLoadTicketReport();

            // Truck loads with both inbound and outbound weights
            if (detail.StartQty.HasValue && detail.EndQty.HasValue)
                return new RecievedOutboundLoadTicketReport();

            // Truck loads with only inbound weight (not yet scaled out)
            return new RecievedInboundLoadTicketReport();
        }

        /// <summary>
        /// Selects the correct transfer-flavored report based on the transaction's
        /// quantity fields. Mirrors SelectReport but for TRANSFER_IN / TRANSFER_OUT.
        ///
        /// Rules:
        ///   1. DirectQty is set              → DirectQuantityLoadTicketReport
        ///      (transfers don't have a transfer-specific direct report yet — fall back
        ///       to the shared direct report which uses LoadTicketDataModel; non-transfer
        ///       direct loads dominate in practice)
        ///   2. StartQty set + EndQty set      → TransferOutboundLoadTicketReport
        ///   3. StartQty set + EndQty null      → TransferInboundLoadTicketReport
        /// </summary>
        private static XtraReport SelectTransferReport(InventoryTransactionDetail detail)
        {
            if (detail.DirectQty.HasValue)
                return new DirectQuantityLoadTicketReport();

            if (detail.StartQty.HasValue && detail.EndQty.HasValue)
                return new TransferOutboundLoadTicketReport();

            return new TransferInboundLoadTicketReport();
        }

        /// <summary>
        /// Resolves item-level fields used by the load-ticket and weight-sheet
        /// reports: item id/description, crop id/description, and the seed
        /// flag. IsSeed mirrors /api/Lookups/SeedItems — TraitId=31 AND the
        /// product's Category is not one of the non-seed codes (CHEM/FERT/
        /// PACK/SERVICE). Returns empty/false when itemId is null or unknown.
        /// </summary>
        private (string ItemIdStr, string ItemDescription, string CropIdStr, string Crop, bool IsSeed) ResolveItemSeedInfo(long? itemId)
        {
            string itemIdStr = "";
            string description = "";
            string cropIdStr = "";
            string cropDescription = "";
            bool isSeedItem = false;

            if (!itemId.HasValue) return (itemIdStr, description, cropIdStr, cropDescription, isSeedItem);

            itemIdStr = itemId.Value.ToString();
            var nonSeedCategoryCodes = new[] { "CHEM", "FERT", "PACK", "SERVICE" };
            var info = _db.Items.AsNoTracking()
                .Where(i => i.ItemId == itemId.Value)
                .Select(i => new
                {
                    i.Description,
                    CropId = (long?)(i.Product != null ? i.Product.CropId : null),
                    HasSeedTrait = i.ItemTraits.Any(x => x.TraitId == 31),
                    CategoryCode = i.Product != null && i.Product.Category != null
                        ? i.Product.Category.CategoryCode
                        : null,
                })
                .FirstOrDefault();
            if (info != null)
            {
                description = info.Description ?? "";
                isSeedItem = info.HasSeedTrait
                    && (info.CategoryCode == null
                        || !nonSeedCategoryCodes.Contains(info.CategoryCode));
                if (info.CropId.HasValue && info.CropId.Value > 0)
                {
                    cropIdStr = info.CropId.Value.ToString();
                    cropDescription = _db.Items.AsNoTracking()
                        .Where(i => i.ItemId == info.CropId.Value)
                        .Select(i => i.Description)
                        .FirstOrDefault() ?? "";
                }
            }
            return (itemIdStr, description, cropIdStr, cropDescription, isSeedItem);
        }

        /// <summary>
        /// Builds the TransferLoadTicketDataModel for a transfer transaction.
        /// Joins to the WS for ItemId / Source / Destination, the Items table for
        /// Variety, and Locations for source/destination names. No lot / account /
        /// split fields — transfer loads aren't tied to a producer lot.
        /// </summary>
        private TransferLoadTicketDataModel BuildTransferLoadTicketDataModel(InventoryTransactionDetail detail)
        {
            var transactionId = detail.TransactionId;

            // Re-fetch with the joins we need for the report (ResolveLoadDetail
            // does a cheap, no-include lookup so the branch in GetLoadTicketPdf
            // is fast).
            var full = _db.InventoryTransactionDetails
                .Include(d => d.Transaction).ThenInclude(t => t.Location)
                .Include(d => d.Product)
                .AsNoTracking()
                .FirstOrDefault(d => d.TransactionId == transactionId)
                ?? detail;

            // ── Resolve the WS for hauler / BOL / source / destination ──────
            string haulerName = "";
            string wsDisplay = "";
            string bolType = "";
            long? sourceLocationId = null;
            long? destinationLocationId = null;
            long? wsItemId = null;
            var ws = _db.WeightSheets
                .Include(w => w.Hauler)
                .AsNoTracking()
                .FirstOrDefault(w => w.RowUid == full.RefId && full.RefType == "WeightSheet");
            if (ws != null)
            {
                haulerName = ws.Hauler?.Description ?? "";
                wsDisplay = ws.As400Id > 0
                    ? ws.As400Id.ToString()
                    : FormatId(ws.WeightSheetId);
                bolType = ws.RateType switch
                {
                    "U" => "Universal",
                    "A" => "Along Side Field",
                    "F" => "Farm Storage",
                    "C" => "Custom",
                    _   => ws.RateType ?? ""
                };
                sourceLocationId = ws.SourceLocationId;
                destinationLocationId = ws.DestinationLocationId;
                wsItemId = ws.ItemId;
            }

            // ── Resolve source / destination location names ─────────────────
            string sourceName = "";
            if (sourceLocationId.HasValue)
            {
                sourceName = _db.Locations.AsNoTracking()
                    .Where(l => l.LocationId == sourceLocationId.Value)
                    .Select(l => l.Name)
                    .FirstOrDefault() ?? "";
            }
            string destName = "";
            if (destinationLocationId.HasValue)
            {
                destName = _db.Locations.AsNoTracking()
                    .Where(l => l.LocationId == destinationLocationId.Value)
                    .Select(l => l.Name)
                    .FirstOrDefault() ?? "";
            }

            // ── Variety / item / crop / seed-flag ───────────────────────────
            var seedInfo = ResolveItemSeedInfo(wsItemId);
            string variety = !string.IsNullOrEmpty(seedInfo.ItemDescription)
                ? seedInfo.ItemDescription
                : (full.Product?.Description ?? "");

            // ── Bin (container) ─────────────────────────────────────────────
            var binName = _db.InventoryTransactionDetailToContainers
                .Include(c => c.Container)
                .AsNoTracking()
                .Where(c => c.TransactionId == transactionId)
                .Select(c => c.Container.Description)
                .FirstOrDefault() ?? "";

            // ── Net weight ──────────────────────────────────────────────────
            int netWeight = full.DirectQty.HasValue
                ? (int)full.DirectQty.Value
                : (int)(full.NetQty ?? 0);

            // ── Manual-entry legal flags ────────────────────────────────────
            var manualSources = (from qs in _db.TransactionQuantitySources.AsNoTracking()
                                 join st in _db.QuantitySourceTypes.AsNoTracking()
                                    on qs.SourceTypeId equals st.QuantitySourceTypeId
                                 where qs.TransactionId == transactionId
                                 select new { qs.QuantityField, st.Code })
                                .ToList();
            string startManualFlag  = manualSources.Any(s => s.QuantityField == "START"  && s.Code == "MANUAL") ? "M" : " ";
            string endManualFlag    = manualSources.Any(s => s.QuantityField == "END"    && s.Code == "MANUAL") ? "M" : " ";
            string directManualFlag = manualSources.Any(s => s.QuantityField == "DIRECT" && s.Code == "MANUAL") ? "M" : " ";

            // ── Truck Id (TRUCK_ID transaction attribute) ───────────────────
            string truckIdValue = _db.TransactionAttributes.AsNoTracking()
                .Where(a => a.TransactionId == transactionId
                            && a.AttributeType.Code == "TRUCK_ID")
                .Select(a => a.StringValue)
                .FirstOrDefault() ?? "";

            // Direction is derived from where this row sits relative to the WS
            // header: TRANSFER_IN at the destination = Received, TRANSFER_OUT at
            // the source = Shipped.
            string direction = full.TxnType == "TRANSFER_IN" ? "Received"
                              : full.TxnType == "TRANSFER_OUT" ? "Shipped"
                              : "";

            return new TransferLoadTicketDataModel
            {
                LoadId              = FormatId(transactionId),
                WeightSheetId       = wsDisplay,
                Direction           = direction,
                Variety             = variety,
                ItemId              = seedInfo.ItemIdStr,
                ItemDescription     = variety,
                CropId              = seedInfo.CropIdStr,
                Crop                = seedInfo.Crop,
                IsSeed              = seedInfo.IsSeed,
                SourceLocation      = sourceName,
                SourceLocationId    = sourceLocationId?.ToString() ?? "",
                DestinationLocation = destName,
                DestinationLocationId = destinationLocationId?.ToString() ?? "",
                BolType             = bolType,
                Hauler              = haulerName,
                TruckId             = truckIdValue,
                InboundTime         = ToServerTime(full.StartedAt ?? full.TxnAt),
                OutboundTime        = ToServerTime(full.CompletedAt),
                InboundWeight       = (int)(full.StartQty ?? 0),
                OutboundWeight      = (int)(full.EndQty ?? 0),
                NetWeight           = netWeight,
                DirectQty           = (int)(full.DirectQty ?? 0),
                Location            = full.Transaction?.Location?.Name ?? "",
                LocationId          = full.Transaction?.LocationId.ToString() ?? "",
                Commodity           = full.Product?.Description ?? "",
                Bin                 = binName,
                StartManualFlag     = startManualFlag,
                EndManualFlag       = endManualFlag,
                DirectManualFlag    = directManualFlag,
            };
        }

        /// <summary>
        /// Builds the shared LoadTicketDataModel from a transaction ID.
        /// Also outputs the raw detail entity for report selection logic.
        /// Returns null if the transaction is not found.
        /// </summary>
        private LoadTicketDataModel? BuildLoadTicketDataModel(
            string ticket,
            out InventoryTransactionDetail? detailOut)
        {
            detailOut = null;

            if (!long.TryParse(ticket, out var transactionId))
                return null;

            var detail = _db.InventoryTransactionDetails
                .Include(d => d.Transaction).ThenInclude(t => t.Location)
                .Include(d => d.Product)
                .Include(d => d.Account)
                .Include(d => d.Lot).ThenInclude(l => l.SplitGroup)
                .AsNoTracking()
                .FirstOrDefault(d => d.TransactionId == transactionId);

            if (detail == null)
                return null;

            detailOut = detail;

            // ── Resolve split group info ─────────────────────────────────────
            var splitGroup = detail.Lot?.SplitGroup;
            string splitNumber = splitGroup?.SplitGroupId.ToString() ?? "";
            string splitDesc = splitGroup?.SplitGroupDescription ?? "";

            // ── Resolve primary account name from split group ────────────────
            string accountName = "";
            if (splitGroup != null)
            {
                var acct = _db.Accounts
                    .AsNoTracking()
                    .FirstOrDefault(a => a.AccountId == splitGroup.PrimaryAccountId);
                if (acct != null)
                    accountName = !string.IsNullOrWhiteSpace(acct.EntityName) ? acct.EntityName : acct.LookupName ?? "";
            }
            // Fall back to the detail's own account
            if (string.IsNullOrEmpty(accountName) && detail.Account != null)
            {
                accountName = !string.IsNullOrWhiteSpace(detail.Account.EntityName)
                    ? detail.Account.EntityName
                    : detail.Account.LookupName ?? "";
            }

            // ── Resolve hauler + weight sheet + BOL type ─────────────────────
            string haulerName = "";
            string wsId = "";
            string bolType = "";
            var ws = _db.WeightSheets
                .Include(w => w.Hauler)
                .AsNoTracking()
                .FirstOrDefault(w => w.LotId == detail.LotId
                                  && w.LocationId == detail.Transaction.LocationId);
            if (ws != null)
            {
                haulerName = ws.Hauler?.Description ?? "";
                wsId = ws.WeightSheetId.ToString();
                bolType = ws.RateType switch
                {
                    "U" => "Universal",
                    "A" => "Along Side Field",
                    "F" => "Farm Storage",
                    "C" => "Custom",
                    _   => ws.RateType ?? ""
                };
            }

            // ── Net weight calculation ───────────────────────────────────────
            // Outbound: use stored NetQty (database-computed)
            // Direct:   use DirectQty
            // Inbound:  no net — just inbound weight
            int netWeight;
            if (detail.DirectQty.HasValue)
                netWeight = (int)detail.DirectQty.Value;
            else
                netWeight = (int)(detail.NetQty ?? 0);

            // ── Resolve Bin (container) ──────────────────────────────────────
            var binName = _db.InventoryTransactionDetailToContainers
                .Include(c => c.Container)
                .AsNoTracking()
                .Where(c => c.TransactionId == transactionId)
                .Select(c => c.Container.Description)
                .FirstOrDefault() ?? "";

            // ── Use As400 IDs when available ────────────────────────────────
            string lotDisplay = detail.Lot?.As400Id > 0
                ? detail.Lot.As400Id.ToString()
                : (detail.LotId.HasValue ? FormatId(detail.LotId.Value) : "");
            string wsDisplay = ws?.As400Id > 0
                ? ws.As400Id.ToString()
                : FormatId(wsId);

            // ── Manual-entry legal flags (START / END / DIRECT) ─────────────
            var manualSources = (from qs in _db.TransactionQuantitySources.AsNoTracking()
                                 join st in _db.QuantitySourceTypes.AsNoTracking()
                                    on qs.SourceTypeId equals st.QuantitySourceTypeId
                                 where qs.TransactionId == transactionId
                                 select new { qs.QuantityField, st.Code })
                                .ToList();
            string startManualFlag  = manualSources.Any(s => s.QuantityField == "START"  && s.Code == "MANUAL") ? "M" : " ";
            string endManualFlag    = manualSources.Any(s => s.QuantityField == "END"    && s.Code == "MANUAL") ? "M" : " ";
            string directManualFlag = manualSources.Any(s => s.QuantityField == "DIRECT" && s.Code == "MANUAL") ? "M" : " ";

            // ── Truck Id (TRUCK_ID transaction attribute) ───────────────────
            string truckIdValue = _db.TransactionAttributes.AsNoTracking()
                .Where(a => a.TransactionId == transactionId
                            && a.AttributeType.Code == "TRUCK_ID")
                .Select(a => a.StringValue)
                .FirstOrDefault() ?? "";

            // ── Item / crop / seed-flag ─────────────────────────────────────
            var seedInfo = ResolveItemSeedInfo(detail.ItemId);
            string commodity = !string.IsNullOrEmpty(seedInfo.ItemDescription)
                ? seedInfo.ItemDescription
                : (detail.Product?.Description ?? "");

            return new LoadTicketDataModel
            {
                LoadId           = FormatId(ticket),
                WeightSheetId    = wsDisplay,
                LotNumber        = lotDisplay,
                CropAccount      = accountName,
                SplitNumber      = splitNumber,
                SplitDescription = splitDesc,
                BolType          = bolType,
                Hauler           = haulerName,
                TruckId          = truckIdValue,
                InboundTime      = ToServerTime(detail.StartedAt ?? detail.TxnAt),
                OutboundTime     = ToServerTime(detail.CompletedAt),
                InboundWeight    = (int)(detail.StartQty ?? 0),
                OutboundWeight   = (int)(detail.EndQty ?? 0),
                NetWeight        = netWeight,
                DirectQty        = (int)(detail.DirectQty ?? 0),
                Location         = detail.Transaction?.Location?.Name ?? "",
                LocationId       = detail.Transaction?.LocationId.ToString() ?? "",
                Commodity        = commodity,
                ItemDescription  = commodity,
                ItemId           = seedInfo.ItemIdStr,
                CropId           = seedInfo.CropIdStr,
                Crop             = seedInfo.Crop,
                IsSeed           = seedInfo.IsSeed,
                Bin              = binName,
                StartManualFlag  = startManualFlag,
                EndManualFlag    = endManualFlag,
                DirectManualFlag = directManualFlag,
            };
        }

        /// <summary>
        /// Builds the IntakeWeightSheetDto by loading the WS header + all delivery loads.
        /// Uses the same SQL query as GetWeightSheetDeliveryLoads for consistency.
        /// </summary>
        private IntakeWeightSheetDto? BuildIntakeWeightSheet(long wsId, bool original = true)
        {
            // Load WS header
            var ws = _db.WeightSheets
                .Include(w => w.Hauler)
                .AsNoTracking()
                .FirstOrDefault(w => w.WeightSheetId == wsId);

            if (ws == null) return null;

            // Resolve lot info
            string lotId = ws.LotId?.ToString() ?? "";
            long lotAs400Id = 0;
            string itemId = "";
            string cropName = "";
            string accountName = "";
            string accountId = "";
            string splitId = "";
            string splitName = "";
            string locationName = "";
            bool isLicensed = true;
            bool isFinalWeightSheet = false;
            string lotLandlordName = "";
            string lotFarmNumber = "";
            string lotState = "";
            string lotCounty = "";
            string lotNotes = "";

            if (ws.LotId.HasValue)
            {
                var lotInfo = _db.Lots
                    .Include(l => l.SplitGroup)
                    .Include(l => l.Item)
                    .AsNoTracking()
                    .FirstOrDefault(l => l.LotId == ws.LotId);

                if (lotInfo != null)
                {
                    lotAs400Id = lotInfo.As400Id;
                    itemId = lotInfo.ItemId?.ToString() ?? "";
                    cropName = lotInfo.Item?.Description ?? "";
                    lotNotes = lotInfo.Notes ?? "";
                    splitId = lotInfo.SplitGroupId?.ToString() ?? "";
                    splitName = lotInfo.SplitGroup?.SplitGroupDescription ?? "";

                    // Primary account is read from the lot's own LotSplitGroups row
                    // flagged PrimaryAccount = 1 — override-mode split groups have a
                    // null SplitGroup.PrimaryAccountId and would otherwise print blank.
                    var primaryAcctId = _db.LotSplitGroups
                        .AsNoTracking()
                        .Where(lsg => lsg.LotId == lotInfo.LotId && lsg.PrimaryAccount)
                        .Select(lsg => (long?)lsg.AccountId)
                        .FirstOrDefault();
                    if (primaryAcctId.HasValue)
                    {
                        var acct = _db.Accounts
                            .AsNoTracking()
                            .FirstOrDefault(a => a.AccountId == primaryAcctId.Value);
                        if (acct != null)
                        {
                            accountName = !string.IsNullOrWhiteSpace(acct.EntityName) ? acct.EntityName : acct.LookupName ?? "";
                            accountId = acct.AccountId.ToString();
                        }
                    }
                }

                var locInfo = _db.Locations
                    .AsNoTracking()
                    .Where(l => l.LocationId == ws.LocationId)
                    .Select(l => new { l.Name, l.Licensed })
                    .FirstOrDefault();
                locationName = locInfo?.Name ?? "";
                isLicensed = locInfo?.Licensed ?? true;

                // Read lot-level traits (Landlord Name + Farm Number)
                if (ws.LotId.HasValue)
                {
                    lotLandlordName = _db.LotTraits.AsNoTracking()
                        .Where(t => t.LotId == ws.LotId && t.TraitTypeId == 18)
                        .Select(t => t.Trait.TraitCode).FirstOrDefault() ?? "";
                    lotFarmNumber = _db.LotTraits.AsNoTracking()
                        .Where(t => t.LotId == ws.LotId && t.TraitTypeId == 19)
                        .Select(t => t.Trait.TraitCode).FirstOrDefault() ?? "";
                    lotState = _db.LotTraits.AsNoTracking()
                        .Where(t => t.LotId == ws.LotId && t.TraitTypeId == 15)
                        .Select(t => t.Trait.TraitCode).FirstOrDefault() ?? "";
                    lotCounty = _db.LotTraits.AsNoTracking()
                        .Where(t => t.LotId == ws.LotId && t.TraitTypeId == 16)
                        .Select(t => t.Trait.TraitCode).FirstOrDefault() ?? "";
                }

                // Check if the lot is closed and this is the last weight sheet for it
                if (lotInfo != null && !lotInfo.IsOpen)
                {
                    var maxWsId = _db.WeightSheets
                        .AsNoTracking()
                        .Where(w => w.LotId == ws.LotId)
                        .Max(w => (long?)w.WeightSheetId) ?? 0;
                    isFinalWeightSheet = ws.WeightSheetId == maxWsId;
                }
            }

            string bolType = ws.RateType switch
            {
                "U" => "Universal",
                "A" => "Along Side Field",
                "F" => "Farm Storage",
                "C" => "Custom",
                _ => ws.RateType ?? ""
            };

            // Load delivery loads via raw SQL (same query as the API endpoint)
            var conn = (Microsoft.Data.SqlClient.SqlConnection)_db.Database.GetDbConnection();
            if (conn.State != System.Data.ConnectionState.Open)
                conn.Open();

            var sql = @"
                SELECT
                    itd.TransactionId,
                    itd.StartedAt,
                    itd.CompletedAt,
                    itd.StartQty   AS InWeight,
                    itd.EndQty     AS OutWeight,
                    itd.NetQty     AS Net,
                    itd.Notes,
                    c.Description  AS ContainerDescription,
                    attr1.DecimalValue AS Protein,
                    attr2.DecimalValue AS Moisture,
                    truckAttr.StringValue AS TruckId,
                    bolAttr.StringValue AS BOL,
                    CASE WHEN startSrcType.Code = 'MANUAL' THEN 1 ELSE 0 END AS StartIsManual,
                    CASE WHEN endSrcType.Code = 'MANUAL' THEN 1 ELSE 0 END AS EndIsManual
                FROM [warehouse].[WeightSheets] ws
                INNER JOIN [Inventory].[InventoryTransactionDetails] itd ON itd.RefId = ws.RowUid AND itd.RefType = 'WeightSheet'
                LEFT JOIN [Inventory].[InventoryTransactionDetailToContainers] tc ON tc.TransactionId = itd.TransactionId
                LEFT JOIN [container].[Containers] c ON c.ContainerId = tc.ContainerId
                LEFT JOIN [Inventory].[TransactionAttributes] attr1
                    ON attr1.TransactionId = itd.TransactionId
                    AND attr1.AttributeTypeId = (SELECT TOP 1 Id FROM [Inventory].[TransactionAttributeTypes] WHERE Code = 'PROTEIN')
                LEFT JOIN [Inventory].[TransactionAttributes] attr2
                    ON attr2.TransactionId = itd.TransactionId
                    AND attr2.AttributeTypeId = (SELECT TOP 1 Id FROM [Inventory].[TransactionAttributeTypes] WHERE Code = 'MOISTURE')
                LEFT JOIN [Inventory].[TransactionAttributes] truckAttr
                    ON truckAttr.TransactionId = itd.TransactionId
                    AND truckAttr.AttributeTypeId = (SELECT TOP 1 Id FROM [Inventory].[TransactionAttributeTypes] WHERE Code = 'TRUCK_ID')
                LEFT JOIN [Inventory].[TransactionAttributes] bolAttr
                    ON bolAttr.TransactionId = itd.TransactionId
                    AND bolAttr.AttributeTypeId = (SELECT TOP 1 Id FROM [Inventory].[TransactionAttributeTypes] WHERE Code = 'BOL')
                LEFT JOIN [Inventory].[TransactionQuantitySources] startSrc
                    ON startSrc.TransactionId = itd.TransactionId AND startSrc.QuantityField = 'START'
                LEFT JOIN [Inventory].[TransactionQuantitySources] endSrc
                    ON endSrc.TransactionId = itd.TransactionId AND endSrc.QuantityField = 'END'
                LEFT JOIN [Inventory].[QuantitySourceTypes] startSrcType
                    ON startSrcType.QuantitySourceTypeId = startSrc.SourceTypeId
                LEFT JOIN [Inventory].[QuantitySourceTypes] endSrcType
                    ON endSrcType.QuantitySourceTypeId = endSrc.SourceTypeId
                WHERE ws.WeightSheetId = @wsId
                ORDER BY itd.TxnAt DESC";

            var loads = new List<IntakeWeightSheetLoadRow>();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@wsId", wsId);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var txnId = reader.GetInt64(reader.GetOrdinal("TransactionId"));
                    var inWt = reader.IsDBNull(reader.GetOrdinal("InWeight")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("InWeight"));
                    var outWt = reader.IsDBNull(reader.GetOrdinal("OutWeight")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("OutWeight"));
                    var net = reader.IsDBNull(reader.GetOrdinal("Net")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Net"));
                    var protein = reader.IsDBNull(reader.GetOrdinal("Protein")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Protein"));
                    var moisture = reader.IsDBNull(reader.GetOrdinal("Moisture")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Moisture"));
                    var container = reader.IsDBNull(reader.GetOrdinal("ContainerDescription")) ? "" : reader.GetString(reader.GetOrdinal("ContainerDescription"));
                    var startedAt = reader.IsDBNull(reader.GetOrdinal("StartedAt")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("StartedAt"));
                    var completedAt = reader.IsDBNull(reader.GetOrdinal("CompletedAt")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("CompletedAt"));
                    var notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? "" : reader.GetString(reader.GetOrdinal("Notes"));
                    var truckId = reader.IsDBNull(reader.GetOrdinal("TruckId")) ? "" : reader.GetString(reader.GetOrdinal("TruckId"));
                    var bol = reader.IsDBNull(reader.GetOrdinal("BOL")) ? "" : reader.GetString(reader.GetOrdinal("BOL"));
                    var startIsManual = reader.GetInt32(reader.GetOrdinal("StartIsManual")) == 1;
                    var endIsManual = reader.GetInt32(reader.GetOrdinal("EndIsManual")) == 1;

                    bool complete = outWt.HasValue && !string.IsNullOrEmpty(container) && protein.HasValue && protein > 0;
                    bool completeMissingProtein = outWt.HasValue && !string.IsNullOrEmpty(container) && !(protein.HasValue && protein > 0);

                    loads.Add(new IntakeWeightSheetLoadRow
                    {
                        WeightSheetId = wsId.ToString(),
                        As400Id = ws.As400Id > 0 ? ws.As400Id.ToString() : "",
                        LoadNumber = txnId.ToString(),
                        TruckId = truckId.Length > 4 ? truckId[..4] : truckId,
                        BOL = bol,
                        TimeIn = ToServerTime(startedAt),
                        TimeOut = ToServerTime(completedAt),
                        Bin = container,
                        InWeight = inWt,
                        OutWeight = outWt,
                        Net = net,
                        Protein = protein.HasValue && protein > 0 ? protein.Value.ToString("F2") : "",
                        Moisture = moisture.HasValue && moisture > 0 ? moisture.Value.ToString("F2") : "",
                        Notes = notes,
                        Status = complete ? "Complete" : (completeMissingProtein ? "Complete*" : "Incomplete"),
                        StartManualFlag = startIsManual ? "M" : " ",
                        EndManualFlag = endIsManual ? "M" : " ",
                    });
                }
            }

            // Sort ascending by TransactionId (lowest first) and assign row numbers
            loads = loads.OrderBy(l => l.LoadNumber).ToList();
            for (int i = 0; i < loads.Count; i++)
                loads[i].RowNumber = i + 1;

            int completedCount = loads.Count(l => l.Status.StartsWith("Complete"));
            decimal totalNet = loads.Where(l => l.Net.HasValue).Sum(l => l.Net.Value);

            // Parse As400Id into ServerId (first 3 digits) and WeightSheetNumber (last 6 digits)
            var as400Str = ws.As400Id.ToString();
            var serverId = as400Str.Length >= 3 ? as400Str[..3] : as400Str;
            var weightSheetNumber = as400Str.Length >= 6 ? as400Str[^6..] : as400Str;
            var serverName = "";
            if (int.TryParse(serverId, out var serverIdInt))
            {
                serverName = _db.Servers.AsNoTracking()
                    .Where(s => s.ServerId == serverIdInt)
                    .Select(s => s.FriendlyName)
                    .FirstOrDefault() ?? "";
            }

            return new IntakeWeightSheetDto
            {
                WeightSheetId = ws.As400Id > 0 ? ws.As400Id.ToString() : wsId.ToString(),
                As400Id = ws.As400Id > 0 ? ws.As400Id.ToString() : "",
                ServerId = serverId,
                ServerName = serverName,
                WeightSheetNumber = weightSheetNumber,
                LotId = lotAs400Id > 0 ? lotAs400Id.ToString() : lotId,
                ItemId = itemId,
                CropName = cropName,
                PrimaryAccountName = accountName,
                PrimaryAccountId = accountId,
                SplitGroupId = splitId,
                SplitName = splitName,
                RateType = bolType,
                HaulerName = ws.Hauler?.Description ?? "",
                IsFinalWeightSheet = isFinalWeightSheet,
                WeightmasterName = ws.WeightmasterName ?? "",
                CreationDate = ws.CreationDate.ToDateTime(TimeOnly.MinValue),
                Miles = ws.Miles,
                Rate = ws.Rate,
                LotNotes = lotNotes,
                WeightSheetNotes = ws.Notes ?? "",
                Location = locationName,
                LocationId = ws.LocationId.ToString(),
                CertificateTitle = isLicensed
                    ? "UNITED STATES WAREHOUSE ACT GRAIN WEIGHT CERTIFICATE"
                    : "GRAIN WEIGHT CERTIFICATE",
                LandlordName = lotLandlordName,
                FarmNumber = lotFarmNumber,
                State = lotState,
                County = lotCounty,
                PrintDate = ToServerTime(DateTime.UtcNow),
                CopyType = original ? "ORIGINAL" : "COPY",
                TotalLoads = loads.Count,
                CompletedLoads = completedCount,
                TotalNetWeight = ((int)totalNet).ToString("N0") + " lbs",
                Loads = loads,
            };
        }

        // ── Transfer Weight Sheet ───────────────────────────────────────────────

        [HttpGet("transfer-weight-sheet/{wsId:long}/pdf")]
        public IActionResult GetTransferWeightSheetPdf([FromRoute] long wsId, [FromQuery] bool original = true)
        {
            var model = BuildTransferWeightSheet(wsId, original);
            if (model == null)
                return NotFound(new { error = $"Transfer weight sheet {wsId} not found." });

            var report = new TransferWeightSheetReport();
            report.DataSource = new[] { model };
            report.CreateDocument();

            if (report.Pages.Count == 0)
                return Problem("Report rendered zero pages.");

            using var ms = new MemoryStream();
            report.ExportToPdf(ms);
            return File(ms.ToArray(), "application/pdf", $"TransferWeightSheet-{wsId}.pdf");
        }

        /// <summary>
        /// Builds the TransferWeightSheetDto by loading the transfer WS header +
        /// all transfer loads. Mirrors BuildIntakeWeightSheet without the lot
        /// joins and adds Source/Destination/Variety/Direction.
        /// </summary>
        private TransferWeightSheetDto? BuildTransferWeightSheet(long wsId, bool original = true)
        {
            var ws = _db.WeightSheets
                .Include(w => w.Hauler)
                .AsNoTracking()
                .FirstOrDefault(w => w.WeightSheetId == wsId);

            if (ws == null) return null;
            if (!string.Equals(ws.WeightSheetType, "Transfer", StringComparison.OrdinalIgnoreCase))
                return null;

            var seedInfo = ResolveItemSeedInfo(ws.ItemId);
            string variety = seedInfo.ItemDescription;
            string itemIdStr = seedInfo.ItemIdStr;
            string cropIdStr = seedInfo.CropIdStr;
            string cropDescription = seedInfo.Crop;
            bool isSeedItem = seedInfo.IsSeed;

            string sourceName = "";
            if (ws.SourceLocationId.HasValue)
            {
                sourceName = _db.Locations.AsNoTracking()
                    .Where(l => l.LocationId == ws.SourceLocationId.Value)
                    .Select(l => l.Name)
                    .FirstOrDefault() ?? "";
            }
            string destName = "";
            if (ws.DestinationLocationId.HasValue)
            {
                destName = _db.Locations.AsNoTracking()
                    .Where(l => l.LocationId == ws.DestinationLocationId.Value)
                    .Select(l => l.Name)
                    .FirstOrDefault() ?? "";
            }

            var locInfo = _db.Locations
                .AsNoTracking()
                .Where(l => l.LocationId == ws.LocationId)
                .Select(l => new { l.Name, l.Licensed })
                .FirstOrDefault();
            string locationName = locInfo?.Name ?? "";
            bool isLicensed = locInfo?.Licensed ?? true;

            string bolType = ws.RateType switch
            {
                "U" => "Universal",
                "A" => "Along Side Field",
                "F" => "Farm Storage",
                "C" => "Custom",
                _ => ws.RateType ?? ""
            };

            // Same SQL as the intake report — works for transfer rows because
            // ItemId on the detail comes from the WS header and all attribute
            // joins are by transaction id.
            var conn = (Microsoft.Data.SqlClient.SqlConnection)_db.Database.GetDbConnection();
            if (conn.State != System.Data.ConnectionState.Open)
                conn.Open();

            const string sql = @"
                SELECT
                    itd.TransactionId,
                    itd.StartedAt,
                    itd.CompletedAt,
                    itd.StartQty   AS InWeight,
                    itd.EndQty     AS OutWeight,
                    itd.NetQty     AS Net,
                    itd.DirectQty  AS DirectQty,
                    itd.Notes,
                    c.Description  AS ContainerDescription,
                    truckAttr.StringValue AS TruckId,
                    bolAttr.StringValue AS BOL,
                    CASE WHEN startSrcType.Code = 'MANUAL' THEN 1 ELSE 0 END AS StartIsManual,
                    CASE WHEN endSrcType.Code = 'MANUAL' THEN 1 ELSE 0 END AS EndIsManual
                FROM [warehouse].[WeightSheets] ws
                INNER JOIN [Inventory].[InventoryTransactionDetails] itd ON itd.RefId = ws.RowUid AND itd.RefType = 'WeightSheet'
                LEFT JOIN [Inventory].[InventoryTransactionDetailToContainers] tc ON tc.TransactionId = itd.TransactionId
                LEFT JOIN [container].[Containers] c ON c.ContainerId = tc.ContainerId
                LEFT JOIN [Inventory].[TransactionAttributes] truckAttr
                    ON truckAttr.TransactionId = itd.TransactionId
                    AND truckAttr.AttributeTypeId = (SELECT TOP 1 Id FROM [Inventory].[TransactionAttributeTypes] WHERE Code = 'TRUCK_ID')
                LEFT JOIN [Inventory].[TransactionAttributes] bolAttr
                    ON bolAttr.TransactionId = itd.TransactionId
                    AND bolAttr.AttributeTypeId = (SELECT TOP 1 Id FROM [Inventory].[TransactionAttributeTypes] WHERE Code = 'BOL')
                LEFT JOIN [Inventory].[TransactionQuantitySources] startSrc
                    ON startSrc.TransactionId = itd.TransactionId AND startSrc.QuantityField = 'START'
                LEFT JOIN [Inventory].[TransactionQuantitySources] endSrc
                    ON endSrc.TransactionId = itd.TransactionId AND endSrc.QuantityField = 'END'
                LEFT JOIN [Inventory].[QuantitySourceTypes] startSrcType
                    ON startSrcType.QuantitySourceTypeId = startSrc.SourceTypeId
                LEFT JOIN [Inventory].[QuantitySourceTypes] endSrcType
                    ON endSrcType.QuantitySourceTypeId = endSrc.SourceTypeId
                WHERE ws.WeightSheetId = @wsId
                ORDER BY itd.TxnAt ASC";

            var loads = new List<TransferWeightSheetLoadRow>();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@wsId", wsId);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var txnId = reader.GetInt64(reader.GetOrdinal("TransactionId"));
                    var inWt    = reader.IsDBNull(reader.GetOrdinal("InWeight"))    ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("InWeight"));
                    var outWt   = reader.IsDBNull(reader.GetOrdinal("OutWeight"))   ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("OutWeight"));
                    var net     = reader.IsDBNull(reader.GetOrdinal("Net"))         ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Net"));
                    var direct  = reader.IsDBNull(reader.GetOrdinal("DirectQty"))   ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("DirectQty"));
                    var bin     = reader.IsDBNull(reader.GetOrdinal("ContainerDescription")) ? "" : reader.GetString(reader.GetOrdinal("ContainerDescription"));
                    var startedAt   = reader.IsDBNull(reader.GetOrdinal("StartedAt"))   ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("StartedAt"));
                    var completedAt = reader.IsDBNull(reader.GetOrdinal("CompletedAt")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("CompletedAt"));
                    var notes   = reader.IsDBNull(reader.GetOrdinal("Notes"))    ? "" : reader.GetString(reader.GetOrdinal("Notes"));
                    var truckId = reader.IsDBNull(reader.GetOrdinal("TruckId"))  ? "" : reader.GetString(reader.GetOrdinal("TruckId"));
                    var bol     = reader.IsDBNull(reader.GetOrdinal("BOL"))      ? "" : reader.GetString(reader.GetOrdinal("BOL"));
                    var startIsManual = reader.GetInt32(reader.GetOrdinal("StartIsManual")) == 1;
                    var endIsManual   = reader.GetInt32(reader.GetOrdinal("EndIsManual"))   == 1;

                    var effectiveNet = net ?? direct;
                    bool complete = (outWt.HasValue || direct.HasValue) && !string.IsNullOrEmpty(bin);

                    loads.Add(new TransferWeightSheetLoadRow
                    {
                        WeightSheetId = wsId.ToString(),
                        As400Id       = ws.As400Id > 0 ? ws.As400Id.ToString() : "",
                        LoadNumber    = txnId.ToString(),
                        TruckId       = truckId.Length > 4 ? truckId[..4] : truckId,
                        BOL           = bol,
                        TimeIn        = ToServerTime(startedAt),
                        TimeOut       = ToServerTime(completedAt),
                        Bin           = bin,
                        InWeight      = inWt,
                        OutWeight     = outWt,
                        Net           = effectiveNet,
                        Notes         = notes,
                        Status        = complete ? "Complete" : "Incomplete",
                        StartManualFlag = startIsManual ? "M" : " ",
                        EndManualFlag   = endIsManual   ? "M" : " ",
                    });
                }
            }

            for (int i = 0; i < loads.Count; i++)
                loads[i].RowNumber = i + 1;

            int completedCount = loads.Count(l => l.Status.StartsWith("Complete"));
            decimal totalNet = loads.Where(l => l.Net.HasValue).Sum(l => l.Net.Value);

            var as400Str = ws.As400Id.ToString();
            var serverId = as400Str.Length >= 3 ? as400Str[..3] : as400Str;
            var weightSheetNumber = as400Str.Length >= 6 ? as400Str[^6..] : as400Str;
            var serverName = "";
            if (int.TryParse(serverId, out var serverIdInt))
            {
                serverName = _db.Servers.AsNoTracking()
                    .Where(s => s.ServerId == serverIdInt)
                    .Select(s => s.FriendlyName)
                    .FirstOrDefault() ?? "";
            }

            string direction = (ws.DestinationLocationId == ws.LocationId) ? "Received" : "Shipped";

            return new TransferWeightSheetDto
            {
                WeightSheetId        = ws.As400Id > 0 ? ws.As400Id.ToString() : wsId.ToString(),
                As400Id              = ws.As400Id > 0 ? ws.As400Id.ToString() : "",
                ServerId             = serverId,
                ServerName           = serverName,
                WeightSheetNumber    = weightSheetNumber,
                Direction            = direction,
                Variety              = variety,
                ItemId               = itemIdStr,
                ItemDescription      = variety,
                CropId               = cropIdStr,
                Crop                 = cropDescription,
                IsSeed               = isSeedItem,
                SourceLocation       = sourceName,
                SourceLocationId     = ws.SourceLocationId?.ToString() ?? "",
                DestinationLocation  = destName,
                DestinationLocationId= ws.DestinationLocationId?.ToString() ?? "",
                RateType             = bolType,
                HaulerName           = ws.Hauler?.Description ?? "",
                Miles                = ws.Miles,
                Rate                 = ws.Rate,
                CustomRateDescription= ws.CustomRateDescription ?? "",
                WeightSheetNotes     = ws.Notes ?? "",
                Location             = locationName,
                LocationId           = ws.LocationId.ToString(),
                CertificateTitle     = isLicensed
                    ? "UNITED STATES WAREHOUSE ACT GRAIN WEIGHT CERTIFICATE"
                    : "GRAIN WEIGHT CERTIFICATE",
                WeightmasterName     = ws.WeightmasterName ?? "",
                CreationDate         = ws.CreationDate.ToDateTime(TimeOnly.MinValue),
                PrintDate            = ToServerTime(DateTime.UtcNow),
                CopyType             = original ? "ORIGINAL" : "COPY",
                TotalLoads           = loads.Count,
                CompletedLoads       = completedCount,
                TotalNetWeight       = ((int)totalNet).ToString("N0") + " lbs",
                Loads                = loads,
            };
        }

        // ── End Of Day ──────────────────────────────────────────────────────

        public sealed class EndOfDayFinalizeRequest
        {
            public int Pin { get; set; }
        }

        /// <summary>
        /// GET /api/printjobs/eod/{locationId}/pdf
        ///
        /// Builds the End-Of-Day combined PDF for the operator: Daily Intake
        /// + Daily Transfer + Closed Lots summaries first, then every today-
        /// open weight sheet's full report appended after. Returned as a
        /// single PDF stream so the browser shows it inline; the operator
        /// reviews, then confirms email + close via the finalize endpoint.
        /// </summary>
        [HttpGet("eod/{locationId:int}/pdf")]
        public async Task<IActionResult> GetEndOfDayPdf(int locationId, [FromQuery] DateTime? day, CancellationToken ct)
        {
            if (locationId <= 0) return BadRequest(new { message = "locationId is required." });

            // "Today" is the calendar day in the operator's local zone, not
            // UTC — otherwise an evening EOD lands on tomorrow's date.
            var dayValue = (day ?? ServerToday()).Date;

            // 1. Daily summaries
            var intakeDto    = await _eodService.BuildDailyIntakeAsync(locationId, dayValue, ct);
            var transferDto  = await _eodService.BuildDailyTransferAsync(locationId, dayValue, ct);
            var closedDto    = await _eodService.BuildClosedLotsAsync(locationId, dayValue, dayValue, ct);

            var intakeReport   = new DailyIntakeReport   { DataSource = new[] { intakeDto } };
            var transferReport = new DailyTransferReport { DataSource = new[] { transferDto } };
            var closedReport   = new ClosedLotsReport    { DataSource = new[] { closedDto } };
            intakeReport.CreateDocument();
            transferReport.CreateDocument();
            closedReport.CreateDocument();

            // 2. Per-WS reports for every still-open WS today at this location
            var wsIds = await _eodService.GetTodaysOpenWeightSheetIdsAsync(locationId, dayValue, ct);
            var perWsReports = new List<XtraReport>();
            foreach (var wsId in wsIds)
            {
                var wsType = await _db.WeightSheets.AsNoTracking()
                    .Where(w => w.WeightSheetId == wsId)
                    .Select(w => w.WeightSheetType)
                    .FirstOrDefaultAsync(ct);
                if (string.IsNullOrEmpty(wsType)) continue;

                if (string.Equals(wsType, "Transfer", StringComparison.OrdinalIgnoreCase))
                {
                    var dto = BuildTransferWeightSheet(wsId, original: true);
                    if (dto == null) continue;
                    var rpt = new TransferWeightSheetReport { DataSource = new[] { dto } };
                    rpt.CreateDocument();
                    perWsReports.Add(rpt);
                }
                else
                {
                    var dto = BuildIntakeWeightSheet(wsId, original: true);
                    if (dto == null) continue;
                    var rpt = new IntakeWeightSheetReport { DataSource = new[] { dto } };
                    rpt.CreateDocument();
                    perWsReports.Add(rpt);
                }
            }

            // 3. Merge into one master XtraReport's pages so we can export a
            //    single PDF. Documented merge path: keep the first report
            //    as the master and AddRange the others' pages after each
            //    has CreateDocument'd. Continuous page numbering ensures
            //    appended pages re-number correctly.
            var allReports = new List<XtraReport>
            {
                intakeReport, transferReport, closedReport
            };
            allReports.AddRange(perWsReports);

            // Drop reports that produced zero pages so we don't pick a
            // blank one as the master.
            allReports = allReports.Where(r => r.Pages.Count > 0).ToList();
            if (allReports.Count == 0)
                return Problem("End-Of-Day report rendered zero pages.");

            var master = allReports[0];
            for (int i = 1; i < allReports.Count; i++)
            {
                master.Pages.AddRange(allReports[i].Pages);
            }
            master.PrintingSystem.ContinuousPageNumbering = true;

            using var ms = new MemoryStream();
            master.ExportToPdf(ms);
            // Inline so the browser displays the PDF in a tab instead of
            // forcing a download. Filename: "EOD For {LocationName} MM-dd-yy.pdf".
            // Stripped of any chars that browsers won't accept in a quoted
            // filename token; falls back to the location id if the name is
            // unavailable.
            var locName = intakeDto.LocationName;
            if (string.IsNullOrWhiteSpace(locName)) locName = locationId.ToString();
            var safeName = new string(locName
                .Select(c => "\"\\/<>|:*?\r\n".IndexOf(c) >= 0 ? '_' : c)
                .ToArray())
                .Trim();
            var fileName = $"EOD For {safeName} {dayValue:MM-dd-yy}.pdf";
            Response.Headers["Content-Disposition"] = $"inline; filename=\"{fileName}\"";
            return File(ms.ToArray(), "application/pdf");
        }

        /// <summary>
        /// POST /api/printjobs/eod/{locationId}/finalize
        ///
        /// Finalizes End Of Day for the location:
        ///   1. Validates the operator's PIN.
        ///   2. For every today-open Received (Delivery) weight sheet at this
        ///      location, looks up the lot's split-group accounts. Any account
        ///      flagged EmailWeightSheet with a valid Email gets the per-WS
        ///      PDF (marked COPY) emailed via <see cref="IEmailService"/>.
        ///   3. Closes every today-open WS at this location (StatusId = 3,
        ///      ClosedAt = now). Transfer WSs aren't emailed (no producer
        ///      split group) but ARE closed.
        ///
        /// Returns a summary { closed, emailed, failures }.
        /// </summary>
        [HttpPost("eod/{locationId:int}/finalize")]
        public async Task<IActionResult> FinalizeEndOfDay(
            int locationId,
            [FromQuery] DateTime? day,
            [FromBody] EndOfDayFinalizeRequest req,
            CancellationToken ct)
        {
            if (locationId <= 0) return BadRequest(new { message = "locationId is required." });
            if (req == null || req.Pin <= 0)
                return BadRequest(new { message = "PIN is required." });

            // PIN must belong to an active user. Privilege isn't currently
            // enforced for End Of Day — the operator who's running EOD is
            // typically the warehouse weighmaster. Add a priv check here if
            // you want to gate further.
            var pinUser = await _db.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Pin == req.Pin && u.IsActive, ct);
            if (pinUser == null)
                return Unauthorized(new { message = "Invalid or inactive PIN." });

            // Operator-local "today" — see GetEndOfDayPdf.
            var dayValue = (day ?? ServerToday()).Date;
            var wsIds = await _eodService.GetTodaysOpenWeightSheetIdsAsync(locationId, dayValue, ct);

            int closed = 0;
            int emailed = 0;
            var failures = new List<string>();

            // Test-mode behavior: skip the recipient dedupe and send one
            // email per qualifying account (the EmailService rewrites each
            // call to the configured test inbox, so the dev sees one
            // delivery per account in the split group — useful for
            // confirming that every EmailWeightSheet=true account is being
            // resolved correctly). In production we keep the distinct +
            // single-send behavior so producers don't get duplicates.
            var testMode = _emailOpts.TestMode.Enabled;

            foreach (var wsId in wsIds)
            {
                var ws = await _db.WeightSheets
                    .FirstOrDefaultAsync(w => w.WeightSheetId == wsId, ct);
                if (ws == null) continue;

                // Email Received WSs (Delivery type) to the lot's split-group
                // accounts that opted in. Transfer WSs skip the email step.
                if (string.Equals(ws.WeightSheetType, "Delivery", StringComparison.OrdinalIgnoreCase)
                    && ws.LotId.HasValue)
                {
                    var recipients = await ResolveWsEmailRecipientsAsync(
                        ws.LotId.Value, ct, distinct: !testMode);
                    if (recipients.Count > 0)
                    {
                        var dto = BuildIntakeWeightSheet(wsId, original: false); // COPY
                        if (dto != null)
                        {
                            var rpt = new IntakeWeightSheetReport { DataSource = new[] { dto } };
                            rpt.CreateDocument();
                            using var pdfStream = new MemoryStream();
                            rpt.ExportToPdf(pdfStream);
                            var subject = $"Weight Sheet {dto.WeightSheetId} ({dto.CropName})";
                            var body =
                                $"Weight Sheet: {dto.WeightSheetId}\n" +
                                $"Location: {dto.Location}\n" +
                                $"Producer: {dto.PrimaryAccountName}\n" +
                                $"Lot: {dto.LotId}\n" +
                                $"Crop: {dto.CropName}\n";
                            var attachment = new EmailAttachment
                            {
                                FileName = $"WeightSheet-{dto.WeightSheetId}.pdf",
                                ContentType = "application/pdf",
                                Content = pdfStream.ToArray(),
                            };

                            if (testMode)
                            {
                                // One SendAsync per account so the test inbox
                                // sees N separate deliveries.
                                bool anySucceeded = false;
                                foreach (var addr in recipients)
                                {
                                    var ok = await _email.SendAsync(
                                        new[] { addr }, subject, body, new[] { attachment }, ct);
                                    if (ok) anySucceeded = true;
                                    else failures.Add($"Email failed for WS {wsId} → {addr}");
                                }
                                if (anySucceeded) emailed++;
                            }
                            else
                            {
                                var ok = await _email.SendAsync(recipients, subject, body, new[] { attachment }, ct);
                                if (ok) emailed++;
                                else failures.Add($"Email failed for WS {wsId}");
                            }
                        }
                    }
                }

                // Close the WS unconditionally as part of EOD.
                ws.StatusId = 3;
                ws.ClosedAt = DateTime.UtcNow;
                ws.UpdatedAt = DateTime.UtcNow;
                closed++;
            }

            await _db.SaveChangesAsync(ct);

            _log.LogInformation(
                "EOD finalize at location {LocationId} day {Day} by {User}: closed={Closed} emailed={Emailed} failures={Failures}",
                locationId, dayValue, pinUser.UserName, closed, emailed, failures.Count);

            // Broadcast to every subscriber at this location so dashboards,
            // open WeightSheetDeliveryLoads / WeightSheetTransferLoads pages,
            // etc. all refresh — same channel WeightSheetNotifier uses for
            // single-WS edits. weightSheetId=0 means "all WSs at this
            // location"; the dashboard handler ignores wsId and the loads
            // pages short-circuit their wsId filter on a falsy value, so
            // every open client at this location refreshes once.
            if (closed > 0)
            {
                await _notifier.NotifyAsync(locationId, 0, "EodFinalize", ct);
            }

            return Ok(new
            {
                day = dayValue.ToString("yyyy-MM-dd"),
                closed,
                emailed,
                failures,
            });
        }

        /// <summary>
        /// GET /api/printjobs/eod/candidates?ids=1,2,3
        ///
        /// Returns EOD-relevant info for each requested location: open WS
        /// count and whether any of them are dated before today. The
        /// multi-location EOD orchestrator calls this with the user's
        /// accessible-location ids (from /api/LocationContextApi/available)
        /// to decide which sites still need to be processed and to drive
        /// the per-location progress UI.
        /// </summary>
        [HttpGet("eod/candidates")]
        public async Task<IActionResult> GetEodCandidates(
            [FromQuery] string ids,
            [FromQuery] DateTime? day,
            CancellationToken ct)
        {
            var parsed = (ids ?? "")
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => int.TryParse(s.Trim(), out var n) ? n : 0)
                .Where(n => n > 0)
                .Distinct()
                .ToList();
            if (parsed.Count == 0)
                return Ok(Array.Empty<object>());

            var dayValue = (day ?? ServerToday()).Date;
            var candidates = await _eodService.GetEodCandidatesAsync(parsed, dayValue, ct);
            return Ok(candidates);
        }

        /// <summary>
        /// GET /api/printjobs/eod/{locationId}/has-stale
        ///
        /// Returns whether this location has any prior-day open weight
        /// sheets (i.e. WSs that were created on a previous server-day and
        /// are still not closed). The warehouse dashboard hits this on
        /// load to decide whether to auto-prompt for End Of Day.
        /// </summary>
        [HttpGet("eod/{locationId:int}/has-stale")]
        public async Task<IActionResult> GetEodHasStale(
            int locationId,
            [FromServices] IPriorDayWeightSheetGuard guard,
            CancellationToken ct)
        {
            if (locationId <= 0) return BadRequest(new { message = "locationId is required." });
            var ids = await guard.GetPriorDayOpenWeightSheetIdsAsync(locationId, ct);
            return Ok(new { hasStale = ids.Count > 0, count = ids.Count });
        }

        /// <summary>
        /// Resolves the producer email recipients for a Received weight
        /// sheet. Sources accounts from the canonical SplitGroupPercents
        /// (the split group's authoritative percent definition) — every
        /// SplitGroupPercents row whose SplitGroupId matches the lot's
        /// SplitGroupId is mapped to its Account, then filtered to those
        /// flagged EmailWeightSheet with a non-empty Email.
        ///
        /// distinct=true (production default) deduplicates by email so an
        /// address that appears on more than one account is only emailed
        /// once. distinct=false returns one entry per qualifying account
        /// — used in test mode so the test inbox sees one delivery per
        /// account, making it easy to verify every account in the split
        /// group resolved correctly.
        ///
        /// Returns an empty list when the lot has no split group or
        /// nobody on the split group opted in.
        /// </summary>
        private async Task<List<string>> ResolveWsEmailRecipientsAsync(
            long lotId, CancellationToken ct, bool distinct = true)
        {
            var splitGroupId = await _db.Lots.AsNoTracking()
                .Where(l => l.LotId == lotId)
                .Select(l => l.SplitGroupId)
                .FirstOrDefaultAsync(ct);
            if (splitGroupId is null) return new List<string>();

            var query = _db.SplitGroupPercents.AsNoTracking()
                .Where(sgp => sgp.SplitGroupId == splitGroupId.Value)
                .Join(_db.Accounts.AsNoTracking(),
                      sgp => sgp.AccountId,
                      a => a.AccountId,
                      (sgp, a) => a)
                .Where(a => a.EmailWeightSheet && a.Email != null && a.Email != "")
                .Select(a => a.Email);

            if (distinct) query = query.Distinct();

            return await query.ToListAsync(ct);
        }
    }
}

