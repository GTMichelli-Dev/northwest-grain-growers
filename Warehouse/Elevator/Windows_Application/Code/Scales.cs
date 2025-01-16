using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
namespace NWGrain
{
    public class Scales
    {
        public static LocalWebService.LocalWebServiceSoapClient Proxy = new LocalWebService.LocalWebServiceSoapClient();

        public static string CurrentlySelectedScale;

      
        public const string Manual = "Manual";

        public static  Scales.ScaleData CurrentInboundScaleData = ManualScale;
        public static  Scales.ScaleData CurrentOutboundScaleData = ManualScale;


        public static  ScaleData ManualScale
        {
            get
            {
                ScaleData retval = new ScaleData(Scales.Manual);
                retval.ConnectionStatus = ScaleData.enumConnectionStatus.Manual;
                retval.CurrentStatus = ScaleData.enumStatus.OK;
                return retval;
            }
        }

        public static List<ScaleData> CurScales = new List<ScaleData>();


        public static void SetScale(ref System.Windows.Forms.ComboBox cboScale)
        {
            cboScale.Items.Clear();
            GetScaleByDescription(Manual).CurWeight = 0;
            foreach (Scales.ScaleData Scale in Scales.CurScales)
            {
                cboScale.Items.Add(Scale.Description);
            }
            int Index = cboScale.FindString(Settings.workStation_SetupRow.Weigh_Scale);

            if (Index == -1)
            {
                Index = cboScale.FindString(Scales.Manual);
            }
            cboScale.SelectedIndex = Index;
        }


        public static void SetScaleNoManual(ref System.Windows.Forms.ComboBox cboScale)
        {
            cboScale.Items.Clear();
            //GetScaleByDescription(Manual).CurWeight = 0;
            foreach (Scales.ScaleData Scale in Scales.CurScales)
            {
                if (Scale.Description != Scales.Manual)
                {
                    cboScale.Items.Add(Scale.Description);
                }
            }
            int Index = 0;// cboScale.FindString(Settings.workStation_SetupRow.Weigh_Scale);

            try
            {
                cboScale.SelectedIndex = Index;
            }
            catch { 
            
            }
        }


        public static void UpdateScaleValues(ScaleData ScaleValuesToUse, ScaleData ScaleValuesToUpdate)
        {
            ScaleValuesToUpdate.CurrentStatus = ScaleValuesToUse.CurrentStatus;
            ScaleValuesToUpdate.CurWeight = ScaleValuesToUse.CurWeight;
            ScaleValuesToUpdate.Motion = ScaleValuesToUse.Motion;  
        }


        public static ScaleData GetScaleByDescription(string Description)
        {
            ScaleData RetScale = null;
            foreach (ScaleData scale in CurScales )
            {
                if (scale.Description==Description )
                {
                    if (scale.Description != "Manual" && scale.ConnectionStatus != ScaleData.enumConnectionStatus.Off)
                    {
                        scale.ConnectionStatus = ScaleData.enumConnectionStatus.Connected;
                    }
                    else if (scale.Description == "Manual" && scale.ConnectionStatus != ScaleData.enumConnectionStatus.Off)
                    {
                        scale.ConnectionStatus = ScaleData.enumConnectionStatus.Manual;
                    }

                    RetScale = scale;
                    //RetScale.ConnectionStatus = scale.ConnectionStatus; 
                    break; 
                }
            }
            System.Diagnostics.Debug.WriteLine(Description);
            return RetScale;
        }

        public static bool Cancel =false;

        public static void Connect()
        {
            Thread ScaleUpdater = new Thread(new ThreadStart(GetScaleData));
            ScaleUpdater.Start();
        }

