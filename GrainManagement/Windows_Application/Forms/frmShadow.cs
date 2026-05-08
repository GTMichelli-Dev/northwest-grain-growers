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
    public partial class frmShadow : Form
    {
        public frmShadow()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Owner = Program.frmMdiMain;
        }
    }
}
