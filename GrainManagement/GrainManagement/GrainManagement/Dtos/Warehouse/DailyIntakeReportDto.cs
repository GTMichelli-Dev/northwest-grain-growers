namespace GrainManagement.Dtos.Warehouse;

/// <summary>
/// Top-level data model for the End Of Day Intake report. Mirrors the legacy
/// DailyIntakeReport.rpt Crystal layout (header + repeating rows), shaped so
/// the DevExpress XtraReport designer can bind directly via ObjectDataSource.
///
/// Grouping in the report: Detail rows are grouped by Commodity (asc) with
/// per-commodity subtotals + a grand total in the footer.
/// </summary>
public class DailyIntakeReportDto
{
    public string LocationName { get; set; } = "";
    public string LocationId { get; set; } = "";
    /// <summary>The day this report covers, formatted "MM/dd/yyyy".</summary>
    public string CreationDate { get; set; } = "";

    public List<DailyIntakeReportRow> Rows { get; set; } = new();
}

/// <summary>One row per delivery weight sheet on the report's day.</summary>
public class DailyIntakeReportRow
{
    public string WeightSheetId { get; set; } = "";
    public string Commodity { get; set; } = "";
    public string CommodityId { get; set; } = "";
    public string LotNumber { get; set; } = "";
    public bool LotOpen { get; set; }
    public string Customer { get; set; } = "";
    public string CustomerId { get; set; } = "";
    public string Landlord { get; set; } = "";
    public string FsaNumber { get; set; } = "";
    public string Comment { get; set; } = "";
    /// <summary>Net weight in lbs (or whatever the location's UOM is).</summary>
    public decimal Net { get; set; }
    /// <summary>Net converted to bushels (or whichever UOM the report shows alongside lbs).</summary>
    public decimal Units { get; set; }
    public string Uom { get; set; } = "";
    public bool Closed { get; set; }
}
