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
            this.TransferServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.TransferServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // TransferServiceProcessInstaller
            // 
            this.TransferServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalService;
            this.TransferServiceProcessInstaller.Password = null;
            this.TransferServiceProcessInstaller.Username = null;
            // 
            // TransferServiceInstaller
            // 
            this.TransferServiceInstaller.Description = "Used To Transfer Agvantage Data To DW_Data databse as well as Seed_Data database";
            this.TransferServiceInstaller.ServiceName = "Agvantage Transfer Service";
            this.TransferServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.TransferServiceProcessInstaller,
            this.TransferServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller TransferServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller TransferServiceInstaller;
    }
}