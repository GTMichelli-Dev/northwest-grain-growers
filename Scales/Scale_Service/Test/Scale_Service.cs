using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Net.Sockets;
using System.Threading;



namespace Scale_Service
{
    public partial class Scale_Service : ServiceBase
    {
        System.Threading.Thread InitializeConnection;
     

        Connections scaleConnection;

        //public bool ControllersSet = false;

        public Scale_Service()
        {
            InitializeComponent();
 
            if (!System.Diagnostics.EventLog.SourceExists("Scale Service"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "Scale Service", "Scale Service Diagnostics");
            }
            eventLog1.Source = "Scale Service";
            eventLog1.Log = "Scale Service Diagnostics";
            
         
           
        }


        public void StartConnection()
        {
             scaleConnection = new Connections(eventLog1);
        }


        protected override void OnStop()
        {
            try
            {
                
                scaleConnection.Dispose();
                eventLog1.WriteEntry("Scale Service - Stopped", EventLogEntryType.Information, 2);
            }
            catch(Exception ex)
            {
                eventLog1.WriteEntry("Scale Service - OnStop Error:"+ex.ToString(), EventLogEntryType.Error ,1369 );
            }
            base.OnStop();
        }

        protected override void OnStart(string[] args)
        {

            eventLog1.WriteEntry("Starting Scale Service - Starting Service thread",EventLogEntryType.Information ,1);
            
            InitializeConnection = new Thread(() => StartConnection());
            InitializeConnection.Start();
        }

    }
}
