using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibplctagWrapper;
using System.Threading;
using System.Collections;

namespace WW_LGX_PLC
{
    public sealed class PLC : IDisposable

    {


        




        public List<TreatmentRow> TreatmentList = new List<TreatmentRow>();

        public List<BinRow> BinList = new List<BinRow>();


        public class ItemRow
        {
            public int Id { get; set; }
            public object Value { get; set; }
        }

        public class ReadTagValue
        {
            public ReadTagValue()
            {
                ErrorMessage = string.Empty;
                Error = false;
            }
            public object Value { get; set; }
            public string ErrorMessage { get; set; }
            public bool Error { get; set; }
        }

        public class TreatmentRow
        {
            public TreatmentRow()
            {
                Id = -1;
                Description = string.Empty;
                Error = false;
                LastUpdate = DateTime.Now.AddYears(-1);
            }
            public int Id { get; set; }
            public string Description { get; set; }
            public bool Error { get; set; }
            public DateTime LastUpdate { get; set; }

        }


        public class BinRow
        {
            public BinRow()
            {
                BinId = -1;
                BinName = string.Empty;
                Variety_Id = string.Empty;
                Available_For_Clean_Load = false;
                Available_For_Treat_Load = false;
                Read_Use_For_Clean_Error = false;
                Read_Use_For_Treat_Error = false;
                Read_Variety_Error = false;
                Last_Variety_Update = DateTime.Now.AddYears(-1);
                Last_Use_For_Clean_Update = DateTime.Now.AddYears(-1);
                Las_Use_For_Treat_Update = DateTime.Now.AddYears(-1);
            }
            public string BinName { get; set; }
            public int BinId { get; set; }
            public string Variety_Id { get; set; }
            public bool Available_For_Clean_Load { get; set; }
            public bool Available_For_Treat_Load { get; set; }
            public bool Read_Variety_Error { get; set; }
            public bool Read_Use_For_Clean_Error { get; set; }
            public bool Read_Use_For_Treat_Error { get; set; }
            public DateTime Last_Variety_Update { get; set; }
            public DateTime Last_Use_For_Clean_Update { get; set; }
            public DateTime Las_Use_For_Treat_Update { get; set; }


        }




        public class stringTagValues
        {
            public stringTagValues(int TagIndex)
            {
                ID = TagIndex;
                Value = string.Empty;
                ErrorMessage = string.Empty;
                Error = false;
            }
            public int ID { get; set; }
            public string Value { get; set; }
            public string ErrorMessage { get; set; }
            public bool Error { get; set; }
        }


        public class booleanTagValues
        {
            public booleanTagValues(int TagIndex)
            {
                ID = TagIndex;
                Value = false;
                ErrorMessage = string.Empty;
                Error = false;
            }


            public int ID { get; set; }
            public bool Value { get; set; }
            public string ErrorMessage { get; set; }
            public bool Error { get; set; }
        }

        public class bitArrayTagValues
        {
            public bitArrayTagValues()
            {
                BitValues = new List<bool>();
                ErrorMessage = string.Empty;
                Error = false;
            }
            public List<bool> BitValues { get; set; }
            public string ErrorMessage { get; set; }
            public bool Error { get; set; }
        }

        public class intTagValues
        {
            public intTagValues(int TagIndex)
            {
                ID = TagIndex;
                Value = -1;
                ErrorMessage = string.Empty;
                Error = false;
            }


            public int ID { get; set; }
            public int Value { get; set; }
            public string ErrorMessage { get; set; }
            public bool Error { get; set; }
        }




        public static bool ScanningTags
        {
            get
            {
                return scanningTags;
            }
        }

        private static bool scanningTags = false;

        public bool GettingValues { get; set; }

        public Guid PcUID= Guid.Empty ;


        public intTagValues CleanBatchStatus = new intTagValues(0);
        public intTagValues TreatBatchStatus = new intTagValues(0);

