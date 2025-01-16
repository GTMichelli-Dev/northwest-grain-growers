using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Lots_WarehouseLots : System.Web.UI.Page
{
    public DateTime StartDate
    {

        get
        {
            DateTime dt = DateTime.Now;
            if (Session["LotStartDate"] != null)
            {
                DateTime.TryParse(Session["LotStartDate"].ToString(), out dt);
            }
            return dt;
        }
        set
        {

            Session["LotStartDate"] = value;
        }
    }



    public Guid LotUID
    {
        get
        {
            Guid UID;

            if ((Request.QueryString["LotUID"] == null) || (!Guid.TryParse(Request.QueryString["LotUID"].ToString(), out UID)))
            {
                UID = Guid.Empty;
            }
            return UID;
        }
    }


    public Guid WSUID
    {
        get
        {
            Guid UID;

            if ((Request.QueryString["WSUID"] == null) || (!Guid.TryParse(Request.QueryString["WSUID"].ToString(), out UID)))
            {
                UID = Guid.Empty;
            }
            return UID;
        }
    }




    public DateTime EndDate
    {

        get
        {
            DateTime dt = DateTime.Now;
            if (Session["LotEndDate"] != null)
            {
                DateTime.TryParse(Session["LotEndDate"].ToString(), out dt);
            }
            return dt;
        }
        set
        {

            Session["LotEndDate"] = value;

        }
    }



    public string Location
    {

        get
        {
            string lc = "All Locations";
            if (Session["lotLocation"] != null)
            {
                lc = Session["lotLocation"].ToString();
            }
            return lc;
        }
        set
        {

            Session["lotLocation"] = value;

        }
    }





    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            try
            {
                if (Request.QueryString["Lot"] != null)
                {
                    this.txtWSID.Text = Request.QueryString["Lot"].ToString();
                }

                bool AllowSelect = (WSUID != Guid.Empty && LotUID != Guid.Empty);
                GridView1.Columns[0].Visible = AllowSelect;
                GridView1.Columns[1].Visible = !AllowSelect;
                this.txtStartDate.Text = StartDate.ToShortDateString();
                this.txtEndDate.Text = EndDate.ToShortDateString();
                this.hfStart.Value = StartDate.ToShortDateString();
                this.hfEnd.Value = EndDate.ToShortDateString();
                cboLocation.DataBind();
                cboLocation.ClearSelection();
                cboLocation.Items.FindByText(Location).Selected = true;

            }
            catch
            { }
        }

    }

    protected void lnkReprint_Click(object sender, EventArgs e)
    {
        LinkButton lnkReprint = (LinkButton)sender;
        GridViewRow row = (GridViewRow)lnkReprint.NamingContainer;
        Guid UID = (Guid)GridView1.DataKeys[row.RowIndex].Value;
        bool EndOfLot = false;
        bool.TryParse(row.Cells[7].Text, out EndOfLot);
        Printing.PrintWSTicket(Server, Response, UID, EndOfLot);
    }

    protected void lnkDetails_Click(object sender, EventArgs e)
    {
        LinkButton lnkReprint = (LinkButton)sender;
        GridViewRow row = (GridViewRow)lnkReprint.NamingContainer;
        Guid UID = (Guid)GridView1.DataKeys[row.RowIndex].Value;
        string RowType = row.Cells[1].Text;
        bool Transfer = (RowType == "Transfer");

        Response.Redirect(string.Format("~/Lots/LotDetails.aspx?UID={0}", UID));

    }

    protected void txtStartDate_TextChanged(object sender, EventArgs e)
    {
        CheckDate(txtStartDate, hfStart, "LotStartDate");

    }

    private void CheckDate(TextBox sender, HiddenField hf, string SessionValue)
    {

        DateTime dt = DateTime.Now;
        if (DateTime.TryParse(sender.Text, out dt))
        {
            hf.Value = dt.ToShortDateString();
            sender.Text = dt.ToShortDateString();
            Session[SessionValue] = dt;

        }
        else
        {
            sender.Text = hf.Value;
        }
    }

    protected void txtEndDate_TextChanged(object sender, EventArgs e)
    {
        CheckDate(txtEndDate, hfEnd, "LotEndDate");
    }



    protected void cboLocation_TextChanged(object sender, EventArgs e)
    {
        Location = cboLocation.SelectedItem.Text;

    }




    protected void lnkSelect_Click(object sender, EventArgs e)
    {
        LinkButton lnkSelect = (LinkButton)sender;
        GridViewRow row = (GridViewRow)lnkSelect.NamingContainer;
        Guid UID = (Guid)GridView1.DataKeys[row.RowIndex].Value;
        using (WeightSheetDataSet.Weight_Inbound_LoadsDataTable weight_Inbound_LoadsDataTable = new WeightSheetDataSet.Weight_Inbound_LoadsDataTable())
        {
            using (WeightSheetDataSetTableAdapters.Weight_Inbound_LoadsTableAdapter weight_Inbound_LoadsTableAdapter = new WeightSheetDataSetTableAdapters.Weight_Inbound_LoadsTableAdapter())
            {
                if (weight_Inbound_LoadsTableAdapter.FillByWSUID(weight_Inbound_LoadsDataTable, WSUID) > 0)
                {
                    WeightSheetDataSet.Weight_Inbound_LoadsRow WIrow = weight_Inbound_LoadsDataTable[0];
                    WIrow.Lot_UID = UID;
                    weight_Inbound_LoadsTableAdapter.Update(weight_Inbound_LoadsDataTable);
                    Response.Redirect(string.Format("~/WeightSheets/WeightSheetDetails/WeightSheetDetails.aspx?UID={0}", WSUID));
                }
            }
        }


    }
}