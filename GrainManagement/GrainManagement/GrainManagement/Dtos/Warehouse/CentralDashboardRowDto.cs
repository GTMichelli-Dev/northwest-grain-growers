namespace GrainManagement.Dtos.Warehouse;

/// <summary>
/// One grid row on the Central deployment home page. All weight values are
/// pounds at the row level — the UI converts to tons (lbs / 2000) and bushels
/// (lbs / item UOM factor) for display.
/// </summary>
public sealed class CentralDashboardRowDto
{
    public int LocationId { get; set; }
    public string LocationName { get; set; } = "";

    public int IntakeLoads { get; set; }
    public decimal IntakeLbs { get; set; }
    public decimal IntakeBu  { get; set; }

    public int TransferInLoads { get; set; }
    public decimal TransferInLbs { get; set; }
    public decimal TransferInBu  { get; set; }

    public int TransferOutLoads { get; set; }
    /// <summary>Always non-negative here — the UI renders the value as negative.</summary>
    public decimal TransferOutLbs { get; set; }
    public decimal TransferOutBu  { get; set; }

    public decimal NetLbs => IntakeLbs + TransferInLbs - TransferOutLbs;
    public decimal NetBu  => IntakeBu  + TransferInBu  - TransferOutBu;

    public bool HasActivity => IntakeLoads > 0 || TransferInLoads > 0 || TransferOutLoads > 0;
}
