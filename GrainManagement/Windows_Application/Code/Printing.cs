using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrystalDecisions;
using CrystalDecisions.CrystalReports;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;

// Check Crystal Decisions and manke sure all refrences are installed

namespace NWGrain
{
    class Printing
    {





        public static void CheckPrinter(string Scale, ref string Printer)
        {
            if ((string.IsNullOrEmpty(Scale) || Scale.ToUpper().Contains("MANUAL")) && string.IsNullOrEmpty(Printer))
            {
                Printer = Settings.ManualWeightPrinter;
            }
        }





        public static void PrintClosedLotReport(DateTime StartDate, DateTime EndDate ,int Location_Id)
        {






            using (PrintingTicket frm = new PrintingTicket(string.Format("Printing Closed Lots Report For Location {0}", Location_Id)))
            {
                frm.Show();
                System.Windows.Forms.Application.DoEvents();
                string Printer = Settings.workStation_SetupRow.Report_Printer;
                try
                {
                    using (Reports.ClosedLotsByDateRange ClosedLotsReport = new Reports.ClosedLotsByDateRange())
                    {



                        //see if the printer is valid 
                        bool ValidPrinter = false;

                        System.Drawing.Printing.PrinterSettings settings = new System.Drawing.Printing.PrinterSettings();
                        string DefaultPrinter = settings.PrinterName;

                        foreach (string InstalledPrinter in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                        {
                            if (Printer.ToUpper() == InstalledPrinter.ToUpper())
                            {
                                ValidPrinter = true;
                                break;
                            }
                        }
                        if (!ValidPrinter) // if not valid use the system default printer
                        {
                            Printer = DefaultPrinter;
                        }
                        {

                            using (ReportDataSetTableAdapters.ClosedLotsByDateRangeTableAdapter closedLotsByDateRangeTableAdapter = new ReportDataSetTableAdapters.ClosedLotsByDateRangeTableAdapter())
                            {
                                using (ReportDataSet reportDataset = new ReportDataSet())
                                {
                                    closedLotsByDateRangeTableAdapter.Fill(reportDataset.ClosedLotsByDateRange, StartDate, EndDate, Location_Id);



                                    ClosedLotsReport.SetDataSource(reportDataset);
                                    string DateRangeHdr = (StartDate.Date == EndDate.Date) ? $"For {StartDate.ToShortDateString() }" : $"From {StartDate.ToShortDateString()  } To {EndDate.ToShortDateString() }";
                                    string LocationHdr = $"{Settings.CurrentWorkStationLocationRow.Description }-{Settings.CurrentWorkStationLocationRow.Id} ";
                                    ClosedLotsReport.SetParameterValue(0, LocationHdr);
                                    ClosedLotsReport.SetParameterValue(1, DateRangeHdr);
                                    CrystalDecisions.ReportAppServer.Controllers.PrintReportOptions printReportOptions = new CrystalDecisions.ReportAppServer.Controllers.PrintReportOptions();
                                    CrystalDecisions.ReportAppServer.Controllers.PrintOutputController printOutputController = new CrystalDecisions.ReportAppServer.Controllers.PrintOutputController();
                                    CrystalDecisions.ReportAppServer.ClientDoc.ISCDReportClientDocument rptClientDoc;
                                    rptClientDoc = ClosedLotsReport.ReportClientDocument;
                                    printReportOptions.PrinterName = Printer;
                                    //printReportOptions.AddPrinterPageRange(1, 1);

                                    rptClientDoc.PrintOutputController.PrintReport(printReportOptions);

                                }
                                System.Threading.Thread.Sleep(1000);
                            }



                        }



                    }

                }
                catch (Exception ocrap)
                {
                    Alert.Show("Looks Like The Closed Lots Report Failed To Print", "Error", false);
                    Logging.Add_System_Log("Printing.PrintClosedLotReport StartDate<" + StartDate.ToString() + ">EndDate<+" + EndDate + "> Location Id <" + Location_Id.ToString() + ">", ocrap.Message);

                }
            }















        }





        public static void PrintInbound_InyardTicket(Guid LoadUID, string Scale = "", string Printer = "")
        {
            CheckPrinter(Scale, ref Printer);
            using (PrintingTicket frm = new PrintingTicket())
            {
                try
                {
                    frm.Show();
                    System.Windows.Forms.Application.DoEvents();
                    using (WS.WSSoapClient proxy = new WS.WSSoapClient())
                    {
                        using (NWDatasetTableAdapters.LoadsTableAdapter LoadsTableAdapter = new NWDatasetTableAdapters.LoadsTableAdapter())
                        {
                            using (NWDataset.LoadsDataTable Loads = new NWDataset.LoadsDataTable())
                            {
                                LoadsTableAdapter.FillByLoad_UID(Loads, LoadUID);
                                proxy.Print_Inbound_Inyard_Ticket(Loads[0].Load_Id, Settings.Location_Id, Scale, Printer);
                            }
                        }
                    }
                }
                catch (Exception ocrap)
                {
                    System.Windows.Forms.MessageBox.Show("Cannot Print Ticket" + System.Environment.NewLine + ocrap.Message);
                    Logging.Add_System_Log("Printing.PrintInboundTicket Load UID<" + LoadUID.ToString() + "> Printer Name<" + Printer + ">", ocrap.Message);
                }
            }
        }

        public static void PrintTransfer_InyardTicket(Guid LoadUID, string Scale = "", string Printer = "")
        {
            CheckPrinter(Scale, ref Printer);
            using (PrintingTicket frm = new PrintingTicket())
            {



                try
                {
                    frm.Show();
                    System.Windows.Forms.Application.DoEvents();
                    using (WS.WSSoapClient proxy = new WS.WSSoapClient())
                    {
                        using (NWDatasetTableAdapters.LoadsTableAdapter LoadsTableAdapter = new NWDatasetTableAdapters.LoadsTableAdapter())
                        {
                            using (NWDataset.LoadsDataTable Loads = new NWDataset.LoadsDataTable())
                            {
                                LoadsTableAdapter.FillByLoad_UID(Loads, LoadUID);
                                proxy.Print_Transfer_Inyard_Ticket(Loads[0].Load_Id, Settings.Location_Id, Scale, Printer);
                            }
                        }
                    }

                }
                catch (Exception ocrap)
                {
                    System.Windows.Forms.MessageBox.Show("Cannot Print Ticket" + System.Environment.NewLine + ocrap.Message);

                    Logging.Add_System_Log("Printing.PrintTransferInyardTicket Load UID<" + LoadUID.ToString() + "> Printer Name<" + Printer + ">", ocrap.Message);
                }
            }
        }

        public static void PrintTransfer_FinalTicket(Guid LoadUID, string Scale = "", string Printer = "")
        {
            CheckPrinter(Scale, ref Printer);
            using (PrintingTicket frm = new PrintingTicket())
            {



                try
                {
                    frm.Show();
                    System.Windows.Forms.Application.DoEvents();
                    using (WS.WSSoapClient proxy = new WS.WSSoapClient())
                    {
                        using (NWDatasetTableAdapters.LoadsTableAdapter LoadsTableAdapter = new NWDatasetTableAdapters.LoadsTableAdapter())
                        {
                            using (NWDataset.LoadsDataTable Loads = new NWDataset.LoadsDataTable())
                            {
                                LoadsTableAdapter.FillByLoad_UID(Loads, LoadUID);
                                proxy.Print_Transfer_Final_Ticket(Loads[0].Load_Id, Settings.Location_Id, Scale, Printer);
                            }
                        }
                    }

                }
                catch (Exception ocrap)
                {
                    System.Windows.Forms.MessageBox.Show("Cannot Print Ticket" + System.Environment.NewLine + ocrap.Message);

                    Logging.Add_System_Log("Printing.PrintInboundTicket Load UID<" + LoadUID.ToString() + "> Printer Name<" + Printer + ">", ocrap.Message);
                }
            }
        }

        public static void PrintOutbound_InyardTicket(Guid LoadUID, string Scale = "", bool OnlyOne = false, string Printer = "")
        {
            CheckPrinter(Scale, ref Printer);

            using (PrintingTicket frm = new PrintingTicket())
            {



                try
                {
                    frm.Show();
                    System.Windows.Forms.Application.DoEvents();
                    using (WS.WSSoapClient proxy = new WS.WSSoapClient())
                    {
                        using (NWDatasetTableAdapters.vw_Outbound_LoadTableAdapter vw_Outbound_LoadTableAdapter = new NWDatasetTableAdapters.vw_Outbound_LoadTableAdapter())
                        {
                            using (NWDataset.vw_Outbound_LoadDataTable vw_Outbound_Load = new NWDataset.vw_Outbound_LoadDataTable())
                            {
                                vw_Outbound_LoadTableAdapter.FillByLoad_UID(vw_Outbound_Load, LoadUID);
                                proxy.Print_Outbound_Inyard_Ticket(vw_Outbound_Load[0].Load_Id, Settings.Location_Id, Scale, OnlyOne, Printer);
                            }
                        }
                    }

                }
                catch (Exception ocrap)
                {
                    System.Windows.Forms.MessageBox.Show("Cannot Print Ticket" + System.Environment.NewLine + ocrap.Message);

                    Logging.Add_System_Log("Printing.PrintInboundTicket Load UID<" + LoadUID.ToString() + "> Printer Name<" + Printer + ">", ocrap.Message);
                }
            }
        }

        public static void PrintInbound_FinalTicket(Guid LoadUID, string Scale = "", string Printer = "")
        {
            CheckPrinter(Scale, ref Printer);
            using (PrintingTicket frm = new PrintingTicket())
            {



                try
                {
                    frm.Show();
                    System.Windows.Forms.Application.DoEvents();
                    using (WS.WSSoapClient proxy = new WS.WSSoapClient())
                    {
                        using (NWDatasetTableAdapters.LoadsTableAdapter LoadsTableAdapter = new NWDatasetTableAdapters.LoadsTableAdapter())
                        {
                            using (NWDataset.LoadsDataTable Loads = new NWDataset.LoadsDataTable())
                            {
                                LoadsTableAdapter.FillByLoad_UID(Loads, LoadUID);
                                proxy.Print_Inbound_Final_Ticket(Loads[0].Load_Id, Settings.Location_Id, Scale, Printer);
                            }
                        }
                    }

                }
                catch (Exception ocrap)
                {
                    System.Windows.Forms.MessageBox.Show("Cannot Print Ticket" + System.Environment.NewLine + ocrap.Message);

                    Logging.Add_System_Log("Printing.PrintOutboundTicket Load UID<" + LoadUID.ToString() + "> Printer Name<" + Printer + ">", ocrap.Message);
                }
            }
        }









        public static void PrintOutbound_Final_Kiosk_Ticket(Guid LoadUID, string Scale = "", string Printer = "", bool Only_One = false)
        {
            CheckPrinter(Scale, ref Printer);
            System_Log.Log_Message("Printing Final Outbound Ticket Scale=<" + Scale + "> Printer = <" + Printer + ">", "");

            using (PrintingTicket frm = new PrintingTicket())
            {



                try
                {
                    frm.Show();
                    System.Windows.Forms.Application.DoEvents();
                    using (WS.WSSoapClient proxy = new WS.WSSoapClient())
                    {
                        using (NWDatasetTableAdapters.LoadsTableAdapter LoadsTableAdapter = new NWDatasetTableAdapters.LoadsTableAdapter())
                        {
                            using (NWDataset.LoadsDataTable Loads = new NWDataset.LoadsDataTable())
                            {
                                LoadsTableAdapter.FillByLoad_UID(Loads, LoadUID);
                                proxy.Print_Outbound_Final_Ticket(Loads[0].Load_Id, Settings.Location_Id, Scale, Only_One, Printer);
                            }
                        }
                    }

                }
                catch (Exception ocrap)
                {
                    System.Windows.Forms.MessageBox.Show("Cannot Print Ticket" + System.Environment.NewLine + ocrap.Message);

                    Logging.Add_System_Log("Printing.PrintOutboundTicket Load UID<" + LoadUID.ToString() + "> Printer Name<" + Printer + ">", ocrap.Message);
                }
            }
        }

        public static void PrintTodays_Outbound_Tickets()
        {
            using (NWDataset nWDataset = new NWDataset())
            {
                using (NWDatasetTableAdapters.vw_Outbound_LoadTableAdapter vw_Outbound_LoadTableAdapter = new NWDatasetTableAdapters.vw_Outbound_LoadTableAdapter())
                {
                    if (vw_Outbound_LoadTableAdapter.FillByTodaysFinishedLoads(nWDataset.vw_Outbound_Load, Settings.Location_Id) > 0)
                    {
                        foreach (NWDataset.vw_Outbound_LoadRow row in nWDataset.vw_Outbound_Load)
                        {
                            Printing.PrintOutbound_Final_Office_Ticket(row.Load_UID);
                        }

                    }
                    else
                    {
                        Alert.Show("No Outbound Tickets Today");
                    }
                }
            }
        }



        public static void PrintReportPrinterTest(string PrinterName)
        {

        
        
        



                try
                {


                    using (Reports.TestTicket testTicket = new Reports.TestTicket())
                    {

                    testTicket.SetParameterValue("PrinterName", PrinterName);
                        
                                        // The New Way to Get the Prenter Name Set
                                        CrystalDecisions.ReportAppServer.Controllers.PrintReportOptions printReportOptions = new CrystalDecisions.ReportAppServer.Controllers.PrintReportOptions();
                                        CrystalDecisions.ReportAppServer.Controllers.PrintOutputController printOutputController = new CrystalDecisions.ReportAppServer.Controllers.PrintOutputController();
                                        CrystalDecisions.ReportAppServer.ClientDoc.ISCDReportClientDocument rptClientDoc;
                                        rptClientDoc = testTicket.ReportClientDocument;

                                        printReportOptions.PrinterName = PrinterName;
                                        rptClientDoc.PrintOutputController.PrintReport(printReportOptions);


                    }
                }
                catch (Exception ocrap)
                {
                    System.Windows.Forms.MessageBox.Show("Cannot Print Ticket" + System.Environment.NewLine + ocrap.Message);

                    Logging.Add_System_Log("PrintingPrintReportPrinterTest> Printer Name<" + PrinterName + ">", ocrap.Message);
                }
        
        }



        public static void printSampleLabelTest(string PrinterName)
        {

            try
            {


                using (Reports.TestGradeTicket testTicket = new Reports.TestGradeTicket())
                {

                    testTicket.SetParameterValue("PrinterName", PrinterName);

                    // The New Way to Get the Prenter Name Set
                    CrystalDecisions.ReportAppServer.Controllers.PrintReportOptions printReportOptions = new CrystalDecisions.ReportAppServer.Controllers.PrintReportOptions();
                    CrystalDecisions.ReportAppServer.Controllers.PrintOutputController printOutputController = new CrystalDecisions.ReportAppServer.Controllers.PrintOutputController();
                    CrystalDecisions.ReportAppServer.ClientDoc.ISCDReportClientDocument rptClientDoc;
                    rptClientDoc = testTicket.ReportClientDocument;

                    printReportOptions.PrinterName = PrinterName;
                    rptClientDoc.PrintOutputController.PrintReport(printReportOptions);


                }
            }
            catch (Exception ocrap)
            {
                System.Windows.Forms.MessageBox.Show("Cannot Print Ticket" + System.Environment.NewLine + ocrap.Message);

                Logging.Add_System_Log("PrintingPrintReportPrinterTest> Printer Name<" + PrinterName + ">", ocrap.Message);
            }

        }


        public static void PrintOutbound_Final_Office_Ticket(Guid LoadUID, string Printer = "")
        {
            bool PrintTicket = true;
            using (PrintingTicket frm = new PrintingTicket())
            {



                try
                {


                    using (Reports.Outbound_Final_Ticket_2_Part Outbound_Office_Ticket = new Reports.Outbound_Final_Ticket_2_Part())
                    {


                        using (NWDataset NWDataset = new NWDataset())
                        {


                            if (string.IsNullOrEmpty(Printer))
                            {

                                Printer = Settings.workStation_SetupRow.Report_Printer;


                            }
                            //see if the printer is valid 
                            bool ValidPrinter = false;

                            System.Drawing.Printing.PrinterSettings settings = new System.Drawing.Printing.PrinterSettings();
                            string DefaultPrinter = settings.PrinterName;

                            foreach (string InstalledPrinter in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                            {
                                if (Printer.ToUpper() == InstalledPrinter.ToUpper())
                                {
                                    ValidPrinter = true;
                                    break;
                                }

                            }
                            if (!ValidPrinter) // if not valid use the system default printer
                            {
                                Printer = DefaultPrinter;
                            }
                            if (PrintTicket)
                            {


                                using (NWDatasetTableAdapters.vw_Outbound_LoadTableAdapter vw_Outbound_LoadTableAdapter = new NWDatasetTableAdapters.vw_Outbound_LoadTableAdapter())
                                {
                                    if (vw_Outbound_LoadTableAdapter.FillByLoad_UID(NWDataset.vw_Outbound_Load, LoadUID) > 0)
                                    {
                                        frm.SetPrompt(String.Format("Printing Outbound Ticket:{0}", NWDataset.vw_Outbound_Load[0].Load_Id));
                                        frm.Show();
                                        System.Windows.Forms.Application.DoEvents();

                                        Outbound_Office_Ticket.SetDataSource(NWDataset);
                                        // The New Way to Get the Prenter Name Set
                                        CrystalDecisions.ReportAppServer.Controllers.PrintReportOptions printReportOptions = new CrystalDecisions.ReportAppServer.Controllers.PrintReportOptions();
                                        CrystalDecisions.ReportAppServer.Controllers.PrintOutputController printOutputController = new CrystalDecisions.ReportAppServer.Controllers.PrintOutputController();
                                        CrystalDecisions.ReportAppServer.ClientDoc.ISCDReportClientDocument rptClientDoc;
                                        rptClientDoc = Outbound_Office_Ticket.ReportClientDocument;
                                        printReportOptions.PrinterName = Printer;
                                        rptClientDoc.PrintOutputController.PrintReport(printReportOptions);


                                    }
                                }
                            }
                        }

                    }
                }
                catch (Exception ocrap)
                {
                    System.Windows.Forms.MessageBox.Show("Cannot Print Ticket" + System.Environment.NewLine + ocrap.Message);

                    Logging.Add_System_Log("Printing.PrintOutbound_Final_Outbound_Ticket Load UID<" + LoadUID.ToString() + "> Printer Name<" + Printer + ">", ocrap.Message);
                }
            }
        }


        public static MemoryStream PrintWeightSheet(Guid Weight_Sheet_UID)
        {

            MemoryStream stream = new MemoryStream();


            try
            {


                using (Reports.Weigh_Sheet rptWeigh_Sheet = new Reports.Weigh_Sheet())
                {


                    using (NWDataset NWDataset = new NWDataset())
                    {


                        {


                            using (NWDatasetTableAdapters.vwWeigh_SheetTableAdapter vwWeigh_SheetTableAdapter = new NWDatasetTableAdapters.vwWeigh_SheetTableAdapter())
                            {
                                vwWeigh_SheetTableAdapter.FillByWeight_Sheet_UID(NWDataset.vwWeigh_Sheet, Weight_Sheet_UID);
                                {


                                    using (NWDatasetTableAdapters.vwWeight_Sheet_InformationTableAdapter vwWeight_Sheet_InformationTableAdapter = new NWDatasetTableAdapters.vwWeight_Sheet_InformationTableAdapter())
                                    {
                                        if (vwWeight_Sheet_InformationTableAdapter.FillByUID(NWDataset.vwWeight_Sheet_Information, Weight_Sheet_UID) > 0)
                                        {
                                            using (NWDatasetTableAdapters.LocationsTableAdapter locationsTableAdapter = new NWDatasetTableAdapters.LocationsTableAdapter())
                                            {
                                                locationsTableAdapter.FillByLocationId(NWDataset.Locations, NWDataset.vwWeight_Sheet_Information[0].Location_Id);
                                            }
                                            {
                                                rptWeigh_Sheet.SetDataSource(NWDataset);
                                                rptWeigh_Sheet.Subreports[0].SetDataSource(NWDataset);
                                                decimal TotalBilled = 0;
                                                if (!NWDataset.vwWeight_Sheet_Information[0].IsTotal_BilledNull())
                                                {
                                                    TotalBilled = NWDataset.vwWeight_Sheet_Information[0].Total_Billed;
                                                }
                                                var Custom = (!NWDataset.vwWeight_Sheet_Information[0].IsBOL_TypeNull() && NWDataset.vwWeight_Sheet_Information[0].BOL_Type.ToUpper().Trim() == "C");

                                                rptWeigh_Sheet.SetParameterValue(rptWeigh_Sheet.Parameter_CustomRate.ParameterFieldName, Custom);

                                                rptWeigh_Sheet.SetParameterValue(rptWeigh_Sheet.Parameter_End_Of_Lot.ParameterFieldName, NWDataset.vwWeight_Sheet_Information[0].Is_End_Lot);
                                                rptWeigh_Sheet.SetParameterValue(rptWeigh_Sheet.Parameter_New_Lot.ParameterFieldName, NWDataset.vwWeight_Sheet_Information[0].Is_New_Lot);
                                                rptWeigh_Sheet.SetParameterValue(rptWeigh_Sheet.Parameter_Total_Billed.ParameterFieldName, TotalBilled);
                                                rptWeigh_Sheet.SetParameterValue(rptWeigh_Sheet.Parameter_Location_Description.ParameterFieldName, NWDataset.vwWeight_Sheet_Information[0].Location_Description);
                                                rptWeigh_Sheet.SetParameterValue(rptWeigh_Sheet.Parameter_Creation_Date.ParameterFieldName, NWDataset.vwWeight_Sheet_Information[0].Creation_Date);
                                                rptWeigh_Sheet.SetParameterValue(rptWeigh_Sheet.Parameter_Unofficial.ParameterFieldName, true);
                                                rptWeigh_Sheet.SetParameterValue(rptWeigh_Sheet.Parameter_Customer_Copy.ParameterFieldName, true);
                                                rptWeigh_Sheet.SetParameterValue(rptWeigh_Sheet.Parameter_Lot.ParameterFieldName, NWDataset.vwWeight_Sheet_Information[0].Lot_Number);
                                                rptWeigh_Sheet.SetParameterValue(rptWeigh_Sheet.Parameter_StrWS_Id.ParameterFieldName, NWDataset.vwWeight_Sheet_Information[0].WS_Id.ToString().PadLeft(8, '0'));
                                                rptWeigh_Sheet.SetParameterValue(rptWeigh_Sheet.Parameter_Closed.ParameterFieldName, NWDataset.vwWeight_Sheet_Information[0].WS_Id.ToString().PadLeft(8, '0'));




                                                ExportOptions exportOpts = new ExportOptions();
                                                exportOpts.ExportFormatType = ExportFormatType.PortableDocFormat;


                                                PdfRtfWordFormatOptions exportFormatOptions = new PdfRtfWordFormatOptions();// = ExportOptions.CreatePdfFormatOptions;

                                                //EditableRTFExportFormatOptions exportFormatOptions =
                                                // ExportOptions.CreateEditableRTFExportFormatOptions();


                                                exportFormatOptions.FirstPageNumber = 1;
                                                //exportFormatOptions.InsertPageBreaks = true;
                                                exportFormatOptions.LastPageNumber = 1;
                                                exportFormatOptions.UsePageRange = true;
                                                exportOpts.ExportFormatOptions = exportFormatOptions;


                                                exportOpts.ExportFormatType = ExportFormatType.PortableDocFormat;

                                                //exportOpts.FormatOptions = new PdfRtfWordFormatOptions();


                                                ExportRequestContext req = new ExportRequestContext();

                                                req.ExportInfo = exportOpts;


                                                Stream s = rptWeigh_Sheet.FormatEngine.ExportToStream(req);
                                                s.CopyTo(stream);




                                                //System.IO.Stream ms = rptWeigh_Sheet.ExportToStream(ExportFormatType.PortableDocFormat);

                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }

                }

            }
            catch (Exception ocrap)
            {
                Logging.Add_System_Log("Printing.PrintWeightSheetTicket Load UID<" + Weight_Sheet_UID.ToString() + "> MemoryStream ", ocrap.Message);

            }
            return stream;
        }



        public static bool PrintDailyBinReport(DateTime SelectedDate, int Location_Id)
        {
            using (PrintingTicket frm = new PrintingTicket(string.Format("Printing Daily Bin Report For Location {0}", Location_Id)))
            {
                frm.Show();
                System.Windows.Forms.Application.DoEvents();
                string Printer = Settings.workStation_SetupRow.Report_Printer;
                try
                {
                    using (Reports.BinTotals binTotals = new Reports.BinTotals())
                    {
                        using (BinsDataSet reportDataset = new NWGrain.BinsDataSet())
                        {

                            //see if the printer is valid 
                            bool ValidPrinter = false;

                            System.Drawing.Printing.PrinterSettings settings = new System.Drawing.Printing.PrinterSettings();
                            string DefaultPrinter = settings.PrinterName;

                            foreach (string InstalledPrinter in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                            {
                                if (Printer.ToUpper() == InstalledPrinter.ToUpper())
                                {
                                    ValidPrinter = true;
                                    break;
                                }
                            }
                            if (!ValidPrinter) // if not valid use the system default printer
                            {
                                Printer = DefaultPrinter;
                            }
                            {
                                using (BinsDataSetTableAdapters.DailyBinTotalsTableAdapter dailyBintotalsTableAdapter = new BinsDataSetTableAdapters.DailyBinTotalsTableAdapter())
                                {
                                    dailyBintotalsTableAdapter.Fill(reportDataset.DailyBinTotals, SelectedDate, SelectedDate, Location_Id);
                                    {

                                        {

                                            binTotals.SetDataSource(reportDataset);

                                            CrystalDecisions.ReportAppServer.Controllers.PrintReportOptions printReportOptions = new CrystalDecisions.ReportAppServer.Controllers.PrintReportOptions();
                                            CrystalDecisions.ReportAppServer.Controllers.PrintOutputController printOutputController = new CrystalDecisions.ReportAppServer.Controllers.PrintOutputController();
                                            CrystalDecisions.ReportAppServer.ClientDoc.ISCDReportClientDocument rptClientDoc;
                                            rptClientDoc = binTotals.ReportClientDocument;
                                            printReportOptions.PrinterName = Printer;
                                            //printReportOptions.AddPrinterPageRange(1, 1);

                                            rptClientDoc.PrintOutputController.PrintReport(printReportOptions);

                                        }
                                        System.Threading.Thread.Sleep(1000);
                                    }



                                }

                            }
                        }
                    }
                    return true;
                }
                catch (Exception ocrap)
                {
                    Alert.Show("Looks Like The Bin Totals Failed To Print", "Error", false);
                    Logging.Add_System_Log("Printing.PrintDailyBinReport SelectDate<" + SelectedDate.ToString() + "> Location Id <" + Location_Id.ToString() + ">", ocrap.Message);
                    return false;
                }
            }
         
        }




        public static bool PrintDailyIntakeReport(DateTime SelectedDate, int Location_Id)
        {
            using (PrintingTicket frm = new PrintingTicket(string.Format("Printing Daily Intake Totals For Location {0}", Location_Id)))
            {
                frm.Show();
                System.Windows.Forms.Application.DoEvents();
                string Printer = Settings.workStation_SetupRow.Report_Printer;
                try
                {
                    using (Reports.DailyIntakeReport DailyIntakeReport = new Reports.DailyIntakeReport())
                    {
                        using (ReportDataSet reportDataset = new NWGrain.ReportDataSet())
                        {

                            //see if the printer is valid 
                            bool ValidPrinter = false;

                            System.Drawing.Printing.PrinterSettings settings = new System.Drawing.Printing.PrinterSettings();
                            string DefaultPrinter = settings.PrinterName;

                            foreach (string InstalledPrinter in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                            {
                                if (Printer.ToUpper() == InstalledPrinter.ToUpper())
                                {
                                    ValidPrinter = true;
                                    break;
                                }
                            }
                            if (!ValidPrinter) // if not valid use the system default printer
                            {
                                Printer = DefaultPrinter;
                            }
                            {
                                using (ReportDataSetTableAdapters.DailyIntakeWSTableAdapter dailyIntakeWSTableAdapter = new ReportDataSetTableAdapters.DailyIntakeWSTableAdapter())
                                {
                                    dailyIntakeWSTableAdapter.Fill(reportDataset.DailyIntakeWS, SelectedDate, Location_Id);
                                    {

                                        {

                                            DailyIntakeReport.SetDataSource(reportDataset);

                                            DailyIntakeReport.SetParameterValue(DailyIntakeReport.Parameter_Details.ParameterFieldName, true);


                                            CrystalDecisions.ReportAppServer.Controllers.PrintReportOptions printReportOptions = new CrystalDecisions.ReportAppServer.Controllers.PrintReportOptions();
                                            CrystalDecisions.ReportAppServer.Controllers.PrintOutputController printOutputController = new CrystalDecisions.ReportAppServer.Controllers.PrintOutputController();
                                            CrystalDecisions.ReportAppServer.ClientDoc.ISCDReportClientDocument rptClientDoc;
                                            rptClientDoc = DailyIntakeReport.ReportClientDocument;
                                            printReportOptions.PrinterName = Printer;
                                            //printReportOptions.AddPrinterPageRange(1, 1);

                                            rptClientDoc.PrintOutputController.PrintReport(printReportOptions);

                                        }
                                        System.Threading.Thread.Sleep(1000);
                                    }



                                }

                            }
                        }
                    }
                    return true;
                }
                catch (Exception ocrap)
                {
                    Alert.Show("Looks Like The Details Failed To Print", "Error", false);
                    Logging.Add_System_Log("Printing.PrintDailyIntakeReport SelectDate<" + SelectedDate.ToString() + "> Location Id <" + Location_Id.ToString() + ">", ocrap.Message);
                    return false;
                }
            }
        }




        public static bool PrintDailyTransferReport(DateTime SelectedDate, int Location_Id)
        {
            using (PrintingTicket frm = new PrintingTicket(string.Format("Printing Daily Transfer Totals For Location {0}", Location_Id)))
            {
                frm.Show();
                System.Windows.Forms.Application.DoEvents();
                string Printer = Settings.workStation_SetupRow.Report_Printer;
                try
                {
                    using (Reports.DailyTransferReport DailyTransferReport = new Reports.DailyTransferReport())
                    {
                        using (ReportDataSet reportDataset = new NWGrain.ReportDataSet())
                        {
                            reportDataset.EnforceConstraints = false;
                            //see if the printer is valid 
                            bool ValidPrinter = false;

                            System.Drawing.Printing.PrinterSettings settings = new System.Drawing.Printing.PrinterSettings();
                            string DefaultPrinter = settings.PrinterName;

                            foreach (string InstalledPrinter in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                            {
                                if (Printer.ToUpper() == InstalledPrinter.ToUpper())
                                {
                                    ValidPrinter = true;
                                    break;
                                }
                            }
                            if (!ValidPrinter) // if not valid use the system default printer
                            {
                                Printer = DefaultPrinter;
                            }
                            {
                                using (ReportDataSetTableAdapters.DailyTransferWSTableAdapter dailyTransferWSTableAdapter = new ReportDataSetTableAdapters.DailyTransferWSTableAdapter())
                                {
                                    dailyTransferWSTableAdapter.Fill(reportDataset.DailyTransferWS, SelectedDate, Location_Id);
                                    {

                                        {

                                            DailyTransferReport.SetDataSource(reportDataset);
                                            DailyTransferReport.SetParameterValue(DailyTransferReport.Parameter_Details.ParameterFieldName, true);

                                            CrystalDecisions.ReportAppServer.Controllers.PrintReportOptions printReportOptions = new CrystalDecisions.ReportAppServer.Controllers.PrintReportOptions();
                                            CrystalDecisions.ReportAppServer.Controllers.PrintOutputController printOutputController = new CrystalDecisions.ReportAppServer.Controllers.PrintOutputController();
                                            CrystalDecisions.ReportAppServer.ClientDoc.ISCDReportClientDocument rptClientDoc;
                                            rptClientDoc = DailyTransferReport.ReportClientDocument;
                                            printReportOptions.PrinterName = Printer;
                                            //printReportOptions.AddPrinterPageRange(1, 1);

                                            rptClientDoc.PrintOutputController.PrintReport(printReportOptions);

                                        }
                                        System.Threading.Thread.Sleep(1000);
                                    }


                                }

                            }
                        }
                    }
                    return true;
                }
                catch (Exception ocrap)
                {
                    Alert.Show("Looks Like The Details Failed To Print", "Error", false);
                    Logging.Add_System_Log("Printing.PrintDailyTransferReport SelectDate<" + SelectedDate.ToString() + "> Location Id <" + Location_Id.ToString() + ">", ocrap.Message);
                    return false;
                }
            }
        }




        public static bool PrintDailyWeightSheetAsc(DateTime SelectedDate, int Location_Id)
        {
            //using (PrintingTicket frm = new PrintingTicket(string.Format("Printing Weight Sheet report In Order For Location {0}", Location_Id)))
            //{
            //    frm.Show();
            //    System.Windows.Forms.Application.DoEvents();
            //    string Printer = Settings.workStation_SetupRow.Report_Printer;
            //    try
            //    {
            //        using (Reports.WeightSheetsAsc weightSheetsAsc = new Reports.WeightSheetsAsc())
            //        {
            //            using (ReportDataSet reportDataset = new NWGrain.ReportDataSet())
            //            {

            //                //see if the printer is valid 
            //                bool ValidPrinter = false;

            //                System.Drawing.Printing.PrinterSettings settings = new System.Drawing.Printing.PrinterSettings();
            //                string DefaultPrinter = settings.PrinterName;

            //                foreach (string InstalledPrinter in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            //                {
            //                    if (Printer.ToUpper() == InstalledPrinter.ToUpper())
            //                    {
            //                        ValidPrinter = true;
            //                        break;
            //                    }
            //                }
            //                if (!ValidPrinter) // if not valid use the system default printer
            //                {
            //                    Printer = DefaultPrinter;
            //                }
            //                {
            //                    using (ReportDataSetTableAdapters.WeightSeriesByLocationDateTableAdapter weightSeriesByLocationDateTableAdapter = new ReportDataSetTableAdapters.WeightSeriesByLocationDateTableAdapter())
            //                    {
            //                        weightSeriesByLocationDateTableAdapter.Fill(reportDataset.WeightSeriesByLocationDate, Location_Id, SelectedDate);
            //                        {

            //                            {

            //                                weightSheetsAsc.SetDataSource(reportDataset);


            //                                CrystalDecisions.ReportAppServer.Controllers.PrintReportOptions printReportOptions = new CrystalDecisions.ReportAppServer.Controllers.PrintReportOptions();
            //                                CrystalDecisions.ReportAppServer.Controllers.PrintOutputController printOutputController = new CrystalDecisions.ReportAppServer.Controllers.PrintOutputController();
            //                                CrystalDecisions.ReportAppServer.ClientDoc.ISCDReportClientDocument rptClientDoc;
            //                                rptClientDoc = weightSheetsAsc.ReportClientDocument;
            //                                printReportOptions.PrinterName = Printer;
            //                                //printReportOptions.AddPrinterPageRange(1, 1);

            //                                rptClientDoc.PrintOutputController.PrintReport(printReportOptions);

            //                            }
            //                            System.Threading.Thread.Sleep(1000);
            //                        }


            //                    }

            //                }
            //            }
            //        }
            //        return true;
            //    }
            //    catch (Exception ocrap)
            //    {
            //        Alert.Show("Looks Like The Report Failed To Print", "Error", false);
            //        Logging.Add_System_Log("Printing.PrintDailyWeightSheetAsc SelectDate<" + SelectedDate.ToString() + "> Location Id <" + Location_Id.ToString() + ">", ocrap.Message);
            //        return false;
            //    }
            //}
            return true;
        }





        public static bool PrintDailyLoadsByCrop(DateTime SelectedDate, int Location_Id)
        {
            //using (PrintingTicket frm = new PrintingTicket(string.Format("Printing Loads By Crop For Location {0}", Location_Id)))
            //{
            //    frm.Show();
            //    System.Windows.Forms.Application.DoEvents();
            //    string Printer = Settings.workStation_SetupRow.Report_Printer;
            //    try
            //    {
            //        using (Reports.DailyCommodityReport dailyCommodityReport = new Reports.DailyCommodityReport())
            //        {
            //            using (ReportDataSet reportDataset = new NWGrain.ReportDataSet())
            //            {

            //                //see if the printer is valid 
            //                bool ValidPrinter = false;

            //                System.Drawing.Printing.PrinterSettings settings = new System.Drawing.Printing.PrinterSettings();
            //                string DefaultPrinter = settings.PrinterName;

            //                foreach (string InstalledPrinter in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            //                {
            //                    if (Printer.ToUpper() == InstalledPrinter.ToUpper())
            //                    {
            //                        ValidPrinter = true;
            //                        break;
            //                    }
            //                }
            //                if (!ValidPrinter) // if not valid use the system default printer
            //                {
            //                    Printer = DefaultPrinter;
            //                }
            //                {
            //                    using (ReportDataSetTableAdapters.DailyLoadCountByCropTableAdapter dailyLoadCountByCropTableAdapter = new ReportDataSetTableAdapters.DailyLoadCountByCropTableAdapter())
            //                    {
            //                        dailyLoadCountByCropTableAdapter.Fill(reportDataset.DailyLoadCountByCrop, SelectedDate, Location_Id);
            //                        {

            //                            {

            //                                dailyCommodityReport.SetDataSource(reportDataset);


            //                                CrystalDecisions.ReportAppServer.Controllers.PrintReportOptions printReportOptions = new CrystalDecisions.ReportAppServer.Controllers.PrintReportOptions();
            //                                CrystalDecisions.ReportAppServer.Controllers.PrintOutputController printOutputController = new CrystalDecisions.ReportAppServer.Controllers.PrintOutputController();
            //                                CrystalDecisions.ReportAppServer.ClientDoc.ISCDReportClientDocument rptClientDoc;
            //                                rptClientDoc = dailyCommodityReport.ReportClientDocument;
            //                                printReportOptions.PrinterName = Printer;
            //                                //printReportOptions.AddPrinterPageRange(1, 1);

            //                                rptClientDoc.PrintOutputController.PrintReport(printReportOptions);

            //                            }
            //                            System.Threading.Thread.Sleep(1000);
            //                        }


            //                    }

            //                }
            //            }
            //        }
            //        return true;
            //    }
            //    catch (Exception ocrap)
            //    {
            //        Alert.Show("Looks Like The Report Failed To Print", "Error", false);
            //        Logging.Add_System_Log("Printing.PrintDailyLoadsByCrop SelectDate<" + SelectedDate.ToString() + "> Location Id <" + Location_Id.ToString() + ">", ocrap.Message);
            //        return false;
            //    }
            //}
            return true;
        }





        public static void Print_Basic_Ticket(int Weight, string Description)
        {

            try
            {
                string Location = Settings.CurrentWorkStationLocationRow.Description;


                using (Reports.BasicTicket Basic_Ticket = new Reports.BasicTicket())
                {

                    string Printer = Settings.workStation_SetupRow.Report_Printer;


                    int Test = Basic_Ticket.ParameterFields.Count;
                    Basic_Ticket.SetParameterValue("Weight", Weight);

                    Basic_Ticket.SetParameterValue("Time", DateTime.Now);
                    Basic_Ticket.SetParameterValue("Description", Description);
                    Basic_Ticket.SetParameterValue("Location", Location);


                    CrystalDecisions.ReportAppServer.Controllers.PrintReportOptions printReportOptions = new CrystalDecisions.ReportAppServer.Controllers.PrintReportOptions();
                    CrystalDecisions.ReportAppServer.Controllers.PrintOutputController printOutputController = new CrystalDecisions.ReportAppServer.Controllers.PrintOutputController();
                    CrystalDecisions.ReportAppServer.ClientDoc.ISCDReportClientDocument rptClientDoc;
                    rptClientDoc = Basic_Ticket.ReportClientDocument;
                    printReportOptions.PrinterName = Printer;


                    rptClientDoc.PrintOutputController.PrintReport(printReportOptions);










                }

            }
            catch (Exception ocrap)
            {

                Logging.Add_System_Log("Printing.Print_Basic_Ticket", ocrap.Message);

            }
        }








        public static bool PrintWeightSheet(Guid Weight_Sheet_UID, bool Unofficial, string Printer = "", bool CustomerCopy = false)
        {




            try
            {


                using (Reports.Weigh_Sheet rptWeigh_Sheet = new Reports.Weigh_Sheet())
                {


                    using (NWDataset NWDataset = new NWDataset())
                    {

                        if (string.IsNullOrEmpty(Printer))
                        {

                            Printer = Settings.workStation_SetupRow.Report_Printer;

                        }
                        //see if the printer is valid 
                        bool ValidPrinter = false;

                        System.Drawing.Printing.PrinterSettings settings = new System.Drawing.Printing.PrinterSettings();
                        string DefaultPrinter = settings.PrinterName;

                        foreach (string InstalledPrinter in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                        {
                            if (Printer.ToUpper() == InstalledPrinter.ToUpper())
                            {
                                ValidPrinter = true;
                                break;
                            }

                        }
                        if (!ValidPrinter) // if not valid use the system default printer
                        {
                            Printer = DefaultPrinter;
                        }
                        {


                            using (NWDatasetTableAdapters.vwWeigh_SheetTableAdapter vwWeigh_SheetTableAdapter = new NWDatasetTableAdapters.vwWeigh_SheetTableAdapter())
                            {
                                NWDataset.EnforceConstraints = false;
                                vwWeigh_SheetTableAdapter.FillByWeight_Sheet_UID(NWDataset.vwWeigh_Sheet, Weight_Sheet_UID);
                                {


                                    using (NWDatasetTableAdapters.vwWeight_Sheet_InformationTableAdapter vwWeight_Sheet_InformationTableAdapter = new NWDatasetTableAdapters.vwWeight_Sheet_InformationTableAdapter())
                                    {
                                        if (vwWeight_Sheet_InformationTableAdapter.FillByUID(NWDataset.vwWeight_Sheet_Information, Weight_Sheet_UID) > 0)
                                        {
                                            using (NWDatasetTableAdapters.LocationsTableAdapter locationsTableAdapter = new NWDatasetTableAdapters.LocationsTableAdapter())
                                            {
                                                locationsTableAdapter.FillByLocationId(NWDataset.Locations, NWDataset.vwWeight_Sheet_Information[0].Location_Id);
                                            }
                                            using (PrintingTicket frm = new PrintingTicket(string.Format("Printing Weight Sheet {0}", NWDataset.vwWeight_Sheet_Information[0].WS_Id)))
                                            {
                                                frm.Show();
                                                System.Windows.Forms.Application.DoEvents();
                                                rptWeigh_Sheet.SetDataSource(NWDataset);
                                                rptWeigh_Sheet.Subreports[0].SetDataSource(NWDataset);
                                                decimal TotalBilled = 0;
                                                if (!NWDataset.vwWeight_Sheet_Information[0].IsTotal_BilledNull())
                                                {
                                                    TotalBilled = NWDataset.vwWeight_Sheet_Information[0].Total_Billed;
                                                }
                                                var Custom = (!NWDataset.vwWeight_Sheet_Information[0].IsBOL_TypeNull() && NWDataset.vwWeight_Sheet_Information[0].BOL_Type.ToUpper().Trim() == "C");

                                                rptWeigh_Sheet.SetParameterValue(rptWeigh_Sheet.Parameter_CustomRate.ParameterFieldName, Custom);

                                                rptWeigh_Sheet.SetParameterValue(rptWeigh_Sheet.Parameter_End_Of_Lot.ParameterFieldName, NWDataset.vwWeight_Sheet_Information[0].Is_End_Lot);
                                                rptWeigh_Sheet.SetParameterValue(rptWeigh_Sheet.Parameter_New_Lot.ParameterFieldName, NWDataset.vwWeight_Sheet_Information[0].Is_New_Lot);
                                                rptWeigh_Sheet.SetParameterValue(rptWeigh_Sheet.Parameter_Total_Billed.ParameterFieldName, TotalBilled);
                                                ///  rptWeigh_Sheet.SetParameterValue(rptWeigh_Sheet.Parameter_Weight_Sheet_ID.ParameterFieldName, NWDataset.vwWeight_Sheet_Information[0].WS_Id);
                                                rptWeigh_Sheet.SetParameterValue(rptWeigh_Sheet.Parameter_Location_Description.ParameterFieldName, NWDataset.vwWeight_Sheet_Information[0].Location_Description);
                                                rptWeigh_Sheet.SetParameterValue(rptWeigh_Sheet.Parameter_Creation_Date.ParameterFieldName, NWDataset.vwWeight_Sheet_Information[0].Creation_Date);
                                                // rptWeigh_Sheet.SetParameterValue(rptWeigh_Sheet.Parameter_Location_Id.ParameterFieldName, NWDataset.vwWeight_Sheet_Information[0].Location_Id);
                                                rptWeigh_Sheet.SetParameterValue(rptWeigh_Sheet.Parameter_Unofficial.ParameterFieldName, Unofficial);
                                                rptWeigh_Sheet.SetParameterValue(rptWeigh_Sheet.Parameter_Customer_Copy.ParameterFieldName, CustomerCopy);
                                                rptWeigh_Sheet.SetParameterValue(rptWeigh_Sheet.Parameter_Lot.ParameterFieldName, NWDataset.vwWeight_Sheet_Information[0].Lot_Number);
                                                rptWeigh_Sheet.SetParameterValue(rptWeigh_Sheet.Parameter_StrWS_Id.ParameterFieldName, NWDataset.vwWeight_Sheet_Information[0].WS_Id.ToString().PadLeft(8, '0'));
                                                rptWeigh_Sheet.SetParameterValue(rptWeigh_Sheet.Parameter_Closed.ParameterFieldName, NWDataset.vwWeight_Sheet_Information[0].WS_Id.ToString().PadLeft(8, '0'));
                                                // The New Way to Get the Printer Name Set
                                                CrystalDecisions.ReportAppServer.Controllers.PrintReportOptions printReportOptions = new CrystalDecisions.ReportAppServer.Controllers.PrintReportOptions();
                                                CrystalDecisions.ReportAppServer.Controllers.PrintOutputController printOutputController = new CrystalDecisions.ReportAppServer.Controllers.PrintOutputController();
                                                CrystalDecisions.ReportAppServer.ClientDoc.ISCDReportClientDocument rptClientDoc;
                                                rptClientDoc = rptWeigh_Sheet.ReportClientDocument;
                                                printReportOptions.PrinterName = Printer;
                                                printReportOptions.AddPrinterPageRange(1, 1);



                                                rptClientDoc.PrintOutputController.PrintReport(printReportOptions);



                                                //string DirectoryPath = @"c:\ScaleTickets";
                                                //string Filename = DirectoryPath + @"\WS" + NWDataset.vwWeigh_Sheet[0].Location_Id.ToString() + "_" + NWDataset.vwWeigh_Sheet[0].WS_Id.ToString() + ".pdf";
                                                //if (!System.IO.Directory.Exists(DirectoryPath)) { System.IO.Directory.CreateDirectory(DirectoryPath); }
                                                //if (!System.IO.File.Exists(Filename)) { System.IO.File.Delete(Filename); }
                                                //rptWeigh_Sheet.ExportToDisk(ExportFormatType.PortableDocFormat, Filename);
                                            }
                                        }
                                        System.Threading.Thread.Sleep(1000);
                                    }

                                }
                            }
                        }
                    }

                }
                return true;
            }
            catch (Exception ocrap)
            {
                Alert.Show("Looks Like The Ticket Failed To Print", "Error", false);
                Logging.Add_System_Log("Printing.PrintWeightSheetTicket Load UID<" + Weight_Sheet_UID.ToString() + "> Printer Name<" + Printer + ">", ocrap.Message);
                return false;
            }

        }

        public static bool PrintTransferWeightSheet(Guid Weight_Sheet_UID, bool Unofficial, string Printer = "")
        {




            try
            {


                using (Reports.Transfer_Weigh_Sheet Transfer_Weigh_Sheet = new Reports.Transfer_Weigh_Sheet())
                {


                    using (NWDataset NWDataset = new NWDataset())
                    {

                        if (string.IsNullOrEmpty(Printer))
                        {
                            Printer = Settings.workStation_SetupRow.Report_Printer;
                        }
                        //see if the printer is valid 
                        bool ValidPrinter = false;

                        System.Drawing.Printing.PrinterSettings settings = new System.Drawing.Printing.PrinterSettings();
                        string DefaultPrinter = settings.PrinterName;

                        foreach (string InstalledPrinter in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                        {
                            if (Printer.ToUpper() == InstalledPrinter.ToUpper())
                            {
                                ValidPrinter = true;
                                break;
                            }

                        }
                        if (!ValidPrinter) // if not valid use the system default printer
                        {
                            Printer = DefaultPrinter;
                        }
                        {


                            using (NWDatasetTableAdapters.vwTransfer_LoadTableAdapter vwTransfer_LoadTableAdapter = new NWDatasetTableAdapters.vwTransfer_LoadTableAdapter())
                            {
                                vwTransfer_LoadTableAdapter.FillByWeight_Sheet_UID(NWDataset.vwTransfer_Load, Weight_Sheet_UID);
                                {

                                    using (NWDatasetTableAdapters.vwTransfer_Weight_Sheet_InformationTableAdapter vwTransfer_Weight_Sheet_InformationTableAdapter = new NWDatasetTableAdapters.vwTransfer_Weight_Sheet_InformationTableAdapter())
                                    {
                                        if (vwTransfer_Weight_Sheet_InformationTableAdapter.FillByWeight_SheetUID(NWDataset.vwTransfer_Weight_Sheet_Information, Weight_Sheet_UID) > 0)
                                        {

                                            using (NWDatasetTableAdapters.LocationsTableAdapter locationsTableAdapter = new NWDatasetTableAdapters.LocationsTableAdapter())
                                            {
                                                locationsTableAdapter.FillByLocationId(NWDataset.Locations, NWDataset.vwTransfer_Weight_Sheet_Information[0].Location_Id);
                                            }
                                            using (PrintingTicket frm = new PrintingTicket(string.Format("Printing Transfer Weight Sheet {0}", NWDataset.vwTransfer_Weight_Sheet_Information[0].WS_Id)))
                                            {
                                                frm.Show();
                                                System.Windows.Forms.Application.DoEvents();
                                                Transfer_Weigh_Sheet.SetDataSource(NWDataset);
                                                Transfer_Weigh_Sheet.Subreports[0].SetDataSource(NWDataset);
                                                decimal TotalBilled = 0;
                                                if (!NWDataset.vwTransfer_Weight_Sheet_Information[0].IsTotal_BilledNull())
                                                {
                                                    TotalBilled = NWDataset.vwTransfer_Weight_Sheet_Information[0].Total_Billed;
                                                }
                                                var Custom = (!NWDataset.vwTransfer_Weight_Sheet_Information[0].IsBOL_TypeNull() && NWDataset.vwTransfer_Weight_Sheet_Information[0].BOL_Type.ToUpper().Trim() == "C");

                                                Transfer_Weigh_Sheet.SetParameterValue(Transfer_Weigh_Sheet.Parameter_CustomRate.ParameterFieldName, Custom);
                                                Transfer_Weigh_Sheet.SetParameterValue(Transfer_Weigh_Sheet.Parameter_Total_Billed.ParameterFieldName, TotalBilled);
                                                Transfer_Weigh_Sheet.SetParameterValue(Transfer_Weigh_Sheet.Parameter_Location_Description.ParameterFieldName, NWDataset.vwTransfer_Weight_Sheet_Information[0].Location_Description);
                                                Transfer_Weigh_Sheet.SetParameterValue(Transfer_Weigh_Sheet.Parameter_Creation_Date.ParameterFieldName, NWDataset.vwTransfer_Weight_Sheet_Information[0].Creation_Date);
                                                Transfer_Weigh_Sheet.SetParameterValue(Transfer_Weigh_Sheet.Parameter_Unofficial.ParameterFieldName, Unofficial);
                                                Transfer_Weigh_Sheet.SetParameterValue(Transfer_Weigh_Sheet.Parameter_StrWS_Id.ParameterFieldName, NWDataset.vwTransfer_Weight_Sheet_Information[0].WS_Id.ToString().PadLeft(8, '0'));
                                                Transfer_Weigh_Sheet.SetParameterValue(Transfer_Weigh_Sheet.Parameter_Closed.ParameterFieldName, true);
                                                // The New Way to Get the Prenter Name Set
                                                CrystalDecisions.ReportAppServer.Controllers.PrintReportOptions printReportOptions = new CrystalDecisions.ReportAppServer.Controllers.PrintReportOptions();
                                                CrystalDecisions.ReportAppServer.Controllers.PrintOutputController printOutputController = new CrystalDecisions.ReportAppServer.Controllers.PrintOutputController();
                                                CrystalDecisions.ReportAppServer.ClientDoc.ISCDReportClientDocument rptClientDoc;
                                                rptClientDoc = Transfer_Weigh_Sheet.ReportClientDocument;
                                                printReportOptions.PrinterName = Printer;
                                                printReportOptions.AddPrinterPageRange(1, 1);
                                                rptClientDoc.PrintOutputController.PrintReport(printReportOptions);



                                                //string DirectoryPath = @"c:\ScaleTickets";
                                                //string Filename = DirectoryPath + @"\WS" + NWDataset.vwWeigh_Sheet[0].Location_Id.ToString() + "_" + NWDataset.vwWeigh_Sheet[0].WS_Id.ToString() + ".pdf";
                                                //if (!System.IO.Directory.Exists(DirectoryPath)) { System.IO.Directory.CreateDirectory(DirectoryPath); }
                                                //if (!System.IO.File.Exists(Filename)) { System.IO.File.Delete(Filename); }
                                                //rptWeigh_Sheet.ExportToDisk(ExportFormatType.PortableDocFormat, Filename);
                                            }
                                        }
                                        System.Threading.Thread.Sleep(1000);
                                    }

                                }
                            }
                        }
                    }

                }
                return true;
            }
            catch (Exception ocrap)
            {
                Alert.Show("Looks Like The Ticket Failed To Print", "Error", false);
                Logging.Add_System_Log("Printing.PrintWeightSheetTicket Load UID<" + Weight_Sheet_UID.ToString() + "> Printer Name<" + Printer + ">", ocrap.Message);
                return false;
            }

        }


        public static MemoryStream PrintTransferWeightSheet(Guid Weight_Sheet_UID)
        {
            MemoryStream stream = new MemoryStream();



            try
            {


                using (Reports.Transfer_Weigh_Sheet Transfer_Weigh_Sheet = new Reports.Transfer_Weigh_Sheet())
                {


                    using (NWDataset NWDataset = new NWDataset())
                    {

                        {


                            using (NWDatasetTableAdapters.vwTransfer_LoadTableAdapter vwTransfer_LoadTableAdapter = new NWDatasetTableAdapters.vwTransfer_LoadTableAdapter())
                            {
                                vwTransfer_LoadTableAdapter.FillByWeight_Sheet_UID(NWDataset.vwTransfer_Load, Weight_Sheet_UID);
                                {

                                    using (NWDatasetTableAdapters.vwTransfer_Weight_Sheet_InformationTableAdapter vwTransfer_Weight_Sheet_InformationTableAdapter = new NWDatasetTableAdapters.vwTransfer_Weight_Sheet_InformationTableAdapter())
                                    {
                                        if (vwTransfer_Weight_Sheet_InformationTableAdapter.FillByWeight_SheetUID(NWDataset.vwTransfer_Weight_Sheet_Information, Weight_Sheet_UID) > 0)
                                        {

                                            using (NWDatasetTableAdapters.LocationsTableAdapter locationsTableAdapter = new NWDatasetTableAdapters.LocationsTableAdapter())
                                            {
                                                locationsTableAdapter.FillByLocationId(NWDataset.Locations, NWDataset.vwTransfer_Weight_Sheet_Information[0].Location_Id);
                                            }

                                            {
                                                Transfer_Weigh_Sheet.SetDataSource(NWDataset);
                                                Transfer_Weigh_Sheet.Subreports[0].SetDataSource(NWDataset);
                                                decimal TotalBilled = 0;
                                                if (!NWDataset.vwTransfer_Weight_Sheet_Information[0].IsTotal_BilledNull())
                                                {
                                                    TotalBilled = NWDataset.vwTransfer_Weight_Sheet_Information[0].Total_Billed;
                                                }
                                                var Custom = (!NWDataset.vwTransfer_Weight_Sheet_Information[0].IsBOL_TypeNull() && NWDataset.vwTransfer_Weight_Sheet_Information[0].BOL_Type.ToUpper().Trim() == "C");

                                                Transfer_Weigh_Sheet.SetParameterValue(Transfer_Weigh_Sheet.Parameter_CustomRate.ParameterFieldName, Custom);
                                                Transfer_Weigh_Sheet.SetParameterValue(Transfer_Weigh_Sheet.Parameter_Total_Billed.ParameterFieldName, TotalBilled);
                                                Transfer_Weigh_Sheet.SetParameterValue(Transfer_Weigh_Sheet.Parameter_Location_Description.ParameterFieldName, NWDataset.vwTransfer_Weight_Sheet_Information[0].Location_Description);
                                                Transfer_Weigh_Sheet.SetParameterValue(Transfer_Weigh_Sheet.Parameter_Creation_Date.ParameterFieldName, NWDataset.vwTransfer_Weight_Sheet_Information[0].Creation_Date);
                                                Transfer_Weigh_Sheet.SetParameterValue(Transfer_Weigh_Sheet.Parameter_Unofficial.ParameterFieldName, true);
                                                Transfer_Weigh_Sheet.SetParameterValue(Transfer_Weigh_Sheet.Parameter_StrWS_Id.ParameterFieldName, NWDataset.vwTransfer_Weight_Sheet_Information[0].WS_Id.ToString().PadLeft(8, '0'));
                                                Transfer_Weigh_Sheet.SetParameterValue(Transfer_Weigh_Sheet.Parameter_Closed.ParameterFieldName, true);


                                                System.IO.Stream ms = Transfer_Weigh_Sheet.ExportToStream(ExportFormatType.PortableDocFormat);
                                                ms.CopyTo(stream);
                                            }
                                        }

                                    }

                                }
                            }
                        }
                    }

                }

            }
            catch (Exception ocrap)
            {

                Logging.Add_System_Log("Printing.PrintWeightSheetTicket Load UID<" + Weight_Sheet_UID.ToString() + "> MemoryStream <", ocrap.Message);

            }
            return stream;
        }




        public static bool PrintSampleLabel(Guid Lot_UID)
        {

            bool Check_Protien = Alert.Show("Check Protein On Sample", "Protein", true) == System.Windows.Forms.DialogResult.Yes;
            System_Log.Log_Message("PrintSampleLabel", Lot_UID.ToString());

            string Printer = "";

            try
            {


                using (Reports.Sample_Label rptSample_Label = new Reports.Sample_Label())
                {


                    using (NWDataset NWDataset = new NWDataset())
                    {

                        if (string.IsNullOrEmpty(Printer))
                        {
                            Printer = Settings.workStation_SetupRow.Grade_Printer;

                        }
                        //see if the printer is valid 
                        bool ValidPrinter = false;

                        System.Drawing.Printing.PrinterSettings settings = new System.Drawing.Printing.PrinterSettings();
                        string DefaultPrinter = settings.PrinterName;

                        foreach (string InstalledPrinter in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                        {
                            System.Diagnostics.Debug.Print(InstalledPrinter);
                            if (Printer.ToUpper() == InstalledPrinter.ToUpper())
                            {

                                ValidPrinter = true;
                                break;
                            }

                        }
                        if (!ValidPrinter) // if not valid use the system default printer
                        {
                            Printer = DefaultPrinter;
                        }
                        {
                            System_Log.Log_Message("PrintSampleLabel- Selected Printer>>", Printer);

                            using (NWDatasetTableAdapters.SampleLabelTableAdapter SampleLabelTableAdapter = new NWDatasetTableAdapters.SampleLabelTableAdapter())
                            {

                                if (SampleLabelTableAdapter.Fill(NWDataset.SampleLabel, Lot_UID, Check_Protien) > 0)
                                {
                                    using (PrintingTicket frm = new PrintingTicket(string.Format("Printing Sample Label For Lot {0}", NWDataset.SampleLabel[0].Lot_Number)))
                                    {
                                        frm.Show();
                                        System.Windows.Forms.Application.DoEvents();
                                        rptSample_Label.SetDataSource(NWDataset);


                                        // The New Way to Get the Prenter Name Set
                                        CrystalDecisions.ReportAppServer.Controllers.PrintReportOptions printReportOptions = new CrystalDecisions.ReportAppServer.Controllers.PrintReportOptions();
                                        CrystalDecisions.ReportAppServer.Controllers.PrintOutputController printOutputController = new CrystalDecisions.ReportAppServer.Controllers.PrintOutputController();
                                        CrystalDecisions.ReportAppServer.ClientDoc.ISCDReportClientDocument rptClientDoc;
                                        rptClientDoc = rptSample_Label.ReportClientDocument;
                                        printReportOptions.PrinterName = Printer;
                                        printReportOptions.AddPrinterPageRange(1, 1);
                                        rptClientDoc.PrintOutputController.PrintReport(printReportOptions);



                                        //string DirectoryPath = @"c:\ScaleTickets";
                                        //string Filename = DirectoryPath + @"\WS" + NWDataset.vwWeigh_Sheet[0].Location_Id.ToString() + "_" + NWDataset.vwWeigh_Sheet[0].WS_Id.ToString() + ".pdf";
                                        //if (!System.IO.Directory.Exists(DirectoryPath)) { System.IO.Directory.CreateDirectory(DirectoryPath); }
                                        //if (!System.IO.File.Exists(Filename)) { System.IO.File.Delete(Filename); }
                                        //rptWeigh_Sheet.ExportToDisk(ExportFormatType.PortableDocFormat, Filename);

                                    }

                                }
                                else
                                {
                                    System_Log.Log_Message("PrintSampleLabel", "No Sample Label Printer Check Protien Selected");
                                }
                                System.Threading.Thread.Sleep(1000);
                            }
                        }
                    }

                }
                return true;
            }
            catch (Exception ocrap)
            {
                Alert.Show("Looks Like The Sample Label Failed To Print", "Error", false);
                Logging.Add_System_Log("Printing.PrintSampleLabel Load UID<" + Lot_UID.ToString() + "> Printer Name<" + Printer + ">", ocrap.Message);
                return false;
            }

        }


        #region  SeedTicketPrinting
        public static void Print_Inbound_Seed_Ticket(int Ticket, string Printer = "")
        {
            bool PrintTicket = true;
            using (PrintingTicket frm = new PrintingTicket())
            {



                try
                {


                    using (Reports.SeedTickets.Inbound_Seed_Ticket Inbound_Seed_Ticket = new Reports.SeedTickets.Inbound_Seed_Ticket())
                    {


                        using (SeedTicketDataSet SeedTicketDataSet = new SeedTicketDataSet())
                        {
                            using (SeedTicketDataSetTableAdapters.Seed_Ticket_Print_InfoTableAdapter Seed_Ticket_Print_InfoTableAdapter = new SeedTicketDataSetTableAdapters.Seed_Ticket_Print_InfoTableAdapter())
                            {
                                Seed_Ticket_Print_InfoTableAdapter.Fill(SeedTicketDataSet.Seed_Ticket_Print_Info, Ticket);
                            }
                            if (string.IsNullOrEmpty(Printer))
                            {
                                Printer = Settings.workStation_SetupRow.Inbound_Printer;

                            }
                            //see if the printer is valid 
                            bool ValidPrinter = false;

                            System.Drawing.Printing.PrinterSettings settings = new System.Drawing.Printing.PrinterSettings();
                            string DefaultPrinter = settings.PrinterName;

                            foreach (string InstalledPrinter in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                            {
                                if (Printer.ToUpper() == InstalledPrinter.ToUpper())
                                {
                                    ValidPrinter = true;
                                    break;
                                }

                            }
                            if (!ValidPrinter) // if not valid use the system default printer
                            {
                                Printer = DefaultPrinter;
                            }
                            if (PrintTicket)
                            {

                                if (SeedTicketDataSet.Seed_Ticket_Print_Info.Count > 0)
                                {
                                    frm.SetPrompt(String.Format("Printing Inbound Ticket:{0}", Ticket));
                                    frm.Show();
                                    System.Windows.Forms.Application.DoEvents();

                                    Inbound_Seed_Ticket.SetDataSource(SeedTicketDataSet);
                                    // The New Way to Get the Prenter Name Set
                                    CrystalDecisions.ReportAppServer.Controllers.PrintReportOptions printReportOptions = new CrystalDecisions.ReportAppServer.Controllers.PrintReportOptions();
                                    CrystalDecisions.ReportAppServer.Controllers.PrintOutputController printOutputController = new CrystalDecisions.ReportAppServer.Controllers.PrintOutputController();
                                    CrystalDecisions.ReportAppServer.ClientDoc.ISCDReportClientDocument rptClientDoc;
                                    rptClientDoc = Inbound_Seed_Ticket.ReportClientDocument;
                                    printReportOptions.PrinterName = Printer;
                                    rptClientDoc.PrintOutputController.PrintReport(printReportOptions);


                                }

                            }

                        }
                    }
                }
                catch (Exception ocrap)
                {
                    System.Windows.Forms.MessageBox.Show("Cannot Print Ticket" + System.Environment.NewLine + ocrap.Message);

                    Logging.Add_System_Log("Printing.Print_INbound_Seed_Ticket Ticket=<" + Ticket.ToString() + "> Printer Name<" + Printer + ">", ocrap.Message);
                }
            }
        }









        public static void Print_Outbound_Seed_Ticket(Guid UID, int TicketCount, string Printer = "")
        {
            bool PrintTicket = true;
            using (PrintingTicket frm = new PrintingTicket())
            {
                try
                {

                    using (Reports.SeedTickets.OutboundSeedTicket Outbound_Seed_Ticket = new Reports.SeedTickets.OutboundSeedTicket())
                    {


                        using (SeedTicketDataSet SeedTicketDataSet = new SeedTicketDataSet())
                        {
                            using (SeedTicketDataSetTableAdapters.Seed_Ticket_Print_InfoTableAdapter Seed_Ticket_Print_InfoTableAdapter = new SeedTicketDataSetTableAdapters.Seed_Ticket_Print_InfoTableAdapter())
                            {
                                Seed_Ticket_Print_InfoTableAdapter.FillByUID(SeedTicketDataSet.Seed_Ticket_Print_Info, UID);

                            }
                            using (SeedTicketDataSetTableAdapters.vwSeed_Ticket_Total_WeightsTableAdapter vwSeed_Ticket_Total_WeightsTableAdapter = new SeedTicketDataSetTableAdapters.vwSeed_Ticket_Total_WeightsTableAdapter())
                            {
                                vwSeed_Ticket_Total_WeightsTableAdapter.FillBySeed_Ticket_UID(SeedTicketDataSet.vwSeed_Ticket_Total_Weights, UID);
                            }

                            using (SeedTicketDataSetTableAdapters.vwSeed_Ticket_Treatment_Rate_TotalsTableAdapter vwSeed_Ticket_Treatment_Rate_TotalsTableAdapter = new SeedTicketDataSetTableAdapters.vwSeed_Ticket_Treatment_Rate_TotalsTableAdapter())
                            {
                                vwSeed_Ticket_Treatment_Rate_TotalsTableAdapter.Fill(SeedTicketDataSet.vwSeed_Ticket_Treatment_Rate_Totals, UID);
                            }

                            using (SeedTicketDataSetTableAdapters.vwSeedTicket_Varieties_Weight_TotalsTableAdapter vwSeedTicket_Varieties_Weight_TotalsTableAdapter = new SeedTicketDataSetTableAdapters.vwSeedTicket_Varieties_Weight_TotalsTableAdapter())
                            {
                                vwSeedTicket_Varieties_Weight_TotalsTableAdapter.Fill(SeedTicketDataSet.vwSeedTicket_Varieties_Weight_Totals, UID);
                            }

                            using (SeedTicketDataSetTableAdapters.Seed_Ticket_WeightsTableAdapter Seed_Ticket_WeightsTableAdapter = new SeedTicketDataSetTableAdapters.Seed_Ticket_WeightsTableAdapter())
                            {

                                Seed_Ticket_WeightsTableAdapter.FillBySeed_Ticket_UID(SeedTicketDataSet.Seed_Ticket_Weights, UID);
                            }
                            if (string.IsNullOrEmpty(Printer))
                            {
                                Printer = Settings.workStation_SetupRow.Report_Printer;

                            }
                            //see if the printer is valid 
                            bool ValidPrinter = false;

                            System.Drawing.Printing.PrinterSettings settings = new System.Drawing.Printing.PrinterSettings();
                            string DefaultPrinter = settings.PrinterName;

                            foreach (string InstalledPrinter in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                            {
                                if (Printer.ToUpper() == InstalledPrinter.ToUpper())
                                {
                                    ValidPrinter = true;
                                    break;
                                }

                            }
                            if (!ValidPrinter) // if not valid use the system default printer
                            {
                                Printer = DefaultPrinter;
                            }
                            if (PrintTicket)
                            {

                                if (SeedTicketDataSet.Seed_Ticket_Print_Info.Count > 0)
                                {
                                    frm.SetPrompt(String.Format("Printing Outbound Ticket:{0}", SeedTicketDataSet.Seed_Ticket_Print_Info[0].Ticket));
                                    frm.Show();
                                    System.Windows.Forms.Application.DoEvents();

                                    Outbound_Seed_Ticket.SetDataSource(SeedTicketDataSet);
                                    // Outbound_Seed_Ticket.Subreports["Outbound_Seed_Ticket_Weights.rpt"].SetDataSource(SeedTicketDataSet);

                                    //foreach (ReportDocument  Report in Outbound_Seed_Ticket.Subreports["Outbound_Seed_Ticket_Details.rpt"].Subreports )
                                    //{
                                    //   // System.Diagnostics.Debug.Print(Report.Name);
                                    ////    Report.SetDataSource(SeedTicketDataSet);
                                    //}
                                    // The New Way to Get the Prenter Name Set
                                    CrystalDecisions.ReportAppServer.Controllers.PrintReportOptions printReportOptions = new CrystalDecisions.ReportAppServer.Controllers.PrintReportOptions();
                                    CrystalDecisions.ReportAppServer.Controllers.PrintOutputController printOutputController = new CrystalDecisions.ReportAppServer.Controllers.PrintOutputController();
                                    CrystalDecisions.ReportAppServer.ClientDoc.ISCDReportClientDocument rptClientDoc;
                                    rptClientDoc = Outbound_Seed_Ticket.ReportClientDocument;
                                    printReportOptions.PrinterName = Printer;
                                    printReportOptions.AddPrinterPageRange(1, 1);
                                    printReportOptions.NumberOfCopies = TicketCount;
                                    rptClientDoc.PrintOutputController.PrintReport(printReportOptions);


                                }

                            }

                        }
                    }
                }
                catch (Exception ocrap)
                {
                    System.Windows.Forms.MessageBox.Show("Cannot Print Ticket" + System.Environment.NewLine + ocrap.Message);

                    Logging.Add_System_Log("Printing.Print_INbound_Seed_Ticket Ticket=<" + UID.ToString() + "> Printer Name<" + Printer + ">", ocrap.Message);
                }
            }
        }










        #endregion
    }




}
