using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using System.Diagnostics;
using System.IO;

namespace Agvantage_Transfer
{
    class AgvantageTransfer : IDisposable
    {


        EventLog eventlog;

        public bool TransferStarted = false;

        public enum enumProcessStatus { Idle, Running, TimedOut };

        public enumProcessStatus ProcessStatus = enumProcessStatus.Idle;

        System.Diagnostics.Process pProcess = new System.Diagnostics.Process();

        TaskFactory TimeoutThread = new TaskFactory();
        TaskFactory StartTransferThread = new TaskFactory();

        string CropsXlsxFile = "Crops.xlsx";
        string CarriersXlsxFile = "Carriers.xlsx";
        string CustomersXlsxFile = "Customers.xlsx";
        string SeedItemMasterXlsxFile = "SeedItemMasterFile.xlsx";
        string SeedItemLocationXlsxFile = "SeedItemLocationPrice.xlsx";
        string SeedDeptXlsxFile = "SeedDept.xlsx";




        public AgvantageTransfer(EventLog ev = null)
        {
            if (ev != null) eventlog = ev;
        }



        private void StartProcess(string BatchFile, string CompletedFilePath, int TimeoutInSeconds, int UpdateInterval)
        {
            do
            {
                try
                {

                    SendMessage("Starting Transfer ", false);
                    if (ProcessStatus != enumProcessStatus.Running)
                    {


                        SendMessage("Deleting Existing Excel Files ", false);
                        DirectoryInfo directory = new DirectoryInfo(CompletedFilePath);
                        foreach (FileInfo file in directory.GetFiles())
                        {
                            file.Delete();
                        }


                        pProcess = new Process();

                        pProcess.OutputDataReceived += PProcess_OutputDataReceived;
                        pProcess.ErrorDataReceived += PProcess_ErrorDataReceived;




                        pProcess.StartInfo.FileName = BatchFile;
                        pProcess.StartInfo.CreateNoWindow = true;
                        pProcess.StartInfo.UseShellExecute = false;
                        pProcess.StartInfo.RedirectStandardOutput = true;
                        pProcess.StartInfo.RedirectStandardError = true;


                        pProcess.StartInfo.WorkingDirectory = Path.GetDirectoryName(BatchFile);

                        SendMessage("Logging on to Agvantage Server", false);
                        pProcess.Start();
                        ProcessStatus = enumProcessStatus.Running;
                        TimeoutThread.StartNew(() => ProcessTimeout(TimeoutInSeconds));

                        pProcess.BeginOutputReadLine();
                        pProcess.BeginErrorReadLine();

                        pProcess.WaitForExit();
                        if (ProcessStatus != enumProcessStatus.TimedOut)
                        {
                            ProcessAgvantageData(CompletedFilePath);
                        }
                        ProcessStatus = enumProcessStatus.Idle;
                        SendMessage("Transfer Complete", false);
                    }
                }
                catch (Exception ex)
                {
                    LogError($"Error Starting StartProcess {ex.Message}", EventLogEntryType.Error);
                }
                DateTime LastUpdate = DateTime.Now;
                SendMessage($"Next Update {LastUpdate.AddMinutes(UpdateInterval)}", false);
                DateTime LastRequestCheck = DateTime.Now;
                do
                {
                    //if ((DateTime.Now - LastRequestCheck).TotalSeconds>10)
                    //{
                    //    LastRequestCheck = DateTime.Now;
                    //    using (AGVWebService.AgvantageTransferService proxy= new AGVWebService.AgvantageTransferService())
                    //    {
                    //        try
                    //        {
                    //            if (proxy.IsTransferRequired()) break;
                    //        }
                    //        catch
                    //        {

                    //        }
                    //    } 
                    //}

                    if ((DateTime.Now - LastUpdate).TotalMinutes > UpdateInterval) break;
                }
                while (!disposedValue);
            }
            while (!disposedValue);

        }



