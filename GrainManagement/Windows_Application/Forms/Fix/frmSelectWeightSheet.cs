using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NWGrain.Code;
using NWGrain;

namespace NWGrain.Forms.Fix
{
    public partial class frmSelectWeightSheet : Form
    {
        public bool Transfer;
        public ChangeWSType changeWSType;

        public frmSelectWeightSheet(bool transfer)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Owner = Program.frmMdiMain;
            Transfer = transfer;
           
            this.weightSheetsForSelectionTableAdapter.Fill(this.nWDataset.WeightSheetsForSelection, Settings.Location_Id);
            if (Transfer)
            {
                this.Text = "Move Transfer To Intake Weightsheet";
                this.weightSheetsForSelectionBindingSource.Filter = "Weight_Sheet_Type='Transfer'";
                label1.Text = "Move Transfer Weightsheet >>> Intake Weightsheet" + System.Environment.NewLine + "Select Weightsheet to move";
            }
            else
            {
                this.Text = "Move Intake To Transfer";
                this.weightSheetsForSelectionBindingSource.Filter = "Weight_Sheet_Type='Inbound'";
                label1.Text = "Move Intake Weightsheet >>> Transfer Weightsheet" + System.Environment.NewLine + "Select Weightsheet to move";
            }
        }


     

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            NWDataset.WeightSheetsForSelectionRow SelectedRow;
            SelectedRow = (NWDataset.WeightSheetsForSelectionRow)(DataRow)((DataRowView)this.weightSheetsForSelectionBindingSource.Current).Row;


            ChangeWSType changeWSType = new Code.ChangeWSType(SelectedRow, Transfer);
            DialogResult dr;
            if (changeWSType.Transfer)
            {
                using (frmTransfer_Weight_Sheet_Details frm = new frmTransfer_Weight_Sheet_Details( ))
                {
                    dr = frm.ShowDialog();
                }
            }
            else
            {

                using (NWGrain.Harvest_Lot.frmSelect_Weight_Sheet_Lot frm = new NWGrain.Harvest_Lot.frmSelect_Weight_Sheet_Lot())
                {
                    dr = frm.ShowDialog();
                }

            }
            if (dr == DialogResult.OK)
            {
                string Prompt = string.Format("Move Intake Weightsheet {0} to Transfer ?", changeWSType.weightSheetRow.WS_Id);
                if (changeWSType.Transfer)
                {
                    Prompt = string.Format("Move Transfer Weightsheet {0} to Intake ?", changeWSType.weightSheetRow.WS_Id);
                }

                if (Alert.Show(Prompt, "Confirm Move", true) == DialogResult.Yes)
                {

                }
            }


            this.Close();

        }

        private void frmSelectWeightSheet_Resize(object sender, EventArgs e)
        {
            //this.WindowState = FormWindowState.Maximized;
        }

      
    }
}
