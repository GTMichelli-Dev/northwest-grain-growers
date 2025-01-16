using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SeedPlant_Chemicals : System.Web.UI.Page
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
     
                cboLocation.DataBind();
                cboLocation.ClearSelection();
                cboLocation.Items.FindByText(Location).Selected = true;
               
            }
            catch
            { }
        }
        GridView1.Columns[2].Visible = cboLocation.SelectedIndex > 0;
    }


    protected void cboLocation_TextChanged(object sender, EventArgs e)
    {
        Location = cboLocation.SelectedItem.Text;

    }

    protected void txtComment_TextChanged(object sender, EventArgs e)
    {



        try
        {

            TextBox  txt = (TextBox)sender;
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
                var UID = Guid.Parse(GridView1.DataKeys[row.RowIndex].Values[1].ToString());
                using (SeedDataSetTableAdapters.Item_LocationTableAdapter item_LocationTableAdapter = new SeedDataSetTableAdapters.Item_LocationTableAdapter())
                {
                    using (SeedDataSet.Item_LocationDataTable item_LocationDataTable = new SeedDataSet.Item_LocationDataTable())
                    {
                        if (item_LocationTableAdapter.FillByUID(item_LocationDataTable, UID) > 0)
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
            if (cboLocation.SelectedIndex<1)
            {
                var UID = Guid.Parse(GridView1.DataKeys[row.RowIndex].Values[0].ToString());

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
                            itemsTableAdapter.Update(itemsDataTable);

                        }
                    }
                }
            }
            else
            {
                var UID = Guid.Parse(GridView1.DataKeys[row.RowIndex].Values[1].ToString());
                using (SeedDataSetTableAdapters.Item_LocationTableAdapter item_LocationTableAdapter = new SeedDataSetTableAdapters.Item_LocationTableAdapter())
                {
                    using (SeedDataSet.Item_LocationDataTable item_LocationDataTable = new SeedDataSet.Item_LocationDataTable())
                    {
                        if (item_LocationTableAdapter.FillByUID(item_LocationDataTable,UID)>0)
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
}