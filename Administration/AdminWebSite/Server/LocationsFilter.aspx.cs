using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Server_LocationFilter : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var id = Request.QueryString["Id"];
        if (string.IsNullOrEmpty(id))
        {
            Response.Redirect("~/Server/Locations.aspx");
            return;
        }
    }

   
}