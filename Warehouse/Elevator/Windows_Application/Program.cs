using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using AppUpdater;
using System.Diagnostics;

namespace NWGrain
{
    static class Program
    {
        static Mutex mutex = new Mutex(true, "{8F6F0AC4-B9A1-45fd-A8CF-72F04E6BDE8F}");
        public static frmMain FrmMain;
        public static mdiMain frmMdiMain;
        static bool AllowMultSessions=false;
        

        // public static SplashScreen.SplashScreen SplashScreen; 
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        [STAThread]
        static void Main(string[] args)
        {
            //try
            //{

            if (args.Length > 0)
            {
                foreach (string argument in args)
                {

                    if (argument.ToLower() == "noupdate")
                    {

                        UpdateInfo.CheckUpdates = false;
                    }
                    if (argument.ToLower() == "mult")
                    {

                        AllowMultSessions = true;
                    }
                }
            }



            if (mutex.WaitOne(TimeSpan.Zero, true) || AllowMultSessions)
            {
                Settings.OkToStart = true;
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Settings.Check_SiteSetup();
                if (Settings.Location_Id > -1 && Settings.OkToStart)
                {

                    SplashScreen.SplashScreen.ShowSplashScreen();
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(2500);
                    Application.DoEvents();
                    frmMdiMain = new NWGrain.mdiMain();


                    Application.Run(frmMdiMain);
                    //  PLC.Disconnect();
                }
                if (! AllowMultSessions) mutex.ReleaseMutex();
            }
            else
            {
                Alert.Show("Program Already Running");
            }



            //}
            //catch (Exception ex )
            //{
            //    MessageBox.Show("Error Starting Program " + ex.Message);
            //}
            //Stop The Scale Connection;
            Scales.Cancel = true;

        }
    }
}
