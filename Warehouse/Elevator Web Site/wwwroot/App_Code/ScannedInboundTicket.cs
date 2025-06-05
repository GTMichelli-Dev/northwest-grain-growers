using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ScannedInboundTicket
/// </summary>
public class ScannedInboundTicket
{
    public bool Valid { get; set; }
    public bool TransferLoad { get; set; }
    public int Sequence { get; set; }
    public long LoadID { get; set; }
    public int LocationId { get; set; }
    public ScannedInboundTicket(string TicketData)
    {
        Valid = false;
        try
        {

            if (TicketData.Contains("R") || TicketData.Contains("T"))
            {
                TransferLoad = (TicketData.Contains("T"));

                LoadID = Convert.ToInt64(TicketData.Substring(1));
                LocationId = Convert.ToInt32(TicketData.Substring(2, 3));
                Sequence = Convert.ToInt32(TicketData.Substring(1, 1));
                Valid = true;
            }

        }
        catch
        {

        }

    }


}