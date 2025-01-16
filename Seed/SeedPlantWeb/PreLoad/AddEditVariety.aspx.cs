using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AddEditVariety : System.Web.UI.Page
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
                    Response.Redirect("../Default.aspx");
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
                txtFilter.Text = $"{ SeedTicketInfo.CurrentSeedTicketVarietyRow.Custom_Name} - { SeedTicketInfo.CurrentSeedTicketVarietyRow.Variety_ID}";
            }

              Session["OriginalVarietyValues"] = SeedTicketInfo.CurrentSeedTicketVarietyRow;

            ddBinsOnly.DataBind();
            if (!PLC.PLCConnected) ddBinsOnly.Items.RemoveAt(0);

            ddBinsOnly.SelectedIndex = 0;

            string UsedItems = string.Empty;
            foreach (var item in SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Varieties )
            {
                if (AddNew )
                {
                    UsedItems = (!string.IsNullOrEmpty(UsedItems)) ? item.Variety_ID.ToString() : "," + item.Variety_ID.ToString();
                }
            }
            hfExistingItems.Value = UsedItems; 
          

        }

        Update_Page();
        if (!this.IsPostBack)
        {
            UpdateBins();
            if (!SeedTicketInfo.CurrentSeedTicketVarietyRow.IsBin_NameNull() )
            {
                ddBins.Visible = true;
                ddBins.ClearSelection();
                ListItem LI = ddBins.Items.FindByText(SeedTicketInfo.CurrentSeedTicketVarietyRow.Bin_Name);
                if (LI == null)
                {
                    LI = new ListItem();
                    LI.Value = SeedTicketInfo.CurrentSeedTicketVarietyRow.Bin.ToString();

                    LI.Text = SeedTicketInfo.CurrentSeedTicketVarietyRow.Bin_Name;
                    ddBins.Items.Add(LI);
                }
                LI.Selected = true;
                ddBins.Enabled = !SeedTicketInfo.TicketComplete ; 
                
            }
            if (!AddNew)
            {
                btnOk.Visible = true;
                txtPercent.Text = ((int)SeedTicketInfo.CurrentSeedTicketVarietyRow.Percent_Of_Load).ToString(); 
            }
            txtFilter.Focus();
        }
       
        

    }


    private void Update_Page()
    {
        string Bins = string.Empty;
        foreach (var item in PLC.plcDataset.Bins)
        {
            Bins += ((string.IsNullOrEmpty(Bins)) ? item.Variety_Id : "," + item.Variety_Id);
        }
        hfBins.Value = Bins;
        grd1.DataBind();

        if (grd1.Rows.Count == 1)
        {
            var row = grd1.Rows[0];
            string VarietyId = grd1.DataKeys[row.RowIndex].Values[1].ToString();
            txtFilter.Text = grd1.DataKeys[row.RowIndex].Values[3].ToString();

            int ID = int.Parse(grd1.DataKeys[row.RowIndex].Values[1].ToString());
            string Description = grd1.DataKeys[row.RowIndex].Values[2].ToString();

            if (SeedTicketInfo.CurrentSeedTicketVarietyRow != null)
            {
                SeedTicketInfo.CurrentSeedTicketVarietyRow.Variety_ID = ID;
                SeedTicketInfo.CurrentSeedTicketVarietyRow.Custom_Name = Description;
            }
            if (hfFilter.Value.ToString() != txtFilter.Text) UpdateBins();
            hfFilter.Value = txtFilter.Text;
        }
        else
        {
            UpdateBins();
        }

        bool BinSelected = ddBins.Visible;
        if (ddBins.Visible && ddBins.Items.Count > 1 && ddBins.SelectedIndex < 1) BinSelected = false;
       
        grd1.Visible = grd1.Rows.Count > 1;
        pnlPrecent.Visible = grd1.Rows.Count == 1;
        btnOk.Visible = (grd1.Rows.Count == 1 && ((ddBins.Visible & BinSelected) || !ddBins.Visible )  );
    }


   private void UpdateBins()
    {
        if (grd1.Rows.Count == 1)
        {
            var row = grd1.Rows[0];
            string VarietyId = grd1.DataKeys[row.RowIndex].Values[1].ToString();
            ddBins.Items.Clear();
            foreach (var item in PLC.plcDataset.Bins)
            {
                if (item.Variety_Id == VarietyId)
                {
                    ddBins.Items.Add(item.Bin_Name.ToString());
                }
            }

        }
        if (ddBins.Items.Count > 1)
        {
            ListItem LI = new ListItem();
            LI.Text = "Select Bin";
            ddBins.Items.Insert(0, LI);
        }

        pnlUseBins.Visible = (PLC.PLCConnected);
        ddBins.Visible = (PLC.PLCConnected && ddBins.Items.Count > 0 && grd1.Rows.Count == 1);
        lblNotInBin.Visible = (PLC.PLCConnected && ddBins.Items.Count == 0 && grd1.Rows.Count == 1);
        if (ddBins.Items.Count > 0) ddBins.SelectedIndex = 0;
       
    }



    protected void lnkSelect_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        GridViewRow row = (GridViewRow)btn.NamingContainer;
        int index = row.RowIndex;
        Guid UID = Guid.Parse(grd1.DataKeys[index].Values[0].ToString());
        //int ID = int.Parse(grd1.DataKeys[index].Values[1].ToString());
        //string Description = grd1.DataKeys[index].Values[2].ToString();

        //if (SeedTicketInfo.CurrentSeedTicketVarietyRow!= null )
        //{
        //    SeedTicketInfo.CurrentSeedTicketVarietyRow.Variety_ID = ID;
        //    SeedTicketInfo.CurrentSeedTicketVarietyRow.Custom_Name = Description;
        //}
        txtFilter.Text = grd1.DataKeys[index].Values[3].ToString();
        grd1.DataBind();
        Update_Page();
        //Response.Redirect(Request.RawUrl);
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
      
        Response.Redirect("TicketCreator.aspx");
    }

  
    

    

    protected void btnOk_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["AddNew"] != null)
            {
                if (SeedTicketInfo.CurrentSeedTicketVarietyRow != null)
                {
                    float PercentOfLoad;
                    float.TryParse(txtPercent.Text, out PercentOfLoad);
                    PLCDataSet.BinsRow row = PLC.plcDataset.Bins.FirstOrDefault(x => x.Bin_Name == ddBins.Text);
                    if (row != null)
                    {
                        SeedTicketInfo.CurrentSeedTicketVarietyRow.Bin = row.Bin_Id;
                        SeedTicketInfo.CurrentSeedTicketVarietyRow.Bin_Name = row.Bin_Name;
                    }



                    SeedTicketInfo.CurrentSeedTicketVarietyRow.Percent_Of_Load = PercentOfLoad;
                    if (AddNew)
                    {
                        string Lot = Lots.GetLot(SeedTicketInfo.CurrentSeedTicketVarietyRow.Variety_ID);
                        if (!string.IsNullOrEmpty(Lot)) SeedTicketInfo.CurrentSeedTicketVarietyRow.Lot = Lot;
                        SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Varieties.AddSeed_Ticket_VarietiesRow(SeedTicketInfo.CurrentSeedTicketVarietyRow);
                        SeedTicketInfo.AddToAudit(SeedTicketInfo.enumAuditType.Create, SeedTicketInfo.CurrentSeedTicketRow.UID, "Seed Ticket Variety Added");
                    }
                    else
                    {
                        if (Session["OriginalVarietyValues"] != null)
                        {
                            try
                            {
                                var OriginalRow = (SeedTicketDataSet.Seed_Ticket_VarietiesRow)Session["OriginalVarietyValues"];
                                if (OriginalRow.UID == SeedTicketInfo.CurrentSeedTicketVarietyRow.UID)
                                {
                                    var crow = SeedTicketInfo.CurrentSeedTicketVarietyRow;
                                    if (crow.PC_Address != OriginalRow.PC_Address) SeedTicketInfo.AddToAudit(SeedTicketInfo.enumAuditType.Update, crow.UID, "Changed Treatment PC Address", crow.PC_Address, OriginalRow.PC_Address);
                                    if (!crow.IsVariety_IDNull() && !OriginalRow.IsVariety_IDNull() && crow.Variety_ID != OriginalRow.Variety_ID) SeedTicketInfo.AddToAudit(SeedTicketInfo.enumAuditType.Update, crow.UID, "Changed Variety_ID", crow.Variety_ID.ToString(), OriginalRow.Variety_ID.ToString());
                                    if (!crow.IsPercent_Of_LoadNull() && !OriginalRow.IsPercent_Of_LoadNull() && crow.Percent_Of_Load != OriginalRow.Percent_Of_Load) SeedTicketInfo.AddToAudit(SeedTicketInfo.enumAuditType.Update, crow.UID, "Changed Percent Of Load", crow.Percent_Of_Load.ToString(), OriginalRow.Percent_Of_Load.ToString());
                                    if (!crow.IsCustom_NameNull() && !OriginalRow.IsCustom_NameNull() && crow.Custom_Name != OriginalRow.Custom_Name) SeedTicketInfo.AddToAudit(SeedTicketInfo.enumAuditType.Update, crow.UID, "Changed Variety Custom Name", crow.Custom_Name, OriginalRow.Custom_Name);
                                    if (!crow.IsLotNull() && !OriginalRow.IsLotNull() && crow.Lot != OriginalRow.Lot) SeedTicketInfo.AddToAudit(SeedTicketInfo.enumAuditType.Update, crow.UID, "Changed Lot", crow.Lot, OriginalRow.Lot);
                                    if (!crow.IsBinNull() && !OriginalRow.IsBinNull() && crow.Bin != OriginalRow.Bin) SeedTicketInfo.AddToAudit(SeedTicketInfo.enumAuditType.Update, crow.UID, "Changed Bin", crow.Bin.ToString(), OriginalRow.Bin.ToString());
                                    if (!crow.IsBin_NameNull() && !OriginalRow.IsBin_NameNull() && crow.Bin_Name != OriginalRow.Bin_Name) SeedTicketInfo.AddToAudit(SeedTicketInfo.enumAuditType.Update, crow.UID, "Changed Bin_Name", crow.Bin_Name, OriginalRow.Bin_Name);
                                }
                            }
                            catch (Exception ex)
                            {
                                Auditing.LogMessage("AddEditVariety.btnOK", ex.Message);
                            }
                        }
                    }
                }

            }
            SeedTicketInfo.CleanUpCurrentPercents();
            Response.Redirect("TicketCreator.aspx");
        }
        catch (Exception ex)
        {
            if (!ex.Message.Contains( "Thread was being aborted")) Auditing.LogMessage("AddEditVariety.btnOK", ex.Message);
        }
    }

    protected void ddClass_TextChanged(object sender, EventArgs e)
    {
        ResetVariety();
    }

    protected void ddBinsOnly_TextChanged(object sender, EventArgs e)
    {
        ResetVariety();
    }

    protected void ddCommodity_TextChanged(object sender, EventArgs e)
    {
        ResetVariety();
    }


    private void ResetVariety()
    {
        txtFilter.Text = "";
        Update_Page();
    }



    protected void txtFilter_TextChanged(object sender, EventArgs e)
    {

        hfFilter.Value = txtFilter.Text; 
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

    protected void btnNotFound_Click(object sender, EventArgs e)
    {
        Response.Redirect($"UnknownVariety?AddNew={AddNew.ToString()}&unknownVariety={txtFilter.Text}");
    }
}
