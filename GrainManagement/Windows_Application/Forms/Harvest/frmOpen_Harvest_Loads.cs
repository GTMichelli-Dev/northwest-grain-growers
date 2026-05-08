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
    public partial class frmOpen_Harvest_Loads : Form
    {

        Weight_Sheet.enumFilterType Filter = Weight_Sheet.enumFilterType.EndOfDay;
        long ID = -1;

        public frmOpen_Harvest_Loads(Weight_Sheet.enumFilterType Weight_Sheet_Filter_Type = Weight_Sheet.enumFilterType.EndOfDay, long Index = -1)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Owner = Program.frmMdiMain;
            Filter = Weight_Sheet_Filter_Type;
            ID = Index;
            UpdateData();
        }

        private void UpdateData()
        {
            if (Filter == Weight_Sheet.enumFilterType.WeightSheet)
            {
                this.label1.Text = string.Format("Open Weight Sheets For Weight Sheet {0}", ID);
                this.vw_Open_Harvest_LoadsTableAdapter.FillByWeight_Sheet_Id(this.nWDataset.vw_Open_Harvest_Loads,Settings.Location_Id,ID);
            }
            else if (Filter == Weight_Sheet.enumFilterType.Lot)
            {
                this.label1.Text = string.Format("Open Weight Sheets For Lot {0}", ID);
                this.vw_Open_Harvest_LoadsTableAdapter.FillByLot_Number(this.nWDataset.vw_Open_Harvest_Loads, ID, Settings.Location_Id);
            }
            else
            {
                this.label1.Text = "Open Weight Sheets ";
                this.vw_Open_Harvest_LoadsTableAdapter.Fill(this.nWDataset.vw_Open_Harvest_Loads, Settings.Location_Id);
            }
            if (this.nWDataset.vw_Open_Harvest_Loads.Count == 0)
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {

                int rowIndex = this.dataGridView1.CurrentRow.Index;

                {
                    NWDataset.vw_Open_Harvest_LoadsRow SelectedRow;
                    SelectedRow = (NWDataset.vw_Open_Harvest_LoadsRow)(DataRow)((DataRowView)this.vwOpenHarvestLoadsBindingSource.Current).Row;

                    using (frmHarvest_Load frm = new frmHarvest_Load(SelectedRow.Load_UID ,false ))
                    {
                        frm.ShowDialog();
                        UpdateData();
                    }

                }
            }
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex== btnEdit.Index )
            {

                int rowIndex = this.dataGridView1.CurrentRow.Index;

                {
                    NWDataset.vw_Open_Harvest_LoadsRow SelectedRow;
                    SelectedRow = (NWDataset.vw_Open_Harvest_LoadsRow)(DataRow)((DataRowView)this.vwOpenHarvestLoadsBindingSource.Current).Row;

                    using (frmHarvest_Load frm = new frmHarvest_Load(SelectedRow.Load_UID,false ))
                    {
                        frm.ShowDialog();
                        UpdateData();
                    }

                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.vwOpenHarvestLoadsBindingSource.Count == 0)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel ;
            }
            this.Close();

        }
    }
}
