namespace GrainManagement.Dtos.Warehouse;

/// <summary>
/// Data model for the Lot Label report (1 1/8" x 3 1/2" label).
/// </summary>
public class LotLabelDto
{
    public string LotId { get; set; } = "";
    public string As400Id { get; set; } = "";
    public string SplitGroupId { get; set; } = "";
    public string SplitGroupDescription { get; set; } = "";
    public string CropName { get; set; } = "";
    public string CreatedByUserName { get; set; } = "";
    public string CreatedDate { get; set; } = "";
    public string PrimaryAccountName { get; set; } = "";
    public string PrimaryAccountId { get; set; } = "";
    public string LocationId { get; set; } = "";
    public string LocationDescription { get; set; } = "";
}
