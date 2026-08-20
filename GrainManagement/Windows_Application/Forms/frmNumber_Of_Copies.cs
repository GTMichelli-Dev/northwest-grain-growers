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
    public partial class frmNumber_Of_Copies : Form
    {

        public int NumberOfCopies = 0;
        public frmNumber_Of_Copies(string Prompt="" )
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Owner = Program.frmMdiMain;
            if (!string.IsNullOrEmpty(Prompt))
            {
                this.label1.Text = Prompt;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            NumberOfCopies = 1;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NumberOfCopies = 2;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NumberOfCopies = 3;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            NumberOfCopies = 0;
            this.DialogResult = DialogResult.Cancel ;
            this.Close();
        }
    }
}
