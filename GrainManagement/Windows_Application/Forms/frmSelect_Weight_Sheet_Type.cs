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
    public partial class frmSelect_Weight_Sheet_Type : Form
    {
        public enum enumLoad_Type { Transfer, Harvest }
        public enumLoad_Type LoadType;

        public frmSelect_Weight_Sheet_Type(Form owner)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Owner = owner;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadType = enumLoad_Type.Harvest;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadType = enumLoad_Type.Transfer ;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();

        }

        private void frmSelect_Weight_Sheet_Type_Load(object sender, EventArgs e)
        {

        }
    }
}
