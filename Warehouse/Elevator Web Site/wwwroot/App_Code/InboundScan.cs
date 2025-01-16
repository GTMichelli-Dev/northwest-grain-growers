using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for InboundScan
/// </summary>
public class InboundScan
{
    public Guid ScaleUID { get; set; }
    public Guid LoadUID { get; set; }
    public DateTime ScanTime { get; set; }
    public bool PrintingTicket { get; set; }
    public bool Acknowledged { get; set; }

    public InboundScan()
    {
        Reset();
    }

    public void Reset()
    {
        ScaleUID = Guid.Empty;
        LoadUID = Guid.Empty;
        ScanTime = DateTime.Now.AddYears(-10);
        Acknowledged = true;
    }


    

}