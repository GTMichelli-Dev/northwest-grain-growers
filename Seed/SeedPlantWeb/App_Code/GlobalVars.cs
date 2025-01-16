using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for GlobalVars
/// </summary>
public class GlobalVars
{
    public GlobalVars()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public const string UnknownItem = "UNKNOWN";





    public enum enumConfirmType
    {
        WeighMaster,
        growerNameInvalid,
        growerUnknown,
        varietyUnknown

    }


    public enum enumGetWeighType
    {
        WeighIn,
        WeighOut
    }





    public static bool PLCVarietiesAvailable
    {
        get
        {
            if (GlobalVars.PlcVarieties == null)
            {
                return false;
            }
            else
            {
                return (GlobalVars.PlcVarieties.Count > 0);
            }

        }
    }



    public static bool PLCTreatmentsAvailable
    {
        get
        {
            if (GlobalVars.PlcTreatments == null)
            {
                return false;
            }
            else
            {
                return (GlobalVars.PlcTreatments.Count > 0);
            }

        }
    }

    public class BinItems
    {
        public int BinNumber { get; set; }
        public int ItemNumber { get; set; }
        public string Bin { get; set; }
    }


    public static List<BinItems> PlcVarieties
    {
        get;

        set;

    }


    public static List<int> PlcTreatments
    {
        get;
        set;
    }






    public static string PlcTreatmentsCsvList
    {
        get
        {
            string retval = "";
            List<int> items = new List<int>();
            foreach (var item in PlcTreatments)
            {
                if (items.FindIndex(x => x == item) < 0)
                {
                    items.Add(item);
                }

            }

            foreach (var item in items)
            {
                retval += item.ToString() + ",";
            }

            if (retval.EndsWith(",")) retval.Remove(retval.Length - 1);
            return retval;
        }
    }







    public static string PlcVarietyCsvList
    {
        get
        {
            string retval = "";
            List<int> items = new List<int>();
            foreach (var item in PlcVarieties)
            {
                if (items.FindIndex(x => x == item.ItemNumber) < 0)
                {
                    items.Add(item.ItemNumber);
                }

            }

            foreach (var item in items)
            {
                retval += item.ToString() + ",";
            }

            if (retval.EndsWith(",")) retval.Remove(retval.Length - 1);
            return retval;
        }
    }



    public static List<BinItems> PLCBins(int ItemId)
    {

        List<BinItems> Bins = new List<BinItems>();
        foreach (var item in PlcVarieties)
        {
            if (item.ItemNumber == ItemId)
            {
                Bins.Add(item);
            }

        }

        if (Bins.Count > 1)
        {
            BinItems b = new BinItems();
            b.Bin = "Select Bin";
            b.ItemNumber = -1;
            b.BinNumber = -1;

            Bins.Insert(0, b);
        }

        return Bins;
    }


    public static bool UsePlcVarieties
    {
        get
        {
            if (HttpContext.Current.Session["UsePlcVarieties"] == null)
            {
                return false;
            }
            else
            {
                try
                {
                    return (bool)HttpContext.Current.Session["UsePlcVarieties"];
                }
                catch
                {
                    return false;
                }
            }
        }
        set
        {
            HttpContext.Current.Session["UsePlcVarieties"] = value;
        }
    }







    public static enumSeedTicketWeighType SeedTicketWeighType
    {
        get
        {
            if (HttpContext.Current.Session["SeedTicketWeighType"] == null)
            {
                return enumSeedTicketWeighType.Truck;
            }
            else
            {
                try
                {
                    return (enumSeedTicketWeighType)HttpContext.Current.Session["SeedTicketWeighType"];
                }
                catch
                {
                    return enumSeedTicketWeighType.Truck;
                }
            }
        }
        set
        {
            HttpContext.Current.Session["SeedTicketWeighType"] = value;
        }
    }



