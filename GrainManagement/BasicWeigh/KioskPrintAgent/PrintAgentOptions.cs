namespace KioskPrintAgent;

public class PrintAgentOptions
{
    public string ServerUrl { get; set; } = "http://localhost:5110";
    public string PrinterName { get; set; } = "";
    public int PrintCopies { get; set; } = 1;
    public string TempDir { get; set; } = "/tmp/kioskprint";
    public int PrinterId { get; set; } = 1; // 1=Inbound kiosk, 2=Outbound kiosk
}
