namespace GrainManagement.Services.Print
{
    /// <summary>
    /// Thread-safe signal used to request a restart of the PrintWorker loop.
    /// Uses ManualResetEventSlim for efficient blocking/signaling.
    /// </summary>
    public sealed class RestartSignal : IDisposable
    {
        private ManualResetEventSlim _event = new ManualResetEventSlim(false);
        private volatile bool _disposed;

        /// <summary>
        /// Signals the worker to restart (e.g., after settings change).
        /// </summary>
        public void RequestRestart()
        {
            if (_disposed) return;
            _event.Set();
        }

        /// <summary>
        /// Blocks the calling thread until a restart is signaled or the token is cancelled.
        /// </summary>
        public void Wait(CancellationToken ct = default)
        {
            if (_disposed) return;
            try
            {
                _event.Wait(ct);
            }
            catch (OperationCanceledException)
            {
                // Expected when the host is stopping
            }
            catch (ObjectDisposedException)
            {
                // Event was disposed during shutdown
            }
        }

        /// <summary>
        /// Blocks the calling thread until a restart is signaled, a timeout expires,
        /// or the token is cancelled.
        /// Returns true if the signal was received, false on timeout.
        /// </summary>
        public bool Wait(TimeSpan timeout, CancellationToken ct = default)
        {
            if (_disposed) return false;
            try
            {
                return _event.Wait(timeout, ct);
            }
            catch (OperationCanceledException)
            {
                return false;
            }
            catch (ObjectDisposedException)
            {
                return false;
            }
        }

        /// <summary>
        /// Resets the signal so it can be waited on again.
        /// Called at the start of each worker loop iteration.
        /// </summary>
        public void Reset()
        {
            if (_disposed) return;
            _event.Reset();
        }

        /// <summary>
        /// Returns true if a restart has been signaled and not yet reset.
        /// </summary>
        public bool IsSet => !_disposed && _event.IsSet;

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            _event.Dispose();
        }
    }
}
