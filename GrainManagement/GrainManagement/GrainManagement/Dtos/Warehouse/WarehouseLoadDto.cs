namespace GrainManagement.Dtos.Warehouse;

public sealed class WarehouseLoadDto
{
    public int Bol { get; set; }
    public string Bin { get; set; } = "";
    public decimal Moist { get; set; }
    public decimal Protein { get; set; }
    public decimal Gross { get; set; }
    public decimal Tare { get; set; }
    public decimal Net { get; set; }
}
