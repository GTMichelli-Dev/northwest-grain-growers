using AppUpdater;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks; // Ensure this using directive is present
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace NWGrain
{
    static class Program
    {
        static Mutex mutex = new Mutex(true, "{8F6F0AC4-B9A1-45fd-A8CF-72F04E6BDE8F}");
        public static frmMain FrmMain;
        public static mdiMain frmMdiMain;
        static bool AllowMultSessions = false;
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) // Changed to async Task
        {
           

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
                    Thread.Sleep(2000); // Synchronous delay
                    Application.DoEvents();
                    frmMdiMain = new NWGrain.mdiMain();
                //    frmMdiMain.WindowState = FormWindowState.Normal; // Ensure the form is maximized
                    Application.Run(frmMdiMain);


                }
                if (!AllowMultSessions) mutex.ReleaseMutex();
            }
            else
            {
                // Try to bring the existing window to the front
                IntPtr hWnd = FindWindow(null, "Your Main Window Title Here");
                if (hWnd != IntPtr.Zero)
                {
                    SetForegroundWindow(hWnd);
                }
                else
                {
                    Alert.Show("Program Already Running");
                }
            }

            // Stop The Scale Connection
            Scales.Cancel = true;
        }
    }
}
