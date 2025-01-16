using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack )
        {
            Session["LastUpdate"] = Scales.LastScaleModification;
            this.grdScales.DataBind();
            using (LocalDataSetTableAdapters.QueriesTableAdapter Q = new LocalDataSetTableAdapters.QueriesTableAdapter())
            {
                 this.lblHeader.Text = "Scales for "+Q.ServerName().ToString() ;
            }

            UpdateScaleValues(); 
        }


        HtmlMeta metaKeywords = new HtmlMeta();
        metaKeywords = new HtmlMeta();
        metaKeywords.HttpEquiv = "Refresh";
        metaKeywords.Content = "20";
        this.Header.Controls.Add(metaKeywords);


    }

    private void UpdateScaleValues()
    {
        this.lblLastUpdate.Text = "Last Updated " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
        //if ((Session["LastUpdate"]==null) || (DateTime)Session["LastUpdate"] != Scales.LastScaleModification)
        //{
        //    Scales.UpdateScales();
        //    Session["LastUpdate"] = Scales.LastScaleModification;
        //}


        foreach (GridViewRow row in grdScales.Rows)
        {

            Label lblScale = (Label)row.FindControl("lblDescription");
            if (lblScale != null)
            {
                Label lblWeight = (Label)row.FindControl("lblWeight");
                Label lblStatus = (Label)row.FindControl("lblStatus");
                Label lblLastUpdate = (Label)row.FindControl("lblLastUpdate");
                HiddenField hfLocation_Id = (HiddenField)row.FindControl("hfLocation_Id");
                int Location_Id;
                int.TryParse (hfLocation_Id.Value,out Location_Id );
                LocalDataSet.Weigh_ScalesRow ScaleRow = Scales.GetScale(lblScale.Text,Location_Id );
                if (ScaleRow != null)
                {
                    lblStatus.Text = Scales.StatusString(ScaleRow);
                    lblWeight.Text = string.Format("{0:N0} lbs.", ScaleRow.Weight);
                    lblLastUpdate.Text = ScaleRow.Last_Update.ToString();
                    if (Scales.ValidateScaleOK(ScaleRow))
                    {
                        row.BackColor = System.Drawing.Color.White;

                    }
                    else
                    {
                        row.BackColor = System.Drawing.Color.Pink;
                    }
                }
                else
                {
                    lblStatus.Text = "Scale Row Null";
                    lblWeight.Text = "0";
                    row.BackColor = System.Drawing.Color.Pink;
                }
            }

        }

    }

    protected void tmrUpdate_Tick(object sender, EventArgs e)
    {
        UpdateScaleValues();
    }
}