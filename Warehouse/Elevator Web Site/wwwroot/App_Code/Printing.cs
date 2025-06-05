using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CrystalDecisions.CrystalReports;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Web;
using System.Web.UI.WebControls;
using CrystalDecisions;
using Newtonsoft.Json;
using System.IO;
using System.Text;

using System.Xml.Serialization;

using System.Xml.Linq;
using System.Activities;

/// <summary>
/// Summary description for Printing
/// </summary>
public class Printing
{
    public Printing()
    {
        //
        // TODO: Add constructor logic here
        //
    }


    public static void UpdatePrinters()
    {
        try

        {
            using (LocalDataSetTableAdapters.QueriesTableAdapter Q = new LocalDataSetTableAdapters.QueriesTableAdapter())
            {
                Q.DeletePrinters();
                string ServerName = Q.ServerName(); 
                using (LocalDataSetTableAdapters.Site_PrintersTableAdapter  printersTableAdapter = new LocalDataSetTableAdapters.Site_PrintersTableAdapter())
                {

                    foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                    {
                        if (!printer.Contains("Microsoft XPS Document Writer") && !printer.Contains("Microsoft Print to PDF"))
                        {
                            printersTableAdapter.Insert(Guid.NewGuid(), ServerName, printer);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //SiteSetup.LogLocalMessage("Error Getting Printers.. " + ex.Message);
        }
    }


    




    public  class SelectedPrinter
    {
        public  SelectedPrinter()
        {
            System.Drawing.Printing.PrinterSettings settings = new System.Drawing.Printing.PrinterSettings();
            string DefaultPrinter = settings.PrinterName;
            Enabled = true;
            PrinterName = DefaultPrinter;
        }
        public string DefaultPrinter;
        public bool Enabled;
        public string PrinterName;

    }


    private static SelectedPrinter DocumentPrinter(string Scale, String Printer, bool Inbound, int Location_Id)
    {
        SelectedPrinter PrinterToUse = new global::Printing.SelectedPrinter();
       

        if (string.IsNullOrEmpty(Printer))
        {
            if (!string.IsNullOrEmpty(Scale))
            {
                LocalDataSet.Weigh_ScalesRow ScaleUsed = Scales.GetScale(Scale, Location_Id);
                if (ScaleUsed == null)
                {
                    Logging.Add_System_Log("Printing.cs .PrinterName ", string.Format("Cannot Find Scale{0} For Printer", Scale), Location_Id);
                }
                else
                {
                    if (Inbound)
                    {
                        PrinterToUse.PrinterName = ScaleUsed.Inbound_Ticket_Printer;
                        PrinterToUse.Enabled = ScaleUsed.Print_Inbound_Ticket;
                    }
                    else
                    {
                        PrinterToUse.PrinterName = ScaleUsed.Outbound_Ticket_Printer;
                        PrinterToUse.Enabled = ScaleUsed.Print_Outbound_Ticket;
                    }
                }
            }
            else
            {
                PrinterToUse.PrinterName = Printer;
            }
        }
        else
        {
            PrinterToUse.PrinterName = Printer;
        }
        bool ValidPrinter = false;
        try
        {
            foreach (string InstalledPrinter in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                if (PrinterToUse.PrinterName.ToUpper() == InstalledPrinter.ToUpper())
                {
                    PrinterToUse.PrinterName = InstalledPrinter;
                    ValidPrinter = true;
                    break;
                }

            }
        }
        catch (Exception ex)
        {
            Logging.Add_System_Log("Web.Printing.DocumentPrinter Installed Printers Error Printer Name<" + Printer + ">", ex.Message, Location_Id);

        }
        if (!ValidPrinter) // if not valid use the system default printer
        {
            PrinterToUse.PrinterName = PrinterToUse.DefaultPrinter;
            PrinterToUse.Enabled = true;
        }




        return PrinterToUse;
    }





    public static bool Print_Basic_Ticket(HttpServerUtility server,string Description, int LocationId, int Weight, string Scale = "",  string Printer = "")
    {

        try
        {
            string Location = "";
            using (LocalDataSet.LocationsDataTable   Locations = new LocalDataSet.LocationsDataTable())
            {
                using (LocalDataSetTableAdapters.LocationsTableAdapter locationsTableAdapter= new LocalDataSetTableAdapters.LocationsTableAdapter())
                {

                   if ( locationsTableAdapter.FillByID(Locations,LocationId )>0)
                    {
                        Location = Locations[0].Description; 
                    }
                }
            }

            using (ReportDocument Basic_Ticket = new ReportDocument())
            {

                {
                    SelectedPrinter PrintertoUse = DocumentPrinter(Scale, Printer, true, LocationId);
                    if (PrintertoUse.Enabled)
                    {


                        Basic_Ticket.Load(server.MapPath("~/Reports/BasicTicket.rpt"));
                        Basic_Ticket.SetParameterValue("Weight", Weight);
                        Basic_Ticket.SetParameterValue("Time", DateTime.Now);
                        Basic_Ticket.SetParameterValue("Description", Description);
                        Basic_Ticket.SetParameterValue("Location", Location);
                        CrystalDecisions.Shared.PageMargins margins = Basic_Ticket.PrintOptions.PageMargins;
                        margins.bottomMargin = 0;
                        margins.topMargin = 0;
                        Basic_Ticket.PrintOptions.ApplyPageMargins(margins);

                        PrintTicket(Basic_Ticket, PrintertoUse.PrinterName, 1);











                    }
                }
            }
            return true;
        }
        catch (Exception ocrap)
        {

            Logging.Add_System_Log("Web.Printing.Print_Basic_Ticket > Printer Name<" + Printer + ">", ocrap.Message, LocationId);
            return false;
        }
    }





    public static string Print_Test_Ticket(HttpServerUtility server, string Printer = "")
    {

        try
        {
            using (ReportDocument Test_Ticket = new ReportDocument())
            {

                {
                
                    {


                        Test_Ticket.Load(server.MapPath("~/Reports/TestTicket.rpt"));
                      
                        CrystalDecisions.Shared.PageMargins margins = Test_Ticket.PrintOptions.PageMargins;
                        margins.bottomMargin = 0;
                        margins.topMargin = 0;
                        Test_Ticket.PrintOptions.ApplyPageMargins(margins);
                        Test_Ticket.SetParameterValue("PrinterName", Printer);
                        PrintTicket(Test_Ticket, Printer, 1);










                    }
                }
            }
            return "Test Ticket Printed";
        }
        catch (Exception ocrap)
        {

            Logging.Add_System_Log("Web.Printing.Print_Basic_Ticket > Printer Name<" + Printer + ">", ocrap.Message, -1);
            return ocrap.Message;
        }
    }


    public static void PrintTicket(ReportDocument DocumentToPrint, string Printer , int NumberOfCopies)
    {
        if (Printer == "Print To Browser")
        {

            ExportOptions exportOpts = new ExportOptions();
            exportOpts.ExportFormatType = ExportFormatType.PortableDocFormat;

            EditableRTFExportFormatOptions exportFormatOptions =
              ExportOptions.CreateEditableRTFExportFormatOptions();
            exportFormatOptions.InsertPageBreaks = true;
            exportOpts.ExportFormatOptions = exportFormatOptions;


            Stream pdf = DocumentToPrint.ExportToStream(ExportFormatType.PortableDocFormat);//   exportOpts, HttpContext.Current.Response, Download, string.Format("BOL_{0}.pdf", printingDataSet.Outbound_Load_Info[0].BOL_Number));
            MemoryStream ms = new MemoryStream();
            pdf.CopyTo(ms);
           
            
        }
        else
        {




            // The New Way to Get the Prenter Name Set
            CrystalDecisions.ReportAppServer.Controllers.PrintReportOptions printReportOptions = new CrystalDecisions.ReportAppServer.Controllers.PrintReportOptions();
            CrystalDecisions.ReportAppServer.Controllers.PrintOutputController printOutputController = new CrystalDecisions.ReportAppServer.Controllers.PrintOutputController();
            CrystalDecisions.ReportAppServer.ClientDoc.ISCDReportClientDocument rptClientDoc;
            rptClientDoc = DocumentToPrint.ReportClientDocument;

            printReportOptions.PrinterName = Printer;
            printReportOptions.NumberOfCopies = NumberOfCopies;
            rptClientDoc.PrintOutputController.PrintReport(printReportOptions);

        }
    }

    public static void SetKioskMessage(string Message, int LocationId, string PrinterName)
    {

        if (string.IsNullOrEmpty(Message)) return;
        if (string.IsNullOrEmpty(PrinterName)) PrinterName = "";
        
        try
        {
            var prompt = Kiosk.ScalePrompts.FirstOrDefault(x => x.PrinterName.ToUpper() == PrinterName.ToUpper());
            if (prompt == null)
            {
                return; // No prompt found for this printer, so we do not set a message.
            }
            
            prompt.ServerMessage = Message;
            prompt.MessageTimeOut = 5000;
            
        }
        catch (Exception ex)
        {
            Logging.Add_System_Log("Web.Printing.SetKioskMessage", ex.Message, LocationId);
        }
    }

    public static void PrintInbound_Inyard_Ticket(HttpServerUtility server, Guid LoadUID, int LocationId, string Scale = "", string Printer = "")
    {

        try
        {

            using (ReportDocument Inbound_Inyard_Ticket = new ReportDocument())
            {
                using (LocalDataSet  localDataSet = new LocalDataSet())
                {
                    SelectedPrinter PrintertoUse = DocumentPrinter(Scale, Printer, true, LocationId);
                    if (PrintertoUse.Enabled)
                    {
                        using (LocalDataSetTableAdapters.vwWeigh_SheetTableAdapter vwWeigh_SheetTableAdapter = new LocalDataSetTableAdapters.vwWeigh_SheetTableAdapter())
                        {

                            if (vwWeigh_SheetTableAdapter.FillByLoad_UID(localDataSet.vwWeigh_Sheet, LoadUID) > 0)
                            {
                                Inbound_Inyard_Ticket.Load(server.MapPath("~/Reports/Inbound_InYard_Ticket.rpt"));
                                Inbound_Inyard_Ticket.SetDataSource(localDataSet);
                                CrystalDecisions.Shared.PageMargins margins = Inbound_Inyard_Ticket.PrintOptions.PageMargins;
                                margins.bottomMargin = 0;
                                margins.topMargin = 0;
                                Inbound_Inyard_Ticket.PrintOptions.ApplyPageMargins(margins);

                                PrintTicket(Inbound_Inyard_Ticket, PrintertoUse.PrinterName, 1);
                                SetKioskMessage("Printing <br /> Inbound Ticket", LocationId, PrintertoUse.PrinterName);


                                string DirectoryPath = @"c:\ScaleTickets";
                                string Filename = DirectoryPath + @"\" + LocationId.ToString() + "_" + localDataSet.vwWeigh_Sheet[0].Load_Id.ToString() + ".pdf";
                                if (!System.IO.Directory.Exists(DirectoryPath)) { System.IO.Directory.CreateDirectory(DirectoryPath); }
                                if (!System.IO.File.Exists(Filename)) { System.IO.File.Delete(Filename); }
                                Inbound_Inyard_Ticket.ExportToDisk(ExportFormatType.PortableDocFormat, Filename);




                                //CrystalDecisions.CrystalReports.Engine.PrintOptions repOptions;
                                //Inbound_Inyard_Ticket.Load(server.MapPath("~/Reports/Inbound_InYard_Ticket.rpt"));
                                //Inbound_Inyard_Ticket.SetDataSource(localDataSet);
                                //repOptions = Inbound_Inyard_Ticket.PrintOptions;
                                //repOptions.PrinterName = PrintertoUse.PrinterName;
                                //CrystalDecisions.Shared.PageMargins margins = Inbound_Inyard_Ticket.PrintOptions.PageMargins;
                                //margins.bottomMargin = 0;
                                //margins.topMargin = 0;


                                //Inbound_Inyard_Ticket.PrintOptions.ApplyPageMargins(margins);
                                //Inbound_Inyard_Ticket.PrintToPrinter(1, false, 1, 1);



                                //string DirectoryPath = @"c:\ScaleTickets";
                                //string Filename = DirectoryPath + @"\" + NWDataset.vwWeigh_Sheet[0].Location_Id.ToString() + "_" + NWDataset.vwWeigh_Sheet[0].Load_Id.ToString()  + ".pdf";
                                //if (!System.IO.Directory.Exists(DirectoryPath)) { System.IO.Directory.CreateDirectory(DirectoryPath); }
                                //if (!System.IO.File.Exists(Filename)) { System.IO.File.Delete(Filename); }
                                //OutboundTicket.ExportToDisk(ExportFormatType.PortableDocFormat, Filename);

                            }

                        }
                    }
                }
            }
        }
        catch (Exception ocrap)
        {

            Logging.Add_System_Log("Web.Printing.PrintInboundInyardTicket Load UID<" + LoadUID.ToString() + "> Printer Name<" + Printer + ">", ocrap.Message, LocationId);

        }
    }


    public static void PrintInbound_Final_Ticket(HttpServerUtility server, Guid LoadUID, int LocationId, string Scale = "", string Printer = "")
    {
        try
        {
            using (ReportDocument OutboundTicket = new ReportDocument())
            {
                using (LocalDataSet localDataSet = new LocalDataSet())
                {
                    SelectedPrinter PrintertoUse = DocumentPrinter(Scale, Printer, false, LocationId);
                    if (PrintertoUse.Enabled)
                    {


                        using (LocalDataSetTableAdapters.vwWeigh_SheetTableAdapter vwWeigh_SheetTableAdapter = new LocalDataSetTableAdapters.vwWeigh_SheetTableAdapter())
                        {
                            if (vwWeigh_SheetTableAdapter.FillByLoad_UID(localDataSet.vwWeigh_Sheet, LoadUID) > 0)
                            {

                                //ReceivingQRData QRData= new ReceivingQRData() { 
                                //    LoadUID=LoadUID,
                                //    LoadID= localDataSet.vwWeigh_Sheet[0].Load_Id,
                                //    Location_ID = localDataSet.vwWeigh_Sheet[0].Location_Id,
                                //    TimeOut = localDataSet.vwWeigh_Sheet[0].Time_Out,
                                //    //TimeIn = LocalDataSet.vwWeigh_Sheet[0].Time_In,
                                //    //WeightIn = LocalDataSet.vwWeigh_Sheet[0].Weight_In,
                                //    //WeightOut = LocalDataSet.vwWeigh_Sheet[0].Weight_Out,
                                //    //TruckID =(LocalDataSet.vwWeigh_Sheet[0].IsTruck_IdNull())?"": LocalDataSet.vwWeigh_Sheet[0].Truck_Id
                                //};


                                StringBuilder sb = new StringBuilder();
                                sb.AppendLine($"LOADUID={LoadUID},");
                                sb.AppendLine($"LOADID={localDataSet.vwWeigh_Sheet[0].Load_Id},");
                                sb.AppendLine($"LOCATIONID={localDataSet.vwWeigh_Sheet[0].Location_Id},");
                                sb.AppendLine($"TIMEOUT={localDataSet.vwWeigh_Sheet[0].Time_Out}");

                                Zen.Barcode.CodeQrBarcodeDraw barcode = new Zen.Barcode.CodeQrBarcodeDraw();
                                System.Drawing.Image img = barcode.Draw(sb.ToString(), 100);

                                Logging.Add_System_Log("Print Inbound Final Ticket", sb.ToString());

                                MemoryStream ms = new MemoryStream();
                                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);




                                //System.Drawing.Image img = barcode.Draw(BarcodesRow.Barcode + "0", 100);\


                             //   string xmlData = ConvertObjectToXMLString(QRData);


                             //string jsonData = JsonConvert.SerializeObject(QRData);
                             //   jsonData= jsonData.Replace('"', '~');
                             //   System.Diagnostics.Debug.Print(xmlData);
                             //   Zen.Barcode.CodeQrBarcodeDraw barcode = new Zen.Barcode.CodeQrBarcodeDraw();
                             //   System.Drawing.Image img = barcode.Draw(jsonData, 100);

                             //   MemoryStream ms = new MemoryStream();
                             //   img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                             //   //System.Drawing.Image img = barcode.Draw(BarcodesRow.Barcode + "0", 100);\
                                localDataSet.QRCode.AddQRCodeRow(QR: ms.ToArray());



                                //CrystalDecisions.CrystalReports.Engine.PrintOptions repOptions;
                                OutboundTicket.Load(server.MapPath("~/Reports/Inbound_Final_Ticket.rpt"));
                                OutboundTicket.SetDataSource(localDataSet);
                                //repOptions = OutboundTicket.PrintOptions;
                                //repOptions.PrinterName = PrintertoUse.PrinterName;
                                CrystalDecisions.Shared.PageMargins margins = OutboundTicket.PrintOptions.PageMargins;
                                margins.bottomMargin = 0;
                                margins.topMargin = 0;


                                OutboundTicket.PrintOptions.ApplyPageMargins(margins);


                               

                                PrintTicket(OutboundTicket, PrintertoUse.PrinterName, 1);
                                SetKioskMessage("Printing <br /> Inbound Ticket", LocationId, PrintertoUse.PrinterName);


                                // OutboundTicket.PrintToPrinter(1, false, 1, 1);

                                string DirectoryPath = @"c:\ScaleTickets";
                                string Filename = DirectoryPath + @"\" + LocationId.ToString() + "_" + localDataSet.vwWeigh_Sheet[0].Load_Id.ToString() + ".pdf";
                                if (!System.IO.Directory.Exists(DirectoryPath)) { System.IO.Directory.CreateDirectory(DirectoryPath); }
                                if (!System.IO.File.Exists(Filename)) { System.IO.File.Delete(Filename); }
                                OutboundTicket.ExportToDisk(ExportFormatType.PortableDocFormat, Filename);

                                //string DirectoryPath = @"c:\ScaleTickets";
                                //string Filename = DirectoryPath + @"\" + NWDataset.vwWeigh_Sheet[0].Location_Id.ToString() + "_" + NWDataset.vwWeigh_Sheet[0].Load_Id.ToString()  + ".pdf";
                                //if (!System.IO.Directory.Exists(DirectoryPath)) { System.IO.Directory.CreateDirectory(DirectoryPath); }
                                //if (!System.IO.File.Exists(Filename)) { System.IO.File.Delete(Filename); }
                                //OutboundTicket.ExportToDisk(ExportFormatType.PortableDocFormat, Filename);
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ocrap)
        {

            Logging.Add_System_Log("Web.Printing. PrintInbound_Final_Ticket Load UID<" + LoadUID.ToString() + "> Printer Name<" + Printer + ">", ocrap.Message, LocationId);

        }
    }


    public static void PrintOutbound_InYard_Ticket(HttpServerUtility server, Guid LoadUID, int LocationId, string Scale = "", bool Only_One = false, string Printer = "")
    {
        try
        {
            using (ReportDocument OutboundInyardTicket = new ReportDocument())
            {
                using (LocalDataSet localDataSet = new LocalDataSet())
                {
                    SelectedPrinter PrintertoUse = DocumentPrinter(Scale, Printer, true, LocationId);
                    if (PrintertoUse.Enabled)
                    {



                        using (LocalDataSetTableAdapters.vw_Outbound_LoadTableAdapter vw_Outbound_LoadTableAdapter = new LocalDataSetTableAdapters.vw_Outbound_LoadTableAdapter())
                        {
                            if (vw_Outbound_LoadTableAdapter.FillByLoad_UID(localDataSet.vw_Outbound_Load, LoadUID) > 0)
                            {
                               // CrystalDecisions.CrystalReports.Engine.PrintOptions repOptions;
                                OutboundInyardTicket.Load(server.MapPath("~/Reports/Outbound_InYard_Ticket.rpt"));
                                OutboundInyardTicket.SetDataSource(localDataSet);
                               // repOptions = OutboundInyardTicket.PrintOptions;
                               // repOptions.PrinterName = PrintertoUse.PrinterName;
                                CrystalDecisions.Shared.PageMargins margins = OutboundInyardTicket.PrintOptions.PageMargins;
                                margins.bottomMargin = 0;
                                margins.topMargin = 0;
                                OutboundInyardTicket.PrintOptions.ApplyPageMargins(margins);
                                PrintTicket(OutboundInyardTicket, PrintertoUse.PrinterName, 1);
                                // OutboundInyardTicket.PrintToPrinter(1, false, 1, 1);
                                SetKioskMessage("Printing <br /> Outbound Ticket", LocationId, PrintertoUse.PrinterName);


                                string DirectoryPath = @"c:\ScaleTickets";
                                string Filename = DirectoryPath + @"\" + LocationId.ToString() + "_" + localDataSet.vw_Outbound_Load[0].Load_Id.ToString() + ".pdf";
                                if (!System.IO.Directory.Exists(DirectoryPath)) { System.IO.Directory.CreateDirectory(DirectoryPath); }
                                if (!System.IO.File.Exists(Filename)) { System.IO.File.Delete(Filename); }
                                OutboundInyardTicket.ExportToDisk(ExportFormatType.PortableDocFormat, Filename);

                                //string DirectoryPath = @"c:\ScaleTickets";
                                //string Filename = DirectoryPath + @"\" + NWDataset.vwWeigh_Sheet[0].Location_Id.ToString() + "_" + NWDataset.vwWeigh_Sheet[0].Load_Id.ToString()  + ".pdf";
                                //if (!System.IO.Directory.Exists(DirectoryPath)) { System.IO.Directory.CreateDirectory(DirectoryPath); }
                                //if (!System.IO.File.Exists(Filename)) { System.IO.File.Delete(Filename); }
                                //OutboundTicket.ExportToDisk(ExportFormatType.PortableDocFormat, Filename);

                            }
                            else
                            {
                                Logging.Add_System_Log("Web.Printing.PrintOutbound_InYard_Ticket Load UID<" + LoadUID.ToString() + ">", "Could Not Find Load_UID", LocationId);
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ocrap)
        {

            Logging.Add_System_Log("Web.Printing.PrintOutbound_InYard_Ticket Load UID<" + LoadUID.ToString() + "> Printer Name<" + Printer + ">", ocrap.Message, LocationId);

        }
    }


    public static void PrintOutbound_Final_Ticket(HttpServerUtility server, Guid LoadUID, int LocationId, string Scale = "", bool Only_One = false, string Printer = "")
    {

        try
        {
            using (ReportDocument OutboundTicket = new ReportDocument())
            {
                using (LocalDataSet localDataSet = new LocalDataSet())
                {
                    SelectedPrinter PrintertoUse = DocumentPrinter(Scale, Printer, true, LocationId);
                    if (PrintertoUse.Enabled)
                    {




                        using (LocalDataSetTableAdapters.vw_Outbound_LoadTableAdapter vw_Outbound_LoadTableAdapter = new LocalDataSetTableAdapters.vw_Outbound_LoadTableAdapter())
                        {
                            if (vw_Outbound_LoadTableAdapter.FillByLoad_UID(localDataSet.vw_Outbound_Load, LoadUID) > 0)
                            {
                                //CrystalDecisions.CrystalReports.Engine.PrintOptions repOptions;
                                OutboundTicket.Load(server.MapPath("~/Reports/Outbound_Final_Ticket.rpt"));
                                OutboundTicket.SetDataSource(localDataSet);
                                //repOptions = OutboundTicket.PrintOptions;
                                //repOptions.PrinterName = PrintertoUse.PrinterName ;
                                CrystalDecisions.Shared.PageMargins margins = OutboundTicket.PrintOptions.PageMargins;
                                margins.bottomMargin = 0;
                                margins.topMargin = 0;

                                int Number_Of_Copies = localDataSet.Site_Setup[0].Outbound_Final_Kiosk_Ticket_Count;
                                if (Only_One) Number_Of_Copies = 1;
                                for (int i = 0; i < Number_Of_Copies; i++)
                                {
                                    OutboundTicket.PrintOptions.ApplyPageMargins(margins);
                                    PrintTicket(OutboundTicket, PrintertoUse.PrinterName, 1);
                                    //OutboundTicket.PrintToPrinter(1, false, 1, 1);
                                }
                                SetKioskMessage("Printing <br /> Outbound Ticket", LocationId, PrintertoUse.PrinterName);

                                string DirectoryPath = @"c:\ScaleTickets";
                                string Filename = DirectoryPath + @"\" + LocationId.ToString() + "_" + localDataSet.vw_Outbound_Load[0].Load_Id.ToString() + ".pdf";
                                if (!System.IO.Directory.Exists(DirectoryPath)) { System.IO.Directory.CreateDirectory(DirectoryPath); }
                                if (!System.IO.File.Exists(Filename)) { System.IO.File.Delete(Filename); }
                                OutboundTicket.ExportToDisk(ExportFormatType.PortableDocFormat, Filename);

                                //string DirectoryPath = @"c:\ScaleTickets";
                                //string Filename = DirectoryPath + @"\" + NWDataset.vwWeigh_Sheet[0].Location_Id.ToString() + "_" + NWDataset.vwWeigh_Sheet[0].Load_Id.ToString()  + ".pdf";
                                //if (!System.IO.Directory.Exists(DirectoryPath)) { System.IO.Directory.CreateDirectory(DirectoryPath); }
                                //if (!System.IO.File.Exists(Filename)) { System.IO.File.Delete(Filename); }
                                //OutboundTicket.ExportToDisk(ExportFormatType.PortableDocFormat, Filename);

                            }
                            else
                            {
                                Logging.Add_System_Log("Web.Printing.PrintOutbound_Final_Ticket Load UID<" + LoadUID.ToString() + ">", "Could Not Find Load_UID", LocationId);
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ocrap)
        {

            Logging.Add_System_Log("Web.Printing.PrintOutbound_Final_Ticket Load UID<" + LoadUID.ToString() + "> Printer Name<" + Printer + ">", ocrap.Message, LocationId);

        }
    }

    public static void PrintOutbound_Final_Office_Ticket(HttpServerUtility server, Guid LoadUID, int LocationId, string Printer = "")
    {

        try
        {
            using (ReportDocument OutboundTicket = new ReportDocument())
            {
                using (LocalDataSet localDataSet = new LocalDataSet())
                {
                    if (string.IsNullOrEmpty(Printer))
                    {
                        System.Drawing.Printing.PrinterSettings settings = new System.Drawing.Printing.PrinterSettings();
                        Printer = settings.PrinterName;
                    }
                    using (LocalDataSetTableAdapters.vw_Outbound_LoadTableAdapter vw_Outbound_LoadTableAdapter = new LocalDataSetTableAdapters.vw_Outbound_LoadTableAdapter())
                    {
                        if (vw_Outbound_LoadTableAdapter.FillByLoad_UID(localDataSet.vw_Outbound_Load, LoadUID) > 0)
                        {
                            //CrystalDecisions.CrystalReports.Engine.PrintOptions repOptions;
                            OutboundTicket.Load(server.MapPath("~/Reports/Outbound_Final_Ticket_2_Part.rpt"));
                            OutboundTicket.SetDataSource(localDataSet);
                            //repOptions = OutboundTicket.PrintOptions;
                            //repOptions.PrinterName = Printer;
                            CrystalDecisions.Shared.PageMargins margins = OutboundTicket.PrintOptions.PageMargins;
                            margins.bottomMargin = 0;
                            margins.topMargin = 0;


                            OutboundTicket.PrintOptions.ApplyPageMargins(margins);
                            //OutboundTicket.PrintToPrinter(1, false, 1, 1);
                            PrintTicket(OutboundTicket, Printer, 1);

                            string DirectoryPath = @"c:\ScaleTickets";
                            string Filename = DirectoryPath + @"\" + LocationId.ToString() + "_" + localDataSet.vw_Outbound_Load[0].Load_Id.ToString() + ".pdf";
                            if (!System.IO.Directory.Exists(DirectoryPath)) { System.IO.Directory.CreateDirectory(DirectoryPath); }
                            if (!System.IO.File.Exists(Filename)) { System.IO.File.Delete(Filename); }
                            OutboundTicket.ExportToDisk(ExportFormatType.PortableDocFormat, Filename);

                            //string DirectoryPath = @"c:\ScaleTickets";
                            //string Filename = DirectoryPath + @"\" + NWDataset.vwWeigh_Sheet[0].Location_Id.ToString() + "_" + NWDataset.vwWeigh_Sheet[0].Load_Id.ToString()  + ".pdf";
                            //if (!System.IO.Directory.Exists(DirectoryPath)) { System.IO.Directory.CreateDirectory(DirectoryPath); }
                            //if (!System.IO.File.Exists(Filename)) { System.IO.File.Delete(Filename); }
                            //OutboundTicket.ExportToDisk(ExportFormatType.PortableDocFormat, Filename);

                        }
                        else
                        {
                            Logging.Add_System_Log("Web.Printing.PrintOutbound_Final_Office_Ticket Load UID<" + LoadUID.ToString() + ">", "Could Not Find Load_UID", LocationId);
                        }
                    }
                }
            }
        }
        catch (Exception ocrap)
        {

            Logging.Add_System_Log("Web.Printing.PrintOutbound_Final_Office_Ticket Load UID<" + LoadUID.ToString() + "> Printer Name<" + Printer + ">", ocrap.Message, LocationId);

        }
    }


    public static void PrintTransfer_InYard_Ticket(HttpServerUtility server, Guid LoadUID, int LocationId, string Scale = "", string Printer = "")
    {

        try
        {
            using (ReportDocument OutboundTicket = new ReportDocument())
            {

                using (LocalDataSet localDataSet = new LocalDataSet())
                {
                    SelectedPrinter PrintertoUse = DocumentPrinter(Scale, Printer, true, LocationId);
                    if (PrintertoUse.Enabled)
                    {



                        using (LocalDataSetTableAdapters.vwTransfer_LoadTableAdapter vwTransfer_LoadTableAdapter = new LocalDataSetTableAdapters.vwTransfer_LoadTableAdapter())
                        {
                            if (vwTransfer_LoadTableAdapter.FillByLoad_UID(localDataSet.vwTransfer_Load, LoadUID) > 0)
                            {
                                //CrystalDecisions.CrystalReports.Engine.PrintOptions repOptions;
                                OutboundTicket.Load(server.MapPath("~/Reports/Transfer_Inyard_Ticket.rpt"));
                                OutboundTicket.SetDataSource(localDataSet);
                                //repOptions = OutboundTicket.PrintOptions;
                                //repOptions.PrinterName = PrintertoUse.PrinterName;
                                CrystalDecisions.Shared.PageMargins margins = OutboundTicket.PrintOptions.PageMargins;
                                margins.bottomMargin = 0;
                                margins.topMargin = 0;
                                OutboundTicket.PrintOptions.ApplyPageMargins(margins);
                                // OutboundTicket.PrintToPrinter(1, false, 1, 1);
                                PrintTicket(OutboundTicket, PrintertoUse.PrinterName, 1);
                                SetKioskMessage("Printing <br /> In Yard Ticket", LocationId, PrintertoUse.PrinterName);

                                string DirectoryPath = @"c:\ScaleTickets";
                                string Filename = DirectoryPath + @"\" + LocationId.ToString() + "_" + localDataSet.vwTransfer_Load[0].Load_Id.ToString() + ".pdf";
                                if (!System.IO.Directory.Exists(DirectoryPath)) { System.IO.Directory.CreateDirectory(DirectoryPath); }
                                if (!System.IO.File.Exists(Filename)) { System.IO.File.Delete(Filename); }
                                OutboundTicket.ExportToDisk(ExportFormatType.PortableDocFormat, Filename);

                            }
                        }
                    }
                }
            }
        }
        catch (Exception ocrap)
        {

            Logging.Add_System_Log("Web.Printing.PrintTransfer_InYard_Ticket Load UID<" + LoadUID.ToString() + "> Printer Name<" + Printer + ">", ocrap.Message, LocationId);

        }
    }


    public static void PrintTransfer_Final_Ticket(HttpServerUtility server, Guid LoadUID, int LocationId, string Scale = "", string Printer = "")
    {

        try
        {
            using (ReportDocument OutboundTicket = new ReportDocument())
            {
                using (LocalDataSet localDataSet = new LocalDataSet())
                {

                    SelectedPrinter PrintertoUse = DocumentPrinter(Scale, Printer, false, LocationId);
                    if (PrintertoUse.Enabled)
                    {



                        using (LocalDataSetTableAdapters.vwTransfer_LoadTableAdapter vwTransfer_LoadTableAdapter = new LocalDataSetTableAdapters.vwTransfer_LoadTableAdapter())
                        {
                            if (vwTransfer_LoadTableAdapter.FillByLoad_UID(localDataSet.vwTransfer_Load, LoadUID) > 0)
                            {


                                //ReceivingQRData QRData = new ReceivingQRData()
                                //{
                                //    LoadUID = LoadUID,
                                //    LoadID = localDataSet.vwTransfer_Load[0].Load_Id,
                                //    Location_ID = localDataSet.vwTransfer_Load[0].Location_Id,
                                //    TimeOut = localDataSet.vwTransfer_Load[0].Time_Out,
                                //    //TimeIn = LocalDataSet.vwWeigh_Sheet[0].Time_In,
                                //    //WeightIn = LocalDataSet.vwWeigh_Sheet[0].Weight_In,
                                //    //WeightOut = LocalDataSet.vwWeigh_Sheet[0].Weight_Out,
                                //    //TruckID =(LocalDataSet.vwWeigh_Sheet[0].IsTruck_IdNull())?"": LocalDataSet.vwWeigh_Sheet[0].Truck_Id
                                //};

                                StringBuilder sb = new StringBuilder();
                                sb.AppendLine($"LOADUID={LoadUID},");
                                sb.AppendLine($"LOADID={localDataSet.vwTransfer_Load[0].Load_Id},");
                                sb.AppendLine($"LOCATIONID={localDataSet.vwTransfer_Load[0].Location_Id},");
                                sb.AppendLine($"TIMEOUT={localDataSet.vwTransfer_Load[0].Time_Out}");

                                Zen.Barcode.CodeQrBarcodeDraw barcode = new Zen.Barcode.CodeQrBarcodeDraw();
                                System.Drawing.Image img = barcode.Draw(sb.ToString(), 100);

                                Logging.Add_System_Log("Print Transfer Final Ticket", sb.ToString());

                                MemoryStream ms = new MemoryStream();
                                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                                localDataSet.QRCode.AddQRCodeRow(QR: ms.ToArray());
                                //string jsonData = JsonConvert.SerializeObject(QRData);
                                //System.Diagnostics.Debug.Print(jsonData);
                                //Zen.Barcode.CodeQrBarcodeDraw barcode = new Zen.Barcode.CodeQrBarcodeDraw();
                                //System.Drawing.Image img = barcode.Draw(jsonData, 100);

                                //MemoryStream ms = new MemoryStream();
                                //img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                                ////System.Drawing.Image img = barcode.Draw(BarcodesRow.Barcode + "0", 100);\
                                //localDataSet.QRCode.AddQRCodeRow(QR: ms.ToArray());






                                //CrystalDecisions.CrystalReports.Engine.PrintOptions repOptions;
                                OutboundTicket.Load(server.MapPath("~/Reports/Transfer_Final_Ticket.rpt"));
                                OutboundTicket.SetDataSource(localDataSet);
                                //repOptions = OutboundTicket.PrintOptions;
                                //repOptions.PrinterName = PrintertoUse.PrinterName ;
                                CrystalDecisions.Shared.PageMargins margins = OutboundTicket.PrintOptions.PageMargins;
                                margins.bottomMargin = 0;
                                margins.topMargin = 0;
                                OutboundTicket.PrintOptions.ApplyPageMargins(margins);
                                //OutboundTicket.PrintToPrinter(1, false, 1, 1);
                                PrintTicket(OutboundTicket, PrintertoUse.PrinterName, 1);
                                SetKioskMessage("Printing <br /> Transfer Ticket", LocationId, PrintertoUse.PrinterName);



                                string DirectoryPath = @"c:\ScaleTickets";
                                string Filename = DirectoryPath + @"\" + LocationId.ToString() + "_" + localDataSet.vwTransfer_Load[0].Load_Id.ToString() + ".pdf";
                                if (!System.IO.Directory.Exists(DirectoryPath)) { System.IO.Directory.CreateDirectory(DirectoryPath); }
                                if (!System.IO.File.Exists(Filename)) { System.IO.File.Delete(Filename); }
                                OutboundTicket.ExportToDisk(ExportFormatType.PortableDocFormat, Filename);

                                //string DirectoryPath = @"c:\ScaleTickets";
                                //string Filename = DirectoryPath + @"\" + NWDataset.vwWeigh_Sheet[0].Location_Id.ToString() + "_" + NWDataset.vwWeigh_Sheet[0].Load_Id.ToString()  + ".pdf";
                                //if (!System.IO.Directory.Exists(DirectoryPath)) { System.IO.Directory.CreateDirectory(DirectoryPath); }
                                //if (!System.IO.File.Exists(Filename)) { System.IO.File.Delete(Filename); }
                                //OutboundTicket.ExportToDisk(ExportFormatType.PortableDocFormat, Filename);

                            }
                        }
                    }

                }
            }
        }
        catch (Exception ocrap)
        {

            Logging.Add_System_Log("Web.Printing.PrintTransfer_Final_Ticket Load UID<" + LoadUID.ToString() + "> Printer Name<" + Printer + ">", ocrap.Message, LocationId);

        }
    }


    static string ConvertObjectToXMLString(object classObject)
    {
        string xmlString = null;
        XmlSerializer xmlSerializer = new XmlSerializer(classObject.GetType());
        using (MemoryStream memoryStream = new MemoryStream())
        {
            xmlSerializer.Serialize(memoryStream, classObject);
            memoryStream.Position = 0;
            xmlString = new StreamReader(memoryStream).ReadToEnd();
        }
        return xmlString;
    }


}