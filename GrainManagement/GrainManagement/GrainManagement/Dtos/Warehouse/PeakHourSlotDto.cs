namespace GrainManagement.Dtos.Warehouse;

/// <summary>
/// One row in the Peak Hours report — load count for a given location
/// at a given hour-of-day. The aggregate across every selected location
/// is included as <see cref="LocationId"/> = 0, <see cref="LocationName"/>
/// = "All".
/// </summary>
public class PeakHourSlotDto
{
    public int LocationId { get; set; }
    public string LocationName { get; set; } = "";
    /// <summary>0–23. Bucket index for the hour of day in Pacific time.</summary>
    public int Hour { get; set; }
    /// <summary>"01:00", "13:00", … — pre-formatted axis label.</summary>
    public string HourLabel { get; set; } = "";
    public int LoadCount { get; set; }
}
