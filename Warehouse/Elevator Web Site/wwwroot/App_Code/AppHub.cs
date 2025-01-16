using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class AppHub : Hub
{
    public void Hello()
    {
        Clients.All.hello();
    }
}
