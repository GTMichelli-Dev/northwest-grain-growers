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
    public partial class frmLot_Weight_Sheets : Form
    {

        public frmLot_Weight_Sheets(Guid Lot_UID)
        {
            InitializeComponent();
            this.vwWeight_Sheet_InformationTableAdapter.FillByLot_UID(this.nWDataset.vwWeight_Sheet_Information, Lot_UID,Settings.Location_Id );
            this.cboFilter.SelectedIndex = 0;
            this.label2.Text = this.nWDataset.vwWeight_Sheet_Information[0].Lot_Number.ToString();
            UpdateData();

        }



        private void UpdateData()
        {
            if (cboFilter.SelectedIndex == 1)
            { this.vwWeightSheetInformationBindingSource.Filter = "Total_Loads=Completed"; }
            else if (cboFilter.SelectedIndex == 2)
            { this.vwWeightSheetInformationBindingSource.Filter = "Total_Loads<>Completed"; }
            else 
            { this.vwWeightSheetInformationBindingSource.Filter = ""; }

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void frm_Lot_Weight_Sheets_Load(object sender, EventArgs e)
        {





        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void cboFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboFilter.SelectedIndex == -1)
            {
                cboFilter.SelectedIndex = 0;
            }
            UpdateData();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex == this.btnWeightSheet.Index)
            {
                SelectRow();
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            int rowIndex = this.dataGridView1.CurrentRow.Index;
            SelectRow();

          
        }

        private void SelectRow()
        {

            NWDataset.vwWeight_Sheet_InformationRow SelectedRow;
            SelectedRow = (NWDataset.vwWeight_Sheet_InformationRow)(DataRow)((DataRowView)this.vwWeightSheetInformationBindingSource.Current).Row;

            using (frmHarvest_WS frmHWS = new frmHarvest_WS(SelectedRow.UID))
            {
                frmHWS.ShowDialog();
            }
        }
    }
}
