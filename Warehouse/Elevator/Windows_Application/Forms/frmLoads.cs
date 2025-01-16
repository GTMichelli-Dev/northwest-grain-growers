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
    public partial class frmLoads : Form
    {
        bool Startup = true;
        public frmLoads()
        {
            InitializeComponent();
            this.cropsTableAdapter.Fill(this.nWDataset.Crops);
            this.producer_SourceSelectionTableAdapter.Fill(this.nWDataset.Producer_SourceSelection, Settings.Location_Id);
            this.cboCrop.SelectedIndex = -1;
            this.cboProducer.SelectedIndex = -1;
            this.cboType.SelectedIndex = 0;
            this.cboCrop.Text = "";
            this.cboProducer.Text = "";
            this.dtDateEnd.Value = DateTime.Now;
            this.dtDateStart.Value = DateTime.Now;

            this.UpdateData();
            
        }


        private void UpdateData()
        {
            string Weight_Sheet_Type = null;
            string ProducerSource = null;
            string Crop = null;
            DateTime StartDate = dtDateStart.Value;
            DateTime EndDate = dtDateEnd.Value;
            if (!string.IsNullOrEmpty(this.cboCrop.Text)) { Crop = this.cboCrop.Text; }
            if (!string.IsNullOrEmpty(this.cboProducer.Text)) { ProducerSource = this.cboProducer.Text; }
            if (cboType.SelectedIndex > 0) { Weight_Sheet_Type = this.cboType.Text; }
            this.vw_Weight_Sheet_By_TypeTableAdapter.Fill(this.nWDataset.vw_Weight_Sheet_By_Type, Crop, null, StartDate, EndDate, ProducerSource,Weight_Sheet_Type, Settings.Location_Id);

        }
        private void frmLoads_Load(object sender, EventArgs e)
        {
            Startup = false;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();

        }

        private void cbo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!Startup) UpdateData();
        }

        private void dtDateStart_CloseUp(object sender, EventArgs e)
        {
            if (!Startup) UpdateData();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            this.cboCrop.SelectedIndex = -1;
            this.cboProducer.SelectedIndex = -1;
            this.cboType.SelectedIndex = 0;
            this.cboCrop.Text = "";
            this.cboProducer.Text = "";
            this.dtDateEnd.Value = DateTime.Now;
            this.dtDateStart.Value = DateTime.Now;
            this.UpdateData();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {

                int rowIndex = this.dataGridView1.CurrentRow.Index;
                NWDataset.vw_Weight_Sheet_By_TypeRow SelectedRow;
                SelectedRow = (NWDataset.vw_Weight_Sheet_By_TypeRow)(DataRow)((DataRowView)this.vwWeightSheetByTypeBindingSource.Current).Row;

                if (e.ColumnIndex == Weight_Sheet.Index)
                {

                    //Loading.Show("Loading Weight Sheet", Form.ActiveForm);
                    if (SelectedRow.Weight_Sheet_Type.ToUpper() == "INBOUND")
                    {
                        frmHarvest_WS frmHWS = new frmHarvest_WS(SelectedRow.Weight_Sheet_UID);
                        
                        Display.ShowForm(frmHWS);
                        
                    }

                    else
                    {
                        frmTransfer_WS frmTWS = new frmTransfer_WS(SelectedRow.Weight_Sheet_UID);
                        {
                            Display.ShowForm(frmTWS);
                            
                        }

                    }
                  
                }
                else if (e.ColumnIndex== btnReopen.Index )
                {
                    if (Alert.Show(string.Format("Reopen Weight Sheet {0}? ",SelectedRow.WS_Id ), "Reopen Weight Sheet", true) == DialogResult.Yes)
                        {
                        using (NWDatasetTableAdapters.Weight_SheetsTableAdapter weight_SheetsTableAdapter = new NWDatasetTableAdapters.Weight_SheetsTableAdapter())
                        {
                            using (NWDataset.Weight_SheetsDataTable weight_SheetsDataTable = new NWDataset.Weight_SheetsDataTable())
                            {
                               
                                if (weight_SheetsTableAdapter.FillByUID(weight_SheetsDataTable, SelectedRow.Weight_Sheet_UID) > 0)
                                {
                                    int Pos = this.vwWeightSheetByTypeBindingSource.Position;
                                    NWDataset.Weight_SheetsRow row = weight_SheetsDataTable[0];
                                    if (row.Original_Printed == false) row.Closed = false;
                                    weight_SheetsTableAdapter.Update(weight_SheetsDataTable);
                                    UpdateData();
                                    try
                                    {
                                        this.vwWeightSheetByTypeBindingSource.Position = Pos; 
                                    }
                                    catch
                                    {

                                    }
                                }
                            }
                        }
                    }
                }

                else if (e.ColumnIndex== btnReprint.Index )
                {
                    if (SelectedRow.Weight_Sheet_Type.ToUpper() == "INBOUND")
                    {
                    //    Printing.PrintWeightSheet(SelectedRow.Weight_Sheet_UID);
                        System.Diagnostics.Debug.Print("Printing Weight Sheet" + SelectedRow.WS_Id.ToString());
                        Printing.PrintWeightSheet(SelectedRow.Weight_Sheet_UID, true);
                    }
                    else
                    {
                  //      Printing.PrintTransferWeightSheet(SelectedRow.Weight_Sheet_UID);
                        Printing.PrintTransferWeightSheet(SelectedRow.Weight_Sheet_UID, true);
                    }
                }
                                
                



            }
        }

        private void frmLoads_Activated(object sender, EventArgs e)
        {
            this.timer1.Start();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.timer1.Stop();
            this.WindowState = FormWindowState.Maximized;
        }
    }
}
