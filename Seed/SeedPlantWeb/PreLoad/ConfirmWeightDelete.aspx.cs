using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ConfirmWeightDelete : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (SeedTicketInfo.CurrentSeedTicketWeightRow ==null )
        {
            Response.Redirect("TicketCreator");
        }
        else
        {

            lblhdr.InnerText = $"Delete Weight From Ticket? ";
        }
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        SeedTicketInfo.AddToAudit(SeedTicketInfo.enumAuditType.Delete, SeedTicketInfo.CurrentSeedTicketWeightRow.UID , $"Removed Weight Row From Ticket. Ticket.UID={SeedTicketInfo.CurrentSeedTicketRow.UID.ToString()}"  );
        SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Weights.FindByUID(SeedTicketInfo.CurrentSeedTicketWeightRow.UID).Delete();
      //  SeedTicketInfo.seedTicketDataSet.AcceptChanges();
        SeedTicketInfo.SaveTicket();

    
        if (SeedTicketInfo.TicketWeighType == GlobalVars.enumSeedTicketWeighType.Tote)
        {

            using (ListDataSetTableAdapters.Misc_ItemsTableAdapter misc_ItemsTableAdapter = new ListDataSetTableAdapters.Misc_ItemsTableAdapter())
            {
                using (ListDataSet.Misc_ItemsDataTable misc_ItemsDataTable = new ListDataSet.Misc_ItemsDataTable())
                {
                    misc_ItemsTableAdapter.Fill(misc_ItemsDataTable, GlobalVars.Location);
                    ListDataSet.Misc_ItemsRow Lrow = misc_ItemsDataTable.FirstOrDefault(x => x.Description.ToUpper() == "TOTES");
                    if (Lrow != null)
                    {
                        SeedTicketDataSet.Seed_Ticket_MiscRow Mrow = SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Misc.FirstOrDefault(x => x.Item_Id == Lrow.ID);
                        if (Mrow != null)
                        {
                            Mrow.Quantity -= 1;
                            if (Mrow.Quantity <= 0) Mrow.Delete();
                        }
                    }
                }
            }
        }
        SeedTicketInfo.SaveTicket();

        Response.Redirect("TicketCreator");
    }
}