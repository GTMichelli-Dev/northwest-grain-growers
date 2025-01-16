using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class _Default : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DateTime currentdate = DateTime.Now;
        string LastUpdatetime = "Last Update: " + currentdate.ToLongDateString() + " @ " + currentdate.ToLongTimeString();
        this.lblLastUpdate.Text = LastUpdatetime;
        this.DataBind();
    }

    protected void tmrUpdate_Tick(object sender, EventArgs e)
    {
       
      
    }

    protected void GridView1_DataBound(object sender, EventArgs e)
    {
       
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
           DataRowView drv = e.Row.DataItem as DataRowView;
            
            if (drv["Status"].ToString().Equals("In Progress"))
            {
                    e.Row.BackColor = System.Drawing.Color.LightYellow ;
            }
            else if (drv["Status"].ToString().Equals("OK"))
            {
                e.Row.BackColor = System.Drawing.Color.LightGreen ;
            }

            else if (drv["Status"].ToString().Equals("Fail"))
            {
                e.Row.BackColor = System.Drawing.Color.Pink ;
            }
            else
            {
                e.Row.BackColor = System.Drawing.Color.White  ;
            }
        }
    }
}