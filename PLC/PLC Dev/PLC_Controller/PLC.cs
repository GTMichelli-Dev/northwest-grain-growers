using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logix;
using System.Collections.ObjectModel;
using System.Threading;

namespace PLC_Controller
{

    public class PLC : IDisposable
    {
        private bool disposed = false;
        Controller MainPLC = new Controller();
        Controller TreaterPLC = new Controller();

      public static TagGroup MainPLCGroup;
       public static TagGroup TreatGroup;

        public static Tag Clean_Batch_Status =new Tag( "Program:SeedLoadoutMain.Clean_Batch_Status", Logix.Tag.ATOMIC.INT);

        public static Tag Treat_Batch_Status =new Tag( "Program:SeedLoadoutMain.Treat_Batch_Status", Logix.Tag.ATOMIC.INT);

        public static Tag Clean_Batch_Unavailable = new Tag( "Program:SeedLoadoutMain.Clean_Scale_Unavailable",Logix.Tag.ATOMIC.BOOL);
        public static  Tag Treat_Batch_Unavailable =new Tag( "Program:SeedLoadoutMain.Treat_Scale_Unavailable",Logix.Tag.ATOMIC.BOOL);




        public struct structBinTags
        {

            public Tag Bin_Description;
            public Tag Item_Number;
            public Tag Variety_Name;
            public Tag Variety_Class;
            public Tag Available_For_Clean_Load;
            public Tag Available_For_Treat_Load;
            public Tag Set_New_Rate;
            public Tag New_Rate;
            public Tag Use_For_Clean_Loadout;
            public Tag Use_For_Treat_Loadout;
        }

        public struct structAndcoTags
        {

           
            public Tag  Faulted;
            public Tag  Fully_Open;
            public Tag Closed;
            public Tag Raw_Position;

        };

        public struct structTreaterPumpTags
        {
            public Tag Description;
            public Tag Product;
            public Tag Rate;
            public Tag Active_Treatment;
         
        }

        public struct structColorTags
        {
            public Tag Description;
            public Tag ID;
            public Tag Blue;
            public Tag Yellow;
            public Tag Red;
            public Tag White;
            public Tag Polymer;
            public Tag Duration;
        }


        public static structColorTags[] ColorTag = new structColorTags[20];

        public static  structAndcoTags[] AncoGateTag = new structAndcoTags[30];

        public static structBinTags[] BinTags = new structBinTags[30];

        public static structTreaterPumpTags[] TreaterTags = new structTreaterPumpTags[30];

        Tag Test = new Logix.Tag();

        Tag MainTag = new Tag();

        private int _ID;

        /// <summary>
        /// Instance of Logix.DTEncoding for Logix Data Type conversion
        /// </summary>
        DTEncoding udtEnc = new DTEncoding();




        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
  
                    this.MainPLC.Disconnect();
                    this.TreaterPLC.Disconnect();
                }

