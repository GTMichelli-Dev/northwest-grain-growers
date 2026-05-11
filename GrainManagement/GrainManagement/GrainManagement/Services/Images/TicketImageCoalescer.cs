using System.Collections.Concurrent;

namespace GrainManagement.Services.Images;

/// <summary>
/// Debounced composite scheduler. Each per-camera upload calls
/// <see cref="Schedule"/>; this resets a 2s timer keyed on
/// <c>{ticket}|{direction}</c>. When the timer fires (no further
/// uploads in that window) the composite runs once.
///
/// Singleton — same instance handles every in-flight load.
/// </summary>
public sealed class TicketImageCoalescer
{
    private static readonly TimeSpan Debounce = TimeSpan.FromSeconds(2);

    private readonly ConcurrentDictionary<string, CancellationTokenSource> _pending = new();
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<TicketImageCoalescer> _log;

    public TicketImageCoalescer(IServiceScopeFactory scopeFactory, ILogger<TicketImageCoalescer> log)
    {
        _scopeFactory = scopeFactory;
        _log = log;
    }

    public void Schedule(string ticket, string direction)
    {
        if (string.IsNullOrWhiteSpace(ticket) || string.IsNullOrWhiteSpace(direction)) return;

        var key = $"{ticket}|{direction}";
        var newCts = new CancellationTokenSource();

        // Cancel any earlier pending composite for the same key. The
        // earlier task observes the OperationCanceledException and exits
        // before doing any work.
        if (_pending.TryGetValue(key, out var prev))
        {
            try { prev.Cancel(); } catch { /* ignore */ }
        }
        _pending[key] = newCts;

        _ = Task.Run(async () =>
        {
            try
            {
                await Task.Delay(Debounce, newCts.Token);
            }
            catch (OperationCanceledException)
            {
                return;
            }

            // Only remove our token if it's still the registered one —
            // a newer Schedule call may have replaced us already.
            _pending.TryUpdate(key, null!, newCts);
            _pending.TryRemove(new KeyValuePair<string, CancellationTokenSource>(key, null!));

            try
            {
                using var scope = _scopeFactory.CreateScope();
                var compositor = scope.ServiceProvider.GetRequiredService<ITicketImageCompositor>();
                await compositor.CompositeAsync(ticket, direction, newCts.Token);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Composite scheduler failed for {Ticket} {Direction}.", ticket, direction);
            }
        });
    }
}
