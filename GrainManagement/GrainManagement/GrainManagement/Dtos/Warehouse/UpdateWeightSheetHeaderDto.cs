namespace GrainManagement.Dtos.Warehouse;

public sealed class UpdateWeightSheetHeaderDto
{
    public int?   HaulerId { get; set; }
    public long?  LotId    { get; set; }
    public string Notes    { get; set; }
    public int?   Pin      { get; set; }
}
