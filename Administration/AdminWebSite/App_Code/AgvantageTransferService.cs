using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
/// Summary description for AgvantageTransferService
/// </summary>
[WebService(Namespace = "http://walws001/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class AgvantageTransferService : System.Web.Services.WebService
{

   

    public AgvantageTransferService()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    
    [WebMethod]
    public void RequestTransfer()
    {
        GlobalVars.RequestTransfer = true;
    }



    [WebMethod]
    public bool IsTransferRequired()
    {
        bool TransferRemoteRequest = GlobalVars.RequestTransfer;
        GlobalVars.RequestTransfer = false;
        return TransferRemoteRequest;
    }

   







}
