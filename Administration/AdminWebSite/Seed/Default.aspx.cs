using System;
using System.Collections.Generic;
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
       
        if (!this.IsPostBack)
        {
            
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
     
    }

    protected void btnSelect_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        GridViewRow row = (GridViewRow)btn.NamingContainer;
        int index = row.RowIndex;
        Guid UID= Guid.Parse( Grd1.DataKeys[index].Values[0].ToString());
     
        Response.Redirect("~/Preload/TicketCreator.aspx");





    }

    protected void tmrUPDate_Tick(object sender, EventArgs e)
    {
        Grd1.DataBind();
    }
}