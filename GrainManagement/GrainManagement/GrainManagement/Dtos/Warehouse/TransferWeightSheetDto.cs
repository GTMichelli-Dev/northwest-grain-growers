namespace GrainManagement.Dtos.Warehouse;

/// <summary>
/// Data model for the Transfer Weight Sheet report. Same shape as
/// IntakeWeightSheetDto with lot-specific fields dropped and source/
/// destination/direction added.
/// </summary>
public class TransferWeightSheetDto
{
    // ── Header ──────────────────────────────────────────────────────────────
    public string WeightSheetId { get; set; } = "";
    public string As400Id { get; set; } = "";
    /// <summary>First 3 digits of As400Id (e.g. "604").</summary>
    public string ServerId { get; set; } = "";
    public string ServerName { get; set; } = "";
    /// <summary>Last 6 digits of As400Id — the weight sheet number.</summary>
    public string WeightSheetNumber { get; set; } = "";

    /// <summary>"Received" or "Shipped".</summary>
    public string Direction { get; set; } = "";
    /// <summary>Item description (variety) being transferred.</summary>
    public string Variety { get; set; } = "";
    /// <summary>Source location name.</summary>
    public string SourceLocation { get; set; } = "";
    public string SourceLocationId { get; set; } = "";
    /// <summary>Destination location name.</summary>
    public string DestinationLocation { get; set; } = "";
    public string DestinationLocationId { get; set; } = "";

    public string RateType { get; set; } = "";
    public string HaulerName { get; set; } = "";
    public decimal? Miles { get; set; }
    public decimal? Rate { get; set; }
    public string CustomRateDescription { get; set; } = "";
    public string WeightSheetNotes { get; set; } = "";
    /// <summary>Current location name (the location creating the WS).</summary>
    public string Location { get; set; } = "";
    public string LocationId { get; set; } = "";
    public string CertificateTitle { get; set; } = "";
    public string WeightmasterName { get; set; } = "";
    public DateTime? CreationDate { get; set; }
    public DateTime? PrintDate { get; set; }
    /// <summary>"ORIGINAL" or "COPY".</summary>
    public string CopyType { get; set; } = "";

    // ── Summary ─────────────────────────────────────────────────────────────
    public int TotalLoads { get; set; }
    public int CompletedLoads { get; set; }
    public string TotalNetWeight { get; set; } = "";

    // ── Loads (detail rows) — same shape as the intake report ──────────────
    public List<TransferWeightSheetLoadRow> Loads { get; set; } = new();
}

public class TransferWeightSheetLoadRow
{
    public int RowNumber { get; set; }
    public string WeightSheetId { get; set; } = "";
    public string As400Id { get; set; } = "";
    public string LoadNumber { get; set; } = "";
    public string TruckId { get; set; } = "";
    public string BOL { get; set; } = "";
    public DateTime? TimeIn { get; set; }
    public DateTime? TimeOut { get; set; }
    public string Bin { get; set; } = "";
    public decimal? InWeight { get; set; }
    public decimal? OutWeight { get; set; }
    public decimal? Net { get; set; }
    public string Notes { get; set; } = "";
    public string Status { get; set; } = "";
    public string StartManualFlag { get; set; } = " ";
    public string EndManualFlag { get; set; } = " ";
}
