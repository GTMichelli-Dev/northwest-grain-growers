using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


public partial class WeightSheets_SelectCommodityReport : System.Web.UI.Page
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

    public string Location
    {

        get
        {
            string lc = "All Locations";
            if (Session["WSLocation"] != null)
            {
                lc = Session["WSLocation"].ToString();
            }
            return lc;
        }
        set
        {

            Session["WSLocation"] = value;

        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            try
            {
                this.txtStartDate.Text = StartDate.ToShortDateString();

                this.hfStart.Value = StartDate.ToShortDateString();
                this.txtEndDate.Text = EndDate.ToShortDateString();
                this.hfEnd.Value = EndDate.ToShortDateString();


                cboLocation.DataBind();
                cboLocation.ClearSelection();
                BindData();
                //if  (cboLocation.Items.FindByText(Location)!=null)  cboLocation.Items.FindByText(Location).Selected = true;

            }
            catch
            { }
        }
        
    }





    protected void cboLocation_SelectedIndexChanged(object sender, EventArgs e)
    {

    }



    protected void txtStartDate_TextChanged(object sender, EventArgs e)
    {
        CheckDate(txtStartDate, hfStart, "WSStartDate");

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

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // Get the value of the "Transfered" field
            int transfered = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Transfered"));

            // If the value is greater than 0, set the background color to alert-warning
            if (transfered > 0)
            {
                e.Row.CssClass = "alert-warning";
            }
            else
            {
                e.Row.CssClass = "alert-success";
            }
        }
    }


    protected void cboLocation_TextChanged(object sender, EventArgs e)
    {
        Location = cboLocation.SelectedItem.Text;

    }

    protected void lnkSelect_Click(object sender, EventArgs e)
    {

        int? LocationId = null;
        var StartDate = DateTime.Parse(txtStartDate.Text);
        var EndDate = DateTime.Parse(txtEndDate.Text);
        if (cboLocation.SelectedIndex > 0 && (!string.IsNullOrEmpty(cboLocation.SelectedValue))) LocationId = LocationId = int.Parse(cboLocation.SelectedValue);

        Reporting.DownloadLoadsByCropAndDate(LocationId, StartDate, EndDate, Response);

    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {

        BindData();


    }


    private void BindData()
    {
        using (LoadReportDataSetTableAdapters.LoadsByCropAndDataTableAdapter dataTableAdapter = new LoadReportDataSetTableAdapters.LoadsByCropAndDataTableAdapter())
        {
            LoadReportDataSet.LoadsByCropAndDataDataTable lds = new LoadReportDataSet.LoadsByCropAndDataDataTable();

            int? LocationId = null;
            var StartDate = DateTime.Parse(txtStartDate.Text);
            var EndDate = DateTime.Parse(txtEndDate.Text);
            if (cboLocation.SelectedIndex > 0 && (!string.IsNullOrEmpty(cboLocation.SelectedValue))) LocationId = LocationId = int.Parse(cboLocation.SelectedValue);


            dataTableAdapter.Fill(lds, LocationId, StartDate, EndDate);

            GridView1.DataSource = lds;

            GridView1.DataBind();
            int recordCount = GridView1.Rows.Count;
            hfRecordCount.Value = recordCount.ToString();


        }
    }

}