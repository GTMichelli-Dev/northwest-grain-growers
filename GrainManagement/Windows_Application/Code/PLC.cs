using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace NWGrain
{
    class PLC
    {
        
        public static PLC_ConnectionService.PLC_ConnectionSoapClient PLCConnection = new PLC_ConnectionService.PLC_ConnectionSoapClient();
        public static PLC_ConnectionService.PLCDataSet.BinsDataTable BinsDataTable = new PLC_ConnectionService.PLCDataSet.BinsDataTable();
        public static PLC_ConnectionService.PLCDataSet.LoadingStatusDataTable LoadingStatusTable = new PLC_ConnectionService.PLCDataSet.LoadingStatusDataTable();
        public static PLC_ConnectionService.PLCDataSet.Treat_PumpsDataTable TreaterTable = new PLC_ConnectionService.PLCDataSet.Treat_PumpsDataTable();
        static Thread thrPLCData = new Thread(new ThreadStart(Get_PLC_Data));
        static bool Cancel = false;


        public static bool TreatUnavailable
        {
            get
            {
                return LoadingStatusTable[0].Treat_Unavailable; 
            }
        }


        public static bool CleanUnavailable
        {
            get
            {
                return LoadingStatusTable[0].Clean_Unavailable;
            }
        }


        public static void SyncData()
        {

            if (!thrPLCData.IsAlive)
            {
                Cancel = false;
                thrPLCData.Start();
                thrPLCData.IsBackground = true;
            }

        }

        public static void Disconnect()
        {
            if (thrPLCData.IsAlive )
            {
                Cancel = true;
                thrPLCData.Join();
               
            }
        }

        public static void Get_PLC_Data()
        {
            try
            {
                while (!Cancel)
                {
                    BinsDataTable = PLCConnection.GetBinStatus();
                    LoadingStatusTable = PLCConnection.GetLoadingStatus();
                    TreaterTable = PLCConnection.GetTreaterPumps();
                    System.Threading.Thread.Sleep(2000);
                }
            }
            catch
            {

            }

        }
    }
}
