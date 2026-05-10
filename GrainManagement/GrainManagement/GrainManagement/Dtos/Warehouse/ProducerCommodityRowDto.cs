namespace GrainManagement.Dtos.Warehouse;

/// <summary>
/// One row in the Producer Commodity Report By District. Intake-only.
/// One row per (District, Location, Producer/SplitGroup, Crop) — or per
/// variety when the crop is a seed item.
/// </summary>
public class ProducerCommodityRowDto
{
    public int DistrictId { get; set; }
    public string DistrictName { get; set; } = "";
    public int LocationId { get; set; }
    public string LocationName { get; set; } = "";

    public string PrimaryAccountId { get; set; } = "";
    public string PrimaryAccountName { get; set; } = "";

    public string SplitGroupId { get; set; } = "";
    public string SplitGroupDescription { get; set; } = "";

    /// <summary>Product.Description (the crop name).</summary>
    public string Crop { get; set; } = "";
    /// <summary>Product.CropId — used for the Crops filter / grouping key.</summary>
    public string CropId { get; set; } = "";

    /// <summary>True when the Lot's item carries the SEED trait (matches
    /// /api/Lookups/SeedItems). Drives whether the Variety column renders.</summary>
    public bool IsSeed { get; set; }
    public string Variety { get; set; } = "";
    public string VarietyId { get; set; } = "";

    /// <summary>Net weight in lbs (sum of intake load NetQty).</summary>
    public decimal NetLbs { get; set; }
    /// <summary>Net in the crop's primary UoM (NetLbs / ToBaseFactor).</summary>
    public decimal NetUnits { get; set; }
    public string PrimaryUom { get; set; } = "";
    public int LoadCount { get; set; }
}
