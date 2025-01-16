using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for Scales
/// </summary>
public class Scales
{

    private static LocalDataSet.Weigh_ScalesDataTable weigh_ScalesDataTable;
    private static LocalDataSetTableAdapters.Weigh_ScalesTableAdapter weigh_ScalesTableAdapter = new LocalDataSetTableAdapters.Weigh_ScalesTableAdapter();
    public static DateTime LastScaleModification = DateTime.Now ;

    


    public static bool ScalesInitialized = false;
  

    public static LocalDataSet.Weigh_ScalesDataTable ScalesDataTable
    {
        get
        {
            try
            {
                if(!ScalesInitialized) UpdateScales();
                foreach (LocalDataSet.Weigh_ScalesRow row in weigh_ScalesDataTable)
                {
                    ValidateScaleOK(row);
                }
                
            }
            catch(Exception ex)
            {
                Logging.Add_System_Log("LocalDataSet.Weigh_ScalesDataTable ScalesDataTable", ex.ToString());
               
                
            }
            
            return weigh_ScalesDataTable;
            
        }
        set
        {
            weigh_ScalesDataTable = value;
        }
    }

    public static LocalDataSet.Weigh_ScalesDataTable GetAllScales()
    {
        if (!ScalesInitialized) UpdateScales();
        return weigh_ScalesDataTable; 
    }

    public static LocalDataSet.Weigh_ScalesRow GetScale(string Description , int Location_Id)
    {
        try
        {
            if (!ScalesInitialized) UpdateScales();
            LocalDataSet.Weigh_ScalesRow SelectedRow = null;
            foreach (LocalDataSet.Weigh_ScalesRow row in weigh_ScalesDataTable)
            {
                if (Description.ToUpper() == row.Description.ToUpper() && Location_Id == row.Location_Id)
                {
                    SelectedRow = row;
                    break;
                }
            }
            return SelectedRow;
        }
        catch
        {
            return null;
        }
    }






    public Scales()
    {
    }

    public static bool ValidateScaleOK(LocalDataSet.Weigh_ScalesRow ScaleRow)
    {
        try
        {
            if (ScaleRow.OK)
            {
                ScaleRow.Error_Message = "";
            }
            if ((DateTime.Now - ScaleRow.Last_Update).TotalSeconds > 10)
            {
                ScaleRow.OK = false;
                ScaleRow.Error_Message = "Connection Time Out";
                ScaleRow.Weight = 0;
            }
            return ScaleRow.OK;
        }
        catch
        {
          //  UpdateScales();
            return false;
        }
    }

    public static string StatusString(LocalDataSet.Weigh_ScalesRow ScaleRow)
    {

        if (!Scales.ValidateScaleOK(ScaleRow))
        {
            if (string.IsNullOrEmpty(ScaleRow.Error_Message ))
            {
                return "Scale Error";
            }
            else
            {
                return ScaleRow.Error_Message; 
            }

        }
        else
        {
            if (ScaleRow.Motion )
            {
                return "Motion";
            }
            else
            {
                return "";
            }
        }
    }


    

    public static void UpdateScales()
    {
        try
        {
            if (!ScalesInitialized)
            {
                ScalesInitialized = true;
                weigh_ScalesDataTable = new LocalDataSet.Weigh_ScalesDataTable();
                weigh_ScalesTableAdapter.Fill(weigh_ScalesDataTable);
                foreach (LocalDataSet.Weigh_ScalesRow row in weigh_ScalesDataTable)
                {
                    row.Last_Update = DateTime.Now;
                }
            }
        }
        catch
        {

        }
    }

    

    public static void AddNewScale()
    {
        try
        {
            using (LocalDataSetTableAdapters.QueriesTableAdapter Q = new LocalDataSetTableAdapters.QueriesTableAdapter())
            {
                Q.AddNewScale();

                //UpdateScales();
               
            }
        }
        catch( Exception ex)
        {
            //SiteSetup.LogLocalMessage("Error Adding Scale .. " + ex.Message);
        }
    }
}