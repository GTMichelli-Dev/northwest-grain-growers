using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Lots
/// </summary>
public class Lots
{
    public Lots()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static string GetLot(int ItemNumber)
    {
        string Retval = string.Empty;
        using (LotsDataSetTableAdapters.LotsTableAdapter lotsTableAdapter = new LotsDataSetTableAdapters.LotsTableAdapter())
        {
            using (LotsDataSet.LotsDataTable lots = new LotsDataSet.LotsDataTable())
            {
                if (lotsTableAdapter.Fill(lots,GlobalVars.Location ,ItemNumber )>0)
                {
                    Retval = lots[0].Lot; 
                }
            }
        }
        return Retval;
    }
}