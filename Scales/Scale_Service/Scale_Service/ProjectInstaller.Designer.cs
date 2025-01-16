namespace Scale_Service
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ScaleServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.ScaleServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // ScaleServiceProcessInstaller
            // 
            this.ScaleServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.ScaleServiceProcessInstaller.Password = null;
            this.ScaleServiceProcessInstaller.Username = null;
            // 
            // ScaleServiceInstaller
            // 
            this.ScaleServiceInstaller.Description = "Connects To The Truck Scales via a Network Connection";
            this.ScaleServiceInstaller.ServiceName = "Scale_Service";
            this.ScaleServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.ScaleServiceProcessInstaller,
            this.ScaleServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller ScaleServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller ScaleServiceInstaller;
    }
}