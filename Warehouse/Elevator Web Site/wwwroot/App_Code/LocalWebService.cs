using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script;
using System.Web.Script.Services;

/// <summary>
/// Summary description for LocalWebService
/// </summary>
[WebService(Namespace = "http://nwgglocal/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class LocalWebService : System.Web.Services.WebService
{



    public LocalWebService()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }


    [WebMethod,ScriptMethod]
    public KioskResponse InboundTicketScanned(Guid ScaleUID,long LoadID)
    {
        return new KioskResponse() { Success = true, Message1 = "Testing", Message2 = "Testing 2", Message3 = "Testing3" };
    }

    [WebMethod, ScriptMethod]
    public string GetTime()
    {
        return DateTime.Now.ToLongTimeString();
    }

    #region Scales

    [WebMethod, ScriptMethod]
    public LocalDataSet.Weigh_ScalesDataTable GetScales()
    {
        foreach (LocalDataSet.Weigh_ScalesRow RecievedScale in Scales.ScalesDataTable)
        {
            System.Diagnostics.Debug.Print(string.Format("Description{0}  Location_ID{1}  Weight{2}", RecievedScale.Description, RecievedScale.Location_Id, RecievedScale.Weight));
        }

        if (!Scales.ScalesInitialized) Scales.UpdateScales();
        return Scales.ScalesDataTable;

    }





    [WebMethod, ScriptMethod]
    public LocalDataSet.All_Weigh_ScalesDataTable AllScales()
    {
        LocalDataSet.All_Weigh_ScalesDataTable AWTD = new LocalDataSet.All_Weigh_ScalesDataTable();

        LocalDataSetTableAdapters.All_Weigh_ScalesTableAdapter wsta = new LocalDataSetTableAdapters.All_Weigh_ScalesTableAdapter();
        wsta.Fill(AWTD);
        return AWTD;// Scales.ScalesDataTable; 
    }


    [WebMethod, ScriptMethod]
    public DateTime UpdateScales(LocalDataSet.Weigh_ScalesDataTable weigh_ScalesDataTable)
    {
        try
        {
            lock (Scales.ScalesDataTable)
            {

                LocalDataSet.Weigh_ScalesDataTable ScalesDT = Scales.ScalesDataTable;

                foreach (LocalDataSet.Weigh_ScalesRow RecievedScale in weigh_ScalesDataTable)
                {
                    foreach (LocalDataSet.Weigh_ScalesRow ScaleRow in ScalesDT)
                    {
                        lock (ScaleRow)
                        {

                            if ((ScaleRow.Description == RecievedScale.Description) && (ScaleRow.Location_Id == RecievedScale.Location_Id))
                            {
                                ScaleRow.BeginEdit();
                                ScaleRow.Last_Update = DateTime.Now;
                                ScaleRow.Weight = RecievedScale.Weight;
                                ScaleRow.Motion = RecievedScale.Motion;
                                ScaleRow.OK = RecievedScale.OK;
                                if (!RecievedScale.OK)
                                {
                                    ScaleRow.Error_Message = RecievedScale.Error_Message;
                                }
                                else
                                {
                                    ScaleRow.Error_Message = "";
                                }

                                ScaleRow.AcceptChanges();
                            }

                        }
                    }

                }

            }
        }
        catch
        {
            Scales.ScalesInitialized = false;
            Scales.UpdateScales();

        }

        return Scales.LastScaleModification;
    }

    [WebMethod]
    public DateTime RefreshScales()
    {
        Scales.UpdateScales();
        return Scales.LastScaleModification;
    }

    [WebMethod, ScriptMethod]
    public DateTime LastScaleModification()
    {
        return Scales.LastScaleModification;
    }


    #endregion
}
