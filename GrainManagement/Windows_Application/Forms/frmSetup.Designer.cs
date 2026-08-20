namespace NWGrain
{
    partial class frmSetup
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Label inbound_Kiosk_PrinterLabel;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            this.button1 = new System.Windows.Forms.Button();
            this.btnNewLot = new System.Windows.Forms.Button();
            this.cboGrade_Printer = new System.Windows.Forms.ComboBox();
            this.cboReport_Printer = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblTestPrint = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.ckAllowTareLookup = new System.Windows.Forms.CheckBox();
            this.cboManualScalePrinter = new System.Windows.Forms.ComboBox();
            this.ckAllowMultlipleLocations = new System.Windows.Forms.CheckBox();
            this.lblSiteSetup = new System.Windows.Forms.LinkLabel();
            this.cboDefaultScale = new System.Windows.Forms.ComboBox();
            this.nWDataset = new NWGrain.NWDataset();
            this.bwReportPrintTest = new System.ComponentModel.BackgroundWorker();
            this.bwSamplePrintTest = new System.ComponentModel.BackgroundWorker();
            this.ckRemoteOriginal = new System.Windows.Forms.CheckBox();
            this.ckTruckType = new System.Windows.Forms.CheckBox();
            inbound_Kiosk_PrinterLabel = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nWDataset)).BeginInit();
            this.SuspendLayout();
            // 
            // inbound_Kiosk_PrinterLabel
            // 
            inbound_Kiosk_PrinterLabel.Location = new System.Drawing.Point(67, 449);
            inbound_Kiosk_PrinterLabel.Name = "inbound_Kiosk_PrinterLabel";
            inbound_Kiosk_PrinterLabel.Size = new System.Drawing.Size(208, 29);
            inbound_Kiosk_PrinterLabel.TabIndex = 90;
            inbound_Kiosk_PrinterLabel.Text = "Sample Printer:";
            inbound_Kiosk_PrinterLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            inbound_Kiosk_PrinterLabel.Click += new System.EventHandler(this.inbound_Kiosk_PrinterLabel_Click);
            // 
            // label1
            // 
            label1.Location = new System.Drawing.Point(62, 492);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(213, 29);
            label1.TabIndex = 93;
            label1.Text = "Report Printer:";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            label2.Location = new System.Drawing.Point(67, 356);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(208, 29);
            label2.TabIndex = 96;
            label2.Text = "Default Scale:";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            label3.Location = new System.Drawing.Point(9, 406);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(266, 29);
            label3.TabIndex = 108;
            label3.Text = "Manual Scale Printer:";
            label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Red;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(542, 72);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(228, 90);
            this.button1.TabIndex = 57;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnNewLot
            // 
            this.btnNewLot.BackColor = System.Drawing.Color.SeaGreen;
            this.btnNewLot.ForeColor = System.Drawing.Color.White;
            this.btnNewLot.Location = new System.Drawing.Point(237, 72);
            this.btnNewLot.Name = "btnNewLot";
            this.btnNewLot.Size = new System.Drawing.Size(228, 90);
            this.btnNewLot.TabIndex = 56;
            this.btnNewLot.Text = "Ok";
            this.btnNewLot.UseVisualStyleBackColor = false;
            this.btnNewLot.Click += new System.EventHandler(this.btnNewLot_Click);
            // 
            // cboGrade_Printer
            // 
            this.cboGrade_Printer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGrade_Printer.FormattingEnabled = true;
            this.cboGrade_Printer.Location = new System.Drawing.Point(293, 446);
            this.cboGrade_Printer.Name = "cboGrade_Printer";
            this.cboGrade_Printer.Size = new System.Drawing.Size(535, 37);
            this.cboGrade_Printer.TabIndex = 91;
            // 
            // cboReport_Printer
            // 
            this.cboReport_Printer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboReport_Printer.FormattingEnabled = true;
            this.cboReport_Printer.Location = new System.Drawing.Point(293, 489);
            this.cboReport_Printer.Name = "cboReport_Printer";
            this.cboReport_Printer.Size = new System.Drawing.Size(535, 37);
            this.cboReport_Printer.TabIndex = 94;
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.ckTruckType);
            this.panel1.Controls.Add(this.ckRemoteOriginal);
            this.panel1.Controls.Add(this.lblTestPrint);
            this.panel1.Controls.Add(this.button5);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.ckAllowTareLookup);
            this.panel1.Controls.Add(label3);
            this.panel1.Controls.Add(this.cboManualScalePrinter);
            this.panel1.Controls.Add(this.ckAllowMultlipleLocations);
            this.panel1.Controls.Add(this.lblSiteSetup);
            this.panel1.Controls.Add(label2);
            this.panel1.Controls.Add(this.cboDefaultScale);
            this.panel1.Controls.Add(label1);
            this.panel1.Controls.Add(this.cboReport_Printer);
            this.panel1.Controls.Add(inbound_Kiosk_PrinterLabel);
            this.panel1.Controls.Add(this.cboGrade_Printer);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.btnNewLot);
            this.panel1.Location = new System.Drawing.Point(13, 14);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1007, 647);
            this.panel1.TabIndex = 95;
            // 
            // lblTestPrint
            // 
            this.lblTestPrint.ForeColor = System.Drawing.Color.Blue;
            this.lblTestPrint.Location = new System.Drawing.Point(288, 547);
            this.lblTestPrint.Name = "lblTestPrint";
            this.lblTestPrint.Size = new System.Drawing.Size(540, 46);
            this.lblTestPrint.TabIndex = 115;
            this.lblTestPrint.Text = "lblTestPrint";
            this.lblTestPrint.Visible = false;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(834, 459);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(94, 37);
            this.button5.TabIndex = 114;
            this.button5.Text = "Test";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(834, 415);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(94, 37);
            this.button4.TabIndex = 113;
            this.button4.Text = "Test";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // ckAllowTareLookup
            // 
            this.ckAllowTareLookup.AutoSize = true;
            this.ckAllowTareLookup.Location = new System.Drawing.Point(293, 213);
            this.ckAllowTareLookup.Name = "ckAllowTareLookup";
            this.ckAllowTareLookup.Size = new System.Drawing.Size(247, 33);
            this.ckAllowTareLookup.TabIndex = 110;
            this.ckAllowTareLookup.Text = "Allow Tare Lookup";
            this.ckAllowTareLookup.UseVisualStyleBackColor = true;
            // 
            // cboManualScalePrinter
            // 
            this.cboManualScalePrinter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboManualScalePrinter.FormattingEnabled = true;
            this.cboManualScalePrinter.Location = new System.Drawing.Point(293, 403);
            this.cboManualScalePrinter.Name = "cboManualScalePrinter";
            this.cboManualScalePrinter.Size = new System.Drawing.Size(535, 37);
            this.cboManualScalePrinter.TabIndex = 109;
            // 
            // ckAllowMultlipleLocations
            // 
            this.ckAllowMultlipleLocations.AutoSize = true;
            this.ckAllowMultlipleLocations.Location = new System.Drawing.Point(293, 174);
            this.ckAllowMultlipleLocations.Name = "ckAllowMultlipleLocations";
            this.ckAllowMultlipleLocations.Size = new System.Drawing.Size(369, 33);
            this.ckAllowMultlipleLocations.TabIndex = 106;
            this.ckAllowMultlipleLocations.Text = "Allow Multliple Site Selection";
            this.ckAllowMultlipleLocations.UseVisualStyleBackColor = true;
            // 
            // lblSiteSetup
            // 
            this.lblSiteSetup.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSiteSetup.Font = new System.Drawing.Font("Arial Black", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSiteSetup.Location = new System.Drawing.Point(0, 0);
            this.lblSiteSetup.Name = "lblSiteSetup";
            this.lblSiteSetup.Size = new System.Drawing.Size(1007, 69);
            this.lblSiteSetup.TabIndex = 104;
            this.lblSiteSetup.TabStop = true;
            this.lblSiteSetup.Text = "lblSiteSetup";
            this.lblSiteSetup.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblSiteSetup.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblSiteSetup_LinkClicked);
            // 
            // cboDefaultScale
            // 
            this.cboDefaultScale.DisplayMember = "Scale";
            this.cboDefaultScale.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDefaultScale.FormattingEnabled = true;
            this.cboDefaultScale.Location = new System.Drawing.Point(293, 353);
            this.cboDefaultScale.Name = "cboDefaultScale";
            this.cboDefaultScale.Size = new System.Drawing.Size(535, 37);
            this.cboDefaultScale.TabIndex = 97;
            this.cboDefaultScale.ValueMember = "Scale";
            // 
            // nWDataset
            // 
            this.nWDataset.DataSetName = "NWDataset";
            this.nWDataset.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // bwReportPrintTest
            // 
            this.bwReportPrintTest.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwReportPrintTest_DoWork);
            this.bwReportPrintTest.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwReportPrintTest_RunWorkerCompleted);
            // 
            // bwSamplePrintTest
            // 
            this.bwSamplePrintTest.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwSamplePrintTest_DoWork);
            this.bwSamplePrintTest.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwSamplePrintTest_RunWorkerCompleted);
            // 
            // ckRemoteOriginal
            // 
            this.ckRemoteOriginal.AutoSize = true;
            this.ckRemoteOriginal.Location = new System.Drawing.Point(293, 252);
            this.ckRemoteOriginal.Name = "ckRemoteOriginal";
            this.ckRemoteOriginal.Size = new System.Drawing.Size(342, 33);
            this.ckRemoteOriginal.TabIndex = 116;
            this.ckRemoteOriginal.Text = "Remote Print The Originals";
            this.ckRemoteOriginal.UseVisualStyleBackColor = true;
            // 
            // ckTruckType
            // 
            this.ckTruckType.AutoSize = true;
            this.ckTruckType.Location = new System.Drawing.Point(293, 291);
            this.ckTruckType.Name = "ckTruckType";
            this.ckTruckType.Size = new System.Drawing.Size(253, 33);
            this.ckTruckType.TabIndex = 117;
            this.ckTruckType.Text = "Ask For Truck Type";
            this.ckTruckType.UseVisualStyleBackColor = true;
            // 
            // frmSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(1032, 673);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Name = "frmSetup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Activated += new System.EventHandler(this.frmSetup_Activated);
            this.Load += new System.EventHandler(this.Setup_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nWDataset)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnNewLot;
        private System.Windows.Forms.ComboBox cboGrade_Printer;
        private System.Windows.Forms.ComboBox cboReport_Printer;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cboDefaultScale;
        private System.Windows.Forms.LinkLabel lblSiteSetup;
        private System.Windows.Forms.CheckBox ckAllowMultlipleLocations;
        private NWDataset nWDataset;
        private System.Windows.Forms.ComboBox cboManualScalePrinter;
        private System.Windows.Forms.CheckBox ckAllowTareLookup;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.ComponentModel.BackgroundWorker bwReportPrintTest;
        private System.ComponentModel.BackgroundWorker bwSamplePrintTest;
        private System.Windows.Forms.Label lblTestPrint;
        private System.Windows.Forms.CheckBox ckTruckType;
        private System.Windows.Forms.CheckBox ckRemoteOriginal;
    }
}