using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TicketCreator : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                if (Session["Weighmaster"] == null) Session["Weighmaster"] = "";
                this.txtWeighmaster.Text = Session["Weighmaster"].ToString();

                this.grdVarieties.Columns[2].Visible = PLC.PLCConnected;
                ReportDataSet reportDataset = SeedTicketInfo.GetAllTicketInfo(SeedTicketInfo.seedTicketDataSet, SeedTicketInfo.CurrentSeedTicketRow.UID);
                SetVarieties(reportDataset);
                SetTreatments(reportDataset);



                if (!SeedTicketInfo.CurrentSeedTicketRow.IsWeighmasterNull()) this.txtWeighmaster.Text = SeedTicketInfo.CurrentSeedTicketRow.Weighmaster;
                if (!SeedTicketInfo.CurrentSeedTicketRow.IsPONull()) this.txtPO.Text = SeedTicketInfo.CurrentSeedTicketRow.PO;
                if (!SeedTicketInfo.CurrentSeedTicketRow.IsBOLNull()) this.txtBOL.Text = SeedTicketInfo.CurrentSeedTicketRow.BOL;
                if (!SeedTicketInfo.CurrentSeedTicketRow.IsCommentsNull()) this.txtComment.Text = SeedTicketInfo.CurrentSeedTicketRow.Comments;
                if (!SeedTicketInfo.CurrentSeedTicketRow.IsTruck_IDNull()) this.txtVehicle.Text = SeedTicketInfo.CurrentSeedTicketRow.Truck_ID;
                lblHeader.Text = (SeedTicketInfo.CurrentSeedTicketRow.IsTicketNull()) ? "New Ticket" : $"Ticket {SeedTicketInfo.CurrentSeedTicketRow.Ticket }";
                ddPrintDestination.DataBind();
                ddPrintDestination.SelectedIndex = (GlobalVars.UsePrinter) ? 0 : 1;
                ddWeightype.Items.Clear();
                ckPending.Checked = SeedTicketInfo.CurrentSeedTicketRow.Pending;
                ckPending.Visible = SeedTicketInfo.CurrentSeedTicketRow.IsTicketNull();
                var Weightypes = Enum.GetValues(typeof(GlobalVars.enumSeedTicketWeighType));
                foreach (GlobalVars.enumSeedTicketWeighType val in Weightypes)
                {
                    ddWeightype.Items.Add(val.ToString());
                }
                ddWeightype.Items.FindByText(SeedTicketInfo.CurrentSeedTicketRow.Ticket_Type).Selected = true;
                SetMisc();
                SetWeights();
                ddBagSizes.DataBind();
                ddBagSizes.SelectedIndex = 0;
            }
            if (SeedTicketInfo.seedTicketDataSet == null || SeedTicketInfo.CurrentSeedTicketRow == null || SeedTicketInfo.CurrentSeedTicketRow.IsGrower_IDNull())
            {
                Response.Redirect("SelectGrower.aspx");
            }
            else
            {
                string ID = (SeedTicketInfo.CurrentSeedTicketRow.Grower_ID > 0) ? SeedTicketInfo.CurrentSeedTicketRow.Grower_ID.ToString() : "UNKNOWN";
                lnkGrower.Text = $"{SeedTicketInfo.CurrentSeedTicketRow.Grower_Name} - {ID}";


            }
            SetButtons();


            var CommentRows = txtComment.Text.Split((char)10);
            txtComment.Rows = (CommentRows.Length > 1) ? CommentRows.Length : 1;

        }
        catch (Exception ex)
        {
            Auditing.LogMessage("TicketCreator.Page_Load", ex.ToString());
            Response.Redirect("../Default.aspx");
        }
    }

    public void SetButtons()
    {


        bool ReadOnly = SeedTicketInfo.CurrentSeedTicketRow.ReadOnly;
        if (SeedTicketInfo.TicketWeighType == GlobalVars.enumSeedTicketWeighType.Truck)
        {
            btnWeight.Visible = (SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Weights.Count == 0 && !ReadOnly);
        }
        else if (SeedTicketInfo.TicketWeighType == GlobalVars.enumSeedTicketWeighType.Tote)
        {
            btnWeight.Visible = ((WeightsComplete() || SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Weights.Count == 0) && !ReadOnly);
        }
        else if (SeedTicketInfo.TicketWeighType == GlobalVars.enumSeedTicketWeighType.Bag || SeedTicketInfo.TicketWeighType == GlobalVars.enumSeedTicketWeighType.ReturnBag)
        {
            btnWeight.Visible = false;
        }
        else

        {
            btnWeight.Visible = !ReadOnly;
        }

        if (SeedTicketInfo.CurrentSeedTicketRow.IsTicketNull())
        {
            btnImage.Visible = false;
        }
        else
        {
            btnImage.Visible = Camera.GetPicturesForTicket(SeedTicketInfo.CurrentSeedTicketRow.Ticket.ToString()).Count > 0;
            btnImage.PostBackUrl = $"TicketImage?Ticket={SeedTicketInfo.CurrentSeedTicketRow.Ticket}";
        }




        btnDone.Visible = SeedTicketInfo.CurrentSeedTicketRow.ReadOnly;
        btnSave.Visible = (!SeedTicketInfo.CurrentSeedTicketRow.ReadOnly && (SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Varieties.Count > 0 || SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Treatments.Count > 0 || SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Weights.Count > 0 || SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Misc.Count > 0));
        btnCancel.Visible = !SeedTicketInfo.CurrentSeedTicketRow.ReadOnly;
        btnPrint.Visible = SeedTicketInfo.TicketComplete;

        ddPrintDestination.Visible = SeedTicketInfo.TicketComplete;


        btnComplete.Visible = (!SeedTicketInfo.TicketComplete && ((SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Varieties.Count > 0 || SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Misc.Count > 0 ) && WeightsComplete() && VarietiesComplete()));



        if (SeedTicketInfo.CurrentSeedTicketRow.IsTicketNull())
        {

        }
       
        lnkGrower.Enabled = !ReadOnly;

        btnVarieties.Visible = !ReadOnly;
        btnTreatments.Visible = !ReadOnly;
        btnMisc.Visible = !ReadOnly;
        txtBOL.ReadOnly = ReadOnly;
        txtComment.ReadOnly = ReadOnly;
        txtVehicle.ReadOnly = ReadOnly;
        txtPO.ReadOnly = ReadOnly;
        pnlTicketDetails.Enabled = !ReadOnly;
        SetPLCButtonState();

    }


    public bool WeightsComplete()
    {


        bool Retval = true;
        
        if (SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Weights.Count == 0 && SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Misc.Count==0 )
        {
            Retval = false;
        }
        else
        {
            foreach (var item in SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Weights)
            {
                if (item.IsStarting_WeightNull() || item.IsEnding_WeightNull())
                {
                    Retval = false;
                    break;
                }

            }
        }
        return Retval;

    }


    public void SetWeights()
    {
        pnlTruckWeights.Visible = false;
        pnlToteWeights.Visible = false;
        lblToteTotals.Visible = false;
        pnlBagWeights.Visible = false;
        if (SeedTicketInfo.TicketWeighType == GlobalVars.enumSeedTicketWeighType.Truck || SeedTicketInfo.TicketWeighType == GlobalVars.enumSeedTicketWeighType.ReturnBulk)
        {
            SetTruckWeights();
        }
        else if (SeedTicketInfo.TicketWeighType == GlobalVars.enumSeedTicketWeighType.Tote || SeedTicketInfo.TicketWeighType == GlobalVars.enumSeedTicketWeighType.ReturnTote)
        {
            SetToteWeights();
        }
        else if (SeedTicketInfo.TicketWeighType == GlobalVars.enumSeedTicketWeighType.Bag || SeedTicketInfo.TicketWeighType == GlobalVars.enumSeedTicketWeighType.ReturnBag)
        {
            SetBagWeights();
        }
        SetPLCButtonState();
    }


    public void SetTruckWeights()
    {
        SeedTicketDataSet.Seed_Ticket_WeightsRow row = (SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Weights.Count == 0) ? null : SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Weights[0];
        pnlTruckWeights.Visible = (row != null);

        bool TruckWeighed = (row != null && !row.IsStarting_WeightNull() && !row.IsEnding_WeightNull());

        tblTruckGTN.Visible = TruckWeighed;
        if (row != null)
        {
            lblTruckTimeIn.Visible = !row.IsStarting_TimeNull();
            lnkSetTruckWeightIn.Visible = row.IsStarting_WeightNull();
            lblTruckWeightIn.Visible = !row.IsStarting_WeightNull();


            lblTruckTimeOut.Visible = !row.IsEnding_TimeNull();
            lblTruckWeightOut.Visible = !row.IsEnding_WeightNull();
            lnkSetTruckOutWeight.Visible = row.IsEnding_WeightNull();


            if (!row.IsStarting_WeightNull()) lblTruckWeightIn.Text = string.Format("{0:N0}", row.Starting_Weight);
            if (!row.IsStarting_TimeNull()) lblTruckTimeIn.Text = row.Starting_Time.ToShortTimeString();

            if (!row.IsEnding_WeightNull()) lblTruckWeightOut.Text = string.Format("{0:N0}", row.Ending_Weight);
            if (!row.IsEnding_TimeNull()) lblTruckTimeOut.Text = row.Ending_Time.ToShortTimeString();
        }
        if (TruckWeighed)
        {
            float G, T, N, B;
            G = (row.Starting_Weight > row.Ending_Weight) ? row.Starting_Weight : row.Ending_Weight;
            T = (row.Starting_Weight < row.Ending_Weight) ? row.Starting_Weight : row.Ending_Weight;
            N = Math.Abs(G - T);
            B = N / 60;
            lblGross.Text = string.Format("{0:N0}", G);
            lblTare.Text = string.Format("{0:N0}", T);
            lblNet.Text = string.Format("{0:N0}", N);
            lblBushels.Text = string.Format("{0:N2}", B);
        }
       
    }


    public void SetToteWeights()
    {
        using (ListDataSet.TicketWeightsDataTable ticketWeightsDataTable = new ListDataSet.TicketWeightsDataTable())
        {
            bool AllTotesWeighed = true;
            float Net = 0;

            foreach (var item in SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Weights)
            {
                ListDataSet.TicketWeightsRow row = ticketWeightsDataTable.NewTicketWeightsRow();
                if (!item.IsStarting_TimeNull()) row.Creation_Date = item.Creation_Date;
                row.Starting_Weight = (item.IsStarting_WeightNull()) ? "Not Set" : item.Starting_Weight.ToString();
                row.Ending_Weight = (item.IsEnding_WeightNull()) ? "Not Set" : item.Ending_Weight.ToString();
                row.UID = item.UID;

                ticketWeightsDataTable.AddTicketWeightsRow(row);

                if (item.IsStarting_WeightNull() || item.IsEnding_WeightNull()) AllTotesWeighed = false;
                if (!item.IsStarting_WeightNull() && !item.IsEnding_WeightNull())
                {
                    float ToteNetWeight = Math.Abs(item.Ending_Weight - item.Starting_Weight);
                    Net += ToteNetWeight;
                    row.Net = string.Format("{0:N0}", ToteNetWeight);
                }
                else
                {
                    row.Net = "Not Set";
                }
            }
            lblToteTotals.Text = string.Format("Total Net:  <strong>{0:N0}</strong>", Net);
            lblToteTotals.Visible = true;
            btnWeight.Text = "Add Tote";
            btnWeight.Visible = AllTotesWeighed;
            pnlToteWeights.Visible = SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Weights.Count > 0;
            DataSet ds = new DataSet();
            ds.Tables.Add(ticketWeightsDataTable);
            DataView dv = ds.Tables[0].DefaultView;
            dv.Sort = ("Creation_Date desc");
            grdTotes.DataSource = dv;
            grdTotes.DataKeyNames = new string[] { "UID" };
            grdTotes.DataBind();
        }
        
    }

    public void SetBagWeights()
    {
        pnlBagWeights.Visible = true;
        ddBagSizes.DataBind();
        ddBagSizes.ClearSelection();
        if (SeedTicketInfo.CurrentSeedTicketRow.IsBag_CntNull()) SeedTicketInfo.CurrentSeedTicketRow.Bag_Cnt = 1;
        if (SeedTicketInfo.CurrentSeedTicketRow.IsBag_SizeNull()) SeedTicketInfo.CurrentSeedTicketRow.Bag_Size = int.Parse(txtBagSize.Text);
        if (SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Weights.Count == 0)
        {
            ddBagSizes.SelectedIndex = 0;
            SeedTicketInfo.CurrentSeedTicketWeightRow = SeedTicketInfo.CreateSeedTicketWeightsRow();

        }
        else
        {
            SeedTicketInfo.CurrentSeedTicketWeightRow = SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Weights[0];
            txtBagSize.Text = SeedTicketInfo.CurrentSeedTicketRow.Bag_Size.ToString();
            //ListItem Li = ddBagSize.Items.FindByValue(SeedTicketInfo.CurrentSeedTicketRow.Bag_Size.ToString());
            //if (Li != null)
            //{
            //    Li.Selected = true;
            //}
            //else
            //{
            //    ddBagSize.SelectedIndex = 0;
            //    SeedTicketInfo.CurrentSeedTicketRow.Bag_Size = int.Parse(ddBagSize.Items[0].Value);
            //}
        }
        txtBagCount.Text = SeedTicketInfo.CurrentSeedTicketRow.Bag_Cnt.ToString();
        int Total = SeedTicketInfo.CurrentSeedTicketRow.Bag_Cnt * SeedTicketInfo.CurrentSeedTicketRow.Bag_Size;
        if (SeedTicketInfo.CurrentSeedTicketWeightRow.IsStarting_TimeNull()) SeedTicketInfo.CurrentSeedTicketWeightRow.Starting_Time = DateTime.Now;
        if (SeedTicketInfo.CurrentSeedTicketWeightRow.IsEnding_TimeNull()) SeedTicketInfo.CurrentSeedTicketWeightRow.Ending_Time = DateTime.Now;
        SeedTicketInfo.CurrentSeedTicketWeightRow.Starting_Weight = Total;
        SeedTicketInfo.CurrentSeedTicketWeightRow.Ending_Weight = 0;
        lblBagTotal.Text = Total.ToString();
        SeedTicketInfo.SaveTicket();
        SetMisc();
        UP1.Update();
    }



    public void SetMisc()
    {
        pnlMisc.Visible = SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Misc.Count > 0;
        //DataSet ds = new DataSet();
        //ds.Tables.Add(SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Misc );
        //DataView dv = ds.Tables[0].DefaultView;
        //dv.Sort = ("Description");
        grdMisc.DataSource = SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Misc;
        grdMisc.DataKeyNames = new string[] { "UID" };
        grdMisc.DataBind();
    }


    public bool VarietiesComplete()
    {
        bool Retval = false;
        ReportDataSet reportDataset = SeedTicketInfo.GetAllTicketInfo(SeedTicketInfo.seedTicketDataSet, SeedTicketInfo.CurrentSeedTicketRow.UID);

            if (reportDataset.Ticket_Varieties.Count > 0)
        {
            using (SeedTicketDataSet.VarietyListDataTable VarietyList = new SeedTicketDataSet.VarietyListDataTable())
            {
                decimal Total = 0;
                foreach (var row in reportDataset.Ticket_Varieties)
                {
                    var vlRow = VarietyList.NewVarietyListRow();
                    vlRow.Item = row.Variety_ID;
                    vlRow.Description = row.Description;// (row.Description.Length > 22) ? row.Description.Substring(0, 22) :
                    vlRow.Percent = (decimal)row.Percent_Of_Load;
                    Total += vlRow.Percent;
                    vlRow.Bin = (row.IsBin_NameNull()) ? "" : row.Bin_Name;
                    vlRow.Total = Convert.ToInt32(row.Total);
                    VarietyList.AddVarietyListRow(vlRow);
                    vlRow.UID = row.UID;
                }
                decimal Remainder = 0;
                decimal RoundingError = 0;
                if (Total < 100)
                {
                    Remainder = 100 - Total;
                    if (Remainder <= 1 && reportDataset.Ticket_Varieties.Count > 0)
                    {
                        Total = 0;
                        RoundingError = Remainder / reportDataset.Ticket_Varieties.Count;
                        foreach (var item in VarietyList)
                        {
                            item.Percent += RoundingError;
                            Total += item.Percent;
                        }
                    }
                }

                Retval = (Total == 100);

            }
        }
        else if (reportDataset.Ticket_Varieties.Count==0  && SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Misc.Count > 0)
        {
            Retval = true;
        }
            return Retval;
    }


    public void SetVarieties(ReportDataSet reportDataset)
    {
        using (SeedTicketDataSet.VarietyListDataTable VarietyList = new SeedTicketDataSet.VarietyListDataTable())
        {
            decimal Total = 0;
            foreach (var row in reportDataset.Ticket_Varieties)
            {
                var vlRow = VarietyList.NewVarietyListRow();
                vlRow.Item = row.Variety_ID;
                vlRow.Description = row.Description;// (row.Description.Length > 22) ? row.Description.Substring(0, 22) :
                vlRow.Percent = (decimal)row.Percent_Of_Load;
                Total += vlRow.Percent;
                vlRow.Bin = (row.IsBin_NameNull()) ? "" : row.Bin_Name;
                vlRow.Total = Convert.ToInt32(row.Total);
                VarietyList.AddVarietyListRow(vlRow);
                vlRow.UID = row.UID;
            }
            decimal Remainder = 0;
            decimal RoundingError = 0;
            if (Total < 100)
            {
                Remainder = 100 - Total;
                if (Remainder <= 1 && reportDataset.Ticket_Varieties.Count > 0)
                {
                    Total = 0;
                    RoundingError = Remainder / reportDataset.Ticket_Varieties.Count;
                    foreach (var item in VarietyList)
                    {
                        item.Percent += RoundingError;
                        Total += item.Percent;
                    }
                }
            }

            grdVarieties.DataSource = VarietyList;
            grdVarieties.DataBind();
            if (Total == 0)
            {
                lblVarietyTotal.Text = "";
            }
            else if (Total == 100)
            {
                lblVarietyTotal.CssClass = "font-weight-bold text-success";
                lblVarietyTotal.Text = "Total 100%";



            }
            else
            {
                lblVarietyTotal.CssClass = "font-weight-bold text-danger blinkingdanger";
                lblVarietyTotal.Text = $"Total {Total}%";
            }

        }
    }






    public void SetTreatments(ReportDataSet reportDataset)
    {

        using (var TreatmentList = new SeedTicketDataSet.TreatmentListDataTable())
        {
            foreach (var row in reportDataset.Ticket_Treatments)
            {
                var tlRow = TreatmentList.NewTreatmentListRow();
                tlRow.Item = row.ID;
                tlRow.Description = row.Description;
                tlRow.Rate = (decimal)row.Rate;


                tlRow.Total = Convert.ToInt32(row.Total);
                TreatmentList.AddTreatmentListRow(tlRow);
                tlRow.UID = row.UID;
            }
            grdTreatments.DataSource = TreatmentList;
            grdTreatments.DataBind();
        }
    }

    protected void lnkDeleteVariety_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        GridViewRow row = (GridViewRow)btn.NamingContainer;
        int index = row.RowIndex;
        Guid UID = Guid.Parse(grdVarieties.DataKeys[index].Values[0].ToString());
        var VarietyRow = SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Varieties.FirstOrDefault(x => x.UID == UID);
        if (VarietyRow != null)
        {
            VarietyRow.Delete();
        }

        Response.Redirect(Request.RawUrl);
    }

    protected void btnAddVariety_Click(object sender, EventArgs e)
    {
        if (SeedTicketInfo.seedTicketDataSet == null) Response.Redirect("SelectGrower.aspx");
        SeedTicketInfo.CurrentSeedTicketVarietyRow = null;
        SeedTicketInfo.CreateSeedTicketVarietyRow();
        Response.Redirect("AddEditVariety?AddNew=true");
    }

    protected void lnkVarietyID_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        GridViewRow row = (GridViewRow)btn.NamingContainer;
        int index = row.RowIndex;
        Guid UID = Guid.Parse(grdVarieties.DataKeys[index].Values[0].ToString());
        if (SeedTicketInfo.seedTicketDataSet == null) Response.Redirect("SelectGrower.aspx");
        SeedTicketInfo.CurrentSeedTicketVarietyRow = SeedTicketInfo.GetSeedTicketVarietyRow(UID);
    
        if (SeedTicketInfo.CurrentSeedTicketVarietyRow.Variety_ID < 0)
        {
            Response.Redirect("UnknownVariety?AddNew=false");
        }
        else
        {
            Response.Redirect("AddEditVariety?AddNew=false");
        }

    }


    protected void lnkTreatmentID_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        GridViewRow row = (GridViewRow)btn.NamingContainer;
        int index = row.RowIndex;
        Guid UID = Guid.Parse(grdTreatments.DataKeys[index].Values[0].ToString());
        if (SeedTicketInfo.seedTicketDataSet == null) Response.Redirect("SelectGrower.aspx");
        SeedTicketInfo.CurrentSeedTicketTreatmentRow = SeedTicketInfo.GetSeedTicketTreatmentRow(UID);
        if (SeedTicketInfo.CurrentSeedTicketTreatmentRow.Treatment_ID < 0)
        {
            Response.Redirect("UnknownTreatment?AddNew=false");
        }
        else
        {
            Response.Redirect("AddEditTreatment?AddNew=false");
        }
    }

    protected void btnAddTreatment_Click(object sender, EventArgs e)
    {
        if (SeedTicketInfo.seedTicketDataSet == null) Response.Redirect("SelectGrower.aspx");
        SeedTicketInfo.CurrentSeedTicketTreatmentRow = null;
        SeedTicketInfo.CreateSeedTicketTreatmentRow();
        Response.Redirect("AddEditTreatment?AddNew=true");
    }

    protected void lnkDeleteTreatment_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        GridViewRow row = (GridViewRow)btn.NamingContainer;
        int index = row.RowIndex;
        Guid UID = Guid.Parse(grdTreatments.DataKeys[index].Values[0].ToString());
        var TreatmentRow = SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Treatments.FirstOrDefault(x => x.UID == UID);
        if (TreatmentRow != null)
        {
            TreatmentRow.Delete();
        }

        Response.Redirect(Request.RawUrl);

    }


    protected void txtWeighmaster_TextChanged(object sender, EventArgs e)
    {
        SeedTicketInfo.CurrentSeedTicketRow.Weighmaster = txtWeighmaster.Text;
        Session["Weighmaster"] = SeedTicketInfo.CurrentSeedTicketRow.Weighmaster;
    }

    protected void txtPO_TextChanged(object sender, EventArgs e)
    {
        SeedTicketInfo.CurrentSeedTicketRow.PO = txtPO.Text;

    }

    protected void txtBOL_TextChanged(object sender, EventArgs e)
    {
        SeedTicketInfo.CurrentSeedTicketRow.BOL = txtBOL.Text;
    }

    protected void txtComment_TextChanged(object sender, EventArgs e)
    {
        SeedTicketInfo.CurrentSeedTicketRow.Comments = txtComment.Text;
    }

    protected void btnWeight_Click(object sender, EventArgs e)
    {
        GlobalVars.SeedTicketWeighType = (GlobalVars.enumSeedTicketWeighType)Enum.Parse(typeof(GlobalVars.enumSeedTicketWeighType), ddWeightype.Text);
        SeedTicketInfo.CurrentSeedTicketRow.Ticket_Type = GlobalVars.SeedTicketWeighType.ToString();
        Response.Redirect("AddWeights?WeighIn=true&NewLoad=true");

    }

    protected void txtVehicle_TextChanged(object sender, EventArgs e)
    {
        SeedTicketInfo.CurrentSeedTicketRow.Truck_ID = txtVehicle.Text;
    }

    protected void ddWeightype_TextChanged(object sender, EventArgs e)
    {
        //if (this.IsPostBack)
        {
            if ((SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Weights.Count > 0 && SeedTicketInfo.TicketWeighType != GlobalVars.enumSeedTicketWeighType.Bag) && (SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Weights.Count > 0 && SeedTicketInfo.TicketWeighType != GlobalVars.enumSeedTicketWeighType.ReturnBag))
            {
                Response.Redirect($"ConfirmWeighTypeChange?NewType={ddWeightype.Text}");

            }
            else
            {
                SeedTicketInfo.ResetWeights();
                SeedTicketInfo.CurrentSeedTicketRow.Ticket_Type = ddWeightype.Text;
                SeedTicketInfo.CurrentSeedTicketRow.Returned = ddWeightype.Text.ToUpper().Contains("RETURN");
                GlobalVars.SeedTicketWeighType = (GlobalVars.enumSeedTicketWeighType)Enum.Parse(typeof(GlobalVars.enumSeedTicketWeighType), ddWeightype.Text);


                if (SeedTicketInfo.TicketWeighType == GlobalVars.enumSeedTicketWeighType.Bag || SeedTicketInfo.TicketWeighType == GlobalVars.enumSeedTicketWeighType.ReturnBag)
                {
                    SeedTicketInfo.AddBag(1);

                }

                Response.Redirect(Request.RawUrl);
            }
        }


    }




    protected void lnkSetTruckWeightIn_Click(object sender, EventArgs e)
    {
        SeedTicketInfo.CurrentSeedTicketWeightRow = SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Weights[0];

        Response.Redirect("AddWeights?WeighIn=true&NewLoad=false");
    }

    protected void lnkSetTruckOutWeight_Click(object sender, EventArgs e)
    {
        SeedTicketInfo.CurrentSeedTicketWeightRow = SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Weights[0];
        Response.Redirect("AddWeights?WeighIn=false&NewLoad=false");
    }

    protected void lblTruckWeightIn_Click(object sender, EventArgs e)
    {
        Response.Redirect("AddWeights?WeighIn=true&NewLoad=false");
    }

    protected void lblTruckWeightOut_Click(object sender, EventArgs e)
    {
        Response.Redirect("AddWeights?WeighIn=false&NewLoad=false");

    }


    protected void lnkSetEndTote_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        GridViewRow row = (GridViewRow)btn.NamingContainer;
        int index = row.RowIndex;
        Guid UID = Guid.Parse(grdTotes.DataKeys[index].Values[0].ToString());
        if (SeedTicketInfo.seedTicketDataSet == null) Response.Redirect("SelectGrower.aspx");
        SeedTicketInfo.CurrentSeedTicketWeightRow = SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Weights.FindByUID(UID);
        if (SeedTicketInfo.CurrentSeedTicketWeightRow != null) Response.Redirect("AddWeights?WeighIn=false&NewLoad=false");

    }

    protected void lbToteStartWeight_Click(object sender, EventArgs e)
    {

        LinkButton btn = (LinkButton)sender;
        GridViewRow row = (GridViewRow)btn.NamingContainer;
        int index = row.RowIndex;
        Guid UID = Guid.Parse(grdTotes.DataKeys[index].Values[0].ToString());
        SeedTicketInfo.CurrentSeedTicketWeightRow = SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Weights.FindByUID(UID);
        if (SeedTicketInfo.CurrentSeedTicketWeightRow != null) Response.Redirect("AddWeights?WeighIn=true&NewLoad=false");

    }

    protected void lbToteEndWeight_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        GridViewRow row = (GridViewRow)btn.NamingContainer;
        int index = row.RowIndex;
        Guid UID = Guid.Parse(grdTotes.DataKeys[index].Values[0].ToString());
        SeedTicketInfo.CurrentSeedTicketWeightRow = SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Weights.FindByUID(UID);
        if (SeedTicketInfo.CurrentSeedTicketWeightRow != null) Response.Redirect("AddWeights?WeighIn=false&NewLoad=false");
    }

    protected void lbToteDelete_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        GridViewRow row = (GridViewRow)btn.NamingContainer;
        int index = row.RowIndex;
        Guid UID = Guid.Parse(grdTotes.DataKeys[index].Values[0].ToString());
        SeedTicketInfo.CurrentSeedTicketWeightRow = SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Weights.FindByUID(UID);
        if (SeedTicketInfo.CurrentSeedTicketWeightRow != null) Response.Redirect("ConfirmWeightDelete");

    }



    protected void txtBagCount_TextChanged(object sender, EventArgs e)
    {
        var Quantity = int.Parse(txtBagCount.Text);
        if (Quantity <= 0)
        {
            Quantity = 1;
            txtBagCount.Text = Quantity.ToString();
        }

        SeedTicketInfo.CurrentSeedTicketRow.Bag_Cnt = Quantity;
        SeedTicketInfo.AddBag(Quantity);
        SetBagWeights();
    }

    protected void ddBagSize_TextChanged(object sender, EventArgs e)
    {
        if (ddBagSizes.SelectedIndex > 0)
        {
            SeedTicketInfo.CurrentSeedTicketRow.Bag_Size = int.Parse(ddBagSizes.Text);
            txtBagSize.Text = SeedTicketInfo.CurrentSeedTicketRow.Bag_Size.ToString();
            SetBagWeights();
            ddBagSizes.SelectedIndex = 0;
            UP1.Update();
            pnlUpdatebtns.Update();
        }
    }

    protected void lnkDeleteMisc_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        GridViewRow row = (GridViewRow)btn.NamingContainer;
        int index = row.RowIndex;
        Guid UID = Guid.Parse(grdMisc.DataKeys[index].Values[0].ToString());
        var MiscRow = SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Misc.FirstOrDefault(x => x.UID == UID);
        if (MiscRow != null)
        {
            MiscRow.Delete();
        }

        Response.Redirect(Request.RawUrl);

    }

    protected void lnkQuantityID_TextChanged(object sender, EventArgs e)
    {

        TextBox tbx = (TextBox)sender;
        int Quantity = int.Parse(tbx.Text);
        GridViewRow row = (GridViewRow)tbx.NamingContainer;
        int index = row.RowIndex;
        Guid UID = Guid.Parse(grdMisc.DataKeys[index].Values[0].ToString());
        var MiscRow = SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Misc.FirstOrDefault(x => x.UID == UID);
        if (MiscRow != null && Quantity > 0)
        {
            MiscRow.Quantity = Quantity;
        }

        SetMisc();
        UP1.Update();
        pnlUpdatebtns.Update();
    }





    protected void btnSave_Click(object sender, EventArgs e)
    {
        SeedTicketInfo.SaveTicket();
        Response.Redirect("~/Default.aspx");
    }

    protected void btnComplete_Click(object sender, EventArgs e)
    {

        SeedTicketInfo.CompleteTicket();
        Guid UID = SeedTicketInfo.CurrentSeedTicketRow.UID;
        SeedTicketInfo.CurrentSeedTicketRow = null;
        SeedTicketInfo.seedTicketDataSet = null;
        PrintTicket(UID, 2);
        



    }



    public void PrintTicket(Guid UID,int Copies=1)
    {
        if (GlobalVars.UsePrinter)
        {
            Response.Redirect($"~/PrintTicket?Copies={Copies}&UID={UID}");
            Printing.Print_Ticket(UID, "", 2);
        }
        else
        {
            Printing.Send_TicketToBrowser(UID, false);
        }

    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        PrintTicket(SeedTicketInfo.CurrentSeedTicketRow.UID);


   
    }

    protected void ddPrintDestination_TextChanged(object sender, EventArgs e)
    {
        GlobalVars.UsePrinter = (ddPrintDestination.SelectedIndex == 0) ? true : false;
    }

    protected void btnImage_Click(object sender, EventArgs e)
    {

    }

    protected void btnSendToPLC_Click(object sender, EventArgs e)
    {

        if (PLC.PLCConnected && PLC.plcDataset != null)
        {
            try
            {
                PLC.pcResponse.Reset();
                bool cleanAvailable = PLC.plcDataset.BatchTypeAvailability[0].Clean_Available;
                bool TreatAvailable = PLC.plcDataset.BatchTypeAvailability[0].Treat_Available;
                bool VarietiesUsed = false;
                bool TreatmentsUsed = false;
                
                
                if (TreatAvailable || cleanAvailable)
                {
                    foreach (var item in SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Varieties)
                    {
                        if (!item.IsBinNull())
                        {
                            var plcRow = PLC.plcDataset.Bins.FirstOrDefault(x => x.Bin_Id == item.Bin);
                            if (plcRow != null)
                            {
                                VarietiesUsed = true;
                                PLC.pcResponse.BinStatus.Add(
                                    new PLC.PCResponse.ItemStatus()
                                    {
                                        ItemId = item.Bin,
                                        Active = true
                                    });
                            }
                        }


                    }
                    foreach (var item in SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Treatments)
                    {
                        var plcRow = PLC.plcDataset.Treatments.FirstOrDefault(x => x.Description.ToUpper()  == item.Custom_Name.ToUpper());
                        if (plcRow != null)
                        {
                            TreatmentsUsed = true;
                            PLC.pcResponse.TreatmentStatus.Add(
                                    new PLC.PCResponse.ItemStatus()
                                    {
                                        ItemId = plcRow.Pump_Index ,
                                        Active = true
                                    });
                        }

                    }

                    if (TreatmentsUsed)
                    {
                        string UsedVarieties = "";
                        foreach(var item in SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Varieties  )
                        {
                            UsedVarieties += (string.IsNullOrEmpty(UsedVarieties)) ? item.Variety_ID.ToString() : ","+item.Variety_ID.ToString();
                        }
                        using (VarietiesDataSetTableAdapters.SeedTicketColorsTableAdapter seedTicketColorsTableAdapter = new VarietiesDataSetTableAdapters.SeedTicketColorsTableAdapter())
                        {
                            using (VarietiesDataSet.SeedTicketColorsDataTable seedTicketColorsDataTable = new VarietiesDataSet.SeedTicketColorsDataTable())
                            {
                                if (seedTicketColorsTableAdapter.FillByUsedVariety(seedTicketColorsDataTable,UsedVarieties ) > 0)
                                {
                                    var cRow = seedTicketColorsDataTable[0];
                                    if (cRow.IsSecondary_Color_IndexNull()) cRow.Secondary_Color_Index = -1;
                                    if (cRow.IsTertiary_Color_IndexNull()) cRow.Tertiary_Color_Index = -1;
                                    for (int I = 0; I < 10; I++)
                                    {
                                        bool Active = (cRow.Primary_Color_Index == I) || (cRow.Secondary_Color_Index == I || cRow.Tertiary_Color_Index == I);
                                        PLC.pcResponse.ColorStatus.Add(new PLC.PCResponse.ItemStatus()
                                            {
                                                ItemId = I,
                                                Active = Active
                                            });
                                    }
                                }
                            }

                        }
                    }
                }
                if (TreatmentsUsed )
                {
                    PLC.pcResponse.BatchType = PLC.enumBatchType.Bulk; 
                }
                else if (VarietiesUsed )
                {
                    PLC.pcResponse.BatchType = PLC.enumBatchType.Clean ;
                }
                PLC.pcResponse.UID = Guid.NewGuid();
                
                btnSendToPLC.Enabled=false;
                btnSendToPLC.Text = "Sending";
                tmrUPdateButtons.Interval = 10000;

            }






            catch
            {

            }
        }


    }

    protected void tmrUPdateButtons_Tick(object sender, EventArgs e)
    {
        SetPLCButtonState();
        tmrUPdateButtons.Interval = 2000;
    }

    public void SetPLCButtonState()
    {
        btnSendToPLC.Enabled = true;
        btnSendToPLC.Text = "Send To PLC";

        bool ShowButton = PLC.PLCConnected && SeedTicketInfo.CurrentSeedTicketRow.IsTicketNull();
        if (ShowButton && PLC.plcDataset != null)
        {
            try
            {
                bool cleanAvailable = PLC.plcDataset.BatchTypeAvailability[0].Clean_Available;
                bool TreatAvailable = PLC.plcDataset.BatchTypeAvailability[0].Treat_Available;
                bool VarietiesUsed = false;
                //bool TreatmentsUsed = false;
                if (TreatAvailable || cleanAvailable)
                {
                    foreach (var item in SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Varieties)
                    {
                        if (!item.IsBinNull())
                        {
                            var plcRow = PLC.plcDataset.Bins.FirstOrDefault(x => x.Bin_Id == item.Bin);
                            if (plcRow != null)
                            {
                                VarietiesUsed = true;
                                break;
                            }
                        }


                    }
                    //foreach(var item in SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Treatments  )
                    //{
                    //    var plcRow = PLC.plcDataset.Treatments.FirstOrDefault(x => x.Treatment_Id == item.Treatment_ID);
                    //    if (plcRow != null)
                    //    {

                    //    }

                    //}
                    ShowButton = ((cleanAvailable || TreatAvailable) && VarietiesUsed);

                }
                else
                {
                    ShowButton = false;
                }


                //ShowButton = PLC.plcDataset.BatchTypeAvailability[0].Clean_Available

            }

            catch
            {
                ShowButton = false;
            }
        }
        else
        {
            ShowButton = false;
        }
        //SeedTicketDataSet.Seed_Ticket_WeightsRow row = (SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Weights.Count == 0) ? null : SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Weights[0];
        bool WeightSet = (SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Weights.Count != 0);
        if (!WeightSet) ShowButton = false;
        btnSendToPLC.Visible = ShowButton;
    }

    protected void btnPending_Click(object sender, EventArgs e)
    {
    
    }





    protected void ckPending_CheckedChanged(object sender, EventArgs e)
    {
        SeedTicketInfo.CurrentSeedTicketRow.Pending = ckPending.Checked; 
        
    }

    protected void txtBagSize_TextChanged(object sender, EventArgs e)
    {
        int bagSize = 60;
        int.TryParse(txtBagSize.Text, out bagSize);
        SeedTicketInfo.CurrentSeedTicketRow.Bag_Size =bagSize ;
        txtBagSize.Text = SeedTicketInfo.CurrentSeedTicketRow.Bag_Size.ToString();
        SetBagWeights();
        ddBagSizes.SelectedIndex = 0;
        UP1.Update();
        pnlUpdatebtns.Update();

    }

   
}