namespace GrainManagement.Dtos.Warehouse;

public sealed class WarehouseWeightSheetDetailDto
{
    public int WeightCertificateId { get; set; }
    public long Lot { get; set; }
    public decimal Net { get; set; }
    public string Grower { get; set; } = "";
    public string Account { get; set; } = "";
    public string Landlord { get; set; } = "";
    public string Field { get; set; } = "";
    public string Commodity { get; set; } = "";
    public string Carrier { get; set; } = "";
    public string Comments { get; set; } = "";
}