        /// <summary>
        /// BatchFile Needs to be in same folder as IBM acslaunch_win-64.exe
        /// </summary>
        /// <param name="BatchFile">the Batch File With Login Info and Files To Download</param>
        /// <param name="CompletedFilePath">the path where the excel files will be located</param>
        /// <param name="TimeoutInSeconds">the amount of time allowed to get All the data from Agvantage</param>
        /// <param name="UpdateInterval">The Time Between Update In Minuets</param>
        public void StartTransfer(string BatchFile, string CompletedFilePath, int TimeoutInSeconds, int UpdateInterval)
        {

            try
            {
                if (!TransferStarted)
                {
                    TransferStarted = true;

                    StartTransferThread.StartNew(() => StartProcess(BatchFile, CompletedFilePath, TimeoutInSeconds, UpdateInterval));
                }
                else
                {
                    SendMessage("Cannot start transfer because it is already started", true);
                }



            }
            catch (Exception ex)
            {
                LogError($"Error Starting Transfer {ex.Message}", EventLogEntryType.Error);
            }


        }






        public void ProcessTimeout(int SecondsToWait)
        {
            try
            {
                do
                {
                    if (pProcess.HasExited || ProcessStatus != enumProcessStatus.Running) break;
                }
                while ((DateTime.Now - pProcess.StartTime).TotalSeconds < SecondsToWait);
                if (!pProcess.HasExited)
                {
                    KillProcess();
                    SendMessage("Agvantage Connection Timed Out", true);
                }
            }
            catch (Exception ex)
            {
                KillProcess();
                SendMessage($"CropsTimeout Function Error {ex.Message}", true);
            }


        }


        public void KillProcess()
        {
            try
            {
                ProcessStatus = enumProcessStatus.TimedOut;
                pProcess.Kill();


            }
            catch
            {

            }
        }


