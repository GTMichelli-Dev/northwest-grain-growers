
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Reporting;
using CrystalDecisions.ReportSource;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WeightSheets_DailyReport : System.Web.UI.Page
{
    ReportDocument rpt = new ReportDocument();

    private enum enumReportType { NotValid, IntakeTransfer, Commodity,WeightSheetSeries,DailyBin,IntakeLoad };

    private static enumReportType ReportType = enumReportType.NotValid;

    protected void Page_Load(object sender, EventArgs e)
    {
    
            if ((Request.QueryString["Date"] != null) && (Request.QueryString["LocationId"] != null) && (Request.QueryString["Transfer"] != null))
            {
                ReportType = enumReportType.IntakeTransfer;
            }
            else if (Request.QueryString["WeightSheetSeries"] != null && Request.QueryString["Date"] != null && Request.QueryString["LocationId"] != null)
            {
                ReportType = enumReportType.WeightSheetSeries ;

            }
            else if (Request.QueryString["DailyCommodity"] != null && Request.QueryString["Date"] != null && Request.QueryString["LocationId"] != null)
            {
                ReportType = enumReportType.Commodity;

            }
            else if (Request.QueryString["DailyBin"] != null && Request.QueryString["StartDate"] != null && Request.QueryString["EndDate"] != null && Request.QueryString["LocationId"] != null && Request.QueryString["LocationName"] != null)
            {
                ReportType = enumReportType.DailyBin;

            }
        else if (Request.QueryString["IntakeLoad"] != null &&  Request.QueryString["LoadUID"] != null )
        {
            ReportType = enumReportType.IntakeLoad ;

        }
        else
            {
                Response.Redirect("~/Default.aspx");
            }


        if (!this.IsPostBack)
        {

            if (ReportType == enumReportType.IntakeTransfer)
            {
                if (Transfer)
                {
                    GetTransferReport(false);
                }
                else
                {
                    GetIntakeReport(false);

                }
            }
            else if (ReportType == enumReportType.WeightSheetSeries)
            {
                GetWeightSheetSeriesReport(false);
            }
            else if (ReportType == enumReportType.Commodity)
            {
                ckDetails.Visible = false;
                GetCommodityReport(false);
            }

            else if (ReportType == enumReportType.DailyBin)
            {
                ckDetails.Visible = false;
                GetBinReport(false);
            }
            else if (ReportType== enumReportType.IntakeLoad )
            {
                GetIntakeFinalTicket(false);
            }
        }
    }


    public Guid LoadUID
    {
        get
        {
            Guid UID =Guid.Empty ;
            if (Request.QueryString["LoadUID"] != null)
            {
               Guid.TryParse(Request.QueryString["LoadUID"].ToString(), out UID);
            }
            else
            {
                Response.Redirect("~/WeightSheets/DailyReportSelection.aspx");
            }
            return UID;
        }
    }


 





    public DateTime StartDate
    {
        get
        {
            DateTime dt = DateTime.Now;
            if (Request.QueryString["StartDate"] != null)
            {
                DateTime.TryParse(Request.QueryString["StartDate"].ToString(), out dt);
            }
            else
            {
                Response.Redirect("~/WeightSheets/DailyReportSelection.aspx");
            }
            return dt;
        }
    }


    public DateTime EndDate
    {
        get
        {
            DateTime dt = DateTime.Now;
            if (Request.QueryString["EndDate"] != null)
            {
                DateTime.TryParse(Request.QueryString["EndDate"].ToString(), out dt);
            }
            else
            {
                Response.Redirect("~/WeightSheets/DailyReportSelection.aspx");
            }
            return dt;
        }
    }





    public DateTime SelectedDate
    {
        get
        {
            DateTime dt = DateTime.Now;
            if (Request.QueryString["Date"] != null)
            {
                DateTime.TryParse(Request.QueryString["Date"].ToString(), out dt);
            }
            else
            {
                Response.Redirect("~/WeightSheets/DailyReportSelection.aspx");
            }
            return dt;
        }
    }


    public int Location
    {
        get
        {
            int location = -1;
            if (Request.QueryString["LocationId"] != null)
            {
                int.TryParse(Request.QueryString["LocationId"].ToString(), out location);
            }
            else
            {
                Response.Redirect("~/WeightSheets/DailyReportSelection.aspx");
            }
            return location;
        }
    }


    public bool Details
    {
        get
        {
            return !ckDetails.Checked;
        }
    }



    public bool Transfer
    {
        get
        {
            bool transfer = false;
            if (Request.QueryString["Transfer"] != null)
            {
                bool.TryParse(Request.QueryString["Transfer"].ToString(), out transfer);
            }
            return transfer;
        }
    }





    public bool GetIntakeReport(bool Export)
    {
        try
        {


            using (NWDataSet NWDataset = new NWDataSet())
            {
                NWDataset.EnforceConstraints = false;
                rpt.Load(Server.MapPath("DailyIntakeReport.rpt"));
                {

                    using (NWDataSetTableAdapters.DailyIntakeWSTableAdapter dailyIntakeWSTableAdapter = new NWDataSetTableAdapters.DailyIntakeWSTableAdapter())
                    {
                        dailyIntakeWSTableAdapter.Fill(NWDataset.DailyIntakeWS, SelectedDate, Location);
                        {

                            rpt.SetDataSource(NWDataset);
                            rpt.SetParameterValue("Details", Details);
                            if (Export)
                            {
                                ExportOptions exportOpts = new ExportOptions();
                                exportOpts.ExportFormatType = ExportFormatType.PortableDocFormat;

                                EditableRTFExportFormatOptions exportFormatOptions =
                                  ExportOptions.CreateEditableRTFExportFormatOptions();

                                exportFormatOptions.InsertPageBreaks = true;

                                exportFormatOptions.UsePageRange = false;
                                exportOpts.ExportFormatOptions = exportFormatOptions;


                                rpt.ExportToHttpResponse(exportOpts, Response, true, string.Format("DailyIntake_Location{0}_{1}.pdf", Location, SelectedDate));
                            }
                            else
                            {
                                CrystalReportViewer1.ReportSource = rpt;
                                CrystalReportViewer1.DataBind();

                            }




                        }

                    }
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.Print(ex.Message);
            return false;
        }

    }





    public bool GetCommodityReport(bool Export)
    {
        try
        {

            using (TallyDataset tallyDataset = new TallyDataset())
            {

                rpt.Load(Server.MapPath("DailyCommodityReport.rpt"));
                {

                    using (TallyDatasetTableAdapters.DailyLoadCountByCropTableAdapter dailyLoadCountByCropTableAdapter = new TallyDatasetTableAdapters.DailyLoadCountByCropTableAdapter())
                    {

                        dailyLoadCountByCropTableAdapter.Fill(tallyDataset.DailyLoadCountByCrop, SelectedDate, Location);
                        {

                            rpt.SetDataSource(tallyDataset);

                            if (Export)
                            {
                                ExportOptions exportOpts = new ExportOptions();
                                exportOpts.ExportFormatType = ExportFormatType.PortableDocFormat;

                                EditableRTFExportFormatOptions exportFormatOptions =
                                  ExportOptions.CreateEditableRTFExportFormatOptions();

                                exportFormatOptions.InsertPageBreaks = true;

                                exportFormatOptions.UsePageRange = false;
                                exportOpts.ExportFormatOptions = exportFormatOptions;


                                rpt.ExportToHttpResponse(exportOpts, Response, true, string.Format("DailyCommodity_Location{0}_{1}.pdf", Location, SelectedDate));
                            }
                            else
                            {
                                CrystalReportViewer1.ReportSource = rpt;
                                CrystalReportViewer1.DataBind();

                            }




                        }

                    }
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.Print(ex.Message);
            return false;
        }

    }


    public bool GetBinReport(bool Export)
    {

        try
        {

            using (BinDataSet binDataSet = new BinDataSet())
            {

                rpt.Load(Server.MapPath("~/Reports/BinTotals.rpt"));
                {

                    using (BinDataSetTableAdapters.AverageProteinByDateRangeTableAdapter averageProteinByDateRangeTableAdapter = new BinDataSetTableAdapters.AverageProteinByDateRangeTableAdapter ())
                    {

                        averageProteinByDateRangeTableAdapter.Fill(binDataSet.AverageProteinByDateRange , StartDate, EndDate, Location);
                        {

                            binDataSet.SelectDates.AddSelectDatesRow(StartDate, EndDate, Request.QueryString["LocationName"]);
                            rpt.SetDataSource(binDataSet);

                            if (Export)
                            {
                                ExportOptions exportOpts = new ExportOptions();
                                exportOpts.ExportFormatType = ExportFormatType.PortableDocFormat;

                                EditableRTFExportFormatOptions exportFormatOptions =
                                  ExportOptions.CreateEditableRTFExportFormatOptions();

                                exportFormatOptions.InsertPageBreaks = true;

                                exportFormatOptions.UsePageRange = false;
                                exportOpts.ExportFormatOptions = exportFormatOptions;


                                rpt.ExportToHttpResponse(ExportFormatType.ExcelWorkbook , Response, true, string.Format("AverageProtein_Location{0}_{1}_{1}.xls", Location, StartDate.ToShortDateString() ,EndDate.ToShortDateString()));
                            }
                            else
                            {
                                CrystalReportViewer1.ReportSource = rpt;
                                CrystalReportViewer1.DataBind();

                            }




                        }

                    }
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.Print(ex.Message);
            return false;
        }

    }


    public bool GetWeightSheetSeriesReport(bool Export)
    {
        try
        {

            using (TallyDataset tallyDataset = new TallyDataset())
            {

                rpt.Load(Server.MapPath("WeightSheetsAsc.rpt"));
                {

                    using (TallyDatasetTableAdapters.WeightSeriesByLocationDateTableAdapter  weightSeriesByLocationDateTableAdapter = new TallyDatasetTableAdapters.WeightSeriesByLocationDateTableAdapter())
                    {

                        weightSeriesByLocationDateTableAdapter.Fill(tallyDataset.WeightSeriesByLocationDate , Location, SelectedDate);
                        {

                            rpt.SetDataSource(tallyDataset);

                            if (Export)
                            {
                                ExportOptions exportOpts = new ExportOptions();
                                exportOpts.ExportFormatType = ExportFormatType.PortableDocFormat;

                                EditableRTFExportFormatOptions exportFormatOptions =
                                  ExportOptions.CreateEditableRTFExportFormatOptions();

                                exportFormatOptions.InsertPageBreaks = true;

                                exportFormatOptions.UsePageRange = false;
                                exportOpts.ExportFormatOptions = exportFormatOptions;


                                rpt.ExportToHttpResponse(exportOpts, Response, true, string.Format("WeightSheet_Series_Location{0}_{1}.pdf", Location, SelectedDate));
                            }
                            else
                            {
                                CrystalReportViewer1.ReportSource = rpt;
                                CrystalReportViewer1.DataBind();

                            }




                        }

                    }
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.Print(ex.Message);
            return false;
        }

    }




    public bool GetTransferReport(bool Export)
    {
        try
        {

            using (NWDataSet NWDataset = new NWDataSet())
            {
                NWDataset.EnforceConstraints = false;
                rpt.Load(Server.MapPath("DailyTransferReport.rpt"));
                {

                    using (NWDataSetTableAdapters.DailyTransferWSTableAdapter dailyTransferWSTableAdapter = new NWDataSetTableAdapters.DailyTransferWSTableAdapter())
                    {

                        dailyTransferWSTableAdapter.Fill(NWDataset.DailyTransferWS, SelectedDate, Location);
                        {

                            rpt.SetDataSource(NWDataset);
                            rpt.SetParameterValue("Details", Details);
                            if (Export)
                            {
                                ExportOptions exportOpts = new ExportOptions();
                                exportOpts.ExportFormatType = ExportFormatType.PortableDocFormat;

                                EditableRTFExportFormatOptions exportFormatOptions =
                                  ExportOptions.CreateEditableRTFExportFormatOptions();

                                exportFormatOptions.InsertPageBreaks = true;

                                exportFormatOptions.UsePageRange = false;
                                exportOpts.ExportFormatOptions = exportFormatOptions;


                                rpt.ExportToHttpResponse(exportOpts, Response, true, string.Format("DailyTransfer_Location{0}_{1}.pdf", Location, SelectedDate));
                            }
                            else
                            {
                                CrystalReportViewer1.ReportSource = rpt;
                                CrystalReportViewer1.DataBind();

                            }




                        }

                    }
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.Print(ex.Message);
            return false;
        }

    }




    public bool GetIntakeFinalTicket(bool Export)
    {
        try
        {

            using (WeightSheetDataSet  weightSheetDataSet = new  WeightSheetDataSet())
            {
                weightSheetDataSet.EnforceConstraints = false;
                rpt.Load(Server.MapPath("~/Reports/Inbound_Final_Ticket.rpt"));
                {

                    using (WeightSheetDataSetTableAdapters.vwWeigh_SheetTableAdapter  vwWeightSheetInformationTableAdapter= new WeightSheetDataSetTableAdapters.vwWeigh_SheetTableAdapter () )
                    {

                        if (vwWeightSheetInformationTableAdapter.Fill(weightSheetDataSet.vwWeigh_Sheet , LoadUID) > 0)
                        {
                            string LoadNumber = weightSheetDataSet.vwWeigh_Sheet[0].Load_Id.ToString();


                            rpt.SetDataSource(weightSheetDataSet);
                            //rpt.SetParameterValue("Details", Details);
                            if (Export)
                            {
                                ExportOptions exportOpts = new ExportOptions();
                                exportOpts.ExportFormatType = ExportFormatType.PortableDocFormat;

                                EditableRTFExportFormatOptions exportFormatOptions =
                                  ExportOptions.CreateEditableRTFExportFormatOptions();

                                exportFormatOptions.InsertPageBreaks = true;

                                exportFormatOptions.UsePageRange = false;
                                exportOpts.ExportFormatOptions = exportFormatOptions;


                                rpt.ExportToHttpResponse(exportOpts, Response, true, string.Format("Intake Load {0}.pdf", LoadNumber));
                            }
                            else
                            {
                                CrystalReportViewer1.ReportSource = rpt;
                                CrystalReportViewer1.DataBind();

                            }

                        }


                    }
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.Print(ex.Message);
            return false;
        }

    }

   
















    protected void lnkPrint_Click(object sender, EventArgs e)
    {
        if (ReportType == enumReportType.Commodity)
        {
            GetCommodityReport(true);
        }
        if (ReportType == enumReportType.WeightSheetSeries )
        {
            GetWeightSheetSeriesReport(true);
        }

        if (ReportType == enumReportType.DailyBin )
        {
            GetBinReport(true);
        }

        if (ReportType == enumReportType.IntakeLoad )
        {
            GetIntakeFinalTicket(true);
        }
        else if (ReportType == enumReportType.IntakeTransfer)
        { 
         if (Transfer)
            {
                GetTransferReport(true);
            }
            else
            {
                GetIntakeReport(true);

            }
        }

    }

}