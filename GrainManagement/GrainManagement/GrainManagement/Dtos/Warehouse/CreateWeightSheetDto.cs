namespace GrainManagement.Dtos.Warehouse;

public sealed class CreateWeightSheetDto
{
    public int  LocationId { get; set; }
    public long? LotId     { get; set; }
}
