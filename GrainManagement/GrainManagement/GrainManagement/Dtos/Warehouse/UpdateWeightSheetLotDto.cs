namespace GrainManagement.Dtos.Warehouse;

public sealed class UpdateWeightSheetLotDto
{
    public string LotDescription { get; set; }
    public long? ItemId          { get; set; }
    public string Notes          { get; set; }
    public string State          { get; set; }
    public string County         { get; set; }
}
