using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SeedPlant_Traits : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
          
            SetColors();
        }
        
    }


    public void SetColors()
    {
        using (SeedColorDataSetTableAdapters.ColorsTableAdapter colorsTableAdapter = new SeedColorDataSetTableAdapters.ColorsTableAdapter())
        {
            using (SeedColorDataSet.ColorsDataTable colorsDataTable = new SeedColorDataSet.ColorsDataTable())
            {
                colorsTableAdapter.Fill(colorsDataTable);
                {
                    for (int i = 0; i < 10; i++)
                    {
                        var row = colorsDataTable.FirstOrDefault(x => x.ID == i);
                        if (row != null)
                        {
                            switch (i)
                            {
                                case 0:
                                    Button0.BackColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    Button0.ForeColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    break;
                                case 1:
                                    Button1.BackColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    Button1.ForeColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    break;
                                case 2:
                                    Button2.BackColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    Button2.ForeColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    break;
                                case 3:
                                    Button3.BackColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    Button3.ForeColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    break;
                                case 4:
                                    Button4.BackColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    Button4.ForeColor = System.Drawing.ColorTranslator.FromHtml(row.Color);

                                    break;
                                case 5:
                                    Button5.BackColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    Button5.ForeColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    break;
                                case 6:
                                    Button6.BackColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    Button6.ForeColor = System.Drawing.ColorTranslator.FromHtml(row.Color);

                                    break;
                                case 7:
                                    Button7.BackColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    Button7.ForeColor = System.Drawing.ColorTranslator.FromHtml(row.Color);

                                    break;
                                case 8:
                                    Button8.BackColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    Button8.ForeColor = System.Drawing.ColorTranslator.FromHtml(row.Color);

                                    break;
                                case 9:
                                    Button9.BackColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    Button9.ForeColor = System.Drawing.ColorTranslator.FromHtml(row.Color);

                                    break;

                                default:
                                    break;
                            }
                        }
                    }
                }
            }
        }
        UPColor.Update();
    }






    protected void btnSetColor_Click(object sender, EventArgs e)
    {
        Button txt = (Button)sender;
        GridViewRow row = (GridViewRow)txt.NamingContainer;
        var UID = Guid.Parse(GridView1.DataKeys[row.RowIndex].Values[0].ToString());
        var ID = GridView1.DataKeys[row.RowIndex].Values[1].ToString();
        var Description = GridView1.DataKeys[row.RowIndex].Values[2].ToString();
        hfUID.Value = UID.ToString();
        hfID.Value = ID;
        lblColorPopupHeader.Text = Description;
        UPColorPopUpHeader.Update();
        using (SeedColorDataSetTableAdapters.TraitColorsTableAdapter traitColorsTableAdapter = new SeedColorDataSetTableAdapters.TraitColorsTableAdapter())
        {
            using (SeedColorDataSet.TraitColorsDataTable traitColorsDataTable = new SeedColorDataSet.TraitColorsDataTable())
            {
                traitColorsTableAdapter.Fill(traitColorsDataTable);
                var vrow = traitColorsDataTable.FindByUID(UID);
                if (vrow != null)
                {
                    if (vrow != null)
                    {

                        lblPrimaryPopupColor.Text = vrow.Color_Index.ToString();
                        lblPrimaryPopupColor.BackColor = System.Drawing.ColorTranslator.FromHtml(vrow.Color);
                        txtPrimaryDuration.Text = vrow.Duration.ToString();


                    }
                    else
                    {
                        pnlPrimaryValues.Visible = false;
                        lblPrimaryPopupColor.Text = "?";
                        lblPrimaryPopupColor.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                    }
                    UPColor.Update();
                }
            }
            ShowColorPopup();
        }
    }




    public void ShowColorPopup()
    {
        pnlNew.Visible = false;
        txtNewTraitDescription.Text = "";
        txtNewTraitID.Text = "";
        btnOK.Visible = false;
        UPColorPopUpHeader.Update();
        UPFooter.Update();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "ColorPopup", "$('#ColorPopup').modal('show')", true);
    }



    protected void ColorButton_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        int ID = int.Parse(btn.Text);
        Guid UID = Guid.Parse(hfUID.Value);
        int ItemId = int.Parse(hfID.Value);
        int primaryDuration = 5;

        int.TryParse(txtPrimaryDuration.Text, out primaryDuration);
        if (primaryDuration < 5) primaryDuration = 5;

        using (SeedColorDataSetTableAdapters.TraitTableAdapter traitTableAdapter = new SeedColorDataSetTableAdapters.TraitTableAdapter())
        {
            using (SeedColorDataSet.TraitDataTable traitDataTable = new SeedColorDataSet.TraitDataTable())
            {
                SeedColorDataSet.TraitRow  row;
                traitTableAdapter.Fill(traitDataTable);
                row = traitDataTable.FindByUID(UID);
                if (row != null)
                {

                    row.Color_Index = ID;
                    row.Duration = primaryDuration;
                    pnlPrimaryValues.Visible = true;

                    traitTableAdapter.Update(traitDataTable);
                    lblPrimaryPopupColor.BackColor = btn.BackColor;
                    lblPrimaryPopupColor.Text = ID.ToString();
                    txtPrimaryDuration.Text = primaryDuration.ToString();

                    pnlPrimaryDuration.Visible = true;
                }
                UPColor.Update();
                GridView1.DataBind();
            }
        }


    }





   


  

    protected void btnColorAccept_Click(object sender, EventArgs e)
    {

    }

    protected void SqlSeedPlantItems_DataBinding(object sender, EventArgs e)
    {

    }

  

    protected void txtPrimaryDuration_TextChanged(object sender, EventArgs e)
    {

    }




    protected void btnOK_Click(object sender, EventArgs e)
    {

    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        pnlNew.Visible = true;
        txtNewTraitDescription.Text = "";
        txtNewTraitID.Text = "";
        btnOK.Visible = true;
        UPColorPopUpHeader.Update();
        UPFooter.Update();

        ScriptManager.RegisterStartupScript(this, this.GetType(), "ColorPopup", "$('#ColorPopup').modal('show')", true);
    }
}