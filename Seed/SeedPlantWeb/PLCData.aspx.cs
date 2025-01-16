using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PLCData : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        grdBins.DataSource = PLC.plcDataset.Bins;
        grdStats.DataSource = PLC.plcDataset.BatchTypeAvailability;
        grdTreatments.DataSource = PLC.plcDataset.Treatments;
        grdBins.DataBind();  
        grdStats.DataBind();
        grdTreatments.DataBind();

    }
}