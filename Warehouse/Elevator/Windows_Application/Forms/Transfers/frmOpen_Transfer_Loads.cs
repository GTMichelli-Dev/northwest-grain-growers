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
    public partial class frmOpen_Transfer_Loads : Form
    {
        Weight_Sheet.enumFilterType Filter = Weight_Sheet.enumFilterType.EndOfDay;
        long ID = -1;


        public frmOpen_Transfer_Loads(Weight_Sheet.enumFilterType Weight_Sheet_Filter_Type = Weight_Sheet.enumFilterType.EndOfDay, long Index = -1)
        {
            InitializeComponent();
            Filter = Weight_Sheet_Filter_Type;
            ID = Index;
            UpdateData();
        }


        private void UpdateData()
        {
            if (Filter == Weight_Sheet.enumFilterType.WeightSheet)
            {
                this.label1.Text = string.Format("Open Weight Sheets For Weight Sheet {0}", ID);
                this.vw_Open_Transfer_LoadsTableAdapter.FillByWeight_Sheet_ID(this.nWDataset.vw_Open_Transfer_Loads, ID, Settings.Location_Id);
            }
            else
            {
                this.label1.Text = "Open Weight Sheets ";
                this.vw_Open_Transfer_LoadsTableAdapter.Fill(this.nWDataset.vw_Open_Transfer_Loads, Settings.Location_Id);
            }
            if (this.nWDataset.vw_Open_Transfer_Loads.Count == 0)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmHarvest_Loads_Open_Load(object sender, EventArgs e)
        {


        }

    
        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {

                int rowIndex = this.dataGridView1.CurrentRow.Index;

                {
                    NWDataset.vw_Open_Transfer_LoadsRow SelectedRow;
                    SelectedRow = (NWDataset.vw_Open_Transfer_LoadsRow)(DataRow)((DataRowView)this.vwOpenTransferLoadsBindingSource.Current).Row;

                    using (frmTransfer_Load frm = new frmTransfer_Load(SelectedRow.Load_UID))
                    {
                        frm.ShowDialog();
                        UpdateData();
                    }

                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {

                NWDataset.vw_Open_Transfer_LoadsRow SelectedRow;
                SelectedRow = (NWDataset.vw_Open_Transfer_LoadsRow)(DataRow)((DataRowView)this.vwOpenTransferLoadsBindingSource.Current).Row;
                if (e.ColumnIndex == btnEdit.Index)
                {

                    int rowIndex = this.dataGridView1.CurrentRow.Index;

                    {


                        using (frmTransfer_Load frm = new frmTransfer_Load(SelectedRow.Weight_Sheet_UID, SelectedRow.Load_UID, false))
                        {
                            frm.ShowDialog();
                            UpdateData();
                        }

                    }
                }
                //else if (e.ColumnIndex == btnWeightSheet.Index)
                //{
                //    using (frmTransfer_WS frm = new NWGrain.frmTransfer_WS(SelectedRow.Weight_Sheet_UID))
                //    {
                //        frmTransfer_WS frmTWS = new frmTransfer_WS(SelectedRow.Weight_Sheet_UID);
                //        Display.ShowForm(frmTWS);
                //        this.Close();

                //    }
                //}

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.vwOpenTransferLoadsBindingSource.Count == 0)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
            this.Close();

        }
      
    }
}