        public booleanTagValues CleanScaleUnavailable = new booleanTagValues(0);
        public booleanTagValues TreatScaleUnavailable = new booleanTagValues(0);

        public DateTime LastBinUpdate = DateTime.Now.AddMinutes(-1);

        public DateTime LastUpdate = DateTime.Now.AddDays(-1000);
        public double UpdateSeconds;

        public PLC()
        {
            GettingValues = false;
            for (int i = 0; i < 29; i++)
            {
                TreatmentList.Add(new TreatmentRow() { Id = i });
            }




            for (int i = 0; i < 26; i++)
            {
                BinList.Add(new BinRow() { BinId = i });
            }

        }

        /// <summary>
        /// StopCancel=0 SelectBins=1  AcceptBins=2
        /// </summary>
        public enum enumBatchStatus
        {
            StopCancel,
            SelectBins
        }


        public class TagResponse
        {
            public object Value = null;
            public bool OK = false;
            public string Message = "";

            public TagResponse()
            {

            }

            public TagResponse(bool ok, string message)
            {
                OK = ok;
                Message = message;
            }

        }





        static Libplctag Mainclient = new Libplctag();
        static Libplctag Treaterclient = new Libplctag();





        /// <summary>
        /// Treat Or Clean Batch
        /// </summary>
        public enum enumBatchType
        {
            Treat,
            Clean
        }




        public enum enumController
        {
            mainPlc,
            treaterPLC

        }


        public static string ControllerAddress(enumController controller)
        {
            switch (controller)
            {
                case enumController.mainPlc:
                    return MainPLCAddress;

                case enumController.treaterPLC:
                    return TreaterPLCAddress;

                default:
                    return string.Empty;

            }
        }




        static string MainPLCAddress = "192.10.21.33";
        static string TreaterPLCAddress = "192.10.21.15";
        private const int DataTimeout = 5000;





        public static void SetBatchState(enumBatchType BatchType, enumBatchStatus Status)
        {
            int BatchCode = 0;
            if (Status == enumBatchStatus.SelectBins)
            {
                BatchCode = 1;
            }
            else
            {
                BatchCode = 0;
            }


            WriteIntTag(GetTag(enumController.mainPlc, BatchStatusTag(BatchType), LibplctagWrapper.DataType.DINT), BatchCode);

        }


        public static void SetBatchBin(enumBatchType BatchType, int Bin, bool UseInBatch)
        {
            Tag tag = null;
            if (BatchType == enumBatchType.Clean)
            {
                tag = new Tag(MainPLCAddress, "1,3", CpuType.LGX, Bin_Use_For_Clean_Loadout(Bin), 1, 1);

            }
            else
            {
                tag = new Tag(MainPLCAddress, "1,3", CpuType.LGX, Bin_Use_For_Treat_Loadout(Bin), 1, 1);

            }



            Thread thread = new Thread(() => SetBooleanTag(tag, UseInBatch));
            thread.Start();
            //SetBooleanTag(tag, UseInBatch);

        }




        public static void SetTreaterPumpRate(int Pump, float NewValue)
        {
            NewValue = NewValue * 40;
            string TagName = "Treater_Pump[" + Pump.ToString() + "].Rate";
            Tag tag = new Tag(TreaterPLCAddress, "1,0", CpuType.LGX, TagName, DataType.REAL, 1);
            Thread thread = new Thread(() => WriteRealTag(tag, NewValue));
            thread.Start();


        }



        public static void SetTreaterPumpActive(int Pump, bool Active)
        {
            string TagName = "Treater_Pump[" + Pump.ToString() + "].Active_Treatment";
            Tag tag = new Tag(TreaterPLCAddress, "1,0", CpuType.LGX, TagName, 1, 1);
            SetBooleanTag(tag, Active);
            //Thread thread = new Thread(() => SetBooleanTag(tag, Active));
            //thread.Start();


        }



