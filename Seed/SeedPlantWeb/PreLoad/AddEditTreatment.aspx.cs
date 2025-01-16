using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AddEditTreatment : System.Web.UI.Page
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
            else if ((SeedTicketInfo.CurrentSeedTicketTreatmentRow == null))
            {
                Response.Redirect("~/PreLoad/TicketCreator.aspx");
            }

            if (!SeedTicketInfo.CurrentSeedTicketTreatmentRow.IsTreatment_IDNull() && !SeedTicketInfo.CurrentSeedTicketTreatmentRow.IsCustom_NameNull())
            {
                txtFilter.Text = $"{ SeedTicketInfo.CurrentSeedTicketTreatmentRow.Custom_Name} - { SeedTicketInfo.CurrentSeedTicketTreatmentRow.Treatment_ID}";
            }

            Session["OriginalTreatmentValues"] = SeedTicketInfo.CurrentSeedTicketTreatmentRow ; 
            string PLCTreatments = string.Empty;
            using (ListDataSet.SeedChemicalsDataTable seedChemicalsDataTable = new ListDataSet.SeedChemicalsDataTable())
            {
                using (ListDataSetTableAdapters.SeedChemicalsTableAdapter seedChemicalsTableAdapter = new ListDataSetTableAdapters.SeedChemicalsTableAdapter())
                {
                    seedChemicalsTableAdapter.FillByAllForLocation(seedChemicalsDataTable, GlobalVars.Location);
                    foreach(var item in PLC.plcDataset.Treatments )
                    {
                        var row = seedChemicalsDataTable.FirstOrDefault(x => x.Description.ToUpper() == item.Description.ToUpper());
                        if (row != null)
                        {
                            PLCTreatments += (string.IsNullOrEmpty(PLCTreatments) ? row.ID.ToString() : "," + row.ID.ToString());
                        }
                    }
                }

            }
            hfPLCTreatments.Value = PLCTreatments;


            ddPLCOnly.DataBind();
            if (!PLC.PLCConnected) ddPLCOnly.Items.RemoveAt(0);

            ddPLCOnly.SelectedIndex = 0;

            string UsedItems = string.Empty;
            foreach (var item in SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Treatments )
            {
                if (AddNew )
                {
                    UsedItems = (!string.IsNullOrEmpty(UsedItems)) ? item.Treatment_ID.ToString() : "," + item.Treatment_ID.ToString();
                }
            }
            hfExistingItems.Value = UsedItems;
         
        }

        Update_Page();
        if (!this.IsPostBack)
        {
          
            if (!AddNew)
            {
                btnOk.Visible = true;
                txtRate.Text = (SeedTicketInfo.CurrentSeedTicketTreatmentRow.Rate).ToString(); 
            }
            txtFilter.Focus();
        }
        

    }


    private void Update_Page()
    {
        grd1.DataBind();

        if (grd1.Rows.Count == 1)
        {
            var row = grd1.Rows[0];
            string TreatmentId = grd1.DataKeys[row.RowIndex].Values[1].ToString();
            txtFilter.Text = grd1.DataKeys[row.RowIndex].Values[3].ToString();

            int ID = int.Parse(grd1.DataKeys[row.RowIndex].Values[1].ToString());
            string Description = grd1.DataKeys[row.RowIndex].Values[2].ToString();
            decimal DefaultValue=decimal.Parse( grd1.DataKeys[row.RowIndex].Values[4].ToString());
            hfDefaultRate.Value = DefaultValue.ToString();
            if (SeedTicketInfo.CurrentSeedTicketTreatmentRow != null)
            {
                SeedTicketInfo.CurrentSeedTicketTreatmentRow.Treatment_ID = ID;
                SeedTicketInfo.CurrentSeedTicketTreatmentRow.Custom_Name = Description;
            }
            if (hfFilter.Value != txtFilter.Text) txtRate.Text = hfDefaultRate.Value;
            hfFilter.Value = txtFilter.Text;
          //  btnSetDefault.Visible = txtRate.Text != hfDefaultRate.Value;
        }

        pnlRate.Visible = (grd1.Rows.Count == 1);
        grd1.Visible = grd1.Rows.Count > 1;

        btnOk.Visible = (grd1.Rows.Count == 1  && CurrentRate>0 );
    }



