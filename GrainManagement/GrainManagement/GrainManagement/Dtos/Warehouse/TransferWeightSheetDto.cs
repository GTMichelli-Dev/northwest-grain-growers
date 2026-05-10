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
    /// <summary>Item description (variety) being transferred. Kept for
    /// existing report bindings; same value as <see cref="ItemDescription"/>.</summary>
    public string Variety { get; set; } = "";

    /// <summary>WS.ItemId formatted as string. Empty when the WS has no item.</summary>
    public string ItemId { get; set; } = "";
    /// <summary>Item.Description for WS.ItemId. Same value as <see cref="Variety"/>.</summary>
    public string ItemDescription { get; set; } = "";

    /// <summary>Product.CropId for the WS's item, formatted as string. Empty when not set.</summary>
    public string CropId { get; set; } = "";
    /// <summary>Crop description — Item.Description for the Item whose ItemId matches the CropId.
    /// (CropId references the "parent crop" item in the product/item hierarchy.) Empty when not resolvable.</summary>
    public string Crop { get; set; } = "";

    /// <summary>
    /// True when the WS's item carries the SEED trait (TraitId=31) AND its
    /// Product's Category is NOT a non-seed category (CHEM / FERT / PACK /
    /// SERVICE). Mirrors the filter used by /api/Lookups/SeedItems and the
    /// Seed Lot / Seed Transfer item pickers, so the report can hide the
    /// ItemId / ItemDescription block for non-seed transfers and show only
    /// the Crop instead.
    /// </summary>
    public bool IsSeed { get; set; }

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

    // ── Image hyperlinks (server-aware) ─────────────────────────────────────
    // Direction mapping depends on the parent WS:
    //   Received WS  : InImage = StartQty photo (in),  OutImage = EndQty photo (out)
    //   Shipped WS   : InImage = StartQty photo (out), OutImage = EndQty photo (in)
    // Empty when no image is expected for that direction (no weight / no
    // scanned BOL).
    public string InImageUrl { get; set; } = "";
    public string OutImageUrl { get; set; } = "";
    public string BolImageUrl { get; set; } = "";
}
