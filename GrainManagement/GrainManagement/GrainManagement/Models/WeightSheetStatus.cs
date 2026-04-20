#nullable disable
using System.Collections.Generic;

namespace GrainManagement.Models;

// Reference table for warehouse.WeightSheets.StatusId.
// Row values are fixed and blocked from DML via INSTEAD OF triggers — see
// SQL/AddWeightSheetStatus.sql. Only these four values are valid:
//   0 = Open                  — still accepting loads
//   1 = PendingNotFinished    — 25 loads reached, outbound/protein/bin incomplete
//   2 = PendingFinished       — 25 loads reached, outbound/protein/bin all set
//   3 = Closed                — end-of-day finalized, immutable
public partial class WeightSheetStatus
{
    public byte StatusId { get; set; }

    public string StatusCode { get; set; }

    public string Description { get; set; }

    public virtual ICollection<WeightSheet> WeightSheets { get; set; } = new List<WeightSheet>();
}
