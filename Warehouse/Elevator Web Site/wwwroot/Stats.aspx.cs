using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Stats : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Label1.Text = "Refreshed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() ;
    }
}