
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Reporting;
using CrystalDecisions.ReportSource;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WeightSheets_WeightSheet : System.Web.UI.Page
{

    ReportDocument rptWeigh_Sheet = new ReportDocument();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            if (UID == Guid.Empty || Request.QueryString["Transfer"] == null)
            {
                Response.Redirect("~/WeightSheets/WeightSheets.aspx");
            }
            PlaceholderAdmin.Visible = (Security.GetUsersSecurityLevel(Session) == Security.enumSecurityLevel.Administrator );
            PlaceholderUser.Visible  = !PlaceholderAdmin.Visible;
            btnSendEmail.Visible = !Transfer;
        }

        using (NWDataSet.Weight_SheetsDataTable  weigh_SheetDataTable = new NWDataSet.Weight_SheetsDataTable())
        {
            using (NWDataSetTableAdapters.Weight_SheetsTableAdapter weigh_SheetTableAdapter = new NWDataSetTableAdapters.Weight_SheetsTableAdapter())
            {
                if (weigh_SheetTableAdapter.Fill(weigh_SheetDataTable, UID)==0)
                {
                    Response.Redirect("~/WeightSheets/WeightSheets.aspx");
                }
                else
                {
                    bool closed = weigh_SheetDataTable[0].Closed;
                    bool OriginalPrinted = weigh_SheetDataTable[0].Original_Printed;
                    string wsStatus = (closed==false) ? "Weight Sheet Open " : "Weight Sheet Closed ";
                    wsStatus += (OriginalPrinted==false ) ? ": Original Not Printed " : ": Original Printed ";
                    lblWSStatus.Text = wsStatus;
                    lnkOriginal.Text = (OriginalPrinted) ? "Mark As Original Not Printed" : "Mark As Original Printed";
                    lnkOriginal_ConfirmButtonExtender.ConfirmText = (OriginalPrinted) ? 
                        "Are You Sure You Want To Mark Weight Sheet as Original Not Printed. The original will print at the elevator and allow the elevator operator to re open the weight sheet " :
                        "Are You Sure You Want To Mark Weight Sheet as Original Printed. Doing this will prevent The original From printing at the elevator and prevent the elevator operator from re opening the weight sheet ";
                    hfOriginalPrinted.Value = OriginalPrinted.ToString(); 
                    if (closed && ! OriginalPrinted )
                    {
                        this.lnkOpenWS.Visible = true;
                        this.ckOriginal.Visible = false;
                        this.lnkOriginal.Visible = true;
                    }
                    else if (!closed && !OriginalPrinted)
                    {
                        this.lnkOpenWS.Visible = false;
                        this.ckOriginal.Visible = false;
                        this.lnkOriginal.Visible = false;
                    }
                    else if (closed && OriginalPrinted)
                    {
                        this.lnkOpenWS.Visible = true;
                        this.ckOriginal.Visible = true;
                        this.lnkOriginal.Visible = true;
                    }
                }
            }
        }

        
        if (Transfer)
        {
            GetTransferWeightSheet(UID, false);
        }
        else
        {
            GetIntakeWeightSheet(UID, false);
          
         
           
        }
    }


    public Guid UID
    {
        get
        {
            Guid UID = Guid.Empty;
            if (Request.QueryString["WSUID"] != null)
            {
                Guid.TryParse(Request.QueryString["WSUID"].ToString(), out UID);
            }
            return UID;
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

    public bool ShowAsOriginalPrinted
    {
        get
        {
           return  (ckOriginal.Visible)? ckOriginal.Checked : false;
        }
    }


    protected void btnSendEmail_Click(object sender, EventArgs e)
    {
        var stream = pdfGenerator.GetIntakeWeightSheet(UID,false , HttpContext.Current);
        Email.EmailWS(UID, stream, txtEmail.Value);
    }


    public MemoryStream GetIntakeWeightSheet(Guid Weight_Sheet_UID)
    {


        MemoryStream stream = new MemoryStream();


        try
        {



            {


                using (NWDataSet NWDataset = new NWDataSet())
                {

                    rptWeigh_Sheet.Load(Server.MapPath("Weigh_Sheet.rpt"));
                    {
                        bool Closed = true;

                        using (NWDataSetTableAdapters.vwWeigh_SheetTableAdapter vwWeigh_SheetTableAdapter = new NWDataSetTableAdapters.vwWeigh_SheetTableAdapter())
                        {
                            vwWeigh_SheetTableAdapter.FillByWeight_Sheet_UID(NWDataset.vwWeigh_Sheet, Weight_Sheet_UID);
                            {
                                using (NWDataSetTableAdapters.LocationsTableAdapter locationsTableAdapter = new NWDataSetTableAdapters.LocationsTableAdapter())
                                {
                                    if (NWDataset.vwWeigh_Sheet.Count > 0)
                                    {
                                        locationsTableAdapter.FillById(NWDataset.Locations, NWDataset.vwWeigh_Sheet[0].Location_Id);
                                        Closed = NWDataset.vwWeigh_Sheet[0].Closed;
                                        if (ckOriginal.Visible) NWDataset.vwWeigh_Sheet[0].Original_Printed = !ShowAsOriginalPrinted;
                                    }
                                    else
                                    {
                                        using (NWDataSetTableAdapters.Weight_SheetsTableAdapter weight_SheetsTableAdapter = new NWDataSetTableAdapters.Weight_SheetsTableAdapter())
                                        {
                                            if (weight_SheetsTableAdapter.Fill(NWDataset.Weight_Sheets, Weight_Sheet_UID) > 0)
                                            {
                                                locationsTableAdapter.FillById(NWDataset.Locations, NWDataset.Weight_Sheets[0].Location_Id);
                                                Closed = NWDataset.Weight_Sheets[0].Closed;

                                            }
                                        }
                                    }
                                }
                                using (NWDataSetTableAdapters.vwWeight_Sheet_InformationTableAdapter vwWeight_Sheet_InformationTableAdapter = new NWDataSetTableAdapters.vwWeight_Sheet_InformationTableAdapter())
                                {
                                    if (vwWeight_Sheet_InformationTableAdapter.FillByUID(NWDataset.vwWeight_Sheet_Information, Weight_Sheet_UID) > 0)
                                    {

                                        {
                                          
                                            decimal TotalBilled = 0;
                                            if (!NWDataset.vwWeight_Sheet_Information[0].IsTotal_BilledNull())
                                            {
                                                TotalBilled = NWDataset.vwWeight_Sheet_Information[0].Total_Billed;
                                            }

                                            var Custom = (!NWDataset.vwWeight_Sheet_Information[0].IsBOL_TypeNull() && NWDataset.vwWeight_Sheet_Information[0].BOL_Type.ToUpper().Trim() == "C");

                                            //rptWeigh_Sheet.SetParameterValue("CustomRate", Custom);

                                            //rptWeigh_Sheet.SetParameterValue("Closed", Closed);
                                            //rptWeigh_Sheet.SetParameterValue("End_Of_Lot", NWDataset.vwWeight_Sheet_Information[0].Is_End_Lot);
                                            //rptWeigh_Sheet.SetParameterValue("New_Lot", NWDataset.vwWeight_Sheet_Information[0].Is_New_Lot);
                                            //rptWeigh_Sheet.SetParameterValue("Total_Billed", TotalBilled);

                                            //rptWeigh_Sheet.SetParameterValue("Location_Description", NWDataset.vwWeight_Sheet_Information[0].Location_Description);
                                            //rptWeigh_Sheet.SetParameterValue("Creation_Date", NWDataset.vwWeight_Sheet_Information[0].Creation_Date);

                                            //rptWeigh_Sheet.SetParameterValue("Unofficial", !ShowAsOriginalPrinted);
                                            //rptWeigh_Sheet.SetParameterValue("Customer_Copy", false);
                                            //rptWeigh_Sheet.SetParameterValue("Lot", NWDataset.vwWeight_Sheet_Information[0].Lot_Number);
                                            //rptWeigh_Sheet.SetParameterValue("StrWS_Id", NWDataset.vwWeight_Sheet_Information[0].WS_Id.ToString().PadLeft(8, '0'));



                                            var parameterRow = NWDataset.ReportParameters.NewReportParametersRow();
                                            parameterRow.WSUID = Weight_Sheet_UID;
                                            parameterRow.CustomRate = Custom;

                                            parameterRow.Closed = Closed;
                                            parameterRow.End_Of_Lot = NWDataset.vwWeight_Sheet_Information[0].Is_End_Lot;
                                            parameterRow.New_Lot = NWDataset.vwWeight_Sheet_Information[0].Is_New_Lot;
                                            parameterRow.Total_Billed = TotalBilled;

                                            parameterRow.Location_Description = NWDataset.vwWeight_Sheet_Information[0].Location_Description;
                                            parameterRow.Creation_Date = NWDataset.vwWeight_Sheet_Information[0].Creation_Date;
                                            // LOOOOOOOOK
                                            parameterRow.Unofficial = !ShowAsOriginalPrinted;
                                            parameterRow.Customer_Copy = false;
                                            parameterRow.Lot = NWDataset.vwWeight_Sheet_Information[0].Lot_Number;
                                            parameterRow.StrWS_Id = NWDataset.vwWeight_Sheet_Information[0].WS_Id.ToString().PadLeft(8, '0');
                                            NWDataset.ReportParameters.AddReportParametersRow(parameterRow);




                                            rptWeigh_Sheet.SetDataSource(NWDataset);
                                            rptWeigh_Sheet.Subreports[0].SetDataSource(NWDataset);








                                            // NWDataset.vwWeight_Sheet_Information[0].Original_Printed = !Official;



                                            //

                                            ExportOptions exportOpts = new ExportOptions();
                                            exportOpts.ExportFormatType = ExportFormatType.PortableDocFormat;

                                            EditableRTFExportFormatOptions exportFormatOptions =
                                              ExportOptions.CreateEditableRTFExportFormatOptions();
                                            exportFormatOptions.FirstPageNumber = 1;
                                            exportFormatOptions.InsertPageBreaks = true;
                                            exportFormatOptions.LastPageNumber = 1;
                                            exportFormatOptions.UsePageRange = true;
                                            exportOpts.ExportFormatOptions = exportFormatOptions;


                                            //rptWeigh_Sheet.ExportToHttpResponse(exportOpts, Response, true, string.Format("WeightSheet_{0}", NWDataset.vwWeight_Sheet_Information[0].WS_Id));



                                            ExportRequestContext req = new ExportRequestContext();

                                            req.ExportInfo = exportOpts;


                                            Stream s = rptWeigh_Sheet.FormatEngine.ExportToStream(req);
                                            s.CopyTo(stream);
                                        }
                                    }

                                }

                            }
                        }
                    }
                }

            }

        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.Print(ex.Message);

        }
        return stream;
    }



    public bool GetIntakeWeightSheet(Guid Weight_Sheet_UID, bool Export)
    {




        try
        {



            {


                using (NWDataSet NWDataset = new NWDataSet())
                {

                    rptWeigh_Sheet.Load(Server.MapPath("Weigh_Sheet.rpt"));
                    {
                        bool Closed = true;

                        using (NWDataSetTableAdapters.vwWeigh_SheetTableAdapter vwWeigh_SheetTableAdapter = new NWDataSetTableAdapters.vwWeigh_SheetTableAdapter())
                        {
                            vwWeigh_SheetTableAdapter.FillByWeight_Sheet_UID(NWDataset.vwWeigh_Sheet, Weight_Sheet_UID);
                            {
                                using (NWDataSetTableAdapters.LocationsTableAdapter locationsTableAdapter = new NWDataSetTableAdapters.LocationsTableAdapter())
                                {
                                    if (NWDataset.vwWeigh_Sheet.Count > 0)
                                    {
                                        locationsTableAdapter.FillById(NWDataset.Locations, NWDataset.vwWeigh_Sheet[0].Location_Id);
                                        Closed = NWDataset.vwWeigh_Sheet[0].Closed;
                                        if (ckOriginal.Visible) NWDataset.vwWeigh_Sheet[0].Original_Printed = !ShowAsOriginalPrinted;
                                    }
                                    else
                                    {
                                        using (NWDataSetTableAdapters.Weight_SheetsTableAdapter weight_SheetsTableAdapter = new NWDataSetTableAdapters.Weight_SheetsTableAdapter())
                                        {
                                            if (weight_SheetsTableAdapter.Fill(NWDataset.Weight_Sheets, Weight_Sheet_UID) > 0)
                                            {
                                                locationsTableAdapter.FillById(NWDataset.Locations, NWDataset.Weight_Sheets[0].Location_Id);
                                                Closed = NWDataset.Weight_Sheets[0].Closed;

                                            }
                                        }
                                    }
                                }
                                using (NWDataSetTableAdapters.vwWeight_Sheet_InformationTableAdapter vwWeight_Sheet_InformationTableAdapter = new NWDataSetTableAdapters.vwWeight_Sheet_InformationTableAdapter())
                                {
                                    if (vwWeight_Sheet_InformationTableAdapter.FillByUID(NWDataset.vwWeight_Sheet_Information, Weight_Sheet_UID) > 0)
                                    {

                                        {
                                            rptWeigh_Sheet.SetDataSource(NWDataset);
                                            rptWeigh_Sheet.Subreports[0].SetDataSource(NWDataset);
                                            decimal TotalBilled = 0;
                                            if (!NWDataset.vwWeight_Sheet_Information[0].IsTotal_BilledNull())
                                            {
                                                TotalBilled = NWDataset.vwWeight_Sheet_Information[0].Total_Billed;
                                            }

                                            var Custom = (!NWDataset.vwWeight_Sheet_Information[0].IsBOL_TypeNull() && NWDataset.vwWeight_Sheet_Information[0].BOL_Type.ToUpper().Trim() == "C");

                                            rptWeigh_Sheet.SetParameterValue("CustomRate", Custom);

                                            rptWeigh_Sheet.SetParameterValue("Closed", Closed);
                                            rptWeigh_Sheet.SetParameterValue("End_Of_Lot", NWDataset.vwWeight_Sheet_Information[0].Is_End_Lot);
                                            rptWeigh_Sheet.SetParameterValue("New_Lot", NWDataset.vwWeight_Sheet_Information[0].Is_New_Lot);
                                            rptWeigh_Sheet.SetParameterValue("Total_Billed", TotalBilled);

                                            rptWeigh_Sheet.SetParameterValue("Location_Description", NWDataset.vwWeight_Sheet_Information[0].Location_Description);
                                            rptWeigh_Sheet.SetParameterValue("Creation_Date", NWDataset.vwWeight_Sheet_Information[0].Creation_Date);

                                            rptWeigh_Sheet.SetParameterValue("Unofficial", !ShowAsOriginalPrinted);
                                            rptWeigh_Sheet.SetParameterValue("Customer_Copy", false);
                                            rptWeigh_Sheet.SetParameterValue("Lot", NWDataset.vwWeight_Sheet_Information[0].Lot_Number);
                                            rptWeigh_Sheet.SetParameterValue("StrWS_Id", NWDataset.vwWeight_Sheet_Information[0].WS_Id.ToString().PadLeft(8, '0'));

                                            //NWDataset.vwWeight_Sheet_Information[0].Original_Printed = !Official;

                                            //var parameterRow = NWDataset.ReportParameters.NewReportParametersRow();
                                            //parameterRow.WSUID = Weight_Sheet_UID;
                                            //parameterRow.CustomRate = Custom;

                                            //parameterRow.Closed = Closed;
                                            //parameterRow.End_Of_Lot = NWDataset.vwWeight_Sheet_Information[0].Is_End_Lot;
                                            //parameterRow.New_Lot = NWDataset.vwWeight_Sheet_Information[0].Is_New_Lot;
                                            //parameterRow.Total_Billed = TotalBilled;

                                            //parameterRow.Location_Description = NWDataset.vwWeight_Sheet_Information[0].Location_Description;
                                            //parameterRow.Creation_Date = NWDataset.vwWeight_Sheet_Information[0].Creation_Date;
                                            //// LOOOOOOOOK
                                            //parameterRow.Unofficial = !ShowAsOriginalPrinted;
                                            //parameterRow.Customer_Copy = false;
                                            //parameterRow.Lot = NWDataset.vwWeight_Sheet_Information[0].Lot_Number;
                                            //parameterRow.StrWS_Id = NWDataset.vwWeight_Sheet_Information[0].WS_Id.ToString().PadLeft(8, '0');
                                            //NWDataset.ReportParameters.AddReportParametersRow(parameterRow);

                                        

                                            //
                                            if (Export)
                                            {
                                                ExportOptions exportOpts = new ExportOptions();
                                                exportOpts.ExportFormatType = ExportFormatType.PortableDocFormat;

                                                EditableRTFExportFormatOptions exportFormatOptions =
                                                  ExportOptions.CreateEditableRTFExportFormatOptions();
                                                exportFormatOptions.FirstPageNumber = 1;
                                                exportFormatOptions.InsertPageBreaks = true;
                                                exportFormatOptions.LastPageNumber = 1;
                                                exportFormatOptions.UsePageRange = true;
                                                exportOpts.ExportFormatOptions = exportFormatOptions;


                                                rptWeigh_Sheet.ExportToHttpResponse(exportOpts, Response, true, string.Format("WeightSheet_{0}", NWDataset.vwWeight_Sheet_Information[0].WS_Id));
                                            }
                                            else
                                            {
                                                CrystalReportViewer1.ReportSource = rptWeigh_Sheet;
                                                CrystalReportViewer1.DataBind();

                                            }





                                            //string DirectoryPath = @"c:\ScaleTickets";
                                            //string Filename = DirectoryPath + @"\WS" + NWDataset.vwWeigh_Sheet[0].Location_Id.ToString() + "_" + NWDataset.vwWeigh_Sheet[0].WS_Id.ToString() + ".pdf";
                                            //if (!System.IO.Directory.Exists(DirectoryPath)) { System.IO.Directory.CreateDirectory(DirectoryPath); }
                                            //if (!System.IO.File.Exists(Filename)) { System.IO.File.Delete(Filename); }
                                            //rptWeigh_Sheet.ExportToDisk(ExportFormatType.PortableDocFormat, Filename);
                                        }
                                    }

                                }

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




    public bool GetTransferWeightSheet(Guid Weight_Sheet_UID, bool Export)
    {




        try
        {


          
            {


                using (NWDataSet NWDataset = new NWDataSet())
                {
                    rptWeigh_Sheet.Load(Server.MapPath("Transfer_Weigh_Sheet.rpt"));
                    {


                        using (NWDataSetTableAdapters.vwTransfer_LoadTableAdapter vwTransfer_LoadTableAdapter = new NWDataSetTableAdapters.vwTransfer_LoadTableAdapter())
                        {
                            bool Closed = true;
                            vwTransfer_LoadTableAdapter.FillByWeight_Sheet_UID(NWDataset.vwTransfer_Load, Weight_Sheet_UID);
                            {
                                using (NWDataSetTableAdapters.LocationsTableAdapter locationsTableAdapter = new NWDataSetTableAdapters.LocationsTableAdapter())
                                {
                                    if (NWDataset.vwTransfer_Load.Count > 0)
                                    {
                                        locationsTableAdapter.FillById(NWDataset.Locations, NWDataset.vwTransfer_Load[0].Location_Id);
                                        Closed = NWDataset.vwTransfer_Load[0].Closed;
                                        if (ckOriginal.Visible) NWDataset.vwTransfer_Load[0].Original_Printed = !ShowAsOriginalPrinted; 
                                    }
                                    else
                                    {
                                        using (NWDataSetTableAdapters.Weight_SheetsTableAdapter weight_SheetsTableAdapter = new NWDataSetTableAdapters.Weight_SheetsTableAdapter())
                                        {
                                            if (weight_SheetsTableAdapter.Fill(NWDataset.Weight_Sheets,Weight_Sheet_UID )>0)
                                            {
                                                locationsTableAdapter.FillById(NWDataset.Locations, NWDataset.Weight_Sheets[0].Location_Id );
                                                Closed = NWDataset.Weight_Sheets[0].Closed;
                                  
                                            }
                                        }
                                    }
                                }
                                using (NWDataSetTableAdapters.vwTransfer_Weight_Sheet_InformationTableAdapter vwTransfer_Weight_Sheet_InformationTableAdapter = new NWDataSetTableAdapters.vwTransfer_Weight_Sheet_InformationTableAdapter())
                                {
                                    if (vwTransfer_Weight_Sheet_InformationTableAdapter.FillByWeight_SheetUID(NWDataset.vwTransfer_Weight_Sheet_Information, Weight_Sheet_UID) > 0)
                                    {
                                        {


                                            rptWeigh_Sheet.SetDataSource(NWDataset);
                                            rptWeigh_Sheet.Subreports[0].SetDataSource(NWDataset);
                                            decimal TotalBilled = 0;
                                            if (!NWDataset.vwTransfer_Weight_Sheet_Information[0].IsTotal_BilledNull())
                                            {
                                                TotalBilled = NWDataset.vwTransfer_Weight_Sheet_Information[0].Total_Billed;
                                            }



                                            var Custom = (!NWDataset.vwTransfer_Weight_Sheet_Information[0].IsBOL_TypeNull() && NWDataset.vwTransfer_Weight_Sheet_Information[0].BOL_Type.ToUpper().Trim() == "C");
                                           
                                            rptWeigh_Sheet.SetParameterValue( "CustomRate", Custom);
                                            rptWeigh_Sheet.SetParameterValue("Closed", Closed);
                                            rptWeigh_Sheet.SetParameterValue("Total_Billed", TotalBilled);
                                            rptWeigh_Sheet.SetParameterValue("Location_Description", NWDataset.vwTransfer_Weight_Sheet_Information[0].Location_Description);
                                            rptWeigh_Sheet.SetParameterValue("Creation_Date", NWDataset.vwTransfer_Weight_Sheet_Information[0].Creation_Date);
                                            //rptWeigh_Sheet.SetParameterValue("Location_Id", NWDataset.vwTransfer_Weight_Sheet_Information[0].Location_Id);
                                            rptWeigh_Sheet.SetParameterValue("Unofficial", !ShowAsOriginalPrinted);
                                            rptWeigh_Sheet.SetParameterValue("StrWS_Id", NWDataset.vwTransfer_Weight_Sheet_Information[0].WS_Id.ToString().PadLeft(8, '0'));
                                            //NWDataset.vwTransfer_Weight_Sheet_Information[0].Original_Printed = !Official;
                                            if (Export)
                                            {

                                                ExportOptions exportOpts = new ExportOptions();
                                                exportOpts.ExportFormatType = ExportFormatType.PortableDocFormat;
                                                EditableRTFExportFormatOptions exportFormatOptions =
                                                  ExportOptions.CreateEditableRTFExportFormatOptions();
                                                exportFormatOptions.FirstPageNumber = 1;
                                                exportFormatOptions.InsertPageBreaks = true;
                                                exportFormatOptions.LastPageNumber = 1;
                                                exportFormatOptions.UsePageRange = true;
                                                exportOpts.ExportFormatOptions = exportFormatOptions;





                                                rptWeigh_Sheet.ExportToHttpResponse(exportOpts, Response, true, string.Format("WeightSheet_{0}.pdf", NWDataset.vwTransfer_Weight_Sheet_Information[0].WS_Id));
                                            }
                                            else
                                            {
                                                CrystalReportViewer1.ReportSource = rptWeigh_Sheet;
                                                CrystalReportViewer1.DataBind();

                                            }
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
        catch (Exception ex)
        {
         
            return false;
        }

    }


    public MemoryStream GetTransferWeightSheet(Guid Weight_Sheet_UID)
    {

        MemoryStream stream = new MemoryStream();


        try
        {



            {


                using (NWDataSet NWDataset = new NWDataSet())
                {
                    rptWeigh_Sheet.Load(Server.MapPath("Transfer_Weigh_Sheet.rpt"));
                    {


                        using (NWDataSetTableAdapters.vwTransfer_LoadTableAdapter vwTransfer_LoadTableAdapter = new NWDataSetTableAdapters.vwTransfer_LoadTableAdapter())
                        {
                            bool Closed = true;
                            vwTransfer_LoadTableAdapter.FillByWeight_Sheet_UID(NWDataset.vwTransfer_Load, Weight_Sheet_UID);
                            {
                                using (NWDataSetTableAdapters.LocationsTableAdapter locationsTableAdapter = new NWDataSetTableAdapters.LocationsTableAdapter())
                                {
                                    if (NWDataset.vwTransfer_Load.Count > 0)
                                    {
                                        locationsTableAdapter.FillById(NWDataset.Locations, NWDataset.vwTransfer_Load[0].Location_Id);
                                        Closed = NWDataset.vwTransfer_Load[0].Closed;
                                        if (ckOriginal.Visible) NWDataset.vwTransfer_Load[0].Original_Printed = !ShowAsOriginalPrinted;
                                    }
                                    else
                                    {
                                        using (NWDataSetTableAdapters.Weight_SheetsTableAdapter weight_SheetsTableAdapter = new NWDataSetTableAdapters.Weight_SheetsTableAdapter())
                                        {
                                            if (weight_SheetsTableAdapter.Fill(NWDataset.Weight_Sheets, Weight_Sheet_UID) > 0)
                                            {
                                                locationsTableAdapter.FillById(NWDataset.Locations, NWDataset.Weight_Sheets[0].Location_Id);
                                                Closed = NWDataset.Weight_Sheets[0].Closed;

                                            }
                                        }
                                    }
                                }
                                using (NWDataSetTableAdapters.vwTransfer_Weight_Sheet_InformationTableAdapter vwTransfer_Weight_Sheet_InformationTableAdapter = new NWDataSetTableAdapters.vwTransfer_Weight_Sheet_InformationTableAdapter())
                                {
                                    if (vwTransfer_Weight_Sheet_InformationTableAdapter.FillByWeight_SheetUID(NWDataset.vwTransfer_Weight_Sheet_Information, Weight_Sheet_UID) > 0)
                                    {
                                        {


                                            rptWeigh_Sheet.SetDataSource(NWDataset);
                                            rptWeigh_Sheet.Subreports[0].SetDataSource(NWDataset);
                                            decimal TotalBilled = 0;
                                            if (!NWDataset.vwTransfer_Weight_Sheet_Information[0].IsTotal_BilledNull())
                                            {
                                                TotalBilled = NWDataset.vwTransfer_Weight_Sheet_Information[0].Total_Billed;
                                            }



                                            var Custom = (!NWDataset.vwTransfer_Weight_Sheet_Information[0].IsBOL_TypeNull() && NWDataset.vwTransfer_Weight_Sheet_Information[0].BOL_Type.ToUpper().Trim() == "C");

                                            rptWeigh_Sheet.SetParameterValue("CustomRate", Custom);
                                            rptWeigh_Sheet.SetParameterValue("Closed", Closed);
                                            rptWeigh_Sheet.SetParameterValue("Total_Billed", TotalBilled);
                                            rptWeigh_Sheet.SetParameterValue("Location_Description", NWDataset.vwTransfer_Weight_Sheet_Information[0].Location_Description);
                                            rptWeigh_Sheet.SetParameterValue("Creation_Date", NWDataset.vwTransfer_Weight_Sheet_Information[0].Creation_Date);
                                            //rptWeigh_Sheet.SetParameterValue("Location_Id", NWDataset.vwTransfer_Weight_Sheet_Information[0].Location_Id);
                                            rptWeigh_Sheet.SetParameterValue("Unofficial", !ShowAsOriginalPrinted);
                                            rptWeigh_Sheet.SetParameterValue("StrWS_Id", NWDataset.vwTransfer_Weight_Sheet_Information[0].WS_Id.ToString().PadLeft(8, '0'));
                                            //NWDataset.vwTransfer_Weight_Sheet_Information[0].Original_Printed = !Official;
                                          

                                                ExportOptions exportOpts = new ExportOptions();
                                                exportOpts.ExportFormatType = ExportFormatType.PortableDocFormat;
                                                EditableRTFExportFormatOptions exportFormatOptions =
                                                  ExportOptions.CreateEditableRTFExportFormatOptions();
                                                exportFormatOptions.FirstPageNumber = 1;
                                                exportFormatOptions.InsertPageBreaks = true;
                                                exportFormatOptions.LastPageNumber = 1;
                                                exportFormatOptions.UsePageRange = true;
                                                exportOpts.ExportFormatOptions = exportFormatOptions;





                                                rptWeigh_Sheet.ExportToHttpResponse(exportOpts, Response, true, string.Format("WeightSheet_{0}.pdf", NWDataset.vwTransfer_Weight_Sheet_Information[0].WS_Id));

                                            ExportRequestContext req = new ExportRequestContext();

                                            req.ExportInfo = exportOpts;


                                            Stream s = rptWeigh_Sheet.FormatEngine.ExportToStream(req);
                                            s.CopyTo(stream);
                                        }
                                    }
                                
                                }

                            }
                        }
                    }
                }

            }
           
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.Print(ex.Message);

        }
        return stream;
    }

    protected void lnkPrint_Click(object sender, EventArgs e)
    {
        if (Transfer)
        {
            GetTransferWeightSheet(UID, true);
        }
        else
        {
            GetIntakeWeightSheet(UID, true);

        }
    }

    protected void lnkOpenWS_Click(object sender, EventArgs e)
    {
        using (NWDataSetTableAdapters.QueriesTableAdapter Q = new NWDataSetTableAdapters.QueriesTableAdapter())
        {
            Q.ChangeWeightSheetClosedStatus(false, UID);
            Session["OpenState"] = "Open/Closed";
        }
        Page.Response.Redirect(Page.Request.Url.ToString(), true);
        //lnkOpenWS.Visible = false;

        //if (Transfer)
        //{
        //    GetTransferWeightSheet(UID, false);
        //}
        //else
        //{
        //    GetIntakeWeightSheet(UID, false);

        //}
    }

    protected void lnkOriginal_Click(object sender, EventArgs e)
    {
        bool OriginalPrinted= hfOriginalPrinted.Value == true.ToString();
        using (NWDataSetTableAdapters.QueriesTableAdapter Q = new NWDataSetTableAdapters.QueriesTableAdapter())
        {
            Q.UpdateOriginalPrintedStatus(!OriginalPrinted, UID);
        }

        Page.Response.Redirect(Page.Request.Url.ToString(), true);
    }

    protected void lnkFix_Click(object sender, EventArgs e)
    {
        if (Transfer)
        {
            Response.Redirect("~/WeightSheets/WeightSheetDetails/TransferSheetDetails.aspx?UID=" + UID.ToString());
        }
        else
        {
            Response.Redirect("~/WeightSheets/WeightSheetDetails/WeightSheetDetails.aspx?UID=" + UID.ToString());
        }
        

    }

    protected void lnkEmail_Click(object sender, EventArgs e)
    {

    }

   
}