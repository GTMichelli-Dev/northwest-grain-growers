namespace GrainManagement.Dtos.Warehouse;

public sealed class CreateWeightSheetLotDto
{
    public int LocationId   { get; set; }
    public int SplitGroupId { get; set; }
    public long? ItemId     { get; set; }
    public string Notes     { get; set; }
    public string State     { get; set; }
    public string County    { get; set; }
    public string Landlord  { get; set; }
}
