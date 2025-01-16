using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class SeedPlant_Lots : System.Web.UI.Page
{

    public string Location
    {

        get
        {
            string lc = "All Locations";
            if (Session["SeedLocation"] != null)
            {
                lc = Session["SeedLocation"].ToString();
            }
            return lc;
        }
        set
        {

            Session["SeedLocation"] = value;

        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            try
            {
                if (Request.QueryString["Commodity_ID"] != null)
                {
                    int Commodity_Id;
                    if (int.TryParse(Request.QueryString["Commodity_ID"].ToString(), out Commodity_Id))
                    {
                        ddCommodity.DataBind();
                        ddCommodity.ClearSelection();
                        ddCommodity.Items.FindByValue(Request.QueryString["Commodity_ID"].ToString()).Selected = true;
                    }
                }

                cboLocation.DataBind();
                cboLocation.ClearSelection();
                cboLocation.Items.FindByText(Location).Selected = true;



            }
            catch
            { }
        }
        if (cboLocation.SelectedIndex > 0)
        {
            GridView1.Visible = true;
            GridView2.Visible = false;
        }
        else   
        {
            GridView1.Visible = false;
            GridView2.Visible = true;


        }
 
        


    }





    protected void cboLocation_TextChanged(object sender, EventArgs e)
    {
        Location = cboLocation.SelectedItem.Text;
    }


    protected void txtComment_TextChanged(object sender, EventArgs e)
    {



        try
        {

            TextBox txt = (TextBox)sender;
            GridViewRow row = (GridViewRow)txt.NamingContainer;
            if (cboLocation.SelectedIndex < 1)
            {
                var UID = Guid.Parse(GridView1.DataKeys[row.RowIndex].Values[0].ToString());

                using (SeedDataSet.ItemsDataTable itemsDataTable = new SeedDataSet.ItemsDataTable())
                {
                    using (SeedDataSetTableAdapters.ItemsTableAdapter itemsTableAdapter = new SeedDataSetTableAdapters.ItemsTableAdapter())
                    {
                        if (itemsTableAdapter.FillByUID(itemsDataTable, UID) > 0)
                        {
                            itemsDataTable[0].Comment = txt.Text;


                            itemsTableAdapter.Update(itemsDataTable);



                        }
                    }
                }
            }
            else
            {
                int Location_Id = int.Parse(cboLocation.SelectedValue);
                var UID = Guid.Parse(GridView1.DataKeys[row.RowIndex].Values[0].ToString());
                using (SeedDataSetTableAdapters.Item_LocationTableAdapter item_LocationTableAdapter = new SeedDataSetTableAdapters.Item_LocationTableAdapter())
                {
                    using (SeedDataSet.Item_LocationDataTable item_LocationDataTable = new SeedDataSet.Item_LocationDataTable())
                    {
                        if (item_LocationTableAdapter.FillByUIDLocationID(item_LocationDataTable, UID, Location_Id) > 0)
                        {
                            item_LocationDataTable[0].Comment = txt.Text;
                            item_LocationTableAdapter.Update(item_LocationDataTable);
                        }
                    }
                }


            }

        }
        catch
        {
        }



    }

    protected void btnInUse_Click(object sender, EventArgs e)
    {
        try
        {

            Button txt = (Button)sender;
            GridViewRow row = (GridViewRow)txt.NamingContainer;
            if (cboLocation.SelectedIndex < 1)
            {
                var UID = Guid.Parse(GridView1.DataKeys[row.RowIndex].Values["UID"].ToString());

                using (SeedDataSet.ItemsDataTable itemsDataTable = new SeedDataSet.ItemsDataTable())
                {
                    using (SeedDataSetTableAdapters.ItemsTableAdapter itemsTableAdapter = new SeedDataSetTableAdapters.ItemsTableAdapter())
                    {
                        if (itemsTableAdapter.FillByUID(itemsDataTable, UID) > 0)
                        {
                            itemsDataTable[0].NotInUse = !itemsDataTable[0].NotInUse;


                            itemsTableAdapter.Update(itemsDataTable);
                            txt.CssClass = (itemsDataTable[0].NotInUse == false) ? "btn btn-outline-success" : "btn btn-outline-danger";
                            txt.Text = (itemsDataTable[0].NotInUse == false) ? "Active" : "Inactive";


                        }
                    }
                }
            }
            else
            {
                int Location_Id = int.Parse(cboLocation.SelectedValue);
                var UID = Guid.Parse(GridView1.DataKeys[row.RowIndex].Values["ItemLocation_UID"].ToString());
                using (SeedDataSetTableAdapters.Item_LocationTableAdapter item_LocationTableAdapter = new SeedDataSetTableAdapters.Item_LocationTableAdapter())
                {
                    using (SeedDataSet.Item_LocationDataTable item_LocationDataTable = new SeedDataSet.Item_LocationDataTable())
                    {
                        if (item_LocationTableAdapter.FillByUID(item_LocationDataTable, UID) > 0)
                        {
                            item_LocationDataTable[0].NotInUse = !item_LocationDataTable[0].NotInUse;

                            item_LocationTableAdapter.Update(item_LocationDataTable);
                            txt.CssClass = (item_LocationDataTable[0].NotInUse == false) ? "btn btn-outline-success" : "btn btn-outline-danger";
                            txt.Text = (item_LocationDataTable[0].NotInUse == false) ? "Active" : "Inactive";
                            item_LocationTableAdapter.Update(item_LocationDataTable);

                        }
                    }
                }


            }

        }
        catch
        {
        }

    }





    protected void SqlSeedPlantItems_DataBinding(object sender, EventArgs e)
    {

    }



}