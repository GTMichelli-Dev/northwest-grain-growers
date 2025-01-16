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



namespace Agvantage_Transfer
{
    public partial class AgvantageTransferService : ServiceBase
    {
     


        AgvantageTransfer AgvTransfer;

        //public bool ControllersSet = false;

        public AgvantageTransferService()
        {
            InitializeComponent();
 
            if (!System.Diagnostics.EventLog.SourceExists("Agvantage Transfer Service"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "Agvantage Transfer Service", "Agvantage Transfer Service");
            }
            eventLog1.Source = "Agvantage Transfer Service";
            eventLog1.Log = string.Empty ;


            AgvTransfer = new AgvantageTransfer(eventLog1);

        }


        public void StartConnection()
        {
            
        }


        protected override void OnStop()
        {
            try
            {

                AgvTransfer.Dispose();
                eventLog1.WriteEntry("Agvantage Transfer Service - Stopped", EventLogEntryType.Information, 2);
            }
            catch(Exception ex)
            {
                eventLog1.WriteEntry("Agvantage Transfer Service - OnStop Error:" + ex.ToString(), EventLogEntryType.Error ,1369 );
            }
            base.OnStop();
        }

        protected override void OnStart(string[] args)
        {

            eventLog1.WriteEntry("Agvantage Transfer Service - Starting Service thread", EventLogEntryType.Information ,1);
            try
            {
                AgvTransfer.StartTransfer(Properties.Settings.Default.BatchFile ,Properties.Settings.Default.CompletedFilePath , Properties.Settings.Default.TimeoutSeconds ,Properties.Settings.Default.UpdateIntervalMinutes );
            }
            catch (Exception ex)
            {
                eventLog1.WriteEntry("Agvantage Transfer Service - OnStart Error:" + ex.ToString(), EventLogEntryType.Error);
            }
        }

    }
}
