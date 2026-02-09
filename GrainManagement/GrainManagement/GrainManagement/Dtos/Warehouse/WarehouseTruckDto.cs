namespace GrainManagement.Dtos.Warehouse;

public sealed class WarehouseTruckDto
{
    public int Id { get; set; }
    public int Bol { get; set; }
    public string Customer { get; set; } = "";
    public string Bin { get; set; } = "";
    public decimal Moist { get; set; }
    public decimal Protein { get; set; }
    public string Carrier { get; set; } = "";
    public string Crop { get; set; } = "";
}
