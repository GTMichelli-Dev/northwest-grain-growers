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
    public partial class frmAudit_Trail : Form
    {
        public frmAudit_Trail()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Owner = Program.frmMdiMain;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void frmAudit_Trail_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'nWDataset.Audit_Trail' table. You can move, or remove it, as needed.
            this.audit_TrailTableAdapter.Fill(this.nWDataset.Audit_Trail);

        }

        private void audit_TrailBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.audit_TrailBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.nWDataset);

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.audit_TrailTableAdapter.Fill(this.nWDataset.Audit_Trail);
        }
    }
}
