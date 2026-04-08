using System.ComponentModel.DataAnnotations;

namespace BasicWeigh.Web.Models;

public class AppSetup
{
    [Key]
    public int Id { get; set; }

    [Display(Name = "Next Ticket Number")]
    public int TicketNumber { get; set; } = 1;

    [StringLength(100)]
    [Display(Name = "Header Line 1")]
    public string? Header1 { get; set; }

    [StringLength(100)]
    [Display(Name = "Header Line 2")]
    public string? Header2 { get; set; }

    [StringLength(100)]
    [Display(Name = "Header Line 3")]
    public string? Header3 { get; set; }

    [StringLength(100)]
    [Display(Name = "Header Line 4")]
    public string? Header4 { get; set; }

    [StringLength(100)]
    [Display(Name = "Printer Name")]
    public string? PrinterName { get; set; }

    [Display(Name = "Tickets Per Page")]
    public int TicketsPerPage { get; set; } = 1;

    [Display(Name = "Demo Mode")]
    public bool DemoMode { get; set; }

    [Display(Name = "Kiosk Count")]
    public int KioskCount { get; set; }

    [Display(Name = "Icon")]
    public byte[]? Icon { get; set; }

    [StringLength(50)]
    [Display(Name = "Icon Content Type")]
    public string? IconContentType { get; set; }

    [StringLength(20)]
    [Display(Name = "Theme")]
    public string Theme { get; set; } = "default";

    // Kiosk prompts
    [Display(Name = "Prompt Commodity")]
    public bool PromptKioskCommodity { get; set; } = true;

    [Display(Name = "Prompt Customer")]
    public bool PromptKioskCustomer { get; set; } = true;

    [Display(Name = "Prompt Carrier")]
    public bool PromptKioskCarrier { get; set; } = true;

    [Display(Name = "Prompt Location")]
    public bool PromptKioskLocation { get; set; } = true;

    [Display(Name = "Prompt Truck ID")]
    public bool PromptKioskTruckId { get; set; } = true;

    [Display(Name = "Prompt Destination on Inbound")]
    public bool PromptKioskDestinationOnInbound { get; set; }

    [Display(Name = "Prompt Destination on Outbound")]
    public bool PromptKioskDestinationOnOutbound { get; set; } = true;

    [Display(Name = "Kiosk Dark Mode")]
    public bool KioskDarkMode { get; set; } = true;

    // Login / Security
    [Display(Name = "Require Login")]
    public bool UseLogin { get; set; }

    [StringLength(20)]
    [Display(Name = "Kiosk PIN Code")]
    public string KioskCode { get; set; } = "12345";

    [StringLength(20)]
    [Display(Name = "API Definition PIN")]
    public string ApiDefinitionPin { get; set; } = "12345";

    // QuickBooks integration
    [Display(Name = "Connect to QuickBooks")]
    public bool UseQuickBooks { get; set; }

    // Camera / ticket images
    [Display(Name = "Save Picture for Ticket")]
    public bool SavePicture { get; set; }

    // Camera assignments (format: "serviceId:cameraId")
    [StringLength(100)]
    [Display(Name = "Inbound Camera")]
    public string? InboundCameraId { get; set; }

    [StringLength(100)]
    [Display(Name = "Outbound Camera")]
    public string? OutboundCameraId { get; set; }

    // Scale assignment (format: "serviceId:scaleId")
    [StringLength(100)]
    [Display(Name = "Scale")]
    public string? ScaleId { get; set; }

    // Recall last ticket values
    [Display(Name = "Recall Last Values")]
    public bool RecallLastValues { get; set; }

    // Remote printing mode: None, Scale, RemotePrinter
    [StringLength(20)]
    [Display(Name = "Remote Printing")]
    public string RemotePrintMode { get; set; } = "None";

    // Printer assignments (format: "serviceId:printerId")
    [StringLength(100)]
    [Display(Name = "Inbound Printer")]
    public string? InboundPrinterId { get; set; }

    [StringLength(100)]
    [Display(Name = "Outbound Printer")]
    public string? OutboundPrinterId { get; set; }

    [StringLength(100)]
    [Display(Name = "Kiosk Printer")]
    public string? KioskPrinterId { get; set; }

}