        private void PProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            //using (AGVWebService.AgvantageTransferService AGV = new AGVWebService.AgvantageTransferService())
            //{
            //    SendMessage(e.Data, false);
            //}
        }
        private void PProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            SendMessage(e.Data, true);
        }

        public void SendMessage(string message, bool isError)
        {
            try
            {
                if (!string.IsNullOrEmpty(message))
                {
                    string PasswordFilter = "/PASSWORD=";
                    if (message.ToUpper().Contains(PasswordFilter)) message = message.Substring(0, message.ToUpper().IndexOf(PasswordFilter)) + PasswordFilter + "**********";
                    System.Diagnostics.Debug.Print(message);
                    using (TransferDataSetTableAdapters.QueriesTableAdapter Q = new TransferDataSetTableAdapters.QueriesTableAdapter())
                    {
                        Q.AddTransferLogMessage(message, isError);
                    }
                }
            }
            catch (Exception ex)
            {
                LogError($"{ex.Message}", EventLogEntryType.Error);
            }

        }


        private void LogError(string Message, EventLogEntryType EventLogType = EventLogEntryType.Information)
        {
            try
            {
                if (eventlog != null) eventlog.WriteEntry(Message, EventLogType);

            }
            catch
            {

            }
        }



        public void ProcessAgvantageData(string xlsxFilePath)
        {
            xlsxFilePath = xlsxFilePath.Trim();
            if (xlsxFilePath.Last() != '\\') xlsxFilePath += "\\";
            SendMessage("Updating Crops", false);
            TransferCrop(xlsxFilePath + CropsXlsxFile);
            System.Threading.Thread.Sleep(10);

            SendMessage("Updating Customers", false);
            TransferCustomer(xlsxFilePath + CustomersXlsxFile);
            System.Threading.Thread.Sleep(10);

            SendMessage("Updating Carriers", false);
            TransferCarrier(xlsxFilePath + CarriersXlsxFile);
            System.Threading.Thread.Sleep(10);

            SendMessage("Updating Seed Master", false);
            TransferSeedMasterFile(xlsxFilePath + SeedItemMasterXlsxFile);
            System.Threading.Thread.Sleep(10);

            SendMessage("Updating Seed Item Location", false);
            TransferSeedItemLocation(xlsxFilePath + SeedItemLocationXlsxFile);
            System.Threading.Thread.Sleep(10);

            SendMessage("Updating Seed Departments", false);
            TransferSeedDept(xlsxFilePath + SeedDeptXlsxFile);
        }


        private string ConvertAgvantageUomToNWDataUom(string UOM)
        {
            if (UOM.ToUpper().Contains("CW"))
            {
                UOM = "H";
            }
            else if (UOM.ToUpper().Contains("TO"))
            {
                UOM = "T";
            }
            else
            {
                UOM = "B";
            }
            return UOM;
        }


        public string SeedItemType(int FLC)
        {
            string Retval = "Other";
            if (FLC == 1 || FLC == 2 || FLC == 3 || FLC == 4 || FLC == 310 || FLC == 311)
            {
                Retval = "Seed";
            }
            else if (FLC == 280)
            {
                Retval = "Chemical";
            }
            return Retval;
        }



        public string SeedItemUmCode(int FLC)
        {
            string Retval = "L";
            if (FLC == 1 || FLC == 2 || FLC == 3 || FLC == 4 || FLC == 310 || FLC == 311)
            {
                Retval = "L";
            }
            else if (FLC == 280)
            {
                Retval = "O";
            }
            return Retval;
        }

        public bool IsNumeric(string value)
        {
            return value.All(char.IsNumber);
        }

        public static bool IsValidEmail(string email)
        {
            try
            {
                string[] emails = email.Split(';');
                foreach (var mailbox in emails)
                {
                    var addr = new System.Net.Mail.MailAddress(mailbox);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }


        public void TransferCrop(string excelFile)
        {
        
            if (!System.IO.File.Exists(excelFile))
            {
                SendMessage(excelFile + " Does Not Exist", true);

            }
            else
            {
                try
                {
                    using (TransferDataSetTableAdapters.QueriesTableAdapter Q = new TransferDataSetTableAdapters.QueriesTableAdapter())
                    {
                        Q.DeleteAgvantageCrops();


                        using (TransferDataSetTableAdapters.AgvantageCropsTableAdapter cropsTableAdapter = new TransferDataSetTableAdapters.AgvantageCropsTableAdapter())
                        {
                            using (TransferDataSet nwDataset = new TransferDataSet())
                            {

                                var workbook = new XLWorkbook(excelFile);
                                var ws1 = workbook.Worksheet(1);
                                SendMessage($"{ws1.Rows().Count() - 1} Crop Records Recieved From Agvantage", false);
                                foreach (IXLRow row in ws1.Rows())
                                {
                                    if (IsNumeric(row.Cell(1).Value.ToString()))
                                    {
                                        var Id = Convert.ToInt32(row.Cell(1).Value.ToString());
                                        var description = row.Cell(2).Value.ToString();
                                        var PoundsPerUnit = Convert.ToDecimal(row.Cell(3).Value.ToString());
                                        var UOM = ConvertAgvantageUomToNWDataUom(row.Cell(4).Value.ToString());


                                        TransferDataSet.AgvantageCropsRow crop;


                                        var Seed = Id > 79;

                                        crop = nwDataset.AgvantageCrops.NewAgvantageCropsRow();
                                        crop.UID = Guid.NewGuid();
                                        crop.Id = Id;
                                        crop.Description = description;
                                        crop.Unit_Of_Measure = UOM;
                                        crop.Pound_Per_Bushel = (float)PoundsPerUnit;
                                        crop.Use_At_Elevator = !Seed;
                                        crop.Use_At_Seed_Mill = Seed;
                                        crop.Active = true;
                                        nwDataset.AgvantageCrops.AddAgvantageCropsRow(crop);

                                    }
                                }


                                try
                                {
                                    cropsTableAdapter.Update(nwDataset);
                                    Q.UpdateCropsFromAgvantage();
                                }
                                catch (Exception ex)
                                {
                                    SendMessage($"Error Updating Crops {ex.Message}", true);
                                   
                                }
                            }
                        }
                    }
                }



                catch (Exception ex)
                {
                    SendMessage("Error Getting Crops " + ex.Message, false);


                }

                //SendMessage($"{RecordsTransfered} Crop Records Transfered", false);

            }
        }


        public void TransferCustomer(string excelFile)
        {

            int RecordsTransfered = 0;
            if (!System.IO.File.Exists(excelFile))
            {
                SendMessage(excelFile + " Does Not Exist", true);

            }
            else
            {
                try
                {
                    using (TransferDataSetTableAdapters.QueriesTableAdapter Q = new TransferDataSetTableAdapters.QueriesTableAdapter())
                    {
                        Q.DeleteAgvantageProducers();

                        using (TransferDataSetTableAdapters.AgvantageProducersTableAdapter producersTableAdapter = new TransferDataSetTableAdapters.AgvantageProducersTableAdapter())
                        {
                            using (TransferDataSet nwDataset = new TransferDataSet())
                            {

                                var workbook = new XLWorkbook(excelFile);
                                var ws1 = workbook.Worksheet(1);
                                SendMessage($"{ws1.Rows().Count() - 1} Customer Records Recieved From Agvantage", false);
                                foreach (IXLRow row in ws1.Rows())
                                {
                                    if (IsNumeric(row.Cell(1).Value.ToString()))
                                    {

                                        int Id = Convert.ToInt32(row.Cell(1).Value.ToString());
                                        string description = row.Cell(2).Value.ToString();
                                        bool Active = (row.Cell(3).Value.ToString() == "I" || row.Cell(1).Value.ToString() == "D") ? false : true;
                                        string Email = row.Cell(4).Value.ToString();

                                        var producer = nwDataset.AgvantageProducers.NewAgvantageProducersRow();

                                        producer.UID = Guid.NewGuid();
                                        producer.Id = Id;
                                        producer.Active = Active;
                                        producer.Description = description;
                                       
                                        producer.Print_WS = true;
                                        producer.Company_Name = row.Cell(5).Value.ToString();
                                        producer.Customer_Name1 = row.Cell(6).Value.ToString();
                                        producer.Customer_Name2 = row.Cell(7).Value.ToString();
                                        producer.Address1 = row.Cell(8).Value.ToString();
                                        producer.Address2 = row.Cell(9).Value.ToString();
                                        producer.City = row.Cell(10).Value.ToString();
                                        producer.State = row.Cell(11).Value.ToString();
                                        producer.Zip1 = row.Cell(12).Value.ToString();
                                        producer.Zip2 = row.Cell(13).Value.ToString();
                                        producer.Home_Phone = row.Cell(14).Value.ToString();
                                        producer.Work_Phone = row.Cell(15).Value.ToString();
                                        producer.Member = row.Cell(16).Value.ToString();
                                        producer.Phone = row.Cell(17).Value.ToString();
                                        producer.Member = row.Cell(18).Value.ToString();
                                      
                                        producer.Email_WS = true;
                                        if (!string.IsNullOrWhiteSpace(Email) && IsValidEmail(Email))
                                        {
                                            producer.Email_Address = Email;
                                            producer.Email_WS = true;
                                        }
                                        else
                                        {
                                            producer.SetEmail_AddressNull();
                                            producer.Email_WS = false;
                                        }
                                        nwDataset.AgvantageProducers.AddAgvantageProducersRow(producer);
                                        RecordsTransfered += 1;
                                    }
                                }
                                try
                                {
                                    producersTableAdapter.Update(nwDataset);
                                    Q.UpdateProducersFromAgvantage();
                                }
                                catch (Exception ex)
                                {
                                    SendMessage($"Error Updating Producers {ex.Message}", true);
                                    RecordsTransfered = 0;
                                }
                            }
                        }
                    }
                }



                catch (Exception ex)
                {
                    SendMessage("Error Getting Producers " + ex.Message, false);


                }

                //SendMessage($"{RecordsTransfered} Crop Records Transfered", false);

            }

        }





        public void TransferCarrier(string excelFile)
        {
            int RecordsTransfered = 0;
            if (!System.IO.File.Exists(excelFile))
            {
                SendMessage(excelFile + " Does Not Exist", true);

            }
            else
            {

                try
                {
                    using (TransferDataSetTableAdapters.QueriesTableAdapter Q = new TransferDataSetTableAdapters.QueriesTableAdapter())
                    {

                        Q.DeleteAgvantageCarriers();
                        using (TransferDataSetTableAdapters.AgvantageCarriersTableAdapter carriersTableAdapter = new TransferDataSetTableAdapters.AgvantageCarriersTableAdapter())
                        {
                            using (TransferDataSet nwDataset = new TransferDataSet())
                            {

                                var workbook = new XLWorkbook(excelFile);
                                var ws1 = workbook.Worksheet(1);
                                SendMessage($"{ws1.Rows().Count() - 1} Carrier Records Recieved From Agvantage", false);
                                foreach (IXLRow row in ws1.Rows())
                                {
                                    if (IsNumeric(row.Cell(1).Value.ToString()))
                                    {
                                        var Id = Convert.ToInt32(row.Cell(1).Value.ToString());
                                        var description = row.Cell(2).Value.ToString();


                                        TransferDataSet.AgvantageCarriersRow carrier;



                                        carrier = nwDataset.AgvantageCarriers.NewAgvantageCarriersRow();
                                        carrier.UID = Guid.NewGuid();
                                        carrier.Id = Id;
                                        carrier.Description = description;

                                        carrier.Active = true;
                                        nwDataset.AgvantageCarriers.AddAgvantageCarriersRow(carrier);
                                    }

                                    RecordsTransfered++;


                                }
                          
                            try
                            {
                                carriersTableAdapter.Update(nwDataset);
                                    Q.UpdateCarriersFromAgvantage();
                            }
                            catch (Exception ex)
                            {
                                SendMessage($"Error Updating Carriers {ex.Message}", true);
                                RecordsTransfered = 0;
                            }

                            }
                        }


                    }
                }
                catch (Exception ex)
                {
                    SendMessage("Error Getting Carriers " + ex.Message, false);


                }

                //SendMessage($"{RecordsTransfered} Carrier Records Transfered", false);

            }
        }






        public void TransferSeedMasterFile(string excelFile)
        {
            int RecordsTransfered = 0;
            if (!System.IO.File.Exists(excelFile))
            {
                SendMessage(excelFile + " Does Not Exist", true);

            }
            else
            {

                try
                {
                    using (TransferDataSetTableAdapters.QueriesTableAdapter Q= new TransferDataSetTableAdapters.QueriesTableAdapter()) {
                        Q.DeleteSeedAgvantageItems();
                    using (TransferDataSetTableAdapters.AgvantageItemsTableAdapter ItemsTableAdapter = new TransferDataSetTableAdapters.AgvantageItemsTableAdapter())
                    {
                            using (TransferDataSet SeedDataSet = new TransferDataSet())
                            {
                                
                                var workbook = new XLWorkbook(excelFile);
                                var ws1 = workbook.Worksheet(1);
                                SendMessage($"{ws1.Rows().Count() - 1} Seed Master Records Recieved From Agvantage", false);
                                foreach (IXLRow row in ws1.Rows())
                                {
                                    if (IsNumeric(row.Cell(1).Value.ToString()))
                                    {
                                        var Id = Convert.ToInt32(row.Cell(1).Value.ToString());

                                        var description = row.Cell(2).Value.ToString();
                                        var FLC = Convert.ToInt32(row.Cell(3).Value.ToString());
                                        var Dept = Convert.ToInt32(row.Cell(4).Value.ToString());
                                        var Inactive = (row.Cell(5).Value.ToString() == "I") ? true : false;

                                        TransferDataSet.AgvantageItemsRow spItem;
                                        Debug.Print(description);
                                        if (description == "TOTES")
                                        {
                                            Debug.Print(row.Cell(1).ToString());
                                        }

                                        {
                                            spItem = SeedDataSet.AgvantageItems.NewAgvantageItemsRow();
                                            spItem.UID = Guid.NewGuid();
                                            spItem.ID = Id;
                                            spItem.Description = description;
                                            spItem.FLC = FLC;
                                            spItem.Inactive = Inactive;
                                            spItem.ItemType = SeedItemType(FLC);
                                            spItem.UOMCode = SeedItemUmCode(FLC);
                                            spItem.NotInUse = false;
                                            spItem.Dept = Dept;
                                            SeedDataSet.AgvantageItems.AddAgvantageItemsRow(spItem);
                                        }

                                        RecordsTransfered++;

                                    }
                                }
                                try
                                {
                                    ItemsTableAdapter.Update(SeedDataSet);
                                    Q.UpdateItemsFromAgvantage();
                                    GetSeedTransferLogs();
                                }
                                catch (Exception ex)
                                {
                                    SendMessage($"Error Updating Seed Master {ex.Message}", true);
                                    RecordsTransfered = 0;
                                }
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    SendMessage("Error Getting Seed Master " + ex.Message, false);


                }

                //SendMessage($"{RecordsTransfered} Seed Master Records Transfered", false);

            }
        }


        public void GetSeedTransferLogs()
        {
            try
            {
                using (TransferDataSetTableAdapters.AgvantageTransferLogTableAdapter agvantageTransferLogTableAdapter = new TransferDataSetTableAdapters.AgvantageTransferLogTableAdapter())
                {


                    using (TransferDataSetTableAdapters.AgvantageSeedTransferLogTableAdapter agvantageSeedTransferLogTableAdapter = new TransferDataSetTableAdapters.AgvantageSeedTransferLogTableAdapter())
                    {
                        using (TransferDataSet transferDataSet = new TransferDataSet())
                        {
                            agvantageSeedTransferLogTableAdapter.Fill(transferDataSet.AgvantageSeedTransferLog);
                            foreach (var row in transferDataSet.AgvantageSeedTransferLog)
                            {
                                var logRow = transferDataSet.AgvantageTransferLog.NewAgvantageTransferLogRow();
                                logRow.ItemArray = row.ItemArray;
                                transferDataSet.AgvantageTransferLog.AddAgvantageTransferLogRow(logRow);
                            }
                            agvantageTransferLogTableAdapter.Update(transferDataSet.AgvantageTransferLog);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }


        public void TransferSeedItemLocation(string excelFile)
        {
            int RecordsTransfered = 0;
            if (!System.IO.File.Exists(excelFile))
            {
                SendMessage(excelFile + " Does Not Exist", true);

            }
            else
            {

                try
                {
                    using (TransferDataSetTableAdapters.QueriesTableAdapter Q = new TransferDataSetTableAdapters.QueriesTableAdapter())
                    {
                        Q.DeleteAgvantageItem_Location();
                        using (TransferDataSetTableAdapters.AgvantageItem_LocationTableAdapter agvantageItem_LocationTableAdapter = new TransferDataSetTableAdapters.AgvantageItem_LocationTableAdapter())
                        {


                            using (TransferDataSet nwDataset = new TransferDataSet())
                            {
                                using (TransferDataSetTableAdapters.LocationsTableAdapter locationsTableAdapter = new TransferDataSetTableAdapters.LocationsTableAdapter())
                                {
                                    locationsTableAdapter.Fill(nwDataset.Locations);
                                }
                               
                                var workbook = new XLWorkbook(excelFile);
                                var ws1 = workbook.Worksheet(1);
                                SendMessage($"{ws1.Rows().Count() - 1} Seed Location Items Records Recieved From Agvantage", false);
                                foreach (IXLRow row in ws1.Rows())
                                {
                                    if (IsNumeric(row.Cell(1).Value.ToString()))
                                    {

                                        var Id = Convert.ToInt32(row.Cell(1).Value.ToString());
                                        
                                        {
                                            var LocationID = Convert.ToInt32(row.Cell(2).Value.ToString());
                                            decimal Price = 0;
                                            decimal.TryParse(row.Cell(6).Value.ToString(), out Price);
                                            int StoreLocation = 0;
                                            int.TryParse(row.Cell(8).Value.ToString(), out StoreLocation);
                                            if (nwDataset.Locations.Any(x => x.ID == LocationID))
                                            {
                                                TransferDataSet.AgvantageItem_LocationRow Item_LocationRow ;

                                              
            


                                                    Item_LocationRow = nwDataset.AgvantageItem_Location.NewAgvantageItem_LocationRow();
                                                    Item_LocationRow.UID = Guid.NewGuid();
                                                    Item_LocationRow.Id = Id;
                                                    Item_LocationRow.Location_ID = LocationID;
                                                    Item_LocationRow.Price = Price;
                                                    Item_LocationRow.Inactive = false;
                                                    Item_LocationRow.NotInUse = false;

                                                    nwDataset.AgvantageItem_Location.AddAgvantageItem_LocationRow(Item_LocationRow);
                                          

                                                RecordsTransfered++;


                                            }
                                        }

                                    }
                                }
                                try
                                {
                                    agvantageItem_LocationTableAdapter.Update(nwDataset);
                                    Q.UpdateItem_LocationFromAgvantage();
                                    GetSeedTransferLogs();

                                }
                                catch (Exception ex)
                                {
                                    SendMessage($"Error Updating Seed Location Items  {ex.Message}", true);
                                    RecordsTransfered = 0;
                                }

                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    SendMessage("Error Getting Seed Location Items  " + ex.Message, false);


                }

                //SendMessage($"{RecordsTransfered} Seed Location Items Records Transfered", false);

            }
        }

















        public void TransferSeedDept(string excelFile)
        {
            int RecordsTransfered = 0;
            if (!System.IO.File.Exists(excelFile))
            {
                SendMessage(excelFile + " Does Not Exist", true);

            }
            else
            {

                try
                {
                    using (TransferDataSetTableAdapters.QueriesTableAdapter Q= new TransferDataSetTableAdapters.QueriesTableAdapter())
                    {

                        Q.DeleteAgvantageSeedDepartments();

                        using (TransferDataSetTableAdapters.AgvantageSeed_DepartmentsTableAdapter seed_DepartmentsTableAdapter = new TransferDataSetTableAdapters.AgvantageSeed_DepartmentsTableAdapter())
                        {
                            using (TransferDataSet SeedDataSet = new TransferDataSet())
                            {
                                seed_DepartmentsTableAdapter.Fill(SeedDataSet.AgvantageSeed_Departments);
                                var workbook = new XLWorkbook(excelFile);
                                var ws1 = workbook.Worksheet(1);
                                SendMessage($"{ws1.Rows().Count() - 1} Seed Dept Records Recieved From Agvantage", false);
                                foreach (IXLRow row in ws1.Rows())
                                {
                                    if (IsNumeric(row.Cell(1).Value.ToString()))
                                    {
                                        var Id = Convert.ToInt32(row.Cell(1).Value.ToString());

                                        var description = row.Cell(2).Value.ToString();

                                        TransferDataSet.AgvantageSeed_DepartmentsRow spItem;

                                       
                                        {
                                            spItem = SeedDataSet.AgvantageSeed_Departments.NewAgvantageSeed_DepartmentsRow();
                                            spItem.UID = Guid.NewGuid();
                                            spItem.Id = Id;
                                            spItem.Description = description;
                                            spItem.Spring_Wheat = false;
                                            spItem.Not_Used = false;

                                            SeedDataSet.AgvantageSeed_Departments.AddAgvantageSeed_DepartmentsRow(spItem);

                                        }

                                        RecordsTransfered++;


                                    }
                                }
                                try
                                {
                                    seed_DepartmentsTableAdapter.Update(SeedDataSet);
                                    Q.UpdateSeed_DepartmentsFromAgvantage();
                                    GetSeedTransferLogs();
                                }
                                catch (Exception ex)
                                {
                                    SendMessage($"Error Updating Seed Departments {ex.Message}", true);
                                    RecordsTransfered = 0;
                                }

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    SendMessage("Error Getting Seed Dept " + ex.Message, false);


                }

                //SendMessage($"{RecordsTransfered} Seed Dept Records Transfered", false);

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
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~AgvantageTransfer() {
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
