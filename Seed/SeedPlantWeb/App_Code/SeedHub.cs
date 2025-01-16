using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

public class SeedHub : Hub
{
    public void GetCustomers()
    {
        using (ListDataSet.ProducersListDataTable Producers = new global::ListDataSet.ProducersListDataTable())
        {
            using (ListDataSetTableAdapters.ProducersListTableAdapter producersListTableAdapter = new ListDataSetTableAdapters.ProducersListTableAdapter())
            {
                producersListTableAdapter.FillAll(Producers);
                List<string> ProducerList = new List<string>();
                foreach (var item in Producers)
                {
                    ProducerList.Add(item.FullName );
                }
                Clients.Caller.setProducers(ProducerList.ToArray());
            }
        }
    }



    public void GetCustomerHistory(string CustomerName)
    {
        using (ListDataSet.ProducersListDataTable Producers = new global::ListDataSet.ProducersListDataTable())
        {
            using (ListDataSetTableAdapters.ProducersListTableAdapter producersListTableAdapter = new ListDataSetTableAdapters.ProducersListTableAdapter())
            {
               if ( producersListTableAdapter.FillByFullName(Producers,CustomerName )==0)
                {
                    Clients.Caller.customerNotFound();
                }
                else
                {
                    var row = Producers[0];
                    using (SeedTicketDataSet.Seed_TicketsDataTable seedTickets = new global::SeedTicketDataSet.Seed_TicketsDataTable())
                    {
                        using (SeedTicketDataSetTableAdapters.Seed_TicketsTableAdapter seed_TicketsTableAdapter = new SeedTicketDataSetTableAdapters.Seed_TicketsTableAdapter())
                        {
                            if (seed_TicketsTableAdapter.FillByHistory(seedTickets,row.Id,GlobalVars.Location )>0)
                            {
                                string link = $"History.aspx?GrowerId={row.Id}&locationId={GlobalVars.Location}&Name={CustomerName} ";
                                Clients.Caller.customerHasHistory(link);
                            }
                            else
                            {
                                Clients.Caller.customerOK();
                            }
                        }
                    }
                }
            }
        }
    }
}
