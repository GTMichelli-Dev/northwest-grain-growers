namespace ScaleReaderService.Services
{
    /// <summary>
    /// Allows external code (e.g. CRUD handlers) to signal the ScaleWorker
    /// to tear down all pollers and restart with fresh configuration.
    /// </summary>
    public sealed class RestartSignal
    {
        private CancellationTokenSource _cts = new();
        private readonly object _lock = new();

        public CancellationToken Token
        {
            get { lock (_lock) return _cts.Token; }
        }

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
}
