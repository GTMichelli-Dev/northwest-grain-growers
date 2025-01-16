using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PreLoad_LoadDetails : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                bool ReadOnly = SeedTicketInfo.CurrentSeedTicketRow.ReadOnly;
                ReportDataSet reportDataset = SeedTicketInfo.GetAllTicketInfo(SeedTicketInfo.seedTicketDataSet, SeedTicketInfo.CurrentSeedTicketRow.UID);
                if (!SeedTicketInfo.CurrentSeedTicketRow.IsPONull()) this.txtPO.Text = SeedTicketInfo.CurrentSeedTicketRow.PO;
                if (!SeedTicketInfo.CurrentSeedTicketRow.IsBOLNull()) this.txtBOL.Text = SeedTicketInfo.CurrentSeedTicketRow.BOL;
                if (!SeedTicketInfo.CurrentSeedTicketRow.IsTruck_IDNull()) this.txtVehicle.Text = SeedTicketInfo.CurrentSeedTicketRow.Truck_ID; 
                if (!SeedTicketInfo.CurrentSeedTicketRow.IsCommentsNull()) this.txtComment.Text = SeedTicketInfo.CurrentSeedTicketRow.Comments;
                lnkGrower.Enabled = !ReadOnly;
                txtPO.ReadOnly = ReadOnly;
                txtBOL.ReadOnly = ReadOnly;
                txtVehicle.ReadOnly = ReadOnly;
                txtComment.ReadOnly = ReadOnly; 
            }
            if (SeedTicketInfo.seedTicketDataSet == null || SeedTicketInfo.CurrentSeedTicketRow == null || SeedTicketInfo.CurrentSeedTicketRow.IsGrower_IDNull())
            {
                Response.Redirect("SelectGrower.aspx");
            }
            else
            {

                lnkGrower.Text = $"{SeedTicketInfo.CurrentSeedTicketRow.Grower_Name} - {SeedTicketInfo.CurrentSeedTicketRow.Grower_ID }";


            }

        }
        catch (Exception ex)
        {
            Auditing.LogMessage("LoadDetails.Page_Load", ex.Message);
            Response.Redirect("../Default.aspx");
        }
    }




   





  

   

   

  

    protected void txtPO_TextChanged(object sender, EventArgs e)
    {
        if (!SeedTicketInfo.CurrentSeedTicketRow.IsPONull()) SeedTicketInfo.AddToAudit(SeedTicketInfo.enumAuditType.Update, SeedTicketInfo.CurrentSeedTicketRow.UID, SeedTicketInfo.CurrentSeedTicketRow.PO, txtPO.Text);
            SeedTicketInfo.CurrentSeedTicketRow.PO = txtPO.Text;
        

    }

    protected void txtBOL_TextChanged(object sender, EventArgs e)
    {
        if (!SeedTicketInfo.CurrentSeedTicketRow.IsBOLNull()) SeedTicketInfo.AddToAudit(SeedTicketInfo.enumAuditType.Update, SeedTicketInfo.CurrentSeedTicketRow.UID, SeedTicketInfo.CurrentSeedTicketRow.BOL, txtBOL.Text);
        SeedTicketInfo.CurrentSeedTicketRow.BOL = txtBOL.Text;
    }

    protected void txtComment_TextChanged(object sender, EventArgs e)
    {
        SeedTicketInfo.CurrentSeedTicketRow.Comments = txtComment.Text;
    }

    protected void txtVehicle_TextChanged(object sender, EventArgs e)
    {
        if (!SeedTicketInfo.CurrentSeedTicketRow.IsTruck_IDNull()) SeedTicketInfo.AddToAudit(SeedTicketInfo.enumAuditType.Update, SeedTicketInfo.CurrentSeedTicketRow.UID, SeedTicketInfo.CurrentSeedTicketRow.Truck_ID , txtVehicle.Text);
        SeedTicketInfo.CurrentSeedTicketRow.Truck_ID = txtVehicle.Text;
    }
}