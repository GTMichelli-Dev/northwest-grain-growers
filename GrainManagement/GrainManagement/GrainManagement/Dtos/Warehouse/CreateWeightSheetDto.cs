namespace GrainManagement.Dtos.Warehouse;

public sealed class CreateWeightSheetDto
{
    public int  LocationId            { get; set; }
    public long? LotId                { get; set; }
    public string RateType            { get; set; }   // U, A, F, C
    public int?  HaulerId             { get; set; }
    public decimal? Miles             { get; set; }
    public string CustomRateDescription { get; set; }
    public decimal? Rate              { get; set; }
}
