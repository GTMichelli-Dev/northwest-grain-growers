namespace GrainManagement.Dtos.Warehouse;

/// <summary>
/// One row in the Producer Delivery Report. Intake-only, grouped by
/// either (Location, Primary Account) or (Location, Split Group)
/// depending on the report's GroupBy toggle.
/// </summary>
public class ProducerDeliveryRowDto
{
    public int DistrictId { get; set; }
    public string DistrictName { get; set; } = "";
    public int LocationId { get; set; }
    public string LocationName { get; set; } = "";

    public string PrimaryAccountId { get; set; } = "";
    public string PrimaryAccountName { get; set; } = "";

    /// <summary>Empty when grouping by primary account.</summary>
    public string SplitGroupId { get; set; } = "";
    /// <summary>Empty when grouping by primary account.</summary>
    public string SplitGroupDescription { get; set; } = "";

    public int LoadCount { get; set; }
    public decimal NetLbs { get; set; }
}
