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

        // ── Weight Sheet Summary ────────────────────────────────────────────────

        [HttpGet("weight-sheet-summary/{wsId:long}/pdf")]
        public IActionResult GetWeightSheetSummaryPdf([FromRoute] long wsId)
        {
            var model = BuildWeightSheetSummary(wsId);
            if (model == null)
                return NotFound(new { error = $"Weight sheet {wsId} not found." });

            var report = new WeightSheetSummaryReport();
            report.DataSource = new[] { model };
            report.CreateDocument();

            if (report.Pages.Count == 0)
                return Problem("Report rendered zero pages.");

            using var ms = new MemoryStream();
            report.ExportToPdf(ms);
            return File(ms.ToArray(), "application/pdf", $"WeightSheet-{wsId}.pdf");
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
            };
        }

        /// <summary>
        /// Builds the WeightSheetSummaryDto by loading the WS header + all delivery loads.
        /// Uses the same SQL query as GetWeightSheetDeliveryLoads for consistency.
        /// </summary>
        private WeightSheetSummaryDto? BuildWeightSheetSummary(long wsId)
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
            string cropName = "";
            string accountName = "";
            string accountId = "";
            string splitId = "";
            string splitName = "";
            string locationName = "";

            if (ws.LotId.HasValue)
            {
                var lotInfo = _db.Lots
                    .Include(l => l.SplitGroup)
                    .Include(l => l.Product)
                    .AsNoTracking()
                    .FirstOrDefault(l => l.LotId == ws.LotId);

                if (lotInfo != null)
                {
                    lotAs400Id = lotInfo.As400Id;
                    cropName = lotInfo.Product?.Description ?? "";
                    splitId = lotInfo.SplitGroupId?.ToString() ?? "";
                    splitName = lotInfo.SplitGroup?.SplitGroupDescription ?? "";

                    if (lotInfo.SplitGroup != null)
                    {
                        var acct = _db.Accounts
                            .AsNoTracking()
                            .FirstOrDefault(a => a.AccountId == lotInfo.SplitGroup.PrimaryAccountId);
                        if (acct != null)
                        {
                            accountName = !string.IsNullOrWhiteSpace(acct.EntityName) ? acct.EntityName : acct.LookupName ?? "";
                            accountId = acct.AccountId.ToString();
                        }
                    }
                }

                locationName = _db.Locations
                    .AsNoTracking()
                    .Where(l => l.LocationId == ws.LocationId)
                    .Select(l => l.Name)
                    .FirstOrDefault() ?? "";
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
                    CASE WHEN startSrcType.Code = 'MANUAL' THEN 1 ELSE 0 END AS StartIsManual,
                    CASE WHEN endSrcType.Code = 'MANUAL' THEN 1 ELSE 0 END AS EndIsManual
                FROM [warehouse].[WeightSheets] ws
                INNER JOIN [Inventory].[InventoryTransactionDetails] itd ON itd.RefId = ws.RowUid AND itd.RefType = 'WeightSheet'
                LEFT JOIN [Inventory].[InventoryTransactionDetailToContainers] tc ON tc.TransactionId = itd.TransactionId
                LEFT JOIN [container].[Containers] c ON c.ContainerId = tc.ContainerId
                LEFT JOIN [Inventory].[TransactionAttributes] attr1 ON attr1.TransactionId = itd.TransactionId AND attr1.AttributeTypeId = 1
                LEFT JOIN [Inventory].[TransactionAttributes] attr2 ON attr2.TransactionId = itd.TransactionId AND attr2.AttributeTypeId = 2
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

            var loads = new List<WeightSheetLoadRow>();
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
                    var startIsManual = reader.GetInt32(reader.GetOrdinal("StartIsManual")) == 1;
                    var endIsManual = reader.GetInt32(reader.GetOrdinal("EndIsManual")) == 1;

                    bool complete = outWt.HasValue && !string.IsNullOrEmpty(container) && protein.HasValue && protein > 0;
                    bool completeMissingProtein = outWt.HasValue && !string.IsNullOrEmpty(container) && !(protein.HasValue && protein > 0);

                    var inWeightStr = inWt.HasValue ? ((int)inWt.Value).ToString("N0") : "";
                    var outWeightStr = outWt.HasValue ? ((int)outWt.Value).ToString("N0") : "";
                    if (startIsManual && inWeightStr != "") inWeightStr += " M";
                    if (endIsManual && outWeightStr != "") outWeightStr += " M";

                    loads.Add(new WeightSheetLoadRow
                    {
                        TransactionId = FormatId(txnId),
                        TimeIn = startedAt?.ToString("M/d/yyyy h:mm tt") ?? "",
                        TimeOut = completedAt?.ToString("M/d/yyyy h:mm tt") ?? "",
                        Bin = container,
                        InWeight = inWeightStr,
                        OutWeight = outWeightStr,
                        Net = net.HasValue ? ((int)net.Value).ToString("N0") : "",
                        Protein = protein.HasValue && protein > 0 ? protein.Value.ToString("F2") : "",
                        Moisture = moisture.HasValue && moisture > 0 ? moisture.Value.ToString("F2") : "",
                        Notes = notes,
                        Status = complete ? "Complete" : (completeMissingProtein ? "Complete*" : "Incomplete"),
                        StartIsManual = startIsManual,
                        EndIsManual = endIsManual,
                    });
                }
            }

            int completedCount = loads.Count(l => l.Status.StartsWith("Complete"));
            decimal totalNet = loads.Where(l => !string.IsNullOrEmpty(l.Net))
                .Sum(l => decimal.TryParse(l.Net.Replace(",", ""), out var v) ? v : 0);

            return new WeightSheetSummaryDto
            {
                WeightSheetId = ws.As400Id > 0 ? ws.As400Id.ToString() : FormatId(wsId),
                LotId = lotAs400Id > 0 ? lotAs400Id.ToString() : FormatId(lotId),
                CropName = cropName,
                PrimaryAccountName = accountName,
                PrimaryAccountId = accountId,
                SplitGroupId = splitId,
                SplitName = splitName,
                RateType = bolType,
                HaulerName = ws.Hauler?.Description ?? "",
                WeightmasterName = ws.WeightmasterName ?? "",
                CreationDate = ws.CreationDate.ToString("MM/dd/yyyy"),
                Miles = ws.Miles,
                Rate = ws.Rate,
                Comment = ws.Notes ?? "",
                Location = locationName,
                LocationId = ws.LocationId.ToString(),
                PrintDate = DateTime.Now.ToString("M/d/yyyy h:mm tt"),
                TotalLoads = loads.Count,
                CompletedLoads = completedCount,
                TotalNetWeight = ((int)totalNet).ToString("N0") + " lbs",
                Loads = loads,
            };
        }
    }
}

