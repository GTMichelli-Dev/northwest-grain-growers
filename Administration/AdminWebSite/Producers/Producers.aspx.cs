using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Producers_Producers : System.Web.UI.Page
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

    public int Producer_Id
    {

        get
        {
            int ID = -1;
            if (Request.QueryString["Producer_Id"] != null) int.TryParse(Request.QueryString["Producer_Id"].ToString(), out ID);
            return ID;
        }
    }


    public string Current_Producer
    {

        get
        {
            string CProducer = string.Empty;
            if (Request.QueryString["Producer"] != null) CProducer = Request.QueryString["Producer"].ToString();
            return CProducer;
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

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            //  if (Producer_Id >= 0) txtFilter.Text = Producer_Id.ToString();
            if (!string.IsNullOrWhiteSpace(Current_Producer) && !string.IsNullOrWhiteSpace(Lot_Number))
            {

                lblHeader.Text = "Update Grower For Lot: <strong>" + Lot_Number + "</strong></br> Current Grower: <strong>" + Current_Producer+"</strong>" ;
            }
          //  txtFilter.Text = (Producer_Id >= 0) ? Producer_Id.ToString() : "";
            GridView1.Columns[0].Visible = !string.IsNullOrWhiteSpace(Lot_Number);
            //GridView1.Rows[0].Visible = !string.IsNullOrWhiteSpace(Lot_Number);
            if (!string.IsNullOrWhiteSpace(Lot_Number)) this.SqlProducers.SelectParameters[1].DefaultValue = true.ToString();


            //this.SqlProducers.DataBind();
            //GridView1.DataBind();
            //if (GridView1.Rows.Count == 0) txtFilter.Text = "";

        }

    }

   

    protected void PrintWS_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox ck = (CheckBox)sender;
        GridViewRow row = (GridViewRow)ck.NamingContainer;
        var UID = (Guid)GridView1.DataKeys[row.RowIndex].Value;
        using (NWDataSetTableAdapters.QueriesTableAdapter Q = new NWDataSetTableAdapters.QueriesTableAdapter())
        {
            Q.UpdateProducerPrintWS(ck.Checked, UID);
        }
    }

    protected void EmailWS_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox ck = (CheckBox)sender;
        GridViewRow row = (GridViewRow)ck.NamingContainer;
        var UID = (Guid)GridView1.DataKeys[row.RowIndex].Value;
        using (NWDataSetTableAdapters.QueriesTableAdapter Q = new NWDataSetTableAdapters.QueriesTableAdapter())
        {
            Q.UpdateProducerEmailWS(ck.Checked, UID);
        }

    }

    protected void lnkSelect_Click(object sender, EventArgs e)
    {
        LinkButton  lb = (LinkButton)sender;
        GridViewRow row = (GridViewRow)lb.NamingContainer;
        var UID = (Guid)GridView1.DataKeys[row.RowIndex].Value;
        using (ProducersDataSetTableAdapters.ProducersTableAdapter producersTableAdapter = new ProducersDataSetTableAdapters.ProducersTableAdapter())
        {
            using (ProducersDataSet.ProducersDataTable producersDataTable = new ProducersDataSet.ProducersDataTable())
            {
                if (producersTableAdapter.FillByUID(producersDataTable, UID) > 0)
                {
                    using (LotsDataSet.LotsDataTable lotsDataTable= new LotsDataSet.LotsDataTable())
                        {
                        using (LotsDataSetTableAdapters.LotsTableAdapter lotsTableAdapter = new LotsDataSetTableAdapters.LotsTableAdapter())
                        {
                            if (lotsTableAdapter.FillByUID(lotsDataTable,LotUID  )>0)
                            {
                                Audit.AddAuditTrail("Changed Lot Producer", lotsDataTable[0].Location_Id, lotsDataTable[0].Lot_Number.ToString(), "Lot Producer_Id Changed From Office", lotsDataTable[0].Producer_Id.ToString(),producersDataTable[0].Id.ToString());
                                Audit.AddAuditTrail("Changed Lot Producer Description", lotsDataTable[0].Location_Id, lotsDataTable[0].Lot_Number.ToString(), "Lot Producer DEscription Changed From Office", lotsDataTable[0].Producer_Description.ToString(), producersDataTable[0].Description.ToString());

                                lotsDataTable[0].Producer_Id = producersDataTable[0].Id;
                                lotsDataTable[0].Producer_Description = producersDataTable[0].Description;
                                lotsTableAdapter.Update(lotsDataTable);
                            }
                                Response.Redirect("~/Lots/LotDetails.aspx?UID=" + LotUID.ToString());
                        }
                    }
                }
            }
        }
    }
}