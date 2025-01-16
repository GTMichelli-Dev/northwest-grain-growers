using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TransferWeightSheets : System.Web.UI.Page
{

    public DateTime  StartDate {

        get
        {
            DateTime dt = DateTime.Now; 
            if (Session["WSStartDate"]!= null )
            {
                DateTime.TryParse(Session["WSStartDate"].ToString(), out dt);
            }
            return dt;
        }
        set
        {
          
            Session["WSStartDate"] = value;
        }
    }

    public DateTime EndDate
    {

        get
        {
            DateTime dt = DateTime.Now;
            if (Session["WSEndDate"] != null)
            {
                DateTime.TryParse(Session["WSEndDate"].ToString(), out dt);
            }
            return dt;
        }
        set
        {
          
            Session["WSEndDate"] = value;

        }
    }


    public string WSType
    {

        get
        {
            string wt = "Intake/Transfer";
            if (Session["WSType"] != null)
            {
               wt=Session["WSType"].ToString();
            }
            return wt;
        }
        set
        {
          
            Session["WSType"] = value;

        }
    }


    public string Location
    {

        get
        {
            string lc = "All Locations";
            if (Session["WSLocation"] != null)
            {
                lc = Session["WSLocation"].ToString();
            }
            return lc;
        }
        set
        {
          
            Session["WSLocation"] = value;

        }
    }


    public string Crop
    {

        get
        {
            string lc = "All Cropss";
            if (Session["Crop"] != null)
            {
                lc = Session["Crop"].ToString();
            }
            return lc;
        }
        set
        {

            Session["Crop"] = value;

        }
    }

    public string Source
    {

        get
        {
            string lc = "All Sources";
            if (Session["Source"] != null)
            {
                lc = Session["Source"].ToString();
            }
            return lc;
        }
        set
        {

            Session["Source"] = value;

        }
    }



    public string Variety
    {

        get
        {
            string lc = "All Varieties";
            if (Session["Varieties"] != null)
            {
                lc = Session["Varieties"].ToString();
            }
            return lc;
        }
        set
        {

            Session["Varieties"] = value;

        }
    }


    public string OpenState
    {

        get
        {
            string lc = "Closed";
            if (Session["OpenState"] != null)
            {
                lc = Session["OpenState"].ToString();
            }
            return lc;
        }
        set
        {

            Session["OpenState"] = value;

        }
    }



    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack )
        {
            try
            {
               
                this.txtStartDate.Text = StartDate.ToShortDateString();
                this.txtEndDate.Text = EndDate.ToShortDateString();
                this.hfStart.Value = StartDate.ToShortDateString();
                this.hfEnd.Value = EndDate.ToShortDateString();
                cboLocation.DataBind();
                cboLocation.ClearSelection();
                cboLocation.Items.FindByText(Location).Selected = true;
                ddOpenClosed.DataBind();
                ddOpenClosed.ClearSelection();
                this.ddOpenClosed.Items.FindByText(OpenState).Selected = true;
                ddSource.DataBind();
                ddSource.ClearSelection();
                if (ddSource.Items.FindByText(Source)!= null ) ddSource.Items.FindByText(Source).Selected = true;
                //ddCrop.DataBind();
                //ddCrop.ClearSelection();
                //if (ddCrop.Items.FindByText(Crop) != null ) ddCrop.Items.FindByText(Crop).Selected = true;
                //ddVariety.DataBind();
                //ddVariety.ClearSelection();
               // ListItem Li=  ddVariety.Items.FindByText(Variety) ;
               // if (Li== null )
                //{
                  //  ddVariety.SelectedIndex = 0;
                //}

                
            }
            catch
            { }
        }
     
    }

    protected void lnkReprint_Click(object sender, EventArgs e)
    {
        LinkButton  lnkReprint = (LinkButton)sender;
        GridViewRow row = (GridViewRow)lnkReprint.NamingContainer;
        Guid UID = (Guid)GridView1.DataKeys[row.RowIndex].Value ;
        bool EndOfLot = false;
        bool.TryParse(row.Cells[7].Text, out EndOfLot);
        Printing.PrintWSTicket(Server, Response, UID,EndOfLot );
    }

    protected void lnkDetails_Click(object sender, EventArgs e)
    {
        LinkButton lnkReprint = (LinkButton)sender;
        GridViewRow row = (GridViewRow)lnkReprint.NamingContainer;
        Guid UID = (Guid)GridView1.DataKeys[row.RowIndex].Value;
     
        bool Transfer = true;
        
        Response.Redirect(string.Format("~/WeightSheets/WeightSheet.aspx?WSUID={0}&Transfer={1}", UID,Transfer));

    }

    protected void txtStartDate_TextChanged(object sender, EventArgs e)
    {
        CheckDate(txtStartDate, hfStart, "WSStartDate");

    }

    private void CheckDate(TextBox sender,HiddenField hf,string SessionValue)
    {

        DateTime dt = DateTime.Now;
        if (DateTime.TryParse(sender.Text,out dt  ))
        {
            hf.Value = dt.ToShortDateString();
            sender.Text = dt.ToShortDateString();
            Session[SessionValue] = dt;
            
        }
        else
        {
            sender.Text = hf.Value;
        }
    }

    protected void txtEndDate_TextChanged(object sender, EventArgs e)
    {
        CheckDate(txtEndDate, hfEnd, "WSEndDate");
    }



    protected void cboLocation_TextChanged(object sender, EventArgs e)
    {
        Location = cboLocation.SelectedItem.Text;

    }

    
    protected void ddOpenClosed_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddOpenClosed_TextChanged(object sender, EventArgs e)
    {
        OpenState = ddOpenClosed.SelectedItem.Text;
    }

    protected void ddvariety_TextChanged(object sender, EventArgs e)
    {
       // Variety = ddVariety.SelectedItem.Text;
    }

    protected void ddCrop_TextChanged(object sender, EventArgs e)
    {
        //Crop = ddCrop.SelectedItem.Text;
      //  ddVariety.DataBind();
    }

    protected void ddSource_TextChanged(object sender, EventArgs e)
    {
        Source = ddSource.SelectedItem.Text;
    }

    protected void SqlVariety_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {

    }
}