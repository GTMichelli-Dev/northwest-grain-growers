using System.Collections.Concurrent;
using System.Diagnostics;

namespace CameraService.Services;

/// <summary>
/// Single-source MJPEG fan-out. Pulls one snapshot stream per camera from
/// <see cref="CameraCaptureService"/> at a fixed frame rate and serves
/// that one capture to N HTTP clients. Without this, four people watching
/// the same camera would spawn four ffmpeg processes (USB) or four
/// connections to the camera (IP — Hikvision caps at 2–5 streams). With
/// fan-out, four viewers cost the same as one.
/// </summary>
public sealed class MjpegStreamPool : IAsyncDisposable
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<MjpegStreamPool> _logger;
    private readonly ConcurrentDictionary<string, SharedStream> _streams =
        new(StringComparer.OrdinalIgnoreCase);

    public MjpegStreamPool(IServiceScopeFactory scopeFactory, ILogger<MjpegStreamPool> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    /// <summary>Subscribe to a camera's frames. Returns a handle that releases the slot on dispose.</summary>
    public StreamSubscription Subscribe(string cameraId)
    {
        var stream = _streams.GetOrAdd(cameraId, id => new SharedStream(id, _scopeFactory, _logger));
        return stream.Subscribe();
    }

    public async ValueTask DisposeAsync()
    {
        foreach (var s in _streams.Values) await s.DisposeAsync();
        _streams.Clear();
    }

    // ── Public subscription handle — what controllers hold ───────────────

    public sealed class StreamSubscription : IDisposable
    {
        private readonly SharedStream _owner;
        private int _disposed;
        internal StreamSubscription(SharedStream owner) { _owner = owner; }
        public (byte[]? frame, DateTime atUtc) GetLatest() => _owner.GetLatest();
        public Task WaitForNextFrameAsync(CancellationToken ct) => _owner.WaitForNextFrameAsync(ct);
        public void Dispose()
        {
            if (Interlocked.Exchange(ref _disposed, 1) == 1) return;
            _owner.Unsubscribe();
        }
    }

    // ── Per-camera shared stream ─────────────────────────────────────────

    internal sealed class SharedStream : IAsyncDisposable
    {
        private readonly string _cameraId;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger _logger;
        private readonly CancellationTokenSource _cts = new();

        private int _subscriberCount;
        private byte[]? _currentFrame;
        private DateTime _currentFrameAtUtc;
        private readonly object _frameLock = new();
        private readonly SemaphoreSlim _frameReady = new(0, int.MaxValue);

        private Task? _producerTask;

        public SharedStream(string cameraId, IServiceScopeFactory scopeFactory, ILogger logger)
        {
            _cameraId = cameraId;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public StreamSubscription Subscribe()
        {
            var count = Interlocked.Increment(ref _subscriberCount);
            if (count == 1)
            {
                _producerTask = Task.Run(() => ProducerLoop(_cts.Token));
                _logger.LogInformation("Started MJPEG producer for camera {Camera}.", _cameraId);
            }
            return new StreamSubscription(this);
        }

        public void Unsubscribe()
        {
            var count = Interlocked.Decrement(ref _subscriberCount);
            if (count <= 0)
            {
                // Don't stop the producer immediately — give a short grace window
                // so a page refresh that drops and re-grabs the stream doesn't
                // pay a cold start.
                _ = Task.Run(async () =>
                {
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    if (Volatile.Read(ref _subscriberCount) <= 0)
                    {
                        _cts.Cancel();
                        _logger.LogInformation("Stopped MJPEG producer for camera {Camera}.", _cameraId);
                    }
                });
            }
        }

        public (byte[]? frame, DateTime atUtc) GetLatest()
        {
            lock (_frameLock) return (_currentFrame, _currentFrameAtUtc);
        }

        public Task WaitForNextFrameAsync(CancellationToken ct) => _frameReady.WaitAsync(ct);

        private async Task ProducerLoop(CancellationToken ct)
        {
            // Target ~6 fps — good enough for "is the truck in frame" framing
            // checks and easy on USB ffmpeg / IP camera bandwidth.
            var period = TimeSpan.FromMilliseconds(167);
            var sw = new Stopwatch();
            while (!ct.IsCancellationRequested)
            {
                sw.Restart();
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var capture = scope.ServiceProvider.GetRequiredService<CameraCaptureService>();
                    var bytes = await capture.CaptureAsync(_cameraId);
                    if (bytes is { Length: > 0 })
                    {
                        lock (_frameLock)
                        {
                            _currentFrame = bytes;
                            _currentFrameAtUtc = DateTime.UtcNow;
                        }
                        // Drain and re-signal — keeps waiters at most one frame behind
                        while (_frameReady.CurrentCount > 0) _frameReady.Wait(0, ct);
                        _frameReady.Release();
                    }
                }
                catch (OperationCanceledException) { break; }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "MJPEG capture failed for {Camera}; backing off.", _cameraId);
                    try { await Task.Delay(TimeSpan.FromSeconds(1), ct); } catch { }
                }

                var elapsed = sw.Elapsed;
                if (elapsed < period)
                {
                    try { await Task.Delay(period - elapsed, ct); } catch { break; }
                }
            }
        }

        public async ValueTask DisposeAsync()
        {
            _cts.Cancel();
            if (_producerTask is not null)
            {
                try { await _producerTask; } catch { /* ignore */ }
            }
            _frameReady.Dispose();
        }
    }
}
