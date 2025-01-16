using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reprint : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            Printing.UpdatePrinters();
        }
        Label lblHeader = (Label)UpdateProgress1.FindControl("lblHeader");
        lblHeader.Text = hfLabel.Value ;


    }




    protected void lnkRecievingPrint_Click(object sender, EventArgs e)
    {
        LinkButton lnkRecievingPrint = (LinkButton)sender;
        GridViewRow row = (GridViewRow)lnkRecievingPrint.NamingContainer;
        Guid UID = (Guid)grdRecieving.DataKeys[row.RowIndex].Value;
        HiddenField lfRLoadID = (HiddenField)row.FindControl("lfRLoadID");
        hfLabel.Value  = "Reprinting Recieving Ticket For Load " + lfRLoadID.Value;
        hfOneShot.Value = "1";
        HiddenField HFRLocationID = (HiddenField)row.FindControl("HFRLocationID");
        hfLocationID.Value = HFRLocationID.Value;
        hfUID.Value = UID.ToString();
        tmrPrintRecieving.Enabled = true;
      

    }

    protected void lnkTransfersPrint_Click(object sender, EventArgs e)
    {
        LinkButton lnkTransfersPrint = (LinkButton)sender;
        GridViewRow row = (GridViewRow)lnkTransfersPrint.NamingContainer;
        Guid UID = (Guid)grdtransfer.DataKeys[row.RowIndex].Value;
        HiddenField lfTLoadID = (HiddenField)row.FindControl("lfTLoadID");
        
        hfLabel.Value = "Reprinting Trasnsfer Ticket For Load " + lfTLoadID.Value;
        hfOneShot.Value = "1";
        HiddenField HFTLocationID = (HiddenField)row.FindControl("HFTLocationID");
        hfLocationID.Value = HFTLocationID.Value;
        hfUID.Value = UID.ToString() ;
        tmrPrintTransfer.Enabled = true;
      
    }


    protected void tmrPrintRecieving_Tick(object sender, EventArgs e)
    {
        tmrPrintRecieving.Enabled = false;
        if (hfOneShot.Value == "1")
        {
            hfOneShot.Value = "0";
            Guid UID = Guid.Parse(hfUID.Value.ToString());
            int LocationID = int.Parse(hfLocationID.Value);
            Printing.PrintInbound_Final_Ticket(Server, UID, LocationID, "", ddPrinter.Text);
        }
    }

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        tmrPrintTransfer.Enabled = false;
        if (hfOneShot.Value == "1")
        {
            hfOneShot.Value = "0";
            Guid UID = Guid.Parse(hfUID.Value.ToString());
            int LocationID = int.Parse(hfLocationID.Value);
            Printing.PrintTransfer_Final_Ticket(Server, UID, LocationID, "", ddPrinter.Text);

        }
    }

    protected void SqlPrinters_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {

    }
}