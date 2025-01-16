using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WeightSheets_IntakeLoadTotals : System.Web.UI.Page
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
                
                this.GridView1.DataBind();
              
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
        LinkButton lnkReprint = (LinkButton)sender;
        GridViewRow row = (GridViewRow)lnkReprint.NamingContainer;
        Guid UID = (Guid)GridView1.DataKeys[row.RowIndex].Value;


        Response.Redirect(string.Format("~/WeightSheets/WeightSheet.aspx?WSUID={0}&Transfer={1}", UID, false));

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





    protected void ddHauler_TextChanged(object sender, EventArgs e)
    {
        //Hauler = ddHauler.SelectedItem.Text;
        this.GridView1.DataBind();

    }

    protected void ddOpenClosed_SelectedIndexChanged(object sender, EventArgs e)
    {

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
            PrintSelectedWeightSheets(uidList);


        }
    }



    public void PrintSelectedWeightSheets(List<Guid> uidList)
    {

        try
        {
            //using (ReportDocument rptWeigh_Sheet = new ReportDocument())
            //{
            //    using (NWDataSet nWDataset = new NWDataSet())
            //    {
            //        using (NWDataSet filterDataset = new NWDataSet())
            //        {
            //            rptWeigh_Sheet.Load(Server.MapPath("Weigh_Sheet.rpt"));

            //            bool Closed = true;
            //            using (NWDataSetTableAdapters.vwWeight_Sheet_InformationTableAdapter vwWeight_Sheet_InformationTableAdapter = new NWDataSetTableAdapters.vwWeight_Sheet_InformationTableAdapter())
            //            {
            //                using (NWDataSetTableAdapters.LocationsTableAdapter locationsTableAdapter = new NWDataSetTableAdapters.LocationsTableAdapter())
            //                {
            //                    locationsTableAdapter.Fill(nWDataset.Locations);
            //                    using (NWDataSetTableAdapters.vwWeigh_SheetTableAdapter vwWeigh_SheetTableAdapter = new NWDataSetTableAdapters.vwWeigh_SheetTableAdapter())
            //                    {
            //                        foreach (var Weight_Sheet_UID in uidList)
            //                        {
            //                            if (vwWeigh_SheetTableAdapter.FillByWeight_Sheet_UID(filterDataset.vwWeigh_Sheet, Weight_Sheet_UID) > 0)
            //                            {

            //                                var WSFilteredRow = filterDataset.vwWeigh_Sheet[0];
            //                                Closed = WSFilteredRow.Closed;
            //                                nWDataset.vwWeigh_Sheet.ImportRow(WSFilteredRow);
            //                                if (vwWeight_Sheet_InformationTableAdapter.FillByUID(filterDataset.vwWeight_Sheet_Information, Weight_Sheet_UID) > 0)
            //                                {
            //                                    var vwWeight_Sheet_InformationRow = filterDataset.vwWeight_Sheet_Information[0];

            //                                    nWDataset.vwWeight_Sheet_Information.ImportRow(filterDataset.vwWeight_Sheet_Information[0]);



            //                                    decimal TotalBilled = 0;
            //                                    if (!vwWeight_Sheet_InformationRow.IsTotal_BilledNull())
            //                                    {
            //                                        TotalBilled = vwWeight_Sheet_InformationRow.Total_Billed;
            //                                    }

            //                                    var Custom = (!vwWeight_Sheet_InformationRow.IsBOL_TypeNull() && vwWeight_Sheet_InformationRow.BOL_Type.ToUpper().Trim() == "C");
            //                                    var parameterRow = nWDataset.ReportParameters.NewReportParametersRow();
            //                                    parameterRow.WSUID = Weight_Sheet_UID;
            //                                    parameterRow.CustomRate = Custom;

            //                                    parameterRow.Closed = Closed;
            //                                    parameterRow.End_Of_Lot = vwWeight_Sheet_InformationRow.Is_End_Lot;
            //                                    parameterRow.New_Lot = vwWeight_Sheet_InformationRow.Is_New_Lot;
            //                                    parameterRow.Total_Billed = TotalBilled;

            //                                    parameterRow.Location_Description = vwWeight_Sheet_InformationRow.Location_Description;
            //                                    parameterRow.Creation_Date = vwWeight_Sheet_InformationRow.Creation_Date;
            //                                    // LOOOOOOOOK
            //                                    parameterRow.Unofficial = true;
            //                                    parameterRow.Customer_Copy = true;
            //                                    parameterRow.Lot = vwWeight_Sheet_InformationRow.Lot_Number;
            //                                    parameterRow.StrWS_Id = vwWeight_Sheet_InformationRow.WS_Id.ToString().PadLeft(8, '0');
            //                                    nWDataset.ReportParameters.AddReportParametersRow(parameterRow);


            //                                }
            //                            }
            //                        }
            //                        rptWeigh_Sheet.SetDataSource(nWDataset);
            //                        rptWeigh_Sheet.Subreports[0].SetDataSource(nWDataset);


            //                        ExportOptions exportOpts = new ExportOptions();
            //                        exportOpts.ExportFormatType = ExportFormatType.PortableDocFormat;

            //                        EditableRTFExportFormatOptions exportFormatOptions =
            //                          ExportOptions.CreateEditableRTFExportFormatOptions();
            //                        //exportFormatOptions.FirstPageNumber = 1;
            //                        exportFormatOptions.InsertPageBreaks = true;
            //                        //exportFormatOptions.LastPageNumber = 1;
            //                        exportFormatOptions.UsePageRange = true;
            //                        exportOpts.ExportFormatOptions = exportFormatOptions;


            //                        rptWeigh_Sheet.ExportToHttpResponse(exportOpts, Response, true, string.Format("WeightSheets"));

            //                    }
            //                }
            //            }

            //        }
            //    }

            //}
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.Print(ex.Message);

        }

    }


    protected void ddVariety_TextChanged(object sender, EventArgs e)
    {
        Variety = ddVariety.SelectedItem.Text;
        this.GridView1.DataBind();
    }

    protected void cboLocation_TextChanged(object sender, EventArgs e)
    {
        Location = cboLocation.SelectedItem.Text;

        this.GridView1.DataBind();
    }
}