        private static string BatchStatusTag(enumBatchType BatchType)
        {
            if (BatchType == enumBatchType.Clean)
            {
                return "Program:SeedLoadoutMain.Clean_Batch_Status";
            }
            else
            {
                return "Program:SeedLoadoutMain.Treat_Batch_Status";
            }
        }



        public static TagResponse CheckTagStatus(Libplctag client, Tag tag)
        {
            try
            {


                if (tag == null) return new TagResponse(false, "Cannot Find Tag");
                client.AddTag(tag);
                while (client.GetStatus(tag) == Libplctag.PLCTAG_STATUS_PENDING)
                {
                    Thread.Sleep(100);
                }



                // if the status is not ok, we have to handle the error
                if (client.GetStatus(tag) != Libplctag.PLCTAG_STATUS_OK)
                {
                    return new TagResponse(false, ($"Error setting up tag internal state. Error { client.DecodeError(client.GetStatus(tag))}\n"));


                }
                return new WW_LGX_PLC.PLC.TagResponse(true, "");
            }
            catch (Exception ex)
            {
                return new WW_LGX_PLC.PLC.TagResponse(false, ex.Message);
            }

        }




        public static intTagValues ReadInt(LibplctagWrapper.Tag tag, Libplctag client, int TagID)
        {

            {

                try
                {




                    intTagValues Retval = new WW_LGX_PLC.PLC.intTagValues(TagID);



                    // check that the tag has been added, if it returns pending we have to retry
                    while (client.GetStatus(tag) == Libplctag.PLCTAG_STATUS_PENDING)
                    {
                        Thread.Sleep(100);
                    }


                    // if the status is not ok, we have to handle the error
                    if (client.GetStatus(tag) != Libplctag.PLCTAG_STATUS_OK)
                    {
                        Retval.Error = true;
                        Retval.ErrorMessage = $"Error setting up tag internal state. Error { client.DecodeError(client.GetStatus(tag))}";



                    }


                    var result1 = client.ReadTag(tag, DataTimeout);


                    // Check the read operation result
                    if (result1 != Libplctag.PLCTAG_STATUS_OK)
                    {
                        Retval.Error = true;
                        Retval.ErrorMessage = $"ERROR: Unable to read the data! Got error code {result1}: {client.DecodeError(result1)}";

                    }



                    Retval.Value = client.GetInt32Value(tag, 0);
                    Retval.Error = false;



                    return Retval;



                }
                finally
                {

                }

            }

        }


        public static bitArrayTagValues ReadBooleanFromDint(Libplctag client, LibplctagWrapper.Tag tag)
        {

            {

                try
                {




                    bitArrayTagValues Retval = new WW_LGX_PLC.PLC.bitArrayTagValues();

                    for (int i = 0; i < 32; i++)
                    {
                        Retval.BitValues.Add(false);
                    }


                    // check that the tag has been added, if it returns pending we have to retry
                    while (client.GetStatus(tag) == Libplctag.PLCTAG_STATUS_PENDING)
                    {
                        Thread.Sleep(100);
                    }


                    // if the status is not ok, we have to handle the error
                    if (client.GetStatus(tag) != Libplctag.PLCTAG_STATUS_OK)
                    {
                        Retval.Error = true;
                        Retval.ErrorMessage = $"Error setting up tag internal state. Error { client.DecodeError(client.GetStatus(tag))}";



                    }


                    var result1 = client.ReadTag(tag, DataTimeout);



                    // Check the read operation result
                    if (result1 != Libplctag.PLCTAG_STATUS_OK)
                    {
                        Retval.Error = true;
                        Retval.ErrorMessage = $"ERROR: Unable to read the data! Got error code {result1}: {client.DecodeError(result1)}";

                    }

                    else
                    {
                        int DINTval = client.GetInt32Value(tag, 0);

                        BitArray b = new BitArray(new int[] { DINTval });
                        bool[] bits = new bool[b.Count];
                        b.CopyTo(bits, 0);
                        Retval.BitValues.Clear();
                        foreach (var bVal in bits)
                        {
                            Retval.BitValues.Add(bVal);
                        }

                    }


                    Retval.Error = false;



                    return Retval;



                }
                finally
                {

                }

            }

        }


