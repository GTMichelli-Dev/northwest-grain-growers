namespace GrainManagement.Dtos.Warehouse;

/// <summary>
/// Shared data model for Inbound, Outbound, and Direct Quantity load ticket reports.
/// Populated from InventoryTransactionDetail + related entities.
/// </summary>
public class LoadTicketDataModel
{
    // ── Identifiers ─────────────────────────────────────────────────────────
    public string LoadId { get; set; } = "";
    public string WeightSheetId { get; set; } = "";
    public string LotNumber { get; set; } = "";

    // ── Account / Split ─────────────────────────────────────────────────────
    public string CropAccount { get; set; } = "";
    public string SplitNumber { get; set; } = "";
    public string SplitDescription { get; set; } = "";

    // ── BOL / Hauler ────────────────────────────────────────────────────────
    public string BolType { get; set; } = "";
    public string Hauler { get; set; } = "";
    /// <summary>Operator-entered truck identifier (TRUCK_ID transaction attribute). Empty for direct loads or when not captured.</summary>
    public string TruckId { get; set; } = "";

    // ── Timestamps ──────────────────────────────────────────────────────────
    public DateTime? InboundTime { get; set; }
    public DateTime? OutboundTime { get; set; }

    // ── Weights ─────────────────────────────────────────────────────────────
    /// <summary>Gross / inbound weight (StartQty).</summary>
    public int InboundWeight { get; set; }

    /// <summary>Tare / outbound weight (EndQty).</summary>
    public int OutboundWeight { get; set; }

    /// <summary>Net weight. For truck loads: from NetQty. For direct loads: from DirectQty.</summary>
    public int NetWeight { get; set; }

    /// <summary>Direct quantity (non-truck loads).</summary>
    public int DirectQty { get; set; }

    // ── Additional display ──────────────────────────────────────────────────
    public string Location { get; set; } = "";
    public string LocationId { get; set; } = "";
    public string Commodity { get; set; } = "";
    /// <summary>Item description on the load. Same value as <see cref="Commodity"/>; kept for binding clarity alongside the seed-flag toggle.</summary>
    public string ItemDescription { get; set; } = "";
    /// <summary>Item id on the load (string for clean report binding — empty when unknown).</summary>
    public string ItemId { get; set; } = "";
    /// <summary>Product.CropId for the load's item, formatted as string. Empty when not set.</summary>
    public string CropId { get; set; } = "";
    /// <summary>Crop description — Item.Description for the Item whose ItemId matches the CropId.</summary>
    public string Crop { get; set; } = "";
    /// <summary>True when the item carries the SEED trait (TraitId=31) AND its product's Category is NOT a non-seed category (CHEM/FERT/PACK/SERVICE). Mirrors /api/Lookups/SeedItems. Use this to show ItemId/ItemDescription only on seed tickets and Crop info always.</summary>
    public bool IsSeed { get; set; }
    public string Bin { get; set; } = "";

    // ── Manual-entry legal flags ────────────────────────────────────────────
    /// <summary>"M" if the inbound weight was entered manually, otherwise " " (space). Must appear on printed tickets for legal reasons.</summary>
    public string StartManualFlag { get; set; } = " ";
    /// <summary>"M" if the outbound weight was entered manually, otherwise " " (space). Must appear on printed tickets for legal reasons.</summary>
    public string EndManualFlag { get; set; } = " ";
    /// <summary>"M" if the direct-quantity value was entered manually, otherwise " " (space). Must appear on printed tickets for legal reasons.</summary>
    public string DirectManualFlag { get; set; } = " ";
}
