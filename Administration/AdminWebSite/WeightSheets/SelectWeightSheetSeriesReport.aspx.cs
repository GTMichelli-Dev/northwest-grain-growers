using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WeightSheets_SelectWeightSheetSeriesReport : System.Web.UI.Page
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

                cboLocation.DataBind();
                cboLocation.ClearSelection();
                cboLocation.Items.FindByText(Location).Selected = true;

            }
            catch
            { }
        }
        this.lnkSelect.Visible = cboLocation.SelectedIndex > 0;
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




    protected void cboLocation_TextChanged(object sender, EventArgs e)
    {
        Location = cboLocation.SelectedItem.Text;

    }

    protected void lnkSelect_Click(object sender, EventArgs e)
    {
        Response.Redirect(string.Format("~/WeightSheets/DailyReport.aspx?WeightSheetSeries=true&Date={0}&LocationId={1}", StartDate, cboLocation.SelectedItem.Value));
    }
}