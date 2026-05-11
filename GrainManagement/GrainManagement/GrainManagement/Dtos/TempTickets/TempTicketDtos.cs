namespace GrainManagement.Dtos.TempTickets;

/// <summary>Payload the kiosk Pi service POSTs when its button is pressed.</summary>
public sealed class TempTicketPressRequest
{
    /// <summary>Friendly id of the kiosk that pressed the button (e.g. "bay-1-kiosk").</summary>
    public string KioskId { get; set; } = "";

    /// <summary>Scale to read the weight from.</summary>
    public int ScaleId { get; set; }

    /// <summary>
    /// Where to send the temp ticket print job. Same shape as the
    /// PrintDispatchService target: "serviceId:printerId" or just "printerId"
    /// to fall through to the default print service.
    /// </summary>
    public string PrinterTarget { get; set; } = "";
}

/// <summary>Synchronous reply to /press — the row has been queued or stored.</summary>
public sealed class TempTicketPressResponse
{
    public long? TempTicketId { get; set; }
    public string Status { get; set; } = "";  // "queued" | "stored" | "rejected"
    public string? Message { get; set; }
}

/// <summary>Row shape returned to the load-create picker + the management view.</summary>
public sealed class TempTicketDto
{
    public long TempTicketId { get; set; }
    public int ServerId { get; set; }
    public int ScaleId { get; set; }
    public string KioskId { get; set; } = "";
    public decimal Gross { get; set; }
    public decimal? Tare { get; set; }
    public decimal? Net { get; set; }
    public string Units { get; set; } = "lbs";
    public string? ImagePath { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}
