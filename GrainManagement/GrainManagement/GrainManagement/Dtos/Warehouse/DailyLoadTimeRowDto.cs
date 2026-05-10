namespace GrainManagement.Dtos.Warehouse;

/// <summary>
/// One row in the Daily Load Times report — one load per row with the
/// truck's time in (StartedAt) and time out (CompletedAt) plus the
/// duration spent on the scale.
/// </summary>
public class DailyLoadTimeRowDto
{
    public int LocationId { get; set; }
    public string LocationName { get; set; } = "";
    /// <summary>"yyyy-MM-dd" — the WS CreationDate.</summary>
    public string Date { get; set; } = "";
    public string WeightSheetId { get; set; } = "";
    public string LoadId { get; set; } = "";
    public string TruckId { get; set; } = "";
    public string Bin { get; set; } = "";

    public DateTime? TimeIn { get; set; }
    public DateTime? TimeOut { get; set; }
    /// <summary>Minutes between TimeIn and TimeOut. Null when either
    /// timestamp is missing.</summary>
    public decimal? DurationMinutes { get; set; }
    public decimal NetLbs { get; set; }
}
