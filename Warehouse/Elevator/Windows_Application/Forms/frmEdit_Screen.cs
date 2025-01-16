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
    public partial class frmEdit_Screen : Form
    {

        public string UserInput;
        

        public frmEdit_Screen(string Prompt)
        {
            InitializeComponent();
            this.lblPrompt.Text = Prompt;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel ;
            this.Close();
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            UserInput = this.txtInput.Text;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void txtInput_TextChanged(object sender, EventArgs e)
        {
            this.BtnOk.Visible = this.txtInput.Text.Trim().Length > 0;

        }

        private void frmEdit_Screen_Load(object sender, EventArgs e)
        {

        }

        
    }
}
