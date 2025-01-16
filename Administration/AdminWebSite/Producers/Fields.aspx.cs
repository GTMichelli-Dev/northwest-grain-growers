using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Producers_Fields : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (! this.IsPostBack )
        {
            if ((Request.QueryString["Producer"] == null) || (Request.QueryString["ProducerId"] == null))  Response.Redirect("~/Producers/Producers.aspx");
            lblHeader.Text = $"Fields For {Request.QueryString["Producer"].ToString()} - { Request.QueryString["ProducerId"].ToString()}";

            ddVariety.DataBind();

        }

    }

    //protected void btnSave_Click(object sender, EventArgs e)
    //{
    //    string Field = txtField.Value;
    //    if (string.IsNullOrEmpty(Field))
    //    {
    //        addFieldError.InnerHtml = "";
    //        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$(document).ready(function() {$('#addField').modal('show');});", true);

    //    }
    //    else 
    //    {
    //        string Producer = Request.QueryString["Producer"].ToString();
    //        using (FieldsDataSet.ProducerVarietyFieldDataTable producerVarietyFieldDataTable = new FieldsDataSet.ProducerVarietyFieldDataTable ())
    //        {
    //            using (FieldsDataSetTableAdapters.ProducerVarietyFieldTableAdapter producerVarietyFieldTableAdapter = new FieldsDataSetTableAdapters.ProducerVarietyFieldTableAdapter())
    //            {
    //                producerVarietyFieldTableAdapter.FillByProducerDescription(producerVarietyFieldDataTable, Producer);
    //                var row = producerVarietyFieldDataTable.FirstOrDefault(x => x.Field.ToUpper() == Field.ToUpper());
    //                if (row != null)
    //                {
    //                    addFieldError.InnerHtml = "Field Already Exists";
    //                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$(document).ready(function() {$('#addField').modal('show');});", true);

    //                }
    //                else
    //                {
    //                    var row = producerVarietyFieldDataTable.NewProducerVarietyFieldRow();
    //                    row.UID = Guid.NewGuid();
    //                    row.
    //                }
    //            }
    //        }
    //            txtField.Value = "";
    //        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$(document).ready(function() {$('#addFieldError').val('');});", true);

    //    }
    //    else
    //    {
    //        addFieldError.InnerHtml = "Error Saving";
    //        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$(document).ready(function() {$('#addField').modal('show');});", true);

    //    }

    //    txtField.Focus();




    //}

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        lblNewHeader.Text = "New Field";
        pnlNew.CssClass = "px-2";
        pnlNew.Visible = true;
        pnlFields.Visible = false;
        ddCrop.DataBind();
        ddVariety.DataBind();
        txtField.Text = "";
        txtField.Focus();
        btnAddNew.Visible = false;
        hfEditUID.Value = Guid.Empty.ToString();
    
    }


    protected void btnCreateNew_Click(object sender, EventArgs e)
    {
        string Field = txtField.Text;
       if (string.IsNullOrEmpty(Field ))
        {
            lblNewHeader.Text = "Field Name Is Empty";
            pnlNew.CssClass = "px-2  table-danger";
        }
       else if (string.IsNullOrEmpty(ddVariety.Text))
            {
            lblNewHeader.Text = "Variety Is Empty";
            
            pnlNew.CssClass = "px-2  table-danger";

        }
       else
        {
            Guid UID = Guid.Empty;
            Guid.TryParse(hfEditUID.Value.ToString(), out UID);
            bool EditMode = (UID != Guid.Empty); 
            using (FieldsDataSetTableAdapters.ProducerVarietyFieldTableAdapter producerVarietyFieldTableAdapter = new FieldsDataSetTableAdapters.ProducerVarietyFieldTableAdapter())
            {
                using (FieldsDataSet.ProducerVarietyFieldDataTable producerVarietyFieldDataTable = new FieldsDataSet.ProducerVarietyFieldDataTable())
                {
                    string Producer = Request.QueryString["Producer"].ToString();

                    producerVarietyFieldTableAdapter.FillByProducerDescription(producerVarietyFieldDataTable, Producer);
                    var row = producerVarietyFieldDataTable.FirstOrDefault(x => x.Field.ToUpper().Trim() == Field.ToUpper().Trim());
                    if (row != null && !EditMode)
                    {
                        lblNewHeader.Text = "Field Name Is Already Assigned";
                        pnlNew.CssClass = "px-2  table-danger";
                    }
                    else
                    {
                        if (EditMode )
                        {
                            row = producerVarietyFieldDataTable.FindByUID(UID);
                        }
                        var rowExists = producerVarietyFieldDataTable.FirstOrDefault(x => x.Field.ToUpper().Trim() == Field.ToUpper().Trim() && x.UID != UID);

                        if ((row != null) && (rowExists != null))
                        {
                            lblNewHeader.Text = "Field Name Is Already Assigned";
                            pnlNew.CssClass = "px-2  table-danger";
                        }
                        else
                        {
                            int ProducerId = int.Parse(Request.QueryString["ProducerId"].ToString());
                            int VarietyId = int.Parse(ddVariety.SelectedValue);
                            if (!EditMode )
                            {
                                row = producerVarietyFieldDataTable.NewProducerVarietyFieldRow();
                                row.UID = Guid.NewGuid();
                            }
                            row.Field = Field;
                            row.Producer_Id = ProducerId;
                            row.Variety_Id = VarietyId;
                            if (!EditMode ) producerVarietyFieldDataTable.AddProducerVarietyFieldRow(row);
                            producerVarietyFieldTableAdapter.Update(producerVarietyFieldDataTable);
                            grdFields.DataBind();
                            pnlNew.Visible = false;
                            pnlFields.Visible = true;
                            btnAddNew.Visible = true;
                        }
                    }
                } 
                    
            }
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        pnlNew.Visible = false;
        pnlFields.Visible = true;
        btnAddNew.Visible = true;
    }

    protected void txtField_TextChanged(object sender, EventArgs e)
    {
     
    }

    protected void ddVariety_TextChanged(object sender, EventArgs e)
    {
     
    }

    
    protected void ddVariety_SelectedIndexChanged(object sender, EventArgs e)
    {
     
    }

    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        LinkButton lb = (LinkButton)sender;
        GridViewRow row = (GridViewRow)lb.NamingContainer;
        var Field = grdFields.DataKeys[row.RowIndex].Values[0].ToString();
        hfSelectedField.Value = Field;
        lblDeleteHeader.Text = $"Delete Field {Field}";
        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$('#confirmPrompt').modal('show');", true);
    }

    protected void btnConfirmDelete_Click(object sender, EventArgs e)
    {
        var Field = hfSelectedField.Value.ToString();

        using (FieldsDataSetTableAdapters.ProducerVarietyFieldTableAdapter producerVarietyFieldTableAdapter = new FieldsDataSetTableAdapters.ProducerVarietyFieldTableAdapter())
        {
            using (FieldsDataSet.ProducerVarietyFieldDataTable producerVarietyFieldDataTable = new FieldsDataSet.ProducerVarietyFieldDataTable())
            {
                string Producer = Request.QueryString["Producer"].ToString();

                producerVarietyFieldTableAdapter.FillByProducerDescription(producerVarietyFieldDataTable, Producer);
                var row = producerVarietyFieldDataTable.FirstOrDefault(x => x.Field.ToUpper().Trim() == Field.ToUpper().Trim());
                if (row != null)
                {
                    using (FieldsDataSetTableAdapters.QueriesTableAdapter Q = new FieldsDataSetTableAdapters.QueriesTableAdapter())
                    {
                        Q.DeleteProducerField(row.UID);
                    }
                        grdFields.DataBind();
                }
                
            }
        }


    }

    protected void lnkEdit_Click(object sender, EventArgs e)
    {
        LinkButton lb = (LinkButton)sender;
        GridViewRow row = (GridViewRow)lb.NamingContainer;
        var Field = grdFields.DataKeys[row.RowIndex].Values[0].ToString();

        using (FieldsDataSetTableAdapters.ProducerVarietyFieldTableAdapter producerVarietyFieldTableAdapter = new FieldsDataSetTableAdapters.ProducerVarietyFieldTableAdapter())
        {
            using (FieldsDataSet.ProducerVarietyFieldDataTable producerVarietyFieldDataTable = new FieldsDataSet.ProducerVarietyFieldDataTable())
            {

                using (FieldsDataSet.VarietyListDataTable  varietyListDataTable = new FieldsDataSet.VarietyListDataTable())
                {
                    using (FieldsDataSetTableAdapters.VarietyListTableAdapter varietyListTableAdapter = new FieldsDataSetTableAdapters.VarietyListTableAdapter())
                    {

                        string Producer = Request.QueryString["Producer"].ToString();

                        producerVarietyFieldTableAdapter.FillByProducerDescription(producerVarietyFieldDataTable, Producer);

                        

                        var fieldrow = producerVarietyFieldDataTable.FirstOrDefault(x => x.Field.ToUpper().Trim() == Field.ToUpper().Trim());
                        if (fieldrow != null)
                        {
                            if (varietyListTableAdapter.FillByItemId(varietyListDataTable,fieldrow.Variety_Id) > 0)
                            {

                                var varietyRow = varietyListDataTable[0];

                                lblNewHeader.Text = $"Edit Field {fieldrow.Field} ";
                                hfEditUID.Value = fieldrow.UID.ToString();
                                pnlNew.CssClass = "px-2";
                                pnlNew.Visible = true;
                                pnlFields.Visible = false;
                                ddCrop.DataBind();
                                ddCrop.ClearSelection();
                                ddCrop.Items.FindByValue(varietyRow.Crop_Prefix).Selected=true ; 
                                ddVariety.DataBind();
                                ddVariety.ClearSelection();
                                ddVariety.Items.FindByValue(varietyRow.Item_Id.ToString() ).Selected = true;
                                txtField.Text = fieldrow.Field;
                                txtField.Focus();
                                btnAddNew.Visible = false;
                                
                            }
                        }

                    }
                }

            }
        }




        hfSelectedField.Value = Field;
        lblDeleteHeader.Text = $"Delete Field {Field}";
    }
}