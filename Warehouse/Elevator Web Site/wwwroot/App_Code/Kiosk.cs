using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
/// Summary description for Kiosk
/// </summary>
[WebService(Namespace = "http://NWGGSCALE.NWG/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class Kiosk : System.Web.Services.WebService
{

    public Kiosk()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    public enum TicketStatus
    {
        Invalid,
        InvalidScaleUID,
        CheckInboundTicket,
        Complete,
        OldTicket,
        InvalidLocation,
        WeightToLow,
        TruckNotUnloaded,
        ReadyToComplete
    }




    public class TicketData
    {
        long ticket = -1;
        int location = -1;
        int sequence = -1;
        bool isValid = false;
        bool inbound = false;
        bool receivedCodeTooOld = false;
        bool receivedLocationIDValid = true;

        ReceivingQRData receivingQRData = null;



        public TicketData(string Ticket)
        {
            try
            {

                //10220100

                if (Ticket.Contains("LOADUID"))
                {
                    receivingQRData = new ReceivingQRData();
                    inbound = true;
                    Ticket = Ticket.Replace("\n", "").Replace("\n", "");
                    var split = Ticket.Split(',');
                    foreach (var item in split)
                    {
                        var itemSplit = item.Split('=');
                        if (itemSplit.Length > 1)
                        {
                            if (item.Contains("UID"))
                            {
                                Guid LoadUID;
                                if (Guid.TryParse(itemSplit[1], out LoadUID))
                                {
                                    receivingQRData.LoadUID = LoadUID;
                                }
                            }
                            else if (item.Contains("LOADID"))
                            {
                                long LoadID = 0;
                                if (long.TryParse(itemSplit[1], out LoadID))
                                {
                                    receivingQRData.LoadID = LoadID;
                                }
                            }

                            else if (item.Contains("LOCATIONID"))
                            {
                                int LocationID = 0;
                                if (int.TryParse(itemSplit[1], out LocationID))
                                {
                                    receivingQRData.LocationID = LocationID;
                                }
                            }

                            else if (item.Contains("TIMEOUT"))
                            {
                                DateTime dt;
                                if (DateTime.TryParse(itemSplit[1], out dt))
                                {
                                    receivingQRData.TimeOut=dt;
                                }
                            }
                        }
                    }
                    // receivingQRData = JsonConvert.DeserializeObject<ReceivingQRData>(Ticket);

                    using (LocalDataSet.Site_SetupDataTable site_Setup = new LocalDataSet.Site_SetupDataTable())
                    {
                        using (LocalDataSetTableAdapters.Site_SetupTableAdapter site_SetupTableAdapter = new LocalDataSetTableAdapters.Site_SetupTableAdapter())
                        {
                            site_SetupTableAdapter.Fill(site_Setup);
                            var siteSetupRow = site_Setup.FirstOrDefault(x => x.Location_Id == receivingQRData.LocationID);
                            if (siteSetupRow == null)
                            {
                                receivedLocationIDValid = false;
                            }
                            receivedCodeTooOld = ((DateTime.Now - receivingQRData.TimeOut).TotalHours >= 5558);
                            isValid = (receivedLocationIDValid && !receivedCodeTooOld);
                        }
                    }


                }
                else
                {
                    Ticket = Ticket.Replace("\r", "").Replace("\n", "").Trim();
                    if (Ticket.Length >= 10)
                    {
                        string strLocation, strSequence, strTicket;
                        strLocation = Ticket.Substring(Ticket.Length - 9, 3);
                        strSequence = Ticket.Substring(0, Ticket.Length - 9);
                        strTicket = Ticket.Substring(Ticket.Length - 6, 6);
                        isValid = (int.TryParse(strLocation, out location) && long.TryParse(Ticket, out ticket) && int.TryParse(strSequence, out sequence));

                        //   Logging.Add_System_Log( String.Format("strLocation{0} strSequence{1} strTicket{2} isValid{3} Ticket{4}",strLocation ,strSequence,strTicket,isValid,Ticket ), String.Format("strLocation{0} strSequence{1} strTicket{2} isValid{3}",strLocation ,strSequence,strTicket,isValid )  , 11);


                    }
                }
            }
            catch (Exception ocrap)
            {
                Logging.Add_System_Log("Ticket Data Conversion Error<" + Ticket + ">", ocrap.Message, location);

            }
        }


        public ReceivingQRData ReceiviedQrData
        {
            get
            {
                return receivingQRData;
            }

        }


        public bool IsValid
        {
            get
            {
                return isValid;
            }

        }




        public bool ReceivedCodeTooOld
        {
            get
            {
                return receivedCodeTooOld;
            }

        }




        public bool ReceivedLocationIDValid
        {
            get
            {
                return receivedLocationIDValid;
            }

        }

        public bool IsInbound
        {
            get
            {
                return inbound;
            }

        }

        public long Ticket
        {
            get
            {
                return ticket;
            }
        }

        public int Location
        {
            get
            {
                return location;
            }
        }

        public int Sequence
        {
            get
            {
                return sequence;
            }
        }

    }





    [WebMethod]
    public LocalDataSet.Site_SetupDataTable GetSites()
    {
        using (LocalDataSetTableAdapters.Site_SetupTableAdapter site_SetupTableAdapter = new LocalDataSetTableAdapters.Site_SetupTableAdapter())
        {
            using (LocalDataSet.Site_SetupDataTable site_SetupDataTable = new global::LocalDataSet.Site_SetupDataTable())
            {
                site_SetupTableAdapter.Fill(site_SetupDataTable);
                return site_SetupDataTable;
            }
        }
    }



    //[WebMethod]
    //public bool PrintBasicTicket(string Description, int LocationId, int Weight, string Scale, string Printer )
    //{
    //   return  Printing.Print_Basic_Ticket(Server, Description, LocationId, Weight, Scale, Printer);
    //}


    [WebMethod]
    public LocalDataSet.Weigh_ScalesDataTable GetScales(DateTime CurrentDate)
    {


        return Scales.GetAllScales();
    }


    [WebMethod]
    public string ProcessTicket(string Ticket, int Weight, string Scale, string Printer)
    {
        string Result = "";
        try
        {



            bool ValidTicket = false;

            TicketData TicketData = new TicketData(Ticket);
            if (TicketData.IsValid)
            {
                bool TicketComplete = false;


                {

                    using (LocalDataSetTableAdapters.LoadsTableAdapter LoadsTableAdapter = new LocalDataSetTableAdapters.LoadsTableAdapter())
                    {

                        //  Logging.Add_System_Log("Processing Ticket #<", TicketData.Ticket.ToString());
                        Logging.Add_System_Log($"Processing Ticket Weight{Weight} Scale{Scale} Printer{Printer} TicketData.Location{TicketData.Location} TicketData.Ticket {TicketData.Ticket}", TicketData.Ticket.ToString());
                        using (LocalDataSet.LoadsDataTable Loads = new LocalDataSet.LoadsDataTable())
                        {
                            if (LoadsTableAdapter.FillByLoad_ID_Location(Loads, TicketData.Ticket, TicketData.Location) > 0)
                            {
                                ValidTicket = true;
                                TicketComplete = !Loads[0].IsTime_OutNull();
                                //       Logging.Add_System_Log("Load UID=", Loads[0].UID.ToString());
                                //     Logging.Add_System_Log("TicketComplete=", TicketComplete.ToString());
                                Result = "Ticket Already Complete";
                            }
                            if (ValidTicket)
                            {
                                string Type_Of_Load = "";
                                using (LocalDataSetTableAdapters.vwLoad_TypeTableAdapter vwLoad_TypeTableAdapter = new LocalDataSetTableAdapters.vwLoad_TypeTableAdapter())
                                {
                                    using (LocalDataSet.vwLoad_TypeDataTable LoadType = new LocalDataSet.vwLoad_TypeDataTable())
                                    {
                                        ValidTicket = vwLoad_TypeTableAdapter.FillByLoad_Id_Location(LoadType, TicketData.Ticket, TicketData.Location) > 0;
                                        if (ValidTicket) Type_Of_Load = LoadType[0].Type_Of_Load;
                                        ValidTicket = Type_Of_Load != "?";
                                    }
                                }

                                if (TicketComplete && ValidTicket)
                                {
                                    Result = "Reprinting Ticket";


                                    if (Type_Of_Load == "I")
                                    {
                                        Logging.Add_System_Log(" Printing.PrintInbound_Final_Ticket(this.Server, Loads[0].UID, Truck Scale);", Loads[0].UID.ToString());
                                        Printing.PrintInbound_Final_Ticket(this.Server, Loads[0].UID, TicketData.Location, Scale, Printer);
                                    }
                                    else if (Type_Of_Load == "O")
                                    {
                                        Printing.PrintOutbound_Final_Ticket(this.Server, Loads[0].UID, TicketData.Location, Scale, true, Printer);
                                    }
                                    else if (Type_Of_Load == "T")
                                    {
                                        Printing.PrintTransfer_Final_Ticket(this.Server, Loads[0].UID, TicketData.Location, Scale, Printer);
                                    }
                                }
                                else
                                {
                                    if (Weight > 100 && (DateTime.Now - Loads[0].Time_In).TotalMinutes > 2)
                                    {
                                        using (LocalDataSetTableAdapters.QueriesTableAdapter Q = new LocalDataSetTableAdapters.QueriesTableAdapter())
                                        {


                                            Q.Weigh_Out_By_Load_Id_Location_Id(TicketData.Ticket, TicketData.Location, DateTime.Now, Weight);

                                          


                                        
                                                Result = "Reprinting Ticket";

                                                if (Type_Of_Load == "I")
                                                {

                                                    Printing.PrintInbound_Final_Ticket(this.Server, Loads[0].UID, TicketData.Location, Scale, Printer);
                                                }
                                                else if (Type_Of_Load == "O")
                                                {
                                                    Printing.PrintOutbound_Final_Ticket(this.Server, Loads[0].UID, TicketData.Location, Scale, true, Printer);
                                                    Printing.PrintOutbound_Final_Office_Ticket(this.Server, Loads[0].UID, TicketData.Location);
                                                }
                                                else if (Type_Of_Load == "T")
                                                {
                                                    Printing.PrintTransfer_Final_Ticket(this.Server, Loads[0].UID, TicketData.Location, Scale, Printer);
                                                }
                                        
                                        }
                                    }
                                    else
                                    {
                                        Result = "Weight Too Low ";
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Logging.Add_System_Log(string.Format("Kiosk.ProcessTicket(string Ticket<{0}>, int Weight<{1}>, string Scale<{2}>, string Printer<{3}>)", Ticket, Weight, Scale, Printer), ex.Message, SiteSetup.Site_SetupRow.Location_Id);
        }
        return Result;
    }


    [WebMethod]
    public TicketStatus CheckInboundTicket(string Ticket, string ScaleUID)
    {
        Guid scaleUID = Guid.Empty;
        if (Guid.TryParse(ScaleUID, out scaleUID))
        {


            TicketData TicketData = new TicketData(Ticket);

            if (!TicketData.IsValid)
            {
                if (TicketData.ReceivedCodeTooOld)
                {
                    return TicketStatus.OldTicket;
                }
                else if (TicketData.ReceivedLocationIDValid)
                {
                    return TicketStatus.InvalidLocation;
                }
                else
                {
                    return TicketStatus.Invalid;
                }
            }
            else if (TicketData.IsInbound)
            {
                GlobalVars.lastInboundScan.Acknowledged = false;
                GlobalVars.lastInboundScan.PrintingTicket = false;
                GlobalVars.lastInboundScan.LoadUID = TicketData.ReceiviedQrData.LoadUID;
                GlobalVars.lastInboundScan.ScanTime = DateTime.Now;
                GlobalVars.lastInboundScan.ScaleUID = scaleUID;
                return TicketStatus.CheckInboundTicket;
            }
            else
            {
                return TicketStatus.Invalid;
            }
        }
        else
        {
            return TicketStatus.Invalid;
        }
    }



    public bool AcknowledgeLastInboundQR()
    {
        return GlobalVars.lastInboundScan.Acknowledged = true;
    }





    [WebMethod]
    public InboundScan GetLastScannedInboundQR()
    {

        return GlobalVars.lastInboundScan;
    }

    [WebMethod]
    public TicketStatus CheckTicket(string Ticket, int Weight)
    {
        TicketData TicketData = new TicketData(Ticket);
        if (Weight < 1000)
        {
            return TicketStatus.WeightToLow;
        }
        else if (!TicketData.IsValid)
        {
            return TicketStatus.Invalid;
        }
        else if (TicketData.IsInbound)
        {
            return TicketStatus.Invalid;
        }
        else
        {
            bool TicketComplete = false;
            using (LocalDataSetTableAdapters.QueriesTableAdapter Q = new LocalDataSetTableAdapters.QueriesTableAdapter())
            {
                using (LocalDataSetTableAdapters.LoadsTableAdapter LoadsTableAdapter = new LocalDataSetTableAdapters.LoadsTableAdapter())
                {
                    using (LocalDataSet.LoadsDataTable Loads = new LocalDataSet.LoadsDataTable())
                    {
                        if (LoadsTableAdapter.FillByLoad_ID_Location(Loads, TicketData.Ticket, TicketData.Location) > 0)
                        {
                            TicketComplete = !Loads[0].IsTime_OutNull();
                            if (Loads[0].IsTime_OutNull())
                            {
                                if (Math.Abs(Loads[0].Weight_In - Weight) < 1000)
                                {
                                    return TicketStatus.TruckNotUnloaded;
                                }
                                else if ((DateTime.Now - Loads[0].Time_In).TotalMinutes < 2)
                                {
                                    return TicketStatus.TruckNotUnloaded;
                                }
                                else
                                {
                                    return TicketStatus.ReadyToComplete;
                                }
                            }
                            else
                            {
                                if (Math.Abs((Loads[0].Time_Out - DateTime.Now).TotalMinutes) > 5)
                                {
                                    return TicketStatus.OldTicket;
                                }
                                else
                                {
                                    return TicketStatus.Complete;
                                }
                            }
                        }
                        else
                        {
                            return TicketStatus.Invalid;
                        }
                    }
                }
            }
        }
    }




    [WebMethod]
    public LocalDataSet.Site_PrintersDataTable GetPrinterList()
    {
        string ServerName = "";
        Printing.UpdatePrinters();
        using (LocalDataSetTableAdapters.QueriesTableAdapter Q = new LocalDataSetTableAdapters.QueriesTableAdapter())
        {
            ServerName = Q.ServerName();
        }

        using (LocalDataSetTableAdapters.Site_PrintersTableAdapter printersTableAdapter = new LocalDataSetTableAdapters.Site_PrintersTableAdapter())
        {
            using (LocalDataSet.Site_PrintersDataTable SitePrinters = new global::LocalDataSet.Site_PrintersDataTable())
            {
                printersTableAdapter.Fill(SitePrinters, ServerName);
                return SitePrinters;
            }
        }


    }


}
