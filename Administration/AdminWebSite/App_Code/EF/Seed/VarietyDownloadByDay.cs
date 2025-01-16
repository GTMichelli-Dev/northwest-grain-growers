using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for VarietyDownloadByDay
/// </summary>
public class VarietyDownloadByDay
{
    public DateTime? Ticket_Date { get; set; }
    public int Location_ID { get; set; }
    public string Location { get; set; }
    public string Commodity { get; set; }
    public int? Loads { get; set; }
    
    public double? Clean { get; set; }
    public double? Treated { get; set; }
    public string Variety { get; set; }
    public int VarietyID { get; set; }
    public string CommodityDetails { get; set; }
    

}
