using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NWGrain

{
    class Lot
    {
        public NWDataset.LotsRow LotsRow = null;
        private NWDataset NWDataset = new NWDataset();
        private NWDatasetTableAdapters.LotsTableAdapter LotsTableAdapter = new NWDatasetTableAdapters.LotsTableAdapter();
        

        public Lot()
        {
            LotsRow = NWDataset.Lots.NewLotsRow();
        }


        public static bool LotHasClosedWeightSheets(long Lot_Number)
        {
            using (LotsDataSet.LotsWithClosedWeightSheetsDataTable lotsWithClosedWeightSheetsDataTable = new NWGrain.LotsDataSet.LotsWithClosedWeightSheetsDataTable())
            {
                using (LotsDataSetTableAdapters.LotsWithClosedWeightSheetsTableAdapter lotsWithClosedWeightSheetsTableAdapter = new LotsDataSetTableAdapters.LotsWithClosedWeightSheetsTableAdapter())
                {
                    return (lotsWithClosedWeightSheetsTableAdapter.Fill(lotsWithClosedWeightSheetsDataTable, Settings.Location_Id, Lot_Number) > 0);
                }
            }
        }


        public Lot(Guid LotUID)
        {
            try
            {
                if (LotsTableAdapter.FillByUID(NWDataset.Lots, LotUID) > 0)
                {
                    LotsRow = NWDataset.Lots[0];
                }
                else
                {
                    System_Log.Log_Message("NWGrain.Code.Lot(" + LotUID.ToString() + ")", "Could Not Find Lot");
                }
            }
            catch( Exception ex)
            {
                System_Log.Log_Message("NWGrain.Code.Lot(" + LotUID.ToString() + ")", ex.Message);
            }
        }
    }
}
