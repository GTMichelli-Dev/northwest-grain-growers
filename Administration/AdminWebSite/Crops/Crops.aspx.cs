using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Crops_Crops : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Security.GetUsersSecurityLevel(Session) == Security.enumSecurityLevel.View) Response.Redirect("~/Default.aspx");
    }

    protected void Use_At_Elevator_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox ck = (CheckBox)sender;
        GridViewRow row = (GridViewRow)ck.NamingContainer;
        var UID = (Guid)GridView1.DataKeys[row.RowIndex].Value;
        using (NWDataSetTableAdapters.QueriesTableAdapter Q = new NWDataSetTableAdapters.QueriesTableAdapter())
        {
            Q.UpdatecropUseAtElevator(ck.Checked, UID);
            
        }
    }

    protected void Use_At_Seed_Mill_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox ck = (CheckBox)sender;
        GridViewRow row = (GridViewRow)ck.NamingContainer;
        var UID = (Guid)GridView1.DataKeys[row.RowIndex].Value;
        using (NWDataSetTableAdapters.QueriesTableAdapter Q = new NWDataSetTableAdapters.QueriesTableAdapter())
        {
            Q.UpdatecropUseAsSeed(ck.Checked, UID);
        }
    }
}