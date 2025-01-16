using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CrystalDecisions.CrystalReports;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportAppServer;
using CrystalDecisions.Web;
using System.Web.UI.WebControls;
using CrystalDecisions.ReportSource;
using ClosedXML.Excel;
using System.Data;
using System.IO;
using System.Net.Mail;
using System.Net;

/// <summary>
/// Summary description for Printing
/// </summary>
public class SeedPlantPrinting

{
    public SeedPlantPrinting()
    {
   

    }

    



    public static void Print_Ticket( Guid UID,  string Printer = "" , int NumberOfCopies=1 )
    {




        try
        {
            using (ReportDocument SeedTicket = new ReportDocument())
            {




                using (SeedPrintingDataSet reportDataSet = GetSeedPrintingDataSet(UID))
                {
                    if (reportDataSet.TicketLocation.Count > 0)
                    {
                        SeedTicket.Load(HttpContext.Current.Server.MapPath("~/SeedReports/OutboundSeedTicket.rpt"));
                        SeedTicket.SetDataSource(reportDataSet);
                        SeedTicket.Subreports[0].SetDataSource(reportDataSet);
                        SeedTicket.Subreports[1].SetDataSource(reportDataSet);
                        SeedTicket.Subreports[2].SetDataSource(reportDataSet);
                        SeedTicket.Subreports[3].SetDataSource(reportDataSet);
                        SeedTicket.Subreports[4].SetDataSource(reportDataSet);

                        //see if the printer is valid 
                        bool ValidPrinter = false;

                        System.Drawing.Printing.PrinterSettings settings = new System.Drawing.Printing.PrinterSettings();
                        string DefaultPrinter = settings.PrinterName;
                        System.Drawing.Printing.PageSettings pageSettings = new System.Drawing.Printing.PageSettings();

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
                        //CrystalDecisions.CrystalReports.Engine.PrintOptions repOptions;
                        //repOptions = SeedTicket.PrintOptions;
                        //repOptions.PrinterName = Printer;
                        CrystalDecisions.Shared.PageMargins margins = SeedTicket.PrintOptions.PageMargins;
                        margins.bottomMargin = 0;
                        margins.topMargin = 0;
                        //settings.Copies = 1;
                        //settings.PrinterName = Printer;
                        //settings.FromPage = 1;
                        //settings.ToPage = 1;
                        //SeedTicket.PrintOptions.PrinterName = Printer;
                        //pageSettings.Margins.Bottom = 0;
                        //pageSettings.Margins.Top = 0;








                        SeedTicket.PrintOptions.ApplyPageMargins(margins);






                        CrystalDecisions.ReportAppServer.Controllers.PrintReportOptions printReportOptions = new CrystalDecisions.ReportAppServer.Controllers.PrintReportOptions();
                        CrystalDecisions.ReportAppServer.Controllers.PrintOutputController printOutputController = new CrystalDecisions.ReportAppServer.Controllers.PrintOutputController();
                        CrystalDecisions.ReportAppServer.ClientDoc.ISCDReportClientDocument rptClientDoc;
                        rptClientDoc = SeedTicket.ReportClientDocument;
                        printReportOptions.PrinterName = Printer;
                        printReportOptions.NumberOfCopies = 1;
                        printReportOptions.PaperSize = CrystalDecisions.ReportAppServer.ReportDefModel.CrPaperSizeEnum.crPaperSizePaperLetter;
                        printReportOptions.AddPrinterPageRange(1, 1);
                        printReportOptions.JobTitle = $"SeedTicket";












                        try
                        {
                            //SeedTicket.PrintToPrinter(settings,pageSettings,false);
                            //SeedTicket.PrintToPrinter(1, false, 1, 1);
                            for (int i = 0; i < NumberOfCopies; i++)
                            {
                                rptClientDoc.PrintOutputController.PrintReport(printReportOptions);
                            }

                        }
                        catch { 

                        }

                        //    if (SendEmail)
                        //    {


                        //        System.IO.Stream ms = SeedTicket.ExportToStream(ExportFormatType.PortableDocFormat);
                        //        MemoryStream stream = new MemoryStream();

                        //        ms.CopyTo(stream);

                        //        var ticketRow = reportDataSet.TicketLocation[0];
                        //        string Header = string.Format("Truck Load {0}", ticketRow.Ticket);
                        //        string Subject = Header;
                        //        string FileName = string.Format("Ticket{0}", ticketRow.Ticket) + ".pdf";


                        //        List<string> Recipients = new List<string>();

                        //        //using (SeedPrintingDataSet.Email_ListDataTable Email_List = new SeedPrintingDataSet.Email_ListDataTable())
                        //        //{
                        //        //    using (SeedPrintingDataSetTableAdapters.Email_ListTableAdapter email_ListTableAdapter = new SeedPrintingDataSetTableAdapters.Email_ListTableAdapter())
                        //        //    {
                        //        //        email_ListTableAdapter.FillBySendTicket(Email_List);
                        //        //        {
                        //        //            foreach (SeedPrintingDataSet.Email_ListRow emailrow in Email_List)
                        //        //            {
                        //        //                Recipients.Add(emailrow.email);
                        //        //            }
                        //        //        }

                        //        //        string EmailList = string.Empty;
                        //        //        foreach (string email in Recipients)
                        //        //        {
                        //        //            if (!string.IsNullOrEmpty(EmailList)) EmailList += ";";

                        //        //            EmailList += email;
                        //        //        }

                        //        //        if (Printing.SendExcellMail(EmailList, stream, FileName, Subject, Header))
                        //        //        { }
                        //        //    }
                        //        //}
                        //    }



                    }
                }
            }
        }
        catch 
        {
        }
    }










