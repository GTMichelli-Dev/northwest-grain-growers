using System.Collections.Concurrent;

namespace GrainManagement.As400Sync;

public sealed class SyncCoordinator
{
    // Global lock so only one sync runs at a time (accounts/products/splits)
    private readonly SemaphoreSlim _global = new(1, 1);

    private readonly ConcurrentDictionary<SyncJob, JobState> _states = new();

    public bool IsRunning(SyncJob job, out DateTimeOffset? startedAtUtc)
    {
        if (_states.TryGetValue(job, out var state) && state.IsRunning)
        {
            startedAtUtc = state.StartedAtUtc;
            return true;
        }

        startedAtUtc = null;
        return false;
    }

    public async Task<Lease?> TryAcquireAsync(SyncJob job, CancellationToken ct)
    {
        // First: try to take global lock immediately (no waiting)
        if (!await _global.WaitAsync(0, ct))
            return null;

        // Mark job running (atomic-ish via dictionary update)
        var now = DateTimeOffset.UtcNow;

        var state = _states.GetOrAdd(job, _ => new JobState());
        lock (state)
        {
            if (state.IsRunning)
            {
                _global.Release();
                return null;
            }

            state.IsRunning = true;
            state.StartedAtUtc = now;
        }

        return new Lease(this, job);
    }

    public sealed class Lease : IAsyncDisposable
    {
        private readonly SyncCoordinator _owner;
        private readonly SyncJob _job;
        private int _disposed;

        internal Lease(SyncCoordinator owner, SyncJob job)
        {
            _owner = owner;
            _job = job;
        }

        public ValueTask DisposeAsync()
        {
            if (Interlocked.Exchange(ref _disposed, 1) == 1)
                return ValueTask.CompletedTask;

            if (_owner._states.TryGetValue(_job, out var state))
            {
                lock (state)
                {
                    state.IsRunning = false;
                    state.StartedAtUtc = null;
                }
            }

            _owner._global.Release();
            return ValueTask.CompletedTask;
        }
    }

    private sealed class JobState
    {
        public bool IsRunning;
        public DateTimeOffset? StartedAtUtc;
    }
}
