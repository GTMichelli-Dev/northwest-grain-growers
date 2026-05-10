namespace PictureUpsert.Services;

/// <summary>
/// Live status snapshot the Swagger / status endpoint reads. Kept as a
/// singleton so the worker writes it and the controller reads it without
/// touching the DB on every health poll.
/// </summary>
public sealed class StatusState
{
    private readonly object _lock = new();

    public bool RemoteReachable { get; private set; }
    public DateTime? LastSuccessUtc { get; private set; }
    public DateTime? LastAttemptUtc { get; private set; }
    public string? LastError { get; private set; }
    public int PendingCount { get; private set; }
    public int FailedCount { get; private set; }
    public long TotalSent { get; private set; }

    public void RecordSuccess(int pending, int failed)
    {
        lock (_lock)
        {
            RemoteReachable = true;
            LastSuccessUtc  = DateTime.UtcNow;
            LastAttemptUtc  = DateTime.UtcNow;
            LastError       = null;
            PendingCount    = pending;
            FailedCount     = failed;
            TotalSent++;
        }
    }

    public void RecordFailure(string error, int pending, int failed)
    {
        lock (_lock)
        {
            RemoteReachable = false;
            LastAttemptUtc  = DateTime.UtcNow;
            LastError       = error;
            PendingCount    = pending;
            FailedCount     = failed;
        }
    }

    public void RecordCounts(int pending, int failed)
    {
        lock (_lock)
        {
            PendingCount = pending;
            FailedCount  = failed;
        }
    }
}
