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

    // Live access for /api/status. Held only between OpenPin and
    // Dispose; null on non-Linux hosts or when the open failed.
    private GpioController? _controller;
    private int _pinNumber;
    private PinEventTypes _triggerEdge;

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

        _controller = controller;
        _pinNumber = opt.GpioPin;
        _triggerEdge = triggerEdge;

        // One-shot read so the first line in the log shows the resting
        // value of the line — useful for confirming the pull mode is
        // wired the way you think it is.
        PinValue? initial = null;
        try { initial = controller.Read(opt.GpioPin); } catch { /* ignore */ }

        _log.LogInformation(
            "GPIO monitor watching pin {Pin} ({Mode}, trigger {Edge}, debounce {Debounce} ms). Idle value = {Idle}.",
            opt.GpioPin, pinMode, triggerEdge, opt.DebounceMs,
            initial?.ToString() ?? "?");

        // Register for BOTH edges so the log can show every transition
        // including bounce. The trigger-edge filter is applied inside
        // the callback so the debounce + dispatch logic stays on a
        // single code path.
        PinChangeEventHandler handler = (sender, args) => OnEdge(args);
        controller.RegisterCallbackForPinValueChangedEvent(
            opt.GpioPin,
            PinEventTypes.Falling | PinEventTypes.Rising,
            handler);

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
            _controller = null;
        }
    }

    private void OnEdge(PinValueChangedEventArgs args)
    {
        var opt = _opt.CurrentValue;
        var kind = args.ChangeType; // Falling or Rising

        // Edges of the non-trigger direction are just for visibility —
        // log them and bail. Useful when you're not sure if the pull
        // mode is right and you want to see exactly which direction
        // your button is producing.
        if ((kind & _triggerEdge) == 0)
        {
            _log.LogInformation("GPIO pin {Pin}: {Edge} (other edge — ignored)",
                opt.GpioPin, kind);
            return;
        }

        var nowTicks = DateTime.UtcNow.Ticks;
        var lastTicks = Interlocked.Read(ref _lastEdgeUtcTicks);
        var sinceLast = TimeSpan.FromTicks(nowTicks - lastTicks);
        if (sinceLast < TimeSpan.FromMilliseconds(opt.DebounceMs))
        {
            _log.LogInformation(
                "GPIO pin {Pin}: {Edge} BOUNCE (Δ={DeltaMs:F0} ms < debounce {Debounce} ms — ignored)",
                opt.GpioPin, kind, sinceLast.TotalMilliseconds, opt.DebounceMs);
            return;
        }

        Interlocked.Exchange(ref _lastEdgeUtcTicks, nowTicks);

        _log.LogInformation(
            "GPIO pin {Pin}: {Edge} PRESS (Δ={DeltaMs:F0} ms — firing press to {Server})",
            opt.GpioPin, kind, sinceLast.TotalMilliseconds, opt.ServerUrl);

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

    /// <summary>
    /// Live read of the configured GPIO pin. Returns null on non-Linux
    /// hosts or when the controller is closed.
    /// </summary>
    public PinValue? ReadPin()
    {
        var c = _controller;
        if (c is null) return null;
        try { return c.Read(_pinNumber); }
        catch { return null; }
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
