using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Setup : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            try
            {
                this.ddLocation.DataBind();
                SetLocation();
                ddPrinter.Items.Add("Not Set");
                for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
                {
                    ddPrinter.Items.Add(PrinterSettings.InstalledPrinters[i]);
                }
              

                    ListItem li = ddPrinter.Items.FindByText(GlobalVars.ReportPrinter );
                    if (li != null)
                    {
                        ddPrinter.ClearSelection();
                        li.Selected = true;
                    }
                    else
                {
                    ddPrinter.SelectedIndex = 0;
                }
              
                
            }
            catch { }
        }
        pnlPrinter.Visible = (GlobalVars.Location > 0);
    }

    public void SetLocation()
    {
        ddLocation.ClearSelection();
        ddLocation.Items.FindByValue(GlobalVars.Location.ToString()).Selected = true;
    }

    protected void ddLocation_TextChanged(object sender, EventArgs e)
    {

        int NewValue;
        if (int.TryParse(ddLocation.SelectedItem.Value, out NewValue))
        {
            GlobalVars.Location = NewValue;

        }
        else
        {
            GlobalVars.Location = 0;
        
        }
        Response.Redirect(Request.RawUrl);
    }







    protected void ddPrinter_TextChanged(object sender, EventArgs e)
    {
        if (ddPrinter.SelectedIndex > -1)
        {
            GlobalVars.ReportPrinter = ddPrinter.Text; 
        }
        else
        {
            GlobalVars.ReportPrinter = string.Empty;
        }

    }
}