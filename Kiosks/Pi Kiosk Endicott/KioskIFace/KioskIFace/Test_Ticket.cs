using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KioskIFace
{
    public partial class Test_Ticket : Form
    {
        public string Ticket = "";
        public Test_Ticket()
        {
            InitializeComponent();
        }

        private void Test_Ticket_Load(object sender, EventArgs e)
        {

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Ticket = this.textBox1.Text;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel ;
            this.Close();
        }
    }
}
