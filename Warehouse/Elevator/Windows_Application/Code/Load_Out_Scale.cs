using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

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
       

        public static void Show(Guid Load_UID)
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
                frm.ShowDialog();
            }
            catch
            {

            }
        }

     
    }
}
