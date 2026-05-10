public sealed class LocationDto
{
    public int LocationId { get; set; }

    public int DistrictId { get; set; }
    public string District { get; set; }

    public string Code { get; set; }

    public string Name { get; set; }

    public bool IsActive { get; set; }

    public bool UseForSeed { get; set; }

    public bool UseForWarehouse { get; set; }

    public bool Licensed { get; set; }

    /// <summary>True when the operator should be prompted "Is this an
    /// end dump?" on each new load at this location. Backed by the
    /// REQUIRE_DUMP_TYPE LocationAttribute.</summary>
    public bool RequireDumpType { get; set; }
}