        public static booleanTagValues ReadBoolean(Libplctag client, LibplctagWrapper.Tag tag, int TagID)
        {

            {

                try
                {




                    booleanTagValues Retval = new WW_LGX_PLC.PLC.booleanTagValues(TagID);



                    // check that the tag has been added, if it returns pending we have to retry
                    while (client.GetStatus(tag) == Libplctag.PLCTAG_STATUS_PENDING)
                    {
                        Thread.Sleep(100);
                    }


                    // if the status is not ok, we have to handle the error
                    if (client.GetStatus(tag) != Libplctag.PLCTAG_STATUS_OK)
                    {
                        Retval.Error = true;
                        Retval.ErrorMessage = $"Error setting up tag internal state. Error { client.DecodeError(client.GetStatus(tag))}";



                    }


                    var result1 = client.ReadTag(tag, DataTimeout);


                    // Check the read operation result
                    if (result1 != Libplctag.PLCTAG_STATUS_OK)
                    {
                        Retval.Error = true;
                        Retval.ErrorMessage = $"ERROR: Unable to read the data! Got error code {result1}: {client.DecodeError(result1)}";

                    }



                    Retval.Value = client.GetBitValue(tag, -1, 5000);
                    Retval.Error = false;



                    return Retval;



                }
                finally
                {

                }

            }

        }



        public static void SetColors(List<booleanTagValues> ColorValues)
        {
            foreach (var value in ColorValues)
            {
                string TagName = $"Colors[{value.ID}].Active";
                Tag tag = new Tag(TreaterPLCAddress, "1,0", CpuType.LGX, TagName, 1, 1);
                SetBooleanTag(tag, value.Value);
            }
        }


        private static TagResponse SetBooleanTag(Tag tag, bool Value)
        {
            TagResponse tr = new WW_LGX_PLC.PLC.TagResponse();
            try
            {


                using (Libplctag client = new Libplctag())
                {
                    tr = CheckTagStatus(client, tag);
                    if (tr.OK)
                    {
                        int result = client.WriteBool(tag, -1, Value, DataTimeout);
                        //int result = client.SetBitValue(tag, 0, value, DataTimeout);
                        //int result=client.WriteBool(tag, 0, value, DataTimeout);
                        //client.WriteTag(tag, DataTimeout);
                        // Check the read operation result
                        if (result != Libplctag.PLCTAG_STATUS_OK)
                        {
                            tr.OK = false;
                            tr.Message = ($"ERROR: Unable to read the data! Got error code {result}: {client.DecodeError(result)}\n");

                        }
                        //client.RemoveTag(tag);

                    }
                }
            }
            finally
            {

            }
            return tr;
        }


        public void StartScanning()
        {
            if (!scanningTags)
            {
                Thread thread = new Thread(() => ScanTags());
                thread.Start();
            }
        }


