using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Locations : System.Web.UI.Page
{

    public  RemoteDataSet.RemoteSitesDataTable remoteSitesDataTable = new RemoteDataSet.RemoteSitesDataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Security.GetUsersSecurityLevel(Session) == Security.enumSecurityLevel.View) Response.Redirect("~/Default.aspx");
        //if (!this.IsPostBack)
        {


            //remoteSitesDataTable.AddRemoteSitesRow("", "ScaleHouse", "100.64.0.10", "http://100.64.0.10/Default.aspx", "",true);
            remoteSitesDataTable.AddRemoteSitesRow("Waitsburg","ScaleHouse", "100.64.0.37", "http://100.64.0.37/Default.aspx", "Waitsburg",true );
            remoteSitesDataTable.AddRemoteSitesRow("Lower Whetstone","ScaleHouse", "100.64.0.39", "http://100.64.0.39/Default.aspx", "LowerWhetstone",true );
            remoteSitesDataTable.AddRemoteSitesRow("Relief","ScaleHouse", "100.64.0.41", "http://100.64.0.41/Default.aspx", "Relief",true );
            remoteSitesDataTable.AddRemoteSitesRow("Mckay", "ScaleHouse", "100.64.0.57", "http://100.64.0.57/Default.aspx", "Mckay", true);
            remoteSitesDataTable.AddRemoteSitesRow("Rulo", "ScaleHouse", "100.64.0.36", "http://100.64.0.36/Default.aspx", "Rulo", true);
            remoteSitesDataTable.AddRemoteSitesRow("Bolles", "ScaleHouse", "100.64.0.47", "http://100.64.0.47/Default.aspx", "Bolles",true);
            remoteSitesDataTable.AddRemoteSitesRow("Coppei", "ScaleHouse", "100.64.0.49", "http://100.64.0.49/Default.aspx", "Coppei", true);
            remoteSitesDataTable.AddRemoteSitesRow("Dixie", "ScaleHouse", "100.64.0.40", "http://100.64.0.40/Default.aspx", "Dixie", true);
            remoteSitesDataTable.AddRemoteSitesRow("Spring Valley", "ScaleHouse", "100.64.0.42", "http://100.64.0.42/Default.aspx", "SpringValley", true);
            remoteSitesDataTable.AddRemoteSitesRow("Valley Grove", "ScaleHouse", "100.64.0.46", "http://100.64.0.46/Default.aspx", "ValleyGrove", true);
            remoteSitesDataTable.AddRemoteSitesRow("Sapolil", "ScaleHouse", "100.64.0.45", "http://100.64.0.45/Default.aspx", "Sapolil", true);
            remoteSitesDataTable.AddRemoteSitesRow("Tracy", "ScaleHouse", "100.64.0.43", "http://100.64.0.43/Default.aspx", "Tracy", true);
            remoteSitesDataTable.AddRemoteSitesRow("Reser", "ScaleHouse", "100.64.0.38", "http://100.64.0.38/Default.aspx", "Reser",true);
            remoteSitesDataTable.AddRemoteSitesRow("Huntsville", "ScaleHouse", "100.64.0.51", "http://100.64.0.51/Default.aspx", "Huntsville", true);
            remoteSitesDataTable.AddRemoteSitesRow("Longs", "ScaleHouse", "100.64.0.50", "http://100.64.0.50/Default.aspx", "Longs", true);
            remoteSitesDataTable.AddRemoteSitesRow("Turner", "ScaleHouse", "100.64.0.44", "http://100.64.0.44/Default.aspx", "Turner", true);
            remoteSitesDataTable.AddRemoteSitesRow("St John", "ScaleHouse", "10.23.1.195", "http://10.23.1.195/Default.aspx", "StJohns", true);
            remoteSitesDataTable.AddRemoteSitesRow("Endicott", "ScaleHouse", "100.64.0.23", "http://100.64.0.23/Default.aspx", "Endicott", true);
            remoteSitesDataTable.AddRemoteSitesRow("Endicott", "Barley House", "100.64.0.75", "http://100.64.0.75/Default.aspx", "Endicott", true);
            remoteSitesDataTable.AddRemoteSitesRow("Endicott", "Ground Pile", "100.64.0.75", "http://100.64.0.75/Default.aspx", "Endicott", true);
            remoteSitesDataTable.AddRemoteSitesRow("Lancaster", "ScaleHouse", "100.64.0.48", "http://100.64.0.7/Default.aspx", "LancasterSeedPlant", true);
            remoteSitesDataTable.AddRemoteSitesRow("Ewan", "ScaleHouse", "100.64.0.80", "http://100.64.0.80/Default.aspx", "Ewan", true);
            remoteSitesDataTable.AddRemoteSitesRow("Sunset", "ScaleHouse", "100.64.0.78", "http://100.64.0.78/Default.aspx", "Sunset", true);
            remoteSitesDataTable.AddRemoteSitesRow("Pleasant Valley", "ScaleHouse", "100.64.0.81", "http://100.64.0.81/Default.aspx", "PleasantValley", true);
            remoteSitesDataTable.AddRemoteSitesRow("Walla Walla", "Seed Plant", "100.64.0.79", "http://100.64.0.79/Default.aspx", "WallaWallaSeedPlant", true);
            remoteSitesDataTable.AddRemoteSitesRow("Wallula", "ScaleHouse", "100.64.0.60", "http://100.64.0.24/Default.aspx", "Wallula", true);
            remoteSitesDataTable.AddRemoteSitesRow("Wallula", "Elevator", "100.64.0.55", "http://100.64.0.24/Default.aspx", "Wallula", true);
            remoteSitesDataTable.AddRemoteSitesRow("Sheffler", "ScaleHouse", "100.64.0.62", "http://100.64.0.20/Default.aspx", "Sheffler", true);
            remoteSitesDataTable.AddRemoteSitesRow("Sheffler", "Elevator", "100.64.0.63", "http://100.64.0.20/Default.aspx", "Sheffler", true);
            remoteSitesDataTable.AddRemoteSitesRow("Prescott", "ScaleHouse", "100.64.0.66", "http://100.64.0.22/Default.aspx", "Prescott", true);
            remoteSitesDataTable.AddRemoteSitesRow("Spofford", "Elevator", "100.64.0.53", "http://100.64.0.30/Default.aspx", "Spofford", true);
            remoteSitesDataTable.AddRemoteSitesRow("Spofford", "ScaleHouse", "100.64.0.29", "http://100.64.0.30/Default.aspx", "Spofford", true);
            remoteSitesDataTable.AddRemoteSitesRow("Athena", "ScaleHouse", "100.64.0.52", "http://100.64.0.31/Default.aspx", "Athena", true);
            remoteSitesDataTable.AddRemoteSitesRow("Port Kelly", "Elevator", "100.64.0.59", "http://100.64.0.18/Default.aspx", "PortKelly", true);
            remoteSitesDataTable.AddRemoteSitesRow("Port Kelly", "ScaleHouse", "100.64.0.58", "http://100.64.0.18/Default.aspx", "PortKelly", true);
            remoteSitesDataTable.AddRemoteSitesRow("Lyons Ferry", "RiverSide", "100.64.0.89", "http://100.64.0.21/Default.aspx", "LyonsFerry", true);
            remoteSitesDataTable.AddRemoteSitesRow("Lyons Ferry", "HighwaySide", "100.64.0.88", "http://100.64.0.21/Default.aspx", "LyonsFerry", true);
            remoteSitesDataTable.AddRemoteSitesRow("Dayton", "898", "100.64.0.61", "http://100.64.0.26/Default.aspx", "Dayton", true);
            remoteSitesDataTable.AddRemoteSitesRow("Dayton", "318", "100.64.0.65", "http://100.64.0.26/Default.aspx", "Dayton", true);
            remoteSitesDataTable.AddRemoteSitesRow("Mission", "ScaleHouse", "100.64.0.54", "http://100.64.0.32/Default.aspx", "Mission", true);
            remoteSitesDataTable.AddRemoteSitesRow("Wilada", "I House", "100.64.0.67", "http://100.64.0.7/Default.aspx", "IHouse", true);
            remoteSitesDataTable.AddRemoteSitesRow("Wilada", "E House", "100.64.0.70", "http://100.64.0.7/Default.aspx", "EHouse", true);
            remoteSitesDataTable.AddRemoteSitesRow("Harsha", "ScaleHouse", "100.64.0.33", "http://100.64.0.33/Default.aspx", "Harsha", true);
            remoteSitesDataTable.AddRemoteSitesRow("Dry Creek", "ScaleHouse", "100.64.0.35", "http://100.64.0.35/Default.aspx", "DryCreek", true);
            remoteSitesDataTable.AddRemoteSitesRow("Thera", "ScaleHouse", "100.64.0.56", "http://100.64.0.56/Default.aspx", "Thera", true);
            remoteSitesDataTable.AddRemoteSitesRow("Winona", "Tepee", "100.64.0.73", "http://100.64.0.73/Default.aspx", "Winona", true);
            remoteSitesDataTable.AddRemoteSitesRow("Winona", "Crib", "100.64.0.77", "http://100.64.0.73/Default.aspx", "Winona", true);
            remoteSitesDataTable.AddRemoteSitesRow("Garfield", "ScaleHouse", "100.64.0.3", "http://100.64.0.3/Default.aspx", "GarfieldSeedPlant", true);
  	remoteSitesDataTable.AddRemoteSitesRow("Miller", "ScaleHouse", "100.64.0.34", "http://100.64.0.34 /Default.aspx", "Miller", true);
  	remoteSitesDataTable.AddRemoteSitesRow("Test", "Test","10.0.1.33", "http://10.0.1.33/Default.aspx", "Test", true);

            if (!this.IsPostBack )
                {
                hfSortDirection.Value = "";
                hfSortField.Value = "SiteName";
                remoteSitesDataTable.DefaultView.Sort = hfSortField.Value + hfSortDirection.Value;


                GridView1.DataSource = remoteSitesDataTable;
                GridView1.DataBind();
            }


        }
    }







    public void SendFile(string URL)
    {
        string filePath = MapPath(URL);
        FileInfo file = new FileInfo(filePath);
        if (file.Exists)
        {

            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
            Response.AddHeader("Content-Length", file.Length.ToString());
            Response.ContentType = "text/plain";
            Response.Flush();
            Response.TransmitFile(file.FullName);
            Response.End();
        }
    }


    protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (e.SortExpression != hfSortField.Value) hfSortDirection.Value = " desc";

        hfSortDirection.Value = (hfSortDirection.Value == " desc") ? "" : " desc";
        hfSortField.Value = e.SortExpression;
        remoteSitesDataTable.DefaultView.Sort = hfSortField.Value + hfSortDirection.Value;

        GridView1.DataSource = remoteSitesDataTable;
        GridView1.DataBind();

    }


    protected void lnkBatchFile_Click(object sender, EventArgs e)
    {
    
        LinkButton lnkBatchFile = (LinkButton)sender;
        GridViewRow row = (GridViewRow)lnkBatchFile.NamingContainer;
        HiddenField hfAddress = (HiddenField)row.FindControl("hfAddress");
        HiddenField hfFileLocation = (HiddenField)row.FindControl("hfFileLocation");
        HiddenField BatchName = (HiddenField)row.FindControl("hfBatchName");

        BatchDownload.DownloadNWGG(BatchName.Value, hfFileLocation.Value, hfAddress.Value, Response);
      
    }


    private void Test()
    {
        MemoryStream inMemoryCopy = new MemoryStream();
        string path = Server.MapPath("~/Template/VNC.vnc");
        string text = File.ReadAllText(path);
        text = text.Replace("<IPADDRESS>", "10.11.1.10");

    }







    protected void lnkVNC_Click(object sender, EventArgs e)
    {
        LinkButton lnkVNC = (LinkButton)sender;
        GridViewRow row = (GridViewRow)lnkVNC.NamingContainer;
        HiddenField hfAddress = (HiddenField)row.FindControl("hfAddress");
        HiddenField hfFileLocation = (HiddenField)row.FindControl("hfFileLocation");
        HiddenField BatchName = (HiddenField)row.FindControl("hfBatchName");
        BatchDownload.DownloadVNC(BatchName.Value, hfAddress.Value, Response);

    }
}