using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NWGrain
{
    public partial class frmRailCar : Form
    {
        public frmRailCar()
        {
            InitializeComponent();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            // Dispose of the form when it is closed
            if (components != null)
            {
                components.Dispose();
            }
            base.OnFormClosed(e);
        }
    }
}
