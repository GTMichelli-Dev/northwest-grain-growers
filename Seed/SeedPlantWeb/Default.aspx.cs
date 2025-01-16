using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : Page
{
    public string  StartDate
    {

        get
        {
            string Date = DateTimeExtensions.getFormattedDate(DateTime.Now);
            if (Session["StartDate"] != null)
            {
                string sessionDate = Session["StartDate"].ToString();
                if (DateTimeExtensions.IsValidDateFormat(sessionDate))
                {
                    Date = sessionDate ;
                    
                }
                

            }
            return Date;
        }
        set
        {

            Session["StartDate"] = value;
        }
    }

    public string EndDate
    {


        get
        {
            string Date = DateTimeExtensions.getFormattedDate(DateTime.Now);
            if (Session["EndDate"] != null)
            {
                string sessionDate = Session["EndDate"].ToString();
                if (DateTimeExtensions.IsValidDateFormat(sessionDate))
                {
                    Date = sessionDate;

                }


            }
            return Date;
        }
        set
        {

            Session["EndDate"] = value;
        }
    }



    public string Customer
    {

        get
        {
            string lc = "";
            if (Session["Customer"] != null)
            {
                lc = Session["Customer"].ToString();
            }
            return lc;
        }
        set
        {

            Session["Customer"] = value;

        }
    }



    public string Ticketstatus
    {

        get
        {
            string Retval="0";
            if (Session["Finished"] != null)
            {
               Retval= ( Session["Finished"].ToString()=="1")?"1":"0";
            }
            return Retval;
        }
        set
        {

            Session["Finished"] = value;

        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (GlobalVars.Location == 0) Response.Redirect("Setup.aspx");
        if (!this.IsPostBack)
        {
            hfLocation.Value = GlobalVars.Location.ToString();
            try
            {
                ddType.DataBind();
                ddType.ClearSelection();
                ddType.Items.FindByValue(Ticketstatus).Selected = true;
                ticketFilter.Visible = ddType.SelectedItem.Value == "1";
            }
            catch
            { }

            txtStartDate.Text = StartDate;
            txtEndDate.Text = EndDate;

            Grd1.DataBind();
           
        }

        try
        {
            Grd1.Columns[1].Visible = ddType.SelectedItem.Value == "0";

            Grd1.Columns[2].Visible = ddType.SelectedItem.Value == "1";

        }
        catch
        {

        }



    }

    protected void ddType_TextChanged(object sender, EventArgs e)
    {
        int index = 0;
        int.TryParse(ddType.SelectedItem.Value.ToString(), out index);
        Ticketstatus = (index==2)?"0":"1" ;
        ticketFilter.Visible = ddType.SelectedItem.Value == "1";
        
    }

    protected void txtEndDate_TextChanged(object sender, EventArgs e)
    {
        var Date = txtEndDate.Text;
        if (!DateTimeExtensions.IsValidDateFormat(Date))
        {
            Date = DateTimeExtensions.getFormattedDate(DateTime.Now);
            txtEndDate.Text = Date;
        }
        EndDate  = Date;



    }

    protected void txtStartDate_TextChanged(object sender, EventArgs e)
    {
        var Date = txtStartDate.Text;
        if (!DateTimeExtensions.IsValidDateFormat(Date))
        {
            Date = DateTimeExtensions.getFormattedDate(DateTime.Now);
            txtStartDate.Text = Date;
        }
        StartDate = Date;


    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtStartDate.Text  = DateTimeExtensions.getFormattedDate(DateTime.Now);
        txtEndDate.Text = DateTimeExtensions.getFormattedDate(DateTime.Now);
        txtCustomer.Text = "";
        txtTicket.Text = "";
    }

  






    protected void btnDelete_Click(object sender, EventArgs e)
    {
        Button  btn = (Button)sender;
        GridViewRow row = (GridViewRow)btn.NamingContainer;
        int index = row.RowIndex;
        string strUID = Grd1.DataKeys[index].Values[0].ToString();
        Response.Redirect($"ConfirmTicketDelete?UID={strUID}");
      
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        SeedTicketInfo.CreateSeedTicket();
        Response.Redirect("~/Preload/SelectGrower.aspx?Reset=true");
    }

    protected void btnSelect_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        GridViewRow row = (GridViewRow)btn.NamingContainer;
        int index = row.RowIndex;
        Guid UID= Guid.Parse( Grd1.DataKeys[index].Values[0].ToString());
        SeedTicketInfo.GetSeedTicketDataset(UID);
        Response.Redirect("~/Preload/TicketCreator.aspx");





    }

    protected void tmrUPDate_Tick(object sender, EventArgs e)
    {
        tmrUPDate.Interval = 5000;
        Grd1.DataBind();
      
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {

       System.Data.DataView  Data = (System.Data.DataView)SqlTickets.Select(DataSourceSelectArguments.Empty );
        List<int> Tickets = new List<int>();
        foreach(System.Data.DataRow  row in Data.Table.Rows )
        {
            Tickets.Add((int)row["Ticket"]);
        }

        string Filename = "";
      
        if (!string.IsNullOrEmpty(txtTicket.Text  ))
        {
            Filename = $"Ticket_{txtTicket.Text}";
        }
        else
        {
            if (!string.IsNullOrEmpty(txtCustomer.Text ))
            {
                Filename = $"{ GlobalVars.LocationDescription} Tickets For Customer {txtCustomer.Text} Between{txtStartDate.Text} and {txtEndDate.Text}";
            }
            else
            {
                Filename = $"{ GlobalVars.LocationDescription} Tickets {txtTicket.Text} Between{txtStartDate.Text} and {txtEndDate.Text}";
            }
        }
       
        Printing.SendTicketsToBrowser(Tickets, Filename, true);
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
       
        // Perform the select operation to get a DataView
       DataView dv = (DataView)SqlReport.Select(DataSourceSelectArguments.Empty);

        // Convert the DataView to a DataTable
        DataTable Data = dv.ToTable();

        string Filename = "";

        if (!string.IsNullOrEmpty(txtTicket.Text))
        {
            Filename = $"Ticket_{txtTicket.Text}";
        }
        else
        {
            if (!string.IsNullOrEmpty(txtCustomer.Text))
            {
                Filename = $"{ GlobalVars.LocationDescription} Tickets For Customer {txtCustomer.Text} Between{txtStartDate.Text} and {txtEndDate.Text}";
            }
            else
            {
                Filename = $"{ GlobalVars.LocationDescription} Tickets {txtTicket.Text} Between{txtStartDate.Text} and {txtEndDate.Text}";
            }
        }

        Filename += ".xlsx";
        var ms= Printing.CreateXLSXReport(Data);
        Printing.DownloadExcellFile(ms, Filename, this.Response);

    }
}