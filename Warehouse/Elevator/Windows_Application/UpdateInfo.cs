using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace AppUpdater
{
    class UpdateInfo
    {
        public static string ProgramName = string.Empty;
        public static string UpdatePath = string.Empty;
        private static string Errormsg = "";
        public static bool ProgramCurrent;
        public static bool CheckUpdates=true ;

        public static string ErrorMessage
        {
            get
            {
                return Errormsg;
            }
        }

        public static string PreviousFilePath
        {
            get
            {
                return Path.Combine(PreviousDirectory, ProgramName);
            }
        }

        private static  void SetErrorMessage(string Message)
        {
            if (!string.IsNullOrEmpty(Errormsg)) Errormsg += System.Environment.NewLine;
            Errormsg+= Message;
        }

        public static bool NewerVersionAvailable()
        {
            try
            {
               
                {
                    Errormsg = "";
                    ProgramCurrent = false;
                    bool retval = false;
                    bool OK = true;
                    FileVersionInfo CurversionInfo = FileVersionInfo.GetVersionInfo(UpdateInfo.CurFilePath);
                    FileVersionInfo NewversionInfo = FileVersionInfo.GetVersionInfo(UpdateInfo.NewFilePath);
                    if (NewversionInfo.FileVersion == null)
                    {
                        SetErrorMessage("Newer Version Info Null");

                        OK = false;
                    }
                    if (CurversionInfo.FileVersion == null)
                    {
                        SetErrorMessage("Current Version Info Null");
                        OK = false;
                    }
                    if (OK)
                    {
                        Version CurrentVersionstr = new Version(CurversionInfo.FileVersion);
                        Version NewVersionstr = new Version(NewversionInfo.FileVersion);

                        int result = NewVersionstr.CompareTo(CurrentVersionstr);
                        ProgramCurrent = (result <= 0);
                        retval = result > 0;
                    }
               
                return retval;
                }
            }
            catch(Exception ex)
            {
                SetErrorMessage(ex.ToString() );
                return false;
            }
            
        }


        public static string NewFilePath
        {
            get
            {
                string FilePath = Path.Combine(UpdateInfo.UpdatePath.Trim(), UpdateInfo.ProgramName);
              
                return FilePath;
            }
        }

        public static string CurDir
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }


        public static ProcessStartInfo startInfo
        {
            get
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                string exe= Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AppUpdater.exe");
                startInfo.FileName = string.Format("\"{0}\"", exe); 
                startInfo.Arguments = string.Format("{0} {1}", UpdateInfo.ProgramName, UpdateInfo.UpdatePath);
                return startInfo; 
            }
        }

        public static string CurFilePath
        {
            get
            {
                return Path.Combine(CurDir, UpdateInfo.ProgramName);
            }
        }







        public static string PreviousDirectory
        {
            get
            {
                string CurDir = AppDomain.CurrentDomain.BaseDirectory;
                CurDir += "Previous Vers";
                if (!Directory.Exists(CurDir))
                {
                    try
                    {
                        Directory.CreateDirectory(CurDir);
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show("Error Creating Previous Version Directory" + ex.Message);
                        CurDir = string.Empty;
                    }
                }
                return CurDir;
            }
        }

    }
}
