using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports_ProducersDeliveredReport : System.Web.UI.Page
{

    public DateTime StartDate
    {

        get
        {
            DateTime dt = DateTime.Now;
            if (Session["StartDate"] != null)
            {
                DateTime.TryParse(Session["StartDate"].ToString(), out dt);
            }
            return dt;
        }
        set
        {

            Session["StartDate"] = value;
        }
    }

    public DateTime EndDate
    {

        get
        {
            DateTime dt = DateTime.Now;
            if (Session["EndDate"] != null)
            {
                DateTime.TryParse(Session["EndDate"].ToString(), out dt);
            }
            return dt;
        }
        set
        {

            Session["EndDate"] = value;

        }
    }
    

    public string Producer
    {

        get
        {
            string lc = "";
            if (Session["Producer"] != null)
            {
                lc = Session["Producer"].ToString();
            }
            return lc;
        }
        set
        {

            Session["Producer"] = value;

        }
    }




    public string District
    {

        get
        {
            string lc = "All Districts";
            if (Session["Districts"] != null)
            {
                lc = Session["Districts"].ToString();
            }
            return lc;
        }
        set
        {

            Session["Districts"] = value;

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
                ddDistricts.DataBind();
                ddDistricts.ClearSelection();
                ddDistricts.Items.FindByText(District).Selected = true;

                txtProducer.Text = Producer;

            }
            catch
            {

            }
        }
    }


  

    protected void txtStartDate_TextChanged(object sender, EventArgs e)
    {
        CheckDate(txtStartDate, hfStart, "StartDate");
    }

    protected void txtEndDate_TextChanged(object sender, EventArgs e)
    {
        CheckDate(txtEndDate, hfEnd, "EndDate");
    }

    protected void ddDistricts_TextChanged(object sender, EventArgs e)
    {
        District = ddDistricts.SelectedItem.Text;
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

    protected void txtProducer_TextChanged(object sender, EventArgs e)
    {
        Producer = txtProducer.Text;
    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        Reporting.DownloadProducersDeliveredReport((ddDistricts.SelectedIndex>0)? District:"", Producer, StartDate, EndDate, Response);
    }
}