using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NWGrain
{
    public partial class frmTransfer_WS : Form
    {



        public bool Startup = true;
        public Guid WS_UID;
        bool Wait = true;

        public frmTransfer_WS(Guid Weight_Sheet_UID)
        {
            InitializeComponent();
            WS_UID = Weight_Sheet_UID;
            UpdateData();
            Wait = false;
        }


        private void UpdateData()
        {

            this.vwTransfer_Weight_Sheet_InformationTableAdapter.FillByWeight_SheetUID(this.nWDataset.vwTransfer_Weight_Sheet_Information, WS_UID);
            this.vwTransferWeight_Sheet_InformationBindingSource.MoveFirst();
            this.single_Transfer_Weight_SheetTableAdapter.Fill(this.nWDataset.Single_Transfer_Weight_Sheet, WS_UID);
            using (NWDatasetTableAdapters.QueriesTableAdapter Q = new NWDatasetTableAdapters.QueriesTableAdapter())
            {
                this.ck_LoadOut.Checked = this.nWDataset.vwTransfer_Weight_Sheet_Information[0].Is_Loadout;
                this.ck_LoadOut.Enabled = this.nWDataset.Single_Transfer_Weight_Sheet.Count == 0;
                this.pnlHauler.Visible = !this.ck_LoadOut.Checked;
                int TotalNet;
                TotalNet = (int)Q.Transfer_Weight_Sheet_Total_Net(WS_UID);
                this.lblTotalNet.Text = string.Format("{0:N0} Lbs.", TotalNet);
                this.lblBushels.Text = string.Format("{0:N0} bu.", TotalNet / 60);

                decimal? TotalBilled;
                TotalBilled = (decimal?)Q.Total_Billed(WS_UID);
                if (TotalBilled != null)
                {
                    this.txtTotalBilled.Text = string.Format("{0:C2}", TotalBilled);
                }

                NWDataset.vwTransfer_Weight_Sheet_InformationRow SelectedRow;
                SelectedRow = (NWDataset.vwTransfer_Weight_Sheet_InformationRow)(DataRow)((DataRowView)this.vwTransferWeight_Sheet_InformationBindingSource.Current).Row;
                lblWeightSheet.Text = SelectedRow.WS_Id.ToString();
                if (SelectedRow.Closed)
                {

                    this.btnClosePrint.Text = "Print Weight Sheet";
                }
                else
                {
                    this.btnClosePrint.Text = "Close Weight Sheet";
                }
                this.mnuMove.Visible =(!SelectedRow.Closed);
                this.btnFixHauler.Visible = !SelectedRow.Closed;
                this.btnFixLot.Visible= !SelectedRow.Closed;


                var Custom = (!SelectedRow.IsBOL_TypeNull() && SelectedRow.BOL_Type.ToUpper().Trim() == "C");

                lblCustom.Visible = Custom;
                

            }

        }



        private void button1_Click(object sender, EventArgs e)
        {
            NWDataset.vwTransfer_Weight_Sheet_InformationRow SelectedRow;
            SelectedRow = (NWDataset.vwTransfer_Weight_Sheet_InformationRow)(DataRow)((DataRowView)this.vwTransferWeight_Sheet_InformationBindingSource.Current).Row;

            int? CarrierId = null;

            if (!SelectedRow.IsCarrier_IdNull())
            {
                CarrierId = SelectedRow.Carrier_Id;
            }

            using (frmEdit_Transfer_Carrier frm = new frmEdit_Transfer_Carrier(this.WS_UID, CarrierId, SelectedRow.Source_Id, SelectedRow.Location_Id))
            {
                frm.ShowDialog();
                UpdateData();
            }
        }


        private void frmHarvest_WS_FormClosing(object sender, FormClosingEventArgs e)
        {
            using (NWDatasetTableAdapters.QueriesTableAdapter Q = new NWDatasetTableAdapters.QueriesTableAdapter())
            {
                Q.Update_WS_Comment(WS_UID, this.commentRichTextBox.Text);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == this.btnEdit.Index)
            {
                Edit_Row();

            }
        }

        private void Edit_Row()
        {
            int rowIndex = this.dataGridView1.CurrentRow.Index;

            {
                Loading.Show("Loading Transfer Ticket", Form.ActiveForm);

                NWDataset.Single_Transfer_Weight_SheetRow SelectedRow;
                SelectedRow = (NWDataset.Single_Transfer_Weight_SheetRow)(DataRow)((DataRowView)this.SingleTransferWeightSheetBindingSource.Current).Row;

                using (frmTransfer_Load frm = new frmTransfer_Load(SelectedRow.Weight_Sheet_UID, SelectedRow.Load_UID,SelectedRow.Closed ))
                {
                    frm.ShowDialog();
                    this.WS_UID = frm.vwTransfer_Weight_SheetRow.Weight_Sheet_UID;
                    if (this.WS_UID == Guid.Empty)
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                        return;
                    }
                    else
                    {
                        UpdateData();
                    }
                    
                }

            }
            Loading.Close();

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex != this.bolTextBox.Index && e.ColumnIndex != binTextBox.Index && e.ColumnIndex != this.ProteinTextBox.Index)
                {
                    int rowIndex = this.dataGridView1.CurrentRow.Index;

                    {
                        Edit_Row();

                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NWDataset.vwTransfer_Weight_Sheet_InformationRow SelectedRow;
            SelectedRow = (NWDataset.vwTransfer_Weight_Sheet_InformationRow)(DataRow)((DataRowView)this.vwTransferWeight_Sheet_InformationBindingSource.Current).Row;

            using (frmTransfer_Weight_Sheet_Details frm = new frmTransfer_Weight_Sheet_Details(SelectedRow.Weight_Sheet_UID))
            {
                frm.ShowDialog();
                this.vwTransfer_Weight_Sheet_InformationTableAdapter.FillByWeight_SheetUID(this.nWDataset.vwTransfer_Weight_Sheet_Information, WS_UID);
                this.vwTransferWeight_Sheet_InformationBindingSource.MoveFirst();
                using (NWDatasetTableAdapters.QueriesTableAdapter Q = new NWDatasetTableAdapters.QueriesTableAdapter())
                {
                    SelectedRow = (NWDataset.vwTransfer_Weight_Sheet_InformationRow)(DataRow)((DataRowView)this.vwTransferWeight_Sheet_InformationBindingSource.Current).Row;
                    int? CarrierId = null;
                    if (!SelectedRow.IsCarrier_IdNull()) { CarrierId = SelectedRow.Carrier_Id; }
                    Q.Update_Transfer_WS_Carrier(SelectedRow.Weight_Sheet_UID, CarrierId, SelectedRow.Location_Id, SelectedRow.Source_Id);
                }
                UpdateData();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            NWDataset.vwTransfer_Weight_Sheet_InformationRow SelectedRow;
            SelectedRow = (NWDataset.vwTransfer_Weight_Sheet_InformationRow)(DataRow)((DataRowView)this.vwTransferWeight_Sheet_InformationBindingSource.Current).Row;
            if (!SelectedRow.Closed)
            {
                if (Alert.Show("Close Weight Sheet?", "Confirm Close", true) == System.Windows.Forms.DialogResult.Yes)
                {

                    if (Weight_Sheet.Close_Transfer_Weight_Sheets(Weight_Sheet.enumFilterType.WeightSheet, SelectedRow.WS_Id,true) == System.Windows.Forms.DialogResult.OK)
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        UpdateData();
                    }


                }
            }
            else
            {
                Printing.PrintTransferWeightSheet(SelectedRow.Weight_Sheet_UID, false);
            }
        }



        private void frmHarvest_WS_Activated(object sender, EventArgs e)
        {
            Loading.Close();
        }

        private void dataGridView1_Validating(object sender, CancelEventArgs e)
        {

        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                using (NWDatasetTableAdapters.QueriesTableAdapter Q = new NWDatasetTableAdapters.QueriesTableAdapter())
                {
                    NWDataset.Single_Transfer_Weight_SheetRow SelectedRow;
                    SelectedRow = (NWDataset.Single_Transfer_Weight_SheetRow)(DataRow)((DataRowView)this.SingleTransferWeightSheetBindingSource.Current).Row;

                    if (e.ColumnIndex == this.bolTextBox.Index)
                    {
                        int BOL;
                        if (!string.IsNullOrEmpty(this.dataGridView1.CurrentCell.Value.ToString()))
                        {
                            if (!int.TryParse(this.dataGridView1.CurrentCell.Value.ToString(), out BOL))
                            {
                                Alert.Show("BOL Must Be A Whole Number", "ERROR", false);
                                this.dataGridView1.CancelEdit();
                            }
                            else
                            {
                                Q.Update_Weight_Sheet_Bol(SelectedRow.Load_UID, BOL.ToString());
                            }

                        }
                        else
                        {
                            Q.Update_Weight_Sheet_Bol(SelectedRow.Load_UID, null);
                        }
                    }
                    if (e.ColumnIndex == this.binTextBox.Index)
                    {

                        if (!string.IsNullOrEmpty(this.dataGridView1.CurrentCell.Value.ToString()))
                        {

                            Q.Update_Weight_Sheet_Bin(SelectedRow.Load_UID, this.dataGridView1.CurrentCell.Value.ToString());


                        }

                        else
                        {
                            Q.Update_Weight_Sheet_Bin(SelectedRow.Load_UID, null);
                        }
                    }
                    else if (e.ColumnIndex == this.ProteinTextBox.Index)
                    {
                        decimal Protein;
                        if (!string.IsNullOrEmpty(this.dataGridView1.CurrentCell.Value.ToString()))
                        {
                            if (!decimal.TryParse(this.dataGridView1.CurrentCell.Value.ToString(), out Protein))
                            {
                                Alert.Show("Protein Must Be A Number", "ERROR", false);
                                this.dataGridView1.CancelEdit();
                            }
                            else
                            {
                                if (Protein < 0 || Protein > 50)
                                {
                                    Alert.Show("Protein Must Be Between 0 and 50", "ERROR", false);
                                    this.dataGridView1.CancelEdit();

                                }
                                else
                                {
                                    Protein = Math.Round(Protein, 2);
                                    this.dataGridView1.CurrentCell.Value = Protein;
                                    Q.Update_Transfer_Protein(SelectedRow.Load_UID, (float)Protein);
                                }

                            }
                        }
                        else
                        {
                            Q.Update_Transfer_Protein(SelectedRow.Load_UID, null);
                        }
                    }
                }
            }
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {

        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (this.dataGridView1.CurrentCell.ColumnIndex == this.ProteinTextBox.Index)
            {
                NWDataset.Single_Transfer_Weight_SheetRow SelectedRow;
                SelectedRow = (NWDataset.Single_Transfer_Weight_SheetRow)(DataRow)((DataRowView)this.SingleTransferWeightSheetBindingSource.Current).Row;
                if (!SelectedRow.IsProteinNull())
                {
                    Alert.Show("Invalid Value For Protein", "Error");
                }
            }
            this.dataGridView1.CancelEdit();

        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (this.dataGridView1.CurrentCell.ColumnIndex == this.ProteinTextBox.Index)
            {
                if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
                {
                    NWDataset.Single_Transfer_Weight_SheetRow SelectedRow;
                    SelectedRow = (NWDataset.Single_Transfer_Weight_SheetRow)(DataRow)((DataRowView)this.SingleTransferWeightSheetBindingSource.Current).Row;
                    SelectedRow.SetProteinNull();
                }

            }
        }

        private void frmTransfer_WS_Load(object sender, EventArgs e)
        {

        }

        private void frmTransfer_WS_Activated(object sender, EventArgs e)
        {
            if (Startup)
            {
                Loading.Close();
                Startup = false;
            }

            Program.FrmMain.Hide();
        }

        private void ck_LoadOut_CheckedChanged(object sender, EventArgs e)
        {
            if (!Wait)
            {
                using (NWDatasetTableAdapters.QueriesTableAdapter Q = new NWDatasetTableAdapters.QueriesTableAdapter())
                {
                    Q.Change_Is_Loadout(this.nWDataset.vwTransfer_Weight_Sheet_Information[0].Weight_Sheet_UID, this.ck_LoadOut.Checked);
                }
                UpdateData();
            }
        }

        private void commentRichTextBox_Validated(object sender, EventArgs e)
        {
            using (NWDatasetTableAdapters.QueriesTableAdapter Q = new NWDatasetTableAdapters.QueriesTableAdapter())
            {
                Q.Update_WS_Comment(WS_UID, this.commentRichTextBox.Text);

            }
        }

        private void frmTransfer_WS_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.FrmMain.Show();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == binTextBox.Index && e.RowIndex > -1)
            {
                NWDataset.Single_Transfer_Weight_SheetRow SelectedRow;
                SelectedRow = (NWDataset.Single_Transfer_Weight_SheetRow)(DataRow)((DataRowView)this.SingleTransferWeightSheetBindingSource.Current).Row;
                string Bin = string.Empty;
                if (!SelectedRow.IsBinNull()) Bin = SelectedRow.Bin;
                using (SelectBin frm = new NWGrain.SelectBin(Bin, SelectedRow.Load_Id.ToString()))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        if (!string.IsNullOrEmpty(frm.SelectedBin)) SelectedRow.Bin = frm.SelectedBin;
                        using (NWDatasetTableAdapters.QueriesTableAdapter Q = new NWDatasetTableAdapters.QueriesTableAdapter())
                        {
                            Q.Update_Weight_Sheet_Bin(SelectedRow.Load_UID, frm.SelectedBin);

                        }



                    }
                }
            }
        }

        private void mnuMove_Click(object sender, EventArgs e)
        {
            if (Alert.Show("Change Weight Sheet To Inbound Weight Sheet?", "Confirm Transfer", true) == DialogResult.Yes)
            {
                using (Harvest_Lot.frmSelect_Weight_Sheet_Lot frm = new Harvest_Lot.frmSelect_Weight_Sheet_Lot())
                {


                    Loading.Show("Getting Lots..", Form.ActiveForm);
                    DialogResult DR = frm.ShowDialog();
                    if (DR == System.Windows.Forms.DialogResult.OK)
                    {
                        Loading.Show("Creating Inbound Load Ticket", Form.ActiveForm);

                        Guid IntakeWSID = frm.Selected_Weight_Sheet_UID;

                        using (NWDatasetTableAdapters.QueriesTableAdapter Q = new NWDatasetTableAdapters.QueriesTableAdapter())
                        {
                            foreach (NWDataset.Single_Transfer_Weight_SheetRow Trow in nWDataset.Single_Transfer_Weight_Sheet)
                            {
                                float? Protien = null;
                                if (!Trow.IsProteinNull()) Protien = Trow.Protein;
                                Guid Load_UID = (Guid)Q.Create_New_Inbound_Harvest_Load(Trow.Location_Id,
                                    IntakeWSID,
                                    Trow.Time_In,
                                    Trow.Weight_In,
                                    Trow.Manual_Weight_In,
                                    (Trow.IsBolNull()) ? null : Trow.Bol,
                                    (Trow.IsBinNull())?null:Trow.Bin ,
                                    Trow.Truck_Id,
                                    Protien,
                                    "Moved From Transfer WeightSheet:" + this.lblWeightSheet.Text, null, null, null, Trow.Weighmaster);
                                if (!Trow.IsTime_OutNull())
                                {
                                    using (NWDatasetTableAdapters.LoadsTableAdapter loadsTableAdapter = new NWDatasetTableAdapters.LoadsTableAdapter())
                                    {
                                        using (NWDataset.LoadsDataTable loadsDatatable = new NWGrain.NWDataset.LoadsDataTable())
                                        {
                                            if (loadsTableAdapter.FillByLoad_UID(loadsDatatable, Load_UID) > 0)
                                            {
                                                loadsDatatable[0].Time_Out = Trow.Time_Out;
                                                loadsDatatable[0].Weight_Out = Trow.Weight_Out;
                                                loadsTableAdapter.Update(loadsDatatable);
                                            }
                                        }
                                    }

                                }
                                Q.Delete_Transfer_Load(Trow.Load_UID);


                            }

                            NWDataset.vwTransfer_Weight_Sheet_InformationRow SelectedRow;
                            SelectedRow = (NWDataset.vwTransfer_Weight_Sheet_InformationRow)(DataRow)((DataRowView)this.vwTransferWeight_Sheet_InformationBindingSource.Current).Row;
                            if (!SelectedRow.Closed)
                            {
                                Weight_Sheet.Close_Transfer_Weight_Sheets(Weight_Sheet.enumFilterType.WeightSheet, SelectedRow.WS_Id, false);
                                {
                                    this.DialogResult = DialogResult.OK;
                                    this.Close();
                                }
                            }
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }

                    }
                }
            }
            Loading.Close();
        }

        private void ckIndirt_Click(object sender, EventArgs e)
        {
            using (WeightSheetDataSetTableAdapters.QueriesTableAdapter Q = new WeightSheetDataSetTableAdapters.QueriesTableAdapter())
            {
                Q.UpdateWeightSheetInDirt(ckIndirt.Checked, WS_UID);
            }

        }
    }
}
