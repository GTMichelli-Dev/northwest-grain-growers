using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DailyReportSelection : System.Web.UI.Page
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
                this.txtEndDate.Text = EndDate.ToShortDateString();
                this.hfStart.Value = StartDate.ToShortDateString();
                this.hfEnd.Value = EndDate.ToShortDateString();
                cboLocation.DataBind();
                cboLocation.ClearSelection();
                cboLocation.Items.FindByText(Location).Selected = true;
                ddType.DataBind();
                ddType.ClearSelection();
                ddType.Items.FindByText(WSType).Selected = true;
            }
            catch
            { }
        }

    }

   

    protected void lnkDetails_Click(object sender, EventArgs e)
    {
        LinkButton lnkReprint = (LinkButton)sender;
        GridViewRow row = (GridViewRow)lnkReprint.NamingContainer;
        //Guid UID = (Guid)GridView1.DataKeys[row.RowIndex].Value;
        string RowType = row.Cells[3].Text;
        string Created = row.Cells[2].Text;
        
        LinkButton lnkDetails = (LinkButton)row.FindControl("lnkDetails");
        int Location_Id =int.Parse( lnkDetails.CommandArgument); 
        bool Transfer = (RowType == "Transfer");

        Response.Redirect(string.Format("~/WeightSheets/DailyReport.aspx?Date={0}&LocationId={1}&Transfer={2}", Created , Location_Id, Transfer));

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

    protected void txtEndDate_TextChanged(object sender, EventArgs e)
    {
        CheckDate(txtEndDate, hfEnd, "WSEndDate");
    }



    protected void cboLocation_TextChanged(object sender, EventArgs e)
    {
        Location = cboLocation.SelectedItem.Text;

    }

    protected void ddType_TextChanged(object sender, EventArgs e)
    {
        WSType = ddType.SelectedItem.Text;
    }


    protected void btnSummary_Click(object sender, EventArgs e)
    {

    }
}