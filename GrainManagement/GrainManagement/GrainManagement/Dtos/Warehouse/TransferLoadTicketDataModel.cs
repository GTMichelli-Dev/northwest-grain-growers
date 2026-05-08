namespace GrainManagement.Dtos.Warehouse;

/// <summary>
/// Shared data model for the transfer-flavored load ticket reports
/// (TransferInboundLoadTicketReport / TransferOutboundLoadTicketReport).
///
/// Mirrors LoadTicketDataModel for the common load fields (load id,
/// weights, hauler, manual-entry flags) and adds transfer-specific
/// header fields (Source / Destination / Variety / Direction). Lot /
/// account / split fields aren't included — transfer loads aren't
/// tied to a producer lot.
/// </summary>
public class TransferLoadTicketDataModel
{
    // ── Identifiers ─────────────────────────────────────────────────────────
    public string LoadId { get; set; } = "";
    public string WeightSheetId { get; set; } = "";

    // ── Transfer-specific header ────────────────────────────────────────────
    /// <summary>"Received" or "Shipped" — the WS direction at the current location.</summary>
    public string Direction { get; set; } = "";
    /// <summary>Item description (variety) being transferred.</summary>
    public string Variety { get; set; } = "";
    public string ItemId { get; set; } = "";
    /// <summary>Source location name (where the grain is coming from).</summary>
    public string SourceLocation { get; set; } = "";
    /// <summary>Source location id (string for clean report binding — empty when unknown).</summary>
    public string SourceLocationId { get; set; } = "";
    /// <summary>Destination location name (where the grain is going).</summary>
    public string DestinationLocation { get; set; } = "";
    /// <summary>Destination location id (string for clean report binding — empty when unknown).</summary>
    public string DestinationLocationId { get; set; } = "";

    // ── BOL / Hauler ────────────────────────────────────────────────────────
    public string BolType { get; set; } = "";
    public string Hauler { get; set; } = "";
    /// <summary>Operator-entered truck identifier (TRUCK_ID transaction attribute). Empty when not captured.</summary>
    public string TruckId { get; set; } = "";

    // ── Timestamps ──────────────────────────────────────────────────────────
    public DateTime? InboundTime { get; set; }
    public DateTime? OutboundTime { get; set; }

    // ── Weights ─────────────────────────────────────────────────────────────
    public int InboundWeight { get; set; }
    public int OutboundWeight { get; set; }
    public int NetWeight { get; set; }
    public int DirectQty { get; set; }

    // ── Additional display ──────────────────────────────────────────────────
    public string Location { get; set; } = "";
    public string LocationId { get; set; } = "";
    public string Commodity { get; set; } = "";
    public string Bin { get; set; } = "";

    // ── Manual-entry legal flags ────────────────────────────────────────────
    public string StartManualFlag { get; set; } = " ";
    public string EndManualFlag { get; set; } = " ";
    public string DirectManualFlag { get; set; } = " ";
}
