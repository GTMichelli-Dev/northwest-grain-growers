using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Weight_Sheet_Transfer
{
    public class Transfer
    {
        public class TransferFileInfo
        {
            public string FilePath;
            public string ErrorMessage;
            public bool Success;
            public TransferFileInfo(string File_Path)
            {
                FilePath = File_Path;
                ErrorMessage = "";
                Success = false;
            }
        }

        public static TransferFileInfo Create_Transfer_File(string FilePath, TransferDataset.TransferValsDataTable TransferValues)
        {
            TransferFileInfo Retval = new Weight_Sheet_Transfer.Transfer.TransferFileInfo(FilePath);
            try
            {

                using (StreamWriter writer = new StreamWriter(FilePath))
                {
                    List<string> Transfers = new List<string>();
                    foreach (TransferDataset.TransferValsRow row in TransferValues)
                    {
                        string LoadNumber;      //9
                        string CustomerNumber;  //7
                        string CropNumber;      //2
                        string Location;        //5
                        string Date;            //8
                        string LoadIO;           //1
                        string Gross;           //11
                        string Lot;             //11
                        string Test;            //1
                        string ApplcationMethod;//2
                        string HaulerNumber;    //7
                        string HaulingRate;     //11
                        string TransferLoc;     //5
                        string TransferTicket;  //9

                        bool IsTransfer = false;

                        LoadNumber = row.WeightSheet.ToString();
                        if (!row.IsCustomer_IDNull())
                        {
                            CustomerNumber = row.Customer_ID.ToString();
                        }
                        else
                        {
                            CustomerNumber = string.Empty;
                        }

                        CropNumber = row.Crop_ID.ToString();
                        Location = row.Location_ID.ToString();
                        Date = row.Time_Date.ToString("yyyyMMdd");


                        //In Or Out
                        if (row.Inbound || !row.IsOutbound_LocationNull())
                        {
                            LoadIO = "I";
                        }
                        else
                        {
                            LoadIO = "O";
                        }


                        //Gross Tare Net
                        Gross = string.Format("{0:N2}", row.Gross).Replace(",","");
                        if (!row.IsLot_IDNull())
                        {
                            Lot = row.Lot_ID.ToString();
                        }
                        else
                        {
                            Lot = string.Empty;
                        }


                        //Test
                        Test = "N";


                        //Transfer Location Transfer Ticket Application Method

                        if (!row.IsOutbound_LocationNull())
                        {
                            TransferLoc = row.Outbound_Location.ToString();
                            TransferTicket = row.WeightSheet.ToString();
                            ApplcationMethod = "TR";
                            IsTransfer = true;
                        }
                        else
                        {
                            TransferLoc = string.Empty;
                            TransferTicket = string.Empty;
                            ApplcationMethod = "  ";
                        }

                        if (!row.IsHauler_IDNull())
                        {
                            HaulerNumber = row.Hauler_ID.ToString();
                        }
                        else
                        {
                            HaulerNumber = string.Empty;
                        }


                        if (!row.IsRateNull())
                        {

                            //string[] rt = row.Rate.Split('.');
                            //string rate = string.Empty.PadLeft(11, '0');
                            //if (rt.Length > 0)
                            //{

                            //    if (rt.Length > 1)
                            //    {
                            //        rt[0] = rt[0].PadLeft(4, '0');
                            //        rt[1] = rt[1].PadRight(7, '0');
                            //        rate = rt[0] + rt[1];
                            //    }
                            //    else
                            //    {
                            //        rate = rt[0].PadLeft(11, '0');
                            //    }

                            //}
                            HaulingRate = row.Rate;// rate;
                        }
                        else
                        {
                            HaulingRate = string.Empty;
                        }





                        StringBuilder Record = new StringBuilder();
                        Record.Append(LoadNumber).Append(",");
                        Record.Append(CustomerNumber).Append(",");
                        Record.Append(string.Empty.PadLeft(9,','));
                        Record.Append(CropNumber).Append(","); 
                        Record.Append(Location).Append(","); 
                        Record.Append(Date).Append(","); 
                        Record.Append(LoadIO).Append(","); 
                        Record.Append(Gross).Append(",");
                        Record.Append("0.00").Append(",");
                        Record.Append(Gross).Append(",");
                        Record.Append(string.Empty.PadLeft(48, ','));
                        Record.Append(Lot).Append(",");
                        Record.Append(Test).Append(",");
                        Record.Append(string.Empty.PadLeft(2, ','));
                        Record.Append(ApplcationMethod).Append(",");
                        Record.Append(string.Empty.PadLeft(1, ','));
                        Record.Append(HaulerNumber).Append(",");
                        Record.Append(HaulingRate).Append(",");
                        Record.Append(string.Empty.PadLeft(24, ','));
                        Record.Append(TransferLoc);
                        if (IsTransfer)
                        {
                            Record.Append(",").Append(LoadNumber);
                        }
                            //Record.Append(TransferTicket).Append(",");
                            //Record.Append(string.Empty.PadLeft(93, ','));

                            writer.WriteLine(Record.ToString());

                        if (IsTransfer )
                        {
                            Record.Clear();
                            Record.Append(LoadNumber).Append(",");
                            Record.Append(CustomerNumber).Append(",");
                            Record.Append(string.Empty.PadLeft(9, ','));
                            Record.Append(CropNumber).Append(",");
                            Record.Append(TransferLoc).Append(",");
                            Record.Append(Date).Append(",");
                            Record.Append("O").Append(",");
                            Record.Append(Gross).Append(",");
                            Record.Append("0.00").Append(",");
                            Record.Append(Gross).Append(",");
                            Record.Append(string.Empty.PadLeft(48, ','));
                            Record.Append(Lot).Append(",");
                            Record.Append(Test).Append(",");
                            Record.Append(string.Empty.PadLeft(2, ','));
                            Record.Append(ApplcationMethod).Append(",");
                            Record.Append(string.Empty.PadLeft(1, ','));
                            Record.Append(HaulerNumber).Append(",");
                            Record.Append(HaulingRate).Append(",");
                            Record.Append(string.Empty.PadLeft(24, ','));
                            Record.Append(",").Append(LoadNumber);
                            writer.WriteLine(Record.ToString());
                        }

                    }
                }
                Retval.Success = true;
            }
            catch (Exception ex)
            {
                Retval.Success = false;
                Retval.ErrorMessage = ex.Message;
            }
            return Retval;
        }

    }


}
