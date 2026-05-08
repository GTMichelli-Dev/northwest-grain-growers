using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;

namespace NWGrain
{
    class Logging
    {
        /// <summary>
        /// Save a system log record
        /// </summary>
        /// <param name="Location">Where in the code it happened</param>
        /// <param name="Message">What happened</param>
        public static void Add_System_Log(string Location, string Message)
        {
            try
            {
                using (NWDatasetTableAdapters.System_LogTableAdapter System_LogTableAdapter = new NWDatasetTableAdapters.System_LogTableAdapter())
                {
                    System_LogTableAdapter.Insert(Guid.NewGuid(),Settings.ServerName , DateTime.Now, Location, Message,Settings.Location_Id );
                }                    

            }
            catch
            {

            }

        }

        /// <summary>
        /// Save a audit trail record
        /// </summary>
        /// <param name="Description">What they are auditing</param>
        /// <param name="OldValue">The original value</param>
        /// <param name="NewValue">The new value</param>
        /// <param name="Reason">Why they did it</param>
        public static void Add_Audit_Trail(string Type_Of_Audit,string Record_Id,string Description, string OldValue, string NewValue, string Reason)
        {
            try
            {
                string UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                using (NWDatasetTableAdapters.Site_SetupTableAdapter Site_SetupTableAdapter= new NWDatasetTableAdapters.Site_SetupTableAdapter())
                {
                    using (NWDataset.Site_SetupDataTable Site_SetupDataTable= new NWDataset.Site_SetupDataTable())
                    {
                        Site_SetupTableAdapter.Fill(Site_SetupDataTable, Settings.Location_Id);
                        using (NWDatasetTableAdapters.Audit_TrailTableAdapter Audit_TrailTableAdapter = new NWDatasetTableAdapters.Audit_TrailTableAdapter())
                        {
                            Audit_TrailTableAdapter.Insert(Guid.NewGuid(),UserName,Settings.Location_Id,Settings.ServerName,Type_Of_Audit,Record_Id, DateTime.Now, Description, OldValue, NewValue, Reason);
                        }
                    }
                 }

             }
            catch
            {

            }
        }

    }


}
