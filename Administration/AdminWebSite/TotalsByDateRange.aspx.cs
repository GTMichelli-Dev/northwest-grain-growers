using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TotalsByDateRange : System.Web.UI.Page
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
            hfSelectedEndDate.Value = dt.ToShortDateString();
            txtSelectedDate.Text = dt.ToShortDateString();
            txtSelectedEndDate.Text = dt.ToShortDateString();
            SetUpdateLabel();
        }
        this.GrdTotals.DataBind();
        this.GridView1.DataBind();
    }



 protected void txtSelectedEndDate_TextChanged(object sender, EventArgs e)
    {
        CheckDate(hfSelectedEndDate, (TextBox)sender);
    }


    private void CheckDate(HiddenField hf, TextBox txtbx)
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
        this.DataBind();
    }

    protected void txtSelectedDate_TextChanged(object sender, EventArgs e)
    {
        
        CheckDate(hfSelectedDate,(TextBox)sender );
    }

    protected void tmrUpdate_Tick(object sender, EventArgs e)
    {
        SetUpdateLabel();
    }

    public void SetUpdateLabel()
    {
        DateTime dt = DateTime.Now;
        if (DateTime.TryParse(txtSelectedDate.Text, out dt))
        {

            if ((int)((dt - DateTime.Now).TotalDays) == 0)
            {
                lblLastUpdate.Text = "Last Updated " + DateTime.Now.ToString();
            }
            else
            {
                lblLastUpdate.Text = "Totals";
            }


        }

    }
}
