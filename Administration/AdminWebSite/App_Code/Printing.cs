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





    public static void PrintTicket(ReportDocument DocumentToPrint, string Printer, int NumberOfCopies)
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







    public static void PrintWSTicket(HttpServerUtility server, HttpResponse Response, Guid LoadUID, bool EndOfLot)
    {

        try
        {
            //    using (ReportDocument WSReport = new ReportDocument())
            //    {
            //        using (NWDataSet nWDataset = new global::NWDataSet())

            //        {





            //            using (NWDataSetTableAdapters.vwWeigh_SheetTableAdapter Weigh_SheetTableAdapter = new NWDataSetTableAdapters.vwWeigh_SheetTableAdapter())
            //            {
            //                if (Weigh_SheetTableAdapter.Fill(nWDataset.vwWeigh_Sheet, LoadUID) > 0)
            //                {
            //                    //CrystalDecisions.CrystalReports.Engine.PrintOptions repOptions;
            //                    WSReport.Load(server.MapPath("~/Reports/Weigh_Sheet.rpt"));
            //                    WSReport.SetDataSource(nWDataset);
            //                    WSReport.Subreports[0].SetDataSource(nWDataset);
            //                    WSReport.SetParameterValue("End_Of_Lot", EndOfLot);
            //                    WSReport.SetParameterValue("NEW_LOT", false);
            //                    WSReport.SetParameterValue("Total_Billed", 0);
            //                    WSReport.SetParameterValue("Creation_Date", nWDataset.vwWeigh_Sheet[0].Creation_Date); ;

            //                    WSReport.SetParameterValue("Location_Description", nWDataset.vwWeigh_Sheet[0].Location_Description);

            //                    WSReport.SetParameterValue("Unofficial", false);
            //                    WSReport.SetParameterValue("Lot", nWDataset.vwWeigh_Sheet[0].Lot_Number);
            //                    WSReport.SetParameterValue("Customer_Copy", true);
            //                    WSReport.SetParameterValue("StrWS_Id", nWDataset.vwWeigh_Sheet[0].WS_Id.ToString());
            //                    //repOptions = OutboundTicket.PrintOptions;
            //                    //repOptions.PrinterName = PrintertoUse.PrinterName ;
            //                    CrystalDecisions.Shared.PageMargins margins = WSReport.PrintOptions.PageMargins;
            //                    margins.bottomMargin = 0;
            //                    margins.topMargin = 0;
            //                    WSReport.PrintOptions.ApplyPageMargins(margins);

            //                    WSReport.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, string.Format("WeightSheet_{0}.pdf", nWDataset.vwWeigh_Sheet[0].WS_Id));


            //                }
            //            }

            //        }
            //    }
        }
        catch
        {


        }
    }



}