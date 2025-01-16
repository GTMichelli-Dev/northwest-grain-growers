using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace LotLabeler   
{
    class Alert
    {
        public static DialogResult  Show(string Prompt,string Header="",bool ShowYesNo_Buttons = false )
        {

            using (frmAlert frm = new frmAlert(Prompt, Header,ShowYesNo_Buttons ))
            {
                 return frm.ShowDialog();
            }

        }

    }
}
