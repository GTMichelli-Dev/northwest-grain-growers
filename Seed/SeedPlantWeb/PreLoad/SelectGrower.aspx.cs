using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SelectGrower : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {

     
            if ((Request.QueryString["Reset"] != null) || (SeedTicketInfo.seedTicketDataSet == null))
            {
                SeedTicketInfo.CreateSeedTicket();
            }
            if (!SeedTicketInfo.CurrentSeedTicketRow.IsGrower_NameNull())
            {
               txtgrower.Text = SeedTicketInfo.CurrentSeedTicketRow.Grower_Name;
            }
            txtgrower.Focus();
        }
   


     

    }




    private bool GrowerHasHistory(int Grower_Id)
    {
        bool retval = false;
        using (ListDataSetTableAdapters.QueriesTableAdapter Q = new ListDataSetTableAdapters.QueriesTableAdapter())
        {
            var val = Q.CountOfRecordsByLocationGrower(GlobalVars.Location, Grower_Id);
            if (val != null)
            {
                int cnt = (int)val;
                retval = cnt > 0;
            }
        }
        return retval;
    }





    protected void btnOk_Click(object sender, EventArgs e)
    {

        using (ListDataSet.ProducersListDataTable Producers = new global::ListDataSet.ProducersListDataTable())
        {
            using (ListDataSetTableAdapters.ProducersListTableAdapter producersListTableAdapter = new ListDataSetTableAdapters.ProducersListTableAdapter())
            {
                if (producersListTableAdapter.FillByFullName(Producers, txtgrower.Text ) > 0)
                {
                    var row = Producers[0];
                    if (SeedTicketInfo.seedTicketDataSet == null)
                    {
                        SeedTicketInfo.CreateSeedTicket();
                    }





                    SeedTicketInfo.CurrentSeedTicketRow.Grower_ID = row.Id;
                    SeedTicketInfo.CurrentSeedTicketRow.Grower_Name =row.Description;
                    Response.Redirect("~/Preload/TicketCreator.aspx");

                }
            }
        }



      
    }

    


    protected void btnNotFound_Click(object sender, EventArgs e)
    {
        string Reset = (Request.QueryString["Reset"] == null) ? true.ToString() : Request.QueryString["Reset"].ToString();
        Response.Redirect($"UnknownGrower?Reset={Reset}&GrowerName={txtgrower.Text }");
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("/default.aspx");

        //if ((Request.QueryString["Reset"] != null) || (SeedTicketInfo.seedTicketDataSet == null))
        //{
        //    Response.Redirect($"UnknownGrower?Reset={Reset}&GrowerName={txtgrower.Text }");
        //}
        //else
        //{

        //}
    }
}
