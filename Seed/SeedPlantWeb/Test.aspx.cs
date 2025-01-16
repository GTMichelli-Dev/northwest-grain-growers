using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
         


    }


   


   
    protected void btnOk_Click(object sender, EventArgs e)
    {
        CloseScalePopup();
       
    }


    private void CloseScalePopup()
    {
        this.tmrScaleUpdate.Enabled = false;
        tmrScaleUpdate.Interval = 100;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "popScales", "$('#popScales').modal('hide')", true);
    }

    protected void btnTest_Click(object sender, EventArgs e)
    {

        txtManualWeight.Text = "0";
        UPManualScale.Update();
        this.tmrScaleUpdate.Enabled = true;
    }

  
  

    protected void tmrScaleUpdate_Tick(object sender, EventArgs e)
    {
        tmrScaleUpdate.Interval = 1000;
        UpdateScales();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "popScales", "$('#popScales').modal('show')", true);
    }

   public void UpdateScales()
    {
        using (ScaleWebService.LocalWebService proxy = new ScaleWebService.LocalWebService())
        {
            ScaleWebService.LocalDataSet.Weigh_ScalesDataTable ScalesDataTable = proxy.GetScales();
            var CurrentScales = new ScaleDataSet.ScalesDataTable();
            ScalesDataTable.DefaultView.Sort = "Description desc ";

           
            int index = 1;
            foreach (DataRowView  sRow in ScalesDataTable.DefaultView) // ScaleWebService.LocalDataSet.Weigh_ScalesRow  scaleRow in ScalesDataTable )
            {
                var newScale = CurrentScales.NewScalesRow();
                
                if ((bool)sRow["OK"])
                {
                    if ((bool)sRow["Motion"] )
                    {
                        newScale.Status = "Motion";
                        newScale.ReadOnly = true;
                        newScale.RowCssClass = "form-control text-right alert-warning";
                    }
                    else
                    {
                        if ((int)sRow["Weight"]>500)
                        {
                            newScale.RowCssClass = "form-control text-right alert-success";
                        }
                        else
                        {
                            newScale.RowCssClass = "form-control text-right ";
                        }
                        newScale.Status = "";
                        newScale.ReadOnly = false;
                        
                    }
                }
                else
                {
                    newScale.ReadOnly = true;
                    newScale.Status = (string)sRow["Error_Message"];
                    newScale.RowCssClass = "form-control text-right alert-danger";
                }
                newScale.Description = (string)sRow["Description"];
                newScale.Index = index;
                newScale.Motion = (bool)sRow["Motion"];


                newScale.Valid = (bool)sRow["OK"];
                newScale.Weight = (int)sRow["Weight"];
                CurrentScales.AddScalesRow(newScale);
                index++;
            }
            grdScales.DataSource = CurrentScales;
            grdScales.DataBind();
            UPScaleWeights.Update();
        }
    }


    

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        CloseScalePopup();
    }

    protected void btnCancelScaleWeight_Click(object sender, EventArgs e)
    {
        tmrScaleUpdate.Enabled = false;
    }

    protected void btnSelect_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        GridViewRow row = (GridViewRow)btn.NamingContainer;
        int index = row.RowIndex;
        TextBox txtWeight = (TextBox)row.FindControl("txtWeight");
        int Weight;
        int.TryParse(txtWeight.Text, out Weight);
        SetWeight(Weight);
    }

    public void SetWeight(int Weight)
    {
        lblWeight.Text = Weight.ToString();
        CloseScalePopup();

    }
 

   
    protected void btnManualSelect_Click(object sender, EventArgs e)
    {
        int Weight;
        int.TryParse(txtManualWeight.Text.Trim(), out Weight);
        SetWeight(Weight);
    }
}