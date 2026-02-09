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
}