using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SeedPlant_SeedLots : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void txtLot_TextChanged(object sender, EventArgs e)
    {
        TextBox  textBox = (TextBox)sender;
        GridViewRow row = (GridViewRow)textBox.NamingContainer;
        var UID = (Guid)grdLots.DataKeys[row.RowIndex].Value;
        using (SeedDataSetTableAdapters.QueriesTableAdapter Q = new SeedDataSetTableAdapters.QueriesTableAdapter())
        {

            if (string.IsNullOrEmpty(textBox.Text))
            {
                Q.UpdateSeedVarietyLot(null, UID);
            }
            else
            {
                Q.UpdateSeedVarietyLot(textBox.Text, UID);
            }
        }
    }
}