using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
/// Summary description for WS
/// </summary>
[WebService(Namespace = "http://NWGGSCALE.NWG/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
 [System.Web.Script.Services.ScriptService]
public class WS : System.Web.Services.WebService {

    public WS () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

  
  

    [WebMethod]
    public string PrintTestTicket(string PrinterToUse)
    {
        return Printing.Print_Test_Ticket(this.Server, PrinterToUse);
        
    }



    [WebMethod]
    public bool  Print_Inbound_Inyard_Ticket(long Load_Id, int LocationID, string Scale = "",string Printer="")
    {
 
        using (LocalDataSetTableAdapters.LoadsTableAdapter LoadsTableAdapter = new LocalDataSetTableAdapters.LoadsTableAdapter())
        {
            using (LocalDataSet.LoadsDataTable Loads = new LocalDataSet.LoadsDataTable())
            {
                if (LoadsTableAdapter.FillByLoad_ID_Location(Loads, Load_Id, LocationID) > 0)
                {
                    
                   
                    Printing.PrintInbound_Inyard_Ticket(this.Server, Loads[0].UID, LocationID, Scale,Printer);
                  
                }
            }
        }
        return true;
    }
    

    [WebMethod]
    public bool Print_Inbound_Final_Ticket(long Load_Id, int LocationID, string Scale = "", string Printer = "")
    {

        using (LocalDataSetTableAdapters.LoadsTableAdapter LoadsTableAdapter = new LocalDataSetTableAdapters.LoadsTableAdapter())
        {
            using (LocalDataSet.LoadsDataTable Loads = new LocalDataSet.LoadsDataTable())
            {
                if (LoadsTableAdapter.FillByLoad_ID_Location(Loads, Load_Id, LocationID) > 0)
                {
           
                    Printing.PrintInbound_Final_Ticket(this.Server, Loads[0].UID , LocationID, Scale,Printer );


                }
            }
        }
        return true;
    }

    [WebMethod]
    public bool Print_Outbound_Inyard_Ticket(long Load_Id,int LocationID, string Scale = "",bool OnlyOne=false, string Printer = "")
    {

        using (LocalDataSetTableAdapters.LoadsTableAdapter LoadsTableAdapter = new LocalDataSetTableAdapters.LoadsTableAdapter())
        {
            using (LocalDataSet.LoadsDataTable Loads = new LocalDataSet.LoadsDataTable())
            {
                if (LoadsTableAdapter.FillByLoad_ID_Location (Loads, Load_Id, LocationID) > 0)
                {
                    Printing.PrintOutbound_InYard_Ticket(this.Server, Loads[0].UID, LocationID,Scale, OnlyOne,Printer);
                    System.Threading.Thread.Sleep(4000);
                }
            }
        }
        return true;
    }



    [WebMethod]
    public bool Print_Outbound_Final_Ticket(long Load_Id,int LocationID, string Scale = "",bool OnlyOne=false, string Printer = "")
    {

        using (LocalDataSetTableAdapters.LoadsTableAdapter LoadsTableAdapter = new LocalDataSetTableAdapters.LoadsTableAdapter())
        {
            using (LocalDataSet.LoadsDataTable Loads = new LocalDataSet.LoadsDataTable())
            {
                if (LoadsTableAdapter.FillByLoad_ID_Location (Loads, Load_Id, LocationID) > 0)
                {

                    Printing.PrintOutbound_Final_Ticket(this.Server, Loads[0].UID, LocationID, Scale,OnlyOne,Printer);
                    System.Threading.Thread.Sleep(4000);
                }
            }
        }
        return true;
    }


    [WebMethod]
    public bool Print_Transfer_Inyard_Ticket(long Load_Id,int Location_Id, string Scale = "",string Printer="")
    {

        using (LocalDataSetTableAdapters.LoadsTableAdapter LoadsTableAdapter = new LocalDataSetTableAdapters.LoadsTableAdapter())
        {
            using (LocalDataSet.LoadsDataTable Loads = new LocalDataSet.LoadsDataTable())
            {
                if (LoadsTableAdapter.FillByLoad_ID_Location(Loads, Load_Id,Location_Id) > 0)
                {
                    Printing.PrintTransfer_InYard_Ticket(this.Server, Loads[0].UID,Location_Id , Scale,Printer );
                    System.Threading.Thread.Sleep(4000);
                }
            }
        }
        return true;
    }

    [WebMethod]
    public bool Print_Transfer_Final_Ticket(long Load_Id,int LocationID, string Scale = "", string Printer = "")
    {

        using (LocalDataSetTableAdapters.LoadsTableAdapter LoadsTableAdapter = new LocalDataSetTableAdapters.LoadsTableAdapter())
        {
            using (LocalDataSet.LoadsDataTable Loads = new LocalDataSet.LoadsDataTable())
            {
                if (LoadsTableAdapter.FillByLoad_ID_Location (Loads, Load_Id, LocationID) > 0)
                {
                    Printing.PrintTransfer_Final_Ticket(this.Server, Loads[0].UID, LocationID, Scale,Printer);
                    System.Threading.Thread.Sleep(4000);
                }
            }
        }
        return true;
    }



       [WebMethod]
    public LocalDataSet.Site_PrintersDataTable GetPrinterList()
    {

        string ServerName = "";
        Printing.UpdatePrinters();
        using (LocalDataSetTableAdapters.QueriesTableAdapter Q = new LocalDataSetTableAdapters.QueriesTableAdapter())
        {
            ServerName = Q.ServerName();
        }
        
            using (LocalDataSetTableAdapters.Site_PrintersTableAdapter printersTableAdapter = new LocalDataSetTableAdapters.Site_PrintersTableAdapter())
            {
                using (LocalDataSet.Site_PrintersDataTable SitePrinters = new global::LocalDataSet.Site_PrintersDataTable())
                {
                    printersTableAdapter.Fill(SitePrinters, ServerName);
                    return SitePrinters;
            }
            }
        
        
    }

    
}
