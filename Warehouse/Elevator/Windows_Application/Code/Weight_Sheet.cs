using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace NWGrain
{
    public class Weight_Sheet
    {

        public static DialogResult PrintEndOfDay(PrintingTicket PrintingTicket)
        {
            DialogResult DR = PrintOriginalWeightSheets(PrintingTicket);
            if (DR == DialogResult.OK)
            {

                if (Alert.Show("Print End Of Day Reports?", "Print Reports", true) == DialogResult.Yes)
                {
                    Printing.PrintDailyIntakeReport(DateTime.Now, Settings.Location_Id);
                    Printing.PrintDailyTransferReport(DateTime.Now, Settings.Location_Id);
                    Printing.PrintDailyWeightSheetAsc(DateTime.Now, Settings.Location_Id);
                    Printing.PrintDailyLoadsByCrop(DateTime.Now, Settings.Location_Id);
                    Printing.PrintDailyBinReport(DateTime.Now, Settings.Location_Id);
                    Printing.PrintClosedLotReport(DateTime.Now, DateTime.Now, Settings.Location_Id);
                }
                PrintAllTodaysWeightSheets(PrintingTicket);
            }


            return DR;
        }





        public static DialogResult PrintOriginalWeightSheets(PrintingTicket PrintingTicket)
        {
            DialogResult DR = DialogResult.Cancel;
            using (WeightSheetDataSetTableAdapters.AllWeightSheetsTableAdapter allWeightSheetsTableAdapter = new WeightSheetDataSetTableAdapters.AllWeightSheetsTableAdapter())
            {
                using (WeightSheetDataSet.AllWeightSheetsDataTable allWeightSheetsDataTable = new WeightSheetDataSet.AllWeightSheetsDataTable())
                {
                    if (allWeightSheetsTableAdapter.FillByOriginalNotPrinted(allWeightSheetsDataTable, Settings.Location_Id) > 0)
                    {
                        PrintingTicket.SetPrompt("Printing Original Weight Sheets");
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(200);
                        
                        foreach (WeightSheetDataSet.AllWeightSheetsRow row in allWeightSheetsDataTable)
                        {
                            bool Inbound = (row.Weight_Sheet_Type.ToUpper() == "INBOUND");
                            if (Inbound)
                            {
                                DR = Close_Weight_Sheets(enumFilterType.EndOfDay, row.WS_Id, false);
                            }
                            else
                            {
                                DR = Close_Transfer_Weight_Sheets(enumFilterType.EndOfDay, row.WS_Id, false);
                            }
                            if (DR != DialogResult.OK)
                            {
                                    Alert.Show("All Loads Must Be Finished To Close Day ", "Loads Not Closed", false);
                                break;
                            }

                        }

                        if (DR == DialogResult.OK)
                        {

                            Email.EmailWeightSheets(allWeightSheetsDataTable);
                            if (Alert.Show("Did Everything Print Ok", "Printing", true) == DialogResult.Yes)
                            {
                                using (WeightSheetDataSetTableAdapters.QueriesTableAdapter Q = new WeightSheetDataSetTableAdapters.QueriesTableAdapter())
                                {
                                    foreach (WeightSheetDataSet.AllWeightSheetsRow row in allWeightSheetsDataTable)
                                    {

                                        Q.UpdateOriginalPrinted(true, row.Weight_Sheet_UID);
                                       
                                    }
                                }
                                DR = DialogResult.OK;
                            }
                            else
                            {
                                DR = DialogResult.Cancel;
                            }
                        }
                    }
                    else
                    {
                        DR = DialogResult.OK;
                    }

 
                }
            }
            return DR;
        }



        public static int PrintAllTodaysWeightSheets(PrintingTicket PrintingTicket)
        {
            using (WeightSheetDataSetTableAdapters.AllWeightSheetsTableAdapter allWeightSheetsTableAdapter = new WeightSheetDataSetTableAdapters.AllWeightSheetsTableAdapter())
            {
                using (WeightSheetDataSet.AllWeightSheetsDataTable allWeightSheetsDataTable = new WeightSheetDataSet.AllWeightSheetsDataTable())
                {
                    if (allWeightSheetsTableAdapter.FillByCurrentDate(allWeightSheetsDataTable, Settings.Location_Id) > 0)
                    {
                        if (Alert.Show("Print A Copy Of All Weight Sheets Today?", "Print Copy", true) == DialogResult.Yes)
                        {
                            PrintingTicket.SetPrompt("Printing Copy Of All Weight Sheets Today");
                            Application.DoEvents();
                            System.Threading.Thread.Sleep(200);
                            foreach (WeightSheetDataSet.AllWeightSheetsRow row in allWeightSheetsDataTable)
                            {
                                bool Inbound = (row.Weight_Sheet_Type.ToUpper() == "INBOUND");
                                if (Inbound)
                                {
                                    Close_Weight_Sheets(enumFilterType.EndOfDay, row.WS_Id, true);
                                }
                                else
                                {
                                    Close_Transfer_Weight_Sheets(enumFilterType.EndOfDay, row.WS_Id, true);
                                }
                            }
                        }

                    }
                    else
                    {
                        PrintingTicket.SetPrompt("No Weight Sheets Today");
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(200);
                    }
                    return allWeightSheetsDataTable.Count;
                }

            }
        }





        public enum enumFilterType { EndOfDay, WeightSheet, Lot };

        public static DialogResult Close_Weight_Sheets(enumFilterType Filter = enumFilterType.EndOfDay, long ID = -1, bool Unofficial = false, bool reprint = false)
        {
            DialogResult DR = DialogResult.OK;
            using (NWDatasetTableAdapters.vw_Open_Harvest_LoadsTableAdapter vw_Open_Harvest_LoadsTableAdapter = new NWDatasetTableAdapters.vw_Open_Harvest_LoadsTableAdapter())
            {

                using (NWDataset.vw_Open_Harvest_LoadsDataTable vw_Open_Harvest_LoadsDataTable = new NWDataset.vw_Open_Harvest_LoadsDataTable())
                {
                    if (Filter == enumFilterType.Lot)
                    {
                        vw_Open_Harvest_LoadsTableAdapter.FillByLot_Number(vw_Open_Harvest_LoadsDataTable, ID, Settings.Location_Id);

                    }
                    else
                    {
                        vw_Open_Harvest_LoadsTableAdapter.FillByWeight_Sheet_Id(vw_Open_Harvest_LoadsDataTable, Settings.Location_Id, ID);
                    }

                    if (vw_Open_Harvest_LoadsDataTable.Count > 0)
                    {

                        string Load = string.Format("There are Intake Loads Not Finished. View Load?");
                        if (Alert.Show(Load, "Open Loads", true) == System.Windows.Forms.DialogResult.Yes)
                        {
                            if (vw_Open_Harvest_LoadsDataTable.Count > 1)
                            {

                                using (frmOpen_Harvest_Loads frm = new frmOpen_Harvest_Loads(Filter, ID))
                                {
                                    DR = frm.ShowDialog();
                                }
                            }
                            else
                            {

                                Loading.Show("Loading Ticket", Form.ActiveForm);
                                using (frmHarvest_Load frm = new frmHarvest_Load(vw_Open_Harvest_LoadsDataTable[0].Load_UID, false))
                                {

                                    frm.ShowDialog();
                                    vw_Open_Harvest_LoadsTableAdapter.Fill(vw_Open_Harvest_LoadsDataTable, Settings.Location_Id);
                                    if (vw_Open_Harvest_LoadsDataTable.Count > 0)
                                    {
                                        DR = DialogResult.Cancel;
                                    }

                                }
                                Loading.Close();
                            }
                        }
                        else
                        {
                            DR = DialogResult.Cancel;
                        }
                    }
                    if (DR == DialogResult.OK)
                    {

                        using (NWDatasetTableAdapters.Weight_SheetsTableAdapter Weight_SheetsTableAdapter = new NWDatasetTableAdapters.Weight_SheetsTableAdapter())
                        {
                            using (NWDataset.Weight_SheetsDataTable Weight_SheetsTable = new NWDataset.Weight_SheetsDataTable())
                            {

                                if (Filter == enumFilterType.Lot)
                                {
                                    Weight_SheetsTableAdapter.FillByLot_Number(Weight_SheetsTable, ID, Settings.Location_Id);
                                }
                                else
                                {
                                    Weight_SheetsTableAdapter.FillByWeight_Sheet_Id(Weight_SheetsTable, ID, Settings.Location_Id);
                                }

                                if (Weight_SheetsTable.Count > 0)
                                {
                                    foreach (NWDataset.Weight_SheetsRow row in Weight_SheetsTable)
                                    {
                                        row.Closed = true;
                                        Weight_SheetsTableAdapter.Update(Weight_SheetsTable);
                                        if (Unofficial && (Filter == enumFilterType.WeightSheet || (Filter == enumFilterType.Lot )))
                                        {
                                            if (Alert.Show("Official Copy Prints At End Of Day" + System.Environment.NewLine + "Print Copy For Grower?", "Printing", true) == DialogResult.Yes)
                                            {
                                                System.Diagnostics.Debug.Print("Printing Weight Sheet" + row.WS_Id.ToString());
                                                Printing.PrintWeightSheet(row.UID, Unofficial);
                                            }
                                        }
                                        else
                                        {
                                            System.Diagnostics.Debug.Print("Printing Weight Sheet" + row.WS_Id.ToString());
                                            Printing.PrintWeightSheet(row.UID, Unofficial);
                                        }
                                    }
                                    foreach (NWDataset.Weight_SheetsRow row in Weight_SheetsTable)
                                    {
                                        row.Closed = true;
                                    }
                                    Weight_SheetsTableAdapter.Update(Weight_SheetsTable);
                                }
                            }
                        }
                    }
                }
            }
            return DR;

        }


        public static DialogResult Close_Transfer_Weight_Sheets(enumFilterType Filter = enumFilterType.EndOfDay, long ID = -1, bool Unofficial = false, bool reprint = false)
        {
            DialogResult DR = DialogResult.OK;
            using (NWDatasetTableAdapters.vw_Open_Transfer_LoadsTableAdapter vw_Open_Transfer_LoadsTableAdapter = new NWDatasetTableAdapters.vw_Open_Transfer_LoadsTableAdapter())
            {

                using (NWDataset.vw_Open_Transfer_LoadsDataTable vw_Open_Transfer_LoadsDataTable = new NWDataset.vw_Open_Transfer_LoadsDataTable())
                {
                    if (Filter == enumFilterType.WeightSheet)
                    {
                        vw_Open_Transfer_LoadsTableAdapter.FillByWeight_Sheet_ID(vw_Open_Transfer_LoadsDataTable, ID, Settings.Location_Id);
                    }
                    else
                    {
                        vw_Open_Transfer_LoadsTableAdapter.Fill(vw_Open_Transfer_LoadsDataTable, Settings.Location_Id);
                    }

                    if (vw_Open_Transfer_LoadsDataTable.Count > 0)
                    {
                        string Load = string.Format("There are Transfer Loads Not Finished. View Loads?");
                        if (Alert.Show(Load, "Open Loads", true) == System.Windows.Forms.DialogResult.Yes)
                        {
                            if (vw_Open_Transfer_LoadsDataTable.Count > 1)
                            {
                                using (frmOpen_Transfer_Loads frm = new frmOpen_Transfer_Loads(Filter, ID))
                                {

                                    DR = frm.ShowDialog();
                                }
                            }
                            else
                            {
                                Loading.Show("Loading Ticket", Form.ActiveForm);
                                using (frmTransfer_Load frm = new frmTransfer_Load(vw_Open_Transfer_LoadsDataTable[0].Weight_Sheet_UID, vw_Open_Transfer_LoadsDataTable[0].Load_UID, false))
                                {

                                    frm.ShowDialog();
                                    vw_Open_Transfer_LoadsTableAdapter.Fill(vw_Open_Transfer_LoadsDataTable, Settings.Location_Id);
                                    if (vw_Open_Transfer_LoadsDataTable.Count > 0)
                                    {
                                        DR = DialogResult.Cancel;
                                    }

                                }
                                Loading.Close();
                            }
                        }
                        else
                        {
                            DR = DialogResult.Cancel;
                        }
                    }
                    if (DR == DialogResult.OK)
                    {
                        using (NWDatasetTableAdapters.Weight_SheetsTableAdapter Weight_SheetsTableAdapter = new NWDatasetTableAdapters.Weight_SheetsTableAdapter())
                        {
                            using (NWDataset.Weight_SheetsDataTable Weight_Sheets = new NWDataset.Weight_SheetsDataTable())
                            {
                                Weight_SheetsTableAdapter.FillByWeight_Sheet_Id(Weight_Sheets, ID, Settings.Location_Id);
                                if (Weight_Sheets.Count > 0)
                                {
                                    foreach (NWDataset.Weight_SheetsRow row in Weight_Sheets)
                                    {
                                        if (Unofficial && Filter == enumFilterType.WeightSheet)
                                        {
                                            if (Alert.Show("Official Copy Prints At End Of Day" + System.Environment.NewLine + "Print Copy?", "Printing", true) == DialogResult.Yes)
                                            {
                                                System.Diagnostics.Debug.Print("Printing Transfer Weight Sheet" + row.WS_Id.ToString());
                                                Printing.PrintTransferWeightSheet(row.UID, Unofficial);
                                            }
                                        }
                                        else
                                        {
                                            System.Diagnostics.Debug.Print("Printing  Transfer Weight Sheet" + row.WS_Id.ToString());
                                            Printing.PrintTransferWeightSheet(row.UID, Unofficial);
                                        }

                                    }
                                    foreach (NWDataset.Weight_SheetsRow row in Weight_Sheets)
                                    {
                                        row.Closed = true;
                                    }
                                    Weight_SheetsTableAdapter.Update(Weight_Sheets);
                                }
                            }
                        }
                    }
                }
            }
            return DR;

        }




    }



}
