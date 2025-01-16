using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Variety_lots : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {







        if (!this.IsPostBack)
        {
            this.ddLocation.DataBind();
            HttpCookie LocationCookie = Request.Cookies["LocationCookie"];
            string LocationId = "24";
            // Read the cookie information and display it.

            if (LocationCookie != null)
            {

                LocationId = LocationCookie.Value;
            }
            ddLocation.Items.FindByValue(LocationId.ToString()).Selected = true;

        }
    }


    private void SaveLocation()
    {
        HttpCookie LocationCookie = new HttpCookie("LocationCookie");
        DateTime now = DateTime.Now;


        LocationCookie.Value = ddLocation.SelectedValue;
        // Set the cookie expiration date.
        LocationCookie.Expires = now.AddYears(50); // For a cookie to effectively never expire

        // Add the cookie.
        Response.Cookies.Add(LocationCookie);

    }

    protected void btnNewItemSave_Click(object sender, EventArgs e)
    {
        bool Save = false;
        if (ddSelectClass.SelectedIndex < 1)
        {
            lblNewItemError.Text = "Select A Class";
        }
        else if (ddSelectCrop.SelectedIndex < 1)
        {
            lblNewItemError.Text = "Select A Crop";
        }
        else if (ddSelectVariety.SelectedIndex < 1)
        {
            lblNewItemError.Text = "Select A Variety";
        }
        else if (string.IsNullOrEmpty(txtNewLot.Text))
        {
            lblNewItemError.Text = "Lot Cannot Be Blank";
        }
        else
        {
            using (NWDataSet.Seed_Class_LotDataTable seed_Class_LotDataTable = new NWDataSet.Seed_Class_LotDataTable())
            {
                using (NWDataSetTableAdapters.Seed_Class_LotTableAdapter seed_Class_LotTableAdapter = new NWDataSetTableAdapters.Seed_Class_LotTableAdapter())
                {
                    int Location;
                    if (int.TryParse(ddLocation.SelectedValue, out Location))
                    {
                        if (seed_Class_LotTableAdapter.FillByLocation_Lot(seed_Class_LotDataTable, Location, txtNewLot.Text) > 0)
                        {
                            lblNewItemError.Text = "Lot Already Exist For Location";
                        }
                        else
                        {
                            NWDataSet.Seed_Class_LotRow row = seed_Class_LotDataTable.NewSeed_Class_LotRow();
                            row.UID = Guid.NewGuid();
                            row.Variety_UID = Guid.Parse(ddSelectVariety.SelectedValue);
                            row.Class_UID = Guid.Parse(ddSelectClass.SelectedValue);
                            row.Location_Id = Location;
                            row.Active = true;
                            row.Lot = txtNewLot.Text;
                            seed_Class_LotDataTable.AddSeed_Class_LotRow(row);
                            seed_Class_LotTableAdapter.Update(seed_Class_LotDataTable);
                            this.GridView1.DataBind();
                            Save = true;
                        }
                    }
                }
            }
        }
        if (!Save) this.pnlNewItemPopupExtender1.Show();
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {

        this.lblCurLocation.Text = ddLocation.SelectedItem.Text;
        this.ddSelectClass.SelectedIndex = 0;
        ddSelectCrop.SelectedIndex = 0;
        if (ddSelectVariety.Items.Count > 0) ddSelectVariety.SelectedIndex = 0;
        txtNewLot.Text = "";
        this.lblNewItemError.Text = "";
        pnlNewItemPopupExtender1.Show();
    }





    protected void ddSelectCrop_SelectedIndexChanged(object sender, EventArgs e)
    {
        pnlNewItemPopupExtender1.Show();
    }

    protected void ddLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        SaveLocation();
    }

    protected void txtDateTested_TextChanged(object sender, EventArgs e)
    {
        TextBox txtDateTested = (TextBox)sender;
        GridViewRow row = (GridViewRow)txtDateTested.NamingContainer;
        Guid UID = (Guid)GridView1.DataKeys[row.RowIndex].Value;
        using (NWDataSet.Seed_Class_LotDataTable seed_Class_LotDataTable = new NWDataSet.Seed_Class_LotDataTable())
        {
            using (NWDataSetTableAdapters.Seed_Class_LotTableAdapter seed_Class_LotTableAdapter = new NWDataSetTableAdapters.Seed_Class_LotTableAdapter())
            {
                seed_Class_LotTableAdapter.FillByUID(seed_Class_LotDataTable, UID);
                NWDataSet.Seed_Class_LotRow lotrow = seed_Class_LotDataTable[0];
                if (!string.IsNullOrEmpty(txtDateTested.Text))
                {
                    DateTime dt;
                    if (DateTime.TryParse(txtDateTested.Text, out dt))
                    {
                        lotrow.Date_Tested = dt;
                        txtDateTested.Text = lotrow.Date_Tested.ToShortDateString();
                    }
                    else
                    {
                        if (!lotrow.IsDate_TestedNull())
                        {
                            txtDateTested.Text = lotrow.Date_Tested.ToShortDateString();
                        }
                        else
                        {
                            txtDateTested.Text = "";
                        }
                    }
                }
                else
                {
                    lotrow.SetDate_TestedNull();
                }
                seed_Class_LotTableAdapter.Update(seed_Class_LotDataTable);

            }

        }





    }

    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void txtLot_TextChanged(object sender, EventArgs e)
    {
        TextBox txtLot = (TextBox)sender;
        GridViewRow row = (GridViewRow)txtLot.NamingContainer;
        Guid UID = (Guid)GridView1.DataKeys[row.RowIndex].Value;
        using (NWDataSet.Seed_Class_LotDataTable seed_Class_LotDataTable = new NWDataSet.Seed_Class_LotDataTable())
        {
            using (NWDataSetTableAdapters.Seed_Class_LotTableAdapter seed_Class_LotTableAdapter = new NWDataSetTableAdapters.Seed_Class_LotTableAdapter())
            {
                using (NWDataSet.Seed_Class_LotDataTable seedTest_Class_LotDataTable = new NWDataSet.Seed_Class_LotDataTable())

                {
                    int LocationId = int.Parse(ddLocation.SelectedValue);
                    seed_Class_LotTableAdapter.FillByUID(seed_Class_LotDataTable, UID);
                    if (seed_Class_LotTableAdapter.FillByLocation_Lot(seedTest_Class_LotDataTable, LocationId, txtLot.Text) > 0 && seedTest_Class_LotDataTable[0].UID != UID)
                    {

                        txtLot.Text = seed_Class_LotDataTable[0].Lot;
                        lblErrMsg.Text = "Lot Already Exists For Location";
                        ModalPopupErrMsg.Show();
                    }
                    else
                    {


                        if (string.IsNullOrEmpty(txtLot.Text))
                        {
                            txtLot.Text = seed_Class_LotDataTable[0].Lot;
                        }
                        else
                        {
                            seed_Class_LotDataTable[0].Lot = txtLot.Text;
                            seed_Class_LotTableAdapter.Update(seed_Class_LotDataTable);
                        }


                    }
                }
            }
        }
    }


    protected void ckActive_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox ckActive = (CheckBox)sender;
        GridViewRow row = (GridViewRow)ckActive.NamingContainer;
        Guid UID = (Guid)GridView1.DataKeys[row.RowIndex].Value;
        using (NWDataSet.Seed_Class_LotDataTable seed_Class_LotDataTable = new NWDataSet.Seed_Class_LotDataTable())
        {
            using (NWDataSetTableAdapters.Seed_Class_LotTableAdapter seed_Class_LotTableAdapter = new NWDataSetTableAdapters.Seed_Class_LotTableAdapter())
            {
                seed_Class_LotTableAdapter.FillByUID(seed_Class_LotDataTable, UID);
                seed_Class_LotDataTable[0].Active = ckActive.Checked;
                seed_Class_LotTableAdapter.Update(seed_Class_LotDataTable);
            }
        }
    }
}