using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Lots_LotDetails : System.Web.UI.Page
{



    public Guid UID
    {
        get
        {
            Guid UID;

            if ((Request.QueryString["UID"] == null) || (!Guid.TryParse(Request.QueryString["UID"].ToString(), out UID)))
            {
                UID = Guid.Empty;
            }
            return UID;
        }
    }


    public LotsDataSet.LotsRow LotRow
    {
        get
        {
            using (LotsDataSetTableAdapters.LotsTableAdapter lotsTableAdapter = new LotsDataSetTableAdapters.LotsTableAdapter())
            {
                using (LotsDataSet.LotsDataTable lotsDataTable = new LotsDataSet.LotsDataTable())
                {


                    if (lotsTableAdapter.FillByUID(lotsDataTable, UID) > 0)
                    {
                        return lotsDataTable[0];
                    }
                    else
                    {
                        return null;
                    }

                }
            }
        }
        set
        {
            using (LotsDataSetTableAdapters.LotsTableAdapter lotsTableAdapter = new LotsDataSetTableAdapters.LotsTableAdapter())
            {
                using (LotsDataSet.LotsDataTable lotsDataTable = new LotsDataSet.LotsDataTable())
                {


                    if (lotsTableAdapter.FillByUID(lotsDataTable, UID) > 0)
                    {


                        lotsDataTable[0].ItemArray = value.ItemArray;


                        lotsTableAdapter.Update(lotsDataTable);
                    }

                }
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!this.IsPostBack)
        {
            if (UID == Guid.Empty)
            {
                Response.Redirect("~/Lots/WarehouseLots");

            }

            DetailsView2.DataBind();

            LinkButton lnkReopen = (LinkButton)DetailsView2.FindControl("lnkReopen");

            bool Admin = (Security.GetUsersSecurityLevel(Session) == Security.enumSecurityLevel.Administrator);
            if (lnkReopen != null) lnkReopen.Visible = Admin;
            DetailsView1.DefaultMode = (Admin == true) ? DetailsViewMode.Edit : DetailsViewMode.ReadOnly;
            if (lnkReopen.Visible)
            {
                lnkReopen.Text = (LotRow.IsClose_DateNull()) ? "Close Lot" : "OpenLot";
            }
            DetailsView1.DataBind();
        }


    }

    protected void lnkWeightSheets_Click(object sender, EventArgs e)
    {

        LinkButton lnkWeightSheets = (LinkButton)sender;


        HiddenField hfLot = (HiddenField)DetailsView2.FindControl("hfLot");



        Session["WSLot"] = hfLot.Value;
        Response.Redirect("~/WeightSheets/IntakeWeightSheets.aspx");

    }

    protected void lnkReopen_Click(object sender, EventArgs e)
    {
        using (LotsDataSet.LotsDataTable lotsDataTable = new LotsDataSet.LotsDataTable())
        {
            using (LotsDataSetTableAdapters.LotsTableAdapter lotsTableAdapter = new LotsDataSetTableAdapters.LotsTableAdapter())
            {
                if (lotsTableAdapter.FillByUID(lotsDataTable, UID) > 0)
                {
                    LotsDataSet.LotsRow row = lotsDataTable[0];
                    if (row.IsClose_DateNull())
                    {
                        row.Close_Date = DateTime.Now;
                        Audit.AddAuditTrail("Close Lot", row.Location_Id, row.Lot_Number.ToString(), "Closed Lot From Office", "Open", "Closed");
                    }
                    else
                    {
                        row.SetClose_DateNull();
                        Audit.AddAuditTrail("Re Open Lot", row.Location_Id, row.Lot_Number.ToString(), "Closed Opened From Office", "Closed", "Open");
                    }
                    lotsTableAdapter.Update(lotsDataTable);
                }
            }

        }
        Response.Redirect(Request.RawUrl);


    }



    protected void ckLotSampled_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox ck = (CheckBox)sender;
        LotsDataSet.LotsRow row = LotRow;
        row.Lot_Sampled = ck.Checked;
        Audit.AddAuditTrail("Changed Lot Sampled", row.Location_Id, row.Lot_Number.ToString(), "Lot Sampled Changed From Office", (!ck.Checked).ToString(), ck.Checked.ToString());
        LotRow = row;
    }

    protected void txtLandlord_TextChanged(object sender, EventArgs e)
    {
        TextBox tb = (TextBox)sender;
        LotsDataSet.LotsRow row = LotRow;
        Audit.AddAuditTrail("Changed Lot Landlord", row.Location_Id, row.Lot_Number.ToString(), "Lot Landlord Changed From Office", (row.IsLandlordNull())?"": row.Landlord, tb.Text);
        row.Landlord = tb.Text;
        LotRow = row;
    }

    protected void cboCrops_TextChanged(object sender, EventArgs e)
    {
        DropDownList dd = (DropDownList)sender;
        LotsDataSet.LotsRow row = LotRow;
        int NewValue;
        if (int.TryParse(dd.SelectedValue, out NewValue))
        {
            Audit.AddAuditTrail("Changed Lot Crop", row.Location_Id, row.Lot_Number.ToString(), "Lot Crop Changed From Office", row.Crop_Id.ToString() , NewValue.ToString());
            row.Crop_Id = NewValue;
            row.SetVariety_IdNull();
            LotRow = row;
            this.DataBind();


        }

    }



    protected void cboCrops_PreRender(object sender, EventArgs e)
    {
        //DropDownList dd = (DropDownList)this.DetailsView1.FindControl("cboVariety");
        //dd.DataBind();
    }

    protected void DetailsView2_PageIndexChanging(object sender, DetailsViewPageEventArgs e)
    {

    }

    protected void txtFarm_TextChanged(object sender, EventArgs e)
    {
        TextBox tb = (TextBox)sender;
        LotsDataSet.LotsRow row = LotRow;
        Audit.AddAuditTrail("Changed Lot FSA #", row.Location_Id, row.Lot_Number.ToString(), "Lot FSA Changed From Office", (row.IsFSA_NumberNull()) ? "" : row.FSA_Number,  tb.Text);
        if (string.IsNullOrEmpty(tb.Text))
        {
            row.SetFSA_NumberNull();
           
        }
        else
        {
            row.FSA_Number = tb.Text;
        }
        LotRow = row;



    }

    protected void txtComment_TextChanged(object sender, EventArgs e)
    {
        TextBox tb = (TextBox)sender;
        LotsDataSet.LotsRow row = LotRow;
        row.Comment = tb.Text;
        LotRow = row;
    }

    protected void lnkSaveComment_Click(object sender, EventArgs e)
    {
        LinkButton  tb = (LinkButton)sender;
        TextBox txtComments =(TextBox)DetailsView1.Rows[0].FindControl("txtComment");
        LotsDataSet.LotsRow row = LotRow;
        Audit.AddAuditTrail("Changed Lot Comment", row.Location_Id, row.Lot_Number.ToString(), "Lot Comment Changed From Office", (row.IsCommentNull()) ? "" : row.Comment , txtComments.Text);

        row.Comment = txtComments.Text;
        LotRow = row;
    }

    protected void ddState_TextChanged(object sender, EventArgs e)
    {
        DropDownList dd = (DropDownList)sender;
        LotsDataSet.LotsRow row = LotRow;
        string NewValue = dd.SelectedValue; 
        if (!string.IsNullOrEmpty(NewValue) )
        {
            Audit.AddAuditTrail("Changed Lot State", row.Location_Id, row.Lot_Number.ToString(), "Lot State Changed From Office",(row.IsState_AbvNull())?"":row.State_Abv , NewValue);
            row.State_Abv  = NewValue;
            LotRow = row;

        }
        else
        {
            if (!row.IsState_AbvNull()) dd.Text = row.State_Abv;
        }
    }
}