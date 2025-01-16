using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SeedPlant_Colors : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SetColors();
    }


    public void SetColors()
    {
        using (SeedColorDataSetTableAdapters.ColorsTableAdapter colorsTableAdapter = new SeedColorDataSetTableAdapters.ColorsTableAdapter())
        {
            using (SeedColorDataSet.ColorsDataTable colorsDataTable = new SeedColorDataSet.ColorsDataTable())
            {
                colorsTableAdapter.Fill(colorsDataTable);
                {
                    for (int i = 0; i < 9; i++)
                    {
                        var row = colorsDataTable.FirstOrDefault(x => x.ID == i);
                        if (row != null)
                        {
                            switch (i)
                            {
                                case 0:
                                    Button0.BackColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    break;
                                case 1:
                                    Button1.BackColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    break;
                                case 2:
                                    Button2.BackColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    break;
                                case 3:
                                    Button3.BackColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    break;
                                case 4:
                                    Button4.BackColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    break;
                                case 5:
                                    Button5.BackColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    break;
                                case 6:
                                    Button6.BackColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    break;
                                case 7:
                                    Button7.BackColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    break;
                                case 8:
                                    Button8.BackColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    break;
                                case 9:
                                    Button9.BackColor = System.Drawing.ColorTranslator.FromHtml(row.Color);
                                    break;

                                default:
                                    break;
                            }
                        }
                    }      
                }
            }
        }
    }
}