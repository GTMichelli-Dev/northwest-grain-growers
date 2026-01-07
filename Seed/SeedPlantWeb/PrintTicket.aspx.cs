using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PrintTicket : System.Web.UI.Page
{
    public Guid UID
    {
        get
        {
            Guid uid = Guid.Empty;
            if (Request.QueryString["UID"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }
            else
            {
                if (!Guid.TryParse(Request.QueryString["UID"].ToString(), out uid))
                {
                    Response.Redirect("~/Default.aspx");
                }
            }
            return uid;
        }
    }

    public int Copies
    {
        get
        {
            int Id = 1;
            if (Request.QueryString["Copies"] != null)
            {
                int.TryParse(Request.QueryString["Copies"].ToString(), out Id);
            }
            return Id;
        }
    }

    public bool NonSeed
    {
        get
        {
            return (Request.QueryString["NonSeed"] != null);
        }
    }

    public string TruckId
    {
        get
        {
            string Vehicle = "";
            if (Request.QueryString["Vehicle"] != null)
            {
                Vehicle = Request.QueryString["Vehicle"].ToString();
            }
            return Vehicle;
        }
    }

    public int Weight
    {
        get
        {
            int Weight = 0;
            if (Request.QueryString["Weight"] != null)
            {
                int.TryParse(Request.QueryString["Weight"].ToString(), out Weight);
            }
            return Weight;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            if (NonSeed)
            {
                lblHeader.Text = "Printing Non Seed ticket";
            }
            else
            {
                using (SeedTicketDataSetTableAdapters.Seed_TicketsTableAdapter seed_TicketsTableAdapter = new SeedTicketDataSetTableAdapters.Seed_TicketsTableAdapter())
                {
                    using (SeedTicketDataSet.Seed_TicketsDataTable seed_TicketsDataTable = new SeedTicketDataSet.Seed_TicketsDataTable())
                    {
                        if (seed_TicketsTableAdapter.FillByUID(seed_TicketsDataTable, UID) == 0)
                        {
                            Response.Redirect("~/Default.aspx");
                        }
                        else
                        {
                            lblHeader.Text = (Copies == 1) ? $"Printing Ticket {seed_TicketsDataTable[0].Ticket}" : $"Printing {Copies} Copies Of Ticket {seed_TicketsDataTable[0].Ticket}";
                        }
                    }
                }
            }
        }
    }

    protected void ddlCopies_SelectedIndexChanged(object sender, EventArgs e)
    {
        hfCnt.Value = ddlCopies.SelectedValue;
        tmrPrint.Enabled = true;
    }




    protected void btnHidden_Click(object sender, EventArgs e)
    {
        hfCnt.Value = ddlCopies.SelectedValue;
        int copies = int.Parse(hfCnt.Value);
        if (NonSeed)
        {
            Printing.PrintNonSeedTicket(TruckId, Weight, copies);
        }
        else
        {
            Printing.Print_Ticket(UID, GlobalVars.ReportPrinter, copies);
        }

        System.Threading.Thread.Sleep(3000);
        Response.Redirect("~/Default.aspx");
    }



    protected void tmrPrint_Tick(object sender, EventArgs e)
    {
        tmrPrint.Enabled = false;
        int copies = int.Parse(hfCnt.Value);
        if (NonSeed)
        {
            Printing.PrintNonSeedTicket(TruckId, Weight, copies);
        }
        else
        {
            Printing.Print_Ticket(UID, GlobalVars.ReportPrinter, copies);
        }

        System.Threading.Thread.Sleep(3000);
        Response.Redirect("~/Default.aspx");
    }
}
