namespace GrainManagement.Dtos.Warehouse;

public sealed class WarehouseCertDto
{
    public long Lot { get; set; }
    public string Customer { get; set; } = "";
    public string Landlord { get; set; } = "";
    public string Carrier { get; set; } = "";
    public string Crop { get; set; } = "";
}