        //Scan Bin and Treatments
        private void ScanTags()
        {

            scanningTags = true;
            bool BinNamesNotSet = true;
            List<string> BinDescriptions = new List<string>();
            using (var client = new Libplctag())
            {
                try
                {

                    do
                    {
                        GettingValues = true;


                        //Update the Bins and treatments every Minute
                        //if ((DateTime.Now - LastBinUpdate).TotalMinutes > 1)
                        {

                            LastBinUpdate = DateTime.Now;


                            var Treat_Unavailable_Tag = new Tag(ControllerAddress(enumController.mainPlc), "1,3", CpuType.LGX, $"Program:SeedLoadoutMain.Treat_Scale_Unavailable", 1, 1);
                            client.AddTag(Treat_Unavailable_Tag);
                            TreatScaleUnavailable = ReadBoolean(client, Treat_Unavailable_Tag, 0);
                            client.RemoveTag(Treat_Unavailable_Tag);

                            var Clean_Unavailable_Tag = new Tag(ControllerAddress(enumController.mainPlc), "1,3", CpuType.LGX, $"Program:SeedLoadoutMain.Clean_Scale_Unavailable", 1, 1);
                            client.AddTag(Clean_Unavailable_Tag);
                            CleanScaleUnavailable = ReadBoolean(client, Clean_Unavailable_Tag, 0);
                            client.RemoveTag(Clean_Unavailable_Tag);



                            var TreatmentTag = new Tag(TreaterPLCAddress, "1,0", CpuType.LGX, $"Program:MainProgram.Pump_Product_Description", DataType.String, 26);
                            client.AddTag(TreatmentTag);
                            ReadTagValue TreatmentValues = ReadStringArray(TreatmentTag, client);


                            List<string> Values = (List<string>)TreatmentValues.Value;
                            for (int i = 0; i < Values.Count; i++)
                            {
                                if (!TreatmentValues.Error)
                                {
                                    TreatmentList[i].Description = Values[i];
                                    TreatmentList[i].LastUpdate = DateTime.Now;
                                    TreatmentList[i].Error = false;
                                }
                                else
                                {
                                    TreatmentList[i].Error = true;
                                }


                            }




                            client.RemoveTag(TreatmentTag);




                            //{::[NWGG]Program: SeedLoadoutMain.Loadout_Bin[10].Bin_Description}
                            bool Errors = false;
                            if (BinNamesNotSet)
                            {
                                BinDescriptions.Clear();
                                for (int i = 0; i < 29; i++)
                                {
                                    string Side = (i < 16) ? "(C)" : "(S)";
                                    var BinDiscriptionTag = new Tag(ControllerAddress(enumController.mainPlc), "1,3", CpuType.LGX, $"Program:SeedLoadoutMain.Loadout_Bin[{i}].Bin_Description", DataType.String, 1);
                                    client.AddTag(BinDiscriptionTag);

                                    stringTagValues BinDescription = ReadString(BinDiscriptionTag, client, 0);
                                    string Name = (BinDescription.Error)?"?":BinDescription.Value+Side;
                                    if (BinDescription.Error) Errors = true;
                                    BinDescriptions.Add(Name);
                                    client.RemoveTag(BinDiscriptionTag);
                                }
                                if (!Errors) BinNamesNotSet = false;
                            }


                            var BinTag = new Tag(ControllerAddress(enumController.mainPlc), "1,3", CpuType.LGX, $"Program:SeedLoadoutMain.Bin_Variety_Id", DataType.String, 29);
                            client.AddTag(BinTag);


                            ReadTagValue BinValues = ReadStringArray(BinTag, client);

                            List<string> BinVarietyItems = (List<string>)BinValues.Value;


                            for (int i = 0; i < BinList.Count; i++)
                            {
                                if (!BinValues.Error)
                                {
                                    if (BinDescriptions.Count > i) BinList[i].BinName = BinDescriptions[i];
                                    BinList[i].Variety_Id = BinVarietyItems[i];
                                    BinList[i].Last_Variety_Update = DateTime.Now;
                                    BinList[i].Read_Variety_Error = false;
                                }
                                else
                                {
                                    BinList[i].Read_Variety_Error = true;
                                }


                            }







                            client.RemoveTag(BinTag);



                            Tag AvailableTag = new Tag(ControllerAddress(enumController.mainPlc), "1,3", CpuType.LGX, "Program:SeedLoadoutMain.Available_For_Clean_Loadout", DataType.Int32, 1);
                            client.AddTag(AvailableTag);
                            bitArrayTagValues Available_For_Clean_Loadout = ReadBooleanFromDint(client, AvailableTag);
                            for (int i = 0; i < BinList.Count; i++)
                            {
                                if (Available_For_Clean_Loadout.Error)
                                {
                                    BinList[i].Available_For_Clean_Load = false;
                                    BinList[i].Read_Use_For_Clean_Error = true;
                                }
                                else
                                {
                                    BinList[i].Available_For_Clean_Load = Available_For_Clean_Loadout.BitValues[i];
                                    BinList[i].Read_Use_For_Clean_Error = false;
                                    BinList[i].Last_Use_For_Clean_Update = DateTime.Now;
                                }

                            }
                            client.RemoveTag(AvailableTag);





                            AvailableTag = new Tag(ControllerAddress(enumController.mainPlc), "1,3", CpuType.LGX, "Program:SeedLoadoutMain.Available_For_Treat_Loadout", DataType.Int32, 1);
                            client.AddTag(AvailableTag);
                            bitArrayTagValues Available_For_Treat_Loadout = ReadBooleanFromDint(client, AvailableTag);
                            for (int i = 0; i < BinList.Count; i++)
                            {
                                if (Available_For_Clean_Loadout.Error)
                                {
                                    BinList[i].Available_For_Treat_Load = false;
                                    BinList[i].Read_Use_For_Treat_Error = true;
                                }
                                else
                                {
                                    BinList[i].Available_For_Treat_Load = Available_For_Treat_Loadout.BitValues[i];
                                    BinList[i].Read_Use_For_Treat_Error = false;
                                    BinList[i].Las_Use_For_Treat_Update = DateTime.Now;
                                }

                            }

                            client.RemoveTag(AvailableTag);

                            if (!disposedValue)
                            {
                                Tag tag = GetTag(enumController.mainPlc, BatchStatusTag(enumBatchType.Clean), LibplctagWrapper.DataType.DINT);
                                client.AddTag(tag);
                                CleanBatchStatus = ReadInt(tag, client, 0);
                                client.RemoveTag(tag);
                            }




                            if (!disposedValue)
                            {
                                Tag tag = new Tag(ControllerAddress(enumController.mainPlc), "1,3", CpuType.LGX, "Program:SeedLoadoutMain.Treat_Batch_Status", DataType.Int32, 1);
                                client.AddTag(tag);
                                TreatBatchStatus = ReadInt(tag, client, 0);
                                client.RemoveTag(tag);
                            }



                        }


                        GettingValues = false;
                        LastUpdate = DateTime.Now;



                        if (!disposedValue) System.Threading.Thread.Sleep(1000);

                        UpdateSeconds = (DateTime.Now - LastBinUpdate).TotalSeconds;
                    }
                    while (!disposedValue);

                }
                finally
                {

                }

            }


        }