                disposed = true;
            }
        }

        ~PLC()
        {
            Dispose(false);
        }


        private PLC()
        {

        }


        public int ID
        {
            get
            {
                return _ID;
            }
        }


        public PLC(int PLC_ID, string IP_Address)
        {
            _ID = PLC_ID;
            MainPLC.IPAddress = IP_Address;
            MainPLC.Path = "3";
            MainPLC.Timeout = 3000;
            Init_Tag_Groups();
        }




        public List<string> TemplateList()
        {
            List<string> TempList = new List<string>();
            try
            {
                if (!MainPLC.IsConnected)
                {
                    MainPLC.IPAddress = "192.10.21.33";
                    MainPLC.Path = "3";
                    MainPLC.Timeout = 3000;
                    if (MainPLC.Connect() != ResultCode.E_SUCCESS)
                    {
                        System.Diagnostics.Debug.Print(MainPLC.ErrorString);
                        return TempList;
                    }
                }
                // connect to plc 
              
                // upload tags
                if (MainPLC.UploadTags() != ResultCode.E_SUCCESS)
                {
                    System.Diagnostics.Debug.Print(MainPLC.ErrorString);
                    return TempList;
                }    // get program list  
                ReadOnlyCollection<Logix.Program> programList = MainPLC.ProgramList;
                // iterate through programs  
                foreach (Logix.Program program in programList)
                {
                    // get list of TagTemplates    
                    ReadOnlyCollection<TagTemplate> templateList = program.TagItems();
                    // iterate though templates
                    foreach (TagTemplate item in templateList)
                    {
                        string Info = "Name = " + item.Name + ", Data Type = " + item.TypeName;
                        TempList.Add(Info);
                        System.Diagnostics.Debug.Print(Info);
                    }
                }
                return TempList;
            }


            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
                return TempList;
            }

        }


        public void ResetMainGroup()
        {
            if (MainPLCGroup != null)
            {
                MainPLCGroup.ScanStop();
                MainPLCGroup.Clear();
                
            }
        }

        public void ResetTreaterGroup()
        {
            if (MainPLCGroup != null)
            {
                TreatGroup.ScanStop();
                TreatGroup.Clear();
                

            }
        }

        public void Init_Tag_Groups()
        {
            int I;
            try
            {
                if (!TreaterPLC.IsConnected)
                {
                    TreaterPLC.IPAddress = "192.10.21.15";
                    TreaterPLC.Path = "0";
                    TreaterPLC.Timeout = 3000;
                    TreaterPLC.Connect();
                }
                if (!MainPLC.IsConnected)
                {
                    MainPLC.IPAddress = "192.10.21.33";
                    MainPLC.Path = "3";
                    MainPLC.Timeout = 3000;
                    MainPLC.Connect();
                }

                if ((BinTags[1].Bin_Description != null) && (BinTags[1].Item_Number != null) && (MainPLCGroup != null && MainPLCGroup.Tags.Count > 0))
                {
                    System.Diagnostics.Debug.Print("Bin Tag Last Scanned" + BinTags[1].Bin_Description.TimeStamp.ToLongTimeString() +
                                                    "  " + (DateTime.Now - BinTags[1].Bin_Description.TimeStamp).TotalSeconds.ToString() + " Sec ago   ");

                    double LasScannedSeconds = (DateTime.Now - BinTags[1].Bin_Description.TimeStamp).TotalSeconds;
                    if (LasScannedSeconds>25 && LasScannedSeconds<10000)
                    {
                        ResetMainGroup();
                    }
                }


                if ((TreaterTags[1].Description!=null))
                {
                    System.Diagnostics.Debug.Print("Treater Tag Last Scanned" + TreaterTags[1].Description.TimeStamp.ToLongTimeString() +
                                                    "  " + (DateTime.Now - TreaterTags[1].Description.TimeStamp).TotalSeconds.ToString() + " Sec ago   ");
                                                   
                    double LasScannedSeconds = (DateTime.Now - TreaterTags[1].Description.TimeStamp).TotalSeconds;
                    if (LasScannedSeconds > 25 && LasScannedSeconds < 10000)
                    {
                        ResetTreaterGroup();
                    }
                }


                

                if (TreatGroup == null || TreatGroup.Count==0)
                {
                    TreatGroup = new TagGroup(TreaterPLC);
                    for (I = 1; I < 20; I++)
                    {
                        TreaterTags[I].Description = new Tag("Treater_Pump[" + I.ToString() + "].Description", Logix.Tag.ATOMIC.STRING);
                        TreatGroup.AddTag(TreaterTags[I].Description);
                        TreaterTags[I].Product = new Tag("Treater_Pump[" + I.ToString() + "].Product", Logix.Tag.ATOMIC.STRING);
                        TreatGroup.AddTag(TreaterTags[I].Product);
                        TreaterTags[I].Rate = new Tag("Treater_Pump[" + I.ToString() + "].Rate", Logix.Tag.ATOMIC.REAL);
                        TreatGroup.AddTag(TreaterTags[I].Rate);
                        TreaterTags[I].Active_Treatment = new Tag("Treater_Pump[" + I.ToString() + "].Active_Treatment", Logix.Tag.ATOMIC.BOOL);
                        TreatGroup.AddTag(TreaterTags[I].Active_Treatment);
                    }
                    for (I = 0; I < 19; I++)
                    {
                        ColorTag[I].Description=new Tag("Colors[" + I.ToString() + "].Description", Logix.Tag.ATOMIC.STRING);
                        ColorTag[I].ID = new Tag("Colors[" + I.ToString() + "].ID", Logix.Tag.ATOMIC.INT);
                        ColorTag[I].Blue  = new Tag("Colors[" + I.ToString() + "].Blue", Logix.Tag.ATOMIC.REAL);
                        ColorTag[I].Yellow = new Tag("Colors[" + I.ToString() + "].Yellow", Logix.Tag.ATOMIC.REAL );
                        ColorTag[I].Red = new Tag("Colors[" + I.ToString() + "].Red", Logix.Tag.ATOMIC.REAL);
                        ColorTag[I].White = new Tag("Colors[" + I.ToString() + "].White", Logix.Tag.ATOMIC.REAL);
                        ColorTag[I].Polymer = new Tag("Colors[" + I.ToString() + "].Polymer", Logix.Tag.ATOMIC.REAL);
                        ColorTag[I].Duration = new Tag("Colors[" + I.ToString() + "].Duration", Logix.Tag.ATOMIC.INT);
                        TreatGroup.AddTag(ColorTag[I].Description);
                        TreatGroup.AddTag(ColorTag[I].ID);
                        TreatGroup.AddTag(ColorTag[I].Blue);
                        TreatGroup.AddTag(ColorTag[I].Yellow);
                        TreatGroup.AddTag(ColorTag[I].White);
                        TreatGroup.AddTag(ColorTag[I].Polymer);
                        TreatGroup.AddTag(ColorTag[I].Duration);
                    }

                    TreatGroup.ScanningMode = TagGroup.SCANMODE.ReadOnly ;
                    TreatGroup.ScanStart();

                }


         

            

                if (MainPLCGroup == null || MainPLCGroup.Tags.Count==0)
                {


                    MainPLCGroup = new TagGroup(MainPLC);
                    MainPLCGroup.ScanningMode = TagGroup.SCANMODE.ReadOnly ;

                    MainPLCGroup.AddTag(Clean_Batch_Status);
                    MainPLCGroup.AddTag(Treat_Batch_Status);
                    MainPLCGroup.AddTag(Clean_Batch_Unavailable);
                    MainPLCGroup.AddTag(Treat_Batch_Unavailable);



                for (I = 0; I < 29; I++)
                {
                    AncoGateTag[I].Faulted = new Tag("Program:SeedLoadoutMain.Loadout_Bin[" + I.ToString() + "].Andco_Values.Faulted", Logix.Tag.ATOMIC.BOOL);
                    MainPLCGroup.AddTag(AncoGateTag[I].Faulted);// new Tag("Program:SeedLoadoutMain.Loadout_Bin[" + I.ToString() + "].Andco_Values.Faulted", Logix.Tag.ATOMIC.BOOL));

                    AncoGateTag[I].Fully_Open = new Tag("Program:SeedLoadoutMain.Loadout_Bin[" + I.ToString() + "].Andco_Values.Fully_Open", Logix.Tag.ATOMIC.BOOL);
                    MainPLCGroup.AddTag(AncoGateTag[I].Fully_Open);// new Tag("Program:SeedLoadoutMain.Loadout_Bin[" + I.ToString() + "].Andco_Values.Fully_Open", Logix.Tag.ATOMIC.BOOL));

                    AncoGateTag[I].Closed = new Tag("Program:SeedLoadoutMain.Loadout_Bin[" + I.ToString() + "].Andco_Values.Closed", Logix.Tag.ATOMIC.BOOL);
                    MainPLCGroup.AddTag(AncoGateTag[I].Closed);// new Tag("Program:SeedLoadoutMain.Loadout_Bin[" + I.ToString() + "].Andco_Values.Closed", Logix.Tag.ATOMIC.BOOL));

                    AncoGateTag[I].Raw_Position = new Tag("Program:SeedLoadoutMain.Loadout_Bin[" + I.ToString() + "].Andco_Values.Raw_Position", Logix.Tag.ATOMIC.DINT);
                    MainPLCGroup.AddTag(AncoGateTag[I].Raw_Position);// new Tag("Program:SeedLoadoutMain.Loadout_Bin[" + I.ToString() + "].Andco_Values.Raw_Position", Logix.Tag.ATOMIC.DINT));


                }


                for (I = 0; I < 29; I++)
                    {

                        BinTags[I].Bin_Description = new Tag("Program:SeedLoadoutMain.Loadout_Bin[" + I.ToString() + "].Bin_Description", Tag.ATOMIC.STRING);
                        MainPLCGroup.AddTag(BinTags[I].Bin_Description);

                        BinTags[I].Item_Number = new Tag("Program:SeedLoadoutMain.Loadout_Bin[" + I.ToString() + "].Bin_Variety.Item_Number", Tag.ATOMIC.STRING);
                        MainPLCGroup.AddTag(BinTags[I].Item_Number);

                        BinTags[I].Variety_Name = new Tag("Program:SeedLoadoutMain.Loadout_Bin[" + I.ToString() + "].Bin_Variety.Name", Logix.Tag.ATOMIC.STRING);
                        MainPLCGroup.AddTag(BinTags[I].Variety_Name);

                        BinTags[I].Variety_Class = new Tag("Program:SeedLoadoutMain.Loadout_Bin[" + I.ToString() + "].Bin_Variety.Class", Tag.ATOMIC.STRING);
                        MainPLCGroup.AddTag(BinTags[I].Variety_Class);

                        BinTags[I].Available_For_Clean_Load = new Tag("Program:SeedLoadoutMain.Loadout_Bin[" + I.ToString() + "].Available_For_Clean_Load", Tag.ATOMIC.BOOL);
                        MainPLCGroup.AddTag(BinTags[I].Available_For_Clean_Load);

                        BinTags[I].Available_For_Treat_Load = new Tag("Program:SeedLoadoutMain.Loadout_Bin[" + I.ToString() + "].Available_For_Treat_Load", Tag.ATOMIC.BOOL);
                        MainPLCGroup.AddTag(BinTags[I].Available_For_Treat_Load);

                        BinTags[I].Set_New_Rate = new Tag("Program:SeedLoadoutMain.Loadout_Bin[" + I.ToString() + "].Andco_Values.Set_New_Rate", Tag.ATOMIC.BOOL);
                       // MainPLCGroup.AddTag(BinTags[I].Set_New_Rate);

                        BinTags[I].New_Rate = new Tag("Program:SeedLoadoutMain.Loadout_Bin[" + I.ToString() + "].Andco_Values.New_Rate", Tag.ATOMIC.REAL);
                        //MainPLCGroup.AddTag(BinTags[I].New_Rate);

                        BinTags[I].Use_For_Clean_Loadout = new Tag("Program:SeedLoadoutMain.Loadout_Bin[" + I.ToString() + "].Use_For_Clean_Loadout", Tag.ATOMIC.BOOL);
                       // MainPLCGroup.AddTag(BinTags[I].Use_For_Clean_Loadout);

                        BinTags[I].Use_For_Treat_Loadout = new Tag("Program:SeedLoadoutMain.Loadout_Bin[" + I.ToString() + "].Use_For_Treat_Loadout", Tag.ATOMIC.BOOL);
                       // MainPLCGroup.AddTag(BinTags[I].Use_For_Treat_Loadout);

                    }

                    MainPLCGroup.ScanStart();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
            }

        }

        public static void PauseTagGroups()
        {
            try
            {
                if (MainPLCGroup != null)
                {
                    MainPLCGroup.ScanSuspend();
                }
            }
            catch { }
       
            try
            {
                if (TreatGroup != null)
                {
                    TreatGroup.ScanSuspend();
                }
            }
            catch { }
          
        }



        public static void ResumeTagGroups()
        {
            try
            {
                if (MainPLCGroup != null)
                {
                    MainPLCGroup.ScanResume();
                }
            }
            catch { }
     
            try
            {
                if (TreatGroup != null)
                {
                    TreatGroup.ScanResume();
                }
            }
            catch { }
         
        }

        //#region Color
        //public struct Color
        //{




        //    public string Description;
        //    public string Hex;
        //    public float Cyan;
        //    public float Yellow;
        //    public float Magenta;
        //    public float Black;

        //};


        //private Color color_values = new Color();



        //public string Updatecolors(int ID,string Description,string Hex,float Cyan,float Yellow,float Magenta,float Black)
        //{
        //    try
        //    {
        //        if (!TreaterPLC.IsConnected)
        //        {
        //            TreaterPLC.IPAddress = "192.10.21.15";
        //            TreaterPLC.Path = "0";
        //            TreaterPLC.Timeout = 3000;
        //            if (TreaterPLC.Connect() != ResultCode.E_SUCCESS)
        //            {
        //                return "Error Connecting To PLC " + MainTag.ErrorString;

        //            }


        //        }

        //        Tag TagDescription = new Tag("Colors[" + ID.ToString() + "].Description", Logix.Tag.ATOMIC.STRING);// .ATOMIC.OBJECT);
        //        Tag TagHex = new Tag("Colors[" + ID.ToString() + "].Hex", Logix.Tag.ATOMIC.STRING);
        //        Tag TagCyan = new Tag("Colors[" + ID.ToString() + "].Cyan", Logix.Tag.ATOMIC.REAL );
        //        Tag TagYellow = new Tag("Colors[" + ID.ToString() + "].Yellow", Logix.Tag.ATOMIC.REAL);
        //        Tag TagMagenta = new Tag("Colors[" + ID.ToString() + "].Magenta", Logix.Tag.ATOMIC.REAL);
        //        Tag TagBlack = new Tag("Colors[" + ID.ToString() + "].Black", Logix.Tag.ATOMIC.REAL);

        //        if (TreaterPLC.ReadTag(TagDescription) != Logix.ResultCode.E_SUCCESS)
        //        {

        //            return TreaterPLC.ErrorString;
        //        }
        //        else
        //        {
        //            TreaterPLC.ReadTag(TagHex);
        //           // color_values = (Color)udtEnc.ToType(TagColor, typeof(Color));
        //            try
        //            {

        //                TagDescription.Value = Description;
        //                TagHex.Value = Hex;
        //                TagCyan.Value = Cyan;
        //                TagYellow.Value = Yellow;
        //                TagMagenta.Value = Magenta;
        //                TagBlack.Value = Black;
        //                // get values for remaining members
        //                //color_values.Description = Description;
        //                //color_values.Hex = Hex;
        //                //color_values.Cyan = Cyan;
        //                //color_values.Yellow = Yellow;
        //                //color_values.Magenta = Magenta;
        //                //color_values.Black = Black;
        //                //TagColor.Value = udtEnc.FromType(color_values);
        //                TreaterPLC.WriteTag(TagDescription);
        //                TreaterPLC.WriteTag(TagHex);
        //                TreaterPLC.WriteTag(TagCyan);
        //                TreaterPLC.WriteTag(TagYellow);
        //                TreaterPLC.WriteTag(TagMagenta);
        //                TreaterPLC.WriteTag(TagBlack);

        //            }
        //            catch (System.Exception ex)
        //            {
        //                return ex.Message;
        //            }
        //            return TreaterPLC.ErrorString;
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //}












        //public Color Current_Color_Values
        //{
        //    get
        //    {
        //        return color_values;
        //    }
        //}
        //#endregion


        #region Colors

        public PLC_ControllerDataset.ColorsDataTable  GetColors()
        {
            Init_Tag_Groups();

            using (PLC_ControllerDataset.ColorsDataTable  ColorsTable = new PLC_ControllerDataset.ColorsDataTable())
            {
                for (int I = 0; I < 19; I++)
                {
                    
                    {
                        PLC_ControllerDataset.ColorsRow  row = ColorsTable.NewColorsRow();
                        row.ID  =Convert.ToInt32( ColorTag[I].ID.Value) ;
                        row.Description = ColorTag[I].Description.Value.ToString();
                        
                        row.Blue  = Convert.ToDouble(ColorTag[I].Blue.Value);
                        row.Yellow = Convert.ToDouble(ColorTag[I].Yellow.Value);
                        row.Red = Convert.ToDouble(ColorTag[I].Red.Value);
                        row.White = Convert.ToDouble(ColorTag[I].White.Value);
                        row.Polymer = Convert.ToDouble(ColorTag[I].Polymer.Value);
                        ColorsTable.AddColorsRow(row);
                    }
                }
                return ColorsTable ;
            }
        }




        #endregion

        #region TreatmentPumps



        public PLC_ControllerDataset.Treat_PumpsDataTable  GetTreaterPumps()
        {
            Init_Tag_Groups();

            using (PLC_ControllerDataset.Treat_PumpsDataTable TreatPumpTable = new PLC_ControllerDataset.Treat_PumpsDataTable())
            {
                for (int I = 1; I < 20; I++)
                {
                    string Product = TreaterTags[I].Product.Value.ToString();
                    if (!string.IsNullOrEmpty(Product) && Product.ToUpper() != "MINERAL OIL" && Product.ToUpper() != "WATER")
                    {
                        PLC_ControllerDataset.Treat_PumpsRow row = TreatPumpTable.NewTreat_PumpsRow();
                        row.Pump_Index = I;
                        row.Pump_Description = TreaterTags[I].Description.Value.ToString();
                        row.Product = TreaterTags[I].Product.Value.ToString();
                        row.Rate = Convert.ToDouble(TreaterTags[I].Rate.Value);
                        row.Active_Treatment = Convert.ToBoolean(TreaterTags[I].Active_Treatment.Value);
                        TreatPumpTable.AddTreat_PumpsRow(row);
                    }
                }
                return TreatPumpTable;
            }
        }


        #endregion


        #region AvailableBins



        public PLC_ControllerDataset.BinsDataTable GetCleanBins()
        {
            Init_Tag_Groups();
            
            using (PLC_ControllerDataset.BinsDataTable BinsTable = new PLC_ControllerDataset.BinsDataTable())
            {
                try
                {
                    for (int I = 0; I < 29; I++)
                    {


                        PLC_ControllerDataset.BinsRow row = BinsTable.NewBinsRow();
                        row.Bin_Index = I;
                        string NamePrefix = "Concrete Bin ";
                        if (I > 15) NamePrefix = "Steel Bin ";

                        row.Bin_Description = NamePrefix + BinTags[I].Bin_Description.Value.ToString();
                        row.Item_Number = BinTags[I].Item_Number.Value.ToString();
                        row.Variety_Name = BinTags[I].Variety_Name.Value.ToString();
                        row.Variety_Class = BinTags[I].Variety_Class.Value.ToString();
                        row.Available_For_Clean_Load = Convert.ToBoolean(BinTags[I].Available_For_Clean_Load.Value);
                        row.Available_For_Treat_Load = Convert.ToBoolean(BinTags[I].Available_For_Treat_Load.Value);
                        row.Set_New_Rate = Convert.ToBoolean(BinTags[I].Set_New_Rate.Value);
                        row.New_Rate = Convert.ToDouble( BinTags[I].New_Rate.Value);
                        row.Use_For_Clean_Loadout = Convert.ToBoolean(BinTags[I].Use_For_Clean_Loadout.Value);
                        row.Use_For_Treat_Loadout = Convert.ToBoolean(BinTags[I].Use_For_Treat_Loadout.Value);
                        BinsTable.AddBinsRow(row);

                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print(ex.Message);
                }
                return BinsTable;
            }
        }


        public PLC_ControllerDataset.LoadingStatusDataTable GetLoadingStatus()
        {
            
            PLC_ControllerDataset.LoadingStatusDataTable LoadingStatus = new PLC_ControllerDataset.LoadingStatusDataTable();
            PLC_ControllerDataset.LoadingStatusRow row = LoadingStatus.NewLoadingStatusRow(); 
            Init_Tag_Groups();
  
            row.Clean_Batch_Status  = Convert.ToInt32(Clean_Batch_Status.Value);
            row.Treat_Batch_Status  = Convert.ToInt32(Treat_Batch_Status.Value);
            row.Clean_Unavailable  = Convert.ToBoolean(Clean_Batch_Unavailable.Value);
            row.Treat_Unavailable  = Convert.ToBoolean(Treat_Batch_Unavailable.Value);
            LoadingStatus.AddLoadingStatusRow(row);
            return LoadingStatus;
        }

        public void SetTreatStatus(int Status)
        {
            Init_Tag_Groups();

            Treat_Batch_Status.Value = Status;

        }

        public void SetCleanStatus(int Status)
        {
            Init_Tag_Groups();

            Clean_Batch_Status.Value = Status;

        }

        public void UpdateBinStatus(PLC_ControllerDataset.BinsDataTable BinsTable)
        {
            Init_Tag_Groups();
            for (int Bin_Index = 0; Bin_Index < 29; Bin_Index++)
            {

        
                PLC_ControllerDataset.BinsRow row = BinsTable[Bin_Index];
   
  

            }

        }


        #endregion






        #region Varieties
        public struct Variety
        {




            public int Item_Number;
            public string Name;
            public float Class;

        };


     



        public string UpdateVariety(int Index,int ID, string Name, string Class)
        {
            try
            {
                if (!TreaterPLC.IsConnected)
                {
                    TreaterPLC.IPAddress = "192.10.21.33";
                    TreaterPLC.Path = "3";
                    TreaterPLC.Timeout = 3000;
                    if (TreaterPLC.Connect() != ResultCode.E_SUCCESS)
                    {
                        return "Error Connecting To PLC " + MainTag.ErrorString;

                    }


                }

                Tag TagID = new Tag("Varieties[" + Index.ToString() + "].Item_Number", Logix.Tag.ATOMIC.INT  );
                Tag TagName = new Tag("Varieties[" + Index.ToString() + "].Name", Logix.Tag.ATOMIC.STRING);
                Tag TagClass = new Tag("Varieties[" + Index.ToString() + "].Class", Logix.Tag.ATOMIC.STRING);
                bool Success = (TreaterPLC.ReadTag(TagID) == Logix.ResultCode.E_SUCCESS);
                if (Success) Success = (TreaterPLC.ReadTag(TagName) == Logix.ResultCode.E_SUCCESS);
                if (Success) Success = (TreaterPLC.ReadTag(TagClass) == Logix.ResultCode.E_SUCCESS);

                if (! Success)
                {

                    return TreaterPLC.ErrorString;
                }
                else
                {
                  
                    try
                    {

                        TagID.Value = ID;
                        TagName.Value = Name;
                        TagClass.Value = Class;
                        TreaterPLC.WriteTag(TagID);
                        TreaterPLC.WriteTag(TagName);
                        TreaterPLC.WriteTag(TagClass);
                    }
                    catch (System.Exception ex)
                    {
                        return ex.Message;
                    }
                    return TreaterPLC.ErrorString;
                }
            }
            catch (System.Exception ex)
            {
                return ex.Message;
            }
        }



        #endregion


        #region Andco Stuff




        //public struct ANDCO
        //{
        //    // boolean values are packed into 32-bit ints
        //    public int boolVals;
        //    public int Cal_Status;
        //    public int Raw_Position;
        //    public int Max_Position;
        //    public int Preact;
        //    public int Tolerance;
        //    public float New_Pct_Open;
        //    public float Percent_Open;
        //};


        private structAndcoTags andco_values = new structAndcoTags();

        public structAndcoTags Current_Andco_Values
        {
            get
            {
                return andco_values;
            }
        }



        public string UpdateGate(int Gate, bool Faulted, bool Closed, bool Fully_Open, int Raw_Position)
        {
            try
            {
                Init_Tag_Groups();
                try
                {
                    if ((bool)AncoGateTag[Gate].Faulted.Value != Faulted) WriteTagValue( enumPlc.Main ,AncoGateTag[Gate].Faulted.Name, Faulted);
                    if ((bool)AncoGateTag[Gate].Closed.Value != Closed) WriteTagValue(enumPlc.Main, AncoGateTag[Gate].Closed.Name, Closed);
                    if ((bool)AncoGateTag[Gate].Fully_Open.Value != Fully_Open) WriteTagValue(enumPlc.Main, AncoGateTag[Gate].Fully_Open.Name, Fully_Open);
                    if ((int)AncoGateTag[Gate].Raw_Position.Value != Raw_Position) WriteTagValue(enumPlc.Main, AncoGateTag[Gate].Raw_Position.Name, Raw_Position);
                    AncoGateTag[Gate].Faulted.Value = Faulted;
                    AncoGateTag[Gate].Closed.Value = Closed;
                    AncoGateTag[Gate].Fully_Open.Value = Fully_Open;
                    AncoGateTag[Gate].Raw_Position.Value = Raw_Position;
                }
                catch (System.Exception ex)
                {
                    return ex.Message;
                }
                return MainPLC.ErrorString;
            }
            catch (System.Exception ex)
            {
                return ex.Message;
            }
        }

        #endregion




        public string MainPLC_Connect()
        {
            if (MainPLC.Connect() != ResultCode.E_SUCCESS)
            {
               return "Error Connecting To PLC "+MainTag.ErrorString;
               
            }
            else
            {
                return "Connected To PLC";
            }

        }

        public struct RetVal
        {
           public bool Sucess;
           public string Message;
        }


        public static Logix.Tag  ReadTagValue(Controller PLC,  Logix.Tag TagToRead)
        {
            try
            {
              
                PLC.ReadTag(TagToRead);
            }
            catch 
            {
            }
      
            return TagToRead;
        }

        public enum enumPlc
        {
            Main,
            treater

        }


        public RetVal WriteTagValue(enumPlc PLCToConnectTo, string TagName, object Value)
        {
            RetVal Results = new PLC_Controller.PLC.RetVal();
            new Thread(() =>
            {
     




           
            try
            {
                Controller PLC;
                if (PLCToConnectTo== enumPlc.treater  )
                {
                    PLC = TreaterPLC;
                }
                else
                {
                    PLC = MainPLC;
                }


                //PauseTagGroups();
                ////////////////////////////////////
                ///provide information to Tag class
                ///
                MainTag.Name = TagName;

                /////////////////////////////////////
                /// update the .Value property
                /// 
                if (PLC.ReadTag(MainTag) == ResultCode.E_SUCCESS)
                {
                    MainTag.Value = Value;
                    MainTag.Length = 1;
                    if (PLC.WriteTag(MainTag) != ResultCode.E_SUCCESS)
                    {

                        Results.Sucess = false;
                        Results.Message = MainPLC.ErrorString;

                    }
                    else
                    {
                        Results.Sucess = true;
                        if (Logix.ResultCode.QUAL_GOOD == MainTag.QualityCode)
                            Results.Message = "OK";
                        else
                            Results.Message = MainTag.QualityString;
                    }
                }
                else
                {
                    Results.Sucess = false;
                    Results.Message = PLC.ErrorString;

                }


            }

            catch (System.Exception ex)
            {
                Results.Sucess = false;
                Results.Message = ex.Message.ToString();
            }
            finally
            {
               // ResumeTagGroups();
            }

            }).Start();
            return Results;
        }
    }
}
