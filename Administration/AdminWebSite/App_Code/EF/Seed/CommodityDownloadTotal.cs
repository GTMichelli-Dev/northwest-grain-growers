using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CommodityDownloadByLoad
/// </summary>
public class CommodityDownloadTotal
{
    public int? Loads { get; set; }
    public double? Clean { get; set; }
    public double? Treated { get; set; }
    public string Commodity { get; set; }
    public int Location_ID { get; set; }
    public string Location{ get; set; }
}
