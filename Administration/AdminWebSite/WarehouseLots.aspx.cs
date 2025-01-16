using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WarehouseLots : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {


        if (!this.IsPostBack)
        {
            int year = DateTime.Now.Year;
            DateTime firstDay = DateTime.Now.AddDays(-365);//  new DateTime(year, 1, 1);


            Session["StartDate"] = firstDay;


            this.txtStartDate.Text = firstDay.ToShortDateString();
            this.txtEndDate.Text = DateTime.Now.ToShortDateString();
            this.ddLocation.DataBind();
            HttpCookie LocationCookie = Request.Cookies["LocationCookie"];
            string LocationId = "24";
            // Read the cookie information and display it.

            if (LocationCookie != null)
            {

                LocationId = LocationCookie.Value;
            }
            ddLocation.Items.FindByValue(LocationId.ToString()).Selected = true;

        }
    }


    private void SaveLocation()
    {
        HttpCookie LocationCookie = new HttpCookie("LocationCookie");
        DateTime now = DateTime.Now;


        LocationCookie.Value = ddLocation.SelectedValue;
        // Set the cookie expiration date.
        LocationCookie.Expires = now.AddYears(50); // For a cookie to effectively never expire

        // Add the cookie.
        Response.Cookies.Add(LocationCookie);

    }


    protected void ddLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        SaveLocation();
    }





    protected void txtStartDate_TextChanged(object sender, EventArgs e)
    {
        DateTime Dateval;
        if (DateTime.TryParse(txtStartDate.Text,out Dateval))
        {
            Session["StartDate"] = Dateval;
            this.txtStartDate.Text = Dateval.ToShortDateString();
        }
        else
        {
            this.txtStartDate.Text = Convert.ToDateTime( Session["StartDate"]).ToShortDateString();
        }
    }

    protected void txtEndDate_TextChanged(object sender, EventArgs e)
    {
        DateTime Dateval;
        if (DateTime.TryParse(txtEndDate.Text, out Dateval))
        {
            this.txtEndDate.Text = Dateval.ToShortDateString();
        }
        else
        {
            this.txtEndDate.Text = DateTime.Now.ToShortDateString();
        }
    }
}