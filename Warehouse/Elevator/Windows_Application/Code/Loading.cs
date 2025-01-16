using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace NWGrain
{
    class Loading
    {

        public static frmLoading frm;
        public static void Show(string prompt, System.Windows.Forms.Form Pform , System.Windows.Forms.FormStartPosition StartPosition = System.Windows.Forms.FormStartPosition.CenterParent )
        {
            var ctl = Program.frmMdiMain.Controls.Find("frmLoading", true);
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
            frm.SetPrompt(prompt);
            frm.Show();


        }




        public static void Close()
        {
            try
            {
                var ctl = Program.frmMdiMain.Controls.Find("frmLoading", true);
                if (ctl.Count() > 0)
                {
                    int count = ctl.Count();
                    for (int I=0;  I< count;I++  )
                    {
                        frm = (frmLoading)ctl[I];
                        frm.Close();
                        frm.Dispose();

                    }
                }
            }
            catch
            {

            }
            //frm.Hide();
                //System.Windows.Forms.Application.DoEvents();
        }

    }
}
