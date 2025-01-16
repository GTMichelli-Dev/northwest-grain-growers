using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WeightSheets_WeightSheetDetails_WeightSheetDetails : System.Web.UI.Page
{

    public Guid UID
    {
        get
        {
            Guid UID = Guid.Empty;
            if (Request.QueryString["UID"] != null)
            {
                Guid.TryParse(Request.QueryString["UID"].ToString(), out UID);
            }
            return UID;
        }
    }


    private WeightSheetDataSet.Weight_SheetsRow WeightSheetRow
    {
 
        get
        {
            using (WeightSheetDataSet.Weight_SheetsDataTable weight_SheetsDataTable = new WeightSheetDataSet.Weight_SheetsDataTable())
            {
                using (WeightSheetDataSetTableAdapters.Weight_SheetsTableAdapter weight_SheetsTableAdapter = new WeightSheetDataSetTableAdapters.Weight_SheetsTableAdapter())
                {
                    if (weight_SheetsTableAdapter.FillByUID(weight_SheetsDataTable,UID)>0)
                    {
                        return weight_SheetsDataTable[0];
                    }
                    else
                    {
                        return null;
                    }
                }
            }

        }
        set
        {
            using (WeightSheetDataSet.Weight_SheetsDataTable weight_SheetsDataTable = new WeightSheetDataSet.Weight_SheetsDataTable())
            {
                using (WeightSheetDataSetTableAdapters.Weight_SheetsTableAdapter weight_SheetsTableAdapter = new WeightSheetDataSetTableAdapters.Weight_SheetsTableAdapter())
                {
                    if (weight_SheetsTableAdapter.FillByUID(weight_SheetsDataTable, UID) > 0)
                    {

                        weight_SheetsDataTable[0].ItemArray=value.ItemArray ;
                        weight_SheetsTableAdapter.Update(weight_SheetsDataTable);
                    }
                }
            }

        }
    } 



    protected void Page_Load(object sender, EventArgs e)
    {
        if (! this.IsPostBack )
        {
            if (UID==Guid.Empty )
            {
                Response.Redirect("~/Default.aspx");
            }
            else
            {

                Label lblDate =(Label)IntakeDetails.FindControl("lblDate");
                TextBox txtDate = (TextBox)IntakeDetails.FindControl("txtDate");

                if (lblDate != null)lblDate.Visible=  (Security.GetUsersSecurityLevel(Session) != Security.enumSecurityLevel.Administrator);
                if (txtDate != null)txtDate.Visible = (Security.GetUsersSecurityLevel(Session) == Security.enumSecurityLevel.Administrator);


                using (WeightSheetDataSetTableAdapters.Single_Weightsheet_TypeTableAdapter  single_Weight_TypeTableAdapter = new WeightSheetDataSetTableAdapters.Single_Weightsheet_TypeTableAdapter())
                {
                    using (WeightSheetDataSet weightSheetDataSet = new WeightSheetDataSet())
                    {

                        weightSheetDataSet.EnforceConstraints = false;
                        if (single_Weight_TypeTableAdapter.Fill(weightSheetDataSet.Single_Weightsheet_Type , UID)>0)
                        {
                            WeightSheetDataSet.Single_Weightsheet_TypeRow  row = weightSheetDataSet.Single_Weightsheet_Type[0];
                            this.lblWeightSheet.Text = string.Format("Details For Weight Sheet {0}", row.WS_Id);
                            PlaceHolderIntake.Visible = row.IsIntake;
                            PlaceHolderTransfer.Visible = !row.IsIntake;

                        }
                    }
                }
            }
        }

    }

    protected void TotalBilled_TextChanged(object sender, EventArgs e)
    {
        TextBox TotalBilled = (TextBox)this.IntakeDetails.FindControl("TotalBilled");
      

      //  WeightSheetRow.Comment = txtComment.Text;

    }

    protected void txtComment_TextChanged(object sender, EventArgs e)
    {
        TextBox txtComment = (TextBox)this.IntakeDetails.FindControl("txtComment");

        WeightSheetRow.Comment = txtComment.Text;
    }

    protected void txtDate_TextChanged(object sender, EventArgs e)
    {
        TextBox txtDate = (TextBox)this.IntakeDetails.FindControl("txtDate");
        try
        {
            var dt = Convert.ToDateTime(txtDate.Text);
            var oldDate = WeightSheetRow.Creation_Date;
            WeightSheetRow.Creation_Date = dt;

            using (WeightSheetDataSetTableAdapters.QueriesTableAdapter Q= new WeightSheetDataSetTableAdapters.QueriesTableAdapter())
            {
                Q.UpdateWeightSheetCreationDate(dt, UID);
                Audit.AddAuditTrail("Date Change", WeightSheetRow.Location_Id, WeightSheetRow.WS_Id.ToString(),"Changed Weight Sheet Creation Date", oldDate.ToString(),dt.ToString());
            }


        }
        catch
        {
            txtDate.Text=WeightSheetRow.Creation_Date.ToShortDateString() ;

        }

        
    }

   
}