using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BasicWeigh.Web.Models;

public class Transaction
{
    [Key]
    [StringLength(20)]
    public string Ticket { get; set; } = string.Empty;

    public bool Void { get; set; }

    [Display(Name = "In Weight")]
    public int InWeight { get; set; }

    [Display(Name = "Date In")]
    public DateTime DateIn { get; set; }

    [Display(Name = "Date Out")]
    public DateTime? DateOut { get; set; }

    [Display(Name = "Out Weight")]
    public int? OutWeight { get; set; }

    [StringLength(50)]
    public string? Customer { get; set; }

    [StringLength(50)]
    public string? Commodity { get; set; }

    [StringLength(50)]
    public string? Carrier { get; set; }

    [StringLength(50)]
    [Display(Name = "Truck ID")]
    public string? TruckId { get; set; }

    [StringLength(50)]
    public string? Location { get; set; }

    [StringLength(50)]
    public string? Destination { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    [Display(Name = "Manual Inbound")]
    public bool ManualInbound { get; set; }

    [Display(Name = "Manual Outbound")]
    public bool ManualOutbound { get; set; }

    [Display(Name = "Sent to QuickBooks")]
    public bool SentToQuickBooks { get; set; }

    [NotMapped]
    public int GrossWeight => Math.Max(InWeight, OutWeight ?? 0);

    [NotMapped]
    public int TareWeight => OutWeight.HasValue ? Math.Min(InWeight, OutWeight.Value) : 0;

    [NotMapped]
    public int NetWeight => OutWeight.HasValue ? Math.Abs(InWeight - OutWeight.Value) : 0;

    [NotMapped]
    public bool IsComplete => OutWeight.HasValue && DateOut.HasValue;
}
