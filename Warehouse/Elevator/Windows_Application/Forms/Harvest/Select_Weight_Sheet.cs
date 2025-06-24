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
    public partial class Select_Weight_Sheet : Form
    {
        public Guid Weight_Sheet_UID;

      
       

        public Select_Weight_Sheet(Guid Current_Weight_Sheet_UID,bool hideTransfer)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Owner = Program.frmMdiMain;
            this.button2.Visible = !hideTransfer;
                this.vw_Open_Weight_SheetsTableAdapter.FillByValid_Weight_Sheet_Moves(this.nWDataset.vw_Open_Weight_Sheets,Settings.Location_Id , Current_Weight_Sheet_UID);
           
            dataGridView1.ClearSelection();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }




        private void button1_Click(object sender, EventArgs e)
        {

            using (Harvest_Lot.frmSelect_Weight_Sheet_Lot frm = new Harvest_Lot.frmSelect_Weight_Sheet_Lot())
            {
                this.Enabled = false;
                DialogResult DR = frm.ShowDialog();
                if (DR == System.Windows.Forms.DialogResult.OK)
                {
                    this.Weight_Sheet_UID = frm.Selected_Weight_Sheet_UID;
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                }
                else
                {
                    this.Enabled = true;
                }

            }
        }




        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
             
             
                {
                    SelectRow();
      
                }
         
            }
        }


        private void SelectRow()
        {
            NWDataset.vw_Open_Weight_SheetsRow SelectedRow;
            SelectedRow = (NWDataset.vw_Open_Weight_SheetsRow)(DataRow)((DataRowView)this.vwOpenWeightSheetsBindingSource.Current).Row;
            this.Weight_Sheet_UID = SelectedRow.Weight_Sheet_UID;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.RowIndex>-1) && (e.ColumnIndex==this.btnSelect.Index))
            {
                SelectRow();


            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (frmSelect_Transfer_Weight_Sheet frm = new frmSelect_Transfer_Weight_Sheet(Weight_Sheet_UID,true))
            {
                this.Hide(); // Hide the current form
                this.DialogResult=frm.ShowDialog(); // Show the new form as a modal dialog
                Weight_Sheet_UID = frm.Weight_Sheet_UID; // Get the value of the Weight_Sheet_UID from the new form
                
                this.Close(); // Close the current form after the new form is closed
            }
        }
    }
}
