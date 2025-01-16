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
    public partial class frmWait : Form
    {
        public frmWait(string Prompt)
        {
            InitializeComponent();
            this.label1.Text = Prompt;
        }

        public void SetPrompt(string Prompt)
        {
            this.label1.Text = Prompt;
        }
    }
}
