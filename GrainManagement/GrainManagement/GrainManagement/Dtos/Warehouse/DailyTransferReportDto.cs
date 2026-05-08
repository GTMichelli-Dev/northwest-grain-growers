namespace GrainManagement.Dtos.Warehouse;

/// <summary>
/// Top-level data model for the End Of Day Transfer report. Mirrors the
/// legacy DailyTransferReport.rpt Crystal layout. Grouped by Commodity (asc)
/// with per-commodity subtotals + grand total.
/// </summary>
public class DailyTransferReportDto
{
    public string LocationName { get; set; } = "";
    public string LocationId { get; set; } = "";
    public string CreationDate { get; set; } = "";

    public List<DailyTransferReportRow> Rows { get; set; } = new();
}

/// <summary>One row per transfer weight sheet on the report's day.</summary>
public class DailyTransferReportRow
{
    public string WeightSheetId { get; set; } = "";
    public string Commodity { get; set; } = "";
    public string CommodityId { get; set; } = "";
    /// <summary>Source location (where the grain came from) — only populated for Received transfers.</summary>
    public string Source { get; set; } = "";
    public string SourceId { get; set; } = "";
    public string Variety { get; set; } = "";
    public string VarietyId { get; set; } = "";
    public string Comment { get; set; } = "";
    public decimal Net { get; set; }
    public decimal Units { get; set; }
    public string Uom { get; set; } = "";
    public bool Closed { get; set; }
}
