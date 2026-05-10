namespace GrainManagement.Dtos.Warehouse;

/// <summary>
/// Data model for the Daily Weight Sheet Series report — one row per
/// weight sheet at a given location on a given day, with totals in the
/// footer. Bound to DevExpress XtraReport
/// <see cref="GrainManagement.Reporting.DailyWeightSheetSeriesReport"/>.
/// </summary>
public class DailyWeightSheetSeriesReportDto
{
    public string LocationName { get; set; } = "";
    public string LocationId { get; set; } = "";
    /// <summary>"MM/dd/yyyy" — the Pacific calendar day the report covers.</summary>
    public string ReportDate { get; set; } = "";

    public List<DailyWeightSheetSeriesRow> Rows { get; set; } = new();

    public int TotalLoads { get; set; }
    /// <summary>Sum of NetLbs for all rows, formatted with thousands separators.</summary>
    public string TotalNetDisplay { get; set; } = "";
    public decimal TotalNetLbs { get; set; }
}

public class DailyWeightSheetSeriesRow
{
    public string WeightSheetId { get; set; } = "";
    /// <summary>"Delivery" or "Transfer".</summary>
    public string Type { get; set; } = "";
    /// <summary>Parent crop label (Item.Description for the Item whose ItemId matches Product.CropId).</summary>
    public string Crop { get; set; } = "";
    public int LoadCount { get; set; }
    public decimal NetLbs { get; set; }
    /// <summary>NetLbs formatted with thousands separators for direct binding.</summary>
    public string NetDisplay { get; set; } = "";
}
