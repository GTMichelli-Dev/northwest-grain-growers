namespace GrainManagement.Services.ScaleReader
{
    /// <summary>
    /// Allows external code (e.g. a config-change handler) to signal the ScaleWorker
    /// that it should tear down all pollers and restart with fresh configuration.
    /// </summary>
    public sealed class RestartSignal
    {
        private CancellationTokenSource _cts = new();
        private readonly object _lock = new();

        /// <summary>
        /// A token that is cancelled when a restart is requested.
        /// The ScaleWorker should monitor this and re-initialize when it fires.
        /// </summary>
        public CancellationToken Token
        {
            get
            {
                lock (_lock) return _cts.Token;
            }
        }

        /// <summary>
        /// Signal the ScaleWorker to restart. This cancels the current token
        /// and creates a fresh one for the next cycle.
        /// </summary>
        public void RequestRestart()
        {
            lock (_lock)
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = new CancellationTokenSource();
            }
        }
    }

    /// <summary>
    /// Allows external code to signal the ScaleWorker that it should announce
    /// (re-push) all current scale states to connected SignalR clients.
    /// Useful after a new client connects and wants the latest data.
    /// </summary>
    public sealed class AnnounceSignal
    {
        private CancellationTokenSource _cts = new();
        private readonly object _lock = new();

        /// <summary>
        /// A token that is cancelled when an announce is requested.
        /// </summary>
        public CancellationToken Token
        {
            get
            {
                lock (_lock) return _cts.Token;
            }
        }

        /// <summary>
        /// Signal the ScaleWorker to re-announce all scale states.
        /// </summary>
        public void RequestAnnounce()
        {
            lock (_lock)
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = new CancellationTokenSource();
            }
        }
    }
}
