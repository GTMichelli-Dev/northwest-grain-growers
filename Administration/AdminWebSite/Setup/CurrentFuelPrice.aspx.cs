using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Setup_CurrentFuelPrice : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Security.GetUsersSecurityLevel(Session) == Security.enumSecurityLevel.View) Response.Redirect("~/Default.aspx");
        if (! this.IsPostBack )
        {
            using (NWDataSetTableAdapters.Site_SetupTableAdapter siteSetupTableAdapters = new NWDataSetTableAdapters.Site_SetupTableAdapter())
            {
                using (NWDataSet.Site_SetupDataTable SiteSetup = new NWDataSet.Site_SetupDataTable())
                {
                    siteSetupTableAdapters.Fill(SiteSetup);
                    this.txtCurrentPrice.Text = SiteSetup[0].Current_Fuel_Price.ToString();
                }
            }
        }
    }

    protected void btnNewOk_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtCurrentPrice.Text))
        {
            lblError.Text = "Price Cannot Be Blank";
        }
        else
        {
            decimal Price = 0;
            if (decimal.TryParse(txtCurrentPrice.Text, out Price))
            {
                if (Price > 10)
                {
                    lblError.Text = "Price Cannot Be Greater Than $10";
                }
                else
                {
                    try
                    {
                        using (NWDataSetTableAdapters.QueriesTableAdapter Q = new NWDataSetTableAdapters.QueriesTableAdapter())
                        {
                            Q.UpdateFuelPrice(Price);
                            Response.Redirect("~/Default.aspx");
                        }

                    }
                    catch (Exception ex)
                    {
                        lblError.Text = "Error Setting Price" + ex.Message;
                    }
                }
            }
            else
            {
                lblError.Text = "Not A Valid Price";
            }
        }
    }
}