    public static string Weighmaster
    {
        get
        {
            if (HttpContext.Current.Session["Weighmaster"] == null)
            {
                return string.Empty;
            }
            else
            {
                try
                {
                    return HttpContext.Current.Session["Weighmaster"].ToString();
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
        set
        {
            HttpContext.Current.Session["Weighmaster"] = value;
        }
    }



    const string usePrinter = "usePrinter";
    const string seedPlantLocation = "seedPlantLocation";
    const string reportPrinter = "reportPrinter";
    const string ticketPrinter = "ticketPrinter ";
    const string plcAddress = "plcAddress";
    const string locationDescription = "locationDescription";



    public enum enumSeedTicketWeighType
    {
        Truck,
        Tote,
        Bag,
        ReturnBulk,
        ReturnTote,
        ReturnBag

    }


    public static int Location
    {
        get
        {
            int value = 0;
            int.TryParse(Cookies.GetFromCookie(seedPlantLocation, ""), out value);

            return value;
        }
        set
        {
            var Plc_Address = string.Empty;
            var Report_Printer = string.Empty;
            var Location_Description = string.Empty;
            var Ticket_Printer = string.Empty;

            Cookies.StoreInCookie(seedPlantLocation, "", "", value.ToString(), DateTime.Now.AddYears(20));
            if (value != 0)
            {
                SetupDataSet.LocationsRow row = GetLocationRow(value);
                if (row != null)
                {
                    Plc_Address = row.PLC_PC_Address;
                    Report_Printer = (row.IsReport_PrinterNull()) ? string.Empty : row.Report_Printer;
                    Location_Description = row.Description;
                    Ticket_Printer = (row.IsTicket_PrinterNull()) ? string.Empty : row.Ticket_Printer;
                }
            }

            Cookies.StoreInCookie(reportPrinter , "", "", Report_Printer, DateTime.Now.AddYears(20));
            Cookies.StoreInCookie(ticketPrinter, "", "", Ticket_Printer, DateTime.Now.AddYears(20));
            Cookies.StoreInCookie(plcAddress, "", "", Plc_Address, DateTime.Now.AddYears(20));
            Cookies.StoreInCookie(locationDescription, "", "", Location_Description, DateTime.Now.AddYears(20));
        }
    }


    public static string ReportPrinter
    {
        get
        {
            string value = string.Empty;
            value = Cookies.GetFromCookie(reportPrinter,"");

            return value;
        }
        set
        {
            var row = GetLocationRow(Location);
            if (string.IsNullOrEmpty(value))
            {
                row.SetReport_PrinterNull();
            }
            else
            {
                row.Report_Printer = value;
            }
            SaveLocationRow(row);

        }
    }





    public static bool UsePrinter
    {
        get
        {
            bool  value = false;
            if (!bool.TryParse(Cookies.GetFromCookie(usePrinter, ""), out value))
            {
                Cookies.StoreInCookie(usePrinter, "", "", value.ToString(), DateTime.Now.AddYears(20));
            }
            return value;
        }
        set
        {
            Cookies.StoreInCookie(usePrinter, "", "", value.ToString(), DateTime.Now.AddYears(20));
      
           

        }
    }




    public static string PlcAddress
    {
        get
        {
            string value = string.Empty;
            value = Cookies.GetFromCookie(plcAddress,"");

            return value;
        }
        set
        {
            var row = GetLocationRow(Location);
            row.PLC_PC_Address = value;
            SaveLocationRow(row);


        }
    }


    public static string LocationDescription
    {
        get
        {
            string value = string.Empty;
            value = Cookies.GetFromCookie(locationDescription,"");

            return value;
        }

    }



    public static SetupDataSet.LocationsRow GetLocationRow(int LocationID)
    {
        SetupDataSet.LocationsRow row = null;

        using (SetupDataSet.LocationsDataTable locationsDataTable = new SetupDataSet.LocationsDataTable())
        {
            using (SetupDataSetTableAdapters.LocationsTableAdapter locationsTableAdapter = new SetupDataSetTableAdapters.LocationsTableAdapter())
            {
                if (locationsTableAdapter.FillByID(locationsDataTable, LocationID) > 0)
                {
                    row= locationsDataTable[0];
                }
            }
        }
        return row;
    }


    public static void SaveLocationRow(SetupDataSet.LocationsRow row)
    {






        using (SetupDataSet.LocationsDataTable locationsDataTable = new SetupDataSet.LocationsDataTable())
        {
            using (SetupDataSetTableAdapters.LocationsTableAdapter locationsTableAdapter = new SetupDataSetTableAdapters.LocationsTableAdapter())
            {
                var Plc_Address = string.Empty;
                var Report_Printer = string.Empty;
                var Location_Description = string.Empty;
                var Ticket_Printer = string.Empty;

                if (locationsTableAdapter.FillByID(locationsDataTable, row.ID) > 0)
                {
                    var fRow = locationsDataTable[0];
                    fRow.ItemArray = row.ItemArray;
                    locationsTableAdapter.Update(locationsDataTable);
                    Plc_Address = row.PLC_PC_Address;
                    Report_Printer = (row.IsReport_PrinterNull()) ? string.Empty : row.Report_Printer;
                    Location_Description = row.Description;
                    Ticket_Printer = (row.IsTicket_PrinterNull()) ? string.Empty : row.Ticket_Printer;
                    Cookies.StoreInCookie(seedPlantLocation, "", "", fRow.ID.ToString(), DateTime.Now.AddYears(20));
                    Cookies.StoreInCookie(reportPrinter, "", "", Report_Printer, DateTime.Now.AddYears(20));
                    Cookies.StoreInCookie(ticketPrinter, "", "", Ticket_Printer, DateTime.Now.AddYears(20));
                    Cookies.StoreInCookie(plcAddress, "", "", Plc_Address, DateTime.Now.AddYears(20));
                    Cookies.StoreInCookie(locationDescription, "", "", Location_Description, DateTime.Now.AddYears(20));
                }

            }
        }

    }









}