    public class TicketList
    {
        public int Ticket;
        public Stream stream;
        public string Filename;

        public TicketList(int ticket )
        {
            Ticket = ticket;
            stream = null;
            Filename =string.Format("Ticket{0}", Ticket) + ".pdf";

        }
    }



    public static List< TicketList> GetTicketAttachmentList(HttpServerUtility server, List<int> Tickets)
    {
        List<TicketList> ticketList = new List<TicketList>();
        //foreach(int Ticket in Tickets)
        //{
        //    TicketList Item = new TicketList(Ticket);
        //    Item.stream = Get_Ticket_Stream(server, Ticket);
        //    if (Item.stream != null) ticketList.Add(Item);
        //}

        return ticketList;

    }



    public static bool SendTicketList(HttpServerUtility server,List<TicketList> ticketList,string Subject,string Message)
    {
        bool Retval = false;

        //foreach (TicketList Item in ticketList)
        //{
        //    Item.stream = Get_Ticket_Stream(server, Item.Ticket);
        //}

     

        List<string> Recipients = new List<string>();

            //using (SeedPrintingDataSet.Email_ListDataTable Email_List = new SeedPrintingDataSet.Email_ListDataTable())
            //{
            //    using (SeedPrintingDataSetTableAdapters.Email_ListTableAdapter email_ListTableAdapter = new SeedPrintingDataSetTableAdapters.Email_ListTableAdapter())
            //    {
            //        email_ListTableAdapter.FillBySendTicket(Email_List);
            //        {
            //            foreach (SeedPrintingDataSet.Email_ListRow emailrow in Email_List)
            //            {
            //                Recipients.Add(emailrow.email);
            //            }
            //        }

            //        string EmailList = string.Empty;
            //        foreach (string email in Recipients)
            //        {
            //            if (!string.IsNullOrEmpty(EmailList)) EmailList += ";";

            //            EmailList += email;
            //        }

            //    if (Printing.SendExcellMailBulk(EmailList, ticketList, Subject,Message ))
            //        { Retval = true; }
            //    }
            //}
        return Retval;
    }