        public static stringTagValues ReadString(LibplctagWrapper.Tag tag, Libplctag client, int TagID)
        {

            {

                try
                {




                    stringTagValues Retval = new WW_LGX_PLC.PLC.stringTagValues(TagID);



                    // check that the tag has been added, if it returns pending we have to retry
                    while (client.GetStatus(tag) == Libplctag.PLCTAG_STATUS_PENDING)
                    {
                        Thread.Sleep(100);
                    }


                    // if the status is not ok, we have to handle the error
                    if (client.GetStatus(tag) != Libplctag.PLCTAG_STATUS_OK)
                    {
                        Retval.Error = true;
                        Retval.ErrorMessage = $"Error setting up tag internal state. Error { client.DecodeError(client.GetStatus(tag))}";



                    }


                    var result1 = client.ReadTag(tag, DataTimeout);

                    // Check the read operation result
                    if (result1 != Libplctag.PLCTAG_STATUS_OK)
                    {
                        Retval.Error = true;
                        Retval.ErrorMessage = $"ERROR: Unable to read the data! Got error code {result1}: {client.DecodeError(result1)}";

                    }


                    byte[] Decode = new byte[tag.ElementSize];

                    for (int i = 0; i < tag.ElementSize; i++)
                    {

                        Decode[i] = client.GetUint8Value(tag, i + 4);


                    }

                    Retval.Value = new string(System.Text.Encoding.ASCII.GetString(Decode).ToCharArray());

                    Retval.Value = Retval.Value.Substring(0, Retval.Value.IndexOf("\0"));



                    return Retval;



                }
                finally
                {

                }

            }

        }




