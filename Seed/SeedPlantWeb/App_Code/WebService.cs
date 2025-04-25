using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

/// <summary>
/// Summary description for WebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebService : System.Web.Services.WebService
{

    public WebService()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    private readonly object currentPLC = new object();


    [WebMethod]
   public  PLC.PCResponse SetPLCValues(PLCDataSet CurrentDataset)
    {
        lock(currentPLC)
        {
            PLC.plcDataset = CurrentDataset; 
        }

      
    

        return PLC.pcResponse ;
        
    }

    [WebMethod]
    [ScriptMethod]
    public void PrintTicket(Guid UID, int Copies = 1)
    {
        if (GlobalVars.UsePrinter)
        {
            //   Response.Redirect($"~/PrintTicket?Copies={Copies}&UID={UID}");
            Printing.Print_Ticket(UID, "", 2);
        }
        else
        {
            Printing.Send_TicketToBrowser(UID, false);
        }

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public string[] GetGrowers(string prefix)
    {
        List<string> Names = new List<string>();
        using (ListDataSetTableAdapters.ProducersListTableAdapter producersListTableAdapter = new ListDataSetTableAdapters.ProducersListTableAdapter())
        {
            using (ListDataSet.ProducersListDataTable  producersListDataTable = new ListDataSet.ProducersListDataTable ())
            {
                producersListTableAdapter.Fill(producersListDataTable, prefix);

                foreach (var row in producersListDataTable)
                {


                    Names.Add(row.FullName );
                }

            }
        }
        return Names.ToArray();
    }



    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public bool SetPLCVarieties(List<GlobalVars.BinItems> Varieties)
    {
        GlobalVars.PlcVarieties = Varieties;
        return true;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public bool SetPLCTreatments(List<int> Treatments)
    {
        GlobalVars.PlcTreatments = Treatments;
        return true;
    }



    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public string[] GetVariety(string prefix, bool usePlc)
    {
        List<string> Names = new List<string>();
        using (ListDataSetTableAdapters.SeedVarietiesTableAdapter  seedVarietiesTableAdapter = new ListDataSetTableAdapters.SeedVarietiesTableAdapter())
        {
            using (ListDataSet.SeedVarietiesDataTable  seedVarietiesDataTable = new ListDataSet.SeedVarietiesDataTable())
            {
                if (usePlc)
                {
                    seedVarietiesTableAdapter.FillByPLC(seedVarietiesDataTable,GlobalVars.PlcVarietyCsvList , GlobalVars.Location, prefix);
                }
                else
                {
                    seedVarietiesTableAdapter.FillByFilter(seedVarietiesDataTable, GlobalVars.Location, prefix);
                }
                    

                foreach (var row in seedVarietiesDataTable)
                {
                    Names.Add(row.FullName);
                }

            }
        }
        return Names.ToArray();
    }


    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public string[] GetTreatments(string prefix, bool usePlc)
    {
        List<string> Names = new List<string>();
        using (ListDataSetTableAdapters.SeedChemicalsTableAdapter  seedChemicalsTableAdapter = new ListDataSetTableAdapters.SeedChemicalsTableAdapter())
        {
            using (ListDataSet.SeedChemicalsDataTable  seedChemicalsDataTable = new ListDataSet.SeedChemicalsDataTable())
            {
                if (usePlc)
                {
                    seedChemicalsTableAdapter.FillByPLC(seedChemicalsDataTable, GlobalVars.PlcVarietyCsvList, GlobalVars.Location, prefix);
                }
                else
                {
                    seedChemicalsTableAdapter.FillByFilter(seedChemicalsDataTable, GlobalVars.Location, prefix);
                }


                foreach (var row in seedChemicalsDataTable)
                {
                    Names.Add(row.FullName);
                }

            }
        }
        return Names.ToArray();
    }






    [WebMethod]
    [ScriptMethod(UseHttpGet = false, ResponseFormat = ResponseFormat.Json)]
    public string[] GetWeighmasters(string prefix)
    {
        List<string> Names = new List<string>();
        using (ListDataSetTableAdapters.WeighMastersTableAdapter weighMastersTableAdapter = new ListDataSetTableAdapters.WeighMastersTableAdapter())
            {
            using (ListDataSet.WeighMastersDataTable weighMastersDataTable = new ListDataSet.WeighMastersDataTable())
            {
                weighMastersTableAdapter.FillByFilter(weighMastersDataTable, prefix);
                    
                    foreach(var row in weighMastersDataTable )
                    {

                    
                    Names.Add(row.Description);
                    }
                
            }
        }
        return Names.ToArray();
    }


   
}
