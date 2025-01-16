using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PreLoad_AddWeights : System.Web.UI.Page
{


    public bool WeighIn
    {
        get
        {
            bool val = false;
            if (Request.QueryString["WeighIn"]== null )
            {
                Response.Redirect("ticketCreator.aspx");
            }
            else
            {
               
               

                    if (!bool.TryParse(Request.QueryString["WeighIn"], out val))
                    {
                        Response.Redirect("ticketCreator.aspx");
                    }
               

            }
            return val;
        }
        
    }


    public bool NewLoad
    {
        get
        {
            bool val = false;
            if (Request.QueryString["NewLoad"] == null)
            {
                Response.Redirect("ticketCreator.aspx");
            }
            else
            {
                if (!bool.TryParse(Request.QueryString["NewLoad"], out val))
                {
                    Response.Redirect("ticketCreator.aspx");
                }
            }
            return val;
        }

    }


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            UpdateScales();
            if (!this.IsPostBack)
            {
                if (!SeedTicketInfo.CurrentSeedTicketRow.IsTruck_IDNull()) this.txtVehicle.Text = SeedTicketInfo.CurrentSeedTicketRow.Truck_ID;
                if (SeedTicketInfo.TicketWeighType == GlobalVars.enumSeedTicketWeighType.Truck || SeedTicketInfo.TicketWeighType == GlobalVars.enumSeedTicketWeighType.ReturnBulk )
                {
                    SetForTruckWeight();

                }
                else if(SeedTicketInfo.TicketWeighType== GlobalVars.enumSeedTicketWeighType.Tote || SeedTicketInfo.TicketWeighType == GlobalVars.enumSeedTicketWeighType.ReturnTote )
                {
                    SetForToteWeight();
                }
            }
        }
        catch (Exception ex)
        {
            Auditing.LogMessage("AddWeights.PageLoad", ex.Message);
            Response.Redirect("ticketCreator.aspx");
        }

    }


    public void SetForToteWeight()
    {
        if (SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Weights.Count == 0)
        {


            lblHeader.Text = "Capture Tote Starting Weight";
        }
        else
        {
            var row = SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Weights[0];
            if (WeighIn)
            {
                if (row.IsStarting_WeightNull())
                {
                    lblHeader.Text = "Capture Tote Starting Weight";
                }
                else
                {
                    lblHeader.Text = $"Edit Tote In Weight (Current={row.Starting_Weight})";
                }

            }
            else
            {
                if (row.IsEnding_WeightNull())
                {
                    lblHeader.Text = "Capture Tote Final Weight";
                }
                else
                {
                    lblHeader.Text = $"Edit Final Weight (Current={row.Ending_Weight })";
                }

            }
        }
    }


    public void SetForTruckWeight()
    {
        RemoveAllButCurrentSeedTicketWeight();

        if (SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Weights.Count == 0)
        {


            lblHeader.Text = "Weigh Truck In";
        }
        else
        {
            var row = SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Weights[0];
            if (WeighIn)
            {
                if (row.IsStarting_WeightNull())
                {
                    lblHeader.Text = "Weigh Truck In";
                }
                else
                {
                    lblHeader.Text = $"Edit Truck In Weight (Current={row.Starting_Weight})";
                }

            }
            else
            {
                if (row.IsEnding_WeightNull())
                {
                    lblHeader.Text = "Weigh Truck Out";
                }
                else
                {
                    lblHeader.Text = $"Edit Truck Out Weight (Current={row.Ending_Weight })";
                }

            }
        }


    }

    public void RemoveAllButCurrentSeedTicketWeight()
    {
        List<Guid> WeightUID = new List<Guid>();
        foreach (SeedTicketDataSet.Seed_Ticket_WeightsRow row in SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Weights)
        {
            if (row.UID != SeedTicketInfo.CurrentSeedTicketWeightRow.UID) WeightUID.Add(row.UID);
        }
        foreach (var UID in WeightUID)
        {
            SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Weights.FindByUID(UID).Delete();
        }
    }

    public GlobalVars.enumGetWeighType WeighType
    {
        get
        {
            return (GlobalVars.enumGetWeighType)Enum.Parse(typeof(GlobalVars.enumGetWeighType), SeedTicketInfo.CurrentSeedTicketRow.Ticket_Type);
        }
    }


  

        
                


    public void UpdateScales()
    {
        using (ScaleWebService.LocalWebService proxy = new ScaleWebService.LocalWebService())
        {
            ScaleWebService.LocalDataSet.Weigh_ScalesDataTable ScalesDataTable = proxy.GetScales();
            var CurrentScales = new ScaleDataSet.ScalesDataTable();
            ScalesDataTable.DefaultView.Sort = "Description Asc ";


            int index = 1;
            foreach (DataRowView sRow in ScalesDataTable.DefaultView) // ScaleWebService.LocalDataSet.Weigh_ScalesRow  scaleRow in ScalesDataTable )
            {
                var newScale = CurrentScales.NewScalesRow();

                if ((bool)sRow["OK"])
                {
                    if ((bool)sRow["Motion"])
                    {
                        newScale.Status = "Motion";
                        newScale.ReadOnly = true;
                        newScale.RowCssClass = "form-control text-right alert-warning";
                    }
                    else
                    {
                        if ((int)sRow["Weight"] > 500)
                        {
                            newScale.RowCssClass = "form-control text-right alert-success";
                        }
                        else
                        {
                            newScale.RowCssClass = "form-control text-right ";
                        }
                        newScale.Status = "";
                        newScale.ReadOnly = false;

                    }
                }
                else
                {
                    newScale.ReadOnly = true;
                    newScale.Status = (string)sRow["Error_Message"];
                    newScale.RowCssClass = "form-control text-right alert-danger";
                }
                newScale.Description = (string)sRow["Description"];
                newScale.Index = index;
                newScale.Motion = (bool)sRow["Motion"];


                newScale.Valid = (bool)sRow["OK"];
                newScale.Weight = (int)sRow["Weight"];
                CurrentScales.AddScalesRow(newScale);
                index++;
            }
            grdScales.DataSource = CurrentScales;
            grdScales.DataBind();
            UPScaleWeights.Update();
        }
    }


    protected void btnSelect_Click(object sender, EventArgs e)
    {

        LinkButton btn = (LinkButton)sender;
        GridViewRow row = (GridViewRow)btn.NamingContainer;
        int index = row.RowIndex;
        Label txtWeight = (Label)row.FindControl("txtWeight");
        int Weight;
        int.TryParse(txtWeight.Text, out Weight);
        string ScaleDescription =grdScales.DataKeys[index].Values[0].ToString();
        SetWeight(Weight,ScaleDescription, false);

    }

    public void SetWeight(int Weight,string ScaleDescription, bool Manual)
    {

       // GlobalVars.enumGetWeighType WeighType = (GlobalVars.enumGetWeighType)Enum.Parse(typeof(GlobalVars.enumGetWeighType),SeedTicketInfo.CurrentSeedTicketRow.Ticket_Type );

        if (SeedTicketInfo.CurrentSeedTicketWeightRow == null || NewLoad ) SeedTicketInfo.CreateSeedTicketWeightsRow();


        if (WeighIn)
        {
            if (!SeedTicketInfo.CurrentSeedTicketWeightRow.IsStarting_WeightNull())
            {
                SeedTicketInfo.AddToAudit(SeedTicketInfo.enumAuditType.Update, SeedTicketInfo.CurrentSeedTicketWeightRow.UID, $"Changed Inbound Weight Original Time={SeedTicketInfo.CurrentSeedTicketWeightRow.Starting_Time.ToString()} Originally Manual ={SeedTicketInfo.CurrentSeedTicketWeightRow.Manual_In} ", Weight.ToString(), SeedTicketInfo.CurrentSeedTicketWeightRow.Starting_Weight.ToString());
            }
            SeedTicketInfo.CurrentSeedTicketWeightRow.Inbound_Scale = ScaleDescription;
            SeedTicketInfo.CurrentSeedTicketWeightRow.Starting_Time = DateTime.Now;
            SeedTicketInfo.CurrentSeedTicketWeightRow.Starting_Weight = Weight;
            SeedTicketInfo.CurrentSeedTicketWeightRow.Manual_In = Manual;
            SeedTicketInfo.CurrentSeedTicketWeightRow.PC_Weight_In_Address = Request.UserHostAddress;


            if (SeedTicketInfo.TicketWeighType == GlobalVars.enumSeedTicketWeighType.Tote)
            {
                AddTote();
            }
            
        }
        else
        {
            if (!SeedTicketInfo.CurrentSeedTicketWeightRow.IsEnding_WeightNull())
            {
                SeedTicketInfo.AddToAudit(SeedTicketInfo.enumAuditType.Update, SeedTicketInfo.CurrentSeedTicketWeightRow.UID, $"Changed Outbound Weight Original Time={SeedTicketInfo.CurrentSeedTicketWeightRow.Ending_Time.ToString()}  Originally Manual ={SeedTicketInfo.CurrentSeedTicketWeightRow.Manual_Out} ", Weight.ToString(), SeedTicketInfo.CurrentSeedTicketWeightRow.Ending_Weight.ToString());
            }
            SeedTicketInfo.CurrentSeedTicketWeightRow.Outbound_Scale  = ScaleDescription;

            SeedTicketInfo.CurrentSeedTicketWeightRow.Ending_Time = DateTime.Now;
            SeedTicketInfo.CurrentSeedTicketWeightRow.Ending_Weight = Weight;
            SeedTicketInfo.CurrentSeedTicketWeightRow.Manual_Out = Manual;
            SeedTicketInfo.CurrentSeedTicketWeightRow.PC_Weight_Out_Address = Request.UserHostAddress;


        
        }
        SeedTicketInfo.SaveTicket();


        Response.Redirect("TicketCreator");
       
    }


    public void AddTote()
    {
        using (ListDataSetTableAdapters.Misc_ItemsTableAdapter misc_ItemsTableAdapter = new ListDataSetTableAdapters.Misc_ItemsTableAdapter())
        {
            using (ListDataSet.Misc_ItemsDataTable misc_ItemsDataTable = new ListDataSet.Misc_ItemsDataTable())
            {
                misc_ItemsTableAdapter.Fill(misc_ItemsDataTable, GlobalVars.Location);
                ListDataSet.Misc_ItemsRow Lrow = misc_ItemsDataTable.FirstOrDefault(x => x.Description.ToUpper() == "TOTES");
                if (Lrow != null)
                {
                    if (SeedTicketInfo.CurrentSeedTicketRow != null)
                    {
                        bool NotFound;
                        int ID = Lrow.ID;
                        int Quantity = 1;
                        SeedTicketDataSet.Seed_Ticket_MiscRow row = SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Misc.FirstOrDefault(x => x.Item_Id == ID);
                        NotFound = (row == null);
                        if (NotFound)
                        {
                            row = SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Misc.NewSeed_Ticket_MiscRow();
                            row.UID = Guid.NewGuid();
                            row.Seed_Ticket_UID = SeedTicketInfo.CurrentSeedTicketRow.UID;
                            row.Item_Id = ID;
                            row.Description = Lrow.Description;
                            row.Quantity = 0;
                        }
                        row.Quantity += Quantity;
                        row.Comment = "";
                        row.Hidden = false;
                        if (NotFound) SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Misc.AddSeed_Ticket_MiscRow(row);

                    }
                }
            }
        }
    }


    protected void btnManualSelect_Click(object sender, EventArgs e)
    {
        int Weight;
        int.TryParse(txtManualWeight.Text.Trim(), out Weight);
        SetWeight(Weight,"Manual", true);
    }



    protected void txtVehicle_TextChanged(object sender, EventArgs e)
    {
        if (!SeedTicketInfo.CurrentSeedTicketRow.IsTruck_IDNull()) SeedTicketInfo.AddToAudit(SeedTicketInfo.enumAuditType.Create, SeedTicketInfo.CurrentSeedTicketRow.UID, txtVehicle.Text, SeedTicketInfo.CurrentSeedTicketRow.Truck_ID);
        SeedTicketInfo.CurrentSeedTicketRow.Truck_ID = txtVehicle.Text;
    }

    protected void txtManualWeight_TextChanged(object sender, EventArgs e)
    {
        int Weight;
        int.TryParse(txtManualWeight.Text.Trim(), out Weight);
        SetWeight(Weight, "Manual", true);
    }
}