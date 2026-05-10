namespace GrainManagement.Models;

/// <summary>
/// Canonical Code values for <see cref="LocationAttributeType.Code"/>.
/// Mirrors the existing pattern around <c>TransactionAttributeType.Code</c>
/// — string constants instead of an enum so the literals are visible at
/// the call sites and stay in lockstep with the seed data.
/// </summary>
public static class LocationAttributeCodes
{
    /// <summary>Bool — when true, the load save flow prompts the
    /// operator for an end-dump answer and persists IS_END_DUMP on the
    /// transaction.</summary>
    public const string RequireDumpType = "REQUIRE_DUMP_TYPE";
}

/// <summary>
/// Companion to <see cref="LocationAttributeCodes"/> for the new
/// transaction-level attribute introduced alongside REQUIRE_DUMP_TYPE.
/// </summary>
public static class TransactionAttributeCodes
{
    /// <summary>Bool — set on a load when the operator answered "yes"
    /// to the end-dump prompt. Only valid at locations whose
    /// REQUIRE_DUMP_TYPE LocationAttribute is true.</summary>
    public const string IsEndDump = "IS_END_DUMP";
}
