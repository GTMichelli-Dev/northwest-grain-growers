#nullable disable
using System;

namespace GrainManagement.Models;

/// <summary>
/// A weight captured by a kiosk button-press at the scale. Survives
/// across page loads so an operator can later "retrieve" the weight
/// when they actually create the inbound load. Deleted once the
/// matching load is saved, or by the nightly purge for orphans.
/// </summary>
public partial class TempWeightTicket
{
    public long TempTicketId { get; set; }

    /// <summary>system.Servers.ServerId — the web instance that stored this ticket.</summary>
    public int ServerId { get; set; }

    /// <summary>Scale that produced the weight.</summary>
    public int ScaleId { get; set; }

    /// <summary>Friendly id of the kiosk that pressed the button.</summary>
    public string KioskId { get; set; }

    public decimal Gross { get; set; }
    public decimal? Tare { get; set; }
    public decimal? Net { get; set; }

    /// <summary>"lbs" by default.</summary>
    public string Units { get; set; }

    /// <summary>Relative path under TicketImages root (e.g. "tmp_4321.jpg").</summary>
    public string ImagePath { get; set; }

    public DateTime CreatedAt { get; set; }

    /// <summary>Set when the temp ticket has been printed + image saved.</summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>Set when an operator clicks "Use This Ticket" and the load saves.</summary>
    public long? ConsumedByLotId { get; set; }

    public virtual Server Server { get; set; }
}
