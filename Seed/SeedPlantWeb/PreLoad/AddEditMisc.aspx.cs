using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PreLoad_AddEditMisc : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (! this.IsPostBack )
        {
            hfLocation.Value = GlobalVars.Location.ToString();
            ddMisc.DataBind();
            ddMisc.SelectedIndex = 0;
            btnOk.Visible = false;
        }
        
        if (SeedTicketInfo.CurrentSeedTicketRow == null )
        {
            Response.Redirect("TicketCreator");
        }

    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        if(SeedTicketInfo.CurrentSeedTicketRow  != null)
        {
            bool NotFound;
            int ID = int.Parse(ddMisc.SelectedValue);
            int Quantity = int.Parse(txtQuantity.Text);
            SeedTicketDataSet.Seed_Ticket_MiscRow row = SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Misc.FirstOrDefault(x => x.Item_Id == ID);
            NotFound = (row == null);
            if (NotFound )
            {
                row = SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Misc.NewSeed_Ticket_MiscRow();
                row.UID = Guid.NewGuid();
                row.Seed_Ticket_UID = SeedTicketInfo.CurrentSeedTicketRow.UID;
                row.Item_Id = ID;
                row.Description = ddMisc.SelectedItem.Text;
                row.Quantity = 0;
            }
            row.Quantity += Quantity;
            row.Comment = "";
            row.Hidden = false;
           if (NotFound ) SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Misc.AddSeed_Ticket_MiscRow(row);
            Response.Redirect("TicketCreator");
        }
    }

    protected void txtQuantity_TextChanged(object sender, EventArgs e)
    {
        int val;
        if (!int.TryParse(txtQuantity.Text, out val)) txtQuantity.Text = "1";

    }

    protected void ddMisc_TextChanged(object sender, EventArgs e)
    {
        btnOk.Visible = ddMisc.SelectedIndex > 0;
    }
}