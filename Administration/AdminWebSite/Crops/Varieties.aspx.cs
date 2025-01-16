using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Crops_Varieties : System.Web.UI.Page
{




   

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Security.GetUsersSecurityLevel(Session) == Security.enumSecurityLevel.View) Response.Redirect("~/Default.aspx");
        if (!this.IsPostBack)
        {
            try
            {
                if (Request.QueryString["Commodity_ID"] != null)
                {
                    int Commodity_Id;
                    if (int.TryParse(Request.QueryString["Commodity_ID"].ToString(), out Commodity_Id))
                    {
                        ddCommodity.DataBind();
                        ddCommodity.ClearSelection();
                        ddCommodity.Items.FindByValue(Request.QueryString["Commodity_ID"].ToString()).Selected = true;
                    }
                }

              


            }
            catch
            { }
        }
    



    }







    protected void SqlSeedPlantItems_DataBinding(object sender, EventArgs e)
    {

    }



}