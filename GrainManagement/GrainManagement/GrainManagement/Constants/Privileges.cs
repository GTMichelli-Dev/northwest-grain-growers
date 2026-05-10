namespace GrainManagement.Constants;

/// <summary>
/// Server-side privilege IDs (matches the Auth.Privileges table).
/// Each value gates a specific PIN-protected action — controllers query
/// UserPrivileges with these IDs to verify the user behind a PIN holds
/// the required privilege before allowing the operation.
///
/// Existing consts duplicated as private fields inside individual
/// controllers (e.g. PrivilegeIdMoveLoads in GrowerDelivery / Transfer)
/// should migrate to these names over time.
/// </summary>
public static class Privileges
{
    /// <summary>Move a load between weight sheets.</summary>
    public const int MoveLoads = 2;

    /// <summary>Manual quantity entry (typed weight instead of scale read).</summary>
    public const int ManualEntry = 6;

    /// <summary>
    /// Access the Remote Admin tiles page — also acts as the master
    /// "Administrator" marker. Holders of this privilege bypass every other
    /// priv check via <see cref="HasPrivilege"/>.
    /// </summary>
    public const int RemoteAdmin = 7;

    /// <summary>Create a new producer lot.</summary>
    public const int AddLots = 9;

    /// <summary>Edit (modify) a producer lot — open/close, change traits, etc.</summary>
    public const int ModifyLot = 10;

    /// <summary>Edit a Received-flavored (intake) weight sheet header / loads.</summary>
    public const int ModifyReceivedWeightSheet = 12;

    /// <summary>Edit a Transfer weight sheet header / loads.</summary>
    public const int ModifyTransferWeightSheet = 13;

    /// <summary>
    /// Hard-delete a load that hasn't been weighed out yet. Distinct from
    /// Void (priv 6) — a delete physically removes the transaction whereas
    /// Void leaves it on the audit trail.
    /// </summary>
    public const int DeleteLoad = 14;

    /// <summary>
    /// Same numeric value as <see cref="DeleteLoad"/> — privilege 14 is
    /// also the gate for the Central deployment's Maintenance / Admin
    /// pages. Held by HQ "office admins" who can edit master data.
    /// </summary>
    public const int OfficeAdmin = 14;

    /// <summary>
    /// Gate for the Agvantage push pages on /Updates (Warehouse Transfer
    /// and Seed Transfer). Held by the small set of office staff allowed
    /// to push data to the AS400. Distinct from OfficeAdmin so day-to-day
    /// maintenance editors don't accidentally trigger an Agvantage push.
    /// </summary>
    public const int Agvantage = 15;

    /// <summary>
    /// Returns true when <paramref name="held"/> contains <paramref name="required"/>
    /// OR when it contains <see cref="RemoteAdmin"/>. Use this as the single
    /// source of truth for priv checks so the admin bypass is consistent
    /// across every PIN-gated endpoint.
    /// </summary>
    public static bool HasPrivilege(IEnumerable<int> held, int required)
    {
        if (held == null) return false;
        foreach (var p in held)
        {
            if (p == required || p == RemoteAdmin) return true;
        }
        return false;
    }
}
