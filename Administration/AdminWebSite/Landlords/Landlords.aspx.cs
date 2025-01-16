using System;
using System.Linq;
using System.Web.UI;

public partial class Landlords_Landlords : System.Web.UI.Page
{
    protected override void Render(HtmlTextWriter writer)
    {
        ClientScript.RegisterForEventValidation(btnDeleteAll.UniqueID, "deleteAll");
        base.Render(writer);
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (Security.GetUsersSecurityLevel(Session) == Security.enumSecurityLevel.View)
        {
            Response.Redirect("~/Default.aspx");
        }

        if (!IsPostBack)
        {
            BindLandlords();
        }
        else
        {
            string eventTarget = Request["__EVENTTARGET"];
            string eventArgument = Request["__EVENTARGUMENT"];

            if (eventTarget == btnDeleteAll.UniqueID && eventArgument == "deleteAll")
            {
                DeleteAllLandlords();
            }
        }

        
    }

    private void BindLandlords()
    {
        using (var context = new NWDataModel())
        {
            var landlords = context.Landlords.OrderBy(x => x.Description).ToList();
            gvLandlords.DataSource = landlords;
            gvLandlords.DataBind();
        }
    }

    private void DeleteAllLandlords()
    {
        using (var context = new NWDataModel())
        {
            var landlords = context.Landlords.ToList();
            context.Landlords.RemoveRange(landlords);
            context.SaveChanges();
        }

        // Redirect to Default.aspx
        Response.Redirect("~/Default.aspx");
    }
}
