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
    public partial class ClosedLotsReport : Form
    {
        public ClosedLotsReport()
        {
            InitializeComponent();

            this.dtDateStart.Value = DateTime.Now;
            this.dtDateEnd.Value = DateTime.Now;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Printing.PrintClosedLotReport(dtDateStart.Value, dtDateEnd.Value, Settings.Location_Id);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
