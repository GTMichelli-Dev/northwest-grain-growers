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

namespace RemotePrinting
{
    class Printing
    {

        private static string baseConnectionString = "Data Source={0};Initial Catalog=NW_Data;Persist Security Info=True;User ID=sa;Password=Scale_Us3r;TrustServerCertificate=True;Connection Timeout=15";




       





     







        public static MemoryStream PrintWeightSheet(Guid Weight_Sheet_UID,string serverName)
        {
            string connStr = string.Format(baseConnectionString, serverName);
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
                                vwWeigh_SheetTableAdapter.Connection = new System.Data.SqlClient.SqlConnection(connStr);
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




                                                CrystalDecisions.Shared.ExportOptions exportOpts = new CrystalDecisions.Shared.ExportOptions();
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
           //     Logging.Add_System_Log("Printing.PrintWeightSheetTicket Load UID<" + Weight_Sheet_UID.ToString() + "> MemoryStream ", ocrap.Message);

            }
            return stream;
        }



      



   



   


   









        public static bool PrintWeightSheet(Guid Weight_Sheet_UID,  string Printer ,string serverName )
        {

            string connStr = string.Format(baseConnectionString, serverName);



            try
            {


                using (Reports.Weigh_Sheet rptWeigh_Sheet = new Reports.Weigh_Sheet())
                {


                    using (NWDataset NWDataset = new NWDataset())
                    {

                  
                        {


                            using (NWDatasetTableAdapters.vwWeigh_SheetTableAdapter vwWeigh_SheetTableAdapter = new NWDatasetTableAdapters.vwWeigh_SheetTableAdapter())
                            {
                                NWDataset.EnforceConstraints = false;
                                vwWeigh_SheetTableAdapter.Connection = new System.Data.SqlClient.SqlConnection(connStr);

                                vwWeigh_SheetTableAdapter.FillByWeight_Sheet_UID(NWDataset.vwWeigh_Sheet, Weight_Sheet_UID);
                                {


                                    using (NWDatasetTableAdapters.vwWeight_Sheet_InformationTableAdapter vwWeight_Sheet_InformationTableAdapter = new NWDatasetTableAdapters.vwWeight_Sheet_InformationTableAdapter())
                                    {
                                        vwWeight_Sheet_InformationTableAdapter.Connection = new System.Data.SqlClient.SqlConnection(connStr);
                                        if (vwWeight_Sheet_InformationTableAdapter.FillByUID(NWDataset.vwWeight_Sheet_Information, Weight_Sheet_UID) > 0)
                                        {
                                            using (NWDatasetTableAdapters.LocationsTableAdapter locationsTableAdapter = new NWDatasetTableAdapters.LocationsTableAdapter())
                                            {
                                                locationsTableAdapter.Connection = new System.Data.SqlClient.SqlConnection(connStr);
                                                locationsTableAdapter.FillByLocationId(NWDataset.Locations, NWDataset.vwWeight_Sheet_Information[0].Location_Id);
                                            }
                                            {
                                             
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
                                                rptWeigh_Sheet.SetParameterValue(rptWeigh_Sheet.Parameter_Unofficial.ParameterFieldName, false);
                                                rptWeigh_Sheet.SetParameterValue(rptWeigh_Sheet.Parameter_Customer_Copy.ParameterFieldName, false);
                                                rptWeigh_Sheet.SetParameterValue(rptWeigh_Sheet.Parameter_Lot.ParameterFieldName, NWDataset.vwWeight_Sheet_Information[0].Lot_Number);
                                                rptWeigh_Sheet.SetParameterValue(rptWeigh_Sheet.Parameter_StrWS_Id.ParameterFieldName, NWDataset.vwWeight_Sheet_Information[0].WS_Id.ToString().PadLeft(8, '0'));
                                                rptWeigh_Sheet.SetParameterValue(rptWeigh_Sheet.Parameter_Closed.ParameterFieldName, NWDataset.vwWeight_Sheet_Information[0].WS_Id.ToString().PadLeft(8, '0'));
                                                // The New Way to Get the Printer Name Set
                                                CrystalDecisions.CrystalReports.Engine.PrintOptions printOptions = rptWeigh_Sheet.PrintOptions;
                                                printOptions.PrinterName = Printer;
                                                rptWeigh_Sheet.PrintToPrinter(1, false, 1, 1);
                                            }
                                        }
                                       // System.Threading.Thread.Sleep(1000);
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
                return false;
            }

        }

        public static bool PrintTransferWeightSheet(Guid Weight_Sheet_UID,  string Printer, string serverName)
        {

            string connStr = string.Format(baseConnectionString, serverName);


            try
            {


                using (Reports.Transfer_Weigh_Sheet Transfer_Weigh_Sheet = new Reports.Transfer_Weigh_Sheet())
                {


                    using (NWDataset NWDataset = new NWDataset())
                    {

                       
                        {


                            using (NWDatasetTableAdapters.vwTransfer_LoadTableAdapter vwTransfer_LoadTableAdapter = new NWDatasetTableAdapters.vwTransfer_LoadTableAdapter())
                            {
                                vwTransfer_LoadTableAdapter.Connection = new System.Data.SqlClient.SqlConnection(connStr);
                                vwTransfer_LoadTableAdapter.FillByWeight_Sheet_UID(NWDataset.vwTransfer_Load, Weight_Sheet_UID);
                                {

                                    using (NWDatasetTableAdapters.vwTransfer_Weight_Sheet_InformationTableAdapter vwTransfer_Weight_Sheet_InformationTableAdapter = new NWDatasetTableAdapters.vwTransfer_Weight_Sheet_InformationTableAdapter())
                                    {
                                        vwTransfer_Weight_Sheet_InformationTableAdapter.Connection = new System.Data.SqlClient.SqlConnection(connStr);
                                        if (vwTransfer_Weight_Sheet_InformationTableAdapter.FillByWeight_SheetUID(NWDataset.vwTransfer_Weight_Sheet_Information, Weight_Sheet_UID) > 0)
                                        {

                                            using (NWDatasetTableAdapters.LocationsTableAdapter locationsTableAdapter = new NWDatasetTableAdapters.LocationsTableAdapter())
                                            {
                                                locationsTableAdapter.Connection = new System.Data.SqlClient.SqlConnection(connStr);
                                                locationsTableAdapter.FillByLocationId(NWDataset.Locations, NWDataset.vwTransfer_Weight_Sheet_Information[0].Location_Id);
                                            }
                                            {
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
                                                Transfer_Weigh_Sheet.SetParameterValue(Transfer_Weigh_Sheet.Parameter_Unofficial.ParameterFieldName, false);
                                                Transfer_Weigh_Sheet.SetParameterValue(Transfer_Weigh_Sheet.Parameter_StrWS_Id.ParameterFieldName, NWDataset.vwTransfer_Weight_Sheet_Information[0].WS_Id.ToString().PadLeft(8, '0'));
                                                Transfer_Weigh_Sheet.SetParameterValue(Transfer_Weigh_Sheet.Parameter_Closed.ParameterFieldName, true);
                                                // The New Way to Get the Prenter Name Set
                                              


                                                CrystalDecisions.CrystalReports.Engine.PrintOptions printOptions = Transfer_Weigh_Sheet.PrintOptions;
                                                printOptions.PrinterName = Printer;
                                                Transfer_Weigh_Sheet.PrintToPrinter(1, false, 1, 1);


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
                return false;
            }

        }


        public static MemoryStream PrintTransferWeightSheet(Guid Weight_Sheet_UID, string serverName)
        {
            string connStr = string.Format(baseConnectionString, serverName);
           
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
                                vwTransfer_LoadTableAdapter.Connection = new System.Data.SqlClient.SqlConnection(connStr);
                                vwTransfer_LoadTableAdapter.FillByWeight_Sheet_UID(NWDataset.vwTransfer_Load, Weight_Sheet_UID);
                                {

                                    using (NWDatasetTableAdapters.vwTransfer_Weight_Sheet_InformationTableAdapter vwTransfer_Weight_Sheet_InformationTableAdapter = new NWDatasetTableAdapters.vwTransfer_Weight_Sheet_InformationTableAdapter())
                                    {
                                        if (vwTransfer_Weight_Sheet_InformationTableAdapter.FillByWeight_SheetUID(NWDataset.vwTransfer_Weight_Sheet_Information, Weight_Sheet_UID) > 0)
                                        {

                                            using (NWDatasetTableAdapters.LocationsTableAdapter locationsTableAdapter = new NWDatasetTableAdapters.LocationsTableAdapter())
                                            {
                                                locationsTableAdapter.Connection = new System.Data.SqlClient.SqlConnection(connStr);
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

         
            }
            return stream;
        }




 
    }




}
