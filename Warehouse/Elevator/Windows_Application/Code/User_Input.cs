using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NWGrain
{
    class User_Input
    {
        public static string Get_User_Input(string Prompt)
        {
            using (frmEdit_Screen frm = new frmEdit_Screen(Prompt))
            {
                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    return frm.UserInput;
                }
                else
                {
                    return string.Empty;
                }

            }

        }



    }
}
