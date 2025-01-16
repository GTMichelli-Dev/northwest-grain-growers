using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports_AverageProtein : System.Web.UI.Page
{
    ReportDocument rpt = new ReportDocument();
    private readonly LoadBinProteinService _service;
    public Reports_AverageProtein()
    {
        _service = new LoadBinProteinService(new NWDataModel());
    }
    public DateTime StartDate
    {

        get
        {
            DateTime dt = DateTime.Now;
            if (Session["WSStartDate"] == null) Session["WSStartDate"] = DateTime.Now.ToShortDateString();
            
            DateTime.TryParse(Session["WSStartDate"].ToString(), out dt);
            
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
            if (Session["WSEndDate"] == null) Session["WSEndDate"] = DateTime.Now.ToShortDateString();
            DateTime.TryParse(Session["WSEndDate"].ToString(), out dt);
           
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
                this.txtEndDate.Text = EndDate.ToShortDateString();

                this.hfStart.Value = StartDate.ToShortDateString();
                this.hfEnd.Value = EndDate.ToShortDateString();

                cboLocation.DataBind();
                cboLocation.ClearSelection();
                cboLocation.Items.FindByText(Location).Selected = true;
                //this.lnkSelect.Visible = cboLocation.SelectedIndex > 0;
               
            }
            catch
            { }

            BindGrid();
        }
        //this.lnkSelect.Visible = cboLocation.SelectedIndex > 0;
    }


    private void BindGrid()
    {
        DateTime startDate = DateTime.Parse(hfStart.Value);
        DateTime endDate = DateTime.Parse(hfEnd.Value);
        int? locationId = string.IsNullOrEmpty(cboLocation.SelectedValue) ? (int?)null : int.Parse(cboLocation.SelectedValue);

        var data = _service.GetLoadBinProteinReport(startDate, endDate, locationId).ToList();
        GridView1.DataSource = data;
        GridView1.DataBind();
    }


    protected void cboLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGrid();
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
        BindGrid();
    }




    protected void cboLocation_TextChanged(object sender, EventArgs e)
    {
        Location = cboLocation.SelectedItem.Text;
        
    }

    protected void lnkSelect_Click(object sender, EventArgs e)
    {
        int Location_Id;
        if (int.TryParse(cboLocation.SelectedValue, out Location_Id))
        {
            Response.Redirect(string.Format("~/WeightSheets/DailyReport.aspx?DailyBin=true&StartDate={0}&EndDate={1}&LocationId={2}&LocationName={3}", StartDate, EndDate, Location_Id,Location ));
        }
     

 
}

    protected void txtEndDate_TextChanged(object sender, EventArgs e)
    {
        CheckDate(txtEndDate, hfEnd , "WSEndDate");
    }
}