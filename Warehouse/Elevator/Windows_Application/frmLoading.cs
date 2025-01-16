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
    public partial class frmLoading : Form
    {
        public frmLoading()
        {
            InitializeComponent();
        
        }

        public void SetPrompt(string Message)
        {
            this.label1.Text = Message;
        }

    }
}
