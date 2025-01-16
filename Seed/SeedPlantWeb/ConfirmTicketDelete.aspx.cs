using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ConfirmTicketDelete : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["UID"] ==null )
        {
            Response.Redirect("Default");
        }
        else
        {

            lblhdr.InnerText = $"Delete Ticket? ";
        }
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        Guid UID;
        
      if ( Guid.TryParse(Request["UID"].ToString(),out UID))
        {
            {
                using (SeedTicketDataSetTableAdapters.QueriesTableAdapter Q = new SeedTicketDataSetTableAdapters.QueriesTableAdapter())
                {
                    Q.DeleteAllSeedticketTreatments(UID);
                    Q.DeleteAllSeedTicketVarieties(UID);
                    Q.DeleteAllSeedTicketWeights(UID);
                    Q.Delete_Seed_Ticket(UID);
                }
               

            }
        }


       


            Response.Redirect("Default");
    }
}