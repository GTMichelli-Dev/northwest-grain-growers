using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Lots_LotVariety : System.Web.UI.Page
{

    public Guid LotUID
    {

        get
        {
            Guid UID = Guid.Empty;
            if (Request.QueryString["LotUID"] != null) Guid.TryParse(Request.QueryString["LotUID"].ToString(), out UID);
            return UID;
        }
    }


    public string Lot_Number
    {

        get
        {
            string Lot = string.Empty;
            if (Request.QueryString["Lot_Number"] != null) Lot = Request.QueryString["Lot_Number"].ToString();
            return Lot;
        }
    }


    public string Crop
    {

        get
        {
            string crop = string.Empty;
            if (Request.QueryString["Crop"] != null) crop = Request.QueryString["Crop"].ToString();
            return crop;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {

            if (!string.IsNullOrWhiteSpace(Lot_Number) && (LotUID != Guid.Empty))
            {
                Update.Visible = false;
                lblHeader.Text = "Update Variety For Lot: <strong>" + Lot_Number + "</strong>";
            }
            else
            {
                Response.Redirect("~/Lots/WarehouseLots.aspx");
            }

        }

    }



    protected void lnkSelect_Click(object sender, EventArgs e)
    {
        using (LotsDataSet.LotsDataTable lotsDataTable = new LotsDataSet.LotsDataTable())
        {
            using (LotsDataSetTableAdapters.LotsTableAdapter lotsTableAdapter = new LotsDataSetTableAdapters.LotsTableAdapter())
            {
                if (lotsTableAdapter.FillByUID(lotsDataTable, LotUID) > 0)
                {

                    LinkButton lb = (LinkButton)sender;
                    GridViewRow row = (GridViewRow)lb.NamingContainer;
                    if (string.IsNullOrWhiteSpace(GridView1.DataKeys[row.RowIndex].Value.ToString()))
                    {
                            lotsDataTable[0].SetVariety_IdNull();
                            lotsTableAdapter.Update(lotsDataTable);
                    }
                    else
                    {
                        var VarietyId = (int)GridView1.DataKeys[row.RowIndex].Value;
                        
                        
                            lotsDataTable[0].Variety_Id = VarietyId;
                            lotsTableAdapter.Update(lotsDataTable);
                        

                    }
                }
                Response.Redirect("~/Lots/LotDetails.aspx?UID=" + LotUID.ToString());
            }
        }
    }
}