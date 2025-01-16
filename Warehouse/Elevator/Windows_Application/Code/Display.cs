using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NWGrain
{
    public class Display
    {


        public static void ShowSmallForm(Form frm)
        {
            var ctl = Program.frmMdiMain.Controls.Find(frm.Name , true);
            if (ctl.Count() > 0)
            {
                frm = (frmLoading)ctl[0];
            }
            else
            {
                frm = new frmLoading();
                frm.TopLevel = false;
                Program.frmMdiMain.Controls.Add(frm);
                frm.Parent = Program.frmMdiMain;
            }
            frm.TopMost = true;
            // frm.WindowState = FormWindowState.Maximized;
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.StartPosition = FormStartPosition.Manual;
            frm.Location = new Point((Program.frmMdiMain.ClientSize.Width - frm.Width) / 2,
                                   (Program.frmMdiMain.ClientSize.Height - frm.Height) / 2);
            frm.ShowIcon = false;
            frm.ControlBox = false;
            frm.MaximizeBox = true;
            frm.MinimizeBox = false;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.ShowInTaskbar = false;
          
            frm.Show();

        }



        public static void ShowForm(System.Windows.Forms.Form frm)
        {



            //fFormObj.TopLevel = false;
            //this.Controls.Add(fFormObj);
            //fFormObj.Parent = this;
            //fFormObj.TopMost = true;
            //fFormObj.Show();



            frm.TopLevel = false;
            Program.frmMdiMain.Controls.Add(frm);
            frm.Parent = Program.frmMdiMain;
            frm.TopMost = true;
            //frm.Show();



            //frm.MdiParent = Program.frmMdiMain;
            frm.WindowState = FormWindowState.Maximized;

            frm.ShowIcon = false;
            frm.ControlBox = false;
            frm.MaximizeBox = true;
            frm.MinimizeBox = false;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.ShowInTaskbar = false;
            frm.Text = "";
            frm.Show();
        }
    }
}
