using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Misc_LastLoadReported : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblLastUpdate.Text = string.Format("Last Load Reported By Location - Last Updated: <strong> {0}</strong>", DateTime.Now);
        if (this.IsPostBack)
        {
            this.grdLastLoad.DataBind();
            
        }
    }
}