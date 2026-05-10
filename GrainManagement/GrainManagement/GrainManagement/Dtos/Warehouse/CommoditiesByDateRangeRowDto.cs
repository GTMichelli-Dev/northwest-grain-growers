namespace GrainManagement.Dtos.Warehouse;

/// <summary>
/// One row in the Commodities By Date Range table — grouped by
/// (Location, Crop). Net values come back in the crop's primary unit of
/// measure (Item → Product → Category → DefaultUom). The grid renders
/// with the location as a group header and crop rows beneath it.
/// </summary>
public class CommoditiesByDateRangeRowDto
{
    public int LocationId { get; set; }
    public string LocationName { get; set; } = "";

    /// <summary>Crop label — Item.Description for the WS item's parent
    /// crop (Product.CropId chased back to Items). Empty when the WS item
    /// has no resolvable parent crop.</summary>
    public string Crop { get; set; } = "";

    /// <summary>Default UoM code for the crop's category — "BU" / "TON"
    /// / "LB" / etc. Empty when the category has no DefaultUomId.</summary>
    public string PrimaryUom { get; set; } = "";

    public decimal IntakeNet { get; set; }
    public int IntakeLoadCount { get; set; }

    public decimal TransferFromNet { get; set; }
    public int TransferFromLoadCount { get; set; }

    public decimal TransferToNet { get; set; }
    public int TransferToLoadCount { get; set; }

    /// <summary>Net change at the location: Intake + TransferTo − TransferFrom.</summary>
    public decimal NetChange { get; set; }
}
