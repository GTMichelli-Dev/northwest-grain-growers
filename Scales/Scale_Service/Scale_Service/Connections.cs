using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMA_Command_LIB;
using StreamingScale_Lib;
using System.Diagnostics;
using System.Data;
using System.Threading;

namespace Scale_Service
{
    public delegate void ConnectionError(object source, EventArgs e);

    public class Connections : IDisposable
    {

       

        LocalWebService.LocalDataSet.Weigh_ScalesDataTable weigh_ScalesDataTable = new LocalWebService.LocalDataSet.Weigh_ScalesDataTable();

        List<SMA> SMA_Scales = new List<SMA>();
        List<StreamingScale> Stream_Scales = new List<StreamingScale>();

        EventLog Eventlog;

        DateTime LastUpdate;
        bool Initialized = false;
        bool Cancel = false;

        public Connections(EventLog ev)
        {
            Eventlog = ev;
            GetScales();
        }


        public Connections()
        {
            GetScales();
        }

        private void LogError(string Message, EventLogEntryType EventLogType = EventLogEntryType.Information)
        {
            try
            {
                if (Eventlog != null) Eventlog.WriteEntry(Message, EventLogType);

            }
            catch
            {

            }
        }


        /// <summary>
        /// Update the scale database with the current scales
        /// </summary>
        public void GetScales()
        {

            do
            {

          
                if (!Initialized )
                {
                    try
                    {
                        

                        {
                            using (LocalWebService.LocalWebService Proxy = new LocalWebService.LocalWebService())
                            {
                                weigh_ScalesDataTable = Proxy.GetScales();
                            }


                            foreach (LocalWebService.LocalDataSet.Weigh_ScalesRow row in weigh_ScalesDataTable)
                            {
                                if (row.Scale_Type == "SMA")
                                {
                                    Add_SMA_Scale(row.Description, row.IP_Address, row.Port, 700, row.Location_Id);
                                }
                                else if (row.Scale_Type == "IDS")
                                {
                                    Add_Streaming_Scale(row.Description, row.IP_Address, row.Port, 700, row.Location_Id);
                                }
                            }
                        }
                        Initialized = true;
                      
                    }
                    catch (Exception ex)
                    {
                        LogError("@GetScales: " + ex.Message, EventLogEntryType.Error);
                      
                    }
                }
                try
                {
                    UpdateWebSite();
                }
                catch (Exception ex)
                {
                    LogError("@GetScales.UpdateWebSite: " + ex.Message, EventLogEntryType.Error);
                  
                }
                System.Threading.Thread.Sleep(800);
            } while (Cancel == false);


        }