    public static Stream Get_Ticket_Stream( Guid UID)
    {

        try
        {
            using (ReportDocument ticketReport = new ReportDocument())
            {
                using (SeedPrintingDataSet reportDataset = GetSeedPrintingDataSet(UID))
                {
                    if (reportDataset.TicketLocation.Count > 0)
                    {
                        ticketReport.Load(HttpContext.Current.Server.MapPath("~/SeedReports/OutboundSeedTicket.rpt"));
                        ticketReport.SetDataSource(reportDataset );
                 
                        ticketReport.Subreports[0].SetDataSource(reportDataset);
                        ticketReport.Subreports[1].SetDataSource(reportDataset);
                        ticketReport.Subreports[2].SetDataSource(reportDataset);
                        ticketReport.Subreports[3].SetDataSource(reportDataset);
                        ticketReport.Subreports[4].SetDataSource(reportDataset);

                        System.IO.Stream ms = ticketReport.ExportToStream(ExportFormatType.PortableDocFormat);
                        MemoryStream stream = new MemoryStream();

                        ms.CopyTo(stream);
                        return ms;



                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
        catch 
        {
           
            return null;
        }
    }


    public static SeedPrintingDataSet GetSeedPrintingDataSet(List<int> Tickets)
    {
        string TicketFilter = "";
        foreach(var item in Tickets)
        {
            if (TicketFilter != "") TicketFilter += ",";
            TicketFilter += item.ToString();

        }
        using (SeedPrintingDataSet reportDataSet = new SeedPrintingDataSet())
        {
            using ( SeedPrintingDataSetTableAdapters.TicketLocationTableAdapter ticketLocationTableAdapter = new SeedPrintingDataSetTableAdapters.TicketLocationTableAdapter())
            {
                ticketLocationTableAdapter.FillByCSVTickets(reportDataSet.TicketLocation, TicketFilter);
            }
            using (SeedPrintingDataSetTableAdapters.crTicketGTNValuesTableAdapter TicketGTNValuesTableAdapter = new SeedPrintingDataSetTableAdapters.crTicketGTNValuesTableAdapter())
            {
                TicketGTNValuesTableAdapter.FillByCSVTickets(reportDataSet.crTicketGTNValues, TicketFilter );
            }
            using (SeedPrintingDataSetTableAdapters.crTicketTreatmentsTableAdapter TicketTreatmentsTableAdapter = new SeedPrintingDataSetTableAdapters.crTicketTreatmentsTableAdapter())
            {
                TicketTreatmentsTableAdapter.FillByCSVTickets(reportDataSet.crTicketTreatments, TicketFilter );
            }
            using (SeedPrintingDataSetTableAdapters.crTicketVarietiesTableAdapter TicketVarietiesTableAdapter = new SeedPrintingDataSetTableAdapters.crTicketVarietiesTableAdapter())
            {
                TicketVarietiesTableAdapter.FillByCSVTickets (reportDataSet.crTicketVarieties, TicketFilter );
            }
            using (SeedPrintingDataSetTableAdapters.crTicketWeightsTableAdapter TicketWeightsTableAdapter = new SeedPrintingDataSetTableAdapters.crTicketWeightsTableAdapter())
            {
                TicketWeightsTableAdapter.FillByCSVTickets(reportDataSet.crTicketWeights, TicketFilter );
            }

            using (SeedPrintingDataSetTableAdapters.crTicketMiscTableAdapter TicketMiscTableAdapter = new SeedPrintingDataSetTableAdapters.crTicketMiscTableAdapter())
            {
                TicketMiscTableAdapter.FillByCSVTickets(reportDataSet.crTicketMisc, TicketFilter );
            }
            using (SeedPrintingDataSetTableAdapters.ClearfieldCoxamiumTicketsTableAdapter clearfieldCoxamiumTicketsTableAdapter = new SeedPrintingDataSetTableAdapters.ClearfieldCoxamiumTicketsTableAdapter())
            {
                clearfieldCoxamiumTicketsTableAdapter.FillByCSVTickets(reportDataSet.ClearfieldCoxamiumTickets, TicketFilter);
            

            }

            foreach(var cRow in reportDataSet.ClearfieldCoxamiumTickets)
            {
                var waterMark = HttpContext.Current.Server.MapPath("~/NoWatermark.png");
                if (reportDataSet.ClearfieldCoxamiumTickets.Count > 0)
                {
                    if (reportDataSet.ClearfieldCoxamiumTickets[0].Clearfield)
                    {
                        waterMark = HttpContext.Current.Server.MapPath("~/Clearfield.png");
                    }
                    else if (reportDataSet.ClearfieldCoxamiumTickets[0].Coxamium)
                    {
                        waterMark = HttpContext.Current.Server.MapPath("~/Coaxium.png");
                    }

                }

                reportDataSet.WatermarkDt.AddWatermarkDtRow(waterMark,cRow.UID);
            }

            return reportDataSet;
        }
    }


    public static SeedPrintingDataSet GetSeedPrintingDataSet(Guid UID)
    {

        using (SeedPrintingDataSet reportDataSet = new SeedPrintingDataSet())
        {
            using (SeedPrintingDataSetTableAdapters.TicketLocationTableAdapter ticketLocationTableAdapter = new SeedPrintingDataSetTableAdapters.TicketLocationTableAdapter())
            {
                ticketLocationTableAdapter.FillByUID(reportDataSet.TicketLocation, UID);
            }
            using (SeedPrintingDataSetTableAdapters.crTicketGTNValuesTableAdapter TicketGTNValuesTableAdapter = new SeedPrintingDataSetTableAdapters.crTicketGTNValuesTableAdapter())
            {
                TicketGTNValuesTableAdapter.FillBySeed_Ticket_UID(reportDataSet.crTicketGTNValues, UID);
            }
            using (SeedPrintingDataSetTableAdapters.crTicketTreatmentsTableAdapter TicketTreatmentsTableAdapter = new SeedPrintingDataSetTableAdapters.crTicketTreatmentsTableAdapter())
            {
                TicketTreatmentsTableAdapter.FillBySeed_Ticket_UID(reportDataSet.crTicketTreatments, UID);
            }
            using (SeedPrintingDataSetTableAdapters.crTicketVarietiesTableAdapter TicketVarietiesTableAdapter = new SeedPrintingDataSetTableAdapters.crTicketVarietiesTableAdapter())
            {
                TicketVarietiesTableAdapter.FillBySeed_Ticket_UID(reportDataSet.crTicketVarieties, UID);
            }
            using (SeedPrintingDataSetTableAdapters.crTicketWeightsTableAdapter TicketWeightsTableAdapter = new SeedPrintingDataSetTableAdapters.crTicketWeightsTableAdapter())
            {
                TicketWeightsTableAdapter.FillBySeed_Ticket_UID(reportDataSet.crTicketWeights, UID);
            }

            using (SeedPrintingDataSetTableAdapters.crTicketMiscTableAdapter TicketMiscTableAdapter = new SeedPrintingDataSetTableAdapters.crTicketMiscTableAdapter())
            {
                TicketMiscTableAdapter.Fill(reportDataSet.crTicketMisc, UID);
            }

            using (SeedPrintingDataSetTableAdapters.ClearfieldCoxamiumTicketsTableAdapter clearfieldCoxamiumTicketsTableAdapter = new SeedPrintingDataSetTableAdapters.ClearfieldCoxamiumTicketsTableAdapter())
            {
                clearfieldCoxamiumTicketsTableAdapter.FillByUID(reportDataSet.ClearfieldCoxamiumTickets, UID);
            }

            var waterMark = HttpContext.Current.Server.MapPath("~/NoWatermark.png");
            if (reportDataSet.ClearfieldCoxamiumTickets.Count > 0)
            {
                if (reportDataSet.ClearfieldCoxamiumTickets[0].Clearfield)
                {
                    waterMark = HttpContext.Current.Server.MapPath("~/Clearfield.png");
                }
                else if (reportDataSet.ClearfieldCoxamiumTickets[0].Coxamium)
                {
                    waterMark = HttpContext.Current.Server.MapPath("~/Coaxium.png");
                }

            }

            reportDataSet.WatermarkDt.AddWatermarkDtRow(waterMark,UID);

                return reportDataSet;
        }
    }


    
    public static MemoryStream Send_TicketToBrowser(Guid UID)
    {
        ReportDocument ticketReport = new ReportDocument();
        MemoryStream memoryStream = new MemoryStream();

        try
        {
            using (SeedPrintingDataSet reportDataset = GetSeedPrintingDataSet(UID))
            {
                if (reportDataset.TicketLocation.Count > 0)
                {
                    ticketReport.Load(HttpContext.Current.Server.MapPath("~/SeedReports/OutboundSeedTicket.rpt"));
                    ticketReport.SetDataSource(reportDataset);

                    ticketReport.Subreports[0].SetDataSource(reportDataset);
                    ticketReport.Subreports[1].SetDataSource(reportDataset);
                    ticketReport.Subreports[2].SetDataSource(reportDataset);
                    ticketReport.Subreports[3].SetDataSource(reportDataset);
                    ticketReport.Subreports[4].SetDataSource(reportDataset);

                    using (Stream exportStream = ticketReport.ExportToStream(ExportFormatType.PortableDocFormat))
                    {
                        exportStream.CopyTo(memoryStream);
                    }

                    memoryStream.Seek(0, SeekOrigin.Begin);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            if (ticketReport != null)
            {
                ticketReport.Close();
                ticketReport.Dispose();
            }
        }

        return memoryStream;
    }

    public static void SendTicketsToBrowser(List<int> Tickets,string Filename, bool Download = true)
    {

        ReportDocument ticketReport = new ReportDocument();
        
            try
            {
                using (SeedPrintingDataSet reportDataset = GetSeedPrintingDataSet(Tickets))
                {
                    if (reportDataset.TicketLocation.Count > 0)
                    {
                        ticketReport.Load(HttpContext.Current.Server.MapPath("~/SeedReports/OutboundSeedTicket.rpt"));
                        ticketReport.SetDataSource(reportDataset);
                        ticketReport.Subreports[0].SetDataSource(reportDataset);
                        ticketReport.Subreports[1].SetDataSource(reportDataset);
                        ticketReport.Subreports[2].SetDataSource(reportDataset);
                        ticketReport.Subreports[3].SetDataSource(reportDataset);
                        ticketReport.Subreports[4].SetDataSource(reportDataset);

                        ExportOptions exportOpts = new ExportOptions();
                        exportOpts.ExportFormatType = ExportFormatType.PortableDocFormat;

                        EditableRTFExportFormatOptions exportFormatOptions =
                          ExportOptions.CreateEditableRTFExportFormatOptions();

                        exportFormatOptions.InsertPageBreaks = true;

                        exportOpts.ExportFormatOptions = exportFormatOptions;




                        ticketReport.ExportToHttpResponse(exportOpts, HttpContext.Current.Response, Download, string.Format("{0}", Filename));

                    }
                    else
                    {
                    }
                }



            }
            catch
            {

            }
            finally
            {
                if (ticketReport != null)
                {
                ticketReport.Close();
                ticketReport.Dispose();
                }
            }
    }



    public static string GetDateForFileName()
    {
        DateTime D = DateTime.Now;
        return string.Format("{0}_{1}_{2}", D.Month, D.Day, D.Year);
    }


    public static string GetDateForFileName(DateTime D)
    {
        
        return string.Format("{0}_{1}_{2}", D.Month, D.Day, D.Year);
    }


 








    public static MemoryStream CreateXLSXReport(DataTable Dt)
    {
        MemoryStream stream = new MemoryStream();
        try
        {
            XLWorkbook wb = new XLWorkbook();
            DataRow Dr = Dt.NewRow();
            wb.Worksheets.Add(Dt);
            wb.SaveAs(stream);
        }
        catch 
        {
           
        }

        return stream;


    }

   



    /// <summary>
    /// Download an xlsx file directly to the browser
    /// </summary>
    /// <param name="stream">memory stream of the excel file</param>
    /// <param name="FileName">Name of file like soomething.xlsx </param>
    /// <param name="Response">HttprRsponse</param>
    public static void DownloadExcellFile(MemoryStream stream , string FileName , HttpResponse Response)
    {
        stream.Seek(0, SeekOrigin.Begin);

        Response.Clear();

        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
        Response.AddHeader("content-length", stream.Length.ToString());

        Response.BinaryWrite(stream.ToArray());
        Response.Flush();
        Response.End();
    }


    /// <summary>
    /// Sends an xlsx excell file to an email address
    /// </summary>
    /// <param name="Recipient">email Address</param>
    /// <param name="stream">memorystream of file</param>
    /// <param name="FileName">Name of file like something.xlsx</param>
    /// <param name="Subject">email Subject</param>
    /// <param name="Message">email Body</param>
    public static bool SendExcellMail(string Recipient ,MemoryStream stream, string FileName, string Subject,string Message )
    {
        try
        {
            //Very important or file will be 0 bytes
            stream.Seek(0, SeekOrigin.Begin);

            System.Net.Mail.Attachment mailAttachment = new System.Net.Mail.Attachment(stream, new System.Net.Mime.ContentType("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"));
            mailAttachment.Name = FileName;

            List<System.Net.Mail.Attachment> Attchments = new List<System.Net.Mail.Attachment>();
            Attchments.Add(mailAttachment);
            SeedPlantPrinting.SendMail(Recipient, Subject, Message , Attchments);
            return true;
        }
        catch
        {
         
            return false;
        }
    }




    public static bool SendExcellMailBulk(string Recipient, List<TicketList> ListOfAttachments,  string Subject, string Message)
    {
        try
        {
            List<System.Net.Mail.Attachment> Attchments = new List<System.Net.Mail.Attachment>();
            //Very important or file will be 0 bytes
            foreach (TicketList Item in ListOfAttachments)
            {
                Item.stream.Seek(0, SeekOrigin.Begin);

                System.Net.Mail.Attachment mailAttachment = new System.Net.Mail.Attachment(Item.stream , new System.Net.Mime.ContentType("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"));
                mailAttachment.Name = Item.Filename ;

                Attchments.Add(mailAttachment);
            }
            
           
            SeedPlantPrinting.SendMail(Recipient, Subject, Message, Attchments);
            return true;
        }
        catch 
        {
           
            return false;
        }
    }

    /// <summary>
    /// Sends an xlsx excell file to an email address
    /// </summary>
    /// <param name="Recipient">email Address</param>
    /// <param name="stream">memorystream of file</param>
    /// <param name="FileName">Name of file like something.xlsx</param>
    /// <param name="Subject">email Subject</param>
    /// <param name="Message">email Body</param>
    public static bool SendPDFMail(string Recipient, MemoryStream stream, string FileName, string Subject, string Message)
    {
        try
        {
            //Very important or file will be 0 bytes
            stream.Seek(0, SeekOrigin.Begin);

            System.Net.Mail.Attachment mailAttachment = new System.Net.Mail.Attachment(stream, new System.Net.Mime.ContentType("application/pdf"));
            mailAttachment.Name = FileName;

            List<System.Net.Mail.Attachment> Attchments = new List<System.Net.Mail.Attachment>();
            Attchments.Add(mailAttachment);
            SeedPlantPrinting.SendMail(Recipient, Subject, Message , Attchments);
            return true;
        }
        catch
        {
          
            return false;
        }
    }


    ///// <summary>
    ///// Send an email
    ///// </summary>
    ///// <param name="Recipient">email Address</param>
    ///// <param name="Subject">email subject</param>
    ///// <param name="Message">email body</param>
    ///// <param name="Attachments">list of attchments to send. Set to null if none</param>
    //public static bool SendMail(string Recipient,string Subject,string Message, List< Attachment> Attachments)
    //{
    //    try
    //    {
    //        SmtpClient smtpClient = new SmtpClient();

    //        smtpClient.Host = "Smtp.Gmail.com";
    //        smtpClient.Port = 587;
    //        smtpClient.EnableSsl = true;
    //        smtpClient.Timeout = 10000;
    //        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
    //        smtpClient.UseDefaultCredentials = false;
    //        smtpClient.Credentials = new NetworkCredential("newtownctap@Gmail.com", "CT@Psand2018");
    //        MailMessage message = new MailMessage();
    //        message.From=new MailAddress("newtownctap@Gmail.com");


    //        foreach (var address in Recipient.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
    //        {
    //            message.To.Add(address);
    //        }

    //        message.Subject = Subject;
    //        message.Body = Message;
    //        if (Attachments != null && Attachments.Count >0)
    //        {
    //            foreach(Attachment attchment in Attachments )
    //            {
    //                message.Attachments.Add(attchment);
    //            }
    //        }
    //        smtpClient.Send(message);
    //        return true;
    //    }
    //    catch (Exception ex)
    //    {

    //        Logging.Log_Event("SendMail", ex.Message);
    //        return false;
    //    }

    //}


    /// <summary>
    /// Send an email
    /// </summary>
    /// <param name="Recipient">email Address</param>
    /// <param name="Subject">email subject</param>
    /// <param name="Message">email body</param>
    /// <param name="Attachments">list of attchments to send. Set to null if none</param>
    public static bool SendMail(string Recipient, string Subject, string Message, List<Attachment> Attachments)
    {
        try
        {
            SmtpClient smtpClient = new SmtpClient();

            smtpClient.Host = "smtp.office365.com";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.Timeout = 10000;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("tnewtown@ctapllc.com", "W1nter2019!");
            smtpClient.Timeout = 20000;
            MailMessage message = new MailMessage();
            message.From = new MailAddress("tnewtown@ctapllc.com");


            foreach (var address in Recipient.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                message.To.Add(address);
            }

            message.Subject = Subject;
            message.Body = Message;
            if (Attachments != null && Attachments.Count > 0)
            {
                foreach (Attachment attchment in Attachments)
                {
                    message.Attachments.Add(attchment);
                }
            }
            smtpClient.Send(message);
            return true;
        }
        catch 
        {

        
            return false;
        }

    }


  

}