using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Weight_Sheet_Transfer_Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = DateTime.Now.ToString("HHmm");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSet1.Weight_Sheets' table. You can move, or remove it, as needed.
           

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           NW_Data_MasterDataSet.vwWeight_Sheet_InformationRow SelectedRow;
            SelectedRow = (NW_Data_MasterDataSet.vwWeight_Sheet_InformationRow)(DataRow)((DataRowView)this.weightSheetsBindingSource.Current).Row;
            Weight_Sheet_Transfer.TransferDataset Ds = new Weight_Sheet_Transfer.TransferDataset();
            Weight_Sheet_Transfer.TransferDataset.TransferValsRow row = Ds.TransferVals.NewTransferValsRow();
          //  row.Crop_ID=SelectedRow.
              
        }
    }
}
