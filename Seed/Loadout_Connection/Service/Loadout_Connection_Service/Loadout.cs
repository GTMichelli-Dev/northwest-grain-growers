using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WW_LGX_PLC;
using System.Threading;
using System.Diagnostics;


namespace Loadout_Connection_Service
{
    class Loadout : IDisposable
    {

        EventLog eventLog1;

        PLC plc = new PLC();

        public bool Cancel = false;


        /// <summary>
        /// Instance of the Thread class for background updates
        /// </summary>
        Thread updateThread;

    


        public Loadout(EventLog el)
        {
            eventLog1 = el;
          //  Initialize();
        }

        /// <summary>
        /// Initializes connection 
        /// </summary>
        internal void Initialize()
        {
            try
            {

             
            

                updateThread = new Thread(plcUpdate);
                updateThread.Start();

                eventLog1.WriteEntry("Scanning PLC Tags", EventLogEntryType.Information );
            }

            catch (SystemException ex)
            {
                eventLog1.WriteEntry(string.Format("Error Initializing Service. {0}",ex.Message ), EventLogEntryType.Warning);
               
            }

        }


    

        public void plcUpdate()
        {
            // start the background thread for updates
            plc.StartScanning(eventLog1 );
            while  (!Cancel)
            {

             
                try
                {
                    using (WebService.WebService proxy = new WebService.WebService())
                    {
                        using (WebService.PLCDataSet pLCDataSet = new WebService.PLCDataSet())
                        {
                            foreach (PLC.BinRow bin in plc.BinList)
                            {
                                WebService.PLCDataSet.BinsRow row = pLCDataSet.Bins.NewBinsRow();
                                row.Available_For_Clean_Load = bin.Available_For_Clean_Load;
                                row.Available_For_Treat_Load = bin.Available_For_Treat_Load;
                                row.Bin_Id = bin.BinId;
                                row.Bin_Name = bin.BinName;
                                row.Variety_Id = bin.Variety_Id;
                                pLCDataSet.Bins.AddBinsRow(row);
                            }
                            foreach (var treatment in plc.TreatmentList)
                            {
                                WebService.PLCDataSet.TreatmentsRow row = pLCDataSet.Treatments.NewTreatmentsRow();
                                row.Description = treatment.Description;
                                row.Pump_Index = treatment.Id;

                                pLCDataSet.Treatments.AddTreatmentsRow(row);
                            }
                            pLCDataSet.BatchTypeAvailability.AddBatchTypeAvailabilityRow(!plc.TreatScaleUnavailable.Value, !plc.CleanScaleUnavailable.Value);
                            proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
                            WebService.PCResponse pcResponse = proxy.SetPLCValues(pLCDataSet);
                            if ((pcResponse.UID != plc.PcUID) && (pcResponse.UID != Guid.Empty) && (plc.PcUID != Guid.Empty))
                            {
                                plc.PcUID = pcResponse.UID;
                                SetPLCBatch(pcResponse);
                            }
                            plc.PcUID = pcResponse.UID;
                        }
                    }

                    System.Threading.Thread.Sleep(5000);

                }
                catch(Exception ex)
                {
                    eventLog1.WriteEntry(string.Format("Error With PLC Update. {0}", ex.Message), EventLogEntryType.Warning);
                }
            }
          
            plc.Dispose();
        }





        private void SetPLCBatch(WebService.PCResponse pcResponse)
        {

            if (pcResponse.BatchType == WebService.enumBatchType.Bulk)
            {
                PLC.SetBatchState(PLC.enumBatchType.Treat, PLC.enumBatchStatus.SelectBins);
                foreach (var item in pcResponse.BinStatus)
                {
                    PLC.SetBatchBin(PLC.enumBatchType.Treat, item.ItemId, item.Active);
                }


                for (int i = 1; i < 20; i++)
                {
                    if (i == 9 || i == 10)
                    {
                        PLC.SetTreaterPumpActive(i, true);
                    }
                    else
                    {
                        var item = pcResponse.TreatmentStatus.FirstOrDefault(x => x.ItemId == i);
                        if (item != null)
                        {
                            PLC.SetTreaterPumpActive(item.ItemId, item.Active);
                        }
                        else
                        {
                            PLC.SetTreaterPumpActive(i, false);
                        }
                    }
                }







                List<PLC.booleanTagValues> colorValues = new List<PLC.booleanTagValues>();
                for (int I = 0; I < 10; I++)
                {
                    colorValues.Add(new PLC.booleanTagValues(I) { Value = false });
                }
                foreach (var item in pcResponse.ColorStatus)
                {
                    colorValues[item.ItemId].Value = item.Active;
                }
                PLC.SetColors(colorValues);
            }

            else if (pcResponse.BatchType == WebService.enumBatchType.Clean)
            {
                {
                    PLC.SetBatchState(PLC.enumBatchType.Clean, PLC.enumBatchStatus.SelectBins);
                    foreach (var item in pcResponse.BinStatus)
                    {
                        PLC.SetBatchBin(PLC.enumBatchType.Clean, item.ItemId, item.Active);
                    }

                }







            }
        }

      



        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Cancel = true;
                    //readMainPLC.Disconnect();
                    //writeTreaterPLC.Disconnect();
                    //updateThread.Abort();
               


                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Loadout() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
