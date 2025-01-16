using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SeedPlant_Commodities : System.Web.UI.Page
{
    public string Location
    {

        get
        {
            string lc = "All Locations";
            if (Session["SeedLocation"] != null)
            {
                lc = Session["SeedLocation"].ToString();
            }
            return lc;
        }
        set
        {

            Session["SeedLocation"] = value;

        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
       if (! this.IsPostBack )
        {
            this.ddUsed.DataBind();
            hfUsed.Value = (ddUsed.SelectedIndex > 0) ? "" : "false";
            SetColors();
        }
        GridView1.Columns[4].Visible = (ddUsed.SelectedIndex > 0) ? true : false;
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
                                    Button10.BackColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    Button0.ForeColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    Button10.ForeColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    break;
                                case 1:
                                    Button1.BackColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    Button11.BackColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    Button1.ForeColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    Button11.ForeColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    break;
                                case 2:
                                    Button2.BackColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    Button12.BackColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    Button2.ForeColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    Button12.ForeColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    break;
                                case 3:
                                    Button3.BackColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    Button13.BackColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    Button3.ForeColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    Button13.ForeColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    break;
                                case 4:
                                    Button4.BackColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    Button14.BackColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    Button4.ForeColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    Button14.ForeColor = System.Drawing.ColorTranslator.FromHtml(row.Color);

                                    break;
                                case 5:
                                    Button5.BackColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    Button15.BackColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    Button5.ForeColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    Button15.ForeColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    break;
                                case 6:
                                    Button6.BackColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    Button16.BackColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    Button6.ForeColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    Button16.ForeColor = System.Drawing.ColorTranslator.FromHtml(row.Color);

                                    break;
                                case 7:
                                    Button7.BackColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    Button17.BackColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    Button7.ForeColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    Button17.ForeColor = System.Drawing.ColorTranslator.FromHtml(row.Color);

                                    break;
                                case 8:
                                    Button8.BackColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    Button18.BackColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    Button8.ForeColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    Button18.ForeColor = System.Drawing.ColorTranslator.FromHtml(row.Color);

                                    break;
                                case 9:
                                    Button9.BackColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    Button19.BackColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    Button9.ForeColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    Button19.ForeColor = System.Drawing.ColorTranslator.FromHtml(row.Color);

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
        using (SeedColorDataSetTableAdapters.ItemColorsTableAdapter  itemColorsTableAdapter = new SeedColorDataSetTableAdapters.ItemColorsTableAdapter())
        {
            using (SeedColorDataSet.ItemColorsDataTable itemColorsDataTable = new SeedColorDataSet.ItemColorsDataTable())
            {
                if (itemColorsTableAdapter.FillByUID(itemColorsDataTable, UID) > 0)
                {
                    var vrow = itemColorsDataTable[0];
                    lblPrimaryPopupColor.Text = (vrow.IsPrimary_Color_IndexNull()) ? "?" : vrow.Primary_Color_Index.ToString();
                    lblPrimaryPopupColor.BackColor = (vrow.IsPrimary_ColorNull()) ? System.Drawing.ColorTranslator.FromHtml("#ffffff") : System.Drawing.ColorTranslator.FromHtml(vrow.Primary_Color);
                    txtPrimaryDuration.Text = (vrow.IsPrimary_Color_DurationNull()) ? "0" : vrow.Primary_Color_Duration.ToString();
                    pnlPrimaryValues.Visible = !(vrow.IsPrimary_Color_IndexNull());
                    pnlSecondary.Visible = !vrow.IsPrimary_Color_IndexNull();
                    pnlSecondaryValues.Visible = (!vrow.IsSecondary_Color_IndexNull());
                    lblSecondaryPopupColor.Text = (vrow.IsSecondary_Color_IndexNull()) ? "?" : vrow.Secondary_Color_Index.ToString();
                    lblSecondaryPopupColor.BackColor = (vrow.IsSecondary_ColorNull()) ? System.Drawing.ColorTranslator.FromHtml("#ffffff") : System.Drawing.ColorTranslator.FromHtml(vrow.Secondary_Color);
                    txtSecondaryDuration.Text = (vrow.IsSecondary_Color_DurationNull()) ? "0" : vrow.Secondary_Color_Duration.ToString();
                    pnlPrimaryDuration.Visible = !vrow.IsSecondary_Color_IndexNull();
                }
                else
                {
                    pnlPrimaryValues.Visible = false;
                    lblPrimaryPopupColor.Text = "?";
                    lblPrimaryPopupColor.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                    pnlSecondary.Visible = false;
                }
                UPColor.Update();
            }
        }
        ShowColorPopup();
    }




    public void ShowColorPopup()
    {


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




        using (SeedColorDataSetTableAdapters.Seed_ColorsTableAdapter seed_ColorsTableAdapter = new SeedColorDataSetTableAdapters.Seed_ColorsTableAdapter())
        {
            using (SeedColorDataSet.Seed_ColorsDataTable seed_ColorsDataTable = new SeedColorDataSet.Seed_ColorsDataTable())
            {
                SeedColorDataSet.Seed_ColorsRow row;
                if (seed_ColorsTableAdapter.FillByItemUID(seed_ColorsDataTable, UID) == 0)
                {
                    row = seed_ColorsDataTable.NewSeed_ColorsRow();
                    row.UID = Guid.NewGuid();
                    row.Commodity_ID  = ItemId;

                }
                else
                {
                    row = seed_ColorsDataTable[0];
                }

                row.Primary_Color_Index = ID;
                row.Primary_Color_Duration = primaryDuration;
                if (seed_ColorsDataTable.Count == 0)
                {
                    seed_ColorsDataTable.AddSeed_ColorsRow(row);
                }
                pnlPrimaryValues.Visible = true;
                pnlSecondary.Visible = true;
                seed_ColorsTableAdapter.Update(seed_ColorsDataTable);
                lblPrimaryPopupColor.BackColor = btn.BackColor;
                lblPrimaryPopupColor.Text = ID.ToString();
                txtPrimaryDuration.Text = primaryDuration.ToString();
                pnlSecondaryValues.Visible = !row.IsSecondary_Color_IndexNull();
                pnlPrimaryDuration.Visible = !row.IsSecondary_Color_IndexNull();
                UPColor.Update();
                GridView1.DataBind();
            }
        }


    }





    protected void ColorButton2_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        int ID = int.Parse(btn.Text);
        Guid UID = Guid.Parse(hfUID.Value);
        int ItemId = int.Parse(hfID.Value);
        int Duration = 5;



        using (SeedColorDataSetTableAdapters.Seed_ColorsTableAdapter seed_ColorsTableAdapter = new SeedColorDataSetTableAdapters.Seed_ColorsTableAdapter())
        {
            using (SeedColorDataSet.Seed_ColorsDataTable seed_ColorsDataTable = new SeedColorDataSet.Seed_ColorsDataTable())
            {
                SeedColorDataSet.Seed_ColorsRow row;
                seed_ColorsTableAdapter.FillByItemUID(seed_ColorsDataTable, UID);
                {
                    row = seed_ColorsDataTable[0];
                }


                row.Secondary_Color_Index = ID;
                row.Secondary_Color_Duration = Duration;
                seed_ColorsTableAdapter.Update(seed_ColorsDataTable);
                lblSecondaryPopupColor.BackColor = btn.BackColor;
                lblSecondaryPopupColor.Text = ID.ToString();
                txtSecondaryDuration.Text = Duration.ToString();
                pnlSecondaryValues.Visible = !row.IsSecondary_Color_IndexNull();
                pnlPrimaryDuration.Visible = !row.IsSecondary_Color_IndexNull();
                UPColor.Update();

                GridView1.DataBind();
            }
        }

    }



    private void UpdateColors()
    {


    }

    protected void btnClearPrimaryColor_Click(object sender, EventArgs e)
    {
        Guid UID = Guid.Parse(hfUID.Value);
        int ItemId = int.Parse(hfID.Value);



        using (SeedColorDataSetTableAdapters.Seed_ColorsTableAdapter seed_ColorsTableAdapter = new SeedColorDataSetTableAdapters.Seed_ColorsTableAdapter())
        {
            using (SeedColorDataSet.Seed_ColorsDataTable seed_ColorsDataTable = new SeedColorDataSet.Seed_ColorsDataTable())
            {
                SeedColorDataSet.Seed_ColorsRow row;
                seed_ColorsTableAdapter.FillByItemUID(seed_ColorsDataTable, UID);
                {
                    row = seed_ColorsDataTable[0];
                }
                row.Delete();
                seed_ColorsTableAdapter.Update(seed_ColorsDataTable);
                lblPrimaryPopupColor.BackColor = System.Drawing.Color.White;
                lblPrimaryPopupColor.Text = "";
                pnlPrimaryValues.Visible = false;
                lblSecondaryPopupColor.BackColor = System.Drawing.Color.White;
                lblSecondaryPopupColor.Text = "";
                txtSecondaryDuration.Text = "0";
                pnlSecondaryValues.Visible = false;
                pnlSecondary.Visible = false;
                UPColor.Update();

                GridView1.DataBind();
            }
        }
    }

    protected void btnClearSecondaryColor_Click(object sender, EventArgs e)
    {
        Guid UID = Guid.Parse(hfUID.Value);
        int ItemId = int.Parse(hfID.Value);



        using (SeedColorDataSetTableAdapters.Seed_ColorsTableAdapter seed_ColorsTableAdapter = new SeedColorDataSetTableAdapters.Seed_ColorsTableAdapter())
        {
            using (SeedColorDataSet.Seed_ColorsDataTable seed_ColorsDataTable = new SeedColorDataSet.Seed_ColorsDataTable())
            {
                SeedColorDataSet.Seed_ColorsRow row;
                seed_ColorsTableAdapter.FillByItemUID(seed_ColorsDataTable, UID);
                {
                    row = seed_ColorsDataTable[0];
                }
                row.SetSecondary_Color_DurationNull();
                row.SetSecondary_Color_IndexNull();
                seed_ColorsTableAdapter.Update(seed_ColorsDataTable);
                lblSecondaryPopupColor.BackColor = System.Drawing.Color.White;
                lblSecondaryPopupColor.Text = "";
                txtSecondaryDuration.Text = "0";
                pnlSecondaryValues.Visible = false;
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

    protected void txtSecondaryDuration_TextChanged(object sender, EventArgs e)
    {

        Guid UID = Guid.Parse(hfUID.Value);
        int ItemId = int.Parse(hfID.Value);
        int Duration = 1000;



        using (SeedColorDataSetTableAdapters.Seed_ColorsTableAdapter seed_ColorsTableAdapter = new SeedColorDataSetTableAdapters.Seed_ColorsTableAdapter())
        {
            using (SeedColorDataSet.Seed_ColorsDataTable seed_ColorsDataTable = new SeedColorDataSet.Seed_ColorsDataTable())
            {
                SeedColorDataSet.Seed_ColorsRow row;
                seed_ColorsTableAdapter.FillByItemUID(seed_ColorsDataTable, UID);
                {
                    row = seed_ColorsDataTable[0];
                }
                row.Secondary_Color_Duration = Duration;
                seed_ColorsTableAdapter.Update(seed_ColorsDataTable);
                UPColor.Update();
                GridView1.DataBind();
            }
        }

    }

    protected void txtPrimaryDuration_TextChanged(object sender, EventArgs e)
    {

    }

    protected void ckSprintWheat_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox ck = (CheckBox)sender;
        GridViewRow row = (GridViewRow)ck.NamingContainer;
        var UID = Guid.Parse(GridView1.DataKeys[row.RowIndex].Values[0].ToString());
        using (SeedDataSetTableAdapters.Seed_DepartmentsTableAdapter seed_DepartmentsTableAdapter = new SeedDataSetTableAdapters.Seed_DepartmentsTableAdapter())
        {
            using (SeedDataSet.Seed_DepartmentsDataTable seed_DepartmentsDataTable = new SeedDataSet.Seed_DepartmentsDataTable())
            {
                seed_DepartmentsTableAdapter.Fill(seed_DepartmentsDataTable);
                var Itemrow = seed_DepartmentsDataTable.FindByUID(UID);
                if (Itemrow != null)
                {
                    Itemrow.Spring_Wheat = ck.Checked;
                    seed_DepartmentsTableAdapter.Update(seed_DepartmentsDataTable);
                }
            }
        }
    }



    protected void ckNotUsed_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox ck = (CheckBox)sender;
        GridViewRow row = (GridViewRow)ck.NamingContainer;
        var UID = Guid.Parse(GridView1.DataKeys[row.RowIndex].Values[0].ToString());
        using (SeedDataSetTableAdapters.Seed_DepartmentsTableAdapter seed_DepartmentsTableAdapter = new SeedDataSetTableAdapters.Seed_DepartmentsTableAdapter())
        {
            using (SeedDataSet.Seed_DepartmentsDataTable seed_DepartmentsDataTable = new SeedDataSet.Seed_DepartmentsDataTable())
            {
                seed_DepartmentsTableAdapter.Fill(seed_DepartmentsDataTable);
                var Itemrow = seed_DepartmentsDataTable.FindByUID(UID);
                if (Itemrow != null)
                {
                    Itemrow.Not_Used  = ck.Checked;
                    seed_DepartmentsTableAdapter.Update(seed_DepartmentsDataTable);
                }
            }
        }
    }

    protected void ddUsed_TextChanged(object sender, EventArgs e)
    {
        hfUsed.Value = (ddUsed.SelectedIndex > 0) ? "" : "false";
        this.GridView1.DataBind();
    }
}