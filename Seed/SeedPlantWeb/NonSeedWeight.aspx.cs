using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class NonSeedWeight : System.Web.UI.Page
{

  


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            UpdateScales();
           
        }
        catch (Exception ex)
        {
            Auditing.LogMessage("NonSeedWeight.PageLoad", ex.Message);
            Response.Redirect("~/default.aspx");
        }

    }



   

   






    public void UpdateScales()
    {
        using (ScaleWebService.LocalWebService proxy = new ScaleWebService.LocalWebService())
        {
            ScaleWebService.LocalDataSet.Weigh_ScalesDataTable ScalesDataTable = proxy.GetScales();
            var CurrentScales = new ScaleDataSet.ScalesDataTable();
            ScalesDataTable.DefaultView.Sort = "Description Asc ";


            int index = 1;
            foreach (DataRowView sRow in ScalesDataTable.DefaultView) // ScaleWebService.LocalDataSet.Weigh_ScalesRow  scaleRow in ScalesDataTable )
            {
                var newScale = CurrentScales.NewScalesRow();

                if ((bool)sRow["OK"])
                {
                    if ((bool)sRow["Motion"])
                    {
                        newScale.Status = "Motion";
                        newScale.ReadOnly = true;
                        newScale.RowCssClass = "form-control text-right alert-warning";
                    }
                    else
                    {
                        if ((int)sRow["Weight"] > 500)
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


    protected void btnSelect_Click(object sender, EventArgs e)
    {

        LinkButton btn = (LinkButton)sender;
        GridViewRow row = (GridViewRow)btn.NamingContainer;
        int index = row.RowIndex;
        Label txtWeight = (Label)row.FindControl("txtWeight");
        int Weight;
        int.TryParse(txtWeight.Text, out Weight);
        string ScaleDescription = grdScales.DataKeys[index].Values[0].ToString();
        SetWeight(Weight, ScaleDescription, false);

    }

    public void SetWeight(int Weight, string ScaleDescription, bool Manual)
    {

        Response.Redirect($"Printticket?NonSeed=true&Vehicle={txtVehicle.Text}&Weight={Weight}");

    }


  

    protected void txtVehicle_TextChanged(object sender, EventArgs e)
    {
      
    }
}