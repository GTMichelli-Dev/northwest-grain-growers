namespace GrainManagement.Dtos.Warehouse;

/// <summary>
/// Payload for creating a transfer weight sheet — the inter-location
/// counterpart of a Delivery weight sheet. Carries an Item (variety)
/// and a source/destination pair instead of a Lot.
/// </summary>
public sealed class CreateTransferWeightSheetDto
{
    /// <summary>The current location creating the WS — required for routing
    /// and as one half of the source/destination pair.</summary>
    public int LocationId { get; set; }

    /// <summary>"Received" or "Shipped". Determines which side of the pair
    /// is the current location.</summary>
    public string Direction { get; set; }

    /// <summary>Variety being transferred.</summary>
    public long ItemId { get; set; }

    /// <summary>Source location for the move. For a Received transfer, this
    /// is the user-picked counterpart; for Shipped, this is the current
    /// location.</summary>
    public int SourceLocationId { get; set; }

    /// <summary>Destination location for the move. For a Shipped transfer,
    /// this is the user-picked counterpart; for Received, this is the
    /// current location.</summary>
    public int DestinationLocationId { get; set; }

    public string  RateType              { get; set; }   // U, A, F, C, I (In House)
    public int?    HaulerId              { get; set; }
    public decimal? Miles                { get; set; }
    public string  CustomRateDescription { get; set; }
    public decimal? Rate                 { get; set; }
    public int? Pin                      { get; set; }
}
