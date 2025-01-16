using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Site
/// </summary>
public class SiteSetup
{
    private static LocalDataSet localDataSet;
    private static LocalDataSet.Site_SetupRow site_SetupRow;



    public SiteSetup()
    {
      
    }


    public static LocalDataSet.Site_SetupRow Site_SetupRow
    {
        get
        {

            if (site_SetupRow == null)
            {
                //using (LocalDataSetTableAdapters.Site_SetupTableAdapter site_SetupTableAdapter = new LocalDataSetTableAdapters.Site_SetupTableAdapter())
                //{
                //    if (localDataSet == null) localDataSet = new LocalDataSet();
                //    if (site_SetupTableAdapter.Fill(localDataSet.Site_Setup) == 0)
                //    {
                //        site_SetupRow = localDataSet.Site_Setup.NewSite_SetupRow();
                //        site_SetupRow.Default_Inbound_Printer = string.Empty;
                //        site_SetupRow.Default_Outbound_Printer = string.Empty;
                //        site_SetupRow.Has_Loadout = false;
                //        site_SetupRow.Load_Out_Ip = "0.0.0.0";
                //        site_SetupRow.Load_Out_Port = 10001;
                //        site_SetupRow.Outbound_Final_Kiosk_Ticket_Count = 1;

                //    }
                //    else
                //    {
                //        site_SetupRow = localDataSet.Site_Setup[0];
                //    }
                //}
            }

            return site_SetupRow;
        }

    }


    /// <summary>
    /// Update the Site Setup Row
    /// </summary>
    public static void UpdateSite()
    {
        try
        {

            if (site_SetupRow != null)
            {
                using (LocalDataSetTableAdapters.Site_SetupTableAdapter site_SetupTableAdapter = new LocalDataSetTableAdapters.Site_SetupTableAdapter())
                {
                    if (localDataSet.Site_Setup.Count == 0)
                    {
                        localDataSet.Site_Setup.AddSite_SetupRow(site_SetupRow);
                    }
                    site_SetupTableAdapter.Update(localDataSet);
                    localDataSet.Site_Setup.AcceptChanges();
                    site_SetupTableAdapter.Fill(localDataSet.Site_Setup);
                    site_SetupRow = localDataSet.Site_Setup[0];
                }
            }
            else
            {
                
               //LogLocalMessage("Cannot Update Site Setup .. Site_SetupRow = Null");
            }
        }
        catch (Exception ex)
        {
            //LogLocalMessage("Error Updating Site "+ ex.Message );
        }
    }



  

}