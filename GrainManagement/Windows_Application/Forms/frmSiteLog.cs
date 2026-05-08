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
    public partial class frmSiteLog : Form
    {
        public frmSiteLog()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Owner = Program.frmMdiMain;
        }

        private void frmSiteLog_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'nWDataset.System_Log' table. You can move, or remove it, as needed.
            this.system_LogTableAdapter.Fill(this.nWDataset.System_Log);

        }

        private void system_LogBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.system_LogBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.nWDataset);

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.system_LogTableAdapter.Fill(this.nWDataset.System_Log);
        }
    }
}
