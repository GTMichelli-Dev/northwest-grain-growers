namespace GrainManagement.As400Sync;

/// <summary>
/// Snapshot pushed up to the website hub so the admin page can show what the
/// AS400Sync service is currently doing. Stages are free-form strings: the UI
/// just renders them; the service decides what to call each step.
/// </summary>
public sealed class SyncProgress
{
    public string ServiceId { get; init; } = "";
    public string Job { get; init; } = "";       // "Accounts" | "Products" | "SplitGroups"
    public string Stage { get; init; } = "";     // "Connecting" | "Reading" | "Upserting" | "MarkInactive" | "Done" | "Error"
    public long Current { get; init; }
    public long? Total { get; init; }
    public string? Message { get; init; }
    public DateTimeOffset AtUtc { get; init; } = DateTimeOffset.UtcNow;
    public Guid? SyncRunId { get; init; }
    public string? Version { get; init; }
}
