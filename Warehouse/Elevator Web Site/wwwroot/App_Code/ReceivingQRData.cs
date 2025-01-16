using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for InboundQRData
/// </summary>
public class ReceivingQRData
{

    public Guid LoadUID { get; set; }
    public long LoadID { get; set; }
    public int LocationID { get; set; }
    public DateTime TimeOut { get; set; }
    //public DateTime TimeIn { get; set; }
    //public int WeightIn { get; set; }
    //public int WeightOut { get; set; }
    //public string TruckID  { get; set; }
    public string Header
    {
        get
        {
            return "INBOUND";
        }
    }


    public ReceivingQRData()
    {

    }
}