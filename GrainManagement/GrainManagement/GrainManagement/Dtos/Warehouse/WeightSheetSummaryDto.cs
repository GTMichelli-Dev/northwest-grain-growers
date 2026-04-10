namespace GrainManagement.Dtos.Warehouse;

/// <summary>
/// Data model for the Weight Sheet Summary report.
/// Mirrors the WeightSheetDeliveryLoads page: header info + load detail rows.
/// </summary>
public class WeightSheetSummaryDto
{
    // ── Header ──────────────────────────────────────────────────────────────
    public string WeightSheetId { get; set; } = "";
    public string LotId { get; set; } = "";
    public string CropName { get; set; } = "";
    public string PrimaryAccountName { get; set; } = "";
    public string PrimaryAccountId { get; set; } = "";
    public string SplitGroupId { get; set; } = "";
    public string SplitName { get; set; } = "";
    public string RateType { get; set; } = "";
    public string HaulerName { get; set; } = "";
    public decimal? Miles { get; set; }
    public decimal? Rate { get; set; }
    public string Comment { get; set; } = "";
    public string Location { get; set; } = "";
    public string LocationId { get; set; } = "";
    public string WeightmasterName { get; set; } = "";
    public string CreationDate { get; set; } = "";
    public string PrintDate { get; set; } = "";

    // ── Summary ─────────────────────────────────────────────────────────────
    public int TotalLoads { get; set; }
    public int CompletedLoads { get; set; }
    public string TotalNetWeight { get; set; } = "";

    // ── Loads (detail rows) ─────────────────────────────────────────────────
    public List<WeightSheetLoadRow> Loads { get; set; } = new();
}

public class WeightSheetLoadRow
{
    public string TransactionId { get; set; } = "";
    public string TimeIn { get; set; } = "";
    public string TimeOut { get; set; } = "";
    public string Bin { get; set; } = "";
    public string InWeight { get; set; } = "";
    public string OutWeight { get; set; } = "";
    public string Net { get; set; } = "";
    public string Protein { get; set; } = "";
    public string Moisture { get; set; } = "";
    public string Notes { get; set; } = "";
    public string Status { get; set; } = "";
    public bool StartIsManual { get; set; }
    public bool EndIsManual { get; set; }
}
