using System.ComponentModel.DataAnnotations;

namespace BasicWeigh.Web.Models;

public class Carrier
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    [Display(Name = "Carrier Name")]
    public string CarrierName { get; set; } = string.Empty;

    public bool Active { get; set; } = true;

    [Display(Name = "Use at Kiosk")]
    public bool UseAtKiosk { get; set; } = true;
}
