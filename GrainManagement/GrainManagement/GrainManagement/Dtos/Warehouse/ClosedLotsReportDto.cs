namespace GrainManagement.Dtos.Warehouse;

/// <summary>
/// Data model for the Closed Lots report. Used by the End-Of-Day flow
/// (date range collapses to "today / today") and also by the on-demand
/// closed-lots-by-date-range Crystal port.
/// </summary>
public class ClosedLotsReportDto
{
    public string LocationHeader { get; set; } = "";
    /// <summary>Pre-formatted "MM/dd/yyyy" or "MM/dd/yyyy - MM/dd/yyyy" string.</summary>
    public string DateRangeHeader { get; set; } = "";
    public int TotalCount => Rows?.Count ?? 0;

    public List<ClosedLotsReportRow> Rows { get; set; } = new();
}

public class ClosedLotsReportRow
{
    public string LotNumber { get; set; } = "";
    /// <summary>The date the lot was closed, "MM/dd/yyyy".</summary>
    public string CloseDate { get; set; } = "";
    public string Crop { get; set; } = "";
    public string Producer { get; set; } = "";
}
