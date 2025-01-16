using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class IntakeWeightSheets : System.Web.UI.Page
{

    public DateTime StartDate
    {

        get
        {
            DateTime dt = DateTime.Now;
            if (Session["WSStartDate"] != null)
            {
                DateTime.TryParse(Session["WSStartDate"].ToString(), out dt);
            }
            return dt;
        }
        set
        {

            Session["WSStartDate"] = value;
        }
    }

    public DateTime EndDate
    {

        get
        {
            DateTime dt = DateTime.Now;
            if (Session["WSEndDate"] != null)
            {
                DateTime.TryParse(Session["WSEndDate"].ToString(), out dt);
            }
            return dt;
        }
        set
        {

            Session["WSEndDate"] = value;

        }
    }


    public string WSType
    {

        get
        {
            string wt = "Intake/Transfer";
            if (Session["WSType"] != null)
            {
                wt = Session["WSType"].ToString();
            }
            return wt;
        }
        set
        {

            Session["WSType"] = value;

        }
    }



    public string Lot
    {

        get
        {
            string lc = "";
            if (Session["WSLot"] != null)
            {
                lc = Session["WSLot"].ToString();
                int LotNumber = 0;
                if (!int.TryParse(lc, out LotNumber))
                {
                    lc = "";
                    Session["WSLot"] = lc;
                }
            }
            return lc;
        }
        set
        {

            Session["WSLot"] = value;

        }
    }

    public string Location
    {

        get
        {
            string lc = "All Locations";
            if (Session["WSLocation"] != null)
            {
                lc = Session["WSLocation"].ToString();

            }
           
                GridView1.AllowPaging = (lc == "All Locations");
                
           
            
            return lc;
        }
        set
        {

            GridView1.AllowPaging = (value == "All Locations");
            Session["WSLocation"] = value;

        }
    }

    //public string Hauler
    //{

    //    get
    //    {
    //        string lc = "";
    //        if (Session["WSHauler"] != null)
    //        {
    //            lc = Session["WSHauler"].ToString();
    //        }
    //        return lc;
    //    }
    //    set
    //    {

    //        Session["WSHauler"] = value;

    //    }
    //}

    public string Producer
    {

        get
        {
            string lc = "All Producers";
            if (Session["WSProducer"] != null)
            {
                lc = Session["WSProducer"].ToString();
            }
            return lc;
        }
        set
        {

            Session["WSProducer"] = value;

        }
    }


    public string OpenState
    {

        get
        {
            string lc = "Closed";
            if (Session["OpenState"] != null)
            {
                lc = Session["OpenState"].ToString();
            }
            return lc;
        }
        set
        {

            Session["OpenState"] = value;

        }
    }

    public string Crop
    {

        get
        {
            string lc = "All Crops";
            if (Session["Crop"] != null)
            {
                lc = Session["Crop"].ToString();
            }
            return lc;
        }
        set
        {

            Session["Crop"] = value;

        }
    }


    public string Variety
    {

        get
        {
            string lc = "All Varieties";
            if (Session["Varieties"] != null)
            {
                lc = Session["All Varieties"].ToString();
            }
            return lc;
        }
        set
        {

            Session["Varieties"] = value;

        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            try
            {
                this.txtStartDate.Text = StartDate.ToShortDateString();
                this.txtEndDate.Text = EndDate.ToShortDateString();
                this.hfStart.Value = StartDate.ToShortDateString();
                this.hfEnd.Value = EndDate.ToShortDateString();
                this.txtLot.Text = Lot;
                cboLocation.DataBind();
                cboLocation.ClearSelection();
                cboLocation.Items.FindByText(Location).Selected = true;
                ddOpenClosed.DataBind();
                ddOpenClosed.ClearSelection();
                ddCrop.DataBind();
                ddCrop.ClearSelection();
                ddCrop.Items.FindByText(Crop).Selected = true;

                ddProducer.DataBind();
                ddProducer.ClearSelection();
                try
                {
                    ddProducer.Items.FindByText(Producer).Selected = true;
                }
                catch
                {
                    ddProducer.SelectedIndex = 0;
                }
                //try
                //{
                //    ddHauler.Items.FindByText(Hauler).Selected = true;
                //}
                //catch
                //{
                //    ddHauler.SelectedIndex = 0;
                //}
                this.ddOpenClosed.Items.FindByText(OpenState).Selected = true;
                this.GridView1.DataBind();
                btnEmail.Visible = (Security.GetUsersSecurityLevel(Session) == Security.enumSecurityLevel.Administrator);
            }
            catch
            { }
        }

        //  FilterPlaceholder.Visible = (string.IsNullOrWhiteSpace(txtWSID.Text) && string.IsNullOrWhiteSpace(txtLot.Text));

    }

    protected void lnkReprint_Click(object sender, EventArgs e)
    {
        LinkButton lnkReprint = (LinkButton)sender;
        GridViewRow row = (GridViewRow)lnkReprint.NamingContainer;
        Guid UID = (Guid)GridView1.DataKeys[row.RowIndex].Value;
        bool EndOfLot = false;
        bool.TryParse(row.Cells[7].Text, out EndOfLot);
        Printing.PrintWSTicket(Server, Response, UID, EndOfLot);
    }

    protected void lnkDetails_Click(object sender, EventArgs e)
    {
        LinkButton hyprLink = (LinkButton)sender;
        GridViewRow row = (GridViewRow)hyprLink.NamingContainer;
        Guid UID = (Guid)GridView1.DataKeys[row.RowIndex].Value;

        string url = string.Format("/WeightSheets/WeightSheet.aspx?WSUID={0}&Transfer={1}", UID, false);
        string script = $"window.open('{url}', '_blank');";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "OpenNewTab", script, true);
    }

    protected void txtStartDate_TextChanged(object sender, EventArgs e)
    {
        CheckDate(txtStartDate, hfStart, "WSStartDate");
        this.GridView1.DataBind();

    }

    private void CheckDate(TextBox sender, HiddenField hf, string SessionValue)
    {

        DateTime dt = DateTime.Now;
        if (DateTime.TryParse(sender.Text, out dt))
        {
            hf.Value = dt.ToShortDateString();
            sender.Text = dt.ToShortDateString();
            Session[SessionValue] = dt;

        }
        else
        {
            sender.Text = hf.Value;
        }
    }

    protected void txtEndDate_TextChanged(object sender, EventArgs e)
    {
        CheckDate(txtEndDate, hfEnd, "WSEndDate");
        this.GridView1.DataBind();
    }



    protected void cboLocation_TextChanged(object sender, EventArgs e)
    {
        Location = cboLocation.SelectedItem.Text;
        
        this.GridView1.DataBind();

    }

    protected void ddHauler_TextChanged(object sender, EventArgs e)
    {
        //Hauler = ddHauler.SelectedItem.Text;
        this.GridView1.DataBind();

    }

    protected void ddOpenClosed_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddOpenClosed_TextChanged(object sender, EventArgs e)
    {
        OpenState = ddOpenClosed.SelectedItem.Text;
        this.GridView1.DataBind();
    }

    protected void ddType_TextChanged1(object sender, EventArgs e)
    {

    }

    protected void ddProducer_TextChanged(object sender, EventArgs e)
    {
        Producer = ddProducer.SelectedItem.Text;
        this.GridView1.DataBind();
    }


    protected void ddCrop_TextChanged(object sender, EventArgs e)
    {
        Crop = ddCrop.SelectedItem.Text;
        this.GridView1.DataBind();
    }

    protected void txtLot_TextChanged(object sender, EventArgs e)
    {
        Lot = txtLot.Text.Trim();
        this.GridView1.DataBind();
    }


    protected void btnEmail_Click(object sender, EventArgs e)
    {
        List<Guid> uidList = new List<Guid>();

        // Loop through each row in the GridView
        foreach (GridViewRow row in GridView1.Rows)
        {
            // Get the underlying data item for the row
            var dataItem = GridView1.DataKeys[row.RowIndex].Value;

            // Assuming the underlying data item has a property named "uid" of type Guid
            Guid uid = (Guid)dataItem;

            uidList.Add(uid);
        }
        if (uidList.Count > 0)
        {
            Email.ResendEmail(uidList, HttpContext.Current);
            string script = $@"<script type='text/javascript'>alert('Weight Sheets Have Been Emailed');</script>";
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "alert", script, false);
        }

    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {

        Excel.ExportToExcel(GridView1, Response,$"Intake_WS");
       
    }



    protected void btnPrint_Click(object sender, EventArgs e)
    {


        List<Guid> uidList = new List<Guid>();

        // Loop through each row in the GridView
        foreach (GridViewRow row in GridView1.Rows)
        {
            // Get the underlying data item for the row
            var dataItem = GridView1.DataKeys[row.RowIndex].Value;

            // Assuming the underlying data item has a property named "uid" of type Guid
            Guid uid = (Guid)dataItem;

            uidList.Add(uid);
        }
        if (uidList.Count > 0)
        {
            PdfDocument outputDocument = new PdfDocument();

            List<MemoryStream> pdfStreams = new List<MemoryStream>();

            foreach (var uid in uidList)
            {
                pdfStreams.Add(GetIntakeWeightSheet(uid));
            }

            foreach (var pdfStream in pdfStreams)
            {
                pdfStream.Position = 0; // Reset stream position for reading
                PdfDocument inputDocument = PdfReader.Open(pdfStream, PdfDocumentOpenMode.Import);

                // Import pages from each report PDF and add them to the output document
                for (int i = 0; i < inputDocument.PageCount; i++)
                {
                    outputDocument.AddPage(inputDocument.Pages[i]);
                }
            }

            // Save combined PDF to a memory stream
            using (MemoryStream combinedPDF = new MemoryStream())
            {
                outputDocument.Save(combinedPDF, false);
                combinedPDF.Position = 0; // Reset stream position for reading

                // Send the combined PDF as an HTTP response
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", "attachment; filename=CombinedReport.pdf");
                Response.AddHeader("Content-Length", combinedPDF.Length.ToString());
                Response.OutputStream.Write(combinedPDF.ToArray(), 0, combinedPDF.ToArray().Length);
                Response.Flush();
                Response.End();
            }
        }

    }

    public MemoryStream GetIntakeWeightSheet(Guid Weight_Sheet_UID)
    {


        MemoryStream stream = new MemoryStream();

        var rptWeigh_Sheet = new ReportDocument();

        try
        {



         


                using (NWDataSet NWDataset = new NWDataSet())
                {
                    
                    var path = Server.MapPath("~/WeightSheets/Weigh_Sheet.rpt");

                    rptWeigh_Sheet.Load(Server.MapPath("~/WeightSheets/Weigh_Sheet.rpt"));
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

                                            rptWeigh_Sheet.SetParameterValue("Unofficial", true);
                                            rptWeigh_Sheet.SetParameterValue("Customer_Copy", false);
                                            rptWeigh_Sheet.SetParameterValue("Lot", NWDataset.vwWeight_Sheet_Information[0].Lot_Number);
                                            rptWeigh_Sheet.SetParameterValue("StrWS_Id", NWDataset.vwWeight_Sheet_Information[0].WS_Id.ToString().PadLeft(8, '0'));
                                            ExportOptions exportOpts = new ExportOptions();
                                            exportOpts.ExportFormatType = ExportFormatType.PortableDocFormat;

                                            EditableRTFExportFormatOptions exportFormatOptions =
                                              ExportOptions.CreateEditableRTFExportFormatOptions();
                                            exportFormatOptions.FirstPageNumber = 1;
                                            exportFormatOptions.InsertPageBreaks = true;
                                            exportFormatOptions.LastPageNumber = 1;
                                            exportFormatOptions.UsePageRange = true;
                                            exportOpts.ExportFormatOptions = exportFormatOptions;





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
        catch (Exception ex)
        {
            System.Diagnostics.Debug.Print(ex.Message);
            
        }
        finally
        {
            if (rptWeigh_Sheet != null)
            {
                rptWeigh_Sheet.Close();
                rptWeigh_Sheet.Dispose();
            }
        }
        return stream;
    }




  
}