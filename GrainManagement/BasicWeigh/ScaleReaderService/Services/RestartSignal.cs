namespace ScaleReaderService.Services;

/// <summary>
/// Singleton signal used to trigger a service restart when settings change.
/// </summary>
public class RestartSignal
{
    private CancellationTokenSource _cts = new();

    public CancellationToken Token => _cts.Token;

    public void TriggerRestart()
    {
        var old = _cts;
        _cts = new CancellationTokenSource();
        old.Cancel();
        old.Dispose();
    }
}

/// <summary>
/// Singleton signal to trigger a scale list re-announce without full restart.
/// </summary>
public class AnnounceSignal
{
    public event Action? OnAnnounceRequested;

    public void TriggerAnnounce() => OnAnnounceRequested?.Invoke();
}
