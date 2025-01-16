using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UnknownTreatment : System.Web.UI.Page
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
                txtFilter.Text = $"{ SeedTicketInfo.CurrentSeedTicketTreatmentRow.Custom_Name}";
            }
            else
            {
                txtFilter.Text = (Request.QueryString["UnknownTreatment"] != null) ? Request.QueryString["UnknownTreatment"] : "";
            }

            Session["OriginalTreatmentValues"] = SeedTicketInfo.CurrentSeedTicketTreatmentRow; 
       
          
            if (!AddNew)
            {
                btnOk.Visible = true;
                txtRate.Text = (SeedTicketInfo.CurrentSeedTicketTreatmentRow.Rate).ToString(); 
            }

        }
        btnOk.Visible = (txtFilter.Text.Length > 1 && CurrentRate > 0);

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
                SeedTicketInfo.CurrentSeedTicketTreatmentRow.Custom_Name = txtFilter.Text; 
                SeedTicketInfo.CurrentSeedTicketTreatmentRow.Rate  = CurrentRate ;
                if (AddNew)
                {

                    int Seed = -1000;
                    SeedTicketDataSet.Seed_Ticket_TreatmentsRow  FoundRow = null;
                    do
                    {
                        FoundRow = SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Treatments.FirstOrDefault(x => x.Treatment_ID  == Seed);
                        Seed -= 1;

                    }
                    while (FoundRow != null);

                    SeedTicketInfo.CurrentSeedTicketTreatmentRow.Treatment_ID  = Seed;
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
                            Auditing.LogMessage("UnknownTreatment.btnOK", ex.Message);
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
      
    }



    protected void txtFilter_TextChanged(object sender, EventArgs e)
    {

      
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
        btnOk.Visible = (txtFilter.Text.Length > 1 && CurrentRate > 0);
    }

    protected void ddrates_TextChanged(object sender, EventArgs e)
    {
        if (ddrates.SelectedIndex>0)
        {
            txtRate.Text = ddrates.Text;
            ddrates.SelectedIndex = 0;
        }
        btnOk.Visible = (txtFilter.Text.Length>1 && CurrentRate > 0);

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
}
