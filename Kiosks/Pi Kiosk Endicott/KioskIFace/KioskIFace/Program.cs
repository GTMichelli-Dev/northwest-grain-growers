using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KioskIFace
{
    static class Program
    {
      //  public static string ErrorMsg;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {try
            {


                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                //if (args.Length == 0)
                //{
                //    Application.Run(new frmDelay());
                //    try
                //    {
                //        //run the program again and close this one
                //        System.Diagnostics.Process.Start(Application.StartupPath + "\\KioskIFace.exe","R");
                //        //or you can use Application.ExecutablePath

                //        //close this one
                //        System.Diagnostics.Process.GetCurrentProcess().Kill();
                //    }
                //    catch(Exception ex)
                //    { MessageBox.Show(ex.Message);
                //    }


                //}
                //else
                {
                    Application.Run(new frmMain());
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Error");
            }
        }
    }
}
