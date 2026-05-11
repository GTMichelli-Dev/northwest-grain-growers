using System.Device.Gpio;
using Microsoft.Extensions.Options;
using TempTicketKioskService.Models;

namespace TempTicketKioskService.Services;

/// <summary>
/// Watches the configured GPIO pin and POSTs a temp-ticket press to
/// the web on every debounced edge.
///
/// On non-Linux hosts (dev Windows / macOS) <see cref="GpioController"/>
/// can't open a pin, so the monitor logs that GPIO is disabled and
/// parks on a long delay — the Swagger UI's POST /api/test/press still
/// lets the developer drive the rest of the flow.
/// </summary>
public sealed class GpioMonitor : BackgroundService
{
    private readonly IOptionsMonitor<KioskOptions> _opt;
    private readonly KioskHttpClient _client;
    private readonly ILogger<GpioMonitor> _log;

    // Tracks the last accepted edge so the callback can apply a
    // software debounce regardless of what the host's interrupt
    // dispatcher feeds us.
    private long _lastEdgeUtcTicks;

    public GpioMonitor(
        IOptionsMonitor<KioskOptions> opt,
        KioskHttpClient client,
        ILogger<GpioMonitor> log)
    {
        _opt = opt;
        _client = client;
        _log = log;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!OperatingSystem.IsLinux())
        {
            _log.LogWarning(
                "GPIO monitoring disabled on non-Linux host. Use POST /api/test/press to simulate a button press.");
            try { await Task.Delay(Timeout.Infinite, stoppingToken); }
            catch (OperationCanceledException) { /* shutdown */ }
            return;
        }

        var opt = _opt.CurrentValue;
        var pullUp = string.Equals(opt.PullMode, "pullup", StringComparison.OrdinalIgnoreCase);
        var pinMode = pullUp ? PinMode.InputPullUp : PinMode.InputPullDown;
        var triggerEdge = pullUp ? PinEventTypes.Falling : PinEventTypes.Rising;

        GpioController? controller = null;
        try
        {
            controller = new GpioController();
            controller.OpenPin(opt.GpioPin, pinMode);
        }
        catch (Exception ex)
        {
            _log.LogError(ex,
                "Failed to open GPIO pin {Pin} in {Mode}. GPIO monitoring disabled. Swagger /api/test/press still works.",
                opt.GpioPin, pinMode);
            try { await Task.Delay(Timeout.Infinite, stoppingToken); }
            catch (OperationCanceledException) { /* shutdown */ }
            return;
        }

        _log.LogInformation(
            "GPIO monitor watching pin {Pin} ({Mode}, trigger {Edge}, debounce {Debounce} ms).",
            opt.GpioPin, pinMode, triggerEdge, opt.DebounceMs);

        // Hold the delegate as a local so the unregister call gets the
        // same instance back (delegate equality compares the target +
        // method; a fresh lambda wouldn't match).
        PinChangeEventHandler handler = (sender, args) => OnEdge(args);
        controller.RegisterCallbackForPinValueChangedEvent(opt.GpioPin, triggerEdge, handler);

        try
        {
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException) { /* shutdown */ }
        finally
        {
            try { controller.UnregisterCallbackForPinValueChangedEvent(opt.GpioPin, handler); } catch { }
            try { controller.ClosePin(opt.GpioPin); } catch { }
            controller.Dispose();
        }
    }

    private void OnEdge(PinValueChangedEventArgs args)
    {
        var opt = _opt.CurrentValue;
        var nowTicks = DateTime.UtcNow.Ticks;
        var lastTicks = Interlocked.Read(ref _lastEdgeUtcTicks);
        var sinceLast = TimeSpan.FromTicks(nowTicks - lastTicks);
        if (sinceLast < TimeSpan.FromMilliseconds(opt.DebounceMs))
        {
            return; // bounce
        }

        Interlocked.Exchange(ref _lastEdgeUtcTicks, nowTicks);

        // Fire-and-forget — we don't want to block the GPIO interrupt
        // dispatcher thread on the HTTP round-trip.
        _ = Task.Run(async () =>
        {
            try { await _client.PressAsync(source: "gpio"); }
            catch (Exception ex)
            {
                _log.LogWarning(ex, "GPIO press POST failed.");
            }
        });
    }

    /// <summary>Exposed for /api/status — when the most recent press fired.</summary>
    public DateTime? LastEdgeUtc
    {
        get
        {
            var ticks = Interlocked.Read(ref _lastEdgeUtcTicks);
            return ticks == 0 ? null : new DateTime(ticks, DateTimeKind.Utc);
        }
    }
}
