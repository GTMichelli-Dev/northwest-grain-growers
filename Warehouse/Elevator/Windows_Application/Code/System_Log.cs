using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NWGrain
{
    class System_Log
    {
        public static void Log_Message(string Location,string Message)
        {
            try
            {


                using (NWDatasetTableAdapters.System_LogTableAdapter System_LogTableAdapter = new NWDatasetTableAdapters.System_LogTableAdapter())
                {
                    System_LogTableAdapter.Insert(Guid.NewGuid(),Settings.ServerName , DateTime.Now, Location, Message, Settings.Location_Id);
                }
            }
            catch
            {

            }
        }
    }
}
