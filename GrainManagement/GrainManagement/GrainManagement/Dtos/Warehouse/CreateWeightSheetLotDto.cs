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
    public string FarmNumber { get; set; }

    /// <summary>PIN of the user creating the lot. Used to populate CreatedByUserName.</summary>
    public int? Pin         { get; set; }

    /// <summary>Lot type: 0 = Seed (default), 1 = Warehouse.</summary>
    public int? LotType     { get; set; }

    /// <summary>
    /// Optional account override — used when the chosen SplitGroup has no PrimaryAccountId.
    /// The caller supplies any active producer account; the server adds a LotSplitGroups row
    /// for it with PrimaryAccount=true and SplitPercent=0 alongside the group's normal rows.
    /// </summary>
    public long? OverrideAccountId { get; set; }
}
