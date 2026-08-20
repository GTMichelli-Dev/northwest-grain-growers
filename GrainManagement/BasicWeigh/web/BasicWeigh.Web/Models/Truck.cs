using System.ComponentModel.DataAnnotations;

namespace BasicWeigh.Web.Models;

public class Truck
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    [Display(Name = "Truck ID")]
    public string TruckId { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    [Display(Name = "Carrier")]
    public string CarrierName { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Description { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    [Display(Name = "Use at Kiosk")]
    public bool UseAtKiosk { get; set; } = true;
}
