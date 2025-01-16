using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CreateNewSite : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        //LinkButton lnkSender
        //GridViewRow row = (GridViewRow)ddScaleType.NamingContainer;
        //int index = row.RowIndex;
        //Guid UID = (Guid)this.GridView1.DataKeys[index].Value;
        //try
        //{
        //    Q.Update_Scale_Type(ddScaleType.SelectedValue, UID);
        //    Scales.UpdateScales();
        //    Scales.LastScaleModification = DateTime.Now;
        //    Update_Button();
        //}
        //catch
        //{

        //}
        //GridView1.DataBind();
    }

    protected void lnkSelect_Click(object sender, EventArgs e)
    {
        LinkButton lnkSelect = (LinkButton)sender;
        GridViewRow row = (GridViewRow)lnkSelect.NamingContainer;
        int index = row.RowIndex;
        Label lblID = (Label)row.FindControl("lblID");
        HiddenField  hfSequenceID = (HiddenField)row.FindControl("hfSequenceID");
        try
        {
            int Location_Id, Sequence_ID;

            if ((int.TryParse(lblID.Text, out Location_Id)) && (int.TryParse(hfSequenceID.Value, out Sequence_ID)))
                {
                using (LocalDataSetTableAdapters.QueriesTableAdapter Q = new LocalDataSetTableAdapters.QueriesTableAdapter())
                {
                    Q.AddNewSite(Location_Id, Sequence_ID);
                }


            }
        }
        catch
        {

        }
        GridView1.DataBind();
    }
}