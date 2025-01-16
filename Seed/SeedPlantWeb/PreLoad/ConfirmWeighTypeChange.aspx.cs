using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PreLoad_ConfirmWeighTypeChange : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["NewType"]==null )
        {
            Response.Redirect("TicketCreator");
        }
        else
        {
            lblFrom.InnerText =SeedTicketInfo.CurrentSeedTicketRow.Ticket_Type;
            lblTo.InnerText=Request.QueryString["NewType"];
        }
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        SeedTicketInfo.AddToAudit(SeedTicketInfo.enumAuditType.Update, SeedTicketInfo.CurrentSeedTicketRow.UID, SeedTicketInfo.CurrentSeedTicketRow.Ticket_Type, Request.QueryString["NewType"]);
        SeedTicketInfo.ResetWeights();
        GlobalVars.SeedTicketWeighType = (GlobalVars.enumSeedTicketWeighType)Enum.Parse(typeof(GlobalVars.enumSeedTicketWeighType), Request.QueryString["NewType"]);
        SeedTicketInfo.CurrentSeedTicketRow.Returned = Request.QueryString["NewType"].ToUpper().Contains("RETURN");
        SeedTicketInfo.CurrentSeedTicketRow.Ticket_Type = GlobalVars.SeedTicketWeighType.ToString();
        if (SeedTicketInfo.TicketWeighType == GlobalVars.enumSeedTicketWeighType.Bag || SeedTicketInfo.TicketWeighType == GlobalVars.enumSeedTicketWeighType.ReturnBag) SeedTicketInfo.AddBag(1);
        SeedTicketInfo.SaveTicket();
        Response.Redirect("TicketCreator");
    }
}