        public static ReadTagValue ReadStringArray(LibplctagWrapper.Tag tag, Libplctag client)
        {

            {

                try
                {


                    ReadTagValue Retval = new ReadTagValue();
                    List<string> Values = new List<string>();

                    // check that the tag has been added, if it returns pending we have to retry
                    while (client.GetStatus(tag) == Libplctag.PLCTAG_STATUS_PENDING)
                    {
                        Thread.Sleep(100);
                    }


                    // if the status is not ok, we have to handle the error
                    if (client.GetStatus(tag) != Libplctag.PLCTAG_STATUS_OK)
                    {
                        Retval.Error = true;
                        Retval.ErrorMessage = $"Error setting up tag internal state. Error { client.DecodeError(client.GetStatus(tag))}";

                    }


                    var result1 = client.ReadTag(tag, DataTimeout);

                    // Check the read operation result
                    if (result1 != Libplctag.PLCTAG_STATUS_OK)
                    {
                        Retval.Error = true;
                        Retval.ErrorMessage = $"ERROR: Unable to read the data! Got error code {result1}: {client.DecodeError(result1)}";

                    }
                    else
                    {
                        Values.Clear();


                        for (int tagCount = 0; tagCount < tag.ElementCount; tagCount++)
                        {
                            byte[] Decode = new byte[tag.ElementSize];
                            for (int i = 0; i < tag.ElementSize; i++)
                            {

                                Decode[i] = client.GetUint8Value(tag, (tag.ElementSize * tagCount) + i + 4);


                            }
                            string ID = new string(System.Text.Encoding.ASCII.GetString(Decode).ToCharArray());
                            Values.Add(ID.Substring(0, ID.IndexOf("\0")));
                        }
                    }


                    Retval.Value = Values;



                    return Retval;



                }
                finally
                {

                }

            }

        }



        public static string ReadString(LibplctagWrapper.Tag tag)
        {
            using (var client = new Libplctag())
            {

                try
                {





                    //Loadout_Bin[0].Available_For_Treat_Load
                    client.AddTag(tag);




                    // check that the tag has been added, if it returns pending we have to retry
                    while (client.GetStatus(tag) == Libplctag.PLCTAG_STATUS_PENDING)
                    {
                        Thread.Sleep(100);
                    }


                    // if the status is not ok, we have to handle the error
                    if (client.GetStatus(tag) != Libplctag.PLCTAG_STATUS_OK)
                    {
                        return ($"Error setting up tag internal state. Error { client.DecodeError(client.GetStatus(tag))}\n");


                    }


                    var result1 = client.ReadTag(tag, DataTimeout);

                    // Check the read operation result
                    if (result1 != Libplctag.PLCTAG_STATUS_OK)
                    {
                        return ($"ERROR: Unable to read the data! Got error code {result1}: {client.DecodeError(result1)}\n");

                    }


                    byte[] Decode = new byte[tag.ElementSize];

                    for (int i = 0; i < tag.ElementSize; i++)
                    {

                        Decode[i] = client.GetUint8Value(tag, i + 4);


                    }
                    client.RemoveTag(tag);

                    return new string(System.Text.Encoding.UTF8.GetString(Decode).ToCharArray());



                }
                finally
                {

                }

            }

        }


        public static string GetBinItemNumber(int Bin)
        {
            var tag = new Tag(ControllerAddress(enumController.mainPlc), "1,3", CpuType.LGX, "Program:SeedLoadoutMain.Loadout_Bin[1].Bin_Variety.Item_Number", DataType.String, 1);


            string Response = ReadString(tag);
            return Response;
        }


