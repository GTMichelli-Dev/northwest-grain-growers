using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Variety : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void txtDescription_TextChanged(object sender, EventArgs e)
    {
        //TextBox txtDescription = (TextBox)sender;
        //GridViewRow row = (GridViewRow)txtDescription.NamingContainer;
        //Guid UID = (Guid)GridView1.DataKeys[row.RowIndex].Value;
        //using (NWDataSetTableAdapters.VarietyTableAdapter varietyTableAdapter = new NWDataSetTableAdapters.VarietyTableAdapter())
        //{
        //    using (NWDataSet.VarietyDataTable varietyDataTable = new NWDataSet.VarietyDataTable())
        //    {
        //        if (varietyTableAdapter.FillByUID (varietyDataTable, txtDescription.Text) > 0 && varietyDataTable[0].UID != UID)
        //        {

        //            ModalPopupErrMsg.Show();
        //        }
        //        else
        //        {
        //            varietyTableAdapter.FillByUID(varietyDataTable, UID);
        //            varietyDataTable[0].Description = txtDescription.Text;
        //            varietyTableAdapter.Update(varietyDataTable);
        //            using (NWDataSetTableAdapters.QueriesTableAdapter Q = new NWDataSetTableAdapters.QueriesTableAdapter())
        //            {
        //                Q.Update_Seed_Variety_Defaults(24);
        //            }
        //        }
                
        //    }
        //}
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        txtNewBreeder.Text = "";
        txtNewID.Text = "";
        txtNewTrait.Text = "";
        ddSelectCrop.SelectedIndex = 0;
        txtNewVariety.Text = "";
        lblNewItemError.Text = "";
        pnlNewItemPopupExtender1.Show();
    }

    protected void btnNewItemSave_Click(object sender, EventArgs e)
    {
        bool ShowPopup = true;
        string Variety = txtNewVariety.Text;
        int Id;
        if (ddSelectCrop.SelectedIndex<1)
        {
            lblNewItemError.Text = "Crop cannot Be Blank";
        }
        else if (string.IsNullOrEmpty(Variety))
        {
            lblNewItemError.Text = "Variety cannot Be Blank";
        }
        else if (!int.TryParse(txtNewID.Text, out Id))
        {
            
            lblNewItemError.Text = "Id Must Be A Number";
        }
        else
        {
            

            //using (NWDataSetTableAdapters.VarietyTableAdapter varietyTableAdapter = new NWDataSetTableAdapters.VarietyTableAdapter())
            //{
            //    using (NWDataSet.VarietyDataTable varietyDataTable = new NWDataSet.VarietyDataTable())
            //    {
            //        if (varietyTableAdapter.FillByDescription(varietyDataTable,Variety )>0 )
            //        {
            //            lblNewItemError.Text = "Variety already exists";
            //        }
            //        else if (varietyTableAdapter.FillByIdCrop (varietyDataTable, Id , ddSelectCrop.SelectedItem.Text) > 0)
            //        {
            //            lblNewItemError.Text = "Id already exists for crop";
            //        }
            //        else
            //        {
            //            using (NWDataSetTableAdapters.QueriesTableAdapter Q = new NWDataSetTableAdapters.QueriesTableAdapter())
            //            {
            //                Q.Create_New_Seed_Variety(24, ddSelectCrop.SelectedItem.Text , Id, Variety, txtNewBreeder.Text, txtNewTrait.Text,"");
            //                ShowPopup = false;
            //                this.DataBind();
            //            }
            //        }
            //    }
            //}
        }
        if (ShowPopup) pnlNewItemPopupExtender1.Show();
    }

    protected void btndelete_Click(object sender, EventArgs e)
    {
        LinkButton btndelete = (LinkButton)sender;
        GridViewRow row = (GridViewRow)btndelete.NamingContainer;
        Guid UID = (Guid)GridView1.DataKeys[row.RowIndex].Value;
        using (NWDataSet.VarietyDataTable varietyDataTable = new NWDataSet.VarietyDataTable())
        {
            using (NWDataSetTableAdapters.VarietyTableAdapter varietyTableAdapter = new NWDataSetTableAdapters.VarietyTableAdapter())
            {
                varietyTableAdapter.Delete(UID);
               
                this.DataBind();
            }
        }
    }
}