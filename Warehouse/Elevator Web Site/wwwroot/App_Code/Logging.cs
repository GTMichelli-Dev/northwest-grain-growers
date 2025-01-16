using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;


   public class Logging
    {
        /// <summary>
        /// Save a system log record
        /// </summary>
        /// <param name="Location">Where in the code it happened</param>
        /// <param name="Message">What happened</param>
        public static void Add_System_Log(string Location, string Message, int Location_Id)
        {
            try
            {
                using (LocalDataSetTableAdapters.System_LogTableAdapter System_LogTableAdapter = new LocalDataSetTableAdapters.System_LogTableAdapter()  )
                {
                    System_LogTableAdapter.Insert(Location, Message, Location_Id );
                }                    

            }
            catch
            {

            }

        }


    public static void Add_System_Log(string Location, string Message)
    {
        Add_System_Log(Location, Message, 0);
    }

    /// <summary>
    /// Save a audit trail record
    /// </summary>
    /// <param name="Description">What they are auditing</param>
    /// <param name="OldValue">The original value</param>
    /// <param name="NewValue">The new value</param>
    /// <param name="Reason">Why they did it</param>
    //public static void Add_Audit_Trail(string Description, string OldValue, string NewValue, string Reason,string TypeOfAudit="WEB",string RecordID="")
    //{
    //    try
    //    {
    //        string UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
    //        using (NWDatasetTableAdapters.Site_SetupTableAdapter Site_SetupTableAdapter= new NWDatasetTableAdapters.Site_SetupTableAdapter())
    //        {
    //            using (NWDataset.Site_SetupDataTable Site_SetupDataTable= new NWDataset.Site_SetupDataTable())
    //            {
    //                Site_SetupTableAdapter.Fill(Site_SetupDataTable);
    //                using (NWDatasetTableAdapters.Audit_TrailTableAdapter Audit_TrailTableAdapter = new NWDatasetTableAdapters.Audit_TrailTableAdapter())
    //                {
    //                    Audit_TrailTableAdapter.Insert(Guid.NewGuid(),UserName, Site_SetupDataTable[0].Location_Id, DateTime.Now,TypeOfAudit , Description, OldValue, NewValue, Reason);
    //                }
    //            }
    //         }

    //     }
    //    catch
    //    {

    //    }
    //}

}


