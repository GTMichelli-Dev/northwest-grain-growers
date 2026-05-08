using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace NWGrain
{
   public class Load_Out_Scale
    {
        private static bool InUse;
      
        public static bool IsRunning
        {
            get {
                frmLoad_Out_Scale frm = (frmLoad_Out_Scale)System.Windows.Forms.Application.OpenForms["frmLoad_Out_Scale"];
                InUse = frm != null;
                return InUse; 
                }
                
        }

        public static frmHarvest_Load parentFrm;
       

        public static void ShowFrm(Guid Load_UID)
        {
            if (!InUse)
            {
              
                Thread thread = new Thread(() => GetWeight(Load_UID));
                thread.Start();
                
               
            }

        }


        private static void GetWeight(Guid Load_UID)
        {
            try
            {
                frmLoad_Out_Scale frm = new frmLoad_Out_Scale(Load_UID);

                // Set the form's start position to manual and specify the location
                frm.StartPosition = FormStartPosition.Manual;
                frm.Location = new Point(0, 0); // Upper left corner of the screen

                // Set the size of the form to 100px by 100px
                frm.Size = new Size(100, 60);

                // Show the form as a non-modal dialog so the MDI parent can still function
                if (Application.OpenForms["mdiMain"] is mdiMain mdiInstance)
                {
                    if (mdiInstance.InvokeRequired)
                    {
                        mdiInstance.Invoke(new Action(() => frm.Show()));
                    }
                    else
                    {
                        frm.Show();
                    }
                }
                else
                {
                    MessageBox.Show("mdiMain form is not open. Unable to display frmLoad_Out_Scale.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error displaying frmLoad_Out_Scale: {ex.Message}");
            }
        }


    }
}
