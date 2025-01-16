using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.IO;
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

                this.GridView1.DataBind();
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
      
            LinkButton hyprLink = (LinkButton)sender;
            GridViewRow row = (GridViewRow)hyprLink.NamingContainer;
            Guid UID = (Guid)GridView1.DataKeys[row.RowIndex].Value;

            string url = string.Format("/WeightSheets/WeightSheet.aspx?WSUID={0}&Transfer={1}", UID, true);
            string script = $"window.open('{url}', '_blank');";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "OpenNewTab", script, true);
      
    }

    protected void txtStartDate_TextChanged(object sender, EventArgs e)
    {
        CheckDate(txtStartDate, hfStart, "WSStartDate");
        this.GridView1.DataBind();

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
        this.GridView1.DataBind();
    }



    protected void cboLocation_TextChanged(object sender, EventArgs e)
    {
        Location = cboLocation.SelectedItem.Text;
        this.GridView1.DataBind();
    }

    
    protected void ddOpenClosed_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddOpenClosed_TextChanged(object sender, EventArgs e)
    {
        OpenState = ddOpenClosed.SelectedItem.Text;
        this.GridView1.DataBind();
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


    protected void ddHauler_TextChanged(object sender, EventArgs e)
    {
        //Hauler = ddHauler.SelectedItem.Text;
        this.GridView1.DataBind();

    }

    protected void ddSource_TextChanged(object sender, EventArgs e)
    {
        Source = ddSource.SelectedItem.Text;
        this.GridView1.DataBind();
    }

    protected void SqlVariety_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {

    }


    protected void btnExcel_Click(object sender, EventArgs e)
    {

        Excel.ExportToExcel(GridView1, Response, $"Transfer_WS");
    
    }



    protected void btnPrint_Click(object sender, EventArgs e)
    {


        List<Guid> uidList = new List<Guid>();

        // Loop through each row in the GridView
        foreach (GridViewRow row in GridView1.Rows)
        {
            // Get the underlying data item for the row
            var dataItem = GridView1.DataKeys[row.RowIndex].Value;

            // Assuming the underlying data item has a property named "uid" of type Guid
            Guid uid = (Guid)dataItem;

            uidList.Add(uid);
        }
        if (uidList.Count > 0)
        {
            PdfDocument outputDocument = new PdfDocument();

            List<MemoryStream> pdfStreams = new List<MemoryStream>();

            foreach (var uid in uidList)
            {
                pdfStreams.Add(GetTransferWeightSheet(uid));
            }

            foreach (var pdfStream in pdfStreams)
            {
                pdfStream.Position = 0; // Reset stream position for reading
                PdfDocument inputDocument = PdfReader.Open(pdfStream, PdfDocumentOpenMode.Import);

                // Import pages from each report PDF and add them to the output document
                for (int i = 0; i < inputDocument.PageCount; i++)
                {
                    outputDocument.AddPage(inputDocument.Pages[i]);
                }
            }

            // Save combined PDF to a memory stream
            using (MemoryStream combinedPDF = new MemoryStream())
            {
                outputDocument.Save(combinedPDF, false);
                combinedPDF.Position = 0; // Reset stream position for reading

                // Send the combined PDF as an HTTP response
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", "attachment; filename=CombinedReport.pdf");
                Response.AddHeader("Content-Length", combinedPDF.Length.ToString());
                Response.OutputStream.Write(combinedPDF.ToArray(), 0, combinedPDF.ToArray().Length);
                Response.Flush();
                Response.End();
            }
        }

    }

    public MemoryStream GetTransferWeightSheet(Guid Weight_Sheet_UID)
    {
        var rptWeigh_Sheet = new ReportDocument();

        MemoryStream stream = new MemoryStream();
        try
        {



            {


                using (NWDataSet NWDataset = new NWDataSet())
                {
                    rptWeigh_Sheet.Load(Server.MapPath("Transfer_Weigh_Sheet.rpt"));
                    {


                        using (NWDataSetTableAdapters.vwTransfer_LoadTableAdapter vwTransfer_LoadTableAdapter = new NWDataSetTableAdapters.vwTransfer_LoadTableAdapter())
                        {
                            bool Closed = true;
                            vwTransfer_LoadTableAdapter.FillByWeight_Sheet_UID(NWDataset.vwTransfer_Load, Weight_Sheet_UID);
                            {
                                using (NWDataSetTableAdapters.LocationsTableAdapter locationsTableAdapter = new NWDataSetTableAdapters.LocationsTableAdapter())
                                {
                                    if (NWDataset.vwTransfer_Load.Count > 0)
                                    {
                                        locationsTableAdapter.FillById(NWDataset.Locations, NWDataset.vwTransfer_Load[0].Location_Id);
                                        Closed = NWDataset.vwTransfer_Load[0].Closed;
                                        
                                    }
                                    else
                                    {
                                        using (NWDataSetTableAdapters.Weight_SheetsTableAdapter weight_SheetsTableAdapter = new NWDataSetTableAdapters.Weight_SheetsTableAdapter())
                                        {
                                            if (weight_SheetsTableAdapter.Fill(NWDataset.Weight_Sheets, Weight_Sheet_UID) > 0)
                                            {
                                                locationsTableAdapter.FillById(NWDataset.Locations, NWDataset.Weight_Sheets[0].Location_Id);
                                                Closed = NWDataset.Weight_Sheets[0].Closed;

                                            }
                                        }
                                    }
                                }
                                using (NWDataSetTableAdapters.vwTransfer_Weight_Sheet_InformationTableAdapter vwTransfer_Weight_Sheet_InformationTableAdapter = new NWDataSetTableAdapters.vwTransfer_Weight_Sheet_InformationTableAdapter())
                                {
                                    if (vwTransfer_Weight_Sheet_InformationTableAdapter.FillByWeight_SheetUID(NWDataset.vwTransfer_Weight_Sheet_Information, Weight_Sheet_UID) > 0)
                                    {
                                        {


                                            rptWeigh_Sheet.SetDataSource(NWDataset);
                                            rptWeigh_Sheet.Subreports[0].SetDataSource(NWDataset);
                                            decimal TotalBilled = 0;
                                            if (!NWDataset.vwTransfer_Weight_Sheet_Information[0].IsTotal_BilledNull())
                                            {
                                                TotalBilled = NWDataset.vwTransfer_Weight_Sheet_Information[0].Total_Billed;
                                            }



                                            var Custom = (!NWDataset.vwTransfer_Weight_Sheet_Information[0].IsBOL_TypeNull() && NWDataset.vwTransfer_Weight_Sheet_Information[0].BOL_Type.ToUpper().Trim() == "C");

                                            rptWeigh_Sheet.SetParameterValue("CustomRate", Custom);
                                            rptWeigh_Sheet.SetParameterValue("Closed", Closed);
                                            rptWeigh_Sheet.SetParameterValue("Total_Billed", TotalBilled);
                                            rptWeigh_Sheet.SetParameterValue("Location_Description", NWDataset.vwTransfer_Weight_Sheet_Information[0].Location_Description);
                                            rptWeigh_Sheet.SetParameterValue("Creation_Date", NWDataset.vwTransfer_Weight_Sheet_Information[0].Creation_Date);
                                            //rptWeigh_Sheet.SetParameterValue("Location_Id", NWDataset.vwTransfer_Weight_Sheet_Information[0].Location_Id);
                                            rptWeigh_Sheet.SetParameterValue("Unofficial", true);
                                            rptWeigh_Sheet.SetParameterValue("StrWS_Id", NWDataset.vwTransfer_Weight_Sheet_Information[0].WS_Id.ToString().PadLeft(8, '0'));
                                            //NWDataset.vwTransfer_Weight_Sheet_Information[0].Original_Printed = !Official;
                                            ExportOptions exportOpts = new ExportOptions();
                                            exportOpts.ExportFormatType = ExportFormatType.PortableDocFormat;

                                            EditableRTFExportFormatOptions exportFormatOptions =
                                              ExportOptions.CreateEditableRTFExportFormatOptions();
                                            exportFormatOptions.FirstPageNumber = 1;
                                            exportFormatOptions.InsertPageBreaks = true;
                                            exportFormatOptions.LastPageNumber = 1;
                                            exportFormatOptions.UsePageRange = true;
                                            exportOpts.ExportFormatOptions = exportFormatOptions;





                                            ExportRequestContext req = new ExportRequestContext();

                                            req.ExportInfo = exportOpts;


                                            Stream s = rptWeigh_Sheet.FormatEngine.ExportToStream(req);
                                            s.CopyTo(stream);
                                        }
                                    }
                                    System.Threading.Thread.Sleep(1000);
                                }

                            }
                        }
                    }
                }

            }
            
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.Print(ex.Message);

        }
        finally
        {
            if (rptWeigh_Sheet != null)
            {
                rptWeigh_Sheet.Close();
                rptWeigh_Sheet.Dispose();
            }
        }

        return stream;
    }
}