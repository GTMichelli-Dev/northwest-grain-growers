using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SeedPlant_SeedPlantLots : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            if (Request.QueryString["UID"] == null)
            {
                Response.Redirect("Seed.aspx");
            }
            else
            {
                Guid UID;
                if (!Guid.TryParse(Request.QueryString["UID"].ToString(), out UID)) Response.Redirect("Seed.aspx");
                using (SeedDataSet.ItemsDataTable itemsDataTable = new SeedDataSet.ItemsDataTable())
                {
                    using (SeedDataSetTableAdapters.ItemsTableAdapter SpItemsTableAdapter = new SeedDataSetTableAdapters.ItemsTableAdapter())
                    {
                        if (SpItemsTableAdapter.FillByUID(itemsDataTable, UID) == 0)
                        {
                            Response.Redirect("Seed.aspx");
                        }
                        else
                        {
                            SeedDataSet.ItemsRow row = itemsDataTable[0];
                            lblHeader.Text = row.Description;
                        }

                    }
                }
            }
        }
    }

    protected void txtComment_TextChanged(object sender, EventArgs e)
    {
        try
        {
            TextBox txt = (TextBox)sender;
            GridViewRow row = (GridViewRow)txt.NamingContainer;
            var UID = Guid.Parse(Grd1.DataKeys[row.RowIndex].Value.ToString());
            using (SeedDataSet.Item_LocationDataTable  itemLocationDataTable = new SeedDataSet.Item_LocationDataTable())
            {
                using (SeedDataSetTableAdapters.Item_LocationTableAdapter  itemLocationTableAdapter = new SeedDataSetTableAdapters.Item_LocationTableAdapter())
                {
                    if (itemLocationTableAdapter.FillByUID(itemLocationDataTable, UID) > 0)
                    {
                        itemLocationDataTable[0].Comment = txt.Text;
                        itemLocationTableAdapter.Update(itemLocationDataTable);
                    }
                }
            }

        }
        catch
        {

        }

    }

    protected void txtLot_TextChanged(object sender, EventArgs e)
    {
        try
        {
            TextBox txt = (TextBox)sender;
            GridViewRow row = (GridViewRow)txt.NamingContainer;
            var UID = Guid.Parse(Grd1.DataKeys[row.RowIndex].Value.ToString());
            using (SeedDataSet.Item_LocationDataTable  itemLocationDataTable = new SeedDataSet.Item_LocationDataTable ())
            {
                using (SeedDataSetTableAdapters.Item_LocationTableAdapter itemLocationTableAdapter = new SeedDataSetTableAdapters.Item_LocationTableAdapter())
                {
                    if (itemLocationTableAdapter.FillByUID(itemLocationDataTable, UID) > 0)
                    {
                        if (string.IsNullOrWhiteSpace(txt.Text))
                        {
                            itemLocationDataTable[0].SetLotNull();
                        }
                        else
                        {
                            itemLocationDataTable[0].Lot = txt.Text;
                        }
                        itemLocationTableAdapter.Update(itemLocationDataTable);
                    }
                }
            }

            Response.Redirect(Request.RawUrl);
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
            var UID = Guid.Parse(Grd1.DataKeys[row.RowIndex].Value.ToString());
            using (SeedDataSet.Item_LocationDataTable  itemLocationDataTable = new SeedDataSet.Item_LocationDataTable())
            {
                using (SeedDataSetTableAdapters.Item_LocationTableAdapter itemLocationTableAdapter = new SeedDataSetTableAdapters.Item_LocationTableAdapter())
                {
                    if (itemLocationTableAdapter.FillByUID(itemLocationDataTable, UID) > 0)
                    {
                        itemLocationDataTable[0].NotInUse = !itemLocationDataTable[0].NotInUse;

                        itemLocationTableAdapter.Update(itemLocationDataTable);
                        txt.CssClass = (itemLocationDataTable[0].NotInUse == false) ? "btn btn-outline-success" : "btn btn-outline-danger";
                        txt.Text = (itemLocationDataTable[0].NotInUse == false) ? "Active" : "Inactive";

                    }
                }
            }
        }
        catch
        {
        }

    }

  

   

    protected void txtPure_Seed_TextChanged(object sender, EventArgs e)
    {
        try
        {


            TextBox txt = (TextBox)sender;
            GridViewRow row = (GridViewRow)txt.NamingContainer;
            var UID = Guid.Parse(Grd1.DataKeys[row.RowIndex].Value.ToString());

            
            using (SeedDataSet.Lot_AnalysisDataTable lot_AnalysisDataTable = new SeedDataSet.Lot_AnalysisDataTable ())
            {
                using (SeedDataSetTableAdapters.Lot_AnalysisTableAdapter lot_AnalysisTableAdapter = new SeedDataSetTableAdapters.Lot_AnalysisTableAdapter())
                {
                    
                    if (lot_AnalysisTableAdapter.FillByItem_Location_UID( lot_AnalysisDataTable, UID) > 0)
                    {
                        decimal val = lot_AnalysisDataTable[0].Pure_Seed;
                        decimal.TryParse(txt.Text, out val);
                        if (val > 100 || val < 0) val = lot_AnalysisDataTable[0].Pure_Seed;
                        lot_AnalysisDataTable[0].Pure_Seed  = val;
                        txt.Text = val.ToString();
                        lot_AnalysisTableAdapter.Update(lot_AnalysisDataTable);
                    }
                }
            }

           
        }
        catch
        {

        }
    }

    protected void txtOther_Crop_TextChanged(object sender, EventArgs e)
    {
        try
        {


            TextBox txt = (TextBox)sender;
            GridViewRow row = (GridViewRow)txt.NamingContainer;
            var UID = Guid.Parse(Grd1.DataKeys[row.RowIndex].Value.ToString());


            using (SeedDataSet.Lot_AnalysisDataTable lot_AnalysisDataTable = new SeedDataSet.Lot_AnalysisDataTable())
            {
                using (SeedDataSetTableAdapters.Lot_AnalysisTableAdapter lot_AnalysisTableAdapter = new SeedDataSetTableAdapters.Lot_AnalysisTableAdapter())
                {

                    if (lot_AnalysisTableAdapter.FillByItem_Location_UID(lot_AnalysisDataTable, UID) > 0)
                    {
                        decimal val = lot_AnalysisDataTable[0].Other_Crop;
                        decimal.TryParse(txt.Text, out val);
                        if (val > 100 || val < 0) val = lot_AnalysisDataTable[0].Other_Crop;
                        lot_AnalysisDataTable[0].Other_Crop = val;
                        txt.Text = val.ToString();
                        lot_AnalysisTableAdapter.Update(lot_AnalysisDataTable);
                    }
                }
            }


        }
        catch
        {

        }
    }

    protected void txtInert_Matter_TextChanged(object sender, EventArgs e)
    {
        try
        {


            TextBox txt = (TextBox)sender;
            GridViewRow row = (GridViewRow)txt.NamingContainer;
            var UID = Guid.Parse(Grd1.DataKeys[row.RowIndex].Value.ToString());


            using (SeedDataSet.Lot_AnalysisDataTable lot_AnalysisDataTable = new SeedDataSet.Lot_AnalysisDataTable())
            {
                using (SeedDataSetTableAdapters.Lot_AnalysisTableAdapter lot_AnalysisTableAdapter = new SeedDataSetTableAdapters.Lot_AnalysisTableAdapter())
                {

                    if (lot_AnalysisTableAdapter.FillByItem_Location_UID(lot_AnalysisDataTable, UID) > 0)
                    {
                        decimal val = lot_AnalysisDataTable[0].Inert_Matter;
                        decimal.TryParse(txt.Text, out val);
                        if (val > 100 || val < 0) val = lot_AnalysisDataTable[0].Inert_Matter;
                        lot_AnalysisDataTable[0].Inert_Matter = val;
                        txt.Text = val.ToString();
                        lot_AnalysisTableAdapter.Update(lot_AnalysisDataTable);
                    }
                }
            }


        }
        catch
        {

        }
    }

    protected void txtGermination_TextChanged(object sender, EventArgs e)
    {
        try
        {


            TextBox txt = (TextBox)sender;
            GridViewRow row = (GridViewRow)txt.NamingContainer;
            var UID = Guid.Parse(Grd1.DataKeys[row.RowIndex].Value.ToString());


            using (SeedDataSet.Lot_AnalysisDataTable lot_AnalysisDataTable = new SeedDataSet.Lot_AnalysisDataTable())
            {
                using (SeedDataSetTableAdapters.Lot_AnalysisTableAdapter lot_AnalysisTableAdapter = new SeedDataSetTableAdapters.Lot_AnalysisTableAdapter())
                {

                    if (lot_AnalysisTableAdapter.FillByItem_Location_UID(lot_AnalysisDataTable, UID) > 0)
                    {
                        decimal val = lot_AnalysisDataTable[0].Germination;
                        decimal.TryParse(txt.Text, out val);
                        if (val > 100 || val < 0) val = lot_AnalysisDataTable[0].Germination;
                        lot_AnalysisDataTable[0].Germination = val;
                        txt.Text = val.ToString();
                        lot_AnalysisTableAdapter.Update(lot_AnalysisDataTable);
                    }
                }
            }


        }
        catch
        {

        }
    }

    protected void txtWeed_Seed_TextChanged(object sender, EventArgs e)
    {
        try
        {


            TextBox txt = (TextBox)sender;
            GridViewRow row = (GridViewRow)txt.NamingContainer;
            var UID = Guid.Parse(Grd1.DataKeys[row.RowIndex].Value.ToString());


            using (SeedDataSet.Lot_AnalysisDataTable lot_AnalysisDataTable = new SeedDataSet.Lot_AnalysisDataTable())
            {
                using (SeedDataSetTableAdapters.Lot_AnalysisTableAdapter lot_AnalysisTableAdapter = new SeedDataSetTableAdapters.Lot_AnalysisTableAdapter())
                {

                    if (lot_AnalysisTableAdapter.FillByItem_Location_UID(lot_AnalysisDataTable, UID) > 0)
                    {
                        decimal val = lot_AnalysisDataTable[0].Weed_Seed; 
                        decimal.TryParse(txt.Text, out val);
                        if (val>100 ||val<0) val = lot_AnalysisDataTable[0].Weed_Seed;
                        lot_AnalysisDataTable[0].Weed_Seed = val;
                        txt.Text = val.ToString();
                        lot_AnalysisTableAdapter.Update(lot_AnalysisDataTable);
                    }
                }
            }


        }
        catch
        {

        }
    }

    protected void txtDate_Tested_TextChanged(object sender, EventArgs e)
    {
        try
        {


            TextBox txt = (TextBox)sender;
            GridViewRow row = (GridViewRow)txt.NamingContainer;
            var UID = Guid.Parse(Grd1.DataKeys[row.RowIndex].Value.ToString());


            using (SeedDataSet.Lot_AnalysisDataTable lot_AnalysisDataTable = new SeedDataSet.Lot_AnalysisDataTable())
            {
                using (SeedDataSetTableAdapters.Lot_AnalysisTableAdapter lot_AnalysisTableAdapter = new SeedDataSetTableAdapters.Lot_AnalysisTableAdapter())
                {

                    if (lot_AnalysisTableAdapter.FillByItem_Location_UID(lot_AnalysisDataTable, UID) > 0)
                    {
                        DateTime val  ;
                        if (DateTime.TryParse(txt.Text, out val))
                        {
                            lot_AnalysisDataTable[0].Date_Tested  = val;
                            txt.Text = val.ToShortDateString();
                            lot_AnalysisTableAdapter.Update(lot_AnalysisDataTable);
                        }
                        else
                        {
                            if (lot_AnalysisDataTable[0].IsDate_TestedNull())
                            {
                                txt.Text = "";

                            }
                            else
                            {
                        
                                txt.Text = lot_AnalysisDataTable[0].Date_Tested.ToShortDateString();

                            }

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