        public static void GetScaleData()
        {
            CurScales.Clear();
            using (ListsDataSet.Current_ScalesDataTable CurrentScaleList = new ListsDataSet.Current_ScalesDataTable())
            {
                using (ListsDataSetTableAdapters.Current_ScalesTableAdapter current_ScalesTableAdapter = new ListsDataSetTableAdapters.Current_ScalesTableAdapter())
                {
                    current_ScalesTableAdapter.Fill(CurrentScaleList);
                    foreach(ListsDataSet.Current_ScalesRow row in CurrentScaleList )
                    {
                        ScaleData Scale = new NWGrain.Scales.ScaleData(row.Description);
                        CurScales.Add(Scale);
                    }

                    CurScales.Add(ManualScale);
                    string DefaultScale = Settings.workStation_SetupRow.Weigh_Scale ;
                    
                    {
                        bool ValidName = false;
                        foreach (ScaleData Scale in CurScales)
                        {
                            if (DefaultScale==Scale.Description )
                            {
                                ValidName = true;
                                break;
                            }
                        }
                        if (!ValidName)
                        {
                            Settings.workStation_SetupRow.Weigh_Scale = Scales.Manual;
                            Settings.SaveWorkStationSettings();
                        }

                    }
                    do
                        {
                        try
                        {
                            LocalWebService.LocalDataSet.Weigh_ScalesDataTable Weigh_Scales = Proxy.GetScales();

                            foreach (LocalWebService.LocalDataSet.Weigh_ScalesRow row in Weigh_Scales)
                            {
                               
                                foreach (ScaleData Scale in CurScales )
                                {
                                    
                                    if (Scale.Description== Scales.Manual)
                                    {
                                        Scale.Manual = true;
                                        Scale.ConnectionStatus = ScaleData.enumConnectionStatus.Connected; 
                                        Scale.CurrentStatus = ScaleData.enumStatus.OK;
                                        Scale.InboundPrinter = "";
                                        Scale.OutboundPrinter = "";
                                        Scale.PrintInbound = false;
                                        Scale.PrintOutbound = false;

                                    }
                                    else if (Scale.Description==row.Description )
                                    {
                                        Scale.Manual = false;
                                        Scale.Motion =  row.Motion;
                                        Scale.CurWeight = row.Weight;
                                        Scale.InboundPrinter = row.Inbound_Ticket_Printer;
                                        Scale.OutboundPrinter = row.Outbound_Ticket_Printer;

                                        
                                        Scale.PrintInbound = row.Print_Inbound_Ticket;
                                        Scale.PrintOutbound = row.Print_Outbound_Ticket;

                                        if (row.OK )
                                        {
                                            Scale.CurrentStatus = ScaleData.enumStatus.OK; 
                                        }
                                        else
                                        {
                                            Scale.CurrentStatus = ScaleData.enumStatus.Fail; 
                                        }
                                        break;
                                    }
                                }
                            }
                            System.Threading.Thread.Sleep(800);
                        }
                        catch
                        {
                        }
                    }
                    while (Cancel == false);
                }
            }
        }

        
        
        public static bool ManualBydefault
        {
            get
            {
                return (Settings.workStation_SetupRow.Weigh_Scale == Scales.Manual);
            }
        }

    
        private static void SetScaleData(LocalWebService.LocalDataSet.Weigh_ScalesRow Weigh_Scale,ref Scales.ScaleData scaleData)
        {
            
            scaleData.CurWeight = Weigh_Scale.Weight;
            if (Weigh_Scale.OK)
            {
                scaleData.CurrentStatus = ScaleData.enumStatus.OK;
            }
            else
            {
                scaleData.CurrentStatus = ScaleData.enumStatus.Fail;
            }
            scaleData.Motion = Weigh_Scale.Motion;

        }

       


        public class ScaleData
        {

            public bool Manual { get; set; } = false;
            private string description;
            public string InboundPrinter { get; set; }
            public string OutboundPrinter { get; set; }
            public bool PrintInbound { get; set; }
            public bool PrintOutbound { get; set; }
            public ScaleData(string ScaleDescription)
            {
                description = ScaleDescription;
                CurWeight = 0;
                CurrentStatus = enumStatus.Fail ;
                ConnectionStatus = enumConnectionStatus.Connected; 
                Motion = false;
            }

            public string Description
            {
                get
                {
                    return description;
                }
            }


            /// <summary>
            /// return true if the scale is in motion ( Unstable)
            /// </summary>
            public bool Motion;

            public enum enumStatus
            {
                /// <summary>
                /// Everything is Ok
                /// </summary>
                OK,
                /// <summary>
                /// Scale is in a faulted State
                /// </summary>
                Fail,
         
            };

            public enumConnectionStatus ConnectionStatus;


            public enum enumConnectionStatus
            {
                Connected,
                Manual,
                Off
            }



            /// <summary>
            ///  The current status of the scale
            /// </summary>
            public enumStatus CurrentStatus;

            /// <summary>
            ///  The Current Weight of the scale
            /// </summary>
            public int CurWeight;

        

            /// <summary>
            /// Class for holding the current scale infoemation
            /// </summary>
            private ScaleData()
            {
               
            }

        }
    }
}
