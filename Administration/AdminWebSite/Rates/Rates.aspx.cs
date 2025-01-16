using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Rates_Rates : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Security.GetUsersSecurityLevel(Session) == Security.enumSecurityLevel.View) Response.Redirect("~/Default.aspx");
    }
}