using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TicketImage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["Ticket"] == null)
        {
            Response.Redirect("TicketCreator");
        }
        else
        {

           List<Camera.PicData> TicketPictures=  Camera.GetPicturesForTicket(Request.QueryString["Ticket"].ToString());
          
           lblHdr.Text =(TicketPictures.Count>1)? $"Images For Ticket:{Request.QueryString["Ticket"].ToString()}" : $"Image For Ticket:{Request.QueryString["Ticket"].ToString()}";
            Repeater1.DataSource = TicketPictures;
            Repeater1.DataBind();
    
        }
    }






   
}