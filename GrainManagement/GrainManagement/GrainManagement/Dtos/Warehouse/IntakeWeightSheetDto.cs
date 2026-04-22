namespace GrainManagement.Dtos.Warehouse;

/// <summary>
/// Data model for the Intake Weight Sheet report.
/// Mirrors the WeightSheetDeliveryLoads page: header info + load detail rows.
/// </summary>
public class IntakeWeightSheetDto
{
    // ── Header ──────────────────────────────────────────────────────────────
    public string WeightSheetId { get; set; } = "";
    public string As400Id { get; set; } = "";
    /// <summary>First 3 digits of As400Id (e.g. "604").</summary>
    public string ServerId { get; set; } = "";
    /// <summary>Server.FriendlyName matching the ServerId (e.g. "Alto").</summary>
    public string ServerName { get; set; } = "";
    /// <summary>Last 6 digits of As400Id — the weight sheet number (e.g. "000013").</summary>
    public string WeightSheetNumber { get; set; } = "";
    public string LotId { get; set; } = "";
    public string ItemId { get; set; } = "";
    public string CropName { get; set; } = "";
    public string PrimaryAccountName { get; set; } = "";
    public string PrimaryAccountId { get; set; } = "";
    public string SplitGroupId { get; set; } = "";
    public string SplitName { get; set; } = "";
    public string RateType { get; set; } = "";
    public string HaulerName { get; set; } = "";
    public decimal? Miles { get; set; }
    public decimal? Rate { get; set; }
    public string LotNotes { get; set; } = "";
    public string WeightSheetNotes { get; set; } = "";
    public string Location { get; set; } = "";
    public string LocationId { get; set; } = "";
    /// <summary>
    /// "GRAIN WEIGHT CERTIFICATE" when the location is licensed;
    /// "UNITED STATES WAREHOUSE ACT GRAIN WEIGHT CERTIFICATE" otherwise.
    /// </summary>
    public string CertificateTitle { get; set; } = "";
    /// <summary>Landlord name from the lot (TraitTypeId=18).</summary>
    public string LandlordName { get; set; } = "";
    /// <summary>Farm number from the lot (TraitType FARM_NUMBER).</summary>
    public string FarmNumber { get; set; } = "";
    /// <summary>State from the lot (TraitTypeId=15).</summary>
    public string State { get; set; } = "";
    /// <summary>County from the lot (TraitTypeId=16).</summary>
    public string County { get; set; } = "";
    /// <summary>True when this is the last weight sheet used for the lot before it was closed.</summary>
    public bool IsFinalWeightSheet { get; set; }
    public string WeightmasterName { get; set; } = "";
    public DateTime? CreationDate { get; set; }
    public DateTime? PrintDate { get; set; }
    /// <summary>"Original" for first print, "Copy" for reprints.</summary>
    public string CopyType { get; set; } = "";

    // ── Summary ─────────────────────────────────────────────────────────────
    public int TotalLoads { get; set; }
    public int CompletedLoads { get; set; }
    public string TotalNetWeight { get; set; } = "";

    // ── Loads (detail rows) ─────────────────────────────────────────────────
    public List<IntakeWeightSheetLoadRow> Loads { get; set; } = new();
}

public class IntakeWeightSheetLoadRow
{
    public int RowNumber { get; set; }
    public string WeightSheetId { get; set; } = "";
    public string As400Id { get; set; } = "";
    /// <summary>Last 6 digits of the TransactionId (e.g. "000001").</summary>
    public string LoadNumber { get; set; } = "";
    public string TruckId { get; set; } = "";
    public string BOL { get; set; } = "";
    public DateTime? TimeIn { get; set; }
    public DateTime? TimeOut { get; set; }
    public string Bin { get; set; } = "";
    public decimal? InWeight { get; set; }
    public decimal? OutWeight { get; set; }
    public decimal? Net { get; set; }
    public string Protein { get; set; } = "";
    public string Moisture { get; set; } = "";
    public string Notes { get; set; } = "";
    public string Status { get; set; } = "";

    /// <summary>"M" if the inbound weight was entered manually, otherwise " " (space). Legal requirement: must appear on tickets.</summary>
    public string StartManualFlag { get; set; } = " ";
    /// <summary>"M" if the outbound weight was entered manually, otherwise " " (space). Legal requirement: must appear on tickets.</summary>
    public string EndManualFlag { get; set; } = " ";
}
