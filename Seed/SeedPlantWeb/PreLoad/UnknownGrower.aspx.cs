using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UnknownGrower : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            
            if ((Request.QueryString["Reset"] != null) || (SeedTicketInfo.seedTicketDataSet == null))
            {
                SeedTicketInfo.CreateSeedTicket();
            }
            if (!SeedTicketInfo.CurrentSeedTicketRow.IsGrower_NameNull())
            {
                txtgrower.Text = SeedTicketInfo.CurrentSeedTicketRow.Grower_Name;
            }
            else
            {
                txtgrower.Text=  (Request.QueryString["GrowerName"] != null) ? Request.QueryString["GrowerName"] : "";
            }         
        }


        Update_Page();

    }


    private void Update_Page()
    {
     
     
            if (SeedTicketInfo.seedTicketDataSet == null)
            {
                SeedTicketInfo.CreateSeedTicket();
            }

        btnOk.Visible = txtgrower.Text.Length > 1;


         up1.Update();
    }



   


    protected void txtgrower_TextChanged(object sender, EventArgs e)
    {
        btnOk.Visible = txtgrower.Text.Length > 1;
    }



  

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default.aspx");
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        SeedTicketInfo.CurrentSeedTicketRow.Grower_Name = txtgrower.Text ;
        SeedTicketInfo.CurrentSeedTicketRow.Grower_ID = -1000;
        Response.Redirect("/Preload/TicketCreator.aspx");
    }

   
}