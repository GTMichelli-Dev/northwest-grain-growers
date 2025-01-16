using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WeightSheets_WeightSheetLoads : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.lblHeader.Text = "Loads For Weight Sheet " + WS_ID;
    }


    public string WS_ID
    {
        get
        {
            string WSID = string.Empty;
            if (Request.QueryString["WS_ID"] != null)
            {
                WSID= Request.QueryString["WS_ID"].ToString();
            }
            return WSID;
        }
    }
}