        public static string WriteIntTag(LibplctagWrapper.Tag tag, int value)
        {
            using (var client = new Libplctag())
            {
                try
                {

                    client.AddTag(tag);

                    // check that the tag has been added, if it returns pending we have to retry
                    while (client.GetStatus(tag) == Libplctag.PLCTAG_STATUS_PENDING)
                    {
                        Thread.Sleep(100);
                    }


                    // if the status is not ok, we have to handle the error
                    if (client.GetStatus(tag) != Libplctag.PLCTAG_STATUS_OK)
                    {
                        return ($"Error setting up tag internal state. Error { client.DecodeError(client.GetStatus(tag))}\n");


                    }

                    client.SetInt32Value(tag, 0, value);
                    int result1 = client.WriteTag(tag, DataTimeout);

                    // Check the read operation result
                    if (result1 != Libplctag.PLCTAG_STATUS_OK)
                    {
                        return ($"ERROR: Unable to read the data! Got error code {result1}: {client.DecodeError(result1)}\n");

                    }


                    client.RemoveTag(tag);

                    return "OK";



                }
                finally
                {

                }

            }

        }





        public static TagResponse WriteRealTag(LibplctagWrapper.Tag tag, float value)
        {
            TagResponse tr = new WW_LGX_PLC.PLC.TagResponse();
            using (var client = new Libplctag())
            {
                try
                {
                    tr = CheckTagStatus(client, tag);


                    if (tr.OK)
                    {
                        client.SetFloat32Value(tag, 0, value);
                        int result = client.WriteTag(tag, DataTimeout);

                        // Check the read operation result
                        if (result != Libplctag.PLCTAG_STATUS_OK)
                        {
                            tr.OK = false;
                            tr.Message = ($"ERROR: Unable to read the data! Got error code {result}: {client.DecodeError(result)}\n");

                        }
                        client.RemoveTag(tag);
                    }
                }
                finally
                {

                }
            }
            return tr;
        }


        #region Tags
        private static Tag GetTag(enumController controller, string TagName, int TagLength)
        {
            return new Tag(ControllerAddress(controller), "1,3", CpuType.LGX, TagName, TagLength, 1);
        }


        #endregion



        #region TagStrings

        private static string TreaterPumpProduct(int Pump)
        {
            return "Program:SeedLoadoutMain.Treater_Pump[" + Pump.ToString() + "].Product.Description";
        }


        private static string Bin_AvailableForCleanLoad(int bin)
        {
            return "Program:SeedLoadoutMain.Loadout_Bin[" + bin.ToString() + "].Available_For_Clean_Load";
        }


        private static string Bin_Description(int bin)
        {
            return "Program:SeedLoadoutMain.Loadout_Bin[" + bin.ToString() + "].Bin_Description";
        }

        private static string Bin_Variety_Name(int bin)
        {
            return "Program:SeedLoadoutMain.Loadout_Bin[" + bin.ToString() + "].Bin_Variety.Name";
        }

        private static string Bin_Variety_Item_Number(int GateIndex)
        {
            return "Program:SeedLoadoutMain.Loadout_Bin[" + GateIndex.ToString() + "].Bin_Variety.Item_Number";
        }

        private static string Bin_Variety_Class(int bin)
        {
            return "Program:SeedLoadoutMain.Loadout_Bin[" + bin.ToString() + "].Bin_Variety.Class";
        }




        private static string Bin_AvailableForTreatLoad(int bin)
        {
            return "Program:SeedLoadoutMain.Loadout_Bin[" + bin.ToString() + "].Available_For_Treat_Load";
        }




        private static string Bin_Use_For_Clean_Loadout(int GateIndex)
        {
            return "Program:SeedLoadoutMain.Loadout_Bin[" + GateIndex.ToString() + "].Use_For_Clean_Loadout";
        }


        private static string Bin_Use_For_Treat_Loadout(int GateIndex)
        {
            return "Program:SeedLoadoutMain.Loadout_Bin[" + GateIndex.ToString() + "].Use_For_Treat_Loadout";
        }

        private static string Bin_Use_For_Transfer_To(int GateIndex)
        {
            return "Program:SeedLoadoutMain.Loadout_Bin[" + GateIndex.ToString() + "].Use_For_Transfer_To";
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~PLC() {
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
        #endregion

    }
}
