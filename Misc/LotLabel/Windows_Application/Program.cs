using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.IO;

using System.Diagnostics;

namespace LotLabeler
{
    static class Program
    {
        static Mutex mutex = new Mutex(true, "{8F6F0AC4-B9A1-45fd-A8CF-72F04E6BDE8F}");
       
        
        

        // public static SplashScreen.SplashScreen SplashScreen; 
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        [STAThread]
        static void Main(string[] args)
        {

          

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
               
                {


                Application.Run(new LotLabeler.frmMain());
                }


        }
    }
}
