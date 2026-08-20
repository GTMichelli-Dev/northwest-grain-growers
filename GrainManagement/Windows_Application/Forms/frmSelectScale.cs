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
    public partial class frmSelectScale : Form
    {
        public string Scale_Description= string.Empty ;
        public frmSelectScale()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmSelectScale_Load(object sender, EventArgs e)
        {
            this.weigh_Scale_ListTableAdapter.FillBySelect(this.listsDataSet.Weigh_Scale_List);
            this.comboBox1.SelectedIndex = 0;



        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.btnOk.Visible = this.comboBox1.SelectedIndex > 0;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Scale_Description = this.comboBox1.Text;
            this.DialogResult = System.Windows.Forms.DialogResult.OK; 
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
