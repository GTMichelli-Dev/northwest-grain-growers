namespace TempTicketKioskService.Models;

/// <summary>
/// Bound from <c>appsettings.json → Kiosk</c>. One row of config per Pi.
/// </summary>
public sealed class KioskOptions
{
    /// <summary>
    /// Friendly id stamped onto every temp ticket this kiosk produces
    /// (e.g. "bay-1-kiosk"). Operators see this in the management view.
    /// </summary>
    public string KioskId { get; set; } = "kiosk-1";

    /// <summary>
    /// Scale this kiosk reads the weight from. Must match a row in
    /// the web's scale registry.
    /// </summary>
    public int ScaleId { get; set; }

    /// <summary>
    /// Where temp-ticket prints are dispatched. Same shape as the rest
    /// of the print system: "serviceId:printerId" or just "printerId"
    /// to use the default print service.
    /// </summary>
    public string PrinterTarget { get; set; } = "";

    /// <summary>
    /// Base URL of the GrainManagement web app this kiosk POSTs to.
    /// </summary>
    public string ServerUrl { get; set; } = "http://localhost:5000";

    /// <summary>
    /// BCM GPIO pin the button is wired to. Default 17 (physical pin 11
    /// on a 40-pin Raspberry Pi header).
    /// </summary>
    public int GpioPin { get; set; } = 17;

    /// <summary>
    /// Software debounce window. A second falling edge inside this many
    /// milliseconds is treated as bounce and ignored.
    /// </summary>
    public int DebounceMs { get; set; } = 75;

    /// <summary>
    /// Wire-up mode for the button: <c>pullup</c> means the button shorts
    /// the pin to GND when pressed (most common — pin idles HIGH, fires
    /// on Falling edge); <c>pulldown</c> means the button pulls the pin
    /// to 3.3V (idles LOW, fires on Rising edge).
    /// </summary>
    public string PullMode { get; set; } = "pullup";
}
