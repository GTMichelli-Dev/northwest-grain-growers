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
    public partial class TareLookup : Form
    {
        public TareLookup()
        {
            InitializeComponent();
            this.truckTareListTableAdapter.Fill(this.listsDataSet.TruckTareList, Settings.Location_Id, 1);
        }


        public int Tare { get; set; }
        public string VehicleID { get; set; }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex == this.btnSelect.Index )
            {
                
                var SelectedRow = (NWGrain.ListsDataSet.TruckTareListRow )(DataRow)((DataRowView)this.truckTareListBindingSource.Current).Row;
                Tare = SelectedRow.Tare;
                VehicleID = SelectedRow.Truck_Id;
                this.DialogResult = DialogResult.OK;
                this.Close();

            }
        }
    }
}
