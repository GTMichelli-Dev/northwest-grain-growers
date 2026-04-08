namespace GrainManagement.Dtos.Warehouse;

public sealed class UpdateWeightSheetHeaderDto
{
    public int?     HaulerId              { get; set; }
    public long?    LotId                 { get; set; }
    public string   Notes                 { get; set; }
    public int?     Pin                   { get; set; }
    public string   RateType              { get; set; }
    public int?     Miles                 { get; set; }
    public string   CustomRateDescription { get; set; }
    public decimal? Rate                  { get; set; }
}
