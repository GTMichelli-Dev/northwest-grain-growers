using GrainManagement.Dtos.Warehouse;
using GrainManagement.Models;
using GrainManagement.Reporting;
using DevExpress.XtraReports.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrainManagement.Api
{
    [ApiController]
    [Route("api/printjobs")]
    public class PrintJobsController : ControllerBase
    {
        private readonly dbContext _db;
        private readonly ILogger<PrintJobsController> _log;

        public PrintJobsController(dbContext db, ILogger<PrintJobsController> log)
        {
            _db = db;
            _log = log;
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
            var model = BuildLoadTicketDataModel(ticket, out var detail);
            if (model == null || detail == null)
                return NotFound(new { error = $"Transaction {ticket} not found." });

            var report = SelectReport(detail);

            _log.LogInformation(
                "Generating {ReportType} PDF for transaction {TransactionId}",
                report.GetType().Name, ticket);

            report.DataSource = new[] { model };
            report.CreateDocument();

            if (report.Pages.Count == 0)
                return Problem("Report rendered zero pages.");

            using var ms = new MemoryStream();
            report.ExportToPdf(ms);

            return File(ms.ToArray(), "application/pdf", $"LoadTicket-{ticket}.pdf");
        }

        // ── Explicit endpoints (for when the caller knows which report they want) ──

        [HttpGet("inbound-ticket/{ticket}/pdf")]
        public IActionResult GetInboundTicketPdf([FromRoute] string ticket)
            => RenderReport<InboundLoadTicketReport>(ticket, "InboundTicket");

        [HttpGet("outbound-ticket/{ticket}/pdf")]
        public IActionResult GetOutboundTicketPdf([FromRoute] string ticket)
            => RenderReport<OutboundLoadTicketReport>(ticket, "OutboundTicket");

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
            return File(ms.ToArray(), "application/pdf", $"WeightSheet-{wsId}.pdf");
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
        ///   2. StartQty set + EndQty set      → OutboundLoadTicketReport
        ///   3. StartQty set + EndQty null      → InboundLoadTicketReport
        /// </summary>
        private static XtraReport SelectReport(InventoryTransactionDetail detail)
        {
            // Direct quantity loads (non-truck: manual, rail, bulk loader, etc.)
            if (detail.DirectQty.HasValue)
                return new DirectQuantityLoadTicketReport();

            // Truck loads with both inbound and outbound weights
            if (detail.StartQty.HasValue && detail.EndQty.HasValue)
                return new OutboundLoadTicketReport();

            // Truck loads with only inbound weight (not yet scaled out)
            return new InboundLoadTicketReport();
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
                : FormatId(detail.LotId);
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
                InboundTime      = detail.StartedAt ?? detail.TxnAt,
                OutboundTime     = detail.CompletedAt,
                InboundWeight    = (int)(detail.StartQty ?? 0),
                OutboundWeight   = (int)(detail.EndQty ?? 0),
                NetWeight        = netWeight,
                DirectQty        = (int)(detail.DirectQty ?? 0),
                Location         = detail.Transaction?.Location?.Name ?? "",
                LocationId       = detail.Transaction?.LocationId.ToString() ?? "",
                Commodity        = detail.Product?.Description ?? "",
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
                        TimeIn = startedAt,
                        TimeOut = completedAt,
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
                PrintDate = DateTime.Now,
                CopyType = original ? "ORIGINAL" : "COPY",
                TotalLoads = loads.Count,
                CompletedLoads = completedCount,
                TotalNetWeight = ((int)totalNet).ToString("N0") + " lbs",
                Loads = loads,
            };
        }
    }
}

