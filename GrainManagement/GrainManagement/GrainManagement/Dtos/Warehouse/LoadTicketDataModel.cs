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
    public string Bin { get; set; } = "";
}
