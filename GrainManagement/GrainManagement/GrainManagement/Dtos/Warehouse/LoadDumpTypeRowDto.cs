namespace GrainManagement.Dtos.Warehouse;

/// <summary>
/// One row in the Load Dump Type report — one load per row, surfaced
/// only when the load carries an IS_END_DUMP transaction attribute.
/// </summary>
public class LoadDumpTypeRowDto
{
    /// <summary>InventoryTransactionDetail.TransactionId — the load id.</summary>
    public long LoadId { get; set; }
    public int LocationId { get; set; }
    public string LocationName { get; set; } = "";
    /// <summary>StartedAt (gross capture) → falls back to TxnAt when null.</summary>
    public DateTime? TimeIn { get; set; }
    /// <summary>True → "End Dump", false → "Belly Dump".</summary>
    public bool IsEndDump { get; set; }
    /// <summary>"End Dump" or "Belly Dump" — pre-rendered for the grid.</summary>
    public string DumpType { get; set; } = "";
}
