
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace Loadout_Connection_Service
{
    public partial class Loadout_Service : ServiceBase
    {
        Loadout loadOut;


        public Loadout_Service()
        {
            InitializeComponent();

            if (!System.Diagnostics.EventLog.SourceExists("Loadout Connection Service"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "Loadout Connection Service", "Loadout Connection Service Diagnostics");
            }
            eventLog1.Source = "Loadout Connection Service";
            eventLog1.Log = "Loadout Connection Service Diagnostics";
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                eventLog1.WriteEntry("Starting Loadout Connection Service 1");
                loadOut = new Loadout(eventLog1);
                loadOut.Initialize();
            }
            catch (Exception ex)
            {
                eventLog1.WriteEntry(string.Format("Error Starting {0}",ex.Message ));
            }
        }

        protected override void OnStop()
        {
            try
            {
                loadOut.Dispose();
            }
            catch
            {

            }
            eventLog1.WriteEntry("Stopping Loadout Service", EventLogEntryType.Warning);

        }
    }
}
