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

        bool Cancel = false;
        
        List< StreamingScale_Lib.StreamingScale> Scales = new List<StreamingScale_Lib.StreamingScale>();
        public Truck_Scale_Connection.Scale_Connection proxy = new Truck_Scale_Connection.Scale_Connection ();


        public bool ControllersSet = false;

        public Scale_Service()
        {
            InitializeComponent();
            bwGetWeight.DoWork += BwGetWeight_DoWork;
            bwGetWeight.RunWorkerCompleted += BwGetWeight_RunWorkerCompleted;
 
            if (!System.Diagnostics.EventLog.SourceExists("Scale Service"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "Scale Service", "Scale Service Diagnostics");
            }
            eventLog1.Source = "Scale Service";
            eventLog1.Log = "Scale Service Diagnostics";
        }


        protected override void OnStop()
        {
            try
            {
                Cancel = true;
                bwGetWeight.CancelAsync();
             
            }
            catch
            {

            }
            base.OnStop();
        }

        protected override void OnStart(string[] args)
        {

            eventLog1.WriteEntry("Starting Scale Service - Starting Background Worker");
            GetScales();
            bwGetWeight.RunWorkerAsync();
        }



        private void BwGetWeight_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!Cancel)
            {
                GetScales();
            
                System.Threading.Thread.Sleep(1000);
                bwGetWeight.RunWorkerAsync();
            }
        }


        private void GetScales()
        {
            if (Scales.Count  == 0)
            {
                eventLog1.WriteEntry("Connecting To Scale 1 ");
                Scales.Add(new StreamingScale_Lib.StreamingScale("Scale 1", Properties.Settings.Default.Scale_1_Address, Properties.Settings.Default.Scale1_Port));

                if (!string.IsNullOrEmpty(Properties.Settings.Default.Scale_2_Address))
                {
                    eventLog1.WriteEntry("Connecting To Scale 2 ");
                    Scales.Add(new StreamingScale_Lib.StreamingScale("Scale 2", Properties.Settings.Default.Scale_2_Address, Properties.Settings.Default.Scale2_Port));
                }
            }
        }



        private void BwGetWeight_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {

                {
                    using (Truck_Scale_Connection.ScaleDataSet.ScalesDataTable CurrentScales = new Truck_Scale_Connection.ScaleDataSet.ScalesDataTable())
                    {
                        foreach (StreamingScale_Lib.StreamingScale Scale in Scales)
                        {
                            Truck_Scale_Connection.ScaleDataSet.ScalesRow row = CurrentScales.NewScalesRow();
                            row.CurrentWeight = Scale.CurrentScaleData.CurWeight;
                            row.Motion = Scale.CurrentScaleData.Motion;
                            row.ScaleName = Scale.Description;
                            row.Status = Scale.ConnectionStatusString;
                            row.Valid = Scale.CurrentScaleData.CurrentStatus == StreamingScale_Lib.StreamingScale.ScaleData.enumStatus.OK;
                           
                            CurrentScales.AddScalesRow(row);


                        }
                        proxy.SetScaleData(CurrentScales);
                    }
                }
            }
            catch (Exception ex)
            {
                eventLog1.WriteEntry("OnTimer Error " + ex.Message, EventLogEntryType.Error);
                
                foreach (StreamingScale_Lib.StreamingScale Scale in Scales)
                {
                    try
                    {
                        eventLog1.WriteEntry(string.Format("Closing Connection To {0} ",Scale.Description ));
                        Scale.Disconnect();
                        Scale.Dispose();

                    }
                    catch (Exception scl)
                    {
                        eventLog1.WriteEntry(string.Format("Error Closing Connection To {0} ", Scale.Description)+" "+scl.Message );

                    }

                }
                Scales.Clear();
                System.Threading.Thread.Sleep(1000);
            }
        }


       

   
    }
}