        public bool UpdateWebSite()
        {
            bool Retval = true;
            try
            {
                #region Get Streaming Scale
                try
                {
                    foreach (StreamingScale scale in Stream_Scales)
                    {
                        LocalWebService.LocalDataSet.Weigh_ScalesRow row = null;
                        foreach (LocalWebService.LocalDataSet.Weigh_ScalesRow Scalerow in weigh_ScalesDataTable)
                        {
                            if (Scalerow.Description == scale.Description && Scalerow.Location_Id == scale.Location_Id)
                            {
                                row = Scalerow;
                                break;
                            }
                        }

                        if (row != null)
                        {
                            if (scale.ConnectionStatus != StreamingScale.enumConnectionStatus.Ok)
                            {
                                row.Error_Message = scale.ConnectionStatusString;
                                row.OK = false;
                                row.Weight = 0;
                                row.Motion = false;
                            }
                            else
                            {
                                if (scale.CurrentScaleData.CurrentStatus != StreamingScale.ScaleData.enumStatus.OK)
                                {
                                    row.Error_Message = scale.CurrentScaleData.CurrentStatus.ToString();
                                    row.OK = false;
                                    row.Weight = 0;
                                    row.Motion = false;

                                }
                                else
                                {
                                    row.Error_Message = "";
                                    row.OK = true;
                                    row.Weight = scale.CurrentScaleData.CurWeight;
                                    row.Motion = scale.CurrentScaleData.Motion;

                                }
                            }

                        }
                    }

                }
                catch (Exception ex_streamingScale)
                {
                    LogError(ex_streamingScale.Message);

                }
                #endregion

                #region Get SMA Scale
                try
                {
                    foreach (SMA scale in SMA_Scales)
                    {
                        LocalWebService.LocalDataSet.Weigh_ScalesRow row = null;
                        foreach (LocalWebService.LocalDataSet.Weigh_ScalesRow Scalerow in weigh_ScalesDataTable)
                        {
                            if (Scalerow.Description == scale.Description && Scalerow.Location_Id == scale.Location_Id)
                            {
                                row = Scalerow;
                                break;
                            }
                        }
                        if (row != null)
                        {
                            if (scale.ConnectionStatus != SMA.enumConnectionStatus.Ok)
                            {
                                row.Error_Message = scale.ConnectionStatusString;
                                row.OK = false;
                                row.Weight = 0;
                                row.Motion = false;
                            }
                            else
                            {
                                if (scale.CurrentScaleData.CurrentStatus != SMA.ScaleData.enumStatus.OK)
                                {
                                    row.Error_Message = scale.CurrentScaleData.CurrentStatus.ToString();
                                    row.OK = false;
                                    row.Weight = 0;
                                    row.Motion = false;

                                }
                                else
                                {
                                    row.Error_Message = "";
                                    row.OK = true;
                                    row.Weight = scale.CurrentScaleData.CurWeight;
                                    row.Motion = scale.CurrentScaleData.Motion;

                                }
                            }

                        }
                    }
                }
                catch (Exception ex_SMAScale)
                {
                    LogError(ex_SMAScale.Message);
                }

                #endregion

                #region Update Web Service
                using (LocalWebService.LocalWebService Proxy = new LocalWebService.LocalWebService())
                {

                    DateTime SysModDate = Proxy.UpdateScales(weigh_ScalesDataTable);
                    if (LastUpdate != SysModDate) Initialized = false;
                    LastUpdate = SysModDate;
                }
                #endregion
            }
            catch (Exception ex)
            {
                LogError("@UpdateWebSite: " + ex.Message, EventLogEntryType.Error);
                Retval = false;


            }
            return Retval;


        }

        private void TmrUpdate_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

        }

        private void Add_SMA_Scale(string Description, string Address, int Port, int Interval, int Location_Id)
        {
            bool Found = false;
            foreach(SMA ExistingScale in SMA_Scales )
            {
                if (ExistingScale.Description.ToUpper()==Description.ToUpper())
                {
                    Found = true;
                    break;
                }
            }
            if (!Found)
            {
                SMA Scale = new SMA_Command_LIB.SMA(Description, Address, Port, Location_Id);
                LogError("Adding SMA Scale " + Description + " at " + Address + " Port:" + Port.ToString());
                Scale.Connect(Interval);
                SMA_Scales.Add(Scale);
            }
        }


        private void Add_Streaming_Scale(string Description, string Address, int Port, int Interval, int Location_Id)
        {
            bool Found = false;
            foreach (StreamingScale ExistingScale in Stream_Scales)
            {
                if (ExistingScale.Description.ToUpper() == Description.ToUpper())
                {
                    Found = true;
                    break;
                }
            }
            if (!Found)
            {
                StreamingScale Scale = new StreamingScale_Lib.StreamingScale(Description, Address, Port, Location_Id);
                LogError("Adding Streaming Scale " + Description + " at " + Address + " Port:" + Port.ToString());
                Scale.Connect(Interval);
                Stream_Scales.Add(Scale);
            }
        }


        /// <summary>
        /// Removes All Scales From The System
        /// </summary>
        private void Remove_Scales()
        {
            //Clear Out Old Scale Information
            foreach (StreamingScale_Lib.StreamingScale scale in Stream_Scales)
            {
                try
                {
                    scale.Disconnect();
                    scale.Dispose();
                }
                catch { }
            }
            try
            {
                Stream_Scales.Clear();
            }
            catch { }

            foreach (SMA scale in SMA_Scales)
            {
                try
                {
                    scale.Disconnect();
                    scale.Dispose();
                }
                catch { }
            }
            try
            {
                SMA_Scales.Clear();
            }
            catch { }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    LogError("Disposing Scale Connection", EventLogEntryType.Warning);
                    Cancel = true;

                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Connections() {
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
