using System.Threading;
using System.Threading.Tasks;

namespace GrainManagement.Services.Camera;

/// <summary>
/// Allows external code to signal the CameraWorker to restart its connection loop.
/// Register as a singleton.
/// </summary>
public sealed class RestartSignal
{
    private volatile TaskCompletionSource<bool> _tcs = new(TaskCreationOptions.RunContinuationsAsynchronously);

    /// <summary>
    /// Signal the worker to restart. Any pending WaitAsync call will complete.
    /// </summary>
    public void Signal()
    {
        var old = Interlocked.Exchange(ref _tcs, new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously));
        old.TrySetResult(true);
    }

    /// <summary>
    /// Waits until Signal() is called or the cancellation token fires.
    /// </summary>
    public async Task WaitAsync(CancellationToken ct)
    {
        var current = _tcs;
        using var reg = ct.Register(() => current.TrySetCanceled());
        await current.Task;
    }
}

/// <summary>
/// Allows the CameraWorker to signal that it has connected and is ready.
/// Register as a singleton.
/// </summary>
public sealed class AnnounceSignal
{
    private volatile TaskCompletionSource<bool> _tcs = new(TaskCreationOptions.RunContinuationsAsynchronously);

    /// <summary>
    /// Signal that the worker is ready / connected.
    /// </summary>
    public void Signal()
    {
        var old = Interlocked.Exchange(ref _tcs, new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously));
        old.TrySetResult(true);
    }

    /// <summary>
    /// Waits until Signal() is called or the cancellation token fires.
    /// </summary>
    public async Task WaitAsync(CancellationToken ct)
    {
        var current = _tcs;
        using var reg = ct.Register(() => current.TrySetCanceled());
        await current.Task;
    }
}
