namespace CameraService.Services;

/// <summary>
/// Singleton signal used to trigger a service restart when settings change.
/// </summary>
public class RestartSignal
{
    private CancellationTokenSource _cts = new();

    /// <summary>Token that gets cancelled when a restart is requested.</summary>
    public CancellationToken Token => _cts.Token;

    /// <summary>Triggers a restart by cancelling the current token and creating a new one.</summary>
    public void TriggerRestart()
    {
        var old = _cts;
        _cts = new CancellationTokenSource();
        old.Cancel();
        old.Dispose();
    }
}

/// <summary>
/// Singleton signal to trigger a camera list re-announce without full restart.
/// Fired when cameras are added, updated, or deleted via the API.
/// </summary>
public class AnnounceSignal
{
    public event Action? OnAnnounceRequested;

    public void TriggerAnnounce() => OnAnnounceRequested?.Invoke();
}
