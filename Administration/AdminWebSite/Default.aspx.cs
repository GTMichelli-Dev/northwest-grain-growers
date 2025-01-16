using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{

    public DateTime SelectedDate
    {

        get
        {
            DateTime dt = DateTime.Now;
            if (!string.IsNullOrWhiteSpace(hfSelectedDate.Value.ToString()))
            {
                DateTime.TryParse(hfSelectedDate.Value.ToString(), out dt);

            }
            else
            {
                this.hfSelectedDate.Value = DateTime.Now.ToString() ;
            }
            
            return dt;
        }
        set
        {

            hfSelectedDate.Value  = value.ToString();

        }
    }



    protected void Page_Load(object sender, EventArgs e)
    {





        if (!this.IsPostBack)
        {
            DateTime dt = DateTime.Now;

            hfSelectedDate.Value = dt.ToShortDateString();
            txtSelectedDate.Text = dt.ToShortDateString();
            SetUpdateLabel();
        }
        this.GrdTotals.DataBind();
        this.GridView1.DataBind();
            



        //    int LoadCount, Weight, Bushels;
        //LoadCount = 0;
        //Weight = 0;
        //Bushels = 0;
        ////lblIntakeTotals.Text = "No Intake Loads Today";
        ////lbltransferTotals.Text = "No Transfer Loads Today";

        
        ////if (this.IsPostBack)
        //{
        //    using (TallyDataset tTallyDataset = new TallyDataset())
        //    {
             
        //        using (TallyDatasetTableAdapters.DailyIntakeTotalForAllSitesTableAdapter dailyIntakeTotalForAllSitesTableAdapter = new TallyDatasetTableAdapters.DailyIntakeTotalForAllSitesTableAdapter())
        //        {
                 
        //            if (dailyIntakeTotalForAllSitesTableAdapter.Fill(tTallyDataset.DailyIntakeTotalForAllSites,SelectedDate )>0)
        //            {
        //                TallyDataset.DailyIntakeTotalForAllSitesRow row = tTallyDataset.DailyIntakeTotalForAllSites[0];
        //                if (row.Loads > 0)
        //                {
        //                 //   lblIntakeTotals.Text = string.Format("# Intake Loads:<strong>{0:N0}</strong> Total Tons:<strong>{1:N0}</strong> Total Bushels:<strong>{2:N0}</strong>", row.Loads, row.Net / 2000, row.Bushels);
        //                    LoadCount = row.Loads;
        //                    Weight = row.Net;
        //                    Bushels = row.Bushels;
        //                }
        //            }
                 
        //        }


        //        using (TallyDatasetTableAdapters.DailyTransferTotalsForAllSitesTableAdapter  dailyTransferTotalForAllSitesTableAdapter = new TallyDatasetTableAdapters.DailyTransferTotalsForAllSitesTableAdapter())
        //        {

        //            if (dailyTransferTotalForAllSitesTableAdapter.Fill(tTallyDataset.DailyTransferTotalsForAllSites,SelectedDate ) > 0)
        //            {
        //                    TallyDataset.DailyTransferTotalsForAllSitesRow row = tTallyDataset.DailyTransferTotalsForAllSites[0];
        //                if (row.Loads > 0)
        //                {

        //                  //  lbltransferTotals.Text = string.Format("# Transfer Loads:<strong>{0:N0}</strong> Total Tons:<strong>{1:N0}</strong> Total Bushels:<strong>{2:N0}</strong>", row.Loads, row.Net / 2000, row.Bushels);
        //                    LoadCount += row.Loads;
        //                    Weight += row.Net;
        //                    Bushels += row.Bushels;
        //                }
        //            }
              
        //        }
        //        lblTotals.Visible = (LoadCount > 0);

        //        lblTotals.Text = string.Format("#Loads:<strong>{0:N0}</strong> Total Tons:<strong>{1:N0}</strong> Total Bushels:<strong>{2:N0}</strong>", LoadCount , Weight/2000, Bushels);

        //    }

        //    if ((SelectedDate - DateTime.Now).TotalDays == 0)
        //    {
        //        lblLastUpdate.Text = string.Format("Daily Totals Updated: <strong> {0}</strong>", DateTime.Now);

        //    }
        //    else
        //    {
        //        lblLastUpdate.Text = string.Format("Daily Totals For: <strong> {0}</strong>", SelectedDate.ToShortDateString());
                
        //    }
            ////this.GridView1.DataBind();
            //this.grdIntake.DataBind();
            //this.grdTransfer.DataBind();
        //}
    }

    private void CheckDate( HiddenField hf)
    {
        
        DateTime dt = DateTime.Now;
        if (DateTime.TryParse(txtSelectedDate.Text, out dt))
        {
            hf.Value = dt.ToShortDateString();
            txtSelectedDate.Text = dt.ToShortDateString();
            

        }
        else
        {
            txtSelectedDate.Text = hf.Value;
        }
    }

    protected void txtSelectedDate_TextChanged(object sender, EventArgs e)
    {
        CheckDate( hfSelectedDate);
    }

    protected void tmrUpdate_Tick(object sender, EventArgs e)
    {
        SetUpdateLabel();
    }

    public void SetUpdateLabel()
    {
        DateTime dt = DateTime.Now;
        if (DateTime.TryParse(txtSelectedDate.Text, out dt))
        {
          
          if ((int)((dt-DateTime.Now).TotalDays) ==0)
            {
                lblLastUpdate.Text = "Last Updated " + DateTime.Now.ToString();
            }
          else
            {
                lblLastUpdate.Text = "Totals";
            }


        }
        
    }
}