using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UnknownVariety : System.Web.UI.Page
{


    private bool AddNew
    {
        get
        {
            if (Request["AddNew"] == null)
            {
                Response.Redirect("../Default.aspx");
                return false;
            }
            else
            {
                bool val;
                if (bool.TryParse(Request["AddNew"].ToString(), out val))
                {
                    return val;
                }
                else
                {
                    Response.Redirect("~/Default.aspx");
                    return false;
                }
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            if ((SeedTicketInfo.seedTicketDataSet == null))
            {
                Response.Redirect("~/PreLoad/SelectGrower.aspx");
            }
            else if ((SeedTicketInfo.CurrentSeedTicketVarietyRow == null))
            {
                Response.Redirect("~/PreLoad/TicketCreator.aspx");
            }

            if (!SeedTicketInfo.CurrentSeedTicketVarietyRow.IsVariety_IDNull() && !SeedTicketInfo.CurrentSeedTicketVarietyRow.IsCustom_NameNull())
            {
                txtFilter.Text = $"{ SeedTicketInfo.CurrentSeedTicketVarietyRow.Custom_Name}";
            }

            else
            {
                txtFilter.Text = (Request.QueryString["unknownVariety"] != null) ? Request.QueryString["unknownVariety"] : "";
            }

            Session["OriginalVarietyValues"] = SeedTicketInfo.CurrentSeedTicketRow;

            if (!AddNew)
            {
                btnOk.Visible = true;
                txtPercent.Text = ((int)SeedTicketInfo.CurrentSeedTicketVarietyRow.Percent_Of_Load).ToString();
            }


        }
        btnOk.Visible = (txtFilter.Text.Length>1);




    }


  


   
    protected void btnCancel_Click(object sender, EventArgs e)
    {
      
        Response.Redirect("TicketCreator.aspx");
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["AddNew"] != null)
        {
            if (SeedTicketInfo.CurrentSeedTicketVarietyRow != null)
            {
                int PercentOfLoad;
                int.TryParse(txtPercent.Text, out PercentOfLoad);
                SeedTicketInfo.CurrentSeedTicketVarietyRow.Custom_Name = txtFilter.Text;
              
                
                SeedTicketInfo.CurrentSeedTicketVarietyRow.Percent_Of_Load = PercentOfLoad;
                if (AddNew)
                {
                    int Seed = -1000;
                    SeedTicketDataSet.Seed_Ticket_VarietiesRow FoundRow = null;
                    do
                    {
                        FoundRow = SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Varieties.FirstOrDefault(x => x.Variety_ID == Seed);
                        Seed -= 1;

                    }
                    while (FoundRow != null);

                    SeedTicketInfo.CurrentSeedTicketVarietyRow.Variety_ID = Seed;
                    SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Varieties.AddSeed_Ticket_VarietiesRow(SeedTicketInfo.CurrentSeedTicketVarietyRow);
                    SeedTicketInfo.AddToAudit(SeedTicketInfo.enumAuditType.Create, SeedTicketInfo.CurrentSeedTicketRow.UID, "Seed Ticket Unknown Variety Added");
                }
                else
                {
                  if ( Session["OriginalVarietyValues"]!= null)
                    {
                        try
                        {
                            var OriginalRow = (SeedTicketDataSet.Seed_Ticket_VarietiesRow)Session["OriginalVarietyValues"];
                            if (OriginalRow.UID== SeedTicketInfo.CurrentSeedTicketVarietyRow.UID )
                            {
                                var crow = SeedTicketInfo.CurrentSeedTicketVarietyRow ;
                                if (crow.PC_Address != OriginalRow.PC_Address) SeedTicketInfo.AddToAudit(SeedTicketInfo.enumAuditType.Update, crow.UID, "Changed Treatment PC Address", crow.PC_Address, OriginalRow.PC_Address);
                                if(!crow.IsVariety_IDNull() && !OriginalRow.IsVariety_IDNull()  && crow.Variety_ID != OriginalRow.Variety_ID) SeedTicketInfo.AddToAudit(SeedTicketInfo.enumAuditType.Update, crow.UID, "Changed Variety_ID", crow.Variety_ID.ToString() , OriginalRow.Variety_ID.ToString() );
                                if (!crow.IsPercent_Of_LoadNull () && !OriginalRow.IsPercent_Of_LoadNull() && crow.Percent_Of_Load  != OriginalRow.Percent_Of_Load ) SeedTicketInfo.AddToAudit(SeedTicketInfo.enumAuditType.Update, crow.UID, "Changed Percent Of Load", crow.Percent_Of_Load.ToString(), OriginalRow.Percent_Of_Load.ToString());
                                if (!crow.IsCustom_NameNull() && !OriginalRow.IsCustom_NameNull() && crow.Custom_Name  != OriginalRow.Custom_Name ) SeedTicketInfo.AddToAudit(SeedTicketInfo.enumAuditType.Update, crow.UID, "Changed Variety Custom Name", crow.Custom_Name , OriginalRow.Custom_Name);
                                if (!crow.IsLotNull() && !OriginalRow.IsLotNull() && crow.Lot != OriginalRow.Lot) SeedTicketInfo.AddToAudit(SeedTicketInfo.enumAuditType.Update, crow.UID, "Changed Lot", crow.Lot, OriginalRow.Lot);
                                if (!crow.IsBinNull() && !OriginalRow.IsBinNull() && crow.Bin != OriginalRow.Bin) SeedTicketInfo.AddToAudit(SeedTicketInfo.enumAuditType.Update, crow.UID, "Changed Bin", crow.Bin.ToString(), OriginalRow.Bin.ToString());
                                if (!crow.IsBin_NameNull() && !OriginalRow.IsBin_NameNull() && crow.Bin_Name != OriginalRow.Bin_Name) SeedTicketInfo.AddToAudit(SeedTicketInfo.enumAuditType.Update, crow.UID, "Changed Bin_Name", crow.Bin_Name, OriginalRow.Bin_Name);
                            }
                        }
                        catch (Exception ex)
                        {
                            Auditing.LogMessage("UnknownVariety.btnOK", ex.Message);
                        }
                    } 
                }
            }

        }
        Response.Redirect("TicketCreator.aspx");
        
    }


    private void ResetVariety()
    {
        txtFilter.Text = "";
    }



    protected void txtFilter_TextChanged(object sender, EventArgs e)
    {

    }

    protected void txtPercent_TextChanged(object sender, EventArgs e)
    {
        int val = 100;
        if (!int.TryParse(txtPercent.Text, out val))
        {
            val = 100;
        }

        //val = Math.Round((double)val, 2);

        val = (val <= 0 || val>100) ? 100 : val;

        txtPercent.Text = val.ToString();
    }

    protected void ddPercents_TextChanged(object sender, EventArgs e)
    {
        if (ddPercents.SelectedIndex>0)
        {
            txtPercent.Text = ddPercents.Text;
            ddPercents.SelectedIndex = 0;
        }
        
    }
}
