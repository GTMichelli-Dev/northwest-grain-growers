using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports_IntakeLocationReport : System.Web.UI.Page
{

    public DateTime SelectedDate
    {

        get
        {
            DateTime dt = DateTime.Now;
            if (!string.IsNullOrWhiteSpace(hfSelectedDate.Value.ToString()))
            {
                DateTime.TryParse(hfSelectedDate.Value.ToString(), out dt);

            }
            else
            {
                this.hfSelectedDate.Value = DateTime.Now.ToString();
            }

            return dt;
        }
        set
        {

            hfSelectedDate.Value = value.ToString();

        }
    }



    protected void Page_Load(object sender, EventArgs e)
    {





        if (!this.IsPostBack)
        {
            DateTime dt = DateTime.Now;

            hfSelectedDate.Value = dt.ToShortDateString();
            txtSelectedDate.Text = dt.ToShortDateString();
            hfSelectedEndDate.Value = dt.ToShortDateString();
            txtSelectedEndDate.Text = dt.ToShortDateString();
      
        }
    
        this.grd1.DataBind();




    }

    private void CheckDate(TextBox txtbx, HiddenField hf)
    {

        DateTime dt = DateTime.Now;
        if (DateTime.TryParse(txtbx.Text, out dt))
        {
            hf.Value = dt.ToShortDateString();
            txtbx.Text = dt.ToShortDateString();


        }
        else
        {
            txtbx.Text = hf.Value;
        }
        grd1.DataBind();
    }

    protected void txtSelectedDate_TextChanged(object sender, EventArgs e)
    {
        CheckDate(txtSelectedDate,hfSelectedDate);
    }



    protected void txtSelectedEndDate_TextChanged(object sender, EventArgs e)
    {
        CheckDate(txtSelectedEndDate,hfSelectedEndDate);
    }

    protected void Download_Click(object sender, EventArgs e)
    {
        Reporting.DownloadLocationIntakeReport( Convert.ToDateTime( hfSelectedDate.Value), Convert.ToDateTime( hfSelectedEndDate.Value), Response);
    }
}