private float CurrentRate
    {
        get
        {

            float Rate=0;
            float.TryParse(txtRate.Text, out Rate);
            return Rate;
        }
    }  



    protected void lnkSelect_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        GridViewRow row = (GridViewRow)btn.NamingContainer;
        int index = row.RowIndex;
        Guid UID = Guid.Parse(grd1.DataKeys[index].Values[0].ToString());
     
        txtFilter.Text = grd1.DataKeys[index].Values[3].ToString();
        grd1.DataBind();
        Update_Page();
       
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
      
        Response.Redirect("TicketCreator.aspx");
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["AddNew"] != null)
        {
            if (SeedTicketInfo.CurrentSeedTicketTreatmentRow != null)
            {
               
                SeedTicketInfo.CurrentSeedTicketTreatmentRow.Rate  = CurrentRate ;
                if (AddNew)
                {
                    SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Treatments.AddSeed_Ticket_TreatmentsRow(SeedTicketInfo.CurrentSeedTicketTreatmentRow);
                }
                else
                {
                    if (Session["OriginalTreatmentValues"]!= null)
                    {
                        try
                        {


                    var OriginalRow = (SeedTicketDataSet.Seed_Ticket_TreatmentsRow)Session["OriginalTreatmentValues"];
                    var crow = SeedTicketInfo.CurrentSeedTicketTreatmentRow;
                    if (crow.UID== OriginalRow.UID )
                    {
                        if (crow.PC_Address != OriginalRow.PC_Address) SeedTicketInfo.AddToAudit(SeedTicketInfo.enumAuditType.Update, crow.UID, "Changed Treatment PC_Address", crow.PC_Address, OriginalRow.PC_Address);
                        if (!crow.IsTreatment_IDNull() && !OriginalRow.IsTreatment_IDNull() && crow.Treatment_ID != OriginalRow.Treatment_ID) SeedTicketInfo.AddToAudit(SeedTicketInfo.enumAuditType.Update, crow.UID, "Changed Treatment_ID", crow.Treatment_ID.ToString(), OriginalRow.Treatment_ID.ToString());
                        if (!crow.IsCustom_NameNull() && !OriginalRow.IsCustom_NameNull() && crow.Custom_Name != OriginalRow.Custom_Name) SeedTicketInfo.AddToAudit(SeedTicketInfo.enumAuditType.Update, crow.UID, "Changed Treatment Custom Name", crow.Custom_Name, OriginalRow.Custom_Name);
                        if (!crow.IsRateNull() && !OriginalRow.IsRateNull() && crow.Rate != OriginalRow.Rate) SeedTicketInfo.AddToAudit(SeedTicketInfo.enumAuditType.Update, crow.UID, "Changed Rate", crow.Rate.ToString(), OriginalRow.Rate.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            Auditing.LogMessage("AddEditTreatment.btnOK", ex.Message);
                        }
                    }
                }
            }

        }
        Response.Redirect("TicketCreator.aspx");
        
    }

  

    protected void ddPLCOnly_TextChanged(object sender, EventArgs e)
    {
        ResetTreatment();
    }

  

    private void ResetTreatment()
    {
        txtFilter.Text = "";
        Update_Page();
    }



    protected void txtFilter_TextChanged(object sender, EventArgs e)
    {

        hfFilter.Value = txtFilter.Text; 
    }

    protected void txtRate_TextChanged(object sender, EventArgs e)
    {
        decimal val =decimal.Parse( hfDefaultRate.Value) ;
        if (!decimal.TryParse(txtRate.Text, out val))
        {
            val = decimal.Parse(hfDefaultRate.Value);
        }

        //val = Math.Round((double)val, 2);

        val = (val <= 0 || val>50) ? decimal.Parse(hfDefaultRate.Value)  : val;

        txtRate.Text = val.ToString();
        btnOk.Visible = (grd1.Rows.Count == 1 && CurrentRate > 0);
    }

    protected void ddrates_TextChanged(object sender, EventArgs e)
    {
        if (ddrates.SelectedIndex>0)
        {
            txtRate.Text = ddrates.Text;
            ddrates.SelectedIndex = 0;
        }
        btnOk.Visible = (grd1.Rows.Count == 1 && CurrentRate > 0);

    }

    protected void btnSetDefault_Click(object sender, EventArgs e)
    {
        if (SeedTicketInfo.CurrentSeedTicketTreatmentRow != null)
        {
          if (!SeedTicketInfo.CurrentSeedTicketTreatmentRow.IsTreatment_IDNull())
            {
                using (ListDataSetTableAdapters.Item_LocationTableAdapter item_LocationTableAdapter = new ListDataSetTableAdapters.Item_LocationTableAdapter())
                {
                    using (ListDataSet.Item_LocationDataTable item_LocationDataTable = new ListDataSet.Item_LocationDataTable())
                    {
                        if (item_LocationTableAdapter.FillByIdLocation(item_LocationDataTable,SeedTicketInfo.CurrentSeedTicketTreatmentRow.Treatment_ID,GlobalVars.Location  )>0)
                        {
                            decimal NewDefault;
                            if (decimal.TryParse(txtRate.Text,out NewDefault ))
                            {
                                item_LocationDataTable[0].DefaultValue = NewDefault ;
                                item_LocationTableAdapter.Update(item_LocationDataTable);
                            }
                            
                        }
                    }
                }
            }
            
        }
    }

    protected void btnNotFound_Click(object sender, EventArgs e)
    {
        Response.Redirect($"UnknownTreatment?AddNew={AddNew.ToString()}&UnknownTreatment={txtFilter.Text}");
    }
}
