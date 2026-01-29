using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PreLoad_History : System.Web.UI.Page
{

    public int GrowerId
    {
        
        get
        {
            int Id = -1;
                try
            {
                   if (Request.QueryString["GrowerId"] != null)
                {
                    int.TryParse(Request.QueryString["GrowerId"].ToString(), out Id);

                }

            }
            catch
            {
                
            }
            return Id;
        }

    }



    public string Name
    {

        get
        {
            string name = string.Empty;
            try
            {
                if (Request.QueryString["Name"] != null)
                {
                    name = Request.QueryString["Name"].ToString();
                }

            }
            catch
            {

            }
            return name;
        }
    }


    public int LocationId
    {

        get
        {
            int Id = -1;
            try
            {
                if (Request.QueryString["locationId"] != null)
                {
                    int.TryParse(Request.QueryString["locationId"].ToString(), out Id);

                }

            }
            catch
            {

            }
            return Id;
        }
    }










    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (GrowerId ==-1 ||LocationId==-1 || Name==string.Empty  )
        {
            Response.Redirect("SelectGrower.aspx");
        }
        else
        {
            lblName.Text = Name;
        }
    }

    protected void lnkSelect_Click(object sender, EventArgs e)
    {
        LinkButton  btn = (LinkButton)sender;
        GridViewRow row = (GridViewRow)btn.NamingContainer;
        int index = row.RowIndex;
        Guid UID = Guid.Parse(grdHistory.DataKeys[index].Values[0].ToString());
        using (SeedTicketDataSet HSeedTicketDataSet = new SeedTicketDataSet())
        {
            using (SeedTicketDataSetTableAdapters.Seed_TicketsTableAdapter HSeed_TicketsTableAdapter = new SeedTicketDataSetTableAdapters.Seed_TicketsTableAdapter())
            {
                if (HSeed_TicketsTableAdapter.FillByUID(HSeedTicketDataSet.Seed_Tickets,UID)>0)
                {
                    using (SeedTicketDataSetTableAdapters.Seed_Ticket_TreatmentsTableAdapter HSeed_Ticket_TreatmentsTableAdapter = new SeedTicketDataSetTableAdapters.Seed_Ticket_TreatmentsTableAdapter())
                    {
                        HSeed_Ticket_TreatmentsTableAdapter.FillBySeed_Ticket_UID(HSeedTicketDataSet.Seed_Ticket_Treatments, UID);
                    }
                    using (SeedTicketDataSetTableAdapters.Seed_Ticket_VarietiesTableAdapter HSeed_Ticket_VarietiesTableAdapter = new SeedTicketDataSetTableAdapters.Seed_Ticket_VarietiesTableAdapter())
                    {
                        HSeed_Ticket_VarietiesTableAdapter.FillBySeed_Ticket_UID(HSeedTicketDataSet.Seed_Ticket_Varieties, UID);
                    }
                    SeedTicketInfo.seedTicketDataSet.Clear();

                    //SeedTicketInfo.CurrentSeedTicketRow = null;
                    //SeedTicketInfo.CurrentSeedTicketTreatmentRow = null;
                    //SeedTicketInfo.CurrentSeedTicketVarietyRow = null;
                    //SeedTicketInfo.CurrentSeedTicketWeightRow = null;
                    SeedTicketInfo.seedTicketDataSet = new SeedTicketDataSet();
                    var newRow = SeedTicketInfo.seedTicketDataSet.Seed_Tickets.NewSeed_TicketsRow();

                    newRow.ItemArray = HSeedTicketDataSet.Seed_Tickets[0].ItemArray;


                    newRow.UID = Guid.NewGuid();
                    newRow.SetTicketNull();
                    newRow.SetCommentsNull();
                    newRow.SetPONull();
                    newRow.SetBOLNull();
                    newRow.Ticket_Date = DateTime.Now;
                    newRow.ReadOnly = false;
                    SeedTicketInfo.seedTicketDataSet.Seed_Tickets.AddSeed_TicketsRow(newRow);

                    foreach(var item in HSeedTicketDataSet.Seed_Ticket_Varieties )
                    {
                        var vRow = SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Varieties.NewSeed_Ticket_VarietiesRow();
                        vRow.ItemArray = item.ItemArray;
                        vRow.Seed_Ticket_UID = SeedTicketInfo.CurrentSeedTicketRow.UID;
                        vRow.UID = Guid.NewGuid();
                        vRow.SetCommentNull();
                        SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Varieties.AddSeed_Ticket_VarietiesRow(vRow);

                    } 

                    foreach (var item in HSeedTicketDataSet.Seed_Ticket_Treatments )
                    {
                        var tRow = SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Treatments.NewSeed_Ticket_TreatmentsRow();
                        tRow.ItemArray = item.ItemArray;
                        tRow.Seed_Ticket_UID = SeedTicketInfo.CurrentSeedTicketRow.UID;
                        tRow.UID = Guid.NewGuid();
                        tRow.SetCommentNull();
                        SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Treatments.AddSeed_Ticket_TreatmentsRow(tRow);
                    }
                    SeedTicketInfo.CurrentSeedTicketRow = newRow;
                    Response.Redirect("TicketCreator.aspx");

                }
                else
                {
                    Response.Redirect("SelectGrower.aspx");
                }
            }
        }
    }
}