namespace GrainManagement.Dtos.Warehouse;

/// <summary>
/// One row in the Intake Location Report. Intake-only, grouped by
/// either (Location, Primary Account) or (Location, Split Group)
/// depending on the report's GroupBy toggle. Adds Net Units + Primary
/// UoM on top of the Producer Delivery columns.
/// </summary>
public class IntakeLocationRowDto
{
    public int DistrictId { get; set; }
    public string DistrictName { get; set; } = "";
    public int LocationId { get; set; }
    public string LocationName { get; set; } = "";

    public string PrimaryAccountId { get; set; } = "";
    public string PrimaryAccountName { get; set; } = "";

    public string SplitGroupId { get; set; } = "";
    public string SplitGroupDescription { get; set; } = "";

    public int LoadCount { get; set; }
    public decimal NetLbs { get; set; }
    /// <summary>NetLbs converted to the crop's primary UoM
    /// (NetLbs / Category.DefaultUom.ToBaseFactor).</summary>
    public decimal NetUnits { get; set; }
    /// <summary>Default UoM code for the row's first observed crop —
    /// "BU" / "TON" / "LB" etc. Empty when the category had no
    /// DefaultUomId.</summary>
    public string PrimaryUom { get; set